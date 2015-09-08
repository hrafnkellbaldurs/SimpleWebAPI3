using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Services.Exceptions
{
    /// <summary>
    /// An instance of this class is thrown if information
    /// inside the server is inconsistent 
    /// </summary>
    public class AppServerErrorException : ApplicationException
    {
    }
}
