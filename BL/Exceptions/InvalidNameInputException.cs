using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class InvalidNameInputException : Exception
    {
        public InvalidNameInputException(string message="To Create a New Item, Must Enter a Name") : base(message)
        {
        }
    }
}
