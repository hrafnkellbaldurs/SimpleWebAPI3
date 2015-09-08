using System.ComponentModel.DataAnnotations;
using System;

namespace API.Models
{
    /// <summary>
    /// This class represents the data sent by the client to update a course.
    /// </summary>
    public class UpdateCourseViewModel
    {
        /// <summary>
        /// The new startdate
        /// Example: "2015-08-20T00:00:00"
        /// </summary>
        [Required]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// The new enddate
        /// Example: "2015-11-20T00:00:00"
        /// </summary>
        [Required]
        public DateTime EndDate { get; set; }
    }
}
