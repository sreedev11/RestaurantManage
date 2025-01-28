using System;
using System.Data;
using Microsoft.Data.SqlClient;
using RestaurantManage.Models;

namespace RestaurantManage.Repositories
{
    public class CancelledTableRepository
    {
        private readonly string _connectionString;

        public CancelledTableRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void InsertCancelledRecord(int id, int amount)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            try
            {
                SqlCommand command = new SqlCommand("sp_InsertCancelledRecord", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@ID", id);
                command.Parameters.AddWithValue("@Amount", amount);

                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while inserting the record.", ex);
            }
            finally
            {
                connection.Close();
            }
        }
        public CancelledTable GetCancelledAmountByReserveId(int reserveId)
        {
            CancelledTable cancelledRecord = null;
            SqlConnection connection = new SqlConnection(_connectionString);

            try
            {
                SqlCommand command = new SqlCommand("sp_CancelledAmount", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@ReserveId", reserveId);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        cancelledRecord = new CancelledTable
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            Amount = Convert.ToInt32(reader["Amount"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the record.", ex);
            }
            finally
            {
                connection.Close();
            }

            return cancelledRecord;
        }
        public List<CancelledTable> GetCancelledRecordsById(int id)
        {
            List<CancelledTable> cancelledRecords = new List<CancelledTable>();
            SqlConnection connection = new SqlConnection(_connectionString);

            try
            {
                SqlCommand command = new SqlCommand("sp_GetCancelledRecordsById", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@ID", id);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CancelledTable cancelled = new CancelledTable
                        {
                            Amount = Convert.ToInt32(reader["Amount"]),
                            CreatedDate = Convert.ToDateTime(reader["CreatedDate"])
                        };

                        cancelledRecords.Add(cancelled);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the records.", ex);
            }
            finally
            {
                connection.Close();
            }

            return cancelledRecords;
        }
    }
}