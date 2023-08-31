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
        public static FdrModal instance;

        public FdrModal()
        {
            InitializeComponent();
            instance = this;
            TopMost = true;
            instance.StartPosition = FormStartPosition.Manual;
            instance.Left = Cursor.Position.X;
            instance.Top = Cursor.Position.Y;
        }

        private void FdrTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;

                if (FdrTextBox.Text == string.Empty) 
                {
                    LblInfo.Text = "Enter a flight!";
                    return; 
                }

                if (!eStrips.validFlights.ContainsKey(FdrTextBox.Text)) 
                {
                    LblInfo.Text = "Flight not found!";
                    return; 
                };

                FDR form = new FDR(FdrTextBox.Text);
                form.Show();
                Close();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }
    }
}
