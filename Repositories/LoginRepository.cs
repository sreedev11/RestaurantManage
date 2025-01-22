using System.Data;
using Microsoft.Data.SqlClient;
using RestaurantManage.Models;

namespace RestaurantManage.Repositories
{
    public class LoginRepository
    {
        private readonly string _connectionString;
        public LoginRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public Registration ValidateUser(string username, string password)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var command = new SqlCommand("LoginUser", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Registration
                            {
                                ID = (int)reader["ID"],
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                DateOfBirth = (DateTime)reader["DateOfBirth"],
                                Gender = reader["Gender"].ToString(),
                                PhoneNumber = reader["PhoneNumber"].ToString(),
                                EmailAddress = reader["EmailAddress"].ToString(),
                                Address = reader["Address"].ToString(),
                                UserType = reader["UserType"].ToString(),
                                Username = reader["Username"].ToString(),
                                Password = reader["Password"].ToString(),
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {               
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw; 
            }
            return null;
        }
    }
}
