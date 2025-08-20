using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using AounCarSystem.Models;
using ZstdSharp.Unsafe;
using System.Diagnostics.Eventing.Reader;

namespace AounCarSystem.Controllers
{
    
    public class SalesController : Controller
    {
        
        private readonly IConfiguration _config;
        public SalesController(IConfiguration config)
        {
            _config = config;
        }


        [HttpGet("/Sales/Get_MasterInvoices")]
        public IActionResult Get_MasterInvoices()
        {
            try
            {
                var Masters = new List<Object>();
                string connStr = _config.GetConnectionString("MySqlconn");

                using (var conn = new MySqlConnection(connStr))
                {
                    string sql = @"SELECT M.Inv_Id, M.Inv_Value, M.Inv_Date,M.payment_type, C.fullName
                                   FROM Master_Sales M
                                   LEFT JOIN Customers C ON M.cust_Id = C.cust_Id";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        conn.Open();
                        var reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            Masters.Add(new
                            {
                                inv_Id =reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                                inv_Value = reader.IsDBNull(1) ? 0 : reader.GetDecimal(1),
                                inv_date =reader.IsDBNull(2) ? "" : reader.GetDateTime(2).ToString("yyyy-MM-dd"),
                                payment_type = reader.IsDBNull(3) ? "" : reader.GetString(3),
                                fullName =reader.IsDBNull(4) ? "" : reader.GetString(4)
                            });
                        }
                        return Json(Masters);
                    }



                }
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }
      

        [HttpGet("/Sales/Get_DetailsInvoice")]
        public IActionResult Get_DetailsInvoice(int Inv_id) 
        {

            try
            {

                var Details = new List<Object>();
                string connStr = _config.GetConnectionString("MySqlconn");

                using (var conn = new MySqlConnection(connStr))
                {
                    string sql = @"SELECT D.Inv_Id, D.ChassisNumber, D.CarName, D.CarPrice,D.Quantity
                                   FROM Details_Sales D
                                   WHERE D.Inv_Id = @InvId";


                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@InvId", Inv_id);

                        conn.Open();

                        var reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            Details.Add(new
                            {
                                inv_Id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                                chassisNumber = reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                                carName = reader.IsDBNull(2) ? "" : reader.GetString(2),
                                carPrice = reader.IsDBNull(3) ? 0 : reader.GetDecimal(3),
                                quantity =reader.IsDBNull(4) ? 0 : reader.GetInt32(4)




                            });
                        }
                        return Json(Details);
                    }



                }
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }

          
        }


        [HttpGet("/Sales/Get_InvoiceById")]
        public IActionResult Get_InvoiceById(int invId)
        {
            try
            {
                var Invoices = new List<ClsMasterSalesInvoice>();
                ClsMasterSalesInvoice invoice = null;

                string connStr = _config.GetConnectionString("MySqlconn");

                using (var conn = new MySqlConnection(connStr))
                {
                    string sql = @"
                               SELECT M.Inv_Id, M.Inv_Value,M.Inv_Date,M.payment_type,C.FullName,
                                      D.ChassisNumber, D.CarName, D.CarPrice, D.Quantity
                               FROM master_sales M
                               INNER JOIN details_sales D ON M.Inv_Id = D.Inv_Id 
                               INNER JOIN customers C ON M.cust_Id = C.cust_Id
                               WHERE M.Inv_Id = @InvoiceId";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@InvoiceId", invId);
                        conn.Open();


                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {

                            if (invoice == null)
                            {
                                invoice = new ClsMasterSalesInvoice
                                {

                                    Inv_Id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                                    Inv_Value = reader.IsDBNull(1) ? 0 : reader.GetDecimal(1),
                                    Inv_Date = reader.IsDBNull(2) ? "" : reader.GetDateTime(2).ToString("yyyy-MM-dd"),
                                    Payment_type = reader.IsDBNull(3) ? "" : reader.GetString(3),
                                    CustomerName = reader.IsDBNull(4) ? "" : reader.GetString(4),
                                    Details = new List<ClsDetailsSalesInvoice>()


                                };

                            }

                            var Detail = new ClsDetailsSalesInvoice
                            {
                                ChassisNumber = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                                CarName = reader.IsDBNull(1) ? "" : reader.GetString(1),
                                CarPrice = reader.IsDBNull(2) ? 0 : reader.GetDecimal(2),
                                Quantity = reader.IsDBNull(3) ? 0 : reader.GetInt32(3)
                            };
                            invoice.Details.Add(Detail);
                        }

                        if (invoice != null)
                            Invoices.Add(invoice);
                    }
                    return Json(Invoices);
                }
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }

        }


        [HttpPost("/Sales/AddInvoice")]
        public IActionResult AddInvoice()
        {
         
            var form = Request.Form;

            int invId = int.Parse(form["invId"]);
            string invDate = form["invDate"];
            string paymentType = form["paymentType"];
            decimal invValue = decimal.Parse(form["invValue"]);
            int custId = int.Parse(form["CustId"]);
            int detailCount = int.Parse(form["detailCount"]);

            string connStr = _config.GetConnectionString("MySqlconn");

            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Insert Master
                        string sqlMaster = @"INSERT INTO Master_Sales (Inv_Id, Inv_Value, Inv_Date,Payment_Type, Cust_Id)
                                             VALUES (@InvId, @InvValue, @InvDate,@PaymentType, @CustId)";

                        using (var cmd = new MySqlCommand(sqlMaster, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@InvId", invId);
                            cmd.Parameters.AddWithValue("@InvValue", invValue);
                            cmd.Parameters.AddWithValue("@InvDate", invDate);
                            cmd.Parameters.AddWithValue("@PaymentType", paymentType);
                            cmd.Parameters.AddWithValue("@CustId", custId);
                            cmd.ExecuteNonQuery();
                        }

                        // Insert Details
                        string sqlDetails = @"INSERT INTO Details_Sales (Inv_Id, ChassisNumber, CarName, CarPrice, Quantity)
                                               VALUES (@InvId, @ChassisNumber, @CarName, @CarPrice, @Quantity)";

                        for (int i = 0; i < detailCount; i++)
                        {
                            int chassisNumber = int.Parse(form[$"chassisNumber{i}"]);
                            string carName = form[$"carName{i}"];
                            decimal carPrice = decimal.Parse(form[$"carPrice{i}"]);
                            int quantity = int.Parse(form[$"quantity{i}"]);

                            using (var cmdDetail = new MySqlCommand(sqlDetails, conn, transaction))
                            {
                                cmdDetail.Parameters.AddWithValue("@InvId", invId);
                                cmdDetail.Parameters.AddWithValue("@ChassisNumber", chassisNumber);
                                cmdDetail.Parameters.AddWithValue("@CarName", carName);
                                cmdDetail.Parameters.AddWithValue("@CarPrice", carPrice);
                                cmdDetail.Parameters.AddWithValue("@Quantity", quantity);
                                cmdDetail.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                        return Json(new { success = true });
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return Json(new { success = false, message = ex.Message });
                    }
                }
            }
        }



        [HttpPost("/Sales/DeleteInvoice")]
        public IActionResult DeleteInvoice(int invId)
        {
            try
            {
                string connStr = _config.GetConnectionString("MySqlConn");

                using (var conn = new MySqlConnection(connStr))
                {

                    string sql = @"DELETE FROM master_sales M WHERE M.Inv_Id = @InvId";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {

                        cmd.Parameters.AddWithValue("@InvId", invId);

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


        [HttpGet("/Sales/Get_TotalSales")]
        public IActionResult Get_TotalSales()
        {
            try
            {
                string connStr = _config.GetConnectionString("MySqlConn");

                using (var conn = new MySqlConnection(connStr))
                {
                    string sql = "SELECT IFNULL(SUM(Inv_Value),0) FROM master_sales";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        conn.Open();
                        object SumSales = cmd.ExecuteScalar();
                        decimal result = Convert.ToDecimal(SumSales);
                        return Json(result);

                    }

                }
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }



        [HttpGet("/Sales/Get_NextInvoiceId")]
        public IActionResult Get_NextInvoiceId()
        {
            try
            {
                string connStr = _config.GetConnectionString("MySqlconn");

                using (var conn = new MySqlConnection(connStr))
                {
                    string sql = @"SELECT IFNULL(MAX(Inv_Id),0) + 1  FROM Master_Sales M";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        conn.Open();
                        object result = cmd.ExecuteScalar();
                        int nextInvoiceId = Convert.ToInt32(result);
                        return Json(nextInvoiceId);
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

