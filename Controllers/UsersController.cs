using AounCarSystem.Models;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient; 

namespace AounCarSystem.Controllers
{
    public class UsersController : Controller
    {

        private readonly IConfiguration _config;

        public UsersController(IConfiguration config)
        {
                _config = config;
        }


        [HttpGet("/Users/Get_AllUsers")]
        public IActionResult Get_AllUsers()
        {
            var Users = new List<Object>(); 

            string connStr = _config.GetConnectionString("MySqlConn");

            using(var conn = new MySqlConnection(connStr))
            {
                string sql = @"SELECT U.user_Id, U.username, U.password, 
                                   P.PerGroup AS Permissions
                              FROM users U 
                              LEFT JOIN userPermissions P ON u.per_Id = P.per_Id";
                               

                using (var cmd = new MySqlCommand(sql,conn))
                {
                    conn.Open();
                    var reader = cmd.ExecuteReader();

                    while (reader.Read()) 
                    {
                        Users.Add(new
                        {
                            user_Id = reader["user_Id"],
                            username = reader["username"],
                            password = reader["password"],
                            permissions = reader["permissions"]
                        }); 
                    }
                    return Json(Users);

                } 
            }

           
        }


        [HttpGet("/Users/Get_UserById")]
        public IActionResult Get_UserById(int userId) {

          
            var User = new List<Object>();

            string connStr = _config.GetConnectionString("MySqlConn");

            using (var conn = new MySqlConnection(connStr))
            {
                string sql = @"SELECT U.user_Id, U.username, U.password, 
                                   P.PerGroup AS Permissions
                              FROM users U 
                              LEFT JOIN userPermissions P ON u.per_Id = P.per_Id
                              WHERE U.user_Id = @Id";


                using (var cmd = new MySqlCommand(sql,conn))
                {
                    cmd.Parameters.AddWithValue("@Id", userId);
                    conn.Open();
                    var reader = cmd.ExecuteReader(); 

                    while(reader.Read())
                    {
                        User.Add(new
                        {
                            userId = reader["user_Id"],
                            username = reader["username"],
                            password = reader["password"],
                            permissions = reader["Permissions"]
                        }); 
                    }
                    return Json(User);
                }

            }



        }


        [HttpPost("Users/AddUser")]
        public IActionResult AddUser(int UserId, string UserName, string Password, int Per_Id)
        {
            string connStr = _config.GetConnectionString("MySqlConn");

            using (var conn = new MySqlConnection(connStr))
            {
                string sql = @"INSERT INTO Users(user_Id, userName,password, Per_Id)
                        VALUES(@UserId , @UserName, @Password, @Per_Id)";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", UserId);
                    cmd.Parameters.AddWithValue("@UserName", UserName);
                    cmd.Parameters.AddWithValue("@Password", Password);
                    cmd.Parameters.AddWithValue("@Per_Id", Per_Id);


                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    return result > 0 ? Json(new { success = true }) : Json(new { success = false });

                }


            }


        }


        [HttpPost("Users/UpdateUser")]
        public IActionResult UpdateUser(int UserId, string UserName, string Password)
        {
            string connStr = _config.GetConnectionString("MySqlConn");

            using (var conn = new MySqlConnection(connStr))
            {
                string sql = @"UPDATE Users 
                               SET userName = @userName,
                                   password = @password
                              WHERE user_Id = @userId";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@userName", UserName);
                    cmd.Parameters.AddWithValue("@password", Password);
                    cmd.Parameters.AddWithValue("@userId", UserId);

                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    return result > 0 ? Json(new { success = true }) : Json(new { success = false });

                }


            }


        }


        [HttpPost("/Users/DeleteUser")]
        public IActionResult DeleteUser(int userId)
        {
            string connStr = _config.GetConnectionString("MySqlConn");

            using (var conn = new MySqlConnection(connStr))
            {

                string sql = @"DELETE FROM Users U WHERE U.user_id = @UserId";

                using (var cmd = new MySqlCommand(sql, conn))
                {

                    cmd.Parameters.AddWithValue("@UserId", userId);

                    conn.Open();
                    int Result = cmd.ExecuteNonQuery();

                    if (Result > 0)
                        return Json(new { success = true });
                    else
                        return Json(new { success = false });

                }

            }
        }


        [HttpGet("/Users/Get_CountUsers")]
        public IActionResult Get_CountUsers()
        {

            string connStr = _config.GetConnectionString("MySqlConn");

            using (var conn = new MySqlConnection(connStr))
            {
                string sql = @"SELECT IFNULL(COUNT(*),0) FROM Users";

                using (var cmd = new MySqlCommand(sql, conn))
                {

                    conn.Open();
                    object Count = cmd.ExecuteScalar();
                    int result = Convert.ToInt32(Count);
                    return Json(result);
                }

            }
        }



        [HttpGet("/Users/Get_NextUsersId")]
        public IActionResult Get_NextInvoiceId()
        {

            string connStr = _config.GetConnectionString("MySqlconn");

            using (var conn = new MySqlConnection(connStr))
            {
                string sql = @"SELECT IFNULL(MAX(user_Id),0) + 1  FROM users";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    int nextUserId = Convert.ToInt32(result);
                    return Json(nextUserId);
                }



            }
        }


    }
}
