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
    public partial class SignIn : Form
    {
        private const string FolderName = "CarePulse";
        private const string SubFolderName = "Credential";
        private const string FileName = "userRegistrations.json";

        public SignIn()
        {
            InitializeComponent();
            LoadRememberedCredentials();

            //KeyDown event to handle Enter key press
            txtboxUsername.KeyDown += Txtbox_KeyDown;
            txtboxPass.KeyDown += Txtbox_KeyDown;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private async void btnSignIn_Click(object sender, EventArgs e)
        {
            string username = txtboxUsername.Text;
            string password = txtboxPass.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both username and password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (ValidateCredentials(username, password))
            {
                MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (checkBoxRemember.Checked)
                {
                    SaveRememberedCredentials(username, password);
                }
                else
                {
                    ClearRememberedCredentials();
                }

                // Hide the current form
                this.Hide();

                // Instantiate the MainForm but keep it hidden
                Main mainForm = new Main();
                mainForm.InitializeControls();

                // Hide the MainForm
                mainForm.Opacity = 0;
                mainForm.Visible = false;

                // Show the MainForm 
                mainForm.Show();
                await Task.Delay(1000);
                mainForm.Opacity = 100;
                mainForm.Visible = true;
            }
            else
            {
                MessageBox.Show("Invalid username or password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateCredentials(string username, string password)
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string filePath = Path.Combine(appDataPath, FolderName, SubFolderName, FileName);

            if (!File.Exists(filePath))
            {
                return false;
            }

            try
            {
                string json = File.ReadAllText(filePath);
                var registrations = JsonConvert.DeserializeObject<List<dynamic>>(json);
                return registrations.Any(r => r.Username == username && r.Password == password);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading credentials: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void checkBoxRemember_CheckedChanged(object sender)
        {
            if (!checkBoxRemember.Checked)
            {
                ClearRememberedCredentials();
            }
        }

        private void SaveRememberedCredentials(string username, string password)
        {
            Properties.Settings.Default.RememberedUsername = username;
            Properties.Settings.Default.RememberedPassword = password;
            Properties.Settings.Default.RememberLogin = true;
            Properties.Settings.Default.Save();
        }

        private void ClearRememberedCredentials()
        {
            Properties.Settings.Default.RememberedUsername = string.Empty;
            Properties.Settings.Default.RememberedPassword = string.Empty;
            Properties.Settings.Default.RememberLogin = false;
            Properties.Settings.Default.Save();
        }

        private void LoadRememberedCredentials()
        {
            if (Properties.Settings.Default.RememberLogin)
            {
                txtboxUsername.Text = Properties.Settings.Default.RememberedUsername;
                txtboxPass.Text = Properties.Settings.Default.RememberedPassword;
                checkBoxRemember.Checked = true;
            }
        }

        private void labelAskPassword_Click(object sender, EventArgs e)
        {
            RecoveryAccount recover = new RecoveryAccount();
            recover.Show();
        }

        private void labelCreateAcc_Click(object sender, EventArgs e)
        {
            SignUp signUp = new SignUp();
            signUp.Show();
        }

        private void Txtbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                btnSignIn_Click(sender, e);
            }
        }
    }
}
