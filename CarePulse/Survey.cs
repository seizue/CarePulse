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
    public partial class Survey : Form
    {
        public Survey()
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

        private void Survey_Load(object sender, EventArgs e)
        {
            // Set row height
            datagridSurvey.RowTemplate.Height = 35;

            // Apply height to already added rows
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
        }

        private void btnResponseOption_Click(object sender, EventArgs e)
        {
            // Prompt user to enter the new response option
            string newOption = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter a new survey response option:",
                "Add Survey Response Option",
                ""
            );

            // If user cancels or leaves it blank, do nothing
            if (string.IsNullOrWhiteSpace(newOption))
            {
                return;
            }

            // Access the ComboBox column
            var comboCol = datagridSurvey.Columns["surveyResponse"] as DataGridViewComboBoxColumn;

            if (comboCol != null)
            {
                // Add the new option if it's not already in the list
                if (!comboCol.Items.Contains(newOption))
                {
                    comboCol.Items.Add(newOption);
                }

                // Optional: Set the new option for all rows
                foreach (DataGridViewRow row in datagridSurvey.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        row.Cells["surveyResponse"].Value = newOption;
                    }
                }
            }
            else
            {
                MessageBox.Show("The 'surveyResponse' column is not a ComboBox column.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}
