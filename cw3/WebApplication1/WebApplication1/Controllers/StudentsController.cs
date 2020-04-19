using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using WebApplication1.Models;
using Microsoft.AspNetCore.Authorization;
using WebApplication1.DTOs;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        // private readonly DAL.IDbService _dbService;

        //  public StudentsController(DAL.IDbService dbService)
        //  {
        //      _dbService = dbService;
        //   }
        public IConfiguration Configuration { get; set; }
        public StudentsController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [HttpGet]
        [Authorize]
        //[Authorize(Roles = "admin")]
        public IActionResult GetStudents()
        {
            var list = new List<Student>();
            using (var con = new SqlConnection("Data Source=db-mssql ;Initial Catalog=s16424; Integrated Security = True"))
            using (var com = new SqlCommand())
            { //imie, nazwisko, dataur, nazwa studiow, nr semestru
                com.Connection = con;
                com.CommandText = "SELECT student.firstname, student.lastname, student.indexnumber, student.birthdate, Name, semester " +
                    "FROM student INNER JOIN enrollment ON student.IdEnrollment = Enrollment.IdEnrollment " +
                    " INNER JOIN studies ON Enrollment.IdStudy = Studies.IdStudy";
                con.Open();
                var dr = com.ExecuteReader();
                while (dr.Read())
                {
                    var st = new Student();
                    st.FirstName = dr["FirstName"].ToString();
                    st.IndexNumber = dr["indexNumber"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    st.Birthdate = (DateTime)dr["BirthDate"];
                    st.Studies = dr["name"].ToString();
                    st.Semester = int.Parse(dr["semester"].ToString());
                    list.Add(st);
                }
                return Ok(list);
            }
        }

        [HttpPost]
        public IActionResult Login(LoginRequestDto request)
        {
         
            var pass = request.Haslo;
            var passDB = "";
            //////////////////////////////
            using (var con = new SqlConnection("Data Source=db-mssql ;Initial Catalog=s16424; Integrated Security = True"))
            using (var com = new SqlCommand())
            { 
                com.Connection = con;
                com.CommandText = "SELECT Passw from student where IndexNumber = @user";
                com.Parameters.AddWithValue("user", request.Login);
                con.Open();
                var dr = com.ExecuteReader();
                while (dr.Read())
                {
                    passDB = dr["password"].ToString();
                }
            }
            if (!pass.Equals(passDB))
            {
                return BadRequest("Incorrect password");
            }

            /////////////////////////


            var claims = new[] 
{
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Name, request.Login),
                new Claim(ClaimTypes.Role, "admin"),
                new Claim(ClaimTypes.Role, "student")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
                (
                issuer: "Gakko",
                audience: "Students",
                claims: claims,
                expires : DateTime.Now.AddMinutes(10),
                signingCredentials: creds
                ) ;

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken = Guid.NewGuid()
            });
        }


        [HttpGet("{indexNumber}")]

        public IActionResult GetStudents(String indexNumber)
        {
            var list = new List<Student>();
            using (var con = new SqlConnection("Data Source=db-mssql ;Initial Catalog=s16424; Integrated Security = True"))
            using (var com = new SqlCommand())
            { //imie, nazwisko, dataur, nazwa studiow, nr semestru
                com.Connection = con;
                com.CommandText = "select enrollment.idenrollment, semester, name, startdate from enrollment " +
                "INNER JOIN student ON enrollment.idenrollment = student.idenrollment " +
                " INNER JOIN studies ON enrollment.idstudy = studies.idstudy where indexnumber = @index;";
                var dr = com.ExecuteReader();
                com.Parameters.AddWithValue("index", indexNumber);
                con.Open();

                /* 
                SqlParameter par = new SqlParameter();
                par.Value = indexNumber;
                par.ParameterName = "index";
                com.Parameters.Add(par);
                */

                if (dr.Read())
                {   
                    var result = "ID wpisu: " + dr["idenrollment"].ToString() +
                        " , semestr: " + dr["semester"].ToString() + ", na studiach: " + dr["name"].ToString() +
                        ", data rozpoczecia: " + dr["startdate"].ToString();
                    return Ok(result);
                }
                return NotFound();
            }
        }



        //public IActionResult GetStudent(string orderBy)
        //{
        // return Ok(_dbService.GetStudents());
        // }

       // [HttpPost]
     //   public IActionResult CreateStudent(Student student)
      //  {
       //     student.IndexNumber = $"s{new Random().Next(1, 20000)}";
      //      return Ok(student);
     //   }

        // [HttpGet]
        // public string GetStudents(string  orderBy)
        // {
        //    return $"Kowalski, Malewski, Andrzejewski sortowanie={orderBy}";
        // }

        [HttpPut ("{id}")]

        public IActionResult PutStudent(int id)
        {
            if (id == 1 || id == 2)
            {
                Console.WriteLine("Aktualizacja dokonczona");
            var msg = " Aktualizacja ukonczona";
            return Ok(200 + msg);
            }
            else
            {
                return NotFound("Brak takiej osoby");
            }
        }

        [HttpDelete("{id}")]

        public IActionResult DeleteStudent(int id)
        {
            if (id == 1 || id == 2)
            {
                Console.WriteLine("Usuwanie dokonczone");
                var msg = " Usuwanie ukonczone";
                return Ok(200 + msg);
            } else
            {
                return NotFound("Brak takiej osoby");
            }
        }

    }
}