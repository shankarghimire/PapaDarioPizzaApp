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
        private void btnAddNew_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnRefrsh_Click(object sender, RoutedEventArgs e)
        {

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

        }
    }
}
