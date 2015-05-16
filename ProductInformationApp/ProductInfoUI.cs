using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProductInformationApp
{
    public partial class ProductInfoUI : Form
    {
        public ProductInfoUI()
        {
            InitializeComponent();
        }
        string connectionString =
                 ConfigurationManager.ConnectionStrings["ProductDB"].ConnectionString;

        private void saveButton_Click(object sender, EventArgs e)
        {


            if (productCodeTextBox.Text == "" || descriptionTextBox.Text == "" || quantityTextBox.Text == "")
            {
                MessageBox.Show("Please enter all values");
            }
            else
            {
                string productCode = productCodeTextBox.Text;
                string description = descriptionTextBox.Text;
                int quantity = Convert.ToInt32(quantityTextBox.Text);

                if (IsCodeAlreadyExists(productCode))
                {
                    if (quantity > -1)
                    {
                        SqlConnection connection = new SqlConnection(connectionString);

                        string query = "UPDATE Products SET Quantity=(Quantity +'" + quantity + "')";

                        SqlCommand command = new SqlCommand(query, connection);

                        connection.Open();
                        int rowAffected = command.ExecuteNonQuery();
                        connection.Close();

                        if (rowAffected > 0)
                        {
                            MessageBox.Show("Saved Successfully!");
                        }
                        else
                        {
                            MessageBox.Show("Saving failed");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Quatity must be positive number");
                    }


                }
                else
                {




                    SqlConnection connection = new SqlConnection(connectionString);
                    if (productCode.Length <= 3 && quantity > -1)
                    {
                        string query = "INSERT INTO Products Values('" + productCode + "','" + description + "','" +
                                       quantity + "')";
                        SqlCommand command = new SqlCommand(query, connection);
                        connection.Open();
                        int rowAffected = command.ExecuteNonQuery();
                        connection.Close();
                        if (rowAffected > 0)
                        {
                            MessageBox.Show("Product information is Saved");
                        }
                        else
                        {
                            MessageBox.Show("Saving failed");
                        }



                    }
                    else
                    {
                        MessageBox.Show("Protuct code must 3 character long and quantity must be positive");
                    }

                }
            }
            productCodeTextBox.Clear();
            descriptionTextBox.Clear();
            quantityTextBox.Clear();
        }

        public bool IsCodeAlreadyExists(string productCode)
        {
            SqlConnection connection=new SqlConnection(connectionString);
            string query="SELECT * FROM Products Where Pcode='"+productCode+"'";
            bool isProductCodeExists = false;
             SqlCommand command=new SqlCommand(query,connection);
             connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            while(reader.Read())
            {
                isProductCodeExists = true;
                 break;
            }
            reader.Close();
            connection.Close();
            return isProductCodeExists;




        }

        private void showButton_Click(object sender, EventArgs e)
        {
            displayListView.Items.Clear();
           SqlConnection connection=new SqlConnection(connectionString);
            string query = "SELECT * FROM Products";

            SqlCommand command=new SqlCommand(query,connection);
             connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                ListViewItem item=new ListViewItem(reader["Pcode"].ToString());
               
                item.SubItems.Add(reader["Description"].ToString());
                item.SubItems.Add(reader["Quantity"].ToString());
                displayListView.Items.Add(item);

            }
            reader.Close();
            connection.Close();
            totalQuantityTextBox.Text = SumOfQuantity().ToString();

        }

        public int SumOfQuantity()
        {
            int sumOfQuantity;
            SqlConnection connection=new SqlConnection(connectionString);
            string query = "SELECT SUM(Quantity) FROM Products";

            SqlCommand command=new SqlCommand(query,connection);
             connection.Open();
            sumOfQuantity =Convert.ToInt32(command.ExecuteScalar());
            return sumOfQuantity;

        
        }

        
       

       
    }
}
