using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using RestaurantManage.Models;

namespace RestaurantManage.Repositories
{
    public class RegistrationRepository
    {
        private readonly string _connectionString;
        public RegistrationRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public void AddUser(Registration registration)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(_connectionString);
                var command = new SqlCommand("AddUser", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@FirstName", registration.FirstName);
                command.Parameters.AddWithValue("@LastName", registration.LastName);
                command.Parameters.AddWithValue("@DateOfBirth", registration.DateOfBirth);
                command.Parameters.AddWithValue("@Gender", registration.Gender);
                command.Parameters.AddWithValue("@PhoneNumber", registration.PhoneNumber);
                command.Parameters.AddWithValue("@EmailAddress", registration.EmailAddress);
                command.Parameters.AddWithValue("@Address", registration.Address);
                command.Parameters.AddWithValue("@UserType", registration.UserType);
                command.Parameters.AddWithValue("@Username", registration.Username);
                command.Parameters.AddWithValue("@Password", registration.Password);

                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the user.", ex);
            }
            finally
            {
                if (connection != null && connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
        }
        public List<Registration> GetAllUsers()
        {
            var users = new List<Registration>();
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(_connectionString);
                var command = new SqlCommand("GetAllUsers", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new Registration
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                            Gender = reader["Gender"].ToString(),
                            PhoneNumber = reader["PhoneNumber"].ToString(),
                            EmailAddress = reader["EmailAddress"].ToString(),
                            Address = reader["Address"].ToString(),
                            UserType = reader["UserType"].ToString(),
                            Username = reader["Username"].ToString(),
                            Password = reader["Password"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving all users.", ex);
            }
            finally
            {
                if (connection != null && connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            return users;
        }
        public Registration GetUserByID(int id)
        {
            Registration user = null;
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(_connectionString);
                var command = new SqlCommand("GetUserByID", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@ID", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new Registration
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                            Gender = reader["Gender"].ToString(),
                            PhoneNumber = reader["PhoneNumber"].ToString(),
                            EmailAddress = reader["EmailAddress"].ToString(),
                            Address = reader["Address"].ToString(),
                            UserType = reader["UserType"].ToString(),
                            Username = reader["Username"].ToString(),
                            Password = reader["Password"].ToString()
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the user by ID.", ex);
            }
            finally
            {
                if (connection != null && connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            return user;
        }
        public void UpdateUser(Registration registration)
        {
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(_connectionString);
                var command = new SqlCommand("UpdateUser", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@ID", registration.ID);
                command.Parameters.AddWithValue("@FirstName", registration.FirstName);
                command.Parameters.AddWithValue("@LastName", registration.LastName);
                command.Parameters.AddWithValue("@DateOfBirth", registration.DateOfBirth);
                command.Parameters.AddWithValue("@Gender", registration.Gender);
                command.Parameters.AddWithValue("@PhoneNumber", registration.PhoneNumber);
                command.Parameters.AddWithValue("@EmailAddress", registration.EmailAddress);
                command.Parameters.AddWithValue("@Address", registration.Address);
                command.Parameters.AddWithValue("@UserType", registration.UserType);
                command.Parameters.AddWithValue("@Username", registration.Username);
                command.Parameters.AddWithValue("@Password", registration.Password);

                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the user.", ex);
            }
            finally
            {
                if (connection != null && connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
        }
        public void DeleteUser(int id)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(_connectionString);
                var command = new SqlCommand("DeleteUser", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@ID", id);
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the user.", ex);
            }
            finally
            {
                if (connection != null && connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
        }
        public List<Registration> GetRegisteredUsers()
        {
            var users = new List<Registration>();
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(_connectionString);
                connection.Open();

                using (var command = new SqlCommand("GetUsers", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var user = new Registration
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                                Gender = reader["Gender"].ToString(),
                                PhoneNumber = reader["PhoneNumber"].ToString(),
                                EmailAddress = reader["EmailAddress"].ToString(),
                                Address = reader["Address"].ToString(),
                                UserType = reader["UserType"].ToString(),
                                Username = reader["Username"].ToString(),
                                Password = reader["Password"].ToString()
                            };
                            users.Add(user);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving registered users.", ex);
            }
            finally
            {
                if (connection != null && connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            return users;
        }
        // Method to get admins
        public List<Registration> GetRegisteredAdmins()
        {
            var admins = new List<Registration>();
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(_connectionString);
                connection.Open();

                using (var command = new SqlCommand("GetAdmins", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var admin = new Registration
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                                Gender = reader["Gender"].ToString(),
                                PhoneNumber = reader["PhoneNumber"].ToString(),
                                EmailAddress = reader["EmailAddress"].ToString(),
                                Address = reader["Address"].ToString(),
                                UserType = reader["UserType"].ToString(),
                                Username = reader["Username"].ToString(),
                                Password = reader["Password"].ToString()
                            };
                            admins.Add(admin);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving registered admins.", ex);
            }
            finally
            {
                if (connection != null && connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }

            return admins;
        }
    }
}
