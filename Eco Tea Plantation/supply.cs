using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Eco_Tea_Plantation
{
    public partial class supply : Form
    {
        private const string connectionString = "Data Source=.;Initial Catalog=plant;Integrated Security=True";
        private string insertQuery;

        public supply()
        {
            InitializeComponent();
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

        private void nextemregi_Click(object sender, EventArgs e)
        {
            string firstName = fname.Text;
            string lastName = lname.Text;
            string phoneNumber = phone.Text;
            string age = ageb.Text;
            string address = addressb.Text;
            string gender = GetSelectedGender(); // Get the selected gender from radio buttons
            string nicNumber = nicb.Text;

            insertQuery = "INSERT INTO supply (FirstName, LastName, PhoneNumber, Age, Address, Gender, NICNumber) " +
                          "VALUES (@FirstName, @LastName, @PhoneNumber, @Age, @Address, @Gender, @NICNumber)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

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
                            ClearGenderSelection(); // Clear gender selection

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
            }
        }

        private void ClearGenderSelection()
        {
            maler.Checked = false;
            femalelr.Checked = false;
        }
    }
}
