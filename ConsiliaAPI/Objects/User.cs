using System;
using System.Net;
using System.Threading.Tasks;
using Convoy.ErrorHandling;
using Newtonsoft.Json;
using Npgsql;

namespace ConsiliaAPI.Objects
{
    public class User
    {
        /// <summary>
        /// Secure Sign On Key. Random nonce generated by user to be used like a password.
        /// </summary>
        public string SSOKey { private get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePic { get; set; }
        public Guid UserUUID { get; set; }

        public static async Task<User> CreateUser(User us)
        {
            try
            {
                User u = new User();
                // Generate User's details
                u.SSOKey = us.SSOKey;
                u.FirstName = us.FirstName;
                u.LastName = us.LastName;
                u.UserUUID = Guid.NewGuid();

                // Insert them into database
                NpgsqlConnection conn = Database.DatabaseConnection;
                await using NpgsqlCommand command =
                    new NpgsqlCommand(
                        $"INSERT INTO USERS(user_uuid, first_name, last_name, sso_key) " +
                        $"VALUES(\'{u.UserUUID}\', \'{u.FirstName}\', \'{u.LastName}\', \'{u.SSOKey}\')", conn);
                await command.ExecuteNonQueryAsync();

                return u;
            }
            catch (Exception e)
            {
                throw new ConvoyException("Unable to create user.", HttpStatusCode.InternalServerError, e.StackTrace);
            }
        }
        
        public async Task<User> UpdateUser(User u)
        {
            try
            {
                FirstName = u.FirstName ?? FirstName;
                LastName = u.LastName ?? LastName;

                // Insert them into database
                NpgsqlConnection conn = Database.DatabaseConnection;
                
                await using NpgsqlCommand command = new NpgsqlCommand($"UPDATE USERS SET first_name='{FirstName}', last_name='{LastName}' WHERE user_uuid='{UserUUID}'", conn);
                await command.ExecuteNonQueryAsync();

                return u;
            }
            catch (Exception e)
            {
                throw new ConvoyException("Unable to update user.", HttpStatusCode.InternalServerError, e.StackTrace);
            }
        }
        
        public async Task UpdateProfilePicture(string s)
        {
            try
            {
                // Insert them into database
                NpgsqlConnection conn = Database.DatabaseConnection;
                
                await using NpgsqlCommand command = new NpgsqlCommand($"UPDATE USERS SET profile_picture='{s}' WHERE user_uuid='{UserUUID}'", conn);
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception e)
            {
                throw new ConvoyException("Unable to update profile picture.", HttpStatusCode.InternalServerError, e.StackTrace);
            }
        }
        
        public static async Task<User> GetUser(string uuid)
        {
            try
            {
                // Insert them into database
                NpgsqlConnection conn = Database.DatabaseConnection;
                await using NpgsqlCommand command =
                    new NpgsqlCommand(
                        $"SELECT * FROM USERS WHERE user_uuid='{uuid}'", conn);
                await using  NpgsqlDataReader reader = await command.ExecuteReaderAsync();

                if (reader.Read())
                {
                    User u = new User();
                    u.FirstName =  (string) reader["first_name"];
                    u.SSOKey =  (string) reader["sso_key"];
                    u.LastName = (string) reader["last_name"];
                    u.ProfilePic =  reader["profile_picture"].ToString();
                    u.UserUUID =  (Guid) reader["user_uuid"];
                    return u;
                }
                else
                {
                    throw new ConvoyException("User does not exist", HttpStatusCode.NotFound);
                }
            }
            catch (ConvoyException e)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ConvoyException("Unable to get user.", HttpStatusCode.InternalServerError, e.StackTrace);
            }
        }

        public async Task ValidateSSOKey(string ssoKey)
        {
            if (SSOKey != ssoKey)
            {
                throw new ConvoyException("Invalid SSO Key", HttpStatusCode.Unauthorized);
            }
        }
    }
}