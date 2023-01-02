using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class InvalidPublisherInputException : Exception
    {
        public InvalidPublisherInputException(string message = "To Create a New Item, Must Enter a Publisher") : base(message)
        {
        }
    }
}
