using Microsoft.Data.SqlClient;

namespace TrainingFPTCo.Models.Queries
{
    public class LoginQuery
    {
        // kiem tra login 
        public LoginViewModel CheckUserLogin(string? username, string? password)
        {
            LoginViewModel dataUser = new LoginViewModel();
            using(SqlConnection conn = Database.GetSqlConnection())
            {
                string querySql = "SELECT * FROM [User] WHERE UserName = @username AND Password = @password";
                SqlCommand cmd = new SqlCommand(querySql,conn);
                cmd.Parameters.AddWithValue("@username", username ?? (object) DBNull.Value);
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

    }
}
