using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using BL;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LibraryProject
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        readonly UserLogin login = new UserLogin();
        User user;
        public MainPage()
        {
            this.InitializeComponent();
        }
        private void LoginClick(object sender, RoutedEventArgs e)
        {
            user = login.GetUserByNameAndPassword(tbName.Text, tbPassword.Password);
            if (user != null)
            {
                user.UserName = tbName.Text;
                user.Password = tbPassword.Password;
                Frame.Navigate(typeof(LibraryPage), user);
            }
            // if user is null - no such user exists
            else tbResult.Text = $"Wrong User Name or Password\nThe Users are in class UserLogin";
        }
    }
}
