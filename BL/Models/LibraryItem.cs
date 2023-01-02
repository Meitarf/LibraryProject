using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BL
{

    [XmlInclude(typeof(Book))]
    [XmlInclude(typeof(Journal))]
    [Serializable]
    public class LibraryItem
    {
        private string _name;
        private string _publisher;
        private int _isbn;
        private Genre _genre;
        private double _rentPrice;
        private DateTime _rentDate;
        private bool _isRented;
        private bool _isBook;
        // ctor
        public LibraryItem(string name, string publisher, Genre genre, double rentPrice = 0)
        {
            _name = name;
            _publisher = publisher;
            _genre = genre;
            _rentPrice = rentPrice;
            _isRented = false;
            _isbn++;
            _isBook = false;
        }
        public LibraryItem()
        {
            //empty ctor for serialize
        }
        public override string ToString()
        {
            return $"Name: {Name}\nPublisher: {Publisher}\nGenre: {Genre}\nRent Price: {RentPrice}\nIs This Item Rented? {IsRented}";
        }
        public string Name { get => _name; set => _name = value; }
        public string Publisher { get => _publisher; set => _publisher = value; }
        public int ISBN { get => _isbn; set => _isbn = value; }
        public Genre Genre { get => _genre; set => _genre = value; }
        public double RentPrice { get => _rentPrice; set => _rentPrice = value; }
        public DateTime RentDate { get => _rentDate; set => _rentDate = value; }
        public bool IsRented { get => _isRented; set => _isRented = value; }
        public bool IsBook { get => _isBook; set => _isBook = value; }
    }
}
