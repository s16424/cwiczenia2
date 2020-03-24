using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.DAL
{
    public class MockDbService : IDbService
    {
        private static IEnumerable<Models.Student> _students;
        static MockDbService()
        {
            _students = new List<Models.Student>
            {
                new Models.Student{IdStudent = 1, FirstName = "Jan", LastName = "Kowalski"},
                new Models.Student{IdStudent = 2, FirstName = "Anna", LastName = "Malewski"},
                new Models.Student{IdStudent = 3, FirstName = "Andrzej", LastName = "Andrzejewicz"}
            };
            }
        public IEnumerable<Models.Student> GetStudents()
        {
            return _students;
        }
        }
    }

