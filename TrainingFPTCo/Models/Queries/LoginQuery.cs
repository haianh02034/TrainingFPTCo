using Microsoft.Data.SqlClient;

namespace TrainingFPTCo.Models.Queries
{
    public class LoginQuery
    {
        // kiem tra login 
        public LoginViewModel CheckUserLogin(string? username, string? password)
        {
            LoginViewModel dataUser = new LoginViewModel();
            using (SqlConnection conn = Database.GetSqlConnection())
            {
                string querySql = "SELECT * FROM [User] WHERE UserName = @username AND Password = @password";
                SqlCommand cmd = new SqlCommand(querySql, conn);
                cmd.Parameters.AddWithValue("@username", username ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@password", password ?? (object)DBNull.Value);
                // open connection database

                conn.Open();
                // thuc thi cau lenh sql
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //do du lieu table sang users sang model loginviewmodel
                        dataUser.id = reader["id"].ToString();
                        dataUser.UserName = reader["UserName"].ToString();
                        dataUser.ExtraCode = reader["ExtraCode"].ToString();
                        dataUser.Email = reader["Email"].ToString();
                        dataUser.Address = reader["Address"].ToString();
                        dataUser.RoleId = reader["RoleId"].ToString();
                        dataUser.FullName = reader["FullName"].ToString();
                        dataUser.Phone = reader["Phone"].ToString();

                    }
                    conn.Close();
                }
            }
            return dataUser;
        }

        public int RegisterAccount(
                string UserName,
                string Password,
                string Email,
                string Phone,
                string Address,
                string FullName,
                DateTime? Birthday,
                string Gender)
        {
            int lastInsertId = 0;
            string sqlInsertAccount = "INSERT INTO [User]([UserName],[Password],[Email],[Phone],[Address],[FullName],[Birthday],[Gender],[CreatedAt]) " +
                               "VALUES ( @userName, @password, @email, @phone, @address, @fullName, @birthday, @gender, @createdAt); " +
                               "SELECT SCOPE_IDENTITY()";
            ;

            //SELECT SCOPE_IDENTITY()  lay ra id vua dc them
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlInsertAccount, connection);
                command.Parameters.AddWithValue("@userName", UserName ?? DBNull.Value.ToString());
                command.Parameters.AddWithValue("@password", Password ?? DBNull.Value.ToString());
                command.Parameters.AddWithValue("@email", Email ?? DBNull.Value.ToString());
                command.Parameters.AddWithValue("@phone", Phone ?? DBNull.Value.ToString());
                command.Parameters.AddWithValue("@address", Address ?? DBNull.Value.ToString());
                command.Parameters.AddWithValue("@fullName", FullName ?? DBNull.Value.ToString());
                command.Parameters.AddWithValue("@birthday", Birthday);
                command.Parameters.AddWithValue("@gender", Gender ?? DBNull.Value.ToString());
                command.Parameters.AddWithValue("@createdAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                lastInsertId = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();

            }
            return lastInsertId;
        }
        public void UpdateLastLogin(string userId)
        {
            // Tạo kết nối với cơ sở dữ liệu và cập nhật thời gian Last Login
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlUpdate = "UPDATE [User] SET LastLogin = @lastLogin WHERE Id = @userId";
                SqlCommand command = new SqlCommand(sqlUpdate, connection);
                command.Parameters.AddWithValue("@lastLogin", DateTime.Now);
                command.Parameters.AddWithValue("@userId", userId);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        public void UpdateLastLogout(string userId)
        {
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlUpdate = "UPDATE [User] SET LastLogout = GETDATE() WHERE Id = @userId";
                SqlCommand command = new SqlCommand(sqlUpdate, connection);
                command.Parameters.AddWithValue("@userId", userId);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

    }
}