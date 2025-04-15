using Newtonsoft.Json;
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

namespace CarePulse
{
    public partial class EntryNew : Form
    {
       
        public EntryNew()
        {
            InitializeComponent();
            GenerateUniqueId();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNewTemplateSurvey_Click(object sender, EventArgs e)
        {
            NewTemplates newTemplates = new NewTemplates();
            newTemplates.ShowDialog();
        }


        private void btnAnsSurvey_Click(object sender, EventArgs e)
        {
            // Generate the ID if not already entered
            if (string.IsNullOrWhiteSpace(txtBoxIDNo.Text))
            {
                string generatedId = GenerateUniqueId();
                txtBoxIDNo.Text = generatedId;  // Set the generated ID in the textbox
            }

            string id = txtBoxIDNo.Text.Trim();
            if (!string.IsNullOrWhiteSpace(id))
            {
                // If ID is valid, create a new Survey form and pass the ID to it
                Survey survey = new Survey(id);  // Create a new instance of Survey

                // Load the selected survey template and pass the questions
                string selectedTemplate = comboBoxSelectSurveyTemplate.SelectedItem.ToString();
                string templatesPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CarePulse", "SurveyTemplate");
                string filePath = Path.Combine(templatesPath, selectedTemplate + ".json");

                if (File.Exists(filePath))
                {
                    string json = File.ReadAllText(filePath);
                    SurveyTemplate template = JsonConvert.DeserializeObject<SurveyTemplate>(json);

                    if (template?.Questions != null)
                    {
                        // Pass the questions to the Survey form
                        survey.SetSurveyQuestions(template.Questions);
                    }
                }

                // Show the Survey form
                survey.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please enter an ID number first.");
            }
        }

        // Method to generate a unique respondent ID in the format X1D-25D
        private string GenerateUniqueId()
        {
            string id;
            bool idExists;

            do
            {
                id = GenerateRandomId();
                idExists = CheckIdExists(id);
            }
            while (idExists);  // Keep generating until a unique ID is found

            return id;
        }

        // Method to generate a random ID (X1D-25D format)
        private string GenerateRandomId()
        {
            Random random = new Random();
            StringBuilder id = new StringBuilder();

            // Generate random alphanumeric characters
            id.Append((char)random.Next('A', 'Z' + 1));  // First letter
            id.Append(random.Next(0, 10));                // First number
            id.Append((char)random.Next('A', 'Z' + 1));  // Second letter
            id.Append('-');
            id.Append(random.Next(10, 100));             // First two-digit number
            id.Append((char)random.Next('A', 'Z' + 1));  // Third letter

            return id.ToString();
        }

        // Method to check if the ID already exists in the saved data
        private bool CheckIdExists(string id)
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CarePulse", "AnsweredSurvey");
            Directory.CreateDirectory(path);

            // Get all the files in the directory (assuming the files are named by the ID)
            var existingIds = Directory.GetFiles(path)
                                        .Select(file => Path.GetFileNameWithoutExtension(file))
                                        .ToList();

            // Check if the generated ID already exists
            return existingIds.Contains(id);
        }

        private void LoadSurveyTemplates()
        {
            string templatesPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CarePulse", "SurveyTemplate");

            if (Directory.Exists(templatesPath))
            {
                var files = Directory.GetFiles(templatesPath, "*.json");

                foreach (var file in files)
                {
                    comboBoxSelectSurveyTemplate.Items.Add(Path.GetFileNameWithoutExtension(file));
                }
            }
            else
            {
                Directory.CreateDirectory(templatesPath);
            }
        }


        private void comboBoxSelectSurveyTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            string id = txtBoxIDNo.Text.Trim(); // Get the respondent ID from the text box

            if (string.IsNullOrWhiteSpace(id))
            {
                MessageBox.Show("Please enter a valid respondent ID before selecting a template.");
                return;  // Exit if the ID is empty
            }

            // Get the selected template
            string selectedTemplate = comboBoxSelectSurveyTemplate.SelectedItem.ToString();

            // Display the selected template name in the survey status textbox
            txtboxSurveyStatus.Text = $"Selected Template: {selectedTemplate}";
        }



        private void EntryNew_Load(object sender, EventArgs e)
        {
            // Generate and set the unique ID during form load
            string generatedId = GenerateUniqueId();
            txtBoxIDNo.Text = generatedId;

            LoadSurveyTemplates();
        }
    }
}

