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
    public partial class SelectedSurvey: Form
    {
        private string respondentId;
        private int currentPage = 1;
        private int totalPages = 1;
        private int rowsPerPage = 10;
        private List<string> surveyQuestionList = new List<string>();
        public SelectedSurvey(string id)
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

        private void SelectedSurvey_Load(object sender, EventArgs e)
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

            // Initialize pagination
            CalculatePagination();
            LoadPage(currentPage);
        }

        public void SetSurveyQuestions(List<string> questions)
        {
            datagridSurvey.Rows.Clear();

            // Make the questions column read-only
            datagridSurvey.Columns["surveyQuestionss"].ReadOnly = true;


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

        private void LoadPage(int pageNumber)
        {
            datagridSurvey.Rows.Clear();

            // Calculate the range of questions to display
            int startIndex = (pageNumber - 1) * rowsPerPage;
            int endIndex = Math.Min(startIndex + rowsPerPage, surveyQuestionList.Count);

            // Add questions to the DataGridView
            for (int i = startIndex; i < endIndex; i++)
            {
                int index = datagridSurvey.Rows.Add();
                datagridSurvey.Rows[index].Cells["surveyQuestionss"].Value = surveyQuestionList[i];
                datagridSurvey.Rows[index].Cells["surveyID"].Value = $"Q{i + 1:D3}";
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

        private void SelectedSurvey_Resize(object sender, EventArgs e)
        {
            CalculatePagination();
            LoadPage(currentPage);
        }
    }
}
