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
    public partial class vehicle : Form
    {
        private const string connectionString = "Data Source=.;Initial Catalog=plant;Integrated Security=True";

        public vehicle()
        {
            InitializeComponent();
        }

        private void nextemregi_Click(object sender, EventArgs e)
        {
            string vehicleName = fname.Text;
            string vehicleNumber = addressb.Text;
            string vehicleType = phone.Text;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string insertQuery = "INSERT INTO Vehicle (VehicleName, VehicleNumber, VehicleType) VALUES (@VehicleName, @VehicleNumber, @VehicleType)";

                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@VehicleName", vehicleName);
                        command.Parameters.AddWithValue("@VehicleNumber", vehicleNumber);
                        command.Parameters.AddWithValue("@VehicleType", vehicleType);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Vehicle details inserted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Clear the text boxes after successful insertion
                            fname.Clear();
                            addressb.Clear();
                            phone.Clear();
                        }
                        else
                        {
                            MessageBox.Show("Failed to insert vehicle details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
