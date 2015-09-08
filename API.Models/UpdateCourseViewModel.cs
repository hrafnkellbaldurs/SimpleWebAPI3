using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public DateTime StartDate { get; set; }

        /// <summary>
        /// The new enddate
        /// Example: "2015-11-20T00:00:00"
        /// </summary>
        public DateTime EndDate { get; set; }
    }
}
