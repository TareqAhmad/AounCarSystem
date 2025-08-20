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
            try
            {
                var Customers = new List<object>();
                string connStr = _config.GetConnectionString("MySqlConn");

                using (var conn = new MySqlConnection(connStr))
                {
                    string sql = @"SELECT C.Cust_Id,C.FullName,C.Phone,C.Email,C.Address,
                                  C.NationalID
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
                                    cust_Id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                                    fullName = reader.IsDBNull(1) ? "" : reader.GetString(1),
                                    phone = reader.IsDBNull(2)? "" : reader.GetString(2),
                                    email = reader.IsDBNull(3) ? "" : reader.GetString(3),
                                    address = reader.IsDBNull(4) ? "" : reader.GetString(4),
                                    nationalID = reader.IsDBNull(5) ? "" : reader.GetString(5)

                                });
                            }

                            return Json(Customers);

                        }

                    }
                }
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }




        }


        [HttpGet("/Customers/Get_CustomerById")]
        public IActionResult Get_CustomerById(int custId)
        {

            try
            {
                var Cust = new List<Object>();

                string connStr = _config.GetConnectionString("MySqlConn");

                using (var conn = new MySqlConnection(connStr))
                {
                    string sql = @"SELECT C.Cust_Id,C.FullName,C.Phone,C.Email,C.Address,
                                  C.NationalID
                               FROM Customers C
                               WHERE C.Cust_Id = @CustId";


                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustId", custId);
                        conn.Open();
                        var reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            Cust.Add(new
                            {
                                cust_Id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                                fullName = reader.IsDBNull(1) ? "" : reader.GetString(1),
                                phone = reader.IsDBNull(2) ? "" : reader.GetString(2),
                                email = reader.IsDBNull(3) ? "" : reader.GetString(3),
                                address = reader.IsDBNull(4) ? "" : reader.GetString(4),
                                nationalID = reader.IsDBNull(5) ? "" : reader.GetString(5)

                            });
                        }
                        return Json(Cust);
                    }

                }
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }


        }


        [HttpPost("Customers/AddCustomer")]
        public IActionResult AddCustomer(int CustId, string Custname, string Custphone, string Custemail, string Custaddress, string Custnationalid)
        {
            try
            {
                string connStr = _config.GetConnectionString("MySqlConn");

                using (var conn = new MySqlConnection(connStr))
                {
                    string sql = @"INSERT INTO Customers(Cust_Id, FullName, Phone, Email, Address, NationalID)
                        VALUES(@CustId , @Custname, @Custphone, @Custemail, @Custaddress, @Custnationalid)";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustId", CustId);
                        cmd.Parameters.AddWithValue("@Custname", Custname);
                        cmd.Parameters.AddWithValue("@Custphone", Custphone);
                        cmd.Parameters.AddWithValue("@Custemail", Custemail);
                        cmd.Parameters.AddWithValue("@Custaddress", Custaddress);
                        cmd.Parameters.AddWithValue("@Custnationalid", Custnationalid);

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


        [HttpPost("Customers/UpdateCustomer")]
        public IActionResult UpdateCustomer(int CustId, string CustName, string CustPhone,string CustEmail,string CustAddress, string CustNationalId)
        {
            try
            {
                string connStr = _config.GetConnectionString("MySqlConn");

                using (var conn = new MySqlConnection(connStr))
                {
                    string sql = @"UPDATE Customers 
                                   SET FullName = @CustName,
                                   Phone = @CustPhone,
                                   Email = @CustEmail,
                                   Address = @CustAddress,
                                   NationalID = @CustNationalId
                                   WHERE Cust_Id = @CustId";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustName", CustName);
                        cmd.Parameters.AddWithValue("@CustPhone", CustPhone);
                        cmd.Parameters.AddWithValue("@CustEmail", CustEmail);
                        cmd.Parameters.AddWithValue("@CustAddress", CustAddress);
                        cmd.Parameters.AddWithValue("@CustNationalId", CustNationalId);
                        cmd.Parameters.AddWithValue("@CustId", CustId);

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


        [HttpPost("/Customers/DeleteCustomer")]
        public IActionResult DeleteCustomer(int custId)
        {
            try
            {
                string connStr = _config.GetConnectionString("MySqlConn");

                using (var conn = new MySqlConnection(connStr))
                {

                    string sql = @"DELETE FROM Customers C  WHERE C.Cust_Id = @CustId";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {

                        cmd.Parameters.AddWithValue("@CustId", custId);

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


        [HttpGet("/Customer/Get_CustomerInvoice")]
        public IActionResult Get_CustomerInvoice()
        {
            try
            {
                var Customers = new List<object>();
                string connStr = _config.GetConnectionString("MySqlConn");

                using (var conn = new MySqlConnection(connStr))
                {
                    string sql = @"SELECT C.cust_Id,C.fullName FROM Customers C";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {

                        conn.Open();
                        var reader = cmd.ExecuteReader();
                        {
                            while (reader.Read())
                            {

                                Customers.Add(new
                                {
                                    cust_Id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                                    fullName = reader.IsDBNull(1) ? "" : reader.GetString(1)


                                });
                            }

                            return Json(Customers);

                        }

                    }




                }
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }

       }


        [HttpGet("/Customers/Get_NextCustId")]
        public IActionResult Get_NextCustId()
        {
            try
            {
                string connStr = _config.GetConnectionString("MySqlconn");

                using (var conn = new MySqlConnection(connStr))
                {
                    string sql = @"SELECT IFNULL(MAX(Cust_Id),0) + 1  FROM Customers";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        conn.Open();
                        object result = cmd.ExecuteScalar();
                        int nextCustId = Convert.ToInt32(result);
                        return Json(new { nextCustId });
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