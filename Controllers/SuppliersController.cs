using AounCarSystem.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;

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
            try
            {
                var Suppliers = new List<object>();
                string connStr = _config.GetConnectionString("MySqlConn");

                using (var conn = new MySqlConnection(connStr))
                {
                    string sql = @"SELECT S.supplier_Id,S.CompanyName,S.Phone,S.Email,S.Address
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
                                    supplier_Id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                                    companyName = reader.IsDBNull(1) ? "" : reader.GetString(1),
                                    phone =reader.IsDBNull(2) ? "" : reader.GetString(2),
                                    email = reader.IsDBNull(3) ? "" : reader.GetString(3),
                                    address = reader.IsDBNull(4) ? "" : reader.GetString(4),

                                });
                            }

                            return Json(Suppliers);

                        }

                    }







                }
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }


        }


        [HttpGet("/Suppliers/Get_SupplierById")]
        public IActionResult Get_SupplierById(int supplierId)
        {
            try
            {
                var supplier = new List<Object>();

                string connStr = _config.GetConnectionString("MySqlConn");

                using (var conn = new MySqlConnection(connStr))
                {

                    string sql = @"SELECT S.supplier_Id,S.CompanyName,S.Phone,S.Email,S.Address
                               FROM Suppliers S
                               WHERE S.supplier_Id = @supplier";


                    using (var cmd = new MySqlCommand(sql, conn))
                    {

                        cmd.Parameters.AddWithValue("@supplier", supplierId);

                        conn.Open();
                        var reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {

                            supplier.Add(new
                            {
                                supplier_Id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                                companyName = reader.IsDBNull(1) ? "" : reader.GetString(1),
                                phone = reader.IsDBNull(2) ? "" : reader.GetString(2),
                                email = reader.IsDBNull(3) ? "" : reader.GetString(3),
                                address = reader.IsDBNull(4) ? "" : reader.GetString(4),

                            });
                        }

                        return Json(supplier);
                    }

                }
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }


        }


        [HttpPost("Suppliers/AddSupplier")]
        public IActionResult AddSupplier(int SupplierId, string Suppliername, string Supplierphone, string Supplieremail, string Supplieraddress)
        {
            try
            {
                string connStr = _config.GetConnectionString("MySqlConn");

                using (var conn = new MySqlConnection(connStr))
                {
                    string sql = @"INSERT INTO Suppliers(supplier_Id, CompanyName, Phone, Email, Address)
                        VALUES(@SupplierId , @Suppliername, @Supplierphone, @Supplieremail, @Supplieraddress)";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@SupplierId", SupplierId);
                        cmd.Parameters.AddWithValue("@Suppliername", Suppliername);
                        cmd.Parameters.AddWithValue("@Supplierphone", Supplierphone);
                        cmd.Parameters.AddWithValue("@Supplieremail", Supplieremail);
                        cmd.Parameters.AddWithValue("@Supplieraddress", Supplieraddress);

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


        [HttpPost("Suppliers/UpdateSupplier")]
        public IActionResult UpdateCustomer(int SupplierId, string CompanyName, string Phone, string Email, string Address, string CustNationalId)
        {
            try
            {
                string connStr = _config.GetConnectionString("MySqlConn");

                using (var conn = new MySqlConnection(connStr))
                {
                    string sql = @"UPDATE Suppliers 
                                SET companyName = @CompanyName,
                                   Phone = @Phone,
                                   Email = @Email,
                                   Address = @Address
                               WHERE supplier_Id = @SupplierId";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@CompanyName", CompanyName);
                        cmd.Parameters.AddWithValue("@Phone", Phone);
                        cmd.Parameters.AddWithValue("@Email", Email);
                        cmd.Parameters.AddWithValue("@Address", Address);
                        cmd.Parameters.AddWithValue("@SupplierId", SupplierId);

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


        [HttpPost("/Suppliers/DeleteSupplier")]
        public IActionResult DeleteSupplier(int supplierId)
        {
            try
            {
                string connStr = _config.GetConnectionString("MySqlConn");

                using (var conn = new MySqlConnection(connStr))
                {

                    string sql = @"DELETE FROM Suppliers S  WHERE S.supplier_Id = @supplierId";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {

                        cmd.Parameters.AddWithValue("@supplierId", supplierId);

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


        [HttpGet("/Suppliers/GetSupplierToList")]
        public IActionResult GetSupplierToList()
        {
            try
            {
                var suppliersList = new List<object>();

                string connStr = _config.GetConnectionString("MySqlConn");

                using (var conn = new MySqlConnection(connStr))
                {
                    string sql = "SELECT Supplier_Id, CompanyName FROM Suppliers";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        conn.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                suppliersList.Add(new
                                {
                                    supplier_Id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                                    companyName = reader.IsDBNull(1) ? "" : reader.GetString(1)
           
                                });
                            }
                        }
                    }
                }

                return Json(suppliersList);
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }


        [HttpGet("/Suppliers/Get_NextSupplierId")]
        public IActionResult Get_NextSupplierId()
        {
            try
            {
                string connStr = _config.GetConnectionString("MySqlconn");

                using (var conn = new MySqlConnection(connStr))
                {
                    string sql = @"SELECT IFNULL(MAX(supplier_Id),0) + 1  FROM Suppliers";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        conn.Open();
                        object result = cmd.ExecuteScalar();
                        int nextSupplierId = Convert.ToInt32(result);
                        return Json(new { nextSupplierId });
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
