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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.Data.SqlClient;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PapaDarioPizzaApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void ScrollViewer_Loaded(object sender, RoutedEventArgs e)
        {
            EnableDisableUI();
        }

        private void chkLogInAsAdmin_Click(object sender, RoutedEventArgs e)
        {
            EnableDisableUI();
            tbUserName.Focus(FocusState.Programmatic);
        }

        private void EnableDisableUI()
        {
            if (chkLogInAsAdmin.IsChecked == true)
            {
                //Disable Guest section
                //TextBlock_loginAsCustomer.Foreground = new System.Windows.Media.SolidColorBrush(Colors.Navy);
                //TextBlock_loginAsCustomer.Foreground = Color
                btnLoginGuest.IsEnabled = false;
                btnCancel1.IsEnabled = false;
                //Enable Admin Section
                tbUserName.IsEnabled = true;
                pswPassword.IsEnabled = true;
                btnLoginAdmin.IsEnabled = true;
                btnCancel2.IsEnabled = true;
            }
            else
            {
                //Enable Gues Section
                btnLoginGuest.IsEnabled = true;
                btnCancel1.IsEnabled = true;
                //Disable Admin Section
                tbUserName.IsEnabled = false;
                pswPassword.IsEnabled = false;
                btnLoginAdmin.IsEnabled = false;
                btnCancel2.IsEnabled = false;
            }
        }

        private void btnLoginGuest_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CustomerPage));
        }

        private void btnLoginAdmin_Click(object sender, RoutedEventArgs e)
        {
            string userName = "Admin";
            string storedPassword = "123";
            string inputPassword = pswPassword.Password.ToString();
            if(tbUserName.Text == userName && inputPassword  == storedPassword)
            {
                Frame.Navigate(typeof(AdminPage));
            }
            else
            {
                TextBloxk_loginFailureMessage.Text = "Inalid User Name or Password!";
                //pswPassword.ClearValue();
            }
            
        }

        //private void btnCheck_Click(object sender, RoutedEventArgs e)
        //{
        //    string cs = DBConnnection.GetConnectionString();
        //    tbtest.Text += cs;
        //    using (SqlConnection conn = new SqlConnection(cs))
        //    {
        //        conn.Open();
        //        tbtest.Text = conn.State.ToString();
        //    }
        //}

    }
}
