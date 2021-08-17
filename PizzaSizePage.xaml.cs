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
using Windows.UI.Popups;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PapaDarioPizzaApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PizzaSizePage : Page
    {
        string connectionString;
        private List<Pizza> PizzaList { get; set; }
        private Pizza pizza;

        MessageDialog messageDialog;

        public PizzaSizePage()
        {
            this.InitializeComponent();
            pizza = new Pizza();
            PizzaList = new List<Pizza>();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                tbPizzaSizeId.IsEnabled = false;
                dataGridViewPizzaSize.IsReadOnly = true;
                LoadDatatoUI();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                
            }
            finally
            {

            }
           
        }
        private void LoadDatatoUI()
        {
            connectionString = DBConnnection.GetConnectionString();
            SqlConnection conn = new SqlConnection(connectionString);
            string query = "SELECT * FROM PizzaSize;";
            SqlCommand cmd = new SqlCommand(query, conn);
            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                PizzaList.Clear();
                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["Id"]);
                    string pizzaSize = reader["PizzaSize"].ToString();
                    string description = reader["PizzaDescription"].ToString();
                    double price = Convert.ToDouble(reader["Price"]);
                    Pizza tempPizza = new Pizza(id, pizzaSize, description, price);                 
                    PizzaList.Add(tempPizza);

                }

                dataGridViewPizzaSize.ItemsSource = PizzaList;

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

        private  void dataGridViewPizzaSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int id = dataGridViewPizzaSize.SelectedIndex;
                Pizza temp = new Pizza();
                temp = (Pizza)dataGridViewPizzaSize.SelectedItem;
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
        private void LoadCurrentRecord(Pizza currentPizza)
        {
            try
            {
                tbPizzaSizeId.Text = currentPizza.ID.ToString();
                tbPizzaSize.Text = currentPizza.PizzaSize.ToString();
                tbPizzaDescription.Text = currentPizza.Description.ToString();
                tbPizzaPrice.Text = currentPizza.Price.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
            finally
            {

            }
           
        }

        private async void btnAddNew_Click(object sender, RoutedEventArgs e)
        {
            string pizzaSize = tbPizzaSize.Text;
            string pizzaDescription = tbPizzaDescription.Text;
            double pizzaPrice = 0.0;
            string message = "";
            string caption = "";

            try
            {
                if (pizzaSize == "")
                {
                    message = "Pizza Size Cannot be blank!";
                    caption = "Data Validation Error";
                    messageDialog = new MessageDialog(message, caption);
                    await messageDialog.ShowAsync();
                    tbPizzaSize.Focus(FocusState.Programmatic);
                }
                else if (pizzaDescription == "")
                {
                    message = "Pizza Descriptio Cannot be blank!";
                    caption = "Data Validation Error";
                    messageDialog = new MessageDialog(message, caption);
                    await messageDialog.ShowAsync();
                    tbPizzaDescription.Focus(FocusState.Programmatic);
                }
                else if (!double.TryParse(tbPizzaPrice.Text, out pizzaPrice))
                {
                    message = "Pizza Price Must be numeric value!";
                    caption = "Data Validation Error";
                    messageDialog = new MessageDialog(message, caption);
                    await messageDialog.ShowAsync();
                    tbPizzaPrice.Focus(FocusState.Programmatic);
                    //tbPizzaPrice.Text = "";
                }
                else
                {
                    pizzaSize = tbPizzaSize.Text;
                    pizzaDescription = tbPizzaDescription.Text;
                    pizzaPrice = Convert.ToDouble(tbPizzaPrice.Text);

                    Pizza pizza = new Pizza();
                    pizza.PizzaSize = pizzaSize;
                    pizza.Description = pizzaDescription;
                    pizza.Price = pizzaPrice;

                    //call method to insert data into database table
                    InsertPizza(pizza);

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
        async void InsertPizza(Pizza pizza)
        {
            string connectionString = DBConnnection.GetConnectionString();
            string query = "Insert Into PizzaSize(PizzaSize, PizzaDescription,Price)" + 
                "Values(@PizzaSize, @PizzaDescription,@Pizzaprice)";
            SqlConnection conn = new SqlConnection(connectionString); ;
            SqlCommand cmd = new SqlCommand(query, conn); ;
            try
            {
                cmd.Parameters.AddWithValue("PizzaSize", pizza.PizzaSize);
                cmd.Parameters.AddWithValue("PizzaDescription", pizza.Description);
                cmd.Parameters.AddWithValue("PizzaPrice", pizza.Price);
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
            catch(Exception ex)
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
                if (tbPizzaSize.Text == "" || tbPizzaDescription.Text == "" || tbPizzaPrice.Text == "")
                {
                    MessageDialog messageDialog = new MessageDialog("Some text fields are missing!", "Error");
                    await messageDialog.ShowAsync();
                    return;
                }

                int id = Convert.ToInt32(tbPizzaSizeId.Text);
                string pizzaSize = tbPizzaSize.Text;
                string pizzaDescription = tbPizzaDescription.Text;
                double pizzaPirce = Convert.ToDouble(tbPizzaPrice.Text);
                Pizza pizza = new Pizza(id, pizzaSize, pizzaDescription, pizzaPirce);
                UpdatePizza(pizza);

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

        async void UpdatePizza(Pizza pizza)
        {
            
            string connectionString = DBConnnection.GetConnectionString();
            string query = "Update PizzaSize " +
                "SET PizzaSize =@PizzaSize," +
                "PizzaDescription=@PizzaDescription," +
                "Price=@PizzaPrice" +
                " WHERE id = @Id";
            
            SqlConnection conn = new SqlConnection(connectionString); ;
            SqlCommand cmd = new SqlCommand(query, conn); ;
            try
            {
                cmd.Parameters.AddWithValue("Id", pizza.ID);
                cmd.Parameters.AddWithValue("PizzaSize", pizza.PizzaSize);
                cmd.Parameters.AddWithValue("PizzaDescription", pizza.Description);
                cmd.Parameters.AddWithValue("PizzaPrice", pizza.Price);
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
                if (tbPizzaSizeId.Text == "" )
                {
                    MessageDialog messageDialog = new MessageDialog("ID text fields is missing!", "Error");
                    await messageDialog.ShowAsync();
                    return;
                }

                int id = Convert.ToInt32(tbPizzaSizeId.Text);
                string pizzaSize = tbPizzaSize.Text;
                string pizzaDescription = tbPizzaDescription.Text;
                double pizzaPirce = Convert.ToDouble(tbPizzaPrice.Text);
                Pizza pizza = new Pizza(id, pizzaSize, pizzaDescription, pizzaPirce);
                DeletePizza(pizza);
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

        private async void DeletePizza(Pizza pizza)
        {

            string connectionString = DBConnnection.GetConnectionString();
            string query = "DELETE FROM PizzaSize " +
                " WHERE id = @Id";

            SqlConnection conn = new SqlConnection(connectionString); ;
            SqlCommand cmd = new SqlCommand(query, conn); ;
            try
            {
                cmd.Parameters.AddWithValue("Id", pizza.ID);
                //cmd.Parameters.AddWithValue("PizzaSize", pizza.PizzaSize);
                //cmd.Parameters.AddWithValue("PizzaDescription", pizza.Description);
                //cmd.Parameters.AddWithValue("PizzaPrice", pizza.Price);
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
                tbPizzaSizeId.Text = "";
                tbPizzaSize.Text = "";
                tbPizzaDescription.Text = "";
                tbPizzaPrice.Text = "";
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
            try
            {
                //LoadDatatoUI();
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
        private void LoadDataToDataGrid()
        {
            connectionString = DBConnnection.GetConnectionString();
            SqlConnection conn = new SqlConnection(connectionString);
            string query = "SELECT * FROM PizzaSize;";
            SqlCommand cmd = new SqlCommand(query, conn);
            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                PizzaList.Clear();
                dataGridViewPizzaSize.ItemsSource = null;
                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["Id"]);
                    string pizzaSize = reader["PizzaSize"].ToString();
                    string description = reader["PizzaDescription"].ToString();
                    double price = Convert.ToDouble(reader["Price"]);
                    Pizza tempPizza = new Pizza(id, pizzaSize, description, price);
                    PizzaList.Add(tempPizza);

                }

                dataGridViewPizzaSize.ItemsSource = PizzaList;

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
