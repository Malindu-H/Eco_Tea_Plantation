using System;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;

namespace Eco_Tea_Plantation
{
    public partial class cam : Form
    {
        private VideoCaptureDevice videoSource;
        private FilterInfoCollection videoDevices;
        private string connectionString = "Data Source=.;Initial Catalog=plant;Integrated Security=True";

        public cam()
        {
            InitializeComponent();
        }

        private void cam_Load(object sender, EventArgs e)
        {
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count > 0)
            {
                videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
                videoSource.NewFrame += VideoSource_NewFrame;
                videoSource.Start();
            }
            else
            {
                MessageBox.Show("No webcam devices found.");
            }
        }

        private void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            pictureBoxPreview.Image = (Bitmap)eventArgs.Frame.Clone();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void SaveImageToDatabase(byte[] imageBytes)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Photos (ImageData) VALUES (@ImageData)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ImageData", imageBytes);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Photo saved to database.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving photo to database: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void cam_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.Stop();
            }
        }
    }
}
