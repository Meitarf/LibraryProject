using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class ItemCollection
    {
        public List<LibraryItem> Items { get; set; }
        public ItemCollection()
        {
            Items = new List<LibraryItem>();
            Items.Add(new Journal("Vogue", "Condé Nast", Genre.Fashion, 10));
            Items.Add(new Book("Charlie and the Chocolate Factory ", "Roald Dahl", "George Allen & Unwin", Genre.Novel, 25));
            Items.Add(new Book("Harry Potter", "J. K. Rowling", "Bloomsbury Publishing ", Genre.Fantasy, 30));
            Items.Add(new Book("A Song of Ice and Fire", "George R. R. Martin", "Bantam Books", Genre.Fantasy, 32));
            Items.Add(new Book("Wool", "Hugh Howey", "Booktrack", Genre.ScienceFiction, 24));
            Items.Add(new Book("The Girl on the Train", "Paula Hawkins", "Keter-Books", Genre.Thriller, 28));
            Items.Add(new Book("The Notebook", "Nicholas Sparks", "Warner Books", Genre.Romance, 30) /*{ IsRented = true, RentDate = DateTime.Now-TimeSpan.FromDays(15) }*/); // I added this to check if the rent date message works
            Items.Add(new Journal("Time", "Time Warner", Genre.News, 15));
            Items.Add(new Book("Twilight", "Stephanie Meyer", "Little, Brown and Company", Genre.Romance, 32));
        }
        // search
        // search items by name\genre\publisher\author according to the filtered text
        // return the found items to a list
        public List<LibraryItem> FilterBy(string filterText, string filterBy)
        {
            List<LibraryItem> filteredList = new List<LibraryItem>();
            filterText = filterText.ToLower();

            foreach (LibraryItem item in Items)
            {
                switch (filterBy)
                {
                    case "Genre":
                        if (item.Genre.ToString().ToLower().Contains(filterText))
                            filteredList.Add(item);
                        break;
                    case "Name":
                        if (item.Name.ToLower().Contains(filterText))
                            filteredList.Add(item);
                        break;
                    case "Publisher":
                        if (item.Publisher.ToLower().Contains(filterText))
                            filteredList.Add(item);
                        break;
                    case "Author":
                        if (item.IsBook)
                        {
                            Book b = item as Book;
                            if (b.Author.ToLower().Contains(filterText))
                                filteredList.Add(b);
                        }
                        break;
                }
            }
            return filteredList;
        }
        // discount
        // adds discounts to chosen items by name\genre\publisher\author according to the filtered text
        // if the discount is larger than the rent price - throw an exception
        public void PriceDiscount(string filterText, string filterBy, double discount)
        {
            filterText = filterText.ToLower();
            foreach (LibraryItem item in Items)
            {
                switch (filterBy)
                {
                    case "Genre":
                        if (item.Genre.ToString().ToLower().Contains(filterText))
                            if (discount > item.RentPrice) throw new NegativePriceException();
                        item.RentPrice -= discount;
                        break;
                    case "Name":
                        if (item.Name.ToLower().Contains(filterText))
                            if (discount > item.RentPrice) throw new NegativePriceException();
                            item.RentPrice -= discount;
                        break;
                    case "Publisher":
                        if (item.Publisher.ToLower().Contains(filterText))
                            if (discount > item.RentPrice) throw new NegativePriceException();
                        item.RentPrice -= discount;
                        break;
                    case "Author":
                        if (item.IsBook)
                        {
                            Book b = item as Book;
                            if (b.Author.ToLower().Contains(filterText))
                                if (discount > b.RentPrice) throw new NegativePriceException();
                            b.RentPrice -= discount;
                        }
                        break;
                }
            }
        }
        public string RentReport()
        {
            int numBooks = 0;
            int numRented = 0;
            // count how many books
            foreach (LibraryItem item in Items)
            {
                if (item.IsBook) numBooks++;
            }
            // count how many rented
            foreach (LibraryItem item in Items)
            {
                if (item.IsRented) numRented++;
            }
            // amount of journals is total items substract books amount
            int numJournals = Items.Count - numBooks;
            return $"Books and Journals in Library: {Items.Count}\nTotal Books Amount: {numBooks}\nTotal Journals Amount: {numJournals}\nTotal Rented Amount: {numRented}";
        }
    }
}
