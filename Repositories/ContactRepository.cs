using Microsoft.Data.SqlClient;
using RestaurantManage.Models;
using System.Collections.Generic;
using System.Data;

namespace RestaurantManage.Repositories
{
    public class ContactRepository
    {
        private readonly string _connectionString;

        public ContactRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public void AddContact(Contact contact)
        {
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(_connectionString);
                SqlCommand cmd = new SqlCommand("InsertContact", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", contact.Name);
                cmd.Parameters.AddWithValue("@Email", contact.Email);
                cmd.Parameters.AddWithValue("@PhoneNumber", contact.PhoneNumber);
                cmd.Parameters.AddWithValue("@Message", contact.Message);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while adding contact: {ex.Message}");
                throw;
            }
            finally
            {
                if (conn != null && conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
            }
        }
        public IEnumerable<Contact> GetAllContacts()
        {
            var contacts = new List<Contact>();
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(_connectionString);
                SqlCommand cmd = new SqlCommand("GetAllContacts", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        contacts.Add(new Contact
                        {
                            Id = (int)reader["Id"],
                            Name = reader["Name"].ToString(),
                            Email = reader["Email"].ToString(),
                            PhoneNumber = reader["PhoneNumber"].ToString(),
                            Message = reader["Message"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving contacts: {ex.Message}");
                throw;
            }
            finally
            {
                if (conn != null && conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
            }
            return contacts;
        }
        public void DeleteContact(int id)
        {
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(_connectionString);
                SqlCommand cmd = new SqlCommand("DeleteContact", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting contact: {ex.Message}");
                throw;
            }
            finally
            {
                if (conn != null && conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
            }
        }
    }
}
