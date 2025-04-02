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
    public partial class SignUp : Form
    {
        private const string FolderName = "CarePulse";
        private const string SubFolderName = "Credential";
        private const string FileName = "userRegistrations.json";

        public SignUp()
        {
            InitializeComponent();

            txtboxFullName.TextChanged += TextBoxToUpper_TextChanged;

            txtboxFullName.KeyDown += (s, e) => {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    e.Handled = true;
                    btnSignUp_Click(btnSignUp, EventArgs.Empty);
                }
            };

            txtboxUsername.KeyDown += (s, e) => {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    e.Handled = true;
                    SendKeys.Send("{TAB}");
                }
            };

            txtboxPass.KeyDown += (s, e) => {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    e.Handled = true;
                    SendKeys.Send("{TAB}");
                }
            };

            this.KeyPreview = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    
        private void btnSignUp_Click(object sender, EventArgs e)
        {
            string username = txtboxUsername.Text;
            string password = txtboxPass.Text;
            string fullName = txtboxFullName.Text;
            string registrationDate = datePickerRegister.Value.ToString("yyyy-MM-dd");

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(fullName))
            {
                MessageBox.Show("Please fill in all required fields (Username, Password, Full Name, and Date).",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (UsernameExists(username))
            {
                MessageBox.Show("Username already exists. Please choose a different username.",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SaveRegistration(username, password, fullName, registrationDate);
            MessageBox.Show("Registration successful.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Close();
        }

        private bool UsernameExists(string username)
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string folderPath = Path.Combine(appDataPath, FolderName, SubFolderName);
            string filePath = Path.Combine(folderPath, FileName);

            if (!File.Exists(filePath))
            {
                return false;
            }

            try
            {
                string existingJson = File.ReadAllText(filePath);
                var registrations = JsonConvert.DeserializeObject<List<dynamic>>(existingJson) ?? new List<dynamic>();

                return registrations.Any(r => r.Username.ToString() == username);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading registrations: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void SaveRegistration(string username, string password, string fullName, string registrationDate)
        {
            var registration = new
            {
                Username = username,
                Password = password,
                FullName = fullName,
                RegistrationDate = registrationDate
            };

            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string folderPath = Path.Combine(appDataPath, FolderName, SubFolderName);
            string filePath = Path.Combine(folderPath, FileName);

            try
            {
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                    DirectoryInfo credentialFolder = new DirectoryInfo(folderPath);
                    credentialFolder.Attributes |= FileAttributes.Hidden;
                }

                List<dynamic> registrations;
                if (File.Exists(filePath))
                {
                    string existingJson = File.ReadAllText(filePath);
                    registrations = JsonConvert.DeserializeObject<List<dynamic>>(existingJson) ?? new List<dynamic>();
                }
                else
                {
                    registrations = new List<dynamic>();
                }

                registrations.Add(registration);
                string json = JsonConvert.SerializeObject(registrations, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving registration: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TextBoxToUpper_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                // Convert the text to uppercase
                textBox.Text = textBox.Text.ToUpper();

                // Set the cursor to the end of the text
                textBox.SelectionStart = textBox.Text.Length;
            }
        }

        private void labelBackSignIn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
