using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class InvalidPriceInputException : Exception
    {
        public InvalidPriceInputException(string message = "Invalid Price Input, Please Enter a Number") : base(message) { }
    }
}
