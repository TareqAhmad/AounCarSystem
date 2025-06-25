using AounCarSystem.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;

namespace AounCarSystem.Controllers
{
    public class CarsController : Controller
    {

        private readonly IConfiguration _config;

        public CarsController(IConfiguration config)
        {

            _config = config;

        }

        [HttpGet("/Cars/Get_AllCars")]
        public IActionResult Get_AllCars()
        {
            var Cars = new List<object>();
            string connStr = _config.GetConnectionString("MySqlConn");

            using (var conn = new MySqlConnection(connStr))
            {

                string sql = @"SELECT c.ChassisNumber, c.CarName, c.CarType, c.CarModel, c.CarPrice, c.Status,
                                    s.CompanyName AS SupplierName
                             FROM Cars c
                             LEFT JOIN Suppliers s ON c.Supplier_Id = s.Supplier_Id";


                using (var cmd = new MySqlCommand(sql, conn))
                {

                    conn.Open();
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {

                        Cars.Add(new
                        {

                            ChassisNumber = reader["ChassisNumber"],
                            CarName = reader["CarName"],
                            CarType = reader["CarType"],
                            CarModel = reader["CarModel"],
                            CarPrice = reader["CarPrice"],
                            Status = reader["Status"],
                            SupplierName = reader["SupplierName"]

                        });
                    }
                    return Json(Cars);
                }


            }






        }
        
        [HttpDelete("Delete/{chassisNumber}")]
        public IActionResult Delete(int chassisNumber)
        {
            string connStr = _config.GetConnectionString("MySqlConn");

            using (var conn = new MySqlConnection(connStr))
            {
                string sql = @"DELETE FROM Cars 
                              WHERE chassisNumber = @chassisNumber";
                
               using (var cmd = new MySqlCommand(sql,conn))
                {
                    cmd.Parameters.AddWithValue("@chassisNumber", chassisNumber);
                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    return result > 0 ? Ok() : NotFound();

                }


            }

 
        }
    }
}
