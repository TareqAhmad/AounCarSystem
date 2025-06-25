using Microsoft.AspNetCore.Mvc;
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
            var Expenses = new List<object>();

            string connStr = _config.GetConnectionString("MySqlConn");

            using (var conn = new MySqlConnection(connStr))
            {
                
                string sql = @"SELECT E.Expense_Id, E.ExpenseType, E.Amount, E.ExpenseDate, E.Description, 
                                      E.ChassisNumber
                             FROM Expenses E";


                using (var cmd = new MySqlCommand(sql, conn))
                {

                    conn.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {

                        Expenses.Add(new
                        {

                            Expense_Id = reader["expense_Id"],
                            ExpenseType = reader["expenseType"],
                            Amount = reader["amount"],
                            ExpenseDate = reader["expenseDate"],
                            Description = reader["description"],
                            ChassisNumber = reader["chassisNumber"]

                        });
                    }
                    return Json(Expenses);
                }


            }
        }

        [HttpDelete("/Expenses/DeleteExpense")]
        public IActionResult DeleteExpense(int Expense_Id)
        {
            string connStr = _config.GetConnectionString("MySqlConn");

            using (var conn = new MySqlConnection(connStr))
            {
                string sql = @"DELETE FROM Cars 
                              WHERE chassisNumber = @chassisNumber";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@chassisNumber", Expense_Id);
                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    return result > 0 ? Ok() : NotFound();

                }


            }


        }
    }
}
