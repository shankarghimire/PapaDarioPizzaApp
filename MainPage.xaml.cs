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
        string connectionString = "";
        private User user;
        
        public MainPage()
        {
            this.InitializeComponent();
            user = new User();
            //loggedInUser = new User();
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
            string userName = tbUserName.Text;
            //string storedPassword = "12345";
            string inputPassword = pswPassword.Password.ToString();
            bool result = CheckLogInCredential( userName,  inputPassword);
            if (result == true)
            {
                Frame.Navigate(typeof(AdminPage));
            }
            else
            {
                TextBloxk_loginFailureMessage.Text = "Inalid User Name or Password!";
                //pswPassword.ClearValue();
            }

            

        }

        private bool CheckLogInCredential(string userName, string userPassword)
        {
            bool result = false;
            connectionString = DBConnnection.GetConnectionString();
            SqlConnection conn = new SqlConnection(connectionString);
            string query = "SELECT * FROM Users where UserName = '" + userName + "' and UserPassword='" + userPassword +"';";
            SqlCommand cmd = new SqlCommand(query, conn);
            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                //PizzaList.Clear();
                while (reader.Read())
                {
                    //int id = Convert.ToInt32(reader["Id"]);
                    string userNameFromDB = reader["UserName"].ToString();
                    string userPasswordFromDB = reader["UserPassword"].ToString();
                    if(userName == userNameFromDB && userPassword == userPasswordFromDB)
                    {
                        result = true;
                        break;
                    }

                }

                //dataGridViewPizzaSize.ItemsSource = PizzaList;

                //tbPizzaSizeId.Text = PizzaList[0].ID.ToString();
                //tbPizzaSize.Text = PizzaList[0].PizzaSize.ToString();
                //tbPizzaDescription.Text = PizzaList[0].Description.ToString();
                //tbPizzaPrice.Text = PizzaList[0].Price.ToString();
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                //string caption = "Data Error";
                //messageDialog = new MessageDialog(message, caption);
                Console.WriteLine(message);

            }
            finally
            {
                cmd.Dispose();
                conn.Close();
                
            }

            return result;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //tbUserName.Text = "Admin";
            //pswPassword.Password = "12345";
            
        }

        private void btnCancel2_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        private void btnCancel1_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
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
