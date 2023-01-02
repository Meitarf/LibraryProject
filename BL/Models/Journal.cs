using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    [Serializable]
    public class Journal : LibraryItem
    {
        
        // inherits from Library Item
        public Journal(string name, string publisher, Genre genre, double rentPrice) : base(name, publisher, genre, rentPrice)
        {
        }
        public Journal()
        {
            //empty ctor for serialize
        }
    }
}
