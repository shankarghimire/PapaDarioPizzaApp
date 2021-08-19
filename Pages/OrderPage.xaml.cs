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

        private int updateId = -1;

        private double currentPrice = 0;
        //private double currentPizzaRate = 0;
        private double currentPizzaPrice = 0;
        private double currentToppingPrice = 0;
        private double currentPizzaBasePrice = 0.0;
        //private int toppingIndex = 0;

        private double totalAmount = 0.0;
        private double discountAmount = 0.0;
        private double grandTotalAmount = 0.0;

        private bool receiptReadyToPrint = false;
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
            currentPizzaPrice = 0;
            currentToppingPrice = 0;
            currentPizzaBasePrice = 0.0;
        //private int toppingIndex = 0;

            totalAmount = 0.0;
            discountAmount = 0.0;
            grandTotalAmount = 0.0;
            EnableUIElements();
            dataGridViewOrder.ItemsSource = null;

            //btnNewOrder.IsEnabled = false;
            btnPrintBill.IsEnabled = false;

            //Reset all Radio button
            rbSmall.IsChecked = false;
            rbMedium.IsChecked = false;
            rbLarge.IsChecked = false;

            //Reset All Check Boxes
            chkPepperoni.IsChecked = false;
            chkMushrooms.IsChecked = false;
            chkOnion.IsChecked = false;
            chkBacon.IsChecked = false;
            chkSausage.IsChecked = false;
            chkExtraCheese.IsChecked = false;
            chkBlackOlives.IsChecked = false;
            chkGreenPeppers.IsChecked = false;
            chkPineapple.IsChecked = false;
            chkSpiniach.IsChecked = false;
            chkBroccoli.IsChecked = false;
            chkOnion.IsChecked = false;

            //Reset All TextBlock
            TextBlock_TotalAmount.Text = "Total = " + totalAmount.ToString("0.00");
            TextBlock_DiscountAmount.Text = "Discount = " + discountAmount.ToString("0.00");
            TextBlock_GrandTotalAmount.Text = "Grand Total = " + grandTotalAmount.ToString("0.00");

            TextBlock_BasePrice.Text = "Base Price : ";
            TextBlock_PizzaPrice.Text = "Pizza Price : ";
            TextBlock_ToppingPrice.Text = "Topping Price : ";



        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

            //Resetns the variables
            currentPizzaPrice = 0;
            currentToppingPrice = 0;
            currentPizzaBasePrice = 0.0;


            //Resents the Buttons
            btnUpdate.IsEnabled = true;
            btnDelete.IsEnabled = true;
            btnSubmitOrder.IsEnabled = true;
            btnNewOrder.IsEnabled = false;

           


            CheckDataValidation();

            //reset radio buttons
            rbSmall.IsChecked = false;
            rbMedium.IsChecked = false;
            rbLarge.IsChecked = false;

            //Resets all check boxes
            chkPepperoni.IsChecked = false;
            chkMushrooms.IsChecked = false;
            chkOnion.IsChecked = false;
            chkSausage.IsChecked = false;
            chkBacon.IsChecked = false;
            chkExtraCheese.IsChecked = false;
            chkBlackOlives.IsChecked = false;
            chkGreenPeppers.IsChecked = false;
            chkPineapple.IsChecked = false;
            chkSpiniach.IsChecked = false;
            chkBroccoli.IsChecked = false;

            //Resets the TextBlock
            TextBlock_BasePrice.Text = "Base Price: ";
            TextBlock_ToppingPrice.Text = "Topping Price : ";
            TextBlock_PizzaPrice.Text = "Pizza Price : ";

        }
        private async void CheckDataValidation()
        {
            string message = "";
            string caption = "Data Validation Error";

            //if (this.PizzaOrderDate.Date == null)
            //{
            //    message = "You should select the Date to order !";
            //    messageDialog = new MessageDialog(message, caption);
            //    await messageDialog.ShowAsync();
            //    return;
            //}

            var currentDate = DateTime.Now;
            DateTime orderDate = this.PizzaOrderDate.Date.DateTime;
            
           
            var aa = this.PizzaOrderDate.SelectedDate;


            if (orderDate.Year < currentDate.Year  )
            {
                message = "Order Date cannot be past date! !";
                messageDialog = new MessageDialog(message, caption);
                await messageDialog.ShowAsync();
                this.PizzaOrderDate.Focus(FocusState.Programmatic);

                return;
            }
            else if(orderDate.Month < currentDate.Month)
            {
                message = "Order Date cannot be past date! !";
                messageDialog = new MessageDialog(message, caption);
                await messageDialog.ShowAsync();
                this.PizzaOrderDate.Focus(FocusState.Programmatic);

                return;
            }
            else if(orderDate.Day < currentDate.Day)
            {
                message = "Order Date cannot be past date! !";
                messageDialog = new MessageDialog(message, caption);
                await messageDialog.ShowAsync();
                this.PizzaOrderDate.Focus(FocusState.Programmatic);

                return;
            }
            if (tbPhoneNumber.Text == "")
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
            temp.OrderDate = orderDate;
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
                //temp.ToppingsTest[toppingIndex++] = "Pepperoni";
            }
            if(chkMushrooms.IsChecked == true)
            {
                //temp.selectedToppings.Add("Mushrooms", 1.5);
                temp.Toppings +=" Mushrooms";
                //temp.ToppingsTest[toppingIndex++] = "Mushrooms";
            }
             if (chkOnion.IsChecked == true)
            {
                //temp.selectedToppings.Add("Onion", 1.5);
                temp.Toppings += " Onion";
                //temp.ToppingsTest[toppingIndex++] = "Onion";
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
            if (chkGreenPeppers.IsChecked == true)
            {
                //temp.selectedToppings.Add("Black Olives", 1.5);
                temp.Toppings += " Green Peppers";
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
            temp.Price = currentPizzaPrice;
            dataGridViewOrder.ItemsSource = null;
            pizzaOrderList.Add(temp);
            dataGridViewOrder.ItemsSource = pizzaOrderList;


            totalAmount = 0;
            discountAmount = 0;
            grandTotalAmount = 0;
            foreach(PizzaOrder item in pizzaOrderList)
            {
                totalAmount += item.Price;
            }
            discountAmount = 0.0;
            grandTotalAmount = totalAmount - discountAmount;
            TextBlock_TotalAmount.Text = "Total = " + totalAmount.ToString("0.00");
            TextBlock_DiscountAmount.Text = "Discount = " + discountAmount.ToString("0.00");
            TextBlock_GrandTotalAmount.Text = "Grand Total = " + grandTotalAmount.ToString("0.00");

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

            currentPizzaPrice = currentPizzaBasePrice + currentToppingPrice;
            TextBlock_PizzaPrice.Text = "Price = " + (currentPizzaPrice).ToString("0.00");

           

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

            currentPizzaPrice = currentPizzaBasePrice + currentToppingPrice;
            TextBlock_PizzaPrice.Text = "Price = " + (currentPizzaPrice).ToString("0.00");
        }

        private void rbLarge_Click(object sender, RoutedEventArgs e)
        {
            double rate = FindPizzaRate(3);
            currentPizzaBasePrice = rate;
            TextBlock_BasePrice.Text = "Base Price = " + currentPizzaBasePrice.ToString("0.00");

            ///TextBlock_PizzaPrice.Text = "Price = " + (currentPizzaBasePrice + currentToppingPrice).ToString("0.00");
            /// currentPizzaPrice = currentPizzaBasePrice + currentToppingPrice;
            currentPizzaPrice = currentPizzaBasePrice + currentToppingPrice;
            TextBlock_PizzaPrice.Text = "Price = " + (currentPizzaPrice).ToString("0.00");
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

            //TextBlock_PizzaPrice.Text = "Price = " + (currentPizzaBasePrice + currentToppingPrice).ToString("0.00");
            currentPizzaPrice = currentPizzaBasePrice + currentToppingPrice;
            TextBlock_PizzaPrice.Text = "Price = " + (currentPizzaPrice).ToString("0.00");
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

            //TextBlock_PizzaPrice.Text = "Price = " + (currentPizzaBasePrice + currentToppingPrice).ToString("0.00");
            currentPizzaPrice = currentPizzaBasePrice + currentToppingPrice;
            TextBlock_PizzaPrice.Text = "Price = " + (currentPizzaPrice).ToString("0.00");
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

            //TextBlock_PizzaPrice.Text = "Price = " + (currentPizzaBasePrice + currentToppingPrice).ToString("0.00");
            currentPizzaPrice = currentPizzaBasePrice + currentToppingPrice;
            TextBlock_PizzaPrice.Text = "Price = " + (currentPizzaPrice).ToString("0.00");
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

            //TextBlock_PizzaPrice.Text = "Price = " + (currentPizzaBasePrice + currentToppingPrice).ToString("0.00");
            currentPizzaPrice = currentPizzaBasePrice + currentToppingPrice;
            TextBlock_PizzaPrice.Text = "Price = " + (currentPizzaPrice).ToString("0.00");
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

            //TextBlock_PizzaPrice.Text = "Price = " + (currentPizzaBasePrice + currentToppingPrice).ToString("0.00");
            currentPizzaPrice = currentPizzaBasePrice + currentToppingPrice;
            TextBlock_PizzaPrice.Text = "Price = " + (currentPizzaPrice).ToString("0.00");
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

            //TextBlock_PizzaPrice.Text = "Price = " + (currentPizzaBasePrice + currentToppingPrice).ToString("0.00");
            currentPizzaPrice = currentPizzaBasePrice + currentToppingPrice;
            TextBlock_PizzaPrice.Text = "Price = " + (currentPizzaPrice).ToString("0.00");
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

            //TextBlock_PizzaPrice.Text = "Price = " + (currentPizzaBasePrice + currentToppingPrice).ToString("0.00");
            currentPizzaPrice = currentPizzaBasePrice + currentToppingPrice;
            TextBlock_PizzaPrice.Text = "Price = " + (currentPizzaPrice).ToString("0.00");
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

            //TextBlock_PizzaPrice.Text = "Price = " + (currentPizzaBasePrice + currentToppingPrice).ToString("0.00");
            currentPizzaPrice = currentPizzaBasePrice + currentToppingPrice;
            TextBlock_PizzaPrice.Text = "Price = " + (currentPizzaPrice).ToString("0.00");
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

            //TextBlock_PizzaPrice.Text = "Price = " + (currentPizzaBasePrice + currentToppingPrice).ToString("0.00");
            currentPizzaPrice = currentPizzaBasePrice + currentToppingPrice;
            TextBlock_PizzaPrice.Text = "Price = " + (currentPizzaPrice).ToString("0.00");
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

            //TextBlock_PizzaPrice.Text = "Price = " + (currentPizzaBasePrice + currentToppingPrice).ToString("0.00");
            currentPizzaPrice = currentPizzaBasePrice + currentToppingPrice;
            TextBlock_PizzaPrice.Text = "Price = " + (currentPizzaPrice).ToString("0.00");
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

            //TextBlock_PizzaPrice.Text = "Price = " + (currentPizzaBasePrice + currentToppingPrice).ToString("0.00");
            currentPizzaPrice = currentPizzaBasePrice + currentToppingPrice;
            TextBlock_PizzaPrice.Text = "Price = " + (currentPizzaPrice).ToString("0.00");
        }

        private void btnSubmitOrder_Click(object sender, RoutedEventArgs e)
        {
            receiptReadyToPrint = true;
            btnPrintBill.IsEnabled = true;
            btnSubmitOrder.IsEnabled = false;
            btnNewOrder.IsEnabled = true;
            btnAdd.IsEnabled = false;
            btnUpdate.IsEnabled = false;
            btnDelete.IsEnabled = false;
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            int index = -1;
             index = dataGridViewOrder.SelectedIndex;
            pizzaOrderList.RemoveAt(index);
            int sn = 1;
            foreach(PizzaOrder item in pizzaOrderList)
            {
                item.SerialNumber = sn;
                sn++;
            }
            
            dataGridViewOrder.ItemsSource = null;
            dataGridViewOrder.ItemsSource = pizzaOrderList;


            totalAmount = 0;
            discountAmount = 0;
            grandTotalAmount = 0;
            foreach (PizzaOrder item in pizzaOrderList)
            {
                totalAmount += item.Price;
            }
            discountAmount = 0.0;
            grandTotalAmount = totalAmount - discountAmount;
            TextBlock_TotalAmount.Text = "Total = " + totalAmount.ToString("0.00");
            TextBlock_DiscountAmount.Text = "Discount = " + discountAmount.ToString("0.00");
            TextBlock_GrandTotalAmount.Text = "Grand Total = " + grandTotalAmount.ToString("0.00");

            //messageDialog = new MessageDialog(index.ToString(), "select index");
            //await messageDialog.ShowAsync();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var orderUpdate = pizzaOrderList.Single(tempOrder => tempOrder.SerialNumber == updateId);

            orderUpdate.CustomerId = Convert.ToInt32( tbCustomerId.Text);
            orderUpdate.CustomerName = tbCustomerName.Text;
            if(chkDelivery.IsChecked == true)
            {
                orderUpdate.IsDelivery = true;
            }
            else
            {
                orderUpdate.IsDelivery = false;
            }

            orderUpdate.PhoneNumber = tbPhoneNumber.Text;
            orderUpdate.DeliveryAddress = tbDeliveryAddress.Text;

            if(rbSmall.IsChecked == true)
            {
                orderUpdate.Size = "Small";
            }
            else if(rbMedium.IsChecked == true)
            {
                orderUpdate.Size = "Medium";
            }
            else if(rbLarge.IsChecked == true)
            {
                orderUpdate.Size = "Large";
            }

            //update Toppings
            orderUpdate.Toppings = "";
            //Toppings
            if (chkPepperoni.IsChecked == true)
            {
                //temp.selectedToppings.Add("Pepperoni", 1.5);
                orderUpdate.Toppings = "Pepperoni";
                //temp.ToppingsTest[toppingIndex++] = "Pepperoni";
            }
            if (chkMushrooms.IsChecked == true)
            {
                //temp.selectedToppings.Add("Mushrooms", 1.5);
                orderUpdate.Toppings += " Mushrooms";
                //temp.ToppingsTest[toppingIndex++] = "Mushrooms";
            }
            if (chkOnion.IsChecked == true)
            {
                //temp.selectedToppings.Add("Onion", 1.5);
                orderUpdate.Toppings += " Onion";
                //temp.ToppingsTest[toppingIndex++] = "Onion";
            }
            if (chkSausage.IsChecked == true)
            {
                //temp.selectedToppings.Add("Sausage", 1.5);
                orderUpdate.Toppings += " Sausage";
            }
            if (chkBacon.IsChecked == true)
            {
                //temp.selectedToppings.Add("Bacon", 1.5);
                orderUpdate.Toppings += " Bacon";
            }
            if (chkExtraCheese.IsChecked == true)
            {
                //temp.selectedToppings.Add("Extra Cheese", 1.5);
                orderUpdate.Toppings += " Extra Cheese";
            }
            if (chkBlackOlives.IsChecked == true)
            {
                //temp.selectedToppings.Add("Black Olives", 1.5);
                orderUpdate.Toppings += " Black Olives";
            }
            if (chkPineapple.IsChecked == true)
            {
                //temp.selectedToppings.Add("Pineapple", 1.5);
                orderUpdate.Toppings += " Pineapple";
            }
            if (chkSpiniach.IsChecked == true)
            {
                //temp.selectedToppings.Add("Spiniach", 1.5);
                orderUpdate.Toppings += " Spiniach";
            }
            if (chkBroccoli.IsChecked == true)
            {
                //temp.selectedToppings.Add("Broccoli", 1.5);
                orderUpdate.Toppings += " Broccoli";
            }

            //clear Data Grid View
            dataGridViewOrder.ItemsSource = null;
            dataGridViewOrder.ItemsSource = pizzaOrderList;





            totalAmount = 0;
            discountAmount = 0;
            grandTotalAmount = 0;
            foreach (PizzaOrder item in pizzaOrderList)
            {
                totalAmount += item.Price;
            }
            discountAmount = 0.0;
            grandTotalAmount = totalAmount - discountAmount;
            TextBlock_TotalAmount.Text = "Total = " + totalAmount.ToString("0.00");
            TextBlock_DiscountAmount.Text = "Discount = " + discountAmount.ToString("0.00");
            TextBlock_GrandTotalAmount.Text = "Grand Total = " + grandTotalAmount.ToString("0.00");
        }

        private async  void dataGridViewOrder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(dataGridViewOrder != null)
            {
                int index = -1;
                PizzaOrder current = new PizzaOrder();
                current = (PizzaOrder)dataGridViewOrder.SelectedItem;
                if(current != null)
                {
                    updateId = current.SerialNumber;
                    //string testid = current.CustomerId.ToString();
                    //messageDialog = new MessageDialog(testid, "cid");
                    //await messageDialog.ShowAsync();

                    //pizzaOrderList.RemoveAt(index);
                    if (current.CustomerId == 0)
                    {
                        tbCustomerId.Text = current.CustomerId.ToString();
                    }
                    else
                    {
                        tbCustomerId.Text = "";
                    }

                    if (current.CustomerName != null)
                    {
                        tbCustomerName.Text = current.CustomerName;
                    }
                            
                    if (current.IsDelivery == true)
                    {
                        chkDelivery.IsChecked = true;
                    }
                    else
                    {
                        chkDelivery.IsChecked = false;
                    }
                    if(current.PhoneNumber != null)
                    {
                        tbPhoneNumber.Text = current.PhoneNumber;
                    }
                
                    if(current.DeliveryAddress != null)
                    {
                        tbDeliveryAddress.Text = current.DeliveryAddress;
                    }
                

                    if (current.Size == "Small")
                    {
                        rbSmall.IsChecked = true;
                    }
                    else if (current.Size == "Medium")
                    {
                        rbMedium.IsChecked = true;
                    }
                    else if (current.Size == "Large")
                    {
                        rbLarge.IsChecked = true;
                    }             
                    //UncheckAllCheckBoxes();
                    //string[] toppings = current.Toppings.Split(" ");

                    //foreach (string data1 in toppings)
                    //{
                    //    //messageDialog = new MessageDialog(data1, "aaa");
                    //    //await messageDialog.ShowAsync();
                    //    if (data1 == "Pepperoni")
                    //    {
                    //        chkPepperoni.IsChecked = true;
                    //    }
                    //    else if(data1 == "Mushrooms")
                    //    {
                    //        chkMushrooms.IsChecked = true;
                    //    }
                    //    else if (data1 == "Onion")
                    //    {
                    //        chkOnion.IsChecked = true;
                    //    }
                    //    else if (data1 == "Sausage")
                    //    {
                    //        chkSausage.IsChecked = true;
                    //    }
                    //    else if (data1 == "Bacon")
                    //    {
                    //        chkBacon.IsChecked = true;
                    //    }
                    //    else if (data1 == "Extra")
                    //    {
                    //        chkExtraCheese.IsChecked = true;
                    //    }
                    //    else if (data1 == "Black")
                    //    {
                    //        chkBlackOlives.IsChecked = true;
                    //    }
                    //    else if (data1 == "Green")
                    //    {
                    //        chkGreenPeppers.IsChecked = true;
                    //    }
                    //    else if (data1 == "Pineapple")
                    //    {
                    //        chkPineapple.IsChecked = true;
                    //    }
                    //    else if (data1 == "Spiniach")
                    //    {
                    //        chkSpiniach.IsChecked = true;
                    //    }
                    //    else if (data1 == "Broccoli")
                    //    {
                    //        chkBroccoli.IsChecked = true;
                    //    }

                    //}
                    CalculatePriceOfSelectedOrder(current);

                }
            }
        }

        private void UncheckAllCheckBoxes()
        {
            chkPepperoni.IsChecked = false;
            chkMushrooms.IsChecked = false;
            chkOnion.IsChecked = false;
            chkBacon.IsChecked = false;
            chkSausage.IsChecked = false;
            chkExtraCheese.IsChecked = false;
            chkBlackOlives.IsChecked = false;
            chkGreenPeppers.IsChecked = false;
            chkPineapple.IsChecked = false;
            chkSpiniach.IsChecked = false;
            chkBroccoli.IsChecked = false;
            chkOnion.IsChecked = false;
        }

        private void CalculatePriceOfSelectedOrder(PizzaOrder current)
        {
            double rate = 0;
           

            if (current.Size == "Small")
            {
                rate =  FindPizzaRate(1);
            }
            else if(current.Size == "Medium")
            {
                rate = FindPizzaRate(2);
            }
            else if(current.Size == "Large")
            {
                rate = FindPizzaRate(3);
            }
            currentPizzaBasePrice = rate;

            
            UncheckAllCheckBoxes();

            //Calculate the topping rate
            string[] toppings = current.Toppings.Split(" ");

            
            currentToppingPrice = 0;
            foreach (string data1 in toppings)
            {
                double tRate = 0;
                //messageDialog = new MessageDialog(data1, "aaa");
                //await messageDialog.ShowAsync();
                if (data1 == "Pepperoni")
                {
                    chkPepperoni.IsChecked = true;
                    tRate = FindToppingRate(1);
                }
                else if (data1 == "Mushrooms")
                {
                    chkMushrooms.IsChecked = true;
                    tRate = FindToppingRate(2);
                }
                else if (data1 == "Onion")
                {
                    chkOnion.IsChecked = true;
                    tRate = FindToppingRate(3);
                }
                else if (data1 == "Sausage")
                {
                    chkSausage.IsChecked = true;
                    tRate = FindToppingRate(4);
                }
                else if (data1 == "Bacon")
                {
                    chkBacon.IsChecked = true;
                    tRate = FindToppingRate(5);
                }
                else if (data1 == "Extra")
                {
                    chkExtraCheese.IsChecked = true;
                    tRate = FindToppingRate(6);
                }
                else if (data1 == "Black")
                {
                    chkBlackOlives.IsChecked = true;
                    tRate = FindToppingRate(7);
                }
                else if (data1 == "Green")
                {
                    chkGreenPeppers.IsChecked = true;
                    tRate = FindToppingRate(8);
                }
                else if (data1 == "Pineapple")
                {
                    chkPineapple.IsChecked = true;
                    tRate = FindToppingRate(9);
                }
                else if (data1 == "Spiniach")
                {
                    chkSpiniach.IsChecked = true;
                    tRate = FindToppingRate(10);
                }
                else if (data1 == "Broccoli")
                {
                    chkBroccoli.IsChecked = true;
                    tRate = FindToppingRate(11);
                }
                currentToppingPrice += tRate;
            }


            TextBlock_BasePrice.Text = "Base Price = " + currentPizzaBasePrice.ToString("0.00");
            TextBlock_ToppingPrice.Text = "Topping Price = " + currentToppingPrice.ToString("0.00");

            currentPizzaPrice = currentPizzaBasePrice + currentToppingPrice;
            TextBlock_PizzaPrice.Text = "Price = " + (currentPizzaPrice).ToString("0.00");

        }
    }
}
