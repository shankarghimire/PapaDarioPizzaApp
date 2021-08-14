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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.Data.SqlClient;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PapaDarioPizzaApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ToppingsPage : Page
    {
        string connectionString;
        private List<Topping> ToppingList { get; set; }
        private Topping topping;

        MessageDialog messageDialog;

        public ToppingsPage()
        {
            this.InitializeComponent();
            topping = new Topping();
            ToppingList = new List<Topping>();
        }


        private void LoadDatatoUI()
        {
            connectionString = DBConnnection.GetConnectionString();
            SqlConnection conn = new SqlConnection(connectionString);
            string query = "SELECT * FROM Toppings;";
            SqlCommand cmd = new SqlCommand(query, conn);
            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                ToppingList.Clear();
                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["Id"]);
                    string toppingName = reader["ToppingName"].ToString();
                    string toppingDescription = reader["ToppingDescription"].ToString();
                    double toppingPrice = Convert.ToDouble(reader["Price"]);
                    Topping tempTopping = new Topping(id, toppingName, toppingDescription, toppingPrice);
                    ToppingList.Add(tempTopping);

                }

                dataGridViewToppings.ItemsSource = ToppingList;
                 //tbPizzaSizeId.Text = PizzaList[0].ID.ToString();
                //tbPizzaSize.Text = PizzaList[0].PizzaSize.ToString();
                //tbPizzaDescription.Text = PizzaList[0].Description.ToString();
                //tbPizzaPrice.Text = PizzaList[0].Price.ToString();
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
        private async void btnAddNew_Click(object sender, RoutedEventArgs e)
        {
            string toppingName = tbToppingName.Text;
            string toppingDescription = tbToppingDescription.Text;
            double toppingPrice = 0.0;
            string message = "";
            string caption = "";

            try
            {
                if (toppingName == "")
                {
                    message = "Topping Name Cannot be blank!";
                    caption = "Data Validation Error";
                    messageDialog = new MessageDialog(message, caption);
                    await messageDialog.ShowAsync();
                    tbToppingName.Focus(FocusState.Programmatic);
                }               
                else if (!double.TryParse(tbToppingPrice.Text, out toppingPrice))
                {
                    message = "Toppig Price Must be numeric value!";
                    caption = "Data Validation Error";
                    messageDialog = new MessageDialog(message, caption);
                    await messageDialog.ShowAsync();
                    tbToppingPrice.Focus(FocusState.Programmatic);
                    
                }
                else
                {
                    toppingName = tbToppingName.Text;
                    toppingDescription = tbToppingDescription.Text;
                    toppingPrice = Convert.ToDouble(tbToppingPrice.Text);

                    Topping topping = new Topping();
                    topping.ToppingName = toppingName;
                    topping.ToppingDescription = toppingDescription;
                    topping.ToppingPrice = toppingPrice;

                    //call method to insert data into database table
                    InsertTopping(topping);

                    LoadDataToDataGrid();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
            finally
            {

            }

            
        }

        async void InsertTopping(Topping topping)
        {
            string connectionString = DBConnnection.GetConnectionString();
            string query = "Insert Into Toppings(ToppingName, ToppingDescription,Price)" +
                "Values(@ToppingName, @ToppingDescription,@ToppingPrice)";
            SqlConnection conn = new SqlConnection(connectionString); ;
            SqlCommand cmd = new SqlCommand(query, conn);
            try
            {
                cmd.Parameters.AddWithValue("ToppingName", topping.ToppingName);
                cmd.Parameters.AddWithValue("ToppingDescription", topping.ToppingDescription);
                cmd.Parameters.AddWithValue("ToppingPrice", topping.ToppingPrice);
                conn.Open();
                int result = cmd.ExecuteNonQuery();
                if (result == 1)
                {
                    MessageDialog messageDialog = new MessageDialog("New Record successfully saved to the database!", "Success");
                    await messageDialog.ShowAsync();

                }
                else
                {
                    MessageDialog messageDialog = new MessageDialog("Some error occurred! Please, Try again!", "Error");
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
        private async void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tbToppingName.Text == "" ||  tbToppingPrice.Text == "")
                {
                    MessageDialog messageDialog = new MessageDialog("ID text field is missing!", "Error");
                    await messageDialog.ShowAsync();
                    return;
                }

                int id = Convert.ToInt32(tbToppingId.Text);
                string toppingName = tbToppingName.Text;
                string toppingDescription = tbToppingDescription.Text;
                double toppingPirce = Convert.ToDouble(tbToppingPrice.Text);
                Topping topping = new Topping(id, toppingName, toppingDescription, toppingPirce);
                UpdateTopping(topping);

                LoadDataToDataGrid();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
            finally
            {

            }
        }

        async void UpdateTopping(Topping topping)
        {

            string connectionString = DBConnnection.GetConnectionString();
            string query = "Update Toppings " +
                "SET ToppingName =@ToppingName," +
                "ToppingDescription=@ToppingDescription," +
                "Price=@ToppingPrice" +
                " WHERE id = @Id";

            SqlConnection conn = new SqlConnection(connectionString); ;
            SqlCommand cmd = new SqlCommand(query, conn); ;
            try
            {
                cmd.Parameters.AddWithValue("Id", topping.ID);
                cmd.Parameters.AddWithValue("ToppingName", topping.ToppingName);
                cmd.Parameters.AddWithValue("ToppingDescription", topping.ToppingDescription);
                cmd.Parameters.AddWithValue("ToppingPrice", topping.ToppingPrice);
                conn.Open();
                int result = cmd.ExecuteNonQuery();
                if (result == 1)
                {
                    MessageDialog messageDialog = new MessageDialog("Record successfully updated in the database!", "Success");
                    await messageDialog.ShowAsync();

                }
                else
                {
                    MessageDialog messageDialog = new MessageDialog("Some error occurred! Please, Try again!", "Error");
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
        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tbToppingId.Text == "" )
                {
                    MessageDialog messageDialog = new MessageDialog("ID text fields is missing!", "Error");
                    await messageDialog.ShowAsync();
                    return;
                }

                int id = Convert.ToInt32(tbToppingId.Text);
                
                Topping topping = new Topping();
                topping.ID = id;

                DeleteTopping(topping);
                LoadDataToDataGrid();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
            finally
            {

            }
        }

        private async void DeleteTopping(Topping topping)
        {

            string connectionString = DBConnnection.GetConnectionString();
            string query = "DELETE FROM Toppings  WHERE id = @Id";

            SqlConnection conn = new SqlConnection(connectionString); ;
            SqlCommand cmd = new SqlCommand(query, conn); ;
            try
            {
                cmd.Parameters.AddWithValue("Id", topping.ID);
                
                conn.Open();
                int result = cmd.ExecuteNonQuery();
                if (result == 1)
                {
                    MessageDialog messageDialog = new MessageDialog("Record successfully deleted from the database!", "Success");
                    await messageDialog.ShowAsync();

                }
                else
                {
                    MessageDialog messageDialog = new MessageDialog("Some error occurred! Please, Try again!", "Error");
                    await messageDialog.ShowAsync();
                }

                tbToppingId.Text = "";
                tbToppingName.Text = "";
                tbToppingDescription.Text = "";
                tbToppingPrice.Text = "";
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
        private void btnRefrsh_Click(object sender, RoutedEventArgs e)
        {
            LoadDataToDataGrid();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                tbToppingId.IsEnabled = false;
                dataGridViewToppings.IsReadOnly = true;
               
                LoadDatatoUI();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());

            }
            finally
            {

            }
        }

        private void dataGridViewPizzaSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int id = dataGridViewToppings.SelectedIndex;
                Topping temp = new Topping();
                temp = (Topping)dataGridViewToppings.SelectedItem;
                //messageDialog = new MessageDialog("Select Item : " + (temp.ID + ", "  + temp.PizzaSize + ", " + temp.Description + ", " + temp.Price));
                //messageDialog.ShowAsync();
                if (temp != null)
                {
                    LoadCurrentRecord(temp);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
            finally
            {

            }

        }

        private void LoadCurrentRecord(Topping currentTopping)
        {
            try
            {
                tbToppingId.Text = currentTopping.ID.ToString();
                tbToppingName.Text = currentTopping.ToppingName.ToString();
                tbToppingDescription.Text = currentTopping.ToppingDescription.ToString();
                tbToppingPrice.Text = currentTopping.ToppingPrice.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
            finally
            {

            }

        }

        private void LoadDataToDataGrid()
        {
            connectionString = DBConnnection.GetConnectionString();
            SqlConnection conn = new SqlConnection(connectionString);
            string query = "SELECT * FROM Toppings;";
            SqlCommand cmd = new SqlCommand(query, conn);
            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                ToppingList.Clear();
                dataGridViewToppings.ItemsSource = null;
                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["Id"]);
                    string toppingName = reader["ToppingName"].ToString();
                    string toppingDescription = reader["ToppingDescription"].ToString();
                    double toppingPrice = Convert.ToDouble(reader["Price"]);
                    Topping tempTopping = new Topping(id, toppingName, toppingDescription, toppingPrice);
                    ToppingList.Add(tempTopping);

                }

                dataGridViewToppings.ItemsSource = ToppingList;

                //tbPizzaSizeId.Text = PizzaList[0].ID.ToString();
                //tbPizzaSize.Text = PizzaList[0].PizzaSize.ToString();
                //tbPizzaDescription.Text = PizzaList[0].Description.ToString();
                //tbPizzaPrice.Text = PizzaList[0].Price.ToString();
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


    }
}
