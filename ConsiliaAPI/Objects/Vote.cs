using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Convoy.ErrorHandling;
using Npgsql;

namespace ConsiliaAPI.Objects
{
    public class Vote
    {
        public Guid PlaceUUID { get; set; }

        public Guid EventUUID { get; set; }

        public Guid UserUUID { get; set; }

        public Guid VoteUUID { get; set; }

        public int VoteType { get;  set; }

        public static async Task<List<Vote>> GetVotesByPlace(string placeuuid)
        {
            List<Vote> lvote = new List<Vote>();
            try
            {
                // Insert them into database
                NpgsqlConnection conn = Database.DatabaseConnection;
                await using NpgsqlCommand command =
                    new NpgsqlCommand(
                        $"SELECT * FROM VOTES WHERE place_uuid='{placeuuid}'", conn);
                await using  NpgsqlDataReader reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                {
                    Vote u = new Vote();
                    u.VoteType = (int) Convert.ToInt32((Int64) reader["vote_type"]);
                    u.EventUUID =  (Guid) reader["event_uuid"];
                    u.VoteUUID = (Guid) reader["vote_uuid"];
                    u.UserUUID =  (Guid) reader["user_uuid"];
                    lvote.Add(u);
                }

                return lvote;
            }
            catch (ConvoyException e)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ConvoyException("Unable to get vote.", HttpStatusCode.InternalServerError, e.StackTrace);
            }
        }
    }
}
