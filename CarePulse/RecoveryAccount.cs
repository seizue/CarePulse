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
    public partial class RecoveryAccount : Form
    {
        private const string FolderName = "CarePulse";
        private const string SubFolderName = "Credential";
        private const string FileName = "userRegistrations.json";
        public RecoveryAccount()
        {
            InitializeComponent();
            LoadUsernames();

            txtboxNewPassword.KeyDown += (s, e) => {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    e.Handled = true;
                    btnSaveNewPass_Click(btnSaveNewPass, EventArgs.Empty);
                }
            };
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void comBox_UserAccounts_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedUsername = comBox_UserAccounts.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selectedUsername)) return;

            string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), FolderName, SubFolderName);
            string filePath = Path.Combine(appDataPath, FileName);

            // Read the existing data from the JSON file
            List<UserRegistration> userRegistrations = new List<UserRegistration>();
            if (File.Exists(filePath))
            {
                try
                {
                    string json = File.ReadAllText(filePath);
                    userRegistrations = JsonConvert.DeserializeObject<List<UserRegistration>>(json);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error reading JSON file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void LoadUsernames()
        {
            string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), FolderName, SubFolderName);
            string filePath = Path.Combine(appDataPath, FileName);

            // Clear existing items in case of multiple loads
            comBox_UserAccounts.Items.Clear();

            if (File.Exists(filePath))
            {
                try
                {
                    string json = File.ReadAllText(filePath);
                    List<UserRegistration> userRegistrations = JsonConvert.DeserializeObject<List<UserRegistration>>(json);

                    if (userRegistrations != null && userRegistrations.Count > 0)
                    {
                        foreach (var user in userRegistrations)
                        {
                            comBox_UserAccounts.Items.Add(user.Username);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No registered users found.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading usernames: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("No user registration file found.");
            }
        }


        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            string usernameToCheck = comBox_UserAccounts.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(usernameToCheck))
            {
                MessageBox.Show("Please select a username.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (CheckUsernameExists(usernameToCheck))
            {
                txtboxNewPassword.Enabled = true;
                btnSaveNewPass.Enabled = true;
                MessageBox.Show("You can now set a new password.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Username does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtboxNewPassword.Enabled = false;
                btnSaveNewPass.Enabled = false;
            }
        }


        private bool CheckUsernameExists(string username)
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string folderPath = Path.Combine(appDataPath, FolderName, SubFolderName);
            string filePath = Path.Combine(folderPath, FileName);

            if (File.Exists(filePath))
            {
                string existingJson = File.ReadAllText(filePath);
                var registrations = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(existingJson);

                return registrations != null && registrations.Any(r => r["Username"] == username);
            }

            return false;
        }


        private void btnSaveNewPass_Click(object sender, EventArgs e)
        {
            string usernameToUpdate = comBox_UserAccounts.SelectedItem?.ToString();
            string newPassword = txtboxNewPassword.Text;

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                MessageBox.Show("Please enter a new password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (UpdatePassword(usernameToUpdate, newPassword))
            {
                MessageBox.Show("Password updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close(); // Optionally close the form after saving
            }
            else
            {
                MessageBox.Show("Error updating password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool UpdatePassword(string username, string newPassword)
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string folderPath = Path.Combine(appDataPath, FolderName, SubFolderName);
            string filePath = Path.Combine(folderPath, FileName);

            if (File.Exists(filePath))
            {
                string existingJson = File.ReadAllText(filePath);
                var registrations = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(existingJson);

                if (registrations != null)
                {
                    var user = registrations.FirstOrDefault(r => r["Username"] == username);
                    if (user != null)
                    {
                        user["Password"] = newPassword; // Update the password
                        string json = JsonConvert.SerializeObject(registrations, Formatting.Indented);
                        File.WriteAllText(filePath, json);
                        return true;
                    }
                }
            }

            return false; // Password not updated
        }

        private void labelBackToLogin_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public class UserRegistration
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string FullName { get; set; }
            public DateTime RegistrationDate { get; set; }
        }

      
    }
}
