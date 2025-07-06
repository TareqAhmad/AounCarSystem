using AounCarSystem.Models;
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

        [HttpGet("/Suppliers/Get_SupplierById")]
        public IActionResult Get_SupplierById(int supplierId)
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
                                supplier_Id = reader["supplier_Id"],
                                companyName = reader["companyName"],
                                phone = reader["phone"],
                                email = reader["email"],
                                address = reader["address"],

                            });
                        }
                   
                        return Json(supplier);
                }

            }



        }

        [HttpPost("Suppliers/AddSupplier")]
        public IActionResult AddSupplier(int SupplierId, string Suppliername, string Supplierphone, string Supplieremail, string Supplieraddress)
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

        [HttpPost("Suppliers/UpdateSupplier")]
        public IActionResult UpdateCustomer(int SupplierId, string CompanyName, string Phone, string Email, string Address, string CustNationalId)
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

        [HttpPost("/Suppliers/DeleteSupplier")]
        public IActionResult DeleteSupplier(int supplierId)
        {
            string connStr = _config.GetConnectionString("MySqlConn");

            using (var conn = new MySqlConnection(connStr))
            {

                string sql = @"DELETE FROM Suppliers S  WHERE S.supplier_Id = @supplierId";

                using (var cmd = new MySqlCommand(sql, conn))
                {

                    cmd.Parameters.AddWithValue("@supplierId", supplierId);

                    conn.Open();
                    int Result = cmd.ExecuteNonQuery();

                    if (Result > 0)
                        return Json(new { success = true });
                    else
                        return Json(new { success = false });

                }

            }
        }

        [HttpGet("/Suppliers/GetSupplierToList")]
        public IActionResult GetSupplierToList()
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
                                supplier_Id = Convert.ToInt32(reader["supplier_Id"]),
                                companyName = reader["companyName"].ToString()
                            });
                        }
                    }
                }
            }

            return Json(suppliersList);
        }




    }
}
