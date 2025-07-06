using AounCarSystem.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace AounCarSystem.Controllers
{
    public class UserPermissionsController : Controller
    {
        private readonly IConfiguration _config;
        public UserPermissionsController(IConfiguration config) {
         
            _config = config;
        }


        [HttpGet("/UserPermissions/GetPermissionsToList")]
        public IActionResult GetSupplierToList()
        {
            var PermissionsList = new List<ClsUserPermissions>();

            string connStr = _config.GetConnectionString("MySqlConn");

            using (var conn = new MySqlConnection(connStr))
            {
                string sql = "SELECT Per_Id, PerGroup FROM userpermissions";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PermissionsList.Add(new ClsUserPermissions
                            {
                                Per_Id = Convert.ToInt32(reader["Per_Id"]),
                                PerGroup = reader["PerGroup"].ToString()
                            });
                        }
                    }
                }
            }

            return Json(PermissionsList);
        }

    }
}
