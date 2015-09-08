using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Models
{
    /// <summary>
    /// This class represents the data necessary to create a single 
    /// course taught in a given semester.
    /// </summary>
    public class CourseViewModel
    {
        /// <summary>
        /// The ID of the course being created
        /// Example: "T-514-VEFT"
        /// </summary>
        public String CourseID { get; set; }

        /// <summary>
        /// The semester in which the course is taught
        /// Example: "20153"
        /// </summary>
        public String Semester { get; set; }

        /// <summary>
        /// Start date of the course being ceated
        /// Example: "2015-08-20T00:00:00"
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// End date of the course being created
        /// Example: "2015-11-20T00:00:00"
        /// </summary>
        public DateTime EndDate { get; set; }
    }
}
