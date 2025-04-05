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
using ReaLTaiizor.Controls;

namespace CarePulse
{
    public partial class NewTemplates : Form
    {
        private HopeTextBox selectedTextBox = null;

        public NewTemplates()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMaximize_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                // Maximize the window without covering the taskbar
                this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
                this.WindowState = FormWindowState.Maximized;
            }
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }


        private void NewTemplates_Load(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                // Maximize the window without covering the taskbar
                this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
                this.WindowState = FormWindowState.Maximized;
            }
        }

       
        private void btnNew_Click(object sender, EventArgs e)
        {
            HopeTextBox newHopeTextBox = new HopeTextBox
            {
                Width = 400,
                Height = 44,
                Visible = true,
                Multiline = true,
                Hint = "Enter template text...",
                Name = "QuestionSurvey_" + flowLayoutPanel1.Controls.Count,
                ForeColor = Color.FromArgb(30, 35, 29),
                Font = new Font("Calibri", 9.25f, FontStyle.Bold),
                BackColor = Color.White,
                BaseColor = Color.White,
                BorderColorA = Color.FromArgb(64, 158, 255),
                BorderColorB = Color.LightGray,
                Margin = new Padding(5)
            };

            // Add explicit click event handler to mark this textbox as selected
            newHopeTextBox.Click += (s, args) => {
                Console.WriteLine("TextBox clicked: " + newHopeTextBox.Name);
                selectedTextBox = newHopeTextBox;
                HighlightSelectedTextBox();
            };

            // Additional event for when the textbox receives focus
            newHopeTextBox.GotFocus += (s, args) => {
                Console.WriteLine("TextBox got focus: " + newHopeTextBox.Name);
                selectedTextBox = newHopeTextBox;
                HighlightSelectedTextBox();
            };

            // Ensure AutoScroll is enabled
            flowLayoutPanel1.AutoScroll = true;
            // Add the control
            flowLayoutPanel1.Controls.Add(newHopeTextBox);
            // Force layout refresh
            flowLayoutPanel1.PerformLayout();
            // Scroll to make the new control visible
            newHopeTextBox.Focus();
            flowLayoutPanel1.ScrollControlIntoView(newHopeTextBox);
            // Additional approach to ensure scrolling works
            flowLayoutPanel1.AutoScrollPosition = new Point(0, flowLayoutPanel1.VerticalScroll.Maximum);

            // Set this as the selected textbox
            selectedTextBox = newHopeTextBox;
            HighlightSelectedTextBox();
        }

        // Method to visually highlight the selected textbox
        private void HighlightSelectedTextBox()
        {
            // Reset all textboxes to default style
            foreach (Control control in flowLayoutPanel1.Controls)
            {
                if (control is HopeTextBox textBox)
                {
                    textBox.BorderColorA = Color.FromArgb(64, 158, 255); // Default color
                }
            }

            // Highlight the selected textbox
            if (selectedTextBox != null)
            {
                selectedTextBox.BorderColorA = Color.FromArgb(255, 0, 0); // Red highlight for better visibility
                Console.WriteLine("Highlighted textbox: " + selectedTextBox.Name);
            }
        }

        // Implement the delete button to remove the selected textbox
        private void btnDelete_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Delete button clicked");

            // Debug: Check what's currently selected
            if (selectedTextBox != null)
                Console.WriteLine("Current selection: " + selectedTextBox.Name);
            else
                Console.WriteLine("No textbox currently selected");

            if (selectedTextBox != null && flowLayoutPanel1.Controls.Contains(selectedTextBox))
            {
                Console.WriteLine("Deleting textbox: " + selectedTextBox.Name);

                // Store index for selecting the next textbox
                int index = flowLayoutPanel1.Controls.IndexOf(selectedTextBox);

                // Remove the selected control
                flowLayoutPanel1.Controls.Remove(selectedTextBox);
                selectedTextBox.Dispose();

                // Clear selection reference
                selectedTextBox = null;

                // Select another textbox if available
                if (flowLayoutPanel1.Controls.Count > 0)
                {
                    // Try to select the control at the same index or the last one
                    int newIndex = Math.Min(index, flowLayoutPanel1.Controls.Count - 1);

                    if (flowLayoutPanel1.Controls[newIndex] is HopeTextBox nextTextBox)
                    {
                        selectedTextBox = nextTextBox;
                        selectedTextBox.Focus();
                        HighlightSelectedTextBox();
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a template to delete.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnPosted_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if template name is provided
                if (string.IsNullOrWhiteSpace(txtboxTemplateName.Text))
                {
                    MessageBox.Show("Please enter a template name.", "Template Name Required",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Create directory if it doesn't exist
                string folderPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "CarePulse", "SurveyTemplate");

                Directory.CreateDirectory(folderPath);

                // Create a list to hold all the template questions
                List<string> templateQuestions = new List<string>();

                // Collect text from all textboxes in the flowLayoutPanel
                foreach (Control control in flowLayoutPanel1.Controls)
                {
                    if (control is HopeTextBox textBox && !string.IsNullOrWhiteSpace(textBox.Text))
                    {
                        templateQuestions.Add(textBox.Text);
                    }
                }

                // Check if any questions were entered
                if (templateQuestions.Count == 0)
                {
                    MessageBox.Show("Please add at least one template question.", "No Questions Found",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Create the template object
                var template = new
                {
                    TemplateName = txtboxTemplateName.Text,
                    CreatedDate = DateTime.Now,
                    Questions = templateQuestions
                };

                // Convert to JSON
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(template, Newtonsoft.Json.Formatting.Indented);

                // Generate safe filename
                string fileName = txtboxTemplateName.Text;
                foreach (char c in Path.GetInvalidFileNameChars())
                {
                    fileName = fileName.Replace(c, '_');
                }

                string filePath = Path.Combine(folderPath, fileName + ".json");

                // Write to file
                File.WriteAllText(filePath, json);

                MessageBox.Show("Template saved successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving template: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
    
}
