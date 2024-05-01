using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using QRCoder;
using System.Drawing;

namespace Eco_Tea_Plantation
{
    public partial class employeeregi : Form
    {
        private bool isImageSaved = false;
        private const string connectionString = "Data Source=.;Initial Catalog=plant;Integrated Security=True";

        public employeeregi()
        {
            InitializeComponent();
            LoadLastEmployeeID();
            typeselect.Items.Add("Worker");
            typeselect.Items.Add("Field Officer");
            typeselect.Items.Add("Manager");
            typeselect.SelectedIndexChanged += typeselect_SelectedIndexChanged; // Add event handler for combobox
        }


        private void nextemregi_Click(object sender, EventArgs e)
        {
            string firstName = fname.Text;
            string lastName = lname.Text;
            string phoneNumber = phone.Text;
            string age = ageb.Text;
            string address = addressb.Text;
            string gender = GetSelectedGender(); // Get the selected gender from radio buttons
            string nicNumber = nicb.Text;
            string employeeType = typeselect.Text;
            string username = usname.Text;
            string password = pw.Text;
            int weight = 0;
            int days = 0;


            // Check if any of the input fields are empty
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(phoneNumber)
                || string.IsNullOrWhiteSpace(age) || string.IsNullOrWhiteSpace(address) || string.IsNullOrWhiteSpace(gender) || string.IsNullOrWhiteSpace(nicNumber)
                || string.IsNullOrWhiteSpace(employeeType))
            {
                MessageBox.Show("Please fill in all the fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // SQL INSERT statement
            string insertQuery = string.Empty;

            if (employeeType == "Worker")
            {
                // INSERT statement without username and password for Worker type
                insertQuery = "INSERT INTO Employee (FirstName, LastName, PhoneNumber, Age, Address, Gender, NICNumber, Type, QR, LeafWeight, WorkDays) " +
                              "VALUES (@FirstName, @LastName, @PhoneNumber, @Age, @Address, @Gender, @NICNumber, @EmployeeType, @QRCodeImage, @Weight, @Days)";
            }
            else
            {
                // INSERT statement including username and password for other types
                //insertQuery = "INSERT INTO Employee (FirstName, LastName, PhoneNumber, Age, Address, Gender, NICNumber, Type, Username, Password) " +
                //             "VALUES (@FirstName, @LastName, @PhoneNumber, @Age, @Address, @Gender, @NICNumber, @EmployeeType, @Username, @Password)";
                insertQuery = "INSERT INTO Employee (FirstName, LastName, PhoneNumber, Age, Address, Gender, NICNumber, Type, Username, Password, QR, LeafWeight, WorkDays) " +
                       "VALUES (@FirstName, @LastName, @PhoneNumber, @Age, @Address, @Gender, @NICNumber, @EmployeeType, @Username, @Password, @QRCodeImage, @Weight, @Days)";
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Generate QR code
                    QRCodeGenerator qrGenerator = new QRCodeGenerator();
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(label1.Text, QRCodeGenerator.ECCLevel.Q);
                    QRCode qrCode = new QRCode(qrCodeData);
                    Bitmap qrCodeImage = qrCode.GetGraphic(10); // Adjust the size of the QR code

                    // Convert the QR code image to a byte array
                    byte[] qrImageBytes = ImageToByteArray(qrCodeImage);

                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        // Add parameters to the SQL command to prevent SQL injection
                        command.Parameters.AddWithValue("@FirstName", firstName);
                        command.Parameters.AddWithValue("@LastName", lastName);
                        command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                        command.Parameters.AddWithValue("@Age", age);
                        command.Parameters.AddWithValue("@Address", address);
                        command.Parameters.AddWithValue("@Gender", gender);
                        command.Parameters.AddWithValue("@NICNumber", nicNumber);
                        command.Parameters.AddWithValue("@EmployeeType", employeeType);
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password);
                        command.Parameters.AddWithValue("@QRCodeImage", qrImageBytes);
                        command.Parameters.AddWithValue("@Weight", weight);
                        command.Parameters.AddWithValue("@Days", days);

                        // Execute the SQL command
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Employee data inserted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Clear textboxes after successful insertion
                            fname.Clear();
                            lname.Clear();
                            phone.Clear();
                            ageb.Clear();
                            addressb.Clear();
                            ClearGenderSelection(); // Clear radio button selection
                            nicb.Clear();
                            typeselect.SelectedIndex = -1; // Clear ComboBox selection
                            usname.Clear();
                            pw.Clear();
                            pictureBoxQRCode.Image = null; // Clear the QR code image
                        }
                        else
                        {
                            MessageBox.Show("Failed to insert employee data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
        private byte[] ImageToByteArray(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png); // You can choose the image format here (e.g., Png, Jpeg, etc.)
                return ms.ToArray();
            }
        }


        private string GetSelectedGender()
        {
            if (maler.Checked)
                return "Male";
            else if (femalelr.Checked)
                return "Female";
            else
                return string.Empty;
        }

        private void ClearGenderSelection()
        {
            maler.Checked = false;
            femalelr.Checked = false;
        }

        private void typeselect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (typeselect.Text == "Worker")
            {
                // Disable username and password textboxes for Worker type
                usname.Enabled = false;
                pw.Enabled = false;
            }
            else
            {
                // Enable username and password textboxes for other types
                usname.Enabled = true;
                pw.Enabled = true;
            }
        }
        private void LoadLastEmployeeID()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL query to get the last EmployeeID
                    string query = "SELECT TOP 1 EmployeeID FROM Employee ORDER BY EmployeeID DESC";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int lastEmployeeID))
                        {
                            int nextEmployeeID = lastEmployeeID + 1;
                            label1.Text = nextEmployeeID.ToString();
                        }
                        else
                        {
                            // If no EmployeeID is found, start from EmployeeID = 1
                            label1.Text = "1";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void guna2TextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void qr_Click(object sender, EventArgs e)
        {
            string textToEncode = label1.Text;

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

        private void employeeregi_Load(object sender, EventArgs e)
        {

        }
    }
}