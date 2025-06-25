using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace AounCarSystem.Controllers
{
    public class CustomersController : Controller
    {
        private readonly IConfiguration _config;

        public CustomersController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet("/Customers/Get_AllCustomers")]
        public IActionResult Get_AllCustomers()
        {
            var Customers = new List<object>();
            string connStr = _config.GetConnectionString("MySqlConn");

            using (var conn = new MySqlConnection(connStr))
            {
                string sql = "";
                sql = @"SELECT C.Cust_Id,C.FullName,C.Phone,C.Email,C.Address,C.NationalID
                           FROM Customers C";

                using (var cmd = new MySqlCommand(sql, conn))
                {

                    conn.Open();
                    var reader = cmd.ExecuteReader();
                    {
                        while (reader.Read())
                        {

                            Customers.Add(new
                            {
                                cust_Id = reader["cust_Id"],
                                fullName = reader["fullName"],
                                phone = reader["phone"],
                                email = reader["email"],
                                address = reader["address"],
                                nationalID = reader["nationalID"],
                              
                            });
                        }

                        return Json(Customers);

                    }

                }







            }




        }
    }

}