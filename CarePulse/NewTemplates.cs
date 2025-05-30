﻿using System;
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
        private string templateToLoad;

        public NewTemplates(string templateName = null)
        {
            InitializeComponent();
            templateToLoad = templateName;
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

            // Load template files into the ComboBox
            LoadTemplatesIntoComboBox();

            // If a template name is provided, load it
            if (!string.IsNullOrWhiteSpace(templateToLoad))
            {
                comboBoxSelectSurveyTemplate.SelectedItem = templateToLoad;
                comboBoxSelectSurveyTemplate_SelectedIndexChanged(null, null);
            }
        }


        private void btnNew_Click(object sender, EventArgs e)
        {
            // Reset selection
            selectedTextBox = null;

            // Reset ComboBox selection (optional)
            comboBoxSelectSurveyTemplate.SelectedIndex = -1;

            // Create a new HopeTextBox
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

        private void btnEdit_Click(object sender, EventArgs e)
        {
            // Check if a template is selected
            if (comboBoxSelectSurveyTemplate.SelectedItem == null)
            {
                MessageBox.Show("Please select a template to edit.", "No Template Selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Enable editing of the template name
            txtboxTemplateName.Enabled = true;

            // Enable editing of all HopeTextBox controls in the FlowLayoutPanel
            foreach (Control control in flowLayoutPanel1.Controls)
            {
                if (control is HopeTextBox textBox)
                {
                    textBox.Enabled = true;
                    textBox.BorderColorA = Color.FromArgb(64, 158, 255);
                }
            }

            // Optionally, highlight the first textbox for better UX
            if (flowLayoutPanel1.Controls.Count > 0 && flowLayoutPanel1.Controls[0] is HopeTextBox firstTextBox)
            {
                selectedTextBox = firstTextBox;
                firstTextBox.Focus();
                HighlightSelectedTextBox();
            }

            MessageBox.Show("You can now edit the Template. Click 'Save Changes' to apply your modifications.", "Edit Mode",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
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


        private void LoadTemplatesIntoComboBox()
        {
            string folderPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "CarePulse", "SurveyTemplate");

            if (!Directory.Exists(folderPath))
                return;

            var files = Directory.GetFiles(folderPath, "*.json");
            comboBoxSelectSurveyTemplate.Items.Clear();

            foreach (var file in files)
            {
                comboBoxSelectSurveyTemplate.Items.Add(Path.GetFileNameWithoutExtension(file));
            }
        }



        private void comboBoxSelectSurveyTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxSelectSurveyTemplate.SelectedItem == null)
                return;

            string selectedTemplate = comboBoxSelectSurveyTemplate.SelectedItem.ToString();
            string folderPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "CarePulse", "SurveyTemplate");
            string filePath = Path.Combine(folderPath, selectedTemplate + ".json");

            if (!File.Exists(filePath))
                return;

            try
            {
                string json = File.ReadAllText(filePath);
                dynamic template = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

                txtboxTemplateName.Text = template.TemplateName;

                flowLayoutPanel1.Controls.Clear(); // Clear existing questions

                foreach (var question in template.Questions)
                {
                    HopeTextBox questionBox = new HopeTextBox
                    {
                        Width = 400,
                        Height = 44,
                        Visible = true,
                        Multiline = true,
                        Hint = "Enter template text...",
                        Text = question.ToString(),
                        Name = "QuestionSurvey_" + flowLayoutPanel1.Controls.Count,
                        ForeColor = Color.FromArgb(30, 35, 29),
                        Font = new Font("Calibri", 9.25f, FontStyle.Bold),
                        BackColor = Color.White,
                        BaseColor = Color.White,
                        BorderColorA = Color.FromArgb(64, 158, 255),
                        BorderColorB = Color.LightGray,
                        Margin = new Padding(5),
                        Enabled = false,
                    };

                    // Add click/focus for selection
                    questionBox.Click += (s, args) => {
                        selectedTextBox = questionBox;
                        HighlightSelectedTextBox();
                    };
                    questionBox.GotFocus += (s, args) => {
                        selectedTextBox = questionBox;
                        HighlightSelectedTextBox();
                    };

                    flowLayoutPanel1.Controls.Add(questionBox);
                }

                if (flowLayoutPanel1.Controls.Count > 0)
                {
                    selectedTextBox = flowLayoutPanel1.Controls[0] as HopeTextBox;
                    selectedTextBox?.Focus();
                    HighlightSelectedTextBox();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading template: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            // Clear the template name
            txtboxTemplateName.Text = string.Empty;

            // Remove all controls from the FlowLayoutPanel
            flowLayoutPanel1.Controls.Clear();

            // Reset selection
            selectedTextBox = null;

            // Reset ComboBox selection (optional)
            comboBoxSelectSurveyTemplate.SelectedIndex = -1;
        }

    
    }
    
}
