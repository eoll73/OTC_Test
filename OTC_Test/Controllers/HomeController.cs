using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OTC_Test.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace OTC_Test.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;

        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("Default Connection"));
            }
        }

        public IActionResult Index()
        {
            var items = GetAllUser();
            return View(items);
        }

        public IActionResult DepartmentsInfo()
        {
            var items = GetDepartmentInfoList();
            return View(items);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private List<Employee> GetAllUser()
        {
            using (IDbConnection db = Connection)
            {
                var result = db.Query<Employee>("SELECT * FROM Employee").ToList();

                return result;
            }
        }

        private List<DepartmentInfo> GetDepartmentInfoList()
        {
            using (IDbConnection db = Connection)
            {
                var result = db.Query<DepartmentInfo>(@"SELECT TOP(1000) d.[Id]
      , d.[Name]
      , d.[Salary]
      , (SELECT COUNT(*) FROM dbo.Employee AS e Where e.DepartmentId = d.Id) AS EmployeeCount
  FROM[Example].[dbo].[Department] AS d



").ToList();

                return result;
            }
        }

        public class Employee
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public double Salary { get; set; }
            public string DepartmentId { get; set; }

        }

        public class DepartmentInfo
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public double Salary { get; set; }
            public int EmployeeCount { get; set; }
        }

    }
}
