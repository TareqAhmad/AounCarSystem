using AounCarSystem.Models;
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

    }
}
