using Microsoft.Data.SqlClient;
using TrainingFPTCo.DataDBContext;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TrainingFPTCo.Models.Queries

{
    public class CourseQuery
    {

        public bool UpdateCourseById(
           int categoryId,
           string nameCourse,
           string? description,
           string image,
           DateTime startDate,
           DateTime? endDate,
           string status,
           int id
       )
        {
            string valEndate = DBNull.Value.ToString();
            if (endDate != null)
            {
                valEndate = endDate.Value.ToString();
            }
            bool checkUpdate = false;
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sql = "UPDATE [Courses] SET [CategoryId] = @CategoryId, [Name] = @Name, [Description] = @Description, [Image] = @Image, [StartDate] = @StartDate, [EndDate] = @EndDate, [Status] = @Status, [UpdatedAt] = @UpdatedAt WHERE [Id] = @Id AND [DeletedAt] IS NULL";
                connection.Open();
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                cmd.Parameters.AddWithValue("@Name", nameCourse);
                cmd.Parameters.AddWithValue("@Description", description ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@Image", image);
                cmd.Parameters.AddWithValue("@StartDate", startDate);
                cmd.Parameters.AddWithValue("@EndDate", valEndate);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("UpdatedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
                checkUpdate = true;
                connection.Close();
            }
            return checkUpdate;
        }

        public bool DeleteCourseById(int id)
        {
            bool checkDelete = false;
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlQuery = "UPDATE [Courses] SET [DeletedAt] = @DeletedAt WHERE [Id] =@id";
                connection.Open();
                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@DeletedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
                checkDelete = true;
                connection.Close();
            }
            return checkDelete;
        }
        public int InsertItemCourse(
            string nameCourse,
            int categoryId,
            string? description,
            DateTime startDate, DateTime? endDate,
                string imageCourse,
                string status
            )
        {
            string valEndate = DBNull.Value.ToString();
            if (endDate != null)
            {
                valEndate = endDate.Value.ToString();
            }
            int IdCourse = 0;
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlQuery = "INSERT INTO [Courses] ([CategoryId],[Name],[Description],[Image],[Status],[StartDate],[EndDate],[CreatedAt]) VALUES(@CategoryId,@Name,@Description,@Image,@Status,@StartDate,@EndDate,@CreatedAt) SELECT SCOPE_IDENTITY() ";
                SqlCommand cmd = new SqlCommand( sqlQuery, connection );
                connection.Open();
                cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                cmd.Parameters.AddWithValue("@Name", nameCourse);
                cmd.Parameters.AddWithValue("@Description", description ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@Image", imageCourse);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@StartDate", startDate);
                cmd.Parameters.AddWithValue("@EndDate", valEndate);
                cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                IdCourse = Convert.ToInt32(cmd.ExecuteScalar());
                connection.Close();
            }
            return IdCourse;
        }
        public CourseDetail GetDetailCourseById( int id )
        {
            CourseDetail detail = new CourseDetail();
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sql = "SELECT * FROM [Courses] WHERE [Id]=@id AND [DeletedAt] IS NULL";
                connection.Open();
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@id", id);
                using(SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        detail.Id = Convert.ToInt32(reader["ID"]);
                        detail.Name = reader["Name"].ToString();
                        detail.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                        detail.Description = reader["Description"].ToString();
                        detail.StartDate = Convert.ToDateTime(reader["StartDate"]);
                        detail.EndDate = Convert.ToDateTime(reader["EndDate"]);
                        detail.Status = reader["Status"].ToString();
                        detail.NameImage = reader["Image"].ToString();


                    }
                }
                connection.Close();
            }
            
            return detail;
        }
        public List<CourseDetail> GetAllDataCourses(string? SearchString, string? FilterStatus, string? SearchCategoryName)
        {
            List<CourseDetail> courses = new List<CourseDetail>();
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sql = "SELECT [co].*, [ca].[Name] AS [CategoryName] FROM [Courses] AS [co] INNER JOIN [Categories] AS [ca] ON [co].[CategoryId] = [ca].[Id] WHERE [co].[DeletedAt] IS NULL";
                if (!string.IsNullOrEmpty(SearchString))
                {
                    sql += " AND [co].[Name] LIKE @search";
                }

                // Thêm điều kiện lọc theo trạng thái nếu FilterStatus được cung cấp
                if (!string.IsNullOrEmpty(FilterStatus))
                {
                    sql += " AND [co].[Status] = @status";
                }
                if (!string.IsNullOrEmpty(SearchCategoryName))
                {
                    sql += " AND [co].[CategoryId] IN (SELECT [Id] FROM [Categories] WHERE [Name] LIKE @search1)";
                }
                connection.Open();
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@search", "%" + SearchString + "%" ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@search1", "%" + SearchCategoryName + "%" ?? DBNull.Value.ToString());

                if (FilterStatus != null)
                {
                    cmd.Parameters.AddWithValue("@status", FilterStatus ?? DBNull.Value.ToString());
                }
               
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CourseDetail detail = new CourseDetail();
                        detail.Id = Convert.ToInt32(reader["Id"]);
                        detail.Name = reader["Name"].ToString() ?? DBNull.Value.ToString();
                        detail.Description = reader["Description"].ToString();
                        detail.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                        detail.StartDate = Convert.ToDateTime(reader["StartDate"]);
                        detail.EndDate = Convert.ToDateTime(reader["EndDate"]);
                        detail.NameImage = reader["Image"].ToString();
                        detail.ViewCategoryName = reader["CategoryName"].ToString();
                        detail.Status = reader["Status"].ToString() ?? DBNull.Value.ToString();
                        courses.Add(detail);
                    }
                    connection.Close();
                }
            }
            return courses;
        }


    }
}
