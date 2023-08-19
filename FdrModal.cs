using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;

namespace eStrips
{
    public partial class FdrModal : Form
    {
        public static string callsign = string.Empty;

        public FdrModal()
        {
            InitializeComponent();
            TopMost = true;
        }

        private void FdrTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) 
            {
                if (FdrTextBox.Text == string.Empty) { return; }

                callsign = FdrTextBox.Text;

                FDR form = new FDR();
                form.Show();
                Close();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
    }
}
