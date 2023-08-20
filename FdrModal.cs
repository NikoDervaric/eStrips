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
                if (FdrTextBox.Text == string.Empty) { return; }
                if (FdrTextBox.Text != string.Empty) { callsign = FdrTextBox.Text; }
                if (eStrips.exclusionList.Contains(FlightHandler.callsign)) { callsign = FlightHandler.callsign; }

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
}
