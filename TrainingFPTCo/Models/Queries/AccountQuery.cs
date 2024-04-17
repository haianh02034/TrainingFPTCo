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
                string sqlData = "SELECT [co].*, [ro].[Name] AS [RoleName] FROM [User] AS [co] LEFT JOIN [Role] AS [ro] ON [co].[RoleId] = [ro].[Id] WHERE [co].[DeletedAt] IS NULL";

                // Thêm điều kiện tìm kiếm nếu SearchString được cung cấp
                if (!string.IsNullOrEmpty(SearchString))
                {
                    sqlData += " AND [co].[UserName] LIKE @search";
                }

                // Thêm điều kiện lọc theo trạng thái nếu FilterStatus được cung cấp
                if (!string.IsNullOrEmpty(FilterStatus))
                {
                    sqlData += " AND [co].[Status] = @status";
                }
                connection.Open();
                SqlCommand command = new SqlCommand(sqlData, connection);
                command.Parameters.AddWithValue("@search", "%" + SearchString + "%" ?? DBNull.Value.ToString());
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

        //add
        public int InsertItemAccount(int RoleId,
                string UserName,
                string Password,
                string ExtraCode,
                string Email,
                string Phone,
                string Address,
                string FullName,
                DateTime? Birthday,
                string Gender,
                string Status)
        {
            int lastInsertId = 0;
            string sqlInsertAccount = "INSERT INTO [User]([RoleId],[UserName],[Password],[ExtraCode],[Email],[Phone],[Address],[FullName],[Birthday],[Gender],[Status],[CreatedAt]) " +
                               "VALUES (@roleId, @userName, @password, @extraCode, @email, @phone, @address, @fullName, @birthday, @gender, @status, @createdAt); " +
                               "SELECT SCOPE_IDENTITY()";
            ;

            //SELECT SCOPE_IDENTITY()  lay ra id vua dc them
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlInsertAccount, connection);
                command.Parameters.AddWithValue("@roleId", RoleId);
                command.Parameters.AddWithValue("@userName", UserName ?? DBNull.Value.ToString());
                command.Parameters.AddWithValue("@password", Password ?? DBNull.Value.ToString());
                command.Parameters.AddWithValue("@extraCode", ExtraCode ?? DBNull.Value.ToString());
                command.Parameters.AddWithValue("@email", Email ?? DBNull.Value.ToString());
                command.Parameters.AddWithValue("@phone", Phone ?? DBNull.Value.ToString());
                command.Parameters.AddWithValue("@address", Address ?? DBNull.Value.ToString());
                command.Parameters.AddWithValue("@fullName", FullName ?? DBNull.Value.ToString());
                command.Parameters.AddWithValue("@birthday", Birthday);
                command.Parameters.AddWithValue("@gender", Gender ?? DBNull.Value.ToString());
                command.Parameters.AddWithValue("@status", Status ?? DBNull.Value.ToString());
                command.Parameters.AddWithValue("@createdAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                lastInsertId = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();

            }
            return lastInsertId;
        }

        public bool DeleteAccountById(int id)
        {
            bool checkDelete = false;
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlQuery = "UPDATE [User] SET [DeletedAt] = @DeletedAt WHERE [Id] = @id";
                connection.Open();
                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@DeletedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                checkDelete = true;
                connection.Close();
            }
            return checkDelete;
        }

        public AccountDetail GetAccountById(int id = 0)
        {
            AccountDetail accountDetail = new AccountDetail();
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlQuery = "SELECT * FROM [User] WHERE [Id] = @id AND [DeletedAt] IS NULL";
                connection.Open();
                SqlCommand command = new SqlCommand(@sqlQuery, connection);
                command.Parameters.AddWithValue("id", id);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        accountDetail.Id = Convert.ToInt32(reader["id"]);
                        if (reader["RoleId"] != DBNull.Value)
                        {
                            accountDetail.RoleId = Convert.ToInt32(reader["RoleId"]);
                        }
                        else
                        {
                            accountDetail.RoleId = 0;
                        }
                        accountDetail.UserName = reader["UserName"].ToString();
                        accountDetail.Password = reader["Password"].ToString();
                        accountDetail.ExtraCode = reader["ExtraCode"].ToString();
                        accountDetail.Email = reader["Email"].ToString();
                        accountDetail.Phone = reader["Phone"].ToString();
                        accountDetail.Address = reader["Address"].ToString();
                        accountDetail.FullName = reader["FullName"].ToString();
                        accountDetail.Birthday = Convert.ToDateTime(reader["Birthday"]);
                        accountDetail.Gender = reader["Gender"].ToString();
                        accountDetail.Status = reader["Status"].ToString();

                    }
                    connection.Close();
                }
            }
            return accountDetail;
        }

        public bool UpdateAccountById(
    int roleId,
    string userName,
    string? password,
    string? extraCode,
    string? email,
    string? phone,
    string? address,
    string? fullName,
    DateTime? birthday,
    string gender,
    string status,
    int id
)
        {
            bool checkUpdate = false;
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sql = "UPDATE [User] SET [RoleId] = @RoleId, [UserName] = @UserName, [ExtraCode] = @ExtraCode, [Email] = @Email, [Phone] = @Phone, [Address] = @Address, [FullName] = @FullName, [Birthday] = @Birthday, [Gender] = @Gender, [Status] = @Status, [UpdatedAt] = @UpdatedAt WHERE [Id] = @Id AND [DeletedAt] IS NULL";
                connection.Open();
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@RoleId", roleId);
                cmd.Parameters.AddWithValue("@UserName", userName);
                cmd.Parameters.AddWithValue("@ExtraCode", extraCode ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@Email", email ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@Phone", phone ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@Address", address ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@FullName", fullName ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@Birthday", birthday);
                cmd.Parameters.AddWithValue("@Gender", gender);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
                checkUpdate = true;
                connection.Close();
            }
            return checkUpdate;
        }


    }
}
