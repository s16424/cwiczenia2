using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class EnrollStudentRequest
    {
        [Required(ErrorMessage = "Podaj imie")]
        public string FirstName { get; set; }
        
        [Required(ErrorMessage = "Podaj nazwisko")]
        [MaxLength(255)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Podaj nr indeksu")]
        [RegularExpression("^s[0-9]+$")]
        public string IndexNumber { get; set; }

        [Required(ErrorMessage = "Podaj date urodzenia")]
        public string Birthdate { get; set; }
       
        [Required(ErrorMessage = "Podaj nr semestru")]
        public int Semester { get; set; }

        [Required(ErrorMessage = "Podaj nazwe studiow")]
        public string Studies { get; set; }

    }
}
