using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class SqlServerStudentDbService : iStudentDbService
    {
        public void EnrollStudent(EnrollStudentRequest request)
        {

            var st = new Student();
            st.FirstName = request.FirstName;

            using (var con = new SqlConnection(""))
            using (var com = new SqlCommand())
            {
                com.Connection = con;

                con.Open();
                var tran = con.BeginTransaction();
 
                    com.CommandText = "SELECT IdStudies from studies where name = @name";
                    com.Parameters.AddWithValue("name", request.Studies);

                    var dr = com.ExecuteReader();

                    if (!dr.Read())
                    {
                        tran.Rollback();
                       // return BadRequest("Studia nie istnieja");
                    }

                    int idstudies = (int)dr["IdStudies"];
                    com.CommandText = "select * from enrollment inner join studies on enrollment.IdStudy" +
                            " = Studies.IdStudy WHERE semester = 1 and enrollment.idstudy = 1 AND startdate =" +
                            " (select max(startdate) from enrollment)";
                    dr = com.ExecuteReader();
                    if (!dr.Read())
                    {
                        com.CommandText = "INSERT INTO enrollment VALUES( (SELECT max(IdEnrollment) from enrollment) +1, 1, @idstudies, getdate())";
                        com.Parameters.AddWithValue("idstudies", idstudies);
                        com.ExecuteNonQuery();
                        tran.Commit();
                    }
                    com.CommandText = "SELECT * from enrollment where idenrollment = (select MAX(idenrollment) from enrollment)";
                    dr = com.ExecuteReader();
                    int idenrollment = (int)dr["IdEnrollment"];
                    com.CommandText = "INSERT INTO Student VALUES @idenr, ";

                    com.CommandText = "INSERT INTO student VALUES(@Index, @Fname, @Lname, @Bday, @idenr)";
                    com.Parameters.AddWithValue("Fname", request.FirstName);
                    com.Parameters.AddWithValue("Index", request.IndexNumber);
                    com.Parameters.AddWithValue("Lname", request.LastName);
                    com.Parameters.AddWithValue("Bday", request.Birthdate);
                    com.Parameters.AddWithValue("idenr", idenrollment);
                    com.ExecuteNonQuery();
                    tran.Commit();
                }
            }

        public Student GetStud(string index)
        {
            using (var con = new SqlConnection("Data Source=db-mssql ;Initial Catalog=s16424; Integrated Security = True"))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "select * from student where indexnumber = @index;";
                com.Parameters.AddWithValue("index", index);
                     con.Open();
                    var dr = com.ExecuteReader();
                if (!dr.Read())
                {
                    return null;
                }
                else{ 
                    var res = new Student();
                    res.IndexNumber = dr["IndexNumber"].ToString();
                    //res.FirstName = dr["FirstName"].ToString();
                    //res.LastName = dr["LastName"].ToString();
                   // res.Birthdate = (DateTime)dr["Birthdate"];

                    return res; 
                } 
            }
        }
        public void PromoteStudents(int semester, string studies)
        {
            throw new NotImplementedException();
        }
    }
}
