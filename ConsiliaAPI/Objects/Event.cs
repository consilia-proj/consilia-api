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

        public static async Task CastVote(bool vote, Guid user)
        {

        }

    }


}