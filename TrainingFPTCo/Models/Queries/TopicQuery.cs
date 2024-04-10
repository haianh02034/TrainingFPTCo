using Microsoft.Data.SqlClient;

namespace TrainingFPTCo.Models.Queries
{
    public class TopicQuery
    {
        public List<TopicDetail> GetAllTopics(string? SearchString, string? FilterStatus)
        {
            List<TopicDetail> topic = new List<TopicDetail>();
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlData = string.Empty;
                if (FilterStatus != null)
                {
                    sqlData = "SELECT* FROM [Topics] WHERE [Name] LIKE @keyWord AND [DeletedAt] IS NULL AND [Status] =@status";
                }
                else
                {
                    sqlData = "SELECT* FROM [Topics] WHERE [Name] LIKE @keyWord AND [DeletedAt] IS NULL";
                }
                connection.Open();
                SqlCommand command = new SqlCommand(sqlData, connection);
                command.Parameters.AddWithValue("@keyWord", "%" + SearchString + "%" ?? DBNull.Value.ToString());
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



    }
}
