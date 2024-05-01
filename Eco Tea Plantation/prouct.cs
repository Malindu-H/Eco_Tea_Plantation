using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Eco_Tea_Plantation
{
    public partial class prouct : Form
    {
        private const string connectionString = "Data Source=.;Initial Catalog=plant;Integrated Security=True";

        public prouct()
        {
            InitializeComponent();
        }

        private void addressb_TextChanged(object sender, EventArgs e)
        {

        }

        private void nextemregi_Click(object sender, EventArgs e)
        {
            string productName = fname.Text;
            string productCode = addressb.Text;
            int quantity = 0;
            decimal price = 0;

            // Validate quantity (must be a positive integer)
            if (!int.TryParse(phone.Text, out quantity) || quantity <= 0)
            {
                MessageBox.Show("Please enter a valid quantity.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validate price (must be a positive decimal)
            if (!decimal.TryParse(usname.Text, out price) || price <= 0)
            {
                MessageBox.Show("Please enter a valid price.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string insertQuery = "INSERT INTO Product (ProductName, ProductCode, Quantity, Price) VALUES (@ProductName, @ProductCode, @Quantity, @Price)";

                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@ProductName", productName);
                        command.Parameters.AddWithValue("@ProductCode", productCode);
                        command.Parameters.AddWithValue("@Quantity", quantity);
                        command.Parameters.AddWithValue("@Price", price);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Product details inserted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Clear the text boxes after successful insertion
                            fname.Clear();
                            addressb.Clear();
                            phone.Clear();
                            usname.Clear();
                        }
                        else
                        {
                            MessageBox.Show("Failed to insert product details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
