using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    [Serializable]

    public class Book : LibraryItem
    {
        // only book has author
        public string Author { get; set; }
        
        public Book(string name,string author, string publisher, Genre genre, double rentPrice) : base(name, publisher, genre, rentPrice)
        {
            Author = author;
            IsBook = true; // I use this later to determine which of the items is a book
        }
        public Book()
        {
            //empty ctor for serialize
        }
        public override string ToString()
        {
            return $"Name: {Name}\nAuthor: {Author}\nPublisher: {Publisher}\nGenre: {Genre}\nRent Price: {RentPrice}\nIs This Item Rented? {IsRented}";
        }
    }
}
