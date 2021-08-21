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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PapaDarioPizzaApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CustomerPage : Page
    {
        public CustomerPage()
        {
            this.InitializeComponent();
            //if(UserInfo.LoginStatus == true)
            //{
            //    this.btnLogIn.Content = "Log Out";
            //}
            //else
            //{
            //    this.btnLogIn.Content = "Log In";
            //}
            UserInfo.ChangeToLogOut(this);
            
        }

        private void btnGotoHome_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void btnOrder_Click(object sender, RoutedEventArgs e)
        {
            frameCustomerPage.Navigate(typeof(Pages.OrderPage));
        }

        private void btnLogIn_Click(object sender, RoutedEventArgs e)
        {
           
            frameCustomerPage.Navigate(typeof(WelcomePage));
        }

        //private void btnSignUp_Click(object sender, RoutedEventArgs e)
        //{
        //    frameCustomerPage.Navigate(typeof(Pages.SignUpPage));
        //}

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if(UserInfo.LoginStatus == true)
            {
                this.btnLogIn.Content = "Log Out";
            }
            else
            {
                this.btnLogIn.Content = "Log In";
            }
        }

        //private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        //{

        //}

        //private void btnTest1_Click(object sender, RoutedEventArgs e)
        //{
        //    Frame.Navigate(typeof(MainPage));
        //}
    }
}
