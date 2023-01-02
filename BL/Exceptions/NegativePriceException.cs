using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class NegativePriceException : Exception
    {
        public NegativePriceException(string message = "Rent Price Cannot be a Negative Number") : base(message)
        {
        }
    }
}
