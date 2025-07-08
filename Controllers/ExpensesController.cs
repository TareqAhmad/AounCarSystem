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

                            Expense_Id =Convert.ToInt32(reader["expense_Id"]),
                            ExpenseType = reader["expenseType"].ToString(),
                            Amount =Convert.ToDecimal( reader["amount"]),
                            ExpenseDate =reader["expenseDate"].ToString(),
                            Description = reader["description"].ToString(),
                            CarName = reader["carName"].ToString()

                        });
                    }
                    return Json(Expenses);
                }


            }
        }


        [HttpGet("/Expenses/Get_ExpenseById")]
        public IActionResult Get_ExpenseById(int expenseId)
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
                            Expense_Id = Convert.ToInt32(reader["Expense_Id"]),
                            ExpenseType = reader["expenseType"].ToString(),
                            Amount = Convert.ToDecimal(reader["Amount"]),
                            ExpenseDate = reader["expenseDate"].ToString(),
                            Description = reader["Description"].ToString(),
                            ChassisNumber = Convert.ToInt32(reader["ChassisNumber"])

                        });
                    }
                    return Json(Expenses);
                }

            }



        }


        [HttpPost("/Expenses/AddExpense")]
        public IActionResult AddExpense(int ExpenseId, string ExpenseType, decimal ExpenseAmount, string ExpenseDate, string ExpenseDescription, int CarId)
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
                    cmd.Parameters.AddWithValue("@ExpenseDate",ExpenseDate);
                    cmd.Parameters.AddWithValue("@ExpenseDescription", ExpenseDescription);
                    cmd.Parameters.AddWithValue("@CarId", CarId);


                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    return result > 0 ? Json(new { success = true }) : Json(new { success = false });

                }


            }


        }


        [HttpPost("Expenses/UpdateExpense")]
        public IActionResult UpdateExpense(int expensId, string ExpenseType, decimal ExpenseAmount, DateTime ExpenseDate, string ExpenseDescription)
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
        

        [HttpPost("/Expenses/DeleteExpense")]
        public IActionResult DeleteExpense(int expenseId)
        {
            string connStr = _config.GetConnectionString("MySqlConn");

            using (var conn = new MySqlConnection(connStr))
            {
                string sql = @"DELETE FROM Expenses E WHERE E.Expense_Id = @ExpenseId";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ExpenseId", expenseId);
                    conn.Open();
                    int Result = cmd.ExecuteNonQuery();

                    if (Result > 0)
                        return Json(new { success = true });
                    else
                        return Json(new { success = false });

                }


            }


        }


        [HttpGet("/Expenses/Get_TotalExpenses")]
        public IActionResult Get_TotalExpenses()
        {
            string connStr = _config.GetConnectionString("MySqlConn"); 

            using(var conn = new MySqlConnection(connStr))
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


        [HttpGet("/Users/Get_NextExpenseId")]
        public IActionResult Get_NextExpenseId()
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
                    return Json(nextExpenseId);
                }



            }
        }


    }
}
