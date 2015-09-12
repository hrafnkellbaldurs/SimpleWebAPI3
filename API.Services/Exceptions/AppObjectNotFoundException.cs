using System;

namespace API.Services.Exceptions
{
    /// <summary>
    /// An instance of this class will be thrown if an object
    /// (such as a course, or a student in a course) cannot be found.
    /// </summary>
    public class AppObjectNotFoundException : ApplicationException
    {
    }
}
