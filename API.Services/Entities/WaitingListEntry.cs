using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Services.Entities
{
    /// <summary>
    /// 
    /// </summary>
    [Table("WaitingList")]
    class WaitingListEntry
    {
        /// <summary>
        /// StudentID of a student
        /// Exa
        /// </summary>
        public int StudentID { get; set; }

        public int CourseID { get; set; }

        public int Position { get; set; }
    }
}
