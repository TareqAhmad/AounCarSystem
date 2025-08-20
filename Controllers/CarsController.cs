using AounCarSystem.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using System.Diagnostics.Eventing.Reader;
using System.Linq.Expressions;
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
            try
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

                                ChassisNumber = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                                CarName = reader.IsDBNull(1) ? "" : reader.GetString(1),
                                CarType = reader.IsDBNull(2) ? "" : reader.GetString(2),
                                CarModel = reader.IsDBNull(3) ? "" : reader.GetString(3),
                                CarPrice = reader.IsDBNull(4) ? 0 : reader.GetDecimal(4),
                                Status = reader.IsDBNull(5) ? "" : reader.GetString(5),
                                SupplierName = reader.IsDBNull(6) ? "" : reader.GetString(6)

                            });
                        }

                    }


                }
                return Json(Cars);
            }
            catch(Exception ex) {

                return Json(new { success = false, message = "Error: " + ex.Message });
            }


        }


        [HttpGet("/Cars/Get_CarById")]
        public IActionResult Get_CarById(int ChassisNumber)
        {
            try
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
                                chassisNumber = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                                carName = reader.IsDBNull(1) ? "" : reader.GetString(1),
                                carType = reader.IsDBNull(2) ? "" : reader.GetString(2),
                                carModel = reader.IsDBNull(3) ? "" : reader.GetString(3),
                                carPrice = reader.IsDBNull(4) ? 0 : reader.GetDecimal(4),
                                Status = reader.IsDBNull(5) ? "" : reader.GetString(5),
                                supplierName = reader.IsDBNull(6) ? "" : reader.GetString(6)

                            });
                        }
                        return Json(Car);
                    }


                }
            }

            catch(Exception ex) {

                return Json(new { success = false, message = "Error: " + ex.Message });
            }
            

        }


        [HttpPost("Cars/AddCar")]
        public IActionResult AddCar(int ChassisNumber, string CarName, string CarType, string CarModel, decimal CarPrice, string CarStatus, int SupplierId)
        {
           try
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
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }


        }


        [HttpPost("Cars/UpdateCar")]
        public IActionResult UpdateCar(int ChassisNumber,string CarName, string CarType, string CarModel,decimal CarPrice, string CarStatus)
        {

          try
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
            catch(Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }


        }


        [HttpPost("/Cars/DeleteCar")]
        public IActionResult DeleteCar(int chassisNumber)
        {
            try
            {

                string connStr = _config.GetConnectionString("MySqlConn");

                using (var conn = new MySqlConnection(connStr))
                {

                    string sql = @"DELETE FROM Cars C  WHERE C.ChassisNumber = @ChassisNumber";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {

                        cmd.Parameters.AddWithValue("@ChassisNumber", chassisNumber);

                        conn.Open();
                        int result = cmd.ExecuteNonQuery();
                        return result >  0 ? Json(new { success = true }) :  Json(new { success = false });


                     
                    }

                }
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }


        [HttpGet("/Cars/GetCarToList")]
        public IActionResult GetCarToList()
        {

            try
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
                                    chassisNumber = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                                    carName = reader.IsDBNull(1) ? "" : reader.GetString(1)

                                });
                            }
                        }
                    }
                }

                return Json(CarsList);

            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }


        [HttpGet("/Cars/Get_CarToList")]
        public IActionResult Get_CarToList(int CarId)
        {
            try
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


                                    chassisNumber = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                                    carName = reader.IsDBNull(1) ? "" : reader.GetString(1)
         
                                });
                            }
                        }
                    }
                }

                return Json(CarsList);
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }


        [HttpGet("/Cars/Get_CarInvoice")]
        public IActionResult Get_CarInvoice(int ChassisNumber)
        {

            try
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

                                chassisNumber = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                                carName = reader.IsDBNull(1) ? "" : reader.GetString(1),
                                CarPrice = reader.IsDBNull(2) ? 0 : reader.GetDecimal(2)


                            });
                        }
                        return Json(Car);
                    }


                }
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }

        }


        [HttpGet("/Cars/Get_ReportCars")]
        public IActionResult Get_ReportCars(string condition)
        {
            try
            {


                string connStr = _config.GetConnectionString("MySqlConn");

                using (var conn = new MySqlConnection(connStr))
                {
                    string sql = $@"SELECT COUNT(*) FROM Cars WHERE Status like '%{condition}%'";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {

                        conn.Open();
                        object Count = cmd.ExecuteScalar();
                        int result = Convert.ToInt32(Count);
                        return Json(result);
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
