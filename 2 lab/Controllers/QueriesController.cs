using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using DB_lab2.Models;
using ClosedXML.Excel;

namespace DB_lab2.Controllers
{
    public class QueriesController : Controller
    {
        private const string CONNECTION_STRING = "Server=WIN-EPG4JRCB2OV;Database=3PoisonAPI;Trusted_Connection=True;MultipleActiveResultSets=true";
        private const string ERR = "Немає результатів для даного запиту";
        private const string DEFAULT_PATH = @"C:\Users\User\OneDrive\Рабочий стол\Уник\Proga\DB\DB_lab2\Queries\";
        private readonly _3PoisonAPIContext _context;

        public QueriesController(_3PoisonAPIContext context)
        {
            _context = context;
        }
        public IActionResult Index(int errorCode)
        {
            var actors = _context.Herbalists.Select(a => a.Name).Distinct().ToList();

            ViewBag.HerbalistsList = new SelectList(_context.Herbalists, "Name", "Name");
            ViewBag.PoisonsList = new SelectList(_context.Poisons, "Name", "Name");
            ViewBag.PoisonersList = new SelectList(_context.Poisoners, "Name", "Name");
            if (errorCode == 1) ViewBag.Error1 = "Недопустиме значення";
            if (errorCode == 2) ViewBag.Error2 = "Недопустиме значення";
            if (errorCode == 3) ViewBag.Error3 = "Недопустиме значення";
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Simple_Query(Query queryModel)
        {
            string query = System.IO.File.ReadAllText(DEFAULT_PATH + "SIMPLE_QUERY.sql");
            query = query.Replace("HerbalistName", "N\'" + queryModel.HerbalistName + "\'");
            query = query.Replace("\r\n", " ");
            query = query.Replace('\t', ' ');

            queryModel.QueryId = "S1";
            queryModel.PoisonersNames = new List<string>();

            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        int flag = 0;
                        while (reader.Read())
                        {
                            queryModel.PoisonersNames.Add(reader.GetString(0));
                            flag++;
                        }
                        if (flag == 0)
                        {
                            queryModel.ErrorFlag = 1;
                            queryModel.Error = ERR;
                        }
                    }

                }
                connection.Close();
            }
            return RedirectToAction("Result", queryModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Simple_Query2(Query queryModel)
        {
            string query = System.IO.File.ReadAllText(DEFAULT_PATH + "SIMPLE_QUERY2.sql");
            query = query.Replace("PoisonName", "N\'" + queryModel.PoisonName + "\'");
            query = query.Replace("PoisonerBirthDate", queryModel.BirthDate.ToString());
            query = query.Replace("\r\n", " ");
            query = query.Replace('\t', ' ');

            queryModel.QueryId = "S2";
            queryModel.PoisonersNames = new List<string>();
            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        int flag = 0;
                        while (reader.Read())
                        {
                            queryModel.PoisonersNames.Add(reader.GetString(0));
                            flag++;
                        }
                        if (flag == 0)
                        {
                            queryModel.ErrorFlag = 1;
                            queryModel.Error = ERR;
                        }
                    }

                }
                connection.Close();
            }
            return RedirectToAction("Result", queryModel);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Simple_Query3(Query queryModel)
        {
            string query = System.IO.File.ReadAllText(DEFAULT_PATH + "SIMPLE_QUERY3.sql");
            query = query.Replace("CountOfPoisons", queryModel.CountOfPoisons.ToString());
            query = query.Replace("\r\n", " ");
            query = query.Replace('\t', ' ');

            queryModel.QueryId = "S3";
            queryModel.PoisonersNames = new List<string>();

            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        int flag = 0;
                        while (reader.Read())
                        {
                            queryModel.PoisonersNames.Add(reader.GetString(0));
                            flag++;
                        }
                        if (flag == 0)
                        {
                            queryModel.ErrorFlag = 1;
                            queryModel.Error = ERR;
                        }
                    }

                }
                connection.Close();
            }
            return RedirectToAction("Result", queryModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Simple_Query4(Query queryModel)
        {
            string query = System.IO.File.ReadAllText(DEFAULT_PATH + "SIMPLE_QUERY4.sql");
            query = query.Replace("PoisonName", "N\'" + queryModel.PoisonName + "\'");
            query = query.Replace("\r\n", " ");
            query = query.Replace('\t', ' ');

            queryModel.QueryId = "S4";
            queryModel.PoisonersNames = new List<string>();
            queryModel.PoisonersBirthDates = new List<int>();
            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        int flag = 0;
                        while (reader.Read())
                        {
                            queryModel.PoisonersNames.Add(reader.GetString(0));
                            queryModel.PoisonersBirthDates.Add(reader.GetInt32(1));
                            flag++;
                        }
                        if (flag == 0)
                        {
                            queryModel.ErrorFlag = 1;
                            queryModel.Error = ERR;
                        }
                    }

                }
                connection.Close();
            }
            return RedirectToAction("Result", queryModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Simple_Query5(Query queryModel)
        {
            string query = System.IO.File.ReadAllText(DEFAULT_PATH + "SIMPLE_QUERY5.sql");
            query = query.Replace("PoisonName", "N\'" + queryModel.PoisonName + "\'");
            query = query.Replace("\r\n", " ");
            query = query.Replace('\t', ' ');

            queryModel.QueryId = "S5";
            queryModel.AddressesNames = new List<string>();

            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        int flag = 0;
                        while (reader.Read())
                        {
                            queryModel.AddressesNames.Add(reader.GetString(0));
                            flag++;
                        }
                        if (flag == 0)
                        {
                            queryModel.ErrorFlag = 1;
                            queryModel.Error = ERR;
                        }
                    }

                }
                connection.Close();
            }
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var worksheet = workbook.Worksheets.Add();

                worksheet.Cell("A1").Value = "Адреси фармацевтів" + queryModel.PoisonName;
                worksheet.Row(1).Style.Font.Bold = true;

                for (int i = 0; i < queryModel.AddressesNames.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = queryModel.AddressesNames[i];
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {

                        FileDownloadName = $"PoisonBase_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }

            return RedirectToAction("Result", queryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Mul_Query(Query queryModel)
        {
            string query = System.IO.File.ReadAllText(DEFAULT_PATH + "MUL_QUERY.sql");
            query = query.Replace("HerbalistName", "N\'" + queryModel.HerbalistName + "\'");
            query = query.Replace("\r\n", " ");
            query = query.Replace('\t', ' ');

            queryModel.QueryId = "M1";
            queryModel.HerbalistsNames = new List<string>();

            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        int flag = 0;
                        while (reader.Read())
                        {
                            queryModel.HerbalistsNames.Add(reader.GetString(0));
                            flag++;
                        }
                        if (flag == 0)
                        {
                            queryModel.ErrorFlag = 1;
                            queryModel.Error = ERR;
                        }
                    }

                }
                connection.Close();
            }
            return RedirectToAction("Result", queryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Mul_Query2(Query queryModel)
        {
            string query = System.IO.File.ReadAllText(DEFAULT_PATH + "MUL_QUERY2.sql");
            query = query.Replace("PoisonerName", "N\'" + queryModel.PoisonerName + "\'");
            query = query.Replace("\r\n", " ");
            query = query.Replace('\t', ' ');

            queryModel.QueryId = "M2";
            queryModel.PoisonersNames = new List<string>();

            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        int flag = 0;
                        while (reader.Read())
                        {
                            queryModel.PoisonersNames.Add(reader.GetString(0));
                            flag++;
                        }
                        if (flag == 0)
                        {
                            queryModel.ErrorFlag = 1;
                            queryModel.Error = ERR;
                        }
                    }

                }
                connection.Close();
            }
            return RedirectToAction("Result", queryModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Mul_Query3(Query queryModel)
        {
            string query = System.IO.File.ReadAllText(DEFAULT_PATH + "MUL_QUERY3.sql");
            query = query.Replace("PoisonerBirthDate", queryModel.BirthDate.ToString());
            query = query.Replace("\r\n", " ");
            query = query.Replace('\t', ' ');

            queryModel.QueryId = "M3";
            queryModel.PoisonsNames = new List<string>();

            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        int flag = 0;
                        while (reader.Read())
                        {
                            queryModel.PoisonsNames.Add(reader.GetString(0));
                            flag++;
                        }
                        if (flag == 0)
                        {
                            queryModel.ErrorFlag = 1;
                            queryModel.Error = ERR;
                        }
                    }

                }
                connection.Close();
            }
            return RedirectToAction("Result", queryModel);

        }
        public IActionResult Result(Query queryResult)
        {
            return View(queryResult);
        }
    }
}