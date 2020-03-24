using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly DAL.IDbService _dbService;

        public StudentsController(DAL.IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]
        public IActionResult GetStudent(string orderBy)
        {
            return Ok(_dbService.GetStudents());
        }

        [HttpPost]
        public IActionResult CreateStudent(Models.Student student)
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