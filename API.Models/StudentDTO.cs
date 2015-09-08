using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Models
{
    /// <summary>
    /// This class represents a student visible to the client
    /// </summary>
    public class StudentDTO
    {
        /// <summary>
        /// The name of the student
        /// Example: "Gunnar Gunnarsson"
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// The Social Security Number of a student
        /// Example: "1234567890"
        /// </summary>
        public String SSN { get; set; }
    }
}
