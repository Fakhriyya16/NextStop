using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Helpers.Exceptions
{
    public class InvalidImageFormatException : Exception
    {
        public InvalidImageFormatException() : base("The file is not a valid image format.")
        {
        }

        public InvalidImageFormatException(string message) : base(message)
        {
        }
    }
}
