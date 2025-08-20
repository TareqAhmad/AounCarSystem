using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient; 

namespace AounCarSystem.Controllers
{
    public class HomeController : Controller
    {

        private readonly IConfiguration _config;

        public HomeController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet("/Home/Get_infoCars")]

        public IActionResult Get_infoCars()
        {
            try
            {
                var Cars = new List<object>();

                string connStr = _config.GetConnectionString("MySqlConn");

                using (var conn = new MySqlConnection(connStr))
                {
                   
                    string sql = @"SELECT C.chassisNumber,C.carName,C.status,C.carPrice,IFNULL(sum(O.costValue),0) AS SumCosts, IFNULL(C.carPrice, 0) - IFNULL(SUM(O.costValue), 0) AS NetProfit
                                   FROM Cars C
                                   LEFT JOIN Costs O ON C.chassisNumber = O.chassisNumber
                                   GROUP BY C.chassisNumber,C.carName
                                   ORDER BY 3";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {

                        conn.Open();
                        var reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {

                            Cars.Add(new
                            {
                                ChassisNumber = reader.IsDBNull(0) ? 0 : reader.GetInt32(0) ,
                                CarName = reader.IsDBNull(1) ? "" : reader.GetString(1),
                                status = reader.IsDBNull(2) ? "" : reader.GetString(2),
                                carPrice = reader.IsDBNull(3) ? 0 : reader.GetDecimal(3),
                                SumCosts = reader.IsDBNull(4) ? 0 : reader.GetDecimal(4),
                                Profit = reader.IsDBNull(5) ? 0 : reader.GetDecimal(5)
                            });
                        }

                    }
                }
                return Json(Cars);
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }


        [HttpGet("/Home/Get_ExpensesCar")]

        public IActionResult Get_ExpensesCar(int ChassisNumber)
        {

            try
            {
                var Expenses = new List<object>();

                string connStr = _config.GetConnectionString("MySqlConn");

                using (var conn = new MySqlConnection(connStr))
                {
                    string sql = @"SELECT E.Expense_Id,E.ExpenseType,E.Amount,E.ExpenseDate,E.Description
                                FROM Expenses E
                                WHERE E.chassisNumber = @CarId";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@CarId", ChassisNumber);
                        conn.Open();
                        var reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {

                            Expenses.Add(new
                            {
                                expenseId = reader.IsDBNull(0) ? 0:reader.GetInt32(0),
                                expenseType = reader.IsDBNull(1) ? "":reader.GetString(1),
                                amount = reader.IsDBNull(2) ? 0: reader.GetDecimal(2),
                                expenseDate = reader.IsDBNull(3) ? "" : reader.GetDateTime(3).ToString("yyyy-MM-dd"),
                                description = reader.IsDBNull(4) ? "" : reader.GetString(4)
                            });
                        }

                    }
                }
                return Json(Expenses);
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }


        }


        [HttpGet("/Home/Get_CostsCar")]

        public IActionResult Get_CostsCar(int ChassisNumber)
        {
            try
            {
                var Costs = new List<object>();

                string connStr = _config.GetConnectionString("MySqlConn");

                using (var conn = new MySqlConnection(connStr))
                {
                    string sql = @"SELECT C.Cost_Id,C.CostName,C.CostValue
                                FROM Costs C
                                WHERE C.chassisNumber = @CarId";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@CarId", ChassisNumber);
                        conn.Open();
                        var reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {

                            Costs.Add(new
                            {
                                costId = reader.IsDBNull(0)? 0:reader.GetInt32(0),
                                costName = reader.IsDBNull(1) ? "" : reader.GetString(1),
                                costValue = reader.IsDBNull(2) ? 0 : reader.GetDecimal(2),

                            });
                        }

                    }
                }
                return Json(Costs);
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }


        }


    }
}
