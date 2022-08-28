using CompanyApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace CompanyApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public EmployeeController(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                              select EmployeeId, EmployeeName, Department,
                              convert(varchar(10),DateOfJoining,120)  as DateOfJoining, PhotoFileName from dbo.Employee              
                            ";

            DataTable table = new();
            string sqlDataSource = _configuration.GetConnectionString("CompanyAppCon");
            SqlDataReader myReader;

            using (SqlConnection myConn = new(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand sqlCommand = new(query, myConn))
                {
                    myReader = sqlCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();

                }
            }
            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(Employee employee)
        {
            string query = @"
                              insert into  dbo.Employee
                              (EmployeeName,Department,DateOfJoining,PhotoFileName)  
                              values(@EmployeeName,@Department,@DateOfJoining,@PhotoFileName)             
                            ";

            DataTable table = new();
            string sqlDataSource = _configuration.GetConnectionString("CompanyAppCon");
            SqlDataReader myReader;

            using (SqlConnection myConn = new(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand sqlCommand = new(query, myConn))
                {
                    sqlCommand.Parameters.AddWithValue("@EmployeeName", employee.EmployeeName);
                    sqlCommand.Parameters.AddWithValue("@Department", employee.Department);
                    sqlCommand.Parameters.AddWithValue("@DateOfJoining", employee.DateOfJoining);
                    sqlCommand.Parameters.AddWithValue("@PhotoFileName", employee.PhotoFileName);

                    myReader = sqlCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();

                }
            }
            return new JsonResult("Added Successfully");
        }

        [HttpPut]
        public JsonResult Put(Employee employee)
        {
            string query = @"
                              update dbo.Employee
                              set EmployeeName = @EmployeeName,
                              Department = @Department,
                              DateOfJoining = @DateOfJoining,
                              PhotoFileName = @PhotoFileName
                              where EmployeeId = @EmployeeId
                            ";

            DataTable table = new();
            string sqlDataSource = _configuration.GetConnectionString("CompanyAppCon");
            SqlDataReader myReader;

            using (SqlConnection myConn = new(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand sqlCommand = new(query, myConn))
                {
                    sqlCommand.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);
                    sqlCommand.Parameters.AddWithValue("@EmployeeName", employee.EmployeeName);
                    sqlCommand.Parameters.AddWithValue("@Department", employee.Department);
                    sqlCommand.Parameters.AddWithValue("@DateOfJoining", employee.DateOfJoining);
                    sqlCommand.Parameters.AddWithValue("@PhotoFileName", employee.PhotoFileName);
                    myReader = sqlCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();

                }
            }
            return new JsonResult("Updated Successfully");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
                              delete from dbo.Employee                                
                              where EmployeeId = @EmployeeId
                            ";

            DataTable table = new();
            string sqlDataSource = _configuration.GetConnectionString("CompanyAppCon");
            SqlDataReader myReader;

            using (SqlConnection myConn = new(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand sqlCommand = new(query, myConn))
                {
                    sqlCommand.Parameters.AddWithValue("@EmployeeId", id);
                    myReader = sqlCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();

                }
            }
            return new JsonResult("Deleted Successfully");
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string fileName = postedFile.FileName;
                var physicalPath = _webHostEnvironment.ContentRootPath + "/Photos/" + fileName;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
                return new JsonResult(fileName);
            }
            catch (Exception)
            {

                return new JsonResult("anonymous.png");
            }
        }
    }
}
