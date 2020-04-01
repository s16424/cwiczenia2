using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using WebApplication1.Models;

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
    //
        [HttpGet]

        public IActionResult GetStudents()
        {
            var list = new List<Student>();
            using (var con = new SqlConnection("Data Source=db-mssql ;Initial Catalog=s16424; Integrated Security = True"))
            using (var com = new SqlCommand())
            { //imie, nazwisko, dataur, nazwa studiow, nr semestru
                com.Connection = con;
                com.CommandText = "SELECT student.firstname, student.lastname, student.birthdate, Name, semester " +
                    "FROM student INNER JOIN enrollment ON student.IdEnrollment = Enrollment.IdEnrollment " +
                    " INNER JOIN studies ON Enrollment.IdStudy = Studies.IdStudy";
                con.Open();
                var dr = com.ExecuteReader();
                while (dr.Read())
                {
                    var st = new Student();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    st.DataUrodzenia = dr["BirthDate"].ToString();
                    st.NazwaStudiow = dr["name"].ToString();
                    st.NrSemestru = int.Parse(dr["semester"].ToString());
                    list.Add(st);
                }
                return Ok(list);
            }
        }
        //public IActionResult GetStudent(string orderBy)
        //{
           // return Ok(_dbService.GetStudents());
       // }

        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            student.IndexNumber = $"s{new Random().Next(1, 20000)}";
            return Ok(student);
        }

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