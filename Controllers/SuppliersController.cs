using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace AounCarSystem.Controllers
{
    public class SuppliersController : Controller
    {
        private readonly IConfiguration _config;

        public SuppliersController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet("/Suppliers/Get_AllSuppliers")]
        public IActionResult Get_AllSuppliers()
        {
            var Suppliers = new List<object>();
            string connStr = _config.GetConnectionString("MySqlConn");

            using (var conn = new MySqlConnection(connStr))
            {
                string sql = "";
                sql = @"SELECT S.supplier_Id,S.CompanyName,S.Phone,S.Email,S.Address
                           FROM Suppliers S";

                using (var cmd = new MySqlCommand(sql, conn))
                {

                    conn.Open();
                    var reader = cmd.ExecuteReader();
                    {
                        while (reader.Read())
                        {

                            Suppliers.Add(new
                            {
                                supplier_Id = reader["supplier_Id"],
                                companyName = reader["companyName"],
                                phone = reader["phone"],
                                email = reader["email"],
                                address = reader["address"],

                            });
                        }

                        return Json(Suppliers);

                    }

                }







            }




        }
    }
}
