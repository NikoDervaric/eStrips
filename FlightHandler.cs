using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eStrips
{
    public partial class FlightHandler : Form
    {
        public static FlightHandler instance;

        public FlightHandler()
        {
            InitializeComponent();
            instance = this;
            instance.TopMost = true;
            instance.StartPosition = FormStartPosition.Manual;
            instance.Left = System.Windows.Forms.Cursor.Position.X;
            instance.Top = System.Windows.Forms.Cursor.Position.Y;
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            string callsign = eStrips.exclusionCallsign;
            eStrips.exclusionList.Add(callsign);
            instance.Close();
        }
    }
}
