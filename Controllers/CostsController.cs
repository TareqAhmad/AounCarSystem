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

                string sql = @"SELECT C.Cost_Id, C.CostName, C.CostValue,C.ChassisNumber
                               FROM Costs C";


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
                            ChassisNumber = reader["chassisNumber"]

                        });
                    }
                    return Json(Costs);
                }


            }
        }
    }
}
