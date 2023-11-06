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
        public static string callsign;

        public FlightHandler(string csn)
        {
            InitializeComponent();
            instance = this;
            callsign = csn;
            instance.TopMost = true;
            instance.StartPosition = FormStartPosition.Manual;
            instance.Left = Cursor.Position.X;
            instance.Top = Cursor.Position.Y;
            SetCursorOnBtnDelete();
        }

        private void SetCursorOnBtnDelete() {
            // Set the Current cursor, move the cursor's Position,
            // and set its clipping rectangle to the form. 

            this.Cursor = new Cursor(Cursor.Current.Handle);
            Cursor.Position = new System.Drawing.Point(Cursor.Position.X + 65, Cursor.Position.Y + 80);
            Cursor.Clip = new Rectangle(this.Location, this.Size);
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            eStrips.exclusionList.Add(callsign);
            instance.Close();
        }

        private void BtnFdr_Click(object sender, EventArgs e)
        {
            FDR form = new FDR(callsign);
            form.Show();
            Close();
        }

        private void FlightHandler_Deactivate(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
