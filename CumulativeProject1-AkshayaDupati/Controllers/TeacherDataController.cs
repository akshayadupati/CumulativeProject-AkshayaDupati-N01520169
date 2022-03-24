using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MySql.Data.MySqlClient;
using CumulativeProject1_AkshayaDupati.Models;
using System.Diagnostics;

namespace CumulativeProject1_AkshayaDupati.Controllers
{
    public class TeacherDataController : ApiController
    {
        private SchoolDbContext School = new SchoolDbContext();

        /// <summary>
        /// The ListTeachers function returns list of teachers
        /// </summary>
        /// <param name="SearchKey">Search key that defines teacher name . Optional parameter.</param>
        /// <example>GET : /api/TeacherData/ListTeachers</example>
        /// <example>GET : /api/TeacherData/ListTeachers/Akshaya</example>
        /// <returns>
        /// A list of teachers with teacherid, teacherfname and teacherlname
        /// </returns>

        [HttpGet]
        [Route("api/TeacherData/ListTeachers/{SearchKey?}")]
        public List<Teacher> ListTeachers(string SearchKey = null)
        {

            if (SearchKey != null)
            {
                Debug.WriteLine("Hey there! Search key is present! ");
            }

            MySqlConnection Conn = School.AccessDatabase();

            Conn.Open();

            MySqlCommand cmd = Conn.CreateCommand();

            string query = "SELECT * FROM TEACHERS";

            if(SearchKey != null)
            {
                query = query + " where lower(teacherfname) = lower(@key)";
                cmd.Parameters.AddWithValue("@key", SearchKey);
                cmd.Prepare();
            }

            cmd.CommandText = query;

            MySqlDataReader ResultSet = cmd.ExecuteReader();

            List<Teacher> Teachers = new List<Teacher> { };

            while (ResultSet.Read())
            {

                Teacher NewTeacher = new Teacher();

                NewTeacher.TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                NewTeacher.TeacherFName = ResultSet["teacherfname"].ToString();
                NewTeacher.TeacherLName = ResultSet["teacherlname"].ToString();

                Teachers.Add(NewTeacher);
            }

            Conn.Close();

            return Teachers;
        }

        /// <summary>
        /// The FindTeacher function returns data of that particular teacher id.
        /// </summary>
        /// <example>GET : api/TeacherData/FindTeacher/{3}</example>
        /// <returns>
        /// Returns teacherid, teacherfname and teacherlname of 3rd teacher in the database.
        /// </returns>

        [HttpGet]
        [Route("api/TeacherData/FindTeacher/{teacherId}")]

        public Teacher FindTeacher(int teacherId)
        {

            MySqlConnection Conn = School.AccessDatabase();

            Conn.Open();

            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = "select * from teachers where teacherid=@id";

            cmd.Parameters.AddWithValue("@id", teacherId);

            cmd.Prepare();

            MySqlDataReader ResultSet = cmd.ExecuteReader();

            Teacher selectedTeacher = new Teacher();

            while (ResultSet.Read())
            {
                selectedTeacher.TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                selectedTeacher.TeacherFName = ResultSet["teacherfname"].ToString();
                selectedTeacher.TeacherLName = ResultSet["teacherlname"].ToString();
            }

            Conn.Close();

            return selectedTeacher;
        }
    }
}
