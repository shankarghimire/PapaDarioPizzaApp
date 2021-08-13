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
            tbPizzaSizeId.IsEnabled = false;
            LoadDatatoUI();
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

                tbPizzaSizeId.Text = PizzaList[0].ID.ToString();
                tbPizzaSize.Text = PizzaList[0].PizzaSize.ToString();
                tbPizzaDescription.Text = PizzaList[0].Description.ToString();
                tbPizzaPrice.Text = PizzaList[0].Price.ToString();
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

        private async void dataGridViewPizzaSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int id = dataGridViewPizzaSize.SelectedIndex;
            Pizza temp = new Pizza();
            temp = (Pizza)dataGridViewPizzaSize.SelectedItem;
            //messageDialog = new MessageDialog("Select Item : " + (temp.ID + ", "  + temp.PizzaSize + ", " + temp.Description + ", " + temp.Price));
            //messageDialog.ShowAsync();
            LoadCurrentRecord(temp);


        }
        private void LoadCurrentRecord(Pizza currentPizza)
        {
            tbPizzaSizeId.Text = currentPizza.ID.ToString();
            tbPizzaSize.Text = currentPizza.PizzaSize.ToString();
            tbPizzaDescription.Text = currentPizza.Description.ToString();
            tbPizzaPrice.Text = currentPizza.Price.ToString();
        }
    }
}
