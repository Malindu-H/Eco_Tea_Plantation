using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QRCoder;

namespace Eco_Tea_Plantation
{
    public partial class Form3 : Form
    {
        private bool isImageSaved = false;
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string textToEncode = textBoxTextToEncode.Text;

            // Check if the text is not empty
            if (!string.IsNullOrWhiteSpace(textToEncode))
            {
                // Generate QR code
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(textToEncode, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                Bitmap qrCodeImage = qrCode.GetGraphic(10); // Adjust the size of the QR code

                // Display the QR code in the PictureBox
                pictureBoxQRCode.Image = qrCodeImage;
            }
            else
            {
                MessageBox.Show("Please enter text to generate QR code.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void svbut_Click(object sender, EventArgs e)
        {
            if (!isImageSaved)
            {
                // Check if the QR code image is available in the PictureBox
                if (pictureBoxQRCode.Image != null)
                {
                    // Specify the file path where you want to save the image
                    string filePath = @"G:\QRCodeImage.jpg"; // Change the path as needed

                    try
                    {
                        // Save the QR code image as a JPG file directly to the specified path
                        pictureBoxQRCode.Image.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);

                        isImageSaved = true; // Mark the image as saved
                        MessageBox.Show("QR code image saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error saving QR code image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
