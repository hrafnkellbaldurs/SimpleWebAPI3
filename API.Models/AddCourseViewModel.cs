using System;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    /// <summary>
    /// This class represents the data necessary to create a single 
    /// course taught in a given semester.
    /// </summary>
    public class AddCourseViewModel
    {
        /// <summary>
        /// The ID of the course being created
        /// Example: "T-514-VEFT"
        /// </summary>
        [Required]
        public string TemplateID { get; set; }

        /// <summary>
        /// Start date of the course being ceated
        /// Example: "2015-08-20T00:00:00"
        /// </summary>
        [Required]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// End date of the course being created
        /// Example: "2015-11-20T00:00:00"
        /// </summary>
        [Required]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// The semester in which the course is taught
        /// Example: "20153"
        /// </summary>
        [Required]
        public string Semester { get; set; }

        /// <summary>
        /// The max number of students allowed in a course
        /// Example: 20
        /// </summary>
        [Required]
        public int MaxStudents { get; set; }
    }
}
