using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RestaurantManage.Models;

namespace RestaurantManage.Repositories
{
    public class ReservationRepository
    {
        private readonly string _connectionString;

        public ReservationRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public void AddReservation(Reservation reservation)
        {
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(_connectionString);
                using (var command = new SqlCommand("InsertReservation", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@ReserveDate", reservation.ReserveDate);
                    command.Parameters.AddWithValue("@ReserveTime", reservation.ReserveTime);
                    command.Parameters.AddWithValue("@Number_of_Persons", reservation.Number_of_Persons);
                    command.Parameters.AddWithValue("@Table_Name", reservation.Table);
                    command.Parameters.AddWithValue("@TableType", reservation.TableType);
                    command.Parameters.AddWithValue("@Amount", reservation.Amount);
                    command.Parameters.AddWithValue("@ID", reservation.ID);
                    command.Parameters.AddWithValue("@Status", reservation.Status);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while inserting reservation", ex);
            }
            finally
            {
                if (connection != null && connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
        }
        //Check  if the table already Exits
        public bool GetReservationsByCriteria(Reservation reservation)
        {
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(_connectionString);
                using (var command = new SqlCommand("GetReservationsByCriteria", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@ReserveDate", reservation.ReserveDate);
                    command.Parameters.AddWithValue("@ReserveTime", reservation.ReserveTime);
                    command.Parameters.AddWithValue("@Table_Name", reservation.Table);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        return reader.HasRows;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while checking reservation existence", ex);
            }
            finally
            {
                if (connection != null && connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
        }
        // Get all pending reservations
        public IEnumerable<PendingReservationViewModel> GetPendingReservations()
        {
            var reservations = new List<PendingReservationViewModel>();
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(_connectionString);
                using (var command = new SqlCommand("GetPendingReservations", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var reservation = new PendingReservationViewModel
                            {
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                PhoneNumber = reader["PhoneNumber"].ToString(),
                                EmailAddress = reader["EmailAddress"].ToString(),
                                ReserveDate = Convert.ToDateTime(reader["ReserveDate"]),
                                ReserveTime = reader["ReserveTime"].ToString(),
                                Number_of_Persons = Convert.ToInt32(reader["Number_of_Persons"]),
                                Table_Name = reader["Table_Name"].ToString(),
                                TableType = reader["TableType"].ToString(),
                                Amount = Convert.ToInt32(reader["Amount"]),
                                CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                                ReserveId = Convert.ToInt32(reader["ReserveId"])
                            };
                            reservations.Add(reservation);
                        }
                    }
                }
                return reservations;
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching pending reservations", ex);
            }
            finally
            {
                if (connection != null && connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
        }
        public void ApproveReservation(int reserveId)
        {
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(_connectionString);

                using (var command = new SqlCommand("ApproveOrRejectReservation", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ReserveId", reserveId);
                    command.Parameters.AddWithValue("@Status", "Approved");

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error approving reservation with ID {reserveId}", ex);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
        // Method to reject a reservation
        public void RejectReservation(int reserveId)
        {
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(_connectionString);

                using (var command = new SqlCommand("ApproveOrRejectReservation", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ReserveId", reserveId);
                    command.Parameters.AddWithValue("@Status", "Sorry, Unavailable");

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error rejecting reservation with ID {reserveId}", ex);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
        public IEnumerable<UserReservationStatusViewModel> GetUserReservationStatus(int userId)
        {
            var reservations = new List<UserReservationStatusViewModel>();
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(_connectionString);

                using (var command = new SqlCommand("UserReservationStatus", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", userId);
                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var reservation = new UserReservationStatusViewModel
                            {
                                ReserveDate = Convert.ToDateTime(reader["ReserveDate"]),
                                ReserveTime = reader["ReserveTime"].ToString(),
                                Number_of_Persons = Convert.ToInt32(reader["Number_of_Persons"]),
                                Table_Name = reader["Table_Name"].ToString(),
                                TableType = reader["TableType"].ToString(),
                                Amount = Convert.ToInt32(reader["Amount"]),
                                Status = reader["Status"].ToString(),
                                ReserveId = Convert.ToInt32(reader["ReserveId"])
                            };
                            reservations.Add(reservation);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching user reservation status.", ex);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return reservations;
        }

        public void CancelReservation(int reserveId)
        {
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(_connectionString);

                using (var command = new SqlCommand("UserCancelReservation", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ReserveId", reserveId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error cancelling reservation.", ex);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
        public IEnumerable<ReservationStatusHistory> AdminViewReservationStatusHistory()
        {
            var reservations = new List<ReservationStatusHistory>();
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(_connectionString);

                using (var command = new SqlCommand("GetReservationsByStatus", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var reservation = new ReservationStatusHistory
                            {
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                PhoneNumber = reader["PhoneNumber"].ToString(),
                                EmailAddress = reader["EmailAddress"].ToString(),
                                ReserveDate = Convert.ToDateTime(reader["ReserveDate"]),
                                ReserveTime = reader["ReserveTime"].ToString(),
                                Number_of_Persons = Convert.ToInt32(reader["Number_of_Persons"]),
                                Table_Name = reader["Table_Name"].ToString(),
                                TableType = reader["TableType"].ToString(),
                                Amount = Convert.ToInt32(reader["Amount"]),
                                CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                                Status = reader["Status"].ToString()
                            };
                            reservations.Add(reservation);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching reservation status history", ex);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return reservations;
        }
    }
}
