using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using ZXing;

namespace Eco_Tea_Plantation
{
    public partial class twopanel : Form
    {
        private VideoCaptureDevice videoSource;
        private FilterInfoCollection videoDevices;
        private BarcodeReader barcodeReader;
        private const string connectionString = "Data Source=.;Initial Catalog=plant;Integrated Security=True";

        public twopanel()
        {
            InitializeComponent();
        }

        private void twopanel_Load(object sender, EventArgs e)
        {
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count > 0)
            {
                videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
                videoSource.NewFrame += VideoSource_NewFrame;
                videoSource.Start();

                // Initialize barcode reader
                barcodeReader = new BarcodeReader();
            }
            else
            {
                MessageBox.Show("No webcam devices found.");
            }
        }

        private void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            if (pictureBoxPreview.InvokeRequired)
            {
                pictureBoxPreview.Invoke(new Action(() =>
                {
                    pictureBoxPreview.Image = (System.Drawing.Image)eventArgs.Frame.Clone();
                }));
            }
            else
            {
                pictureBoxPreview.Image = (System.Drawing.Image)eventArgs.Frame.Clone();
            }

            // Try decoding QR code from the current frame
            Result result = barcodeReader.Decode((Bitmap)eventArgs.Frame.Clone());
            if (result != null)
            {
                // Update TextBox asynchronously if required
                if (textBoxQRCodeResult.InvokeRequired)
                {
                    textBoxQRCodeResult.Invoke(new Action(() =>
                    {
                        textBoxQRCodeResult.Text = result.Text;
                        // Call method to check employee name using the scanned employee ID
                        CheckEmployeeName(result.Text);
                        UpdateTime();
                    }));
                }
                else
                {
                    textBoxQRCodeResult.Text = result.Text;
                    CheckEmployeeName(result.Text);
                    UpdateTime();
                }
            }
        }
        private void guna2HtmlLabel8_Click(object sender, EventArgs e)
        {

        }

        private void CheckEmployeeName(string employeeID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL query to retrieve employee name based on employee ID
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
                                emname.Text = firstName + " " + lastName;
                            }
                            else
                            {
                                emname.Text = "Employee not found";
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

        private void UpdateTime()
        {
            if (time.InvokeRequired)
            {
                time.Invoke(new Action(() =>
                {
                    time.Text = DateTime.Now.ToString("HH:mm:ss");
                }));
            }
            else
            {
                time.Text = DateTime.Now.ToString("HH:mm:ss");
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.Stop();
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string employeeID = textBoxQRCodeResult.Text; // Assuming textBoxQRCodeResult contains the scanned employee ID
            string attendanceTime = DateTime.Now.ToString("HH:mm:ss");
            string workType = guna2ComboBox1.SelectedItem?.ToString();

            string[] employeeInfo = CheckEmployeeInfo(employeeID);

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL query to insert data into Attendance table without attendanceDate
                    string query = "INSERT INTO Attendance (EmployeeID, EmployeeName, AttendanceTime, WorkType) " +
                           "VALUES (@EmployeeID, @EmployeeName, @AttendanceTime, @WorkType)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@EmployeeID", employeeInfo[0]); // Employee ID from CheckEmployeeInfo
                        command.Parameters.AddWithValue("@EmployeeName", employeeInfo[1]); // Employee name from CheckEmployeeInfo
                        command.Parameters.AddWithValue("@AttendanceTime", attendanceTime);
                        command.Parameters.AddWithValue("@WorkType", workType);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Attendance data inserted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Failed to insert attendance data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
        
        private string[] CheckEmployeeInfo(string employeeID)
        {
            string[] employeeInfo = new string[2]; // Array to store employee ID and name

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL query to retrieve employee ID and name based on employee ID
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
                                employeeInfo[0] = employeeID; // Store employee ID
                                employeeInfo[1] = firstName + " " + lastName; // Store employee name
                            }
                            else
                            {
                                // Employee not found
                                employeeInfo[0] = "0"; // Placeholder for employee ID
                                employeeInfo[1] = "Employee not found";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return employeeInfo;
        }
    
    }
}
