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

namespace PapaDarioPizzaApp.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SignUpPage : Page
    {
        MessageDialog messageDialog;
        public SignUpPage()
        {
            this.InitializeComponent();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidateUserInput();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
                if (pbConfirmPassword.Password == "")
                {
                    string message = "Confirm Password field cannot be blank!";
                    string caption = "Data Validation Error!";
                    messageDialog = new MessageDialog(message, caption);
                    await messageDialog.ShowAsync();
                    //pbConfirmPassword.Focus(FocusState.Programmatic);
                    return;
                }
                if (pbConfirmPassword.Password != pbPassword.Password)
                {
                    string message = "Confirm Password does not match with Pasword!";
                    string caption = "Data Validation Error!";
                    messageDialog = new MessageDialog(message, caption);
                    await messageDialog.ShowAsync();
                    //pbConfirmPassword.Focus(FocusState.Programmatic);
                    return;
                }
                if (tbFirstName.Text == "")
                {
                    string message = "First Name field cannot be blank!";
                    string caption = "Data Validation Error!";
                    messageDialog = new MessageDialog(message, caption);
                    await messageDialog.ShowAsync();
                    //tbFirstName.Focus(FocusState.Programmatic);
                    return;
                }
                if (tbLastName.Text == "")
                {
                    string message = "Last Name field cannot be blank!";
                    string caption = "Data Validation Error!";
                    messageDialog = new MessageDialog(message, caption);
                    await messageDialog.ShowAsync();
                    //tbLastName.Focus(FocusState.Programmatic);
                    return;
                }
                InsertCursomerRecord();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            

        }
        private async void InsertCursomerRecord()
        {
            string connectionString = DBConnnection.GetConnectionString();
            string query = "INSERT INTO Customers (UserId, UserPassword,FirstName, LastName, Phone,Email)" +
                "Values(@UserId, @UserPassword,@FirstName,@LastName,@Phone,@Email)";

            SqlConnection conn = new SqlConnection(connectionString); ;
            SqlCommand cmd = new SqlCommand(query, conn); ;
            try
            {
                Customer customer = new Customer();
                customer.UserId = tbUserId.Text;
                customer.Password = pbPassword.Password;
                customer.FirstName = tbFirstName.Text;
                customer.LastName = tbLastName.Text;
                if(tbPhoneNumber.Text == "")
                {
                    customer.Phone = "";
                }
                else
                {
                    customer.Phone = tbPhoneNumber.Text;
                }
                if(tbEmail.Text == "")
                {
                    customer.Email = "";
                }
                else
                {
                    customer.Email = tbEmail.Text;
                }

                cmd.Parameters.AddWithValue("UserId", customer.UserId);
                cmd.Parameters.AddWithValue("UserPassword", customer.Password);
                cmd.Parameters.AddWithValue("FirstName", customer.FirstName);
                cmd.Parameters.AddWithValue("LastName", customer.LastName);
                cmd.Parameters.AddWithValue("Phone", customer.Phone);
                cmd.Parameters.AddWithValue("Email", customer.Email);

                conn.Open();
                int result = cmd.ExecuteNonQuery();
                if (result == 1)
                {
                    MessageDialog messageDialog = new MessageDialog(" Congratulations!Customer Registration has been successfull!!!", "Success");
                    await messageDialog.ShowAsync();
                    Frame.Navigate(typeof(WelcomePage));

                }
                else
                {
                    MessageDialog messageDialog = new MessageDialog("☻ Sorry! Some error occurred! Please, Try again!", "Error");
                    await messageDialog.ShowAsync();
                }

                //LoadDataToDataGrid();
            }
            catch (Exception ex)
            {
                MessageDialog messageDialog = new MessageDialog(ex.Message.ToString(), "Error");
                await messageDialog.ShowAsync();
            }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Frame.Navigate(typeof(WelcomePage));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        private async void tbUserId_LostFocus(object sender, RoutedEventArgs e)
        {
            ValidateUserInput();
            string connectionString = DBConnnection.GetConnectionString();
            SqlConnection conn = new SqlConnection(connectionString);
            string userId = tbUserId.Text;
            string query = "SELECT COUNT(*) FROM Customers WHERE userId = '" + userId + "'; ";
            SqlCommand cmd = new SqlCommand(query, conn);
            try
            {
                conn.Open();
                int result = Convert.ToInt32(cmd.ExecuteScalar());
                if(result == 1)
                {
                    string message = "The UserId '" + userId + "' has been already taken!, Please try different one!";
                    string caption = "Data Validation Error";
                    messageDialog = new MessageDialog(message, caption);
                    await messageDialog.ShowAsync();
                    tbUserId.Focus(FocusState.Programmatic);

                }                                   
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                tbUserId.Focus(FocusState.Programmatic);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        
    }
}
