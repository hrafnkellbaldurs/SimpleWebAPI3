using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Models
{
    /// <summary>
    /// This class represents a single course, and contains various
    /// details about the course that are visible to the client.
    /// </summary>
    public class CourseDetailsDTO
    {
        /// <summary>
        /// The ID of the course
        /// Example: 1
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// The name of the course
        /// Example: "Vefþjónustur
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// List of students in the course
        /// </summary>
        public List<StudentDTO>  Students { get; set; }

        /// <summary>
        /// The start date of the course
        /// Example: "2015-08-20T00:00:00"
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// The end date of the course
        /// /// Example: "2015-11-20T00:00:00"
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// The semester that a course is tought. 
        /// Example: "20151" -> Spring 2015, 
        ///          "20152" -> Summer 2015, 
        ///          "20153" -> Fall 2015
        /// </summary>
        public string Semester { get; set; }


    }
}
