using System;
using System.Net;
using System.Threading.Tasks;
using Convoy.ErrorHandling;
using Npgsql;

namespace ConsiliaAPI.Objects
{
    public class Event
    {
        public Guid EventID { get; private set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public double LocationLat { get; set; }
        public double LocationLong { get; set; }
        public double Range { get; set; }
        public string Type { get; set; }

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

                return e;
            }
            catch (Exception ex)
            {
                throw new ConvoyException("Unable to create event.", HttpStatusCode.InternalServerError, ex.StackTrace);
            }
        }

        public async Task<> Castvote(User user, Vote vote, Places place)
        {
            try
            {
                // Insert them into database
                NpgsqlConnection conn = Database.DatabaseConnection;
                NpgsqlCommand command =
                    new NpgsqlCommand(
                        $"INSERT VOTE(event_uuid, , user_uuid, place_uuid, vote_uuid, vote_type) " +
                        $"VALUES\'{this.EventID}\', \'{user.uuid}\', \'{place.placeID}\', \'{vote.VoteID}\', \'{vote.Type}\')", conn);
                await command.ExecuteNonQueryAsync();

                return u;
            }
            catch (Exception e)
            {
                throw new ConvoyException("Unable to add vote.", HttpStatusCode.InternalServerError, e.StackTrace);
            }
        }

        public async Task<Event> get(string uuid)
        {
            try
            {
                // Insert them into database
                NpgsqlConnection conn = Database.DatabaseConnection;
                NpgsqlCommand command =
                    new NpgsqlCommand(
                        $"SELECT * FROM EVENTS WHERE event_uuid='{uuid}'", conn);
                NpgsqlDataReader reader = await command.ExecuteReaderAsync();

                if (reader.Read())
                {
                    Event e = new Event();
                    e.EventID = (string)reader["event_uuid"];
                    e.Name = (string)reader["event_name"];
                    e.StartDate = (string)reader["start_date_time"];
                    e.LocationLat = (string)reader["latitude"];
                    e.LocationLong = (string)reader["longitude"];
                    e.Range = (string)reader["range"];
                    e.Type = (string)reader["type"];
                    /**
                     * reference
                    e.LastName = (string)reader["last_name"];
                    e.ProfilePic = reader["profile_picture"].ToString();
                    e.UserUUID = (Guid)reader["user_uuid"];
                    **/
                    return e;
                }
                else
                {
                    throw new ConvoyException("Event does not exist", HttpStatusCode.NotFound);
                }
            }
            catch (ConvoyException e)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ConvoyException("Unable to get event.", HttpStatusCode.InternalServerError, e.StackTrace);
            }
        }
    }


}