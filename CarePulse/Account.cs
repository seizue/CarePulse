using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace CarePulse
{
    public partial class Account : Form
    {
        public Account()
        {
            InitializeComponent();
            this.Load += Account_Load; 
        }

        private void Account_Load(object sender, EventArgs e)
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string credentialPath = Path.Combine(appDataPath, "CarePulse", "Credential", "userRegistrations.json");

            if (!File.Exists(credentialPath))
            {
                MessageBox.Show("User registration file not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string jsonContent = File.ReadAllText(credentialPath);
                var users = JsonConvert.DeserializeObject<List<User>>(jsonContent);

                if (users != null && users.Count > 0)
                {
                    var user = users[0]; 
                    txtboxUsername.Text = user.Username;
                    txtboxPass.Text = user.Password;
                    txtboxFullName.Text = user.FullName;
                    txtboxRegistration.Text = user.RegistrationDate;
                }
                else
                {
                    MessageBox.Show("No user data found in the registration file.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading user data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private class User
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string FullName { get; set; }
            public string RegistrationDate { get; set; }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
