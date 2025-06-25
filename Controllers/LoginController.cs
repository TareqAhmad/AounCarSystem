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

            string connStr = _config.GetConnectionString("MySqlConn");

            using (var conn = new MySqlConnection(connStr))
            {
                
               
                string sql = @"SELECT *  
                               FROM Users 
                               WHERE userName = @userName 
                               AND password = @password";

                using (var cmd = new MySqlCommand(sql,conn))
                {

                    cmd.Parameters.AddWithValue("@userName", username);
                    cmd.Parameters.AddWithValue("@password", password);

                    conn.Open();
                    using (var reader = cmd.ExecuteReader()) 
                    {
                        if(reader.Read())
                        {
                            return Json(new { success = true });
                        }
                     

                        return Json(new { success = false});
                        


                    }

                }
                



            }






           
        }


    }
}
