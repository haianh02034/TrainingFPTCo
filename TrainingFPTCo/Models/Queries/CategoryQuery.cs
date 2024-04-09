using Microsoft.Data.SqlClient;

namespace TrainingFPTCo.Models.Queries
{
    public class CategoryQuery
    {
        public int InsertItemCategory(string nameCategory,
                string description,
                string image,
                string status)
        {
            int lastInsertId=0;
            string sqlInsertCategory = "INSERT INTO [Categories]([Name],[Description],[PosterImage],[ParentId],[Status],[CreatedAt]) " +
                                       "VALUES (@nameCategory, @description, @image, @parentId, @status, @createdAt); " +
                                       "SELECT SCOPE_IDENTITY()";

            //SELECT SCOPE_IDENTITY()  lay ra id vua dc them
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlInsertCategory, connection);
                command.Parameters.AddWithValue("@nameCategory", nameCategory ?? DBNull.Value.ToString());
                command.Parameters.AddWithValue("@description", description ?? DBNull.Value.ToString());
                command.Parameters.AddWithValue("@image", image ?? DBNull.Value.ToString());
                command.Parameters.AddWithValue("@status", status ?? DBNull.Value.ToString());
                command.Parameters.AddWithValue("@parentId", 0);
                command.Parameters.AddWithValue("@createdAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                lastInsertId = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
                
            }
            return lastInsertId;
        }
        public List<CategoryDetail> GetAllCategories( string? SearchString, string? FilterStatus)
        {
            List<CategoryDetail> category = new List<CategoryDetail>();

            using(SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlData =string.Empty;
                if(FilterStatus != null)
                {
                    sqlData = "SELECT* FROM [Categories] WHERE [Name] LIKE @keyWord AND [DeletedAt] IS NULL AND [Status] =@status";
                }
                else {                
                    sqlData = "SELECT* FROM [Categories] WHERE [Name] LIKE @keyWord AND [DeletedAt] IS NULL";
 }
                connection.Open();
                SqlCommand command = new SqlCommand( sqlData, connection);
                command.Parameters.AddWithValue("@keyWord", "%"+SearchString+"%" ?? DBNull.Value.ToString());
                if (FilterStatus!=null)
                {
                    command.Parameters.AddWithValue("@status",FilterStatus ??  DBNull.Value.ToString());
                }
                    using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CategoryDetail cate = new CategoryDetail();
                        cate.Id = Convert.ToInt32(reader["Id"]);
                        cate.Name = reader["Name"].ToString();
                        cate.Description = reader["Description"].ToString();
                        cate.Status = reader["Status"].ToString();
                        cate.PosterNameImage = reader["PosterImage"].ToString();
                        cate.CreatedAt = Convert.ToDateTime(reader["CreatedAt"]);
                        if (reader["UpdatedAt"] != DBNull.Value)
                        {
                            cate.UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"]);
                        }                        //cate.DeletedAt = Convert.ToDateTime(reader["DeletedAt"]);
                        category.Add(cate);
                    }
                    connection.Close();
                }
            }
            return category;
        }
        public bool DeleteItemCategoryById( int id =0)
        {
            bool statusDelete =false;
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlQuery = "UPDATE [Categories] SET [DeletedAt] =@deletedAt WHERE [Id] = @id";
                SqlCommand command = new SqlCommand(@sqlQuery, connection);
                connection.Open();
                command.Parameters.AddWithValue("id", id);  
                command.Parameters.AddWithValue("@deletedAt",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                command.ExecuteNonQuery();
                connection.Close();
                statusDelete = true;

            }


            return (statusDelete);
        }
        public CategoryDetail GetCategoryById(int id =0)
        {
            CategoryDetail categoryDetail = new CategoryDetail();
            using ( SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlQuery = "SELECT * FROM [Categories] WHERE [Id] = @id AND [DeletedAt] IS NULL";
                connection.Open();
                SqlCommand command = new SqlCommand(@sqlQuery, connection);
                command.Parameters.AddWithValue("id", id);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categoryDetail.Id = Convert.ToInt32(reader["id"]);
                        categoryDetail.Name = reader["Name"].ToString();
                        categoryDetail.Description = reader["Description"].ToString();
                        categoryDetail.PosterNameImage= reader["PosterImage"].ToString();
                        categoryDetail.Status = reader["Status"].ToString();

                    }
                    connection.Close();
                }
            }
            return categoryDetail;
        }
        public bool UpdateCategoryById(int id,string nameCategory,
                string description,
                string image,
                string status)
        {
            bool statusUpdate = false;
            string sqlUpdateCategory = "UPDATE [Categories] SET [Name] = @nameCategory, [Description] = @description, [PosterImage] = @image, [Status] = @status, [UpdatedAt] = @updatedAt WHERE [Id] = @id AND [DeletedAt] IS NULL";

            using (SqlConnection connection = Database.GetSqlConnection())
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlUpdateCategory, connection);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@nameCategory", nameCategory ?? DBNull.Value.ToString());
                command.Parameters.AddWithValue("@description", description ?? DBNull.Value.ToString());
                command.Parameters.AddWithValue("@image", image ?? DBNull.Value.ToString());
                command.Parameters.AddWithValue("@status", status ?? DBNull.Value.ToString());
                command.Parameters.AddWithValue("@updatedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                command.ExecuteNonQuery();
                connection.Close();
                statusUpdate = true;
            }
            return statusUpdate;
        }
        
        
    }
}
