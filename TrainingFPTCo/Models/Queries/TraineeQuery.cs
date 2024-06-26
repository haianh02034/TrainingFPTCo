﻿using Microsoft.Data.SqlClient;

namespace TrainingFPTCo.Models.Queries
{
    public class TraineeQuery
    {
        public List<TopicDetail> GetAllTopics(string? SearchString, string? FilterStatus)
        {
            List<TopicDetail> topic = new List<TopicDetail>();
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlData = string.Empty;
                if (FilterStatus != null)
                {
                    sqlData = "SELECT* FROM [User] WHERE [RoleId] = 3 AND [Name] LIKE @keyWord AND [DeletedAt] IS NULL AND [Status] =@status";
                }
                else
                {
                    sqlData = "SELECT* FROM [User]  WHERE [RoleId] = 3 AND [Name] LIKE @keyWord AND [DeletedAt] IS NULL";
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
    }
}
