using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace Eco_Tea_Plantation
{
    public partial class reports : Form
    {
        private const string connectionString = "Data Source=.;Initial Catalog=plant;Integrated Security=True";

        public reports()
        {
            InitializeComponent();
        }

        private void close_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void loadid_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Retrieve employee details based on selected employee ID
            string selectedEmployeeID = loadid.SelectedItem.ToString();
            LoadEmployeeData(selectedEmployeeID);
        }

        private void LoadEmployeeData(string employeeID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM Employee WHERE EmployeeID = @EmployeeID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@EmployeeID", employeeID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Assuming textBox1, textBox2, textBox3, and textBox4 are the TextBox controls
                                string topic = "ECO TEA PLANTATION";
                                string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
                                string firstName = reader["FirstName"].ToString();
                                string lastName = reader["LastName"].ToString();
                                string nid = reader["NICNumber"].ToString();
                                string type = reader["Type"].ToString();
                                string salary = reader["Salary"].ToString();
                                string leaf = reader["LeafWeight"].ToString();
                                string day = reader["WorkDays"].ToString();

                                // Create an image with text data
                                Bitmap bitmap = new Bitmap(pictureBox.Width, pictureBox.Height);
                                using (Graphics g = Graphics.FromImage(bitmap))
                                {
                                    g.Clear(Color.White);
                                    Font topicFont = new Font("Arial", 18, FontStyle.Bold);
                                    Font regularFont = new Font("Arial", 12, FontStyle.Regular);
                                    g.DrawString($"      {topic}",
                                        topicFont, Brushes.Black, new PointF(10, 10));
                                    g.DrawString($"\n\n\n                                                  Date: {currentDate}\n\n~ Employee Name: {firstName} {lastName}\n\n~ NIC Number: {nid}\n\n~ Position: {type}\n\n~ Leaf Weight: {leaf}\n\n~ Work Days: {day}\n\n~ Salary: {salary}\n\n\n\n\n\n.................................\nEco Tea Plantation\n\n                               Thank You!",
                                        new Font("Arial", 12), Brushes.Black, new PointF(10, 10));
                                }

                                // Set the PictureBox image to the dynamically created image
                                pictureBox.Image = bitmap;
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


        private void reports_Load(object sender, EventArgs e)
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

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if there is an image in pictureBox1 to print
                if (pictureBox.Image == null)
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
            Image img = pictureBox.Image;

            // Draw the image on the PrintPageEventArgs
            e.Graphics.DrawImage(img, e.PageBounds);

            // Optionally, you can add additional text or formatting here if needed
        }
    }
}
