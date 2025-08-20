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
            try
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

                                Cost_Id = reader.IsDBNull(0)? 0 : reader.GetInt32(0),
                                CostName = reader.IsDBNull(1)? "" : reader.GetString(1),
                                CostValue = reader.IsDBNull(2) ? 0 : reader.GetDecimal(2),
                                carName = reader.IsDBNull(3) ? "" : reader.GetString(3)

                            });
                        }
                        return Json(Costs);
                    }


                }
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }


        [HttpGet("/Costs/Get_CostById")]
        public IActionResult Get_CostById(int costId)
        {
            try
            {
                var cost = new List<Object>();

                string connStr = _config.GetConnectionString("MySqlConn");

                using (var conn = new MySqlConnection(connStr))
                {


                    string sql = @"SELECT cost_Id, costName, costValue,chassisNumber
                                   FROM Costs 
                                   WHERE cost_Id = @CostId";


                    using (var cmd = new MySqlCommand(sql, conn))
                    {

                        cmd.Parameters.AddWithValue("@CostId", costId);

                        conn.Open();
                        var reader = cmd.ExecuteReader();


                        while (reader.Read())
                        {

                            cost.Add(new
                            {
                                cost_Id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                                costName = reader.IsDBNull(1) ? "" : reader.GetString(1),
                                costValue = reader.IsDBNull(2) ? 0 : reader.GetDecimal(2),
                                chassisNumber = reader.IsDBNull(3) ? 0 : reader.GetInt32(3)
                            });
                        }
                        return Json(cost);
                    }

                }
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }


        [HttpPost("/Costs/AddCost")]
        public IActionResult AddCost(int CostId, string CostName, decimal CostValue, int CarId)
        {
            try
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
            catch(Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }


        }


        [HttpPost("Costs/UpdateCost")]
        public IActionResult UpdateCost(int costId, string costName, decimal CostValue, int chassisNumber)
        {
            try
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
            catch(Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }


        }


        [HttpPost("/Costs/DeleteCost")]
        public IActionResult DeleteCost(int costId)
        {
            try
            {
                string connStr = _config.GetConnectionString("MySqlConn");

                using (var conn = new MySqlConnection(connStr))
                {

                    string sql = @"DELETE FROM Costs C WHERE C.Cost_Id = @CostId";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {

                        cmd.Parameters.AddWithValue("@CostId", costId);

                        conn.Open();
                        int result = cmd.ExecuteNonQuery();
                        return result > 0 ? Json(new { success = true }) :   Json(new { success = false });

  
                    }

                }
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }


        [HttpGet("/Costs/Get_TotalCosts")]
        public IActionResult Get_TotalCosts()
        {
            try
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
            catch(Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }


        [HttpGet("/Costs/Get_NextCostId")]
        public IActionResult Get_NextCostId()
        {

            try
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
                        return Json(new { nextCostId });
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
