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
    public partial class EntryNew : Form
    {
        public EntryNew()
        {
            InitializeComponent();
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
    }
}
