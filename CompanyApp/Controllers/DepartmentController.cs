using CompanyApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace CompanyApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                              select DepartmentId, DepartmentName from dbo.Department              
                            ";

            DataTable table = new();
            string sqlDataSource = _configuration.GetConnectionString("CompanyAppCon");
            SqlDataReader myReader;

            using(SqlConnection myConn = new(sqlDataSource))
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
        public JsonResult Post(Department department)
        {
            string query = @"
                              insert into  dbo.Department
                              values(@DepartmentName)             
                            ";

            DataTable table = new();
            string sqlDataSource = _configuration.GetConnectionString("CompanyAppCon");
            SqlDataReader myReader;

            using (SqlConnection myConn = new(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand sqlCommand = new(query, myConn))
                {
                    sqlCommand.Parameters.AddWithValue("@DepartmentName", department.DepartmentName);
                    myReader = sqlCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();

                }
            }
            return new JsonResult("Added Successfully");
        }

        [HttpPut]
        public JsonResult Put(Department department)
        {
            string query = @"
                              update dbo.Department
                              set DepartmentName = @DepartmentName     
                              where DepartmentId = @DepartmentId
                            ";

            DataTable table = new();
            string sqlDataSource = _configuration.GetConnectionString("CompanyAppCon");
            SqlDataReader myReader;

            using (SqlConnection myConn = new(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand sqlCommand = new(query, myConn))
                {
                    sqlCommand.Parameters.AddWithValue("@DepartmentId", department.DepartmentId);
                    sqlCommand.Parameters.AddWithValue("@DepartmentName", department.DepartmentName);
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
                              delete from dbo.Department                                
                              where DepartmentId = @DepartmentId
                            ";

            DataTable table = new();
            string sqlDataSource = _configuration.GetConnectionString("CompanyAppCon");
            SqlDataReader myReader;

            using (SqlConnection myConn = new(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand sqlCommand = new(query, myConn))
                {
                    sqlCommand.Parameters.AddWithValue("@DepartmentId", id);
                    myReader = sqlCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();

                }
            }
            return new JsonResult("Deleted Successfully");
        }
    }
}

