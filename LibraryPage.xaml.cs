using BL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Serialization;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace LibraryProject
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LibraryPage : Page
    {
        BL.ItemCollection lm = new BL.ItemCollection();
        User user;
        string filePath = $@"{Windows.Storage.ApplicationData.Current.LocalFolder.Path}\ItemsList.xml";
        // at first I would like to apologize for the horrible buttons and TextBoxes names
        // I realized that after it was too late.. :(
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            user = (User)e.Parameter;
            // if the user is a customer (not a librarian)
            // he can't edit, can't add new item, can't remove an item, can't create discount
            // can only rent, search an item and view details of an iten
            if (user.IsCust)
            {
                editbtn.IsEnabled = false;
                addbtn.IsEnabled = false;
                discountbtn.IsEnabled = false;
                rentreportbtn.IsEnabled = false;
                savebtn.IsEnabled = false;
                removebtn.IsEnabled = false;
            }
            // if user is a librarian - can do everything (rule the world)
            else
            {
                editbtn.IsEnabled = true;
                addbtn.IsEnabled = true;
                discountbtn.IsEnabled = true;
                rentreportbtn.IsEnabled = true;
                savebtn.IsEnabled = true;
                removebtn.IsEnabled = true;
            }
        }
        // some of these buttens will appear only when they are needed - first I initialize them as Visibility.Collapsed
        // i.e "Save Edit" will only appear after clicking "Edit Item"
        // when "Save Edit" is clicked (and all the details are valid) - "Edit Item" btn will appear again
        // this goes through all the project with other buttens as well (adding, renting, etc)
        public LibraryPage()
        {
            this.InitializeComponent();
            InitBookListView();
            searchCombobox.Visibility = Visibility.Collapsed;
            saveeditbtn.Visibility = Visibility.Collapsed;
            saveaddbtn.Visibility = Visibility.Collapsed;
            searchtBox.Visibility = Visibility.Collapsed;
            dosearchbtn.Visibility = Visibility.Collapsed;
            returntbtn.Visibility = Visibility.Collapsed;
            reporttBox.Visibility = Visibility.Collapsed;
            adddiscounthbtn.Visibility = Visibility.Collapsed;
            discountBox.Visibility = Visibility.Collapsed;
            hidesearchlstbtn.Visibility = Visibility.Collapsed;
            hidereportbtn.Visibility = Visibility.Collapsed;
            cancelsearchbtn.Visibility = Visibility.Collapsed;
            canceldiscountbtn.Visibility = Visibility.Collapsed;
        }
        // allows textbox to be editable
        // allows genre combobox to be enabled
        private void BookViewEdit()
        {
            itemName.IsReadOnly = false;
            itemAuthor.IsReadOnly = false;
            itemPublisher.IsReadOnly = false;
            itemGenre.IsEnabled = true;
            itemPrice.IsReadOnly = false;
        }
        // clears the textboxes for adding a new item and after removing an item
        private void BookViewClear()
        {
            itemName.Text = "";
            itemAuthor.Text = "";
            itemPublisher.Text = "";
            itemPrice.Text = "";
        }
        // initializes booklistview and sets textboxes to be uneditable
        // Genre will be displayed by a comboBox
        private void InitBookListView()
        {
            BooksListView.Items.Clear();
            foreach (LibraryItem item in lm.Items)
            {
                BooksListView.Items.Add(item.Name);
            }
            var _enumval = Enum.GetValues(typeof(Genre)).Cast<Genre>();
            itemGenre.ItemsSource = _enumval.ToList();
            itemGenre.SelectedIndex = 2;
            itemGenre.IsEnabled = false;
            itemGenre.IsEditable = false;
            itemName.IsReadOnly = true;
            itemAuthor.IsReadOnly = true;
            itemPublisher.IsReadOnly = true;
            itemPrice.IsReadOnly = true;
            itemIsRented.IsReadOnly = true;
        }
        private void BooksListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Selection Changed on the Books List View displays the chosen item
            if (BooksListView.SelectedIndex > -1)
            {
                itemName.Text = lm.Items[BooksListView.SelectedIndex].Name;
                // if the selected item is a book - display the author
                if (lm.Items[BooksListView.SelectedIndex].IsBook)
                {
                    Book b = lm.Items[BooksListView.SelectedIndex] as Book;
                    itemAuthor.Text = b.Author;
                }
                else { itemAuthor.Text = ""; }
                itemPublisher.Text = lm.Items[BooksListView.SelectedIndex].Publisher;
                // parsing enum genre to string and displaying it on the combobox
                string genreText = lm.Items[BooksListView.SelectedIndex].Genre.ToString();
                Genre myGenre = (Genre)Enum.Parse(typeof(Genre), genreText);
                itemGenre.SelectedItem = myGenre;
                itemPrice.Text = lm.Items[BooksListView.SelectedIndex].RentPrice.ToString();
                itemIsRented.Text = lm.Items[BooksListView.SelectedIndex].IsRented.ToString();
                // if the item is rented
                if (lm.Items[BooksListView.SelectedIndex].IsRented)
                {
                    // if it has passed two weeks since the rent date - display it the rent text box
                    if (DateTime.Now - lm.Items[BooksListView.SelectedIndex].RentDate > TimeSpan.FromDays(14))
                    {
                        itemIsRented.Text = lm.Items[BooksListView.SelectedIndex].IsRented.ToString() + " - Rent Date Has Passed Two Weeks";
                    }
                    // if rented, allow book to be returned
                    rentbtn.Visibility = Visibility.Collapsed;
                    returntbtn.Visibility = Visibility.Visible;
                }
                // if item isn't rented
                else
                {
                    rentbtn.Visibility = Visibility.Visible;
                    returntbtn.Visibility = Visibility.Collapsed;
                }
            }
        }
        // enables editing an item
        private void EditItem_Click(object sender, RoutedEventArgs e)
        {
            if (BooksListView.SelectedIndex > -1)
            {
                BookViewEdit();
                saveeditbtn.Visibility = Visibility.Visible;
                editbtn.Visibility = Visibility.Collapsed;
                // I dont want to enable anything else while editing an item because the user is stupid
                savebtn.IsEnabled = false;
                loadbtn.IsEnabled = false;
                rentbtn.IsEnabled = false;
                returntbtn.IsEnabled = false;
                addbtn.IsEnabled = false;
                backbtn.IsEnabled = false;
                removebtn.IsEnabled = false;
            }
        }
        //clear text boxes and make text boxes editable
        //IsRented isn't editable because if you just added the book - obviously it isn't rented (after save can be rented)
        private void AddBtnClick(object sender, RoutedEventArgs e)
        {
            BookViewClear();
            BookViewEdit();
            itemIsRented.IsReadOnly = true;
            saveaddbtn.Visibility = Visibility.Visible;
            addbtn.Visibility = Visibility.Collapsed;
            itemAuthor.PlaceholderText = "If Added Item Is a Book, Please Enter the Author";
            itemIsRented.PlaceholderText = "False";
            // I dont want to enable anything else while adding a new item
            savebtn.IsEnabled = false;
            loadbtn.IsEnabled = false;
            rentbtn.IsEnabled = false;
            returntbtn.IsEnabled = false;
            editbtn.IsEnabled = false;
            backbtn.IsEnabled = false;
            removebtn.IsEnabled = false;
        }
        // saves the edit changes to the item's properties
        private void SaveEditBtnClick(object sender, RoutedEventArgs e)
        {
            // the reason I use "try" for all this code is because this is the only way it works the way I want it to work
            // I don't want to do the rest of the code unless there is no exception in this method
            try
            {
                if (BooksListView.SelectedIndex > -1)
                {
                    // try to save the users input as price 
                    double price;
                    bool canParse = double.TryParse(itemPrice.Text, out price);
                    // if price input is not a number, throw an exception
                    if (!canParse) throw new InvalidPriceInputException();
                    // if price input is a negative number, throw an exception
                    if (price < 0) throw new NegativePriceException();
                    lm.Items[BooksListView.SelectedIndex].RentPrice = price;
                    lm.Items[BooksListView.SelectedIndex].Genre = (Genre)itemGenre.SelectedValue;
                    lm.Items[BooksListView.SelectedIndex].Name = itemName.Text;
                    // if the item is a book, save the changes to the author property
                    if (lm.Items[BooksListView.SelectedIndex].IsBook)
                    {
                        Book b = lm.Items[BooksListView.SelectedIndex] as Book;
                        b.Author = itemAuthor.Text;
                    }
                }
                InitBookListView(); // refresh book list
                editbtn.Visibility = Visibility.Visible;
                saveeditbtn.Visibility = Visibility.Collapsed;
                savebtn.IsEnabled = true;
                loadbtn.IsEnabled = true;
                rentbtn.IsEnabled = true;
                returntbtn.IsEnabled = true;
                addbtn.IsEnabled = true;
                backbtn.IsEnabled = true;
                removebtn.IsEnabled = true;
            }
            //catch exceptions
            catch (InvalidPriceInputException priceE)
            {
                itemPrice.Text = "";
                itemPrice.PlaceholderText = priceE.Message;
            }
            catch (NegativePriceException negE)
            {
                itemPrice.Text = "";
                itemPrice.PlaceholderText = negE.Message;
            }
        }
        // add the new item to the list
        private void SaveAddedItemBtnClick(object sender, RoutedEventArgs e)
        {
            // the reason I use "try" for all this code is because this is the only way it works the way I want it to work
            // I don't want to do the rest of the code unless there is no exception in this method
            try
            {
                LibraryItem item;
                // if name input wasn't inserted - throw an exception
                if (string.IsNullOrWhiteSpace(itemName.Text)) throw new InvalidNameInputException();
                // if publisher input wasn't inserted - throw an exception
                if (string.IsNullOrWhiteSpace(itemPublisher.Text)) throw new InvalidPublisherInputException();
                double price;
                bool canParse = double.TryParse(itemPrice.Text, out price);
                // if price input wasn't inserted - throw an exception
                if (string.IsNullOrWhiteSpace(itemPrice.Text)) throw new InvalidPriceInputException("To Create a New Item, Must Enter a Price");
                // if price input is not a number, throw an exception
                if (!canParse) throw new InvalidPriceInputException();
                // if price input is a negative number, throw an exception
                if (price < 0) throw new NegativePriceException();
                // if author input was inserted - add the item as a book
                if (!string.IsNullOrWhiteSpace(itemAuthor.Text))
                    item = new Book(itemName.Text, itemAuthor.Text, itemPublisher.Text, (Genre)Enum.Parse(typeof(Genre), itemGenre.SelectedItem.ToString()), price);
                else
                {
                    // if not - add the item as a journal
                    item = new Journal(itemName.Text, itemPublisher.Text, (Genre)Enum.Parse(typeof(Genre), itemGenre.SelectedItem.ToString()), price);
                    itemAuthor.PlaceholderText = "";
                }
                // add the item
                lm.Items.Add(item);
                InitBookListView(); // refresh book list
                addbtn.Visibility = Visibility.Visible;
                saveaddbtn.Visibility = Visibility.Collapsed;
                savebtn.IsEnabled = true;
                loadbtn.IsEnabled = true;
                rentbtn.IsEnabled = true;
                returntbtn.IsEnabled = true;
                editbtn.IsEnabled = true;
                backbtn.IsEnabled = true;
                removebtn.IsEnabled = true;
            }
            // catch all exceptoins
            catch (InvalidNameInputException nameE)
            {
                itemName.PlaceholderText = nameE.Message;
            }
            catch (InvalidPublisherInputException pubE)
            {
                itemPublisher.PlaceholderText = pubE.Message;
            }
            catch (InvalidPriceInputException priceE)
            {
                itemPrice.Text = "";
                itemPrice.PlaceholderText = priceE.Message;
            }
            catch (NegativePriceException negE)
            {
                itemPrice.Text = "";
                itemPrice.PlaceholderText = negE.Message;
            }
        }
        // removes the selected item from the Books List and refreshes list
        private void RemoveItemBtnClick(object sender, RoutedEventArgs e)
        {
            if (BooksListView.SelectedIndex > -1)
            {
                lm.Items.RemoveAt(BooksListView.SelectedIndex);
                InitBookListView();
                BookViewClear();
                itemIsRented.Text = "";
            }
        }
        // to search an item - show the needed fields
        private void SearchBtnClick(object sender, RoutedEventArgs e)
        {
            searchCombobox.Visibility = Visibility.Visible;
            searchCombobox.SelectedIndex = 0;
            searchtBox.Visibility = Visibility.Visible;
            searchtBox.PlaceholderText = "Search";
            searchtBox.Text = "";
            dosearchbtn.Visibility = Visibility.Visible;
            discountbtn.Visibility = Visibility.Collapsed;
            discountBox.Visibility = Visibility.Collapsed;
            adddiscounthbtn.Visibility = Visibility.Collapsed;
            cancelsearchbtn.Visibility = Visibility.Visible;
            searchbtn.Visibility = Visibility.Collapsed;
        }
        // search according to the given parameters
        private void DoSearchBtnClick(object sender, RoutedEventArgs e)
        {
            SearchListView.Visibility = Visibility.Visible;
            // if nothing was searched - nothing was found
            // return an empty list
            if (string.IsNullOrWhiteSpace(searchtBox.Text))
            {
                SearchListView.ItemsSource = new List<LibraryItem>();
                hidesearchlstbtn.Visibility = Visibility.Collapsed;
            }
            // search according to the given parameters
            else
                SearchListView.ItemsSource = lm.FilterBy(searchtBox.Text, searchCombobox.SelectedItem.ToString());
            searchtBox.Visibility = Visibility.Collapsed;
            dosearchbtn.Visibility = Visibility.Collapsed;
            searchbtn.Visibility = Visibility.Visible;
            searchCombobox.Visibility = Visibility.Collapsed;
            hidesearchlstbtn.Visibility = Visibility.Visible;
            discountbtn.Visibility = Visibility.Visible;
            cancelsearchbtn.Visibility = Visibility.Collapsed;
            canceldiscountbtn.Visibility = Visibility.Collapsed;
        }
        // rent item  - sets the IsRented field to true
        // and sets the rent date to now
        private void RentItemBtnClick(object sender, RoutedEventArgs e)
        {
            if (BooksListView.SelectedIndex > -1)
            {
                lm.Items[BooksListView.SelectedIndex].IsRented = true;
                lm.Items[BooksListView.SelectedIndex].RentDate = DateTime.Now;
                InitBookListView();
            }
        }
        // return item  - sets the IsRented field to false
        // and sets the rent date to default
        private void ReturnItemBtnClick(object sender, RoutedEventArgs e)
        {
            if (BooksListView.SelectedIndex > -1)
            {
                lm.Items[BooksListView.SelectedIndex].IsRented = false;
                lm.Items[BooksListView.SelectedIndex].RentDate = default(DateTime);
                InitBookListView();
                returntbtn.Visibility = Visibility.Collapsed;
                rentbtn.Visibility = Visibility.Visible;
            }
        }
        // show the rent report
        private void ShowRentReportBtnClick(object sender, RoutedEventArgs e)
        {
            reporttBox.Text = "";
            reporttBox.Visibility = Visibility.Visible;
            reporttBox.Text = lm.RentReport();
            rentreportbtn.Visibility = Visibility.Collapsed;
            hidereportbtn.Visibility = Visibility.Visible;
        }
        // to create a discount - show the needed fields
        private void CreateDiscountBtnClick(object sender, RoutedEventArgs e)
        {
            searchCombobox.Visibility = Visibility.Visible;
            searchCombobox.SelectedIndex = 0;
            searchtBox.Visibility = Visibility.Visible;
            searchtBox.PlaceholderText = "Add Discount To";
            searchtBox.Text = "";
            discountBox.Text = "";
            adddiscounthbtn.Visibility = Visibility.Visible;
            discountBox.Visibility = Visibility.Visible;
            searchbtn.Visibility = Visibility.Collapsed;
            dosearchbtn.Visibility = Visibility.Collapsed;
            discountbtn.Visibility = Visibility.Collapsed;
            canceldiscountbtn.Visibility = Visibility.Visible;
        }
        // adds discounts to chosen items by name\genre\publisher\author
        private void ApplyDiscountBtnClick(object sender, RoutedEventArgs e)
        {
            // the reason I use "try" for all this code is because this is the only way it works the way I want it to work
            // I don't want to do the rest of the code unless there is no exception in this method
            try
            {
                double price;
                bool canParse = double.TryParse(discountBox.Text, out price);
                // if the input isn't a number
                if (!canParse) throw new InvalidPriceInputException();
                lm.PriceDiscount(searchtBox.Text, searchCombobox.SelectedItem.ToString(), price);
                searchtBox.Visibility = Visibility.Collapsed;
                adddiscounthbtn.Visibility = Visibility.Collapsed;
                searchCombobox.Visibility = Visibility.Collapsed;
                discountBox.Visibility = Visibility.Collapsed;
                discountbtn.Visibility = Visibility.Visible;
                searchbtn.Visibility = Visibility.Visible;
                cancelsearchbtn.Visibility = Visibility.Collapsed;
                canceldiscountbtn.Visibility = Visibility.Collapsed;
                exceptionTbox.Text = "";
                InitBookListView();
            }
            // catch exceptions
            catch (InvalidPriceInputException priceE)
            {
                discountBox.Text = "";
                exceptionTbox.Text = priceE.Message;
            }
            catch (NegativePriceException negE)
            {
                discountBox.Text = "";
                exceptionTbox.Text = negE.Message;
            }
        }
        //hides the report
        private void HideRentReportBtnClick(object sender, RoutedEventArgs e)
        {
            reporttBox.Text = "";
            hidereportbtn.Visibility = Visibility.Collapsed;
            rentreportbtn.Visibility = Visibility.Visible;
        }
        // hides the list of found items in search
        private void HideSearchListBtnClick(object sender, RoutedEventArgs e)
        {
            SearchListView.Visibility = Visibility.Collapsed;
            hidesearchlstbtn.Visibility = Visibility.Collapsed;
        }
        // cancel search - if you clicked on search button and don't want to search
        private void CancelSearchBtnClick(object sender, RoutedEventArgs e)
        {
            searchCombobox.Visibility = Visibility.Collapsed;
            searchtBox.Visibility = Visibility.Collapsed;
            dosearchbtn.Visibility = Visibility.Collapsed;
            discountbtn.Visibility = Visibility.Visible;
            discountBox.Visibility = Visibility.Collapsed;
            adddiscounthbtn.Visibility = Visibility.Collapsed;
            cancelsearchbtn.Visibility = Visibility.Collapsed;
            searchbtn.Visibility = Visibility.Visible;
        }
        // cancel discount - if you clicked on create discount and don't want to add one
        private void CancelDiscountBtnClick(object sender, RoutedEventArgs e)
        {
            searchCombobox.Visibility = Visibility.Collapsed;
            searchtBox.Visibility = Visibility.Collapsed;
            adddiscounthbtn.Visibility = Visibility.Collapsed;
            discountBox.Visibility = Visibility.Collapsed;
            searchbtn.Visibility = Visibility.Visible;
            dosearchbtn.Visibility = Visibility.Collapsed;
            discountbtn.Visibility = Visibility.Visible;
            canceldiscountbtn.Visibility = Visibility.Collapsed;
            exceptionTbox.Text = "";
        }
        // loads the saved list
        private void LoadBtnClick(object sender, RoutedEventArgs e)
        {
            if (File.Exists(filePath))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<LibraryItem>));
                FileStream stream = new FileStream(filePath, FileMode.Open);
                lm.Items = (List<LibraryItem>)xmlSerializer.Deserialize(stream);
                stream.Close();
            }
            InitBookListView();
        }
        // saves the updates to the list
        private void SaveBtnClick(object sender, RoutedEventArgs e)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<LibraryItem>));
            FileStream stream = new FileStream(filePath, FileMode.Create);
            xmlSerializer.Serialize(stream, lm.Items);
            stream.Close();
            InitBookListView();
        }
        // return to the login page
        private void BackBtnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}
