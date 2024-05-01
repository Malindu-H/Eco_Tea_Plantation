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
    public partial class threepanel : Form
    {
        private const string connectionString = "Data Source=.;Initial Catalog=plant;Integrated Security=True";

        public threepanel()
        {
            InitializeComponent();
        }

        private void threepanel_Load(object sender, EventArgs e)
        {
            LoadEmployeeIDs();
        }
        private void LoadEmployeeIDs()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT DISTINCT EmployeeID FROM Attendance"; // Query to retrieve distinct employee IDs

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
        private string GetEmployeeName(string employeeID)
        {
            string employeeName = string.Empty;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT FirstName, LastName FROM Employee WHERE EmployeeID = @EmployeeID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@EmployeeID", employeeID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string firstName = reader["FirstName"].ToString();
                                string lastName = reader["LastName"].ToString();
                                employeeName = firstName + " " + lastName;
                            }
                            else
                            {
                                employeeName = "Employee not found";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return employeeName;
        }

        private void loaddata_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedEmployeeID = loaddata.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedEmployeeID))
            {
                MessageBox.Show("Please select an employee.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string employeeName = GetEmployeeName(selectedEmployeeID);
            emname.Text = employeeName;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT WorkType FROM Attendance WHERE EmployeeID = @EmployeeID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@EmployeeID", selectedEmployeeID);

                        object workTypeResult = command.ExecuteScalar();
                        if (workTypeResult != null && workTypeResult != DBNull.Value)
                        {
                            string workType = workTypeResult.ToString();
                            // Display the work type in a TextBox or any other control
                            type.Text = workType;

                            // Disable weight TextBox if work type is "Laborer"
                            if (workType.Equals("Laborer", StringComparison.OrdinalIgnoreCase))
                            {
                                weight.Enabled = false;
                                weight.Text = string.Empty; // Clear the text if it's disabled

                                // Enable day TextBox if it was disabled previously
                                day.Enabled = true;
                            }
                            else if (workType.Equals("Leaf Picker", StringComparison.OrdinalIgnoreCase))
                            {
                                // Disable day TextBox if work type is "Leaf Picker"
                                day.Enabled = false;
                                day.Text = string.Empty; // Clear the text if it's disabled

                                // Enable weight TextBox if it was disabled previously
                                weight.Enabled = true;
                            }
                            else
                            {
                                // Enable both TextBoxes for other work types
                                day.Enabled = true;
                                weight.Enabled = true;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Work type not found for the selected employee.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }

        private void emname_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string selectedEmployeeID = loaddata.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedEmployeeID))
            {
                MessageBox.Show("Please select an employee.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (type.Text.Trim().Equals("Leaf Picker", StringComparison.OrdinalIgnoreCase))
            {
                string leafWeightInput = weight.Text.Trim();
                if (string.IsNullOrEmpty(leafWeightInput))
                {
                    MessageBox.Show("Please enter the leaf weight.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        // Retrieve the current leaf weight of the selected employee
                        string getCurrentLeafWeightQuery = "SELECT LeafWeight FROM Employee WHERE EmployeeID = @EmployeeID";
                        using (SqlCommand getCurrentLeafWeightCmd = new SqlCommand(getCurrentLeafWeightQuery, connection))
                        {
                            getCurrentLeafWeightCmd.Parameters.AddWithValue("@EmployeeID", selectedEmployeeID);
                            object currentLeafWeightResult = getCurrentLeafWeightCmd.ExecuteScalar();

                            if (currentLeafWeightResult != null && currentLeafWeightResult != DBNull.Value)
                            {
                                decimal currentLeafWeight = Convert.ToDecimal(currentLeafWeightResult);
                                decimal inputLeafWeight = Convert.ToDecimal(leafWeightInput);

                                // Add the input leaf weight to the current leaf weight
                                decimal totalLeafWeight = currentLeafWeight + inputLeafWeight;

                                // Update the database with the new total leaf weight
                                string updateLeafWeightQuery = "UPDATE Employee SET LeafWeight = @TotalLeafWeight WHERE EmployeeID = @EmployeeID";
                                using (SqlCommand updateLeafWeightCmd = new SqlCommand(updateLeafWeightQuery, connection))
                                {
                                    updateLeafWeightCmd.Parameters.AddWithValue("@TotalLeafWeight", totalLeafWeight);
                                    updateLeafWeightCmd.Parameters.AddWithValue("@EmployeeID", selectedEmployeeID);

                                    int rowsAffected = updateLeafWeightCmd.ExecuteNonQuery();
                                    if (rowsAffected > 0)
                                    {
                                        MessageBox.Show("Leaf weight updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                    else
                                    {
                                        MessageBox.Show("Failed to update leaf weight.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Current leaf weight not found for the selected employee.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (type.Text.Trim().Equals("Laborer", StringComparison.OrdinalIgnoreCase))
            {
                string workDaysInput = day.Text.Trim();
                if (string.IsNullOrEmpty(workDaysInput))
                {
                    MessageBox.Show("Please enter the work days.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        // Retrieve the current work days of the selected employee
                        string getCurrentWorkDaysQuery = "SELECT WorkDays FROM Employee WHERE EmployeeID = @EmployeeID";
                        using (SqlCommand getCurrentWorkDaysCmd = new SqlCommand(getCurrentWorkDaysQuery, connection))
                        {
                            getCurrentWorkDaysCmd.Parameters.AddWithValue("@EmployeeID", selectedEmployeeID);
                            object currentWorkDaysResult = getCurrentWorkDaysCmd.ExecuteScalar();

                            if (currentWorkDaysResult != null && currentWorkDaysResult != DBNull.Value)
                            {
                                int currentWorkDays = Convert.ToInt32(currentWorkDaysResult);
                                int inputWorkDays = Convert.ToInt32(workDaysInput);

                                // Increment the input work days to the current work days
                                int totalWorkDays = currentWorkDays + inputWorkDays;

                                // Update the database with the new total work days
                                string updateWorkDaysQuery = "UPDATE Employee SET WorkDays = @TotalWorkDays WHERE EmployeeID = @EmployeeID";
                                using (SqlCommand updateWorkDaysCmd = new SqlCommand(updateWorkDaysQuery, connection))
                                {
                                    updateWorkDaysCmd.Parameters.AddWithValue("@TotalWorkDays", totalWorkDays);
                                    updateWorkDaysCmd.Parameters.AddWithValue("@EmployeeID", selectedEmployeeID);

                                    int rowsAffected = updateWorkDaysCmd.ExecuteNonQuery();
                                    if (rowsAffected > 0)
                                    {
                                        MessageBox.Show("Work days updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                    else
                                    {
                                        MessageBox.Show("Failed to update work days.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Current work days not found for the selected employee.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // Clear all fields after update
            type.Clear();
            weight.Clear();
            day.Clear();
            emname.Clear();
        }



        private void guna2HtmlLabel4_Click(object sender, EventArgs e)
        {

        }
    }
}