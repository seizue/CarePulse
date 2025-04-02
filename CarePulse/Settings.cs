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
using static CarePulse.RecoveryAccount;

namespace CarePulse
{
    public partial class Settings : Form
    {
        private readonly Main mainForm;
        private const string FolderName = "CarePulse";
        private const string SubFolderName = "Credential";
        private const string FileName = "userRegistrations.json";

        public Settings(Main owner)
        {
            InitializeComponent();
            mainForm = owner;

            // Setup window state combo box
            comBox_WindowState.Items.AddRange(new object[] { "Normal", "Maximized" });

            // Load current window state
            string savedWindowState = Properties.Settings.Default.MainFormWindowState;
            if (!string.IsNullOrEmpty(savedWindowState))
            {
                comBox_WindowState.SelectedItem = savedWindowState;
            }
            else
            {
                comBox_WindowState.SelectedItem = "Normal";
            }

            SettingsCheck();

            // Ensure proper form positioning
            this.StartPosition = FormStartPosition.Manual;
            this.Load += (s, e) => AdjustFormPosition();


            txtboxFullName.TextChanged += TextBoxToUpper_TextChanged;

            txtboxNewPassword.KeyDown += (s, e) => {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    e.Handled = true;
                    btnSaveUsernamePassword_Click(btnSaveUsernamePassword, EventArgs.Empty);
                }
            };

            txtboxFullName.KeyDown += (s, e) => {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    e.Handled = true;
                    SendKeys.Send("{TAB}");
                }
            };

            txtboxNewUsername.KeyDown += (s, e) => {
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


        private void AdjustFormPosition()
        {
            Screen screen = Screen.FromControl(this);
            Rectangle workingArea = screen.WorkingArea;

            // Calculate center position
            int left = workingArea.Left + (workingArea.Width - this.Width) / 2;
            int top = workingArea.Top + (workingArea.Height - this.Height) / 2;

            // Ensure we stay within working area bounds
            if (left < workingArea.Left) left = workingArea.Left;
            if (top < workingArea.Top) top = workingArea.Top;
            if (left + this.Width > workingArea.Right) left = workingArea.Right - this.Width;
            if (top + this.Height > workingArea.Bottom) top = workingArea.Bottom - this.Height;

            this.Location = new Point(left, top);
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            if (comBox_WindowState.SelectedItem != null &&
       Enum.TryParse<FormWindowState>(comBox_WindowState.SelectedItem.ToString(),
       out FormWindowState newState))
            {
                Properties.Settings.Default.MainFormWindowState = comBox_WindowState.SelectedItem.ToString();
                Properties.Settings.Default.Save();

                mainForm.BeginInvoke(new Action(() =>
                {
                    // First ensure proper positioning if going to normal state
                    if (newState == FormWindowState.Normal)
                    {
                        mainForm.WindowState = FormWindowState.Normal;
                        // Allow time for the window to normalize before positioning
                        Application.DoEvents();
                        typeof(Main).GetMethod("PositionForm",
                            System.Reflection.BindingFlags.NonPublic |
                            System.Reflection.BindingFlags.Instance)
                            ?.Invoke(mainForm, null);
                    }

                    mainForm.WindowState = newState;
                }));

                MessageBox.Show("Settings saved and applied successfully.",
                              "Settings Saved",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Please select a valid window state.",
                              "Invalid Selection",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }


        private void comboBoxUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedUsername = comboBoxUsers.SelectedItem?.ToString();

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

            // Find the selected user by Username
            var user = userRegistrations.FirstOrDefault(u => u.Username == selectedUsername);
            if (user != null)
            {
                // Populate the textboxes with the user's data
                txtboxFullName.Text = user.FullName;
                txtboxRegistration.Text = user.RegistrationDate.ToString("yyyy-MM-dd");
                txtboxNewUsername.Text = user.Username;
                txtboxNewPassword.Text = user.Password;
            }
        }


        private void btnSaveInformation_Click(object sender, EventArgs e)
        {
            string selectedUsername = comboBoxUsers.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedUsername))
            {
                MessageBox.Show("Please select a user to update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

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

            // Find the selected user by Username
            var user = userRegistrations.FirstOrDefault(u => u.Username == selectedUsername);
            if (user != null)
            {
                // Update the user's data
                user.FullName = txtboxFullName.Text;

                // Save the updated data back to the JSON file
                try
                {
                    string updatedJson = JsonConvert.SerializeObject(userRegistrations, Formatting.Indented);
                    File.WriteAllText(filePath, updatedJson);
                    MessageBox.Show(" Information saved successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    comboBoxUsers.Refresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving JSON file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("User not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSaveUsernamePassword_Click(object sender, EventArgs e)
        {
            string selectedUsername = comboBoxUsers.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedUsername))
            {
                MessageBox.Show("Please select a user to update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

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

            // Find the selected user by Username
            var user = userRegistrations.FirstOrDefault(u => u.Username == selectedUsername);
            if (user != null)
            {
                // Ask for confirmation before saving changes
                DialogResult confirmationResult = MessageBox.Show(
                    "Are you sure you want to update the username and password for this user?",
                    "Confirm Update",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                // If user clicks Yes, proceed with update
                if (confirmationResult == DialogResult.Yes)
                {
                    // Update the user's data with new Username and Password
                    user.Username = txtboxNewUsername.Text;
                    user.Password = txtboxNewPassword.Text;

                    // Save the updated data back to the JSON file
                    try
                    {
                        string updatedJson = JsonConvert.SerializeObject(userRegistrations, Formatting.Indented);
                        File.WriteAllText(filePath, updatedJson);
                        MessageBox.Show("Update is saved successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Reload ComboBox with updated list
                        comboBoxUsers.Items.Clear(); // Clear current items
                        foreach (var updatedUser in userRegistrations)
                        {
                            comboBoxUsers.Items.Add(updatedUser.Username); // Add updated user list back to ComboBox
                        }
                        comboBoxUsers.SelectedItem = user.Username; // Select the updated username
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error saving JSON file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // If user clicks No, do nothing
                    MessageBox.Show("Update cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("User not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void SettingsCheck()
        {
            string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), FolderName, SubFolderName);
            if (!Directory.Exists(appDataPath))
            {
                MessageBox.Show("The credentials folder does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

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

            // Populate the ComboBox with usernames
            if (userRegistrations.Any())
            {
                comboBoxUsers.Items.Clear();
                foreach (var user in userRegistrations)
                {
                    comboBoxUsers.Items.Add(user.Username); // Add the Username to the ComboBox
                }
            }
            else
            {
                MessageBox.Show("No users found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void btnDeleteAcc_Click(object sender, EventArgs e)
        {
            // Get the selected username from the ComboBox
            string selectedUsername = comboBoxUsers.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedUsername))
            {
                MessageBox.Show("Please select a user to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

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

            // Find and remove the user by Username
            var userToRemove = userRegistrations.FirstOrDefault(u => u.Username == selectedUsername);
            if (userToRemove != null)
            {
                // Ask for confirmation before deleting
                DialogResult confirmationResult = MessageBox.Show(
                    $"Are you sure you want to delete the user '{selectedUsername}'?",
                    "Confirm Deletion",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                // If user clicks Yes, proceed with deletion
                if (confirmationResult == DialogResult.Yes)
                {
                    userRegistrations.Remove(userToRemove);

                    // Save the updated list back to the JSON file
                    try
                    {
                        string updatedJson = JsonConvert.SerializeObject(userRegistrations, Formatting.Indented);
                        File.WriteAllText(filePath, updatedJson);
                        MessageBox.Show("User deleted successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Clear the textboxes
                        txtboxFullName.Clear();
                        txtboxNewUsername.Clear();
                        txtboxNewPassword.Clear();

                        // Reload ComboBox with updated list
                        comboBoxUsers.Items.Clear(); // Clear current items
                        foreach (var user in userRegistrations)
                        {
                            comboBoxUsers.Items.Add(user.Username); // Add updated user list back to ComboBox
                        }

                        // Optionally select the first user if ComboBox is not empty
                        if (comboBoxUsers.Items.Count > 0)
                        {
                            comboBoxUsers.SelectedIndex = 0; // Select the first user in the list
                        }
                        else
                        {
                            comboBoxUsers.SelectedIndex = -1; // No selection if the ComboBox is empty
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error saving JSON file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // If user clicks No, do nothing
                    MessageBox.Show("Deletion cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("User not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
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

            // Reload ComboBox with updated list
            comboBoxUsers.Items.Clear(); // Clear current items
            foreach (var user in userRegistrations)
            {
                comboBoxUsers.Items.Add(user.Username); // Add updated user list back to ComboBox
            }

            // Optionally select the first user if ComboBox is not empty
            if (comboBoxUsers.Items.Count > 0)
            {
                comboBoxUsers.SelectedIndex = 0; // Select the first user in the list
            }

            MessageBox.Show("Data refreshed successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnExportData_Click(object sender, EventArgs e)
        {
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

            // Prompt the user to select a location to save the CSV file
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "CSV Files (*.csv)|*.csv";
                saveFileDialog.Title = "Export Data as CSV";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Write to the CSV file
                        StringBuilder sb = new StringBuilder();

                        // Add header to CSV
                        sb.AppendLine("Username,FullName,RegistrationDate,Password");

                        // Add each user's data to the CSV
                        foreach (var user in userRegistrations)
                        {
                            sb.AppendLine($"{user.Username},{user.FullName},{user.RegistrationDate.ToString("yyyy-MM-dd")},{user.Password}");
                        }

                        // Save the CSV data to the selected file
                        File.WriteAllText(saveFileDialog.FileName, sb.ToString());

                        MessageBox.Show("Data exported successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error exporting data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
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


        public class UserRegistration
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string FullName { get; set; }
            public DateTime RegistrationDate { get; set; }
        }
    }
}
