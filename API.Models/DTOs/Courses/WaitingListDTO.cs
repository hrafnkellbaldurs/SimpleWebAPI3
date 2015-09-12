
namespace API.Models.DTOs.Courses
{
    /// <summary>
    /// This class represents a waitinglist in a course to the client
    /// </summary>
    public class WaitingListDTO
    {
        /// <summary>
        /// SSN of a student on a waitinglist
        /// </summary>
        public string SSN { get; set; }

        /// <summary>
        /// Name of a student of a waitinglist
        /// </summary>
        public string Name { get; set; }
    }
}
