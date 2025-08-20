using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;
using AounCarSystem.Models;


namespace AounCarSystem.Controllers
{

    
    public class LoginController : Controller
    {


        private readonly IConfiguration _config;


        public LoginController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("/Login/Authenticate")]
        public IActionResult Authenticate(string username, string password)
        {
            try
            {
                string connStr = _config.GetConnectionString("MySqlConn");

                using (var conn = new MySqlConnection(connStr))
                {


                    string sql = @"SELECT *  
                                   FROM Users 
                                   WHERE userName = @userName 
                                   AND password = @password";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {

                        cmd.Parameters.AddWithValue("@userName", username);
                        cmd.Parameters.AddWithValue("@password", password);

                        conn.Open();
                        var reader = cmd.ExecuteReader();
                      
                        while (reader.Read())
                        {
                            int permission = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);
                            return  Json(new { success = true, permission });
                        }
                        return Json(new { success = false }); 


                    }




                }

            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }




           
        }


    }
}
