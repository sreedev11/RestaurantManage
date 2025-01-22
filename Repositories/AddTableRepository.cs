using System.Data;
using Microsoft.Data.SqlClient;
using RestaurantManage.Models;

namespace RestaurantManage.Repositories
{
    public class AddTableRepository
    {
        private readonly string _connectionString;
        public AddTableRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public void InsertAddTable(string tableName, string tableType, int amount)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("InsertIntoAddTable", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add parameters for the stored procedure
                        cmd.Parameters.AddWithValue("@TableName", tableName);
                        cmd.Parameters.AddWithValue("@TableType", tableType);
                        cmd.Parameters.AddWithValue("@Amount", amount);

                        cmd.ExecuteNonQuery();

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        public void DeleteAddTable(int tableId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("DeleteFromAddTable", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TableId", tableId);
                        cmd.ExecuteNonQuery(); 
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw; 
                }
                finally
                {
                    connection.Close(); 
                }
            }
        }
        public List<AddTable> GetAllAddTables()
        {
            var tables = new List<AddTable>();
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(_connectionString);
                connection.Open();

                using (var command = new SqlCommand("GetAllAddTables", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tables.Add(new AddTable
                            {
                                TableId = (int)reader["TableId"],
                                TableName = reader["TableName"].ToString(),
                                TableType = reader["TableType"].ToString(),
                                Amount = (int)reader["Amount"]
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving table data: " + ex.Message);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                    connection.Close();
            }
            return tables;
        }
        public AddTable GetAddTableById(int tableId)
        {
            AddTable table = null;
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(_connectionString);
                connection.Open();

                using (var command = new SqlCommand("GetAddTableById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TableId", tableId);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            table = new AddTable
                            {
                                TableId = (int)reader["TableId"],
                                TableName = reader["TableName"].ToString(),
                                TableType = reader["TableType"].ToString(),
                                Amount = (int)reader["Amount"]
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving table data by ID: " + ex.Message);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                    connection.Close();
            }

            return table;
        }
        public void EditAddTable(AddTable table)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(_connectionString);
                connection.Open();

                using (var command = new SqlCommand("EditAddTable", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TableId", table.TableId);
                    command.Parameters.AddWithValue("@TableName", table.TableName);
                    command.Parameters.AddWithValue("@TableType", table.TableType);
                    command.Parameters.AddWithValue("@Amount", table.Amount);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating table: " + ex.Message);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }
        public List<AddTable> GetTablesByTypeNONAC()
        {
            var tables = new List<AddTable>();
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(_connectionString);
                connection.Open();

                using (var command = new SqlCommand("GetAddTablesByTypeNONAC", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tables.Add(new AddTable
                            {
                                TableId = (int)reader["TableId"],
                                TableName = reader["TableName"].ToString(),
                                TableType = reader["TableType"].ToString(),
                                Amount = (int)reader["Amount"]
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving NON-AC table data: " + ex.Message);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                    connection.Close();
            }
            return tables;
        }
        public List<AddTable> GetTablesByTypeAC()
        {
            var tables = new List<AddTable>();
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(_connectionString);
                connection.Open();

                using (var command = new SqlCommand("GetAddTablesByTypeAC", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tables.Add(new AddTable
                            {
                                TableId = (int)reader["TableId"],
                                TableName = reader["TableName"].ToString(),
                                TableType = reader["TableType"].ToString(),
                                Amount = (int)reader["Amount"]
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving AC table data: " + ex.Message);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                    connection.Close();
            }

            return tables;
        }
    }
}