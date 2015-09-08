using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Services.Exceptions
{
    /// <summary>
    /// An instance of this class will be thrown if an object being added
    /// (such as a class or a student in a class) already exists
    /// </summary>
    public class AppConflictException : ApplicationException
    {
    }
}
