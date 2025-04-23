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

    public partial class Survey : Form
    {       
        private string respondentId;
        private int currentPage = 1;
        private int totalPages = 1;
        private int rowsPerPage = 10;
        private List<string> surveyQuestionList = new List<string>();
        private Dictionary<string, string> surveyResponses = new Dictionary<string, string>();

        public Survey(string id)
        {
            InitializeComponent();

            respondentId = id;
     
        }

        // Add this to the Survey constructor (the one that takes id, questions, responses)
        public Survey(string id, List<string> questions, Dictionary<string, string> responses)
        {
            InitializeComponent();

            respondentId = id;

            // Set survey questions and responses
            SetSurveyQuestions(questions);
            SetSurveyResponses(responses);

            // Load response options before initializing the page
            LoadResponseOptions();

            // Initialize pagination
            CalculatePagination();
            LoadPage(currentPage);
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
                this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
                this.WindowState = FormWindowState.Maximized;
            }
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }


        private void Survey_Load(object sender, EventArgs e)
        {
            // Set row height
            datagridSurvey.RowTemplate.Height = 33;

            foreach (DataGridViewRow row in datagridSurvey.Rows)
            {
                row.Height = 33;
            }

            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
                this.WindowState = FormWindowState.Maximized;
            }

          
        }

        public void SetSurveyQuestions(List<string> questions)
        {
            datagridSurvey.Rows.Clear();

            // Make the questions column read-only
            datagridSurvey.Columns["surveyQuestion"].ReadOnly = true;

            surveyQuestionList = questions; // Store all questions
            CalculatePagination();
            LoadPage(currentPage);
        }

      

        private void CalculatePagination()
        {
            // Calculate rows per page based on DataGridView height
            int availableHeight = datagridSurvey.Height - datagridSurvey.ColumnHeadersHeight;
            rowsPerPage = Math.Max(1, availableHeight / datagridSurvey.RowTemplate.Height);

            // Calculate total pages
            totalPages = (int)Math.Ceiling((double)surveyQuestionList.Count / rowsPerPage);
        }

        public void SetSurveyResponses(Dictionary<string, string> responses)
        {
            // Make a deep copy to avoid reference issues
            surveyResponses = new Dictionary<string, string>();

            if (responses != null)
            {
                foreach (var kvp in responses)
                {
                    surveyResponses[kvp.Key] = kvp.Value;
                }
            }
        }

  
        private void LoadPage(int pageNumber)
        {
            // Save current responses before clearing the grid
            foreach (DataGridViewRow row in datagridSurvey.Rows)
            {
                if (!row.IsNewRow)
                {
                    string questionId = row.Cells["surveyID"]?.Value?.ToString();
                    string response = row.Cells["surveyResponse"]?.Value?.ToString();
                    if (!string.IsNullOrWhiteSpace(questionId) && response != null)
                    {
                        surveyResponses[questionId] = response;
                    }
                }
            }

            datagridSurvey.Rows.Clear();

            // Calculate the range of questions to display
            int startIndex = (pageNumber - 1) * rowsPerPage;
            int endIndex = Math.Min(startIndex + rowsPerPage, surveyQuestionList.Count);

            // Add questions to the DataGridView
            for (int i = startIndex; i < endIndex; i++)
            {
                int index = datagridSurvey.Rows.Add();
                string questionId = $"Q{i + 1:D3}";
                datagridSurvey.Rows[index].Cells["surveyQuestion"].Value = surveyQuestionList[i];
                datagridSurvey.Rows[index].Cells["surveyID"].Value = questionId;

                // Check if we have a response for this question
                if (surveyResponses.ContainsKey(questionId))
                {
                    string response = surveyResponses[questionId];

                    if (!string.IsNullOrEmpty(response))
                    {
                        // Make sure the response is in the dropdown items
                        var comboCol = datagridSurvey.Columns["surveyResponse"] as DataGridViewComboBoxColumn;
                        if (comboCol != null && !comboCol.Items.Contains(response))
                        {
                            comboCol.Items.Add(response);
                        }

                        // Set the cell value
                        datagridSurvey.Rows[index].Cells["surveyResponse"].Value = response;
                    }
                }
            }

            // Scroll to the top of the DataGridView
            if (datagridSurvey.Rows.Count > 0)
            {
                datagridSurvey.FirstDisplayedScrollingRowIndex = 0;
            }

            // Update pagination buttons
            UpdatePaginationButtons();
        }

        private void UpdatePaginationButtons()
        {
            btnStartPage.Enabled = currentPage > 1;
            btnPreviousPage.Enabled = currentPage > 1;
            btnNextPage.Enabled = currentPage < totalPages;
            btnLastPage.Enabled = currentPage < totalPages;
        }


        private void btnResponseOption_Click(object sender, EventArgs e)
        {
            var comboCol = datagridSurvey.Columns["surveyResponse"] as DataGridViewComboBoxColumn;
            if (comboCol == null)
            {
                MessageBox.Show("The 'surveyResponse' column is not a ComboBox column.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Create the dynamic form
            Form dialog = new Form()
            {
                Text = "Edit Response Options",
                Width = 400,
                Height = 400,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false
            };

            ListBox listBox = new ListBox() { Top = 10, Left = 10, Width = 360, Height = 200 };
            TextBox txtInput = new TextBox() { Top = 220, Left = 10, Width = 260 };
            Button btnAdd = new Button() { Text = "Add", Top = 220, Left = 280, Width = 90 };
            Button btnUpdate = new Button() { Text = "Update", Top = 260, Left = 10, Width = 110 };
            Button btnDelete = new Button() { Text = "Delete", Top = 260, Left = 130, Width = 110 };
            Button btnClose = new Button() { Text = "Close", Top = 310, Left = 280, Width = 90 };

            // Load existing options into listbox
            foreach (var item in comboCol.Items)
            {
                listBox.Items.Add(item.ToString());
            }

            // Event: List selection -> update textbox
            listBox.SelectedIndexChanged += (s, ea) =>
            {
                if (listBox.SelectedItem != null)
                    txtInput.Text = listBox.SelectedItem.ToString();
            };

            // Event: Add
            btnAdd.Click += (s, ea) =>
            {
                string newItem = txtInput.Text.Trim();
                if (!string.IsNullOrEmpty(newItem) && !listBox.Items.Contains(newItem))
                {
                    listBox.Items.Add(newItem);
                    txtInput.Clear();
                }
            };

            // Event: Update
            btnUpdate.Click += (s, ea) =>
            {
                if (listBox.SelectedIndex >= 0)
                {
                    string updated = txtInput.Text.Trim();
                    if (!string.IsNullOrEmpty(updated))
                    {
                        listBox.Items[listBox.SelectedIndex] = updated;
                        txtInput.Clear();
                    }
                }
            };

            // Event: Delete
            btnDelete.Click += (s, ea) =>
            {
                if (listBox.SelectedItem != null)
                {
                    listBox.Items.Remove(listBox.SelectedItem);
                    txtInput.Clear();
                }
            };

            // Event: Close
            btnClose.Click += (s, ea) =>
            {
                // Update ComboBox items
                comboCol.Items.Clear();
                foreach (var item in listBox.Items)
                {
                    comboCol.Items.Add(item.ToString());
                }

                dialog.DialogResult = DialogResult.OK;
                dialog.Close();
            };

            // Add controls to form
            dialog.Controls.Add(listBox);
            dialog.Controls.Add(txtInput);
            dialog.Controls.Add(btnAdd);
            dialog.Controls.Add(btnUpdate);
            dialog.Controls.Add(btnDelete);
            dialog.Controls.Add(btnClose);
         
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                SaveResponseOptions();
            }
        }


        private void LoadResponseOptions()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CarePulse", "SurveyTemplateResponses", "responses.json");

            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                var obj = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json);
                if (obj != null && obj.ContainsKey("Responses"))
                {
                    var comboCol = datagridSurvey.Columns["surveyResponse"] as DataGridViewComboBoxColumn;
                    if (comboCol != null)
                    {
                        comboCol.Items.Clear();
                        comboCol.Items.AddRange(obj["Responses"].ToArray());
                    }
                }
            }
        }


        private void SaveResponseOptions()
        {
            var comboCol = datagridSurvey.Columns["surveyResponse"] as DataGridViewComboBoxColumn;
            if (comboCol != null)
            {
                var responses = comboCol.Items.Cast<string>().Distinct().ToList();
                var obj = new Dictionary<string, List<string>> { { "Responses", responses } };

                string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CarePulse", "SurveyTemplateResponses");
                Directory.CreateDirectory(folder);

                string path = Path.Combine(folder, "responses.json");
                File.WriteAllText(path, JsonConvert.SerializeObject(obj, Formatting.Indented));
            }
        }


        private void Survey_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveResponseOptions();
        }


        private void btnPost_Click(object sender, EventArgs e)
        {
            // Save any unsaved responses from current page before collecting all responses
            foreach (DataGridViewRow row in datagridSurvey.Rows)
            {
                if (!row.IsNewRow)
                {
                    string questionId = row.Cells["surveyID"]?.Value?.ToString();
                    string response = row.Cells["surveyResponse"]?.Value?.ToString();
                    if (!string.IsNullOrWhiteSpace(questionId) && response != null)
                    {
                        surveyResponses[questionId] = response;
                    }
                }
            }

            // Create a survey result object with all collected responses
            var surveyResult = new
            {
                RespondentID = respondentId,  // Store the respondent ID
                Date = DateTime.Now,          // Add a timestamp
                Answers = surveyResponses     // Add ALL responses (not just current page)
            };

            // Save survey result as a temporary JSON file
            string tempFolderPath = Path.Combine(Path.GetTempPath(), "CarePulse", "TemporarySurvey");
            Directory.CreateDirectory(tempFolderPath);
            string tempFilePath = Path.Combine(tempFolderPath, $"{respondentId}.json");

            File.WriteAllText(tempFilePath, JsonConvert.SerializeObject(surveyResult, Formatting.Indented));

            MessageBox.Show("Survey saved temporarily. Please confirm changes to save permanently.", "Temporary Save", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Close();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "CSV files (*.csv)|*.csv";
            saveDialog.Title = "Export Survey Responses";
            saveDialog.FileName = $"Survey_{respondentId}.csv";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(saveDialog.FileName))
                    {
                        // Write headers
                        writer.WriteLine("Question ID,Question,Response");

                        // Save current page responses
                        foreach (DataGridViewRow row in datagridSurvey.Rows)
                        {
                            if (!row.IsNewRow)
                            {
                                string id = row.Cells["surveyID"]?.Value?.ToString()?.Replace(",", " ") ?? "";
                                string question = row.Cells["surveyQuestion"]?.Value?.ToString()?.Replace(",", " ") ?? "";
                                string response = row.Cells["surveyResponse"]?.Value?.ToString()?.Replace(",", " ") ?? "";

                                writer.WriteLine($"{id},{question},{response}");
                            }
                        }

                        // Iterate through all pages and export data
                        for (int page = 1; page <= totalPages; page++)
                        {
                            if (page != currentPage)
                            {
                                LoadPage(page); // Load the page data into the DataGridView

                                foreach (DataGridViewRow row in datagridSurvey.Rows)
                                {
                                    if (!row.IsNewRow)
                                    {
                                        string id = row.Cells["surveyID"]?.Value?.ToString()?.Replace(",", " ") ?? "";
                                        string question = row.Cells["surveyQuestion"]?.Value?.ToString()?.Replace(",", " ") ?? "";
                                        string response = row.Cells["surveyResponse"]?.Value?.ToString()?.Replace(",", " ") ?? "";

                                        writer.WriteLine($"{id},{question},{response}");
                                    }
                                }
                            }
                        }

                        // Reload the original page
                        LoadPage(currentPage);
                    }

                    MessageBox.Show("Survey exported successfully!", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error exporting survey:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void btnRefresh_Click(object sender, EventArgs e)
        {
            // Reload the response options from file
            LoadResponseOptions();

            // Clear all responses
            foreach (DataGridViewRow row in datagridSurvey.Rows)
            {
                if (!row.IsNewRow)
                {
                    row.Cells["surveyResponse"].Value = null;
                }
            }

            MessageBox.Show("Survey refreshed!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnStartPage_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            LoadPage(currentPage);
        }

        private void btnPreviousPage_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadPage(currentPage);
            }
        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadPage(currentPage);
            }
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            currentPage = totalPages;
            LoadPage(currentPage);
        }

        private void Survey_Resize(object sender, EventArgs e)
        {
            CalculatePagination();
            LoadPage(currentPage);
        }

        private void btnImportOCR_Click(object sender, EventArgs e)
        {

        }
    }


    public class SurveyTemplate
    {
        public List<string> Questions { get; set; }
    }

}
