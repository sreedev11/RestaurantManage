using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RestaurantManage.Models;

namespace RestaurantManage.Repositories
{
    public class CategoryRepository
    {
        private readonly string _connectionString;

        public CategoryRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        // Insert Category
        public int InsertCategory(Category category)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(_connectionString);
                using (var command = new SqlCommand("InsertCategory", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Category_Name", category.CategoryName);
                    command.Parameters.AddWithValue("@logo", category.Logo ?? (object)DBNull.Value);

                    connection.Open();
                    return command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while inserting the category.", ex);
            }
            finally
            {
                if (connection != null && connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
        }

        //// Get All Categories
        public IEnumerable<Category> GetAllCategories()
        {
            var categories = new List<Category>();
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(_connectionString);
                connection.Open();
                var command = new SqlCommand("ViewAllCategories", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categories.Add(new Category
                        {
                            Id = (int)reader["Id"],
                            Logo = reader["Logo"] as byte[],
                            CategoryName = reader["Category_Name"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving categories.", ex);
            }
            finally
            {
                if (connection != null && connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            return categories;
        }
        //Edit Category
        public int UpdateCategory(Category category)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(_connectionString);
                var query = "UpdateCategory";

                using (var command = new SqlCommand(query, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Id", category.Id);
                    command.Parameters.AddWithValue("@Category_Name", category.CategoryName);
                    command.Parameters.AddWithValue("@logo", category.Logo ?? (object)DBNull.Value);

                    connection.Open();
                    return command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the category.", ex);
            }
            finally
            {
                if (connection != null && connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
        }
        //Get single Category for Edit
        public Category GetCategoryById(int id)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(_connectionString);
                var query = "ViewCategory";

                using (var command = new SqlCommand(query, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Category
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                CategoryName = reader.GetString(reader.GetOrdinal("Category_Name")),
                                Logo = reader.IsDBNull(reader.GetOrdinal("logo")) ? null : reader["logo"] as byte[]
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the category.", ex);
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
        public int DeleteCategory(int id)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(_connectionString);
                using (var command = new SqlCommand("DeleteCategory", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();
                    return command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the category.", ex);
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
