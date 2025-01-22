using System.Data;
using System.Linq.Expressions;
using Microsoft.Data.SqlClient;
using RestaurantManage.Models;

namespace RestaurantManage.Repositories
{
    public class DishRepository
    {
        private readonly string _connectionString;
        public DishRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public int InsertDish(Dish dish)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(_connectionString);
                var command = new SqlCommand("InsertDish", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@DishName", dish.DishName);
                command.Parameters.AddWithValue("@Price", dish.Price);
                command.Parameters.AddWithValue("@CategoryId", dish.CategoryId);

                connection.Open();
                return command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
            finally
            {
                if (connection != null && connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
        }
        public List<Dish> GetDishesByCategoryId(int categoryId)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(_connectionString);
                var command = new SqlCommand("SelectDishByCategory", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@CategoryId", categoryId);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    var dishes = new List<Dish>();
                    while (reader.Read())
                    {
                        dishes.Add(new Dish
                        {
                            Id = (int)reader["Id"],
                            DishName = reader["DishName"].ToString(),
                            Price = (int)reader["Price"],
                            CategoryId = (int)reader["CategoryId"]
                        });
                    }
                    return dishes;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
            finally
            {
                if (connection != null && connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
        }
        //Get Dish By Id
        public Dish GetDishById(int id)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(_connectionString);
                var command = new SqlCommand("SelectDishById", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Dish
                        {
                            Id = (int)reader["Id"],
                            DishName = reader["DishName"].ToString(),
                            Price = (int)reader["Price"],
                            CategoryId = (int)reader["CategoryId"]
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
            finally
            {
                if (connection != null && connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            return null;
        }
        //Edit Dish Details
        public void EditDishDetails(Dish dish)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(_connectionString);
                var command = new SqlCommand("EditDishDetails", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@Id", dish.Id);
                command.Parameters.AddWithValue("@DishName", dish.DishName);
                command.Parameters.AddWithValue("@Price", dish.Price);
                command.Parameters.AddWithValue("@CategoryId", dish.CategoryId);

                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating dish details", ex);
            }
            finally
            {
                if (connection != null && connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
        }
        public void DeleteDish(int id)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(_connectionString);
                var command = new SqlCommand("DeleteDishDetails", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("Id", id);

                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while Deleting dish Details", ex);
            }
            finally
            {
                if (connection != null && connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
        }
        //delete All Dishes for particular category Id
        public void DeleteAllDishByCategory(int id)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(_connectionString);
                var command = new SqlCommand("DeleteAllDishesByCategory", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("CategoryId", id);

                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while Deleting All dish Details with particular Category", ex);
            }
            finally
            {
                if (connection != null && connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
        }
    }
}
