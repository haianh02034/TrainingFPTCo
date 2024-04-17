using Microsoft.Data.SqlClient;

namespace TrainingFPTCo.Models.Queries
{
    public class TopicQuery
    {
        public List<TopicDetail> GetAllTopics(string? SearchString, string? FilterStatus ,string? SearchCourseName)
        {
            List<TopicDetail> topic = new List<TopicDetail>();
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sql = "SELECT [top].*, [co].[Name] AS [NameCourse] FROM [Topics] AS [top] INNER JOIN [Courses] AS [co] ON [top].[CourseId] = [co].[Id] WHERE [top].[DeletedAt] IS NULL";
                if (!string.IsNullOrEmpty(SearchString))
                {
                    sql += " AND [top].[Name] LIKE @search";
                }

                // Thêm điều kiện lọc theo trạng thái nếu FilterStatus được cung cấp
                if (!string.IsNullOrEmpty(FilterStatus))
                {
                    sql += " AND [top].[Status] = @status";
                }
                if (!string.IsNullOrEmpty(SearchCourseName))
                {
                    // Thay đổi điều kiện tìm kiếm để tìm kiếm NameCourse từ bảng Courses nhưng vẫn lọc theo CourseId trong bảng Topics
                    sql += " AND [top].[CourseId] IN (SELECT [Id] FROM [Courses] WHERE [Name] LIKE @search1)";
                }

                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@search", "%" + SearchString + "%" ?? DBNull.Value.ToString());
                command.Parameters.AddWithValue("@search1", "%" + SearchCourseName + "%" ?? DBNull.Value.ToString());
                if (FilterStatus != null)
                {
                    command.Parameters.AddWithValue("@status", FilterStatus ?? DBNull.Value.ToString());
                }
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TopicDetail top = new TopicDetail();
                        top.Id = Convert.ToInt32(reader["Id"]);
                        top.CourseId = Convert.ToInt32(reader["CourseId"]);
                        top.CourseName = reader["NameCourse"].ToString();
                        top.Name = reader["Name"].ToString();
                        top.Description = reader["Description"].ToString();
                        top.Status = reader["Status"].ToString();
                        top.NameDocuments = reader["Documents"].ToString();
                        top.NameAttachFile = reader["AttachFile"].ToString();
                        top.NamePoterTopic = reader["PoterTopic"].ToString();
                        top.TypeDocument = reader["TypeDocument"].ToString();


                        top.CreatedAt = Convert.ToDateTime(reader["CreatedAt"]);
                        if (reader["UpdatedAt"] != DBNull.Value)
                        {
                            top.UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"]);
                        }                        //topic.DeletedAt = Convert.ToDateTime(reader["DeletedAt"]);
                        topic.Add(top);
                    }
                    connection.Close();
                }
            }
            return topic;
        }

        public int InsertItemTopic(
                int courseId,
                string nameTopic,
                string description,
                string status,
                string? documents,
                string? attachFile,
                string? poterTopic,
                string typeDocument
            )
        {
            int idTopic = 0;
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlQuery = "INSERT INTO [Topics] ([CourseId],[Name],[Description],[Status],[Documents],[AttachFile],[PoterTopic],[TypeDocument],[CreatedAt]) VALUES(@CourseId,@Name,@Description,@Status,@Documents,@AttachFile,@PoterTopic,@TypeDocument,@CreatedAt) SELECT SCOPE_IDENTITY() ";
                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                connection.Open();
                cmd.Parameters.AddWithValue("@CourseId", courseId);
                cmd.Parameters.AddWithValue("@Name", nameTopic);
                cmd.Parameters.AddWithValue("@Description", description);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@Documents", documents);
                cmd.Parameters.AddWithValue("@AttachFile", attachFile);
                cmd.Parameters.AddWithValue("@PoterTopic", poterTopic);
                cmd.Parameters.AddWithValue("@TypeDocument", typeDocument);
                cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                idTopic = Convert.ToInt32(cmd.ExecuteScalar());
                connection.Close();
            }
            return idTopic;
        }

        public bool UpdateTopicById(
                int courseId,
                string name,
                string description,
                string documents,
                string attachFile,
                string poterTopic,
                string typeDocument,
                string status,
                int id
            )
        {
            bool checkUpdate = false;
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sql = "UPDATE [Topics] SET [CourseId] = @CourseId, [Name] = @Name, [Description] = @Description, [Documents] = @Documents, [AttachFile] = @AttachFile, [PoterTopic] = @PoterTopic, [TypeDocument] = @TypeDocument, [Status] = @Status, [UpdatedAt] = @UpdatedAt WHERE [Id] = @Id AND [DeletedAt] IS NULL";
                connection.Open();
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@CourseId", courseId);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Description", description);
                cmd.Parameters.AddWithValue("@Documents", documents);
                cmd.Parameters.AddWithValue("@AttachFile", attachFile);
                cmd.Parameters.AddWithValue("@PoterTopic", poterTopic);
                cmd.Parameters.AddWithValue("@TypeDocument", typeDocument);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
                checkUpdate = true;
                connection.Close();
            }
            return checkUpdate;
        }

        public TopicDetail GetDetailTopicById(int id)
        {
            TopicDetail detail = new TopicDetail();
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sql = "SELECT * FROM [Topics] WHERE [Id]=@id AND [DeletedAt] IS NULL";
                connection.Open();
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@id", id);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        detail.Id = Convert.ToInt32(reader["ID"]);
                        detail.CourseId = Convert.ToInt32(reader["CourseId"]);
                        detail.Name = reader["Name"].ToString();
                        detail.Description = reader["Description"].ToString();
                        detail.Status = reader["Status"].ToString();
                        detail.NameDocuments = reader["Documents"].ToString();
                        detail.NameAttachFile = reader["AttachFile"].ToString();
                        detail.NamePoterTopic = reader["PoterTopic"].ToString();
                        detail.TypeDocument = reader["TypeDocument"].ToString();
                    }
                }
                connection.Close();
            }

            return detail;
        }

        public bool DeleteItemTopicById(int id = 0)
        {
            bool statusDelete = false;
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlQuery = "UPDATE [Topics] SET [DeletedAt] =@deletedAt WHERE [Id] = @id";
                SqlCommand command = new SqlCommand(@sqlQuery, connection);
                connection.Open();
                command.Parameters.AddWithValue("id", id);
                command.Parameters.AddWithValue("@deletedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                command.ExecuteNonQuery();
                connection.Close();
                statusDelete = true;

            }


            return (statusDelete);
        }

    }
}