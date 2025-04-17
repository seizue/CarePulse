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

        public Survey(string id)
        {
            InitializeComponent();
            respondentId = id;  
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
                row.Height = 35;
            }

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

            LoadResponseOptions();
        }

        public void SetSurveyQuestions(List<string> questions)
        {
            datagridSurvey.Rows.Clear();

            // Make the questions column read-only
            datagridSurvey.Columns["surveyQuestions"].ReadOnly = true;

            int idCounter = 1;
            foreach (string question in questions)
            {
                int index = datagridSurvey.Rows.Add();
                datagridSurvey.Rows[index].Cells["surveyQuestions"].Value = question;
                datagridSurvey.Rows[index].Cells["surveyID"].Value = $"Q{idCounter:D3}";
                idCounter++;
            }
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
            // Collect survey responses
            var responses = new Dictionary<string, string>();
            foreach (DataGridViewRow row in datagridSurvey.Rows)
            {
                if (!row.IsNewRow)
                {
                    string questionId = row.Cells["surveyID"]?.Value?.ToString();
                    string answer = row.Cells["surveyResponse"]?.Value?.ToString();
                    if (!string.IsNullOrWhiteSpace(questionId))
                    {
                        responses[questionId] = answer ?? "";
                    }
                }
            }

            // Create a survey result object
            var surveyResult = new
            {
                RespondentID = respondentId,  // Store the respondent ID
                Date = DateTime.Now,          // Add a timestamp
                Answers = responses           // Add the responses
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

                        foreach (DataGridViewRow row in datagridSurvey.Rows)
                        {
                            if (!row.IsNewRow)
                            {
                                string id = row.Cells["surveyID"]?.Value?.ToString()?.Replace(",", " ") ?? "";
                                string question = row.Cells["surveyQuestions"]?.Value?.ToString()?.Replace(",", " ") ?? "";
                                string response = row.Cells["surveyResponse"]?.Value?.ToString()?.Replace(",", " ") ?? "";

                                writer.WriteLine($"{id},{question},{response}");
                            }
                        }
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

    }

    public class SurveyTemplate
    {
        public List<string> Questions { get; set; }
    }

}
