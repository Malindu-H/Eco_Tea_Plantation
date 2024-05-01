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
using System.Drawing.Printing;

namespace Eco_Tea_Plantation
{
    public partial class printid : Form
    {
        private const string connectionString = "Data Source=.;Initial Catalog=plant;Integrated Security=True";
        public printid()
        {
            InitializeComponent();
        }

        private void loadid_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedEmployeeID = loadid.SelectedItem?.ToString();
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

                    // Retrieve employee name and QR code image from Employee table based on selected employee ID
                    string query = "SELECT FirstName, LastName, QR FROM Employee WHERE EmployeeID = @EmployeeID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@EmployeeID", selectedEmployeeID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string firstName = reader["FirstName"].ToString();
                                string lastName = reader["LastName"].ToString();
                                byte[] qrCodeImageData = (byte[])reader["QR"];

                                // Display employee name in textbox
                                fname.Text = firstName + " " + lastName;

                                // Display QR code image
                                using (MemoryStream ms = new MemoryStream(qrCodeImageData))
                                {
                                    pictureBoxQRCode.Image = Image.FromStream(ms);
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


        private void printid_Load(object sender, EventArgs e)
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
                                loadid.Items.Add(employeeID); // Add employee IDs to ComboBox
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

        private void guna2HtmlLabel2_Click(object sender, EventArgs e)
        {

        }

        private void qr_Click(object sender, EventArgs e)
        {
            string selectedEmployeeID = loadid.SelectedItem?.ToString();

            // Check if an employee ID is selected
            if (string.IsNullOrWhiteSpace(selectedEmployeeID))
            {
                MessageBox.Show("Please select an Employee ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Create a new bitmap to hold the combined image
            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height); // Use pictureBox1 size

            // Draw the employee details and QR code image on the bitmap
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White); // Clear the background to white

                // Draw the employee details
                using (Font font = new Font("Arial", 12, FontStyle.Regular, GraphicsUnit.Pixel))
                {
                    SolidBrush brush = new SolidBrush(Color.Black);
                    g.DrawString("Employee ID: " + selectedEmployeeID + "\n\n", font, brush, 0, 0);
                    g.DrawString("Employee Name: " + fname.Text + "\n", font, brush, 0, 20);
                }

                // Calculate the position and size for the QR code to fit within pictureBox1
                int qrWidth = pictureBox1.Width - 20; // Leave some margin
                int qrHeight = pictureBox1.Height - 40; // Adjust height based on text size
                Rectangle qrRect = new Rectangle(10, 40, qrWidth, qrHeight); // Position and size for QR code

                // Draw the scaled QR code within the calculated rectangle
                Image qrCodeImage = pictureBoxQRCode.Image;
                g.DrawImage(qrCodeImage, qrRect); // Draw QR code within the calculated rectangle
            }

            // Assign the combined image to pictureBox1
            pictureBox1.Image = bmp;
        }



        private void guna2Button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if there is an image in pictureBox1 to print
                if (pictureBox1.Image == null)
                {
                    MessageBox.Show("No image to print.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Create a PrintDocument object
                PrintDocument pd = new PrintDocument();
                pd.PrintPage += new PrintPageEventHandler(PrintImage);

                // Display the Print Preview dialog
                PrintPreviewDialog printPreviewDialog1 = new PrintPreviewDialog();
                printPreviewDialog1.Document = pd;
                printPreviewDialog1.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintImage(object sender, PrintPageEventArgs e)
        {
            // Get the image from pictureBox1
            Image img = pictureBox1.Image;

            // Draw the image on the PrintPageEventArgs
            e.Graphics.DrawImage(img, e.PageBounds);

            // Optionally, you can add additional text or formatting here if needed
        }



        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            
        }

        private void printDocument1_PrintPage_1(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {

        }

        private void printDocument1_PrintPage_2(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {

        }

        private void printPreviewDialog1_Load(object sender, EventArgs e)
        {

        }
    }
}