using Microsoft.Data.SqlClient;

namespace TrainingFPTCo.Models.Queries
{
    public class RoleQuery
    {
        public RoleViewModel GetAllRoles(string? SearchString, string? FilterStatus)
        {
            RoleViewModel rolesViewModel = new RoleViewModel();
            rolesViewModel.RoleDetailList = new List<RoleDetail>();


            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlData = "SELECT * FROM [Role] WHERE  [DeletedAt] IS NULL";
              
                connection.Open();
                SqlCommand command = new SqlCommand(sqlData, connection);
              

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        RoleDetail role = new RoleDetail();
                        role.Id = Convert.ToInt32(reader["Id"]);
                        role.Name = reader["Name"].ToString();
                        role.Description = reader["Description"].ToString();
                        role.Status = reader["Status"].ToString();
                        role.CreatedAt = Convert.ToDateTime(reader["CreatedAt"]);

                        if (reader["UpdatedAt"] != DBNull.Value)
                        {
                            role.UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"]);
                        }

                        if (reader["DeletedAt"] != DBNull.Value)
                        {
                            role.DeletedAt = Convert.ToDateTime(reader["DeletedAt"]);
                        }

                        rolesViewModel.RoleDetailList.Add(role);
                    }
                    connection.Close();
                }
            }
            return rolesViewModel;
        }
    }
}
