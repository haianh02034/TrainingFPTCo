using Microsoft.Data.SqlClient;
using TrainingFPTCo.DataDBContext;

namespace TrainingFPTCo.Models.Queries
{
    public class AccountQuery
    {
        public List<AccountDetail> GetAllAccounts(string? SearchString, string? FilterStatus)
        {
            List<AccountDetail> account = new List<AccountDetail>();
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlData = string.Empty;
                if (FilterStatus != null)
                {
                    sqlData = "SELECT* FROM [User] WHERE [UserName] LIKE @keyWord AND [DeletedAt] IS NULL AND [Status] =@status";
                }
                else
                {
                    sqlData = "SELECT [co].*, [ro].[Name] AS [RoleName] FROM [User] AS [co] INNER JOIN [Role] AS [ro] ON [co].[RoleId] = [ro].[Id] WHERE [co].[DeletedAt] IS NULL";
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
                        AccountDetail acc = new AccountDetail();
                        acc.Id = Convert.ToInt32(reader["Id"]);
                        acc.ViewRoleName = reader["RoleName"].ToString();
                        acc.UserName = reader["UserName"].ToString();
                        acc.ExtraCode = reader["ExtraCode"].ToString();
                        acc.Email = reader["Email"].ToString();
                        acc.Phone = reader["Phone"].ToString();
                        acc.Address = reader["Address"].ToString();
                        acc.FullName = reader["FullName"].ToString();
                        acc.FirstName = reader["FirstName"].ToString();
                        acc.LastName = reader["LastName"].ToString();
                        acc.Birthday = Convert.ToDateTime(reader["Birthday"]);
                        acc.Gender = reader["Gender"].ToString();
                        acc.Education = reader["Education"].ToString();
                        acc.ProgramingLanguage = reader["ProgramingLanguage"].ToString();
                        acc.ToeicScore = reader["ToeicScore"].ToString();
                        acc.IPClient = reader["IPClient"].ToString();
                        acc.LastLogin = reader["LastLogin"] != DBNull.Value ? Convert.ToDateTime(reader["LastLogin"]) : DateTime.MinValue;
                        acc.LastLogout = reader["LastLogout"] != DBNull.Value ? Convert.ToDateTime(reader["LastLogout"]) : DateTime.MinValue;
                        acc.CreatedAt = Convert.ToDateTime(reader["CreatedAt"]);
                        if (reader["UpdatedAt"] != DBNull.Value)
                        {
                            acc.UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"]);
                        }                        //acc.DeletedAt = Convert.ToDateTime(reader["DeletedAt"]);
                        account.Add(acc);
                    }
                    connection.Close();
                }
            }
            return account;
        }
    }
}
