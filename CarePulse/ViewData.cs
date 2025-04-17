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
          //  txtAnswers.Text = answers;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
