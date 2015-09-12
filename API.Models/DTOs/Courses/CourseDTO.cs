using System;

namespace API.Models.DTOs.Courses
{
    /// <summary>
    /// This class represents an item in a list of courses visible to the client
    /// </summary>
    public class CourseDTO
    {
        /// <summary>
        /// Database-generated unique identifier of the course
        /// Example: 17
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// The name of the course
        /// Example: "Vefþjónustur"
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Startdate of the course
        /// Example: "2015-08-20T00:00:00"
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Enddate of the course
        /// Example: "2015-11-20T00:00:00"
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// The semester that a course is tought. 
        /// Example: "20151" -> Spring 2015, 
        ///          "20152" -> Summer 2015, 
        ///          "20153" -> Fall 2015
        /// </summary>
        public string Semester { get; set; }

        /// <summary>
        /// The number of active students in the course.
        /// </summary>
        public int StudentCount  { get; set; }
    }
}
