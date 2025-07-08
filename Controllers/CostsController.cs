using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace AounCarSystem.Controllers
{
    public class CostsController : Controller
    {
        private readonly IConfiguration _config;

        public CostsController(IConfiguration config)
        {

            _config = config;

        }


        [HttpGet("/Costs/Get_AllCosts")]
        public IActionResult Get_AllCosts()
        {
            var Costs = new List<object>();

            string connStr = _config.GetConnectionString("MySqlConn");

            using (var conn = new MySqlConnection(connStr))
            {

                string sql = @"SELECT S.Cost_Id, S.CostName, S.CostValue, C.CarName
                               FROM Costs S
                               LEFT JOIN Cars C ON S.chassisNumber=C.chassisNumber";


                using (var cmd = new MySqlCommand(sql, conn))
                {

                    conn.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {

                        Costs.Add(new
                        {

                            Cost_Id = reader["cost_Id"],
                            CostName = reader["costName"],
                            CostValue = reader["costValue"],
                            carName = reader["CarName"]

                        });
                    }
                    return Json(Costs);
                }


            }
        }


        [HttpGet("/Costs/Get_CostById")]
        public IActionResult Get_CostById(int costId)
        {

            var cost = new List<Object>();

            string connStr = _config.GetConnectionString("MySqlConn");

            using (var conn = new MySqlConnection(connStr))
            {


                string sql = @"SELECT C.Cost_Id, C.CostName, C.CostValue,C.ChassisNumber
                               FROM Costs C
                               WHERE C.Cost_Id = @CostId";


                using (var cmd = new MySqlCommand(sql, conn))
                {

                    cmd.Parameters.AddWithValue("@CostId", costId);

                    conn.Open();
                    var reader = cmd.ExecuteReader();


                    while (reader.Read())
                    {

                        cost.Add(new
                        {

                            Cost_Id = reader["cost_Id"],
                            CostName = reader["costName"],
                            CostValue = reader["costValue"],
                            ChassisNumber = reader["chassisNumber"]

                        });
                    }
                    return Json(cost);
                }

            }
        }


        [HttpPost("/Costs/AddCost")]
        public IActionResult AddCost(int CostId, string CostName, decimal CostValue, int CarId)
        {

            string connStr = _config.GetConnectionString("MySqlConn");

            using (var conn = new MySqlConnection(connStr))
            {
                string sql = @"INSERT INTO Costs(Cost_Id, CostName, CostValue, ChassisNumber)
                        VALUES(@CostId , @CostName, @CostValue, @CarId)";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@CostId", CostId);
                    cmd.Parameters.AddWithValue("@CostName", CostName);
                    cmd.Parameters.AddWithValue("@CostValue", CostValue);
                    cmd.Parameters.AddWithValue("@CarId", CarId);


                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    return result > 0 ? Json(new { success = true }) : Json(new { success = false });

                }


            }


        }


        [HttpPost("Costs/UpdateCost")]
        public IActionResult UpdateExpense(int costId, string costName, decimal CostValue, int chassisNumber)
        {
            string connStr = _config.GetConnectionString("MySqlConn");

            using (var conn = new MySqlConnection(connStr))
            {
                string sql = @"UPDATE Costs 
                                SET costName = @costName,
                                   CostValue = @CostValue,
                                   ChassisNumber = @ChassisNumber
                               WHERE Cost_Id = @costId";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@costName", costName);
                    cmd.Parameters.AddWithValue("@CostValue", CostValue);
                    cmd.Parameters.AddWithValue("@ChassisNumber", chassisNumber);
                    cmd.Parameters.AddWithValue("@costId", costId);

                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    return result > 0 ? Json(new { success = true }) : Json(new { success = false });

                }


            }


        }


        [HttpPost("/Costs/DeleteCost")]
        public IActionResult DeleteCar(int costId)
        {
            string connStr = _config.GetConnectionString("MySqlConn");

            using (var conn = new MySqlConnection(connStr))
            {

                string sql = @"DELETE FROM Costs C WHERE C.Cost_Id = @CostId";

                using (var cmd = new MySqlCommand(sql, conn))
                {

                    cmd.Parameters.AddWithValue("@CostId", costId);

                    conn.Open();
                    int Result = cmd.ExecuteNonQuery();

                    if (Result > 0)
                        return Json(new { success = true });
                    else
                        return Json(new { success = false });

                }

            }
        }


        [HttpGet("/Costs/Get_TotalCosts")]
        public IActionResult Get_TotalCosts()
        {
            string connStr = _config.GetConnectionString("MySqlConn");

            using (var conn = new MySqlConnection(connStr))
            {
                string sql = "SELECT IFNULL(SUM(costValue),0) FROM costs";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    conn.Open();
                    object SumExpenses = cmd.ExecuteScalar();
                    decimal result = Convert.ToDecimal(SumExpenses);

                    return Json(result);


                }

            }
        }


        [HttpGet("/Users/Get_NextCostId")]
        public IActionResult Get_NextCostId()
        {

            string connStr = _config.GetConnectionString("MySqlconn");

            using (var conn = new MySqlConnection(connStr))
            {
                string sql = @"SELECT IFNULL(MAX(Cost_Id),0) + 1  FROM Costs";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    int nextCostId = Convert.ToInt32(result);
                    return Json(nextCostId);
                }



            }
        }



    }
}
