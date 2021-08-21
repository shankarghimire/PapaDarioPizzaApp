using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Data.SqlClient;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PapaDarioPizzaApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WelcomePage : Page
    {
        MessageDialog messageDialog;
        public WelcomePage()
        {
            this.InitializeComponent();
        }

        private void btnGoToOrderPage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Frame.Navigate(typeof(CustomerPage));
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
           
        }

        private void btnContinueAsGuest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UserInfo.DefaultUserInfo();
                Frame.Navigate(typeof(Pages.OrderPage));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
           
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Frame.Navigate(typeof(MainPage));
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void btnLogIn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidateUserInput();
                CheckUserCredentials();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
           
        }
        private async void ValidateUserInput()
        {
            try
            {
                if (tbUserId.Text == "")
                {
                    string message = "User Id field cannot be blank!";
                    string caption = "Data Validation Error!";
                    messageDialog = new MessageDialog(message, caption);
                    await messageDialog.ShowAsync();
                    tbUserId.Focus(FocusState.Programmatic);
                    return;
                }
                if (pbPassword.Password == "")
                {
                    string message = "Password field cannot be blank!";
                    string caption = "Data Validation Error!";
                    messageDialog = new MessageDialog(message, caption);
                    await messageDialog.ShowAsync();
                    //pbPassword.Focus(FocusState.Programmatic);
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
            
        }

        private async void CheckUserCredentials()
        {
            string connectionString = DBConnnection.GetConnectionString();
            SqlConnection conn = new SqlConnection(connectionString);
            string inputUserId = tbUserId.Text;
            string inputUserPassword = pbPassword.Password;
            string query = "Select  UserId, UserPassword,FirstName, LastName FROM Customers WHERE UserId = '" + inputUserId + "';";
            SqlCommand cmd = new SqlCommand(query, conn);
            try
            {
                //bool result = false;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string userId2 = reader["UserId"].ToString();
                    string password2 = reader["UserPassword"].ToString();
                    string firstName = "";
                    if (reader["UserPassword"] != null)
                    {
                        firstName = reader["FirstName"].ToString();
                    }

                    string lastName = "";
                    if (reader["LastName"] != null)
                    {
                        lastName = reader["LastName"].ToString();
                    }
                    string fullName = firstName + ' ' + lastName;
                    userId2 = userId2.Trim();
                    inputUserId = inputUserId.Trim();
                    if (String.Equals(inputUserId, userId2))
                    {
                        inputUserPassword = inputUserPassword.Trim();
                        password2 = password2.Trim();
                        if (String.Equals(inputUserPassword, password2))
                        {
                            //result = true;
                            //User temp = new User();
                            //temp.UserId = tbUserId.Text;
                            string name = firstName + ' ' + lastName;
                            //User currentUser = new User();
                            UserInfo.GetLoggedInUser(tbUserId.Text, name);

                            //this.btnLogIn.Content = "Log Out";                          
                            //UserInfo.LoggedInUser.UserName = name;
                            //UserInfo.LoginStatus = true;

                            //To access LogIn Button of CustomerPage
                            //CustomerPage customerPage = new CustomerPage();
                            //ButtonContentChange(customerPage);
                            //customerPage.btnLogIn.Content = "Log Out";
                            //AccessCustomerPage.ChangeToLogOutButton();

                            //Opens up Order Page
                            //Frame.Navigate(typeof(OrderPage));
                            //Frame.Navigate(typeof(CustomerPage));
                            Frame.Navigate(typeof(Pages.OrderPage));

                            break;
                        }
                        else
                        {
                            string message = "Sorry! Wrong Credential! You entered wrong Password!";
                            string caption = "Wrong Login Credentials!";
                            messageDialog = new MessageDialog(message, caption);
                            await messageDialog.ShowAsync();
                        }
                    }
                    else
                    {
                        string message = "Sorry! Wrong Credential! You entered wrong UserId!";
                        string caption = "Wrong Login Credentials!";
                        messageDialog = new MessageDialog(message, caption);
                        await messageDialog.ShowAsync();
                    }
                }
                //if(result == false)
                //{
                //    string message = "Sorry! Wrong Credential! You entered wrong UserId or Password!";
                //    string caption = "Wrong Login Credentials!";
                //    messageDialog = new MessageDialog(message, caption);
                //    await messageDialog.ShowAsync();
                //}

            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                string caption = "Data Error";
                messageDialog = new MessageDialog(message, caption);
                Console.WriteLine(message);

            }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }
        }

        private void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Frame.Navigate(typeof(Pages.SignUpPage));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
           
        }
    }
}
