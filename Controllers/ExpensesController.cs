using AounCarSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using MySql.Data.MySqlClient;

namespace AounCarSystem.Controllers
{
    public class ExpensesController : Controller
    {
        private readonly IConfiguration _config;

        public ExpensesController(IConfiguration config)
        {

            _config = config;

        }

        [HttpGet("/Expenses/Get_AllExpenses")]
        public IActionResult Get_AllExpenses()
        {
            try
            {

                var Expenses = new List<ClsExpenses>();

                string connStr = _config.GetConnectionString("MySqlConn");

                using (var conn = new MySqlConnection(connStr))
                {

                    string sql = @"SELECT E.Expense_Id, E.ExpenseType, E.Amount, E.ExpenseDate, E.Description, 
                                      C.carName
                             FROM Expenses E
                             LEFT JOIN Cars C ON E.chassisNumber = C.chassisNumber";


                    using (var cmd = new MySqlCommand(sql, conn))
                    {

                        conn.Open();

                        var reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {

                            Expenses.Add(new ClsExpenses
                            {

                                Expense_Id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                                ExpenseType = reader.IsDBNull(1) ? "" : reader.GetString(1),
                                Amount =  reader.IsDBNull(2) ? 0 : reader.GetDecimal(2),
                                ExpenseDate = reader.IsDBNull(3) ? "" : reader.GetDateTime(3).ToString("yyyy-MM-dd"),
                                Description =reader.IsDBNull(4) ? "" : reader.GetString(4),
                                CarName = reader.IsDBNull(5) ? "" : reader.GetString(5)

                            });
                        }
                        return Json(Expenses);
                    }


                }
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }


        [HttpGet("/Expenses/Get_ExpenseById")]
        public IActionResult Get_ExpenseById(int expenseId)
        {
            try
            {
                var Expenses = new List<ClsExpenses>();

                string connStr = _config.GetConnectionString("MySqlConn");

                using (var conn = new MySqlConnection(connStr))
                {

                    string sql = @"SELECT E.Expense_Id, E.ExpenseType, E.Amount, E.ExpenseDate, E.Description, 
                                      E.ChassisNumber
                             FROM Expenses E
                             WHERE E.Expense_Id = @ExpenseId";


                    using (var cmd = new MySqlCommand(sql, conn))
                    {

                        cmd.Parameters.AddWithValue("@ExpenseId", expenseId);

                        conn.Open();
                        var reader = cmd.ExecuteReader();


                        while (reader.Read())
                        {

                            Expenses.Add(new ClsExpenses
                            {

                                Expense_Id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                                ExpenseType = reader.IsDBNull(1) ? "" : reader.GetString(1),
                                Amount = reader.IsDBNull(2) ? 0 : reader.GetDecimal(2),
                                ExpenseDate = reader.IsDBNull(3) ? "" : reader.GetDateTime(3).ToString("yyyy-MM-dd"),
                                Description = reader.IsDBNull(4) ? "" : reader.GetString(4),
                                ChassisNumber = reader.IsDBNull(5) ? 0 : reader.GetInt32(5)

                            });
                        }
                        return Json(Expenses);
                    }

                }
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }



        }


        [HttpPost("/Expenses/AddExpense")]
        public IActionResult AddExpense(int ExpenseId, string ExpenseType, decimal ExpenseAmount, string ExpenseDate, string ExpenseDescription, int CarId)
        {
            try
            {
                string connStr = _config.GetConnectionString("MySqlConn");

                using (var conn = new MySqlConnection(connStr))
                {
                    string sql = @"INSERT INTO Expenses(Expense_Id, ExpenseType, Amount, ExpenseDate, Description, ChassisNumber)
                               VALUES(@ExpenseId , @ExpenseType, @ExpenseAmount, ExpenseDate,@ExpenseDescription, @CarId)";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ExpenseId", ExpenseId);
                        cmd.Parameters.AddWithValue("@ExpenseType", ExpenseType);
                        cmd.Parameters.AddWithValue("@ExpenseAmount", ExpenseAmount);
                        cmd.Parameters.AddWithValue("@ExpenseDate", ExpenseDate);
                        cmd.Parameters.AddWithValue("@ExpenseDescription", ExpenseDescription);
                        cmd.Parameters.AddWithValue("@CarId", CarId);


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


        [HttpPost("Expenses/UpdateExpense")]
        public IActionResult UpdateExpense(int expensId, string ExpenseType, decimal ExpenseAmount, DateTime ExpenseDate, string ExpenseDescription)
        {
            try
            {
                string connStr = _config.GetConnectionString("MySqlConn");

                using (var conn = new MySqlConnection(connStr))
                {
                    string sql = @"UPDATE Expenses 
                                   SET ExpenseType = @ExpenseType,
                                   Amount = @ExpenseAmount,
                                   ExpenseDate = @ExpenseDate,
                                   Description = @ExpenseDescription
                                   WHERE Expense_Id = @expensId";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ExpenseType", ExpenseType);
                        cmd.Parameters.AddWithValue("@ExpenseAmount", ExpenseAmount);
                        cmd.Parameters.AddWithValue("@ExpenseDate", ExpenseDate.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@ExpenseDescription", ExpenseDescription);
                        cmd.Parameters.AddWithValue("@expensId", expensId);

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
        

        [HttpPost("/Expenses/DeleteExpense")]
        public IActionResult DeleteExpense(int expenseId)
        {
            try
            {
                string connStr = _config.GetConnectionString("MySqlConn");

                using (var conn = new MySqlConnection(connStr))
                {
                    string sql = @"DELETE FROM Expenses E WHERE E.Expense_Id = @ExpenseId";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ExpenseId", expenseId);
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


        [HttpGet("/Expenses/Get_TotalExpenses")]
        public IActionResult Get_TotalExpenses()
        {
            try
            {
                string connStr = _config.GetConnectionString("MySqlConn");

                using (var conn = new MySqlConnection(connStr))
                {
                    string sql = "SELECT IFNULL(SUM(amount),0) FROM expenses";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        conn.Open();
                        object SumExpenses = cmd.ExecuteScalar();
                        decimal result = Convert.ToDecimal(SumExpenses);

                        return Json(result);

                    }

                }
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }


        [HttpGet("/Expenses/Get_NextExpenseId")]
        public IActionResult Get_NextExpenseId()
        {
            try
            {
                string connStr = _config.GetConnectionString("MySqlconn");

                using (var conn = new MySqlConnection(connStr))
                {
                    string sql = @"SELECT IFNULL(MAX(Expense_Id),0) + 1  FROM Expenses";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        conn.Open();
                        object result = cmd.ExecuteScalar();
                        int nextExpenseId = Convert.ToInt32(result);
                        return Json(new { nextExpenseId });
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
