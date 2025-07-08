using AounCarSystem.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Diagnostics.Eventing.Reader;
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


        [HttpGet("/Cars/Get_CarById")]
        public IActionResult Get_CarById(int ChassisNumber)
        {
            var Car = new List<object>();
            string connStr = _config.GetConnectionString("MySqlConn");

            using (var conn = new MySqlConnection(connStr))
            {

                string sql = @"SELECT c.ChassisNumber, c.CarName, c.CarType, c.CarModel, c.CarPrice, c.Status,
                                    s.CompanyName AS SupplierName
                             FROM Cars c
                             LEFT JOIN Suppliers s ON c.Supplier_Id = s.Supplier_Id
                             WHERE c.ChassisNumber = @ChassisNumber";


                using (var cmd = new MySqlCommand(sql, conn))
                {

                    cmd.Parameters.AddWithValue("@ChassisNumber", ChassisNumber);
                    conn.Open();
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {

                        Car.Add(new
                        {

                            chassisNumber = reader["chassisNumber"],
                            carName = reader["carName"],
                            carType = reader["carType"],
                            carModel = reader["carModel"],
                            carPrice = reader["carPrice"],
                            status = reader["status"],
                            supplierName = reader["supplierName"]

                        });
                    }
                    return Json(Car);
                }


            }

        }


        [HttpPost("Cars/AddCar")]
        public IActionResult AddCar(int ChassisNumber, string CarName, string CarType, string CarModel, decimal CarPrice, string CarStatus, int SupplierId)
        {
            string connStr = _config.GetConnectionString("MySqlConn");

            using (var conn = new MySqlConnection(connStr))
            {
                string sql = @"INSERT INTO Cars(ChassisNumber, CarName, CarType, CarModel, CarPrice, Status, Supplier_Id)
                              VALUES(@ChassisNumber , @CarName, @CarType, @CarModel, @CarPrice, @Status,@SupplierId)";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ChassisNumber", ChassisNumber);
                    cmd.Parameters.AddWithValue("@CarName", CarName);
                    cmd.Parameters.AddWithValue("@CarType", CarType);
                    cmd.Parameters.AddWithValue("@CarModel", CarModel);
                    cmd.Parameters.AddWithValue("@CarPrice", CarPrice);
                    cmd.Parameters.AddWithValue("@Status", CarStatus);
                    cmd.Parameters.AddWithValue("@SupplierId", SupplierId);

                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    return result > 0 ? Json(new { success = true }) : Json(new { success = false });

                }


            }


        }


        [HttpPost("Cars/UpdateCar")]
        public IActionResult UpdateCar(int ChassisNumber,string CarName, string CarType, string CarModel,decimal CarPrice, string CarStatus)
        {
            string connStr = _config.GetConnectionString("MySqlConn");

            using (var conn = new MySqlConnection(connStr))
            {
                string sql = @"UPDATE Cars 
                               SET carName = @CarName,
                                   carType = @CarType,
                                   carModel = @CarModel,
                                   carPrice = @CarPrice,
                                   status = @Status
                              WHERE chassisNumber = @chassisNumber";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@CarName", CarName);
                    cmd.Parameters.AddWithValue("@CarType", CarType);
                    cmd.Parameters.AddWithValue("@CarModel", CarModel);
                    cmd.Parameters.AddWithValue("@CarPrice", CarPrice);
                    cmd.Parameters.AddWithValue("@Status", CarStatus);
                    cmd.Parameters.AddWithValue("@chassisNumber", ChassisNumber);

                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    return result > 0 ? Json(new { success = true }) : Json(new { success = false }); 

                }


            }


        }


        [HttpPost("/Cars/DeleteCar")]
        public IActionResult DeleteCar(int chassisNumber)
        {
            string connStr = _config.GetConnectionString("MySqlConn");

            using (var conn = new MySqlConnection(connStr))
            {

                string sql = @"DELETE FROM Cars C  WHERE C.ChassisNumber = @ChassisNumber";

                using (var cmd = new MySqlCommand(sql, conn))
                {

                    cmd.Parameters.AddWithValue("@ChassisNumber", chassisNumber);

                    conn.Open();
                    int Result = cmd.ExecuteNonQuery();

                    if (Result > 0)
                        return Json(new { success = true });
                    else
                        return Json(new { success = false });

                }

            }
        }


        [HttpGet("/Cars/GetCarToList")]
        public IActionResult GetCarToList()
        {
            var CarsList = new List<object>();

            string connStr = _config.GetConnectionString("MySqlConn");

            using (var conn = new MySqlConnection(connStr))
            {
                string sql = "SELECT ChassisNumber, CarName FROM Cars";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CarsList.Add(new
                            {
                                chassisNumber = Convert.ToInt32(reader["ChassisNumber"]),
                                carName = reader["CarName"].ToString()
                            });
                        }
                    }
                }
            }

            return Json(CarsList);
        }


        [HttpGet("/Cars/Get_CarToList")]
        public IActionResult Get_CarToList(int CarId)
        {
            var CarsList = new List<object>();

            string connStr = _config.GetConnectionString("MySqlConn");

            using (var conn = new MySqlConnection(connStr))
            {
                string sql = "SELECT ChassisNumber, CarName FROM Cars WHERE ChassisNumber= @CarId";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@CarId", CarId); 

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CarsList.Add(new
                            {
                                chassisNumber = Convert.ToInt32(reader["ChassisNumber"]),
                                carName = reader["CarName"].ToString()
                            });
                        }
                    }
                }
            }

            return Json(CarsList);
        }


        [HttpGet("/Cars/Get_CarInvoice")]
        public IActionResult Get_CarInvoice(int ChassisNumber)
        {
            var Car = new List<object>();
            string connStr = _config.GetConnectionString("MySqlConn");

            using (var conn = new MySqlConnection(connStr))
            {

                string sql = @"SELECT c.ChassisNumber, c.CarName, c.CarPrice
                             FROM Cars c
                             WHERE c.ChassisNumber = @ChassisNumber";


                using (var cmd = new MySqlCommand(sql, conn))
                {

                    cmd.Parameters.AddWithValue("@ChassisNumber", ChassisNumber);
                    conn.Open();
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {

                        Car.Add(new
                        {

                            chassisNumber = reader["chassisNumber"],
                            carName = reader["carName"],
                            carPrice = reader["carPrice"],

                        });
                    }
                    return Json(Car);
                }


            }

        }



        [HttpGet("/Cars/Get_ReportCars")]
        public IActionResult Get_ReportCars(string condition)
        {
          
            string connStr = _config.GetConnectionString("MySqlConn"); 

            using(var conn = new MySqlConnection(connStr))
            {
                string sql = $@"SELECT COUNT(*) FROM Cars WHERE Status like '%{condition}%'";
                
                using(var cmd = new MySqlCommand(sql, conn))
                {

                    conn.Open();
                    object Count = cmd.ExecuteScalar();
                    int result = Convert.ToInt32(Count);
                    return Json(result); 
                }

            }
        }




    }
}
