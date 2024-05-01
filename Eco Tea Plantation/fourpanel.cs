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
    public partial class fourpanel : Form
    {
        private const string connectionString = "Data Source=.;Initial Catalog=plant;Integrated Security=True";

        public fourpanel()
        {
            InitializeComponent();
        }

        private void guna2HtmlLabel2_Click(object sender, EventArgs e)
        {

        }

        private void fourpanel_Load(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT EmployeeID FROM Employee"; // Query to retrieve employee IDs from Employee table

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string employeeID = reader["EmployeeID"].ToString();
                                loaddata.Items.Add(employeeID); // Add employee IDs to ComboBox
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void loaddata_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedEmployeeID = loaddata.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedEmployeeID))
            {
                MessageBox.Show("Please select an employee.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Retrieve employee name, leaf weight, and work days based on selected employee ID
                    string query = "SELECT FirstName, LastName, LeafWeight, WorkDays FROM Employee WHERE EmployeeID = @EmployeeID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@EmployeeID", selectedEmployeeID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string firstName = reader["FirstName"].ToString();
                                string lastName = reader["LastName"].ToString();
                                string leafWeight = reader["LeafWeight"].ToString();
                                string workDays = reader["WorkDays"].ToString();

                                // Display employee name in text box
                                emname.Text = firstName + " " + lastName;

                                if (string.IsNullOrEmpty(leafWeight) || leafWeight == "0")
                                {
                                    // Leaf weight is null or 0, disable weight and price textboxes, enable days and payment textboxes
                                    weight.Enabled = false;
                                    price.Enabled = false;
                                    days.Enabled = true;
                                    payment.Enabled = true;
                                    days.Text = workDays;
                                    weight.Clear();
                                    price.Clear();
                                }
                                else if (string.IsNullOrEmpty(workDays) || workDays == "0")
                                {
                                    // Work days is null or 0, disable days and payment textboxes, enable weight and price textboxes
                                    days.Enabled = false;
                                    payment.Enabled = false;
                                    weight.Enabled = true;
                                    price.Enabled = true;
                                    weight.Text = leafWeight;
                                    days.Clear();
                                    payment.Clear();
                                }
                                else
                                {
                                    // Neither leaf weight nor work days is 0 or null, enable all textboxes and load values
                                    weight.Enabled = true;
                                    price.Enabled = true;
                                    days.Enabled = true;
                                    payment.Enabled = true;
                                    weight.Text = leafWeight;
                                    days.Text = workDays;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Employee not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cal_Click(object sender, EventArgs e)
        {
            decimal salary = 0;
            decimal leafWeight = 0;
            decimal unitPrice = 0;
            decimal workDays = 0;
            decimal dayPayment = 0;

            // Check if leaf weight and unit price are enabled and valid
            if (weight.Enabled && decimal.TryParse(weight.Text.Trim(), out leafWeight) &&
                price.Enabled && decimal.TryParse(price.Text.Trim(), out unitPrice))
            {
                // Calculate salary based on leaf weight and unit price
                salary = leafWeight * unitPrice;
            }
            // Check if work days and day payment are enabled and valid
            else if (days.Enabled && decimal.TryParse(days.Text.Trim(), out workDays) &&
                     payment.Enabled && decimal.TryParse(payment.Text.Trim(), out dayPayment))
            {
                // Calculate salary based on work days and day payment
                salary = workDays * dayPayment;
            }
            else
            {
                MessageBox.Show("Please enter valid input for salary calculation.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Display the calculated salary in the salaryTextBox
            salaryb.Text = salary.ToString();
        }



        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string selectedEmployeeID = loaddata.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedEmployeeID))
            {
                MessageBox.Show("Please select an employee.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!decimal.TryParse(salaryb.Text.Trim(), out decimal newSalary))
            {
                MessageBox.Show("Please enter a valid salary.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Update the salary in the Employee table
                    string updateSalaryQuery = "UPDATE Employee SET Salary = @Salary WHERE EmployeeID = @EmployeeID";
                    using (SqlCommand command = new SqlCommand(updateSalaryQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Salary", newSalary);
                        command.Parameters.AddWithValue("@EmployeeID", selectedEmployeeID);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Salary updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Failed to update salary.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
