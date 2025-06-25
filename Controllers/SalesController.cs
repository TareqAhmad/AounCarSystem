using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

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
            var Masters = new List<Object>();
            string connStr = _config.GetConnectionString("MySqlconn");

            using (var conn = new MySqlConnection(connStr))
            {
                string sql = @"SELECT M.Inv_Id, M.Inv_Value, M.Inv_Date, C.fullName
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
                            inv_Id = reader["inv_Id"],
                            inv_Value = reader["inv_value"],
                            inv_date = reader["inv_date"],
                            fullName = reader["fullName"]
                        });
                    }
                    return Json(Masters);
                }



            }
        }
        [HttpGet("/Sales/Get_DetailsInvoice")]
        public IActionResult Get_DetailsInvoice(int Inv_id) 
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
                            inv_Id = reader["Inv_Id"],
                            chassisNumber = reader["chassisNumber"],
                            carName = reader["carName"],
                            carPrice = reader["carPrice"],
                            quantity = reader["quantity"]
                        });
                    }
                    return Json(Details);
                }



            }


          
            }

        }
    }

