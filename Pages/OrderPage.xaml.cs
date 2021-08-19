﻿using System;
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

namespace PapaDarioPizzaApp.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OrderPage : Page
    {
        MessageDialog messageDialog;
        private PizzaOrder currentPizzaOrder;
        private List<PizzaOrder> pizzaOrderList;
        private int serialNumber;
        private Dictionary<string, double> sizeDictionary;
        private Dictionary<string, double> toppingDictionary;
        private List<Pizza> pizzaList = new List<Pizza>();
        private List<Topping> toppingList = new List<Topping>();

        private double currentPrice = 0;
        private double currentPizzaRate = 0;
        private double currentPizzaPrice = 0;
        private double currentToppingPrice = 0;
        private double currentPizzaBasePrice = 0.0;
        public OrderPage()
        {
            this.InitializeComponent();
            currentPizzaOrder = new PizzaOrder();
            pizzaOrderList = new List<PizzaOrder>();

            sizeDictionary = new Dictionary<string, double>();
            toppingDictionary = new Dictionary<string, double>();


        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            tbCustomerId.IsEnabled = false;
            tbCustomerName.IsEnabled = false;
            tbDeliveryAddress.IsEnabled = false;

            this.PizzaOrderDate.Date = DateTime.Now;

                           
            DisableUIElements();
            LoadPizzaRate();
            LoadToppingRate();
        }

        private void chkDelivery_Checked(object sender, RoutedEventArgs e)
        {
           
        }

        private void chkDelivery_Click(object sender, RoutedEventArgs e)
        {
            if (chkDelivery.IsChecked == true)
            {
                tbDeliveryAddress.IsEnabled = true;
            }
            else
            {
                tbDeliveryAddress.IsEnabled = false;
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            


        }
        private void DisableUIElements()
        {
            tbCustomerId.IsEnabled = false;
            tbCustomerName.IsEnabled = false;
            tbPhoneNumber.IsEnabled = false;
            tbDeliveryAddress.IsEnabled = false;

            rbSmall.IsEnabled =  false;
            rbMedium.IsEnabled = false;
            rbLarge.IsEnabled = false;

            chkPepperoni.IsEnabled = false;
            chkMushrooms.IsEnabled = false;
            chkOnion.IsEnabled = false;
            chkSausage.IsEnabled = false;
            chkBacon.IsEnabled = false;
            chkExtraCheese.IsEnabled = false;
            chkBlackOlives.IsEnabled = false;
            chkGreenPeppers.IsEnabled = false;
            chkPineapple.IsEnabled = false;
            chkSpiniach.IsEnabled = false;
            chkBroccoli.IsEnabled = false;

            chkDelivery.IsEnabled = false;

            //Disable Buttons
            btnAdd.IsEnabled = false;
            btnUpdate.IsEnabled = false;
            btnDelete.IsEnabled = false;
            btnReset.IsEnabled = false;
            btnSubmitOrder.IsEnabled = false;
            btnPrintBill.IsEnabled = false;
        }
        private void EnableUIElements()
        {
            //tbCustomerId.IsEnabled = true;
            //tbCustomerName.IsEnabled = true;
            tbPhoneNumber.IsEnabled = true;
            //tbDeliveryAddress.IsEnabled = true;
            //orderDate.IsEnabled = false;

            chkDelivery.IsEnabled = true;


            rbSmall.IsEnabled = true;
            rbMedium.IsEnabled = true;
            rbLarge.IsEnabled = true;

            chkPepperoni.IsEnabled = true;
            chkMushrooms.IsEnabled = true;
            chkOnion.IsEnabled = true;
            chkSausage.IsEnabled = true;
            chkBacon.IsEnabled = true;
            chkExtraCheese.IsEnabled = true;
            chkBlackOlives.IsEnabled = true;
            chkGreenPeppers.IsEnabled = true;
            chkPineapple.IsEnabled = true;
            chkSpiniach.IsEnabled = true;
            chkBroccoli.IsEnabled = true;

            //Disable Buttons
            btnAdd.IsEnabled = true;
            btnUpdate.IsEnabled = false;
            btnDelete.IsEnabled = false;
            btnReset.IsEnabled = false;
            btnSubmitOrder.IsEnabled = false;
            btnPrintBill.IsEnabled = false;
        }

        private void btnNewOrder_Click(object sender, RoutedEventArgs e)
        {
            serialNumber = 0;
            EnableUIElements();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            btnUpdate.IsEnabled = true;
            btnDelete.IsEnabled = true;


            CheckDataValidation();
        }
        private async void CheckDataValidation()
        {
            string message = "";
            string caption = "Data Validation Error";

            //if(this.PizzaOrderDate.Date == null)
            //{
            //    message = "You should select the Date to order !";
            //    messageDialog = new MessageDialog(message, caption);
            //    await messageDialog.ShowAsync();
            //    return;
            //}


            //var orderDate = this.PizzaOrderDate.Date;
            //if(orderDate < DateTime.Now)
            //{
            //    message = "Order Date cannot be past date! !";
            //    messageDialog = new MessageDialog(message, caption);
            //    await messageDialog.ShowAsync();
            //    this.PizzaOrderDate.Focus(FocusState.Programmatic );

            //    return;
            //}
            if(tbPhoneNumber.Text == "")
            {
                message = "Phone number is missing!";
                messageDialog = new MessageDialog(message, caption);
                await messageDialog.ShowAsync();
                this.tbPhoneNumber.Focus(FocusState.Programmatic);
                return;
            }
            if (rbLarge.IsChecked == false && rbSmall.IsChecked == false && rbMedium.IsChecked == false)
            {
                message = "You should select at lest one size for the pizza!";
                messageDialog = new MessageDialog(message, caption);
                await messageDialog.ShowAsync();
                return;
            }

            if(chkPepperoni.IsChecked == false && 
                chkMushrooms.IsChecked == false &&
                chkOnion.IsChecked == false &&
                chkSausage.IsChecked == false &&
                chkBacon.IsChecked == false &&
                chkExtraCheese.IsChecked == false &&
                chkBlackOlives.IsChecked == false &&
                chkGreenPeppers.IsChecked == false &&
                chkPineapple.IsChecked == false &&
                chkSpiniach.IsChecked == false &&
                chkBroccoli.IsChecked == false
                )
            {

                message = "You should select at lest one Topping!";
                messageDialog = new MessageDialog(message, caption);
                await messageDialog.ShowAsync();
                return;
            }



            PizzaOrder temp = new PizzaOrder();
            temp.OrderNumber = 1;
            //temp.OrderDate = Convert.ToDateTime(PizzaOrderDate.Date);
            serialNumber += 1;
            temp.SerialNumber = serialNumber ;
            if(chkDelivery.IsChecked == true)
            {
                temp.IsDelivery = true;
            }
            else
            {
                temp.IsDelivery = false;
            }
            //temp.CustomerId = Convert.ToInt32( tbCustomerId.Text);
            temp.PhoneNumber = tbPhoneNumber.Text;

            if(rbSmall.IsChecked == true)
            {
                temp.Size = "Small";
            }else if(rbMedium.IsChecked == true)
            {
                temp.Size = "Medium";
            }
            else if(rbLarge.IsChecked == true)
            {
                temp.Size = "Large";
            }

            //Toppings
            if (chkPepperoni.IsChecked == true)
            {
                //temp.selectedToppings.Add("Pepperoni", 1.5);
                temp.Toppings = "Pepperoni";
            }
            if(chkMushrooms.IsChecked == true)
            {
                //temp.selectedToppings.Add("Mushrooms", 1.5);
                temp.Toppings +=" Mushrooms";
            }
             if (chkOnion.IsChecked == true)
            {
                //temp.selectedToppings.Add("Onion", 1.5);
                temp.Toppings += " Onion";
            }
            if (chkSausage.IsChecked == true)
            {
                //temp.selectedToppings.Add("Sausage", 1.5);
                temp.Toppings += " Sausage";
            }
            if (chkBacon.IsChecked == true)
            {
                //temp.selectedToppings.Add("Bacon", 1.5);
                temp.Toppings += " Bacon";
            }
           if (chkExtraCheese.IsChecked == true)
            {
                //temp.selectedToppings.Add("Extra Cheese", 1.5);
                temp.Toppings += " Extra Cheese";
            }
            if (chkBlackOlives.IsChecked == true)
            {
                //temp.selectedToppings.Add("Black Olives", 1.5);
                temp.Toppings += " Black Olives";
            }
             if (chkPineapple.IsChecked == true)
            {
                //temp.selectedToppings.Add("Pineapple", 1.5);
                temp.Toppings += " Pineapple";
            }
             if (chkSpiniach.IsChecked == true)
            {
                //temp.selectedToppings.Add("Spiniach", 1.5);
                temp.Toppings += " Spiniach";
            }
            if (chkBroccoli.IsChecked == true)
            {
                //temp.selectedToppings.Add("Broccoli", 1.5);
                temp.Toppings += " Broccoli";
            }
            dataGridViewOrder.ItemsSource = null;
            pizzaOrderList.Add(temp);
            dataGridViewOrder.ItemsSource = pizzaOrderList;

        }
        private void LoadPizzaRate()
        {
            string connectionString = DBConnnection.GetConnectionString();
            SqlConnection conn = new SqlConnection(connectionString);
            string query = "SELECT * FROM PizzaSize;";
            SqlCommand cmd = new SqlCommand(query, conn);
            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                pizzaList.Clear();
                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["Id"]);
                    string pizzaSize = reader["PizzaSize"].ToString();
                    string description = reader["PizzaDescription"].ToString();
                    double price = Convert.ToDouble(reader["Price"]);
                    Pizza tempPizza = new Pizza(id, pizzaSize, description, price);
                    pizzaList.Add(tempPizza);

                }

                double smallRate = 0;
                double mediumRate = 0;
                double largeRate = 0;
                foreach(Pizza temp in pizzaList)
                {
                    if(temp.ID == 1)
                    {
                        smallRate = temp.Price;
                    }
                    else if(temp.ID == 2)
                    {
                        mediumRate = temp.Price;
                    }
                    else if(temp.ID == 3)
                    {
                        largeRate = temp.Price;
                    }
                }

                rbSmall.Content += " @ " + smallRate;
                rbMedium.Content += " @ " + mediumRate;
                rbLarge.Content += " @ " + largeRate;
                     
                

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
        private void LoadToppingRate()
        {
            string connectionString = DBConnnection.GetConnectionString();
            SqlConnection conn = new SqlConnection(connectionString);
            string query = "SELECT * FROM Toppings;";
            SqlCommand cmd = new SqlCommand(query, conn);
            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                toppingList.Clear();
                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["Id"]);
                    
                    string toppingName = reader["ToppingName"].ToString();
                    string description = "";

                    if (!reader.IsDBNull(2))
                    {
                        description = reader["Description"].ToString();
                    }

                  
                    double price = Convert.ToDouble(reader["Price"]);
                    Topping temp = new Topping(id, toppingName, description, price);
                    toppingList.Add(temp);
                    
                }

                //double smallRate = 0;
                //double mediumRate = 0;
                //double largeRate = 0;
                foreach (Topping temp in toppingList)
                {
                    if (temp.ID == 1)
                    {
                        chkPepperoni.Content += " @ " + temp.ToppingPrice;
                    }
                    else if (temp.ID == 2)
                    {
                        chkMushrooms.Content += " @ " + temp.ToppingPrice;
                    }
                    else if (temp.ID == 3)
                    {
                        chkOnion.Content += " @ " + temp.ToppingPrice;
                    }
                    else if (temp.ID == 4)
                    {
                        chkSausage.Content += " @ " + temp.ToppingPrice;
                    }
                    else if (temp.ID == 5)
                    {
                        chkBacon.Content += " @ " + temp.ToppingPrice;
                    }
                    else if (temp.ID == 6)
                    {
                        chkExtraCheese.Content += " @ " + temp.ToppingPrice;
                    }
                    else if (temp.ID == 7)
                    {
                        chkBlackOlives.Content += " @ " + temp.ToppingPrice;
                    }
                    else if (temp.ID == 8)
                    {
                        chkGreenPeppers.Content += " @ " + temp.ToppingPrice;
                    }
                    else if (temp.ID == 9)
                    {
                        chkPineapple.Content += " @ " + temp.ToppingPrice;
                    }
                    else if (temp.ID == 10)
                    {
                        chkSpiniach.Content += " @ " + temp.ToppingPrice;
                    }
                    else if (temp.ID == 11)
                    {
                        chkBroccoli.Content += " @ " + temp.ToppingPrice;
                    }


                }

                //rbSmall.Content += " @ " + smallRate;
                //rbMedium.Content += " @ " + mediumRate;
                //rbLarge.Content += " @ " + largeRate;



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

        private double FindToppingRate(int i)
        {
            double rate = 0;
            foreach(Topping temp in toppingList)
            {
                if(temp.ID == i)
                {
                    rate = temp.ToppingPrice;
                    break;
                }
            }
            return rate;
        }
        private double FindPizzaRate(int i)
        {
            double rate = 0;
            foreach (Pizza temp in pizzaList)
            {
                if (temp.ID == i)
                {
                    rate = temp.Price;
                    break;
                }
            }
            return rate;
        }

        private void rbSmall_Click(object sender, RoutedEventArgs e)
        {
            double rate = FindPizzaRate(1);
            currentPizzaBasePrice = rate;
            TextBlock_BasePrice.Text = "Base Price = " + currentPizzaBasePrice.ToString("0.00");

            TextBlock_PizzaPrice.Text = "Price = " + (currentPizzaBasePrice + currentToppingPrice).ToString("0.00");

           

        }
        private void UpdatePrice()
        {

            TextBlock_PizzaPrice.Text = "Price : " + currentPrice;
        }

        private void rbMedium_Click(object sender, RoutedEventArgs e)
        {
            double rate = FindPizzaRate(2);
            currentPizzaBasePrice = rate;
            TextBlock_BasePrice.Text = "Base Price = " + currentPizzaBasePrice.ToString("0.00");

            TextBlock_PizzaPrice.Text = "Price = " + (currentPizzaBasePrice + currentToppingPrice).ToString("0.00");
        }

        private void rbLarge_Click(object sender, RoutedEventArgs e)
        {
            double rate = FindPizzaRate(3);
            currentPizzaBasePrice = rate;
            TextBlock_BasePrice.Text = "Base Price = " + currentPizzaBasePrice.ToString("0.00");

            TextBlock_PizzaPrice.Text = "Price = " + (currentPizzaBasePrice + currentToppingPrice).ToString("0.00");
        }

        private void chkPepperoni_Click(object sender, RoutedEventArgs e)
        {
            double toppingRate = FindToppingRate(1);
            if(chkPepperoni.IsChecked == true)
            {
                currentToppingPrice = currentToppingPrice + toppingRate;
            }
            else 
            {
                currentToppingPrice = currentToppingPrice - toppingRate;
            }

            currentToppingPrice = Math.Round(currentToppingPrice, 2);

            if (currentToppingPrice == 0.0)
            {
                TextBlock_ToppingPrice.Text = "Topping Price = " + currentToppingPrice.ToString("0.00");
            }
            else
            {
                TextBlock_ToppingPrice.Text = "Topping Price = " + currentToppingPrice.ToString("0.00");
            }

            TextBlock_PizzaPrice.Text = "Price = " + (currentPizzaBasePrice + currentToppingPrice).ToString("0.00");
        }
        private void FindTotalToppingPrice()
        {

        }

        private void chkMushrooms_Click(object sender, RoutedEventArgs e)
        {
            double toppingRate = FindToppingRate(2);
            if (chkMushrooms.IsChecked == true)
            {
                currentToppingPrice = currentToppingPrice + toppingRate;
            }
            else
            {
                currentToppingPrice = currentToppingPrice - toppingRate;
            }
            currentToppingPrice = Math.Round(currentToppingPrice, 2);
            if (currentToppingPrice == 0)
            {
                TextBlock_ToppingPrice.Text = "Topping Price = " + currentToppingPrice.ToString("0.00");
            }
            else
            {
                TextBlock_ToppingPrice.Text = "Topping Price = " + currentToppingPrice.ToString("0.00");
            }

            TextBlock_PizzaPrice.Text = "Price = " + (currentPizzaBasePrice + currentToppingPrice).ToString("0.00");
        }

        private void chkOnion_Click(object sender, RoutedEventArgs e)
        {
            double toppingRate = FindToppingRate(3);
            if (chkOnion.IsChecked == true)
            {
                currentToppingPrice = currentToppingPrice + toppingRate;
            }
            else
            {
                currentToppingPrice = currentToppingPrice - toppingRate;
            }

            currentToppingPrice = Math.Round(currentToppingPrice, 2);
            if (currentToppingPrice == 0)
            {
                TextBlock_ToppingPrice.Text = "Topping Price = " + currentToppingPrice.ToString("0.00");
            }
            else
            {
                TextBlock_ToppingPrice.Text = "Topping Price = " + currentToppingPrice.ToString("0.00");
            }

            TextBlock_PizzaPrice.Text = "Price = " + (currentPizzaBasePrice + currentToppingPrice).ToString("0.00");
        }

        private void chkSausage_Click(object sender, RoutedEventArgs e)
        {
            double toppingRate = FindToppingRate(4);
            if (chkSausage.IsChecked == true)
            {
                currentToppingPrice = currentToppingPrice + toppingRate;
            }
            else
            {
                currentToppingPrice = currentToppingPrice - toppingRate;
            }
            currentToppingPrice = Math.Round(currentToppingPrice, 2);
            if (currentToppingPrice == 0)
            {
                TextBlock_ToppingPrice.Text = "Topping Price = " + currentToppingPrice.ToString("0.00");
            }
            else
            {
                TextBlock_ToppingPrice.Text = "Topping Price = " + currentToppingPrice.ToString("0.00");
            }

            TextBlock_PizzaPrice.Text = "Price = " + (currentPizzaBasePrice + currentToppingPrice).ToString("0.00");
        }

        private void chkBacon_Click(object sender, RoutedEventArgs e)
        {
            double toppingRate = FindToppingRate(5);
            if (chkBacon.IsChecked == true)
            {
                currentToppingPrice = currentToppingPrice + toppingRate;
            }
            else
            {
                currentToppingPrice = currentToppingPrice - toppingRate;
            }
            currentToppingPrice = Math.Round(currentToppingPrice, 2);
            if (currentToppingPrice == 0)
            {
                TextBlock_ToppingPrice.Text = "Topping Price = " + currentToppingPrice.ToString("0.00");
            }
            else
            {
                TextBlock_ToppingPrice.Text = "Topping Price = " + currentToppingPrice.ToString("0.00");
            }

            TextBlock_PizzaPrice.Text = "Price = " + (currentPizzaBasePrice + currentToppingPrice).ToString("0.00");
        }

        private void chkExtraCheese_Click(object sender, RoutedEventArgs e)
        {
            double toppingRate = FindToppingRate(6);
            if (chkExtraCheese.IsChecked == true)
            {
                currentToppingPrice = currentToppingPrice + toppingRate;
            }
            else
            {
                currentToppingPrice = currentToppingPrice - toppingRate;
            }
            currentToppingPrice = Math.Round(currentToppingPrice, 2);
            if (currentToppingPrice == 0)
            {
                TextBlock_ToppingPrice.Text = "Topping Price = " + currentToppingPrice.ToString("0.00");
            }
            else
            {
                TextBlock_ToppingPrice.Text = "Topping Price = " + currentToppingPrice.ToString("0.00");
            }

            TextBlock_PizzaPrice.Text = "Price = " + (currentPizzaBasePrice + currentToppingPrice).ToString("0.00");
        }

        private void chkBlackOlives_Click(object sender, RoutedEventArgs e)
        {
            double toppingRate = FindToppingRate(7);
            if (chkBlackOlives.IsChecked == true)
            {
                currentToppingPrice = currentToppingPrice + toppingRate;
            }
            else
            {
                currentToppingPrice = currentToppingPrice - toppingRate;
            }
            currentToppingPrice = Math.Round(currentToppingPrice, 2);
            if (currentToppingPrice == 0)
            {
                TextBlock_ToppingPrice.Text = "Topping Price = " + currentToppingPrice.ToString("0.00");
            }
            else
            {
                TextBlock_ToppingPrice.Text = "Topping Price = " + currentToppingPrice.ToString("0.00");
            }

            TextBlock_PizzaPrice.Text = "Price = " + (currentPizzaBasePrice + currentToppingPrice).ToString("0.00");
        }

        private void chkGreenPeppers_Click(object sender, RoutedEventArgs e)
        {
            double toppingRate = FindToppingRate(8);
            if (chkGreenPeppers.IsChecked == true)
            {
                currentToppingPrice = currentToppingPrice + toppingRate;
            }
            else
            {
                currentToppingPrice = currentToppingPrice - toppingRate;
            }
            currentToppingPrice = Math.Round(currentToppingPrice, 2);
            if (currentToppingPrice == 0)
            {
                TextBlock_ToppingPrice.Text = "Topping Price = " + currentToppingPrice.ToString("0.00");
            }
            else
            {
                TextBlock_ToppingPrice.Text = "Topping Price = " + currentToppingPrice.ToString("0.00");
            }

            TextBlock_PizzaPrice.Text = "Price = " + (currentPizzaBasePrice + currentToppingPrice).ToString("0.00");
        }

        private void chkPineapple_Click(object sender, RoutedEventArgs e)
        {
            double toppingRate = FindToppingRate(9);
            if (chkPineapple.IsChecked == true)
            {
                currentToppingPrice = currentToppingPrice + toppingRate;
            }
            else
            {
                currentToppingPrice = currentToppingPrice - toppingRate;
            }
            currentToppingPrice = Math.Round(currentToppingPrice, 2);
            if (currentToppingPrice == 0)
            {
                TextBlock_ToppingPrice.Text = "Topping Price = " + currentToppingPrice.ToString("0.00");
            }
            else
            {
                TextBlock_ToppingPrice.Text = "Topping Price = " + currentToppingPrice.ToString("0.00");
            }

            TextBlock_PizzaPrice.Text = "Price = " + (currentPizzaBasePrice + currentToppingPrice).ToString("0.00");
        }

        private void chkSpiniach_Click(object sender, RoutedEventArgs e)
        {
            double toppingRate = FindToppingRate(10);
            if (chkSpiniach.IsChecked == true)
            {
                currentToppingPrice = currentToppingPrice + toppingRate;
            }
            else
            {
                currentToppingPrice = currentToppingPrice - toppingRate;
            }
            currentToppingPrice = Math.Round(currentToppingPrice, 2);
            if (currentToppingPrice == 0)
            {
                TextBlock_ToppingPrice.Text = "Topping Price = " + currentToppingPrice.ToString("0.00");
            }
            else
            {
                TextBlock_ToppingPrice.Text = "Topping Price = " + currentToppingPrice.ToString("0.00");
            }

            TextBlock_PizzaPrice.Text = "Price = " + (currentPizzaBasePrice + currentToppingPrice).ToString("0.00");
        }

        private void chkBroccoli_Click(object sender, RoutedEventArgs e)
        {
            double toppingRate = FindToppingRate(11);
            if (chkBroccoli.IsChecked == true)
            {
                currentToppingPrice = currentToppingPrice + toppingRate;
            }
            else
            {
                currentToppingPrice = currentToppingPrice - toppingRate;
            }
            currentToppingPrice = Math.Round(currentToppingPrice, 2);
            if (currentToppingPrice == 0)
            {
                TextBlock_ToppingPrice.Text = "Topping Price = " + currentToppingPrice.ToString("0.00");
            }
            else
            {
                TextBlock_ToppingPrice.Text = "Topping Price = " + currentToppingPrice.ToString("0.00");
            }

            TextBlock_PizzaPrice.Text = "Price = " + (currentPizzaBasePrice + currentToppingPrice).ToString("0.00");
        }
    }
}