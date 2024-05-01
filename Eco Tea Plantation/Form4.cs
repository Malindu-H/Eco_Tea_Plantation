using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using ZXing;

namespace Eco_Tea_Plantation
{
    public partial class Form4 : Form
    {
        private VideoCaptureDevice videoSource;
        private FilterInfoCollection videoDevices;
        private BarcodeReader barcodeReader;
        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
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
                // Update PictureBox asynchronously if required
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
                        UpdateTime(); // Call method to update time when QR code is detected
                    }));
                }
                else
                {
                    textBoxQRCodeResult.Text = result.Text;
                    UpdateTime(); // Call method to update time when QR code is detected
                }
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.Stop();
            }
        }

        private void time_TextChanged(object sender, EventArgs e)
        {

        }
        private void UpdateTime()
        {
            // Update time TextBox with current time
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
    }
}
