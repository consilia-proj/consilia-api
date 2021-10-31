using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Convoy.ErrorHandling;
using GooglePlacesApi.Models;
using Newtonsoft.Json.Linq;
using Npgsql;

namespace ConsiliaAPI.Objects
{
    public class Event
    {
        public Guid EventID { get; private set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public decimal LocationLat { get; set; }
        public decimal LocationLong { get; set; }
        public decimal Range { get; set; }
        public string Type { get; set; }

        public static async Task<Event> GetEvent(string uuid)
        {
            try
            {
                // Insert them into database
                NpgsqlConnection conn = Database.DatabaseConnection;
                await using NpgsqlCommand command =
                    new NpgsqlCommand(
                        $"SELECT * FROM EVENTS WHERE event_uuid='{uuid}'", conn);
                await using  NpgsqlDataReader reader = await command.ExecuteReaderAsync();

                if (reader.Read())
                {
                    Event u = new Event();
                    u.EventID =  (Guid) reader["event_uuid"];
                    
                    u.Name =  (string) reader["event_name"];
                    u.StartDate = (DateTime) reader["start_date_time"];
                    u.LocationLat =  (decimal) reader["latitude"];
                    u.LocationLong =  (decimal) reader["longitude"];
                    u.Range =  (decimal) reader["range"];
                    u.Type =  (string) reader["type"];
                    return u;
                }
                else
                {
                    throw new ConvoyException("Event does not exist", HttpStatusCode.NotFound);
                }
            }
            catch (ConvoyException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ConvoyException("Unable to get event.", HttpStatusCode.InternalServerError, ex.StackTrace);
            }
        }

        public async Task<List<Places>> GetPlaces()
        {
            List<Places> placesList = new List<Places>();
            try
            {
               
                // Generate User's details

                // Insert them into database
                NpgsqlConnection conn = Database.DatabaseConnection;
                await using NpgsqlCommand command =
                    new NpgsqlCommand(
                        $"SELECT * FROM PLACES WHERE event_uuid='{EventID}'", conn);
                await using NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    Places p = new Places()
                    {
                        PlaceID = (Guid) reader["place_uuid"],
                        EventID = (Guid) reader["event_uuid"],
                        GooglePlaceId = (string) reader["google_place_id"],
                    };
                   
                    placesList.Add(p);
                }
                
                
            }
            catch (Exception ex)
            {
                throw new ConvoyException("Unable to get places.", HttpStatusCode.InternalServerError, ex.StackTrace);
            }

            foreach (var p in placesList)
            {
                await p.Prepare();
            }

            return placesList;
        }

        public static async Task<Event> CreateEvent(Event e)
        {
            try
            {
                // Generate User's details
                e.EventID = Guid.NewGuid();

                // Insert them into database
                NpgsqlConnection conn = Database.DatabaseConnection;
                await using NpgsqlCommand command =
                    new NpgsqlCommand(
                        $"INSERT INTO EVENTS (event_uuid, event_name, start_date_time, latitude, longitude, range, type) " +
                        $"VALUES(\'{e.EventID}\', \'{e.Name}\', \'{e.StartDate}\', {e.LocationLat}, {e.LocationLong}, {e.Range}, \'{e.Type}\')", conn);
                await command.ExecuteNonQueryAsync();
                await e.CompileEvents();
                return e;
            }
            catch (Exception ex)
            {
                throw new ConvoyException("Unable to create event.", HttpStatusCode.InternalServerError, ex.StackTrace);
            }
        }

        private static string FileGetContents(string fileName)
        {
            string sContents = string.Empty;
            string me = string.Empty;
            try
            {
                if (fileName.ToLower().IndexOf("https:") > -1)
                {
                    System.Net.WebClient wc = new System.Net.WebClient();
                    byte[] response = wc.DownloadData(fileName);
                    sContents = System.Text.Encoding.ASCII.GetString(response);

                }
                else
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader(fileName);
                    sContents = sr.ReadToEnd();
                    sr.Close();
                }
            }
            catch { sContents = "unable to connect to server "; }
            return sContents;
        }
        
        public async Task CompileEvents()
        {
            using (var client = new HttpClient())
            {
                var response = FileGetContents((
                                   $"https://maps.googleapis.com/maps/api/place/textsearch/json?location={LocationLat},{LocationLong}&radius={Range}&query={Type}&key={GlobalConstants.GOOGLE_MAPS_API_KEY}"));
                JObject jobj = JObject.Parse(response);
                List<JToken> jarr = jobj["results"].ToList();
                NpgsqlConnection conn = Database.DatabaseConnection;
               
                for (int i = 0; i < Math.Min(jarr.Count, 20); i++)
                {
                    await using NpgsqlCommand command =
                        new NpgsqlCommand(
                            $"INSERT INTO PLACES (place_uuid, event_uuid, google_place_id) " +
                            $"VALUES('{Guid.NewGuid()}','{EventID}','{jarr[i]["place_id"].ToString()}')", conn);
                    await command.ExecuteNonQueryAsync();
                }
            }
            
            // data processing here
            
        }
        
        public async Task CastVote(User user, Vote vote, string placeuuid)
        {
            vote.VoteUUID = Guid.NewGuid();
            vote.UserUUID = user.UserUUID;
            vote.EventUUID = EventID;
            vote.EventUUID = Guid.Parse(placeuuid);
            try
            {
                // Insert them into database
                NpgsqlConnection conn = Database.DatabaseConnection;
                NpgsqlCommand command =
                    new NpgsqlCommand(
                        $"INSERT INTO VOTES(event_uuid, user_uuid, place_uuid, vote_uuid, vote_type) " +
                        $"VALUES(\'{EventID}\', \'{user.UserUUID}\', \'{placeuuid}\', \'{vote.VoteUUID}\', \'{vote.VoteType}\')", conn);
                await command.ExecuteNonQueryAsync();
            }
            catch (NpgsqlException e)
            {
                if(!e.Message.Contains("duplicate key"))
                    throw new ConvoyException("Unable to cast vote.", HttpStatusCode.InternalServerError, e.StackTrace);
            }
            catch (Exception e)
            {
                throw new ConvoyException("Unable to cast vote.", HttpStatusCode.InternalServerError, e.StackTrace);
            }
        }
    }


}