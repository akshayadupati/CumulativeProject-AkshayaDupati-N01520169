using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MySql.Data.MySqlClient;
using CumulativeProject1_AkshayaDupati.Models;

namespace CumulativeProject1_AkshayaDupati.Controllers
{
    public class TeacherDataController : ApiController
    {
        private SchoolDbContext School = new SchoolDbContext();

        [HttpGet]
        public IEnumerable<string> ListTeachers()
        {
            MySqlConnection Conn = School.AccessDatabase();

            Conn.Open();

            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = "SELECT * FROM TEACHERS";

            MySqlDataReader ResultSet = cmd.ExecuteReader();

            List<string> TeacherNames = new List<string> { };

            while (ResultSet.Read())
            {
                string TeacherName = ResultSet["teacherfname"] + " " + ResultSet["teacherlname"];

                TeacherNames.Add(TeacherName);
            }

            Conn.Close();

            return TeacherNames;
        }
    }
}
