using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarePulse
{
    public partial class ViewData: Form
    {
        public ViewData(string respondentID, string patientName, string surveyScore, string date, string month, string year, string patientFeedback, string surveyTemplate, string answers)
        {
            InitializeComponent();

            // Set the data to the corresponding text boxes
            txtBoxIDNo.Text = respondentID;
            txtPatientName.Text = patientName;
            txtSurveyScore.Text = surveyScore;
            txtDateSurvey.Text = date;
            txtMonth.Text = month;
            txtYear.Text = year;
            txtFeedbacks.Text = patientFeedback;
            txtSurveyTemplate.Text = surveyTemplate;

            // Populate the datagridSurveyView with the answers
            if (!string.IsNullOrWhiteSpace(answers))
            {
                var answersDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(answers);

                if (answersDict != null)
                {
                    foreach (var answer in answersDict)
                    {
                        int rowIndex = datagridSurveyView.Rows.Add();
                        DataGridViewRow row = datagridSurveyView.Rows[rowIndex];
                        row.Cells["datagridQuestions"].Value = answer.Key; // Question
                        row.Cells["datagridAnswers"].Value = answer.Value; // Answer
                    }
                }
            }
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void labelSurveyQuestions_Click(object sender, EventArgs e)
        {
            datagridSurveyView.Visible = true;
            labelSurveyQuestions.ForeColor = Color.SaddleBrown;
            labelFeedback.ForeColor = Color.Gray;
        }

        private void labelFeedback_Click(object sender, EventArgs e)
        {
            datagridSurveyView.Visible = false;
            labelFeedback.ForeColor = Color.SaddleBrown;
            labelSurveyQuestions.ForeColor = Color.Gray;
        }

        private void btnSurveyTemplate_Click(object sender, EventArgs e)
        {
            string selectedTemplate = txtSurveyTemplate.Text;
            NewTemplates newTemplates = new NewTemplates(selectedTemplate);
            newTemplates.ShowDialog();
        }
    }
}
