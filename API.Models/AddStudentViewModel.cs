using System;
using System.Runtime.CompilerServices;
using Microsoft.Build.Framework;

namespace API.Models
{
    /// <summary>
    /// This class represents the data sent by the client to add a student to a course.
    /// </summary>
    public class AddStudentViewModel
    {
        /// <summary>
        /// The SSN of the student to add
        /// Example: "1234567890"
        /// </summary>
        [Required]
        public String  SSN { get; set; }
    }
}
