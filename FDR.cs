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
    public partial class FDR : Form
    {
        public static FDR instance;
        public static Flight flight = eStrips.validFlights.Find(f => f.Callsign == FdrModal.callsign);

        public FDR()
        {
            InitializeComponent();
            instance = this;
            TopMost = true;
            instance.StartPosition = FormStartPosition.Manual;
            instance.Left = Cursor.Position.X;
            instance.Top = Cursor.Position.Y;
        }

        private void FDR_Load(object sender, EventArgs e)
        {
            LblCallsign.BackColor = Color.FromArgb(0, 235, 100, 100);

            if (flight.Route == null) { return; }
            LblCallsign.Text = FdrModal.callsign;
            FdrAC.Text = flight.Flightplan.AcType;
            FdrWtc.Text = flight.Flightplan.Wtc;
            FdrRules.Text = flight.Flightplan.FlightRules;
            FdrType.Text = flight.Flightplan.FlightType;
            FdrAdep.Text = flight.Flightplan.Adep;
            FdrAdes.Text = flight.Flightplan.Ades;
            FdrRmk.Text = flight.Flightplan.Remarks;
            FdrSpd.Text = flight.Flightplan.CruiseSpd;
            FdrFl.Text = flight.Flightplan.CruiseAlt.ToString();
            FdrEq.Text = flight.Flightplan.Equipment;
            FdrAltn.Text = flight.Flightplan.Altn;
            FdrAltn2.Text = string.Empty;
            FdrRoute.Text = string.Join(" ", flight.Flightplan.Route);

            if (eStrips.exclusionCallsign.Contains(FdrModal.callsign))
            { panel1.BackColor = Color.FromArgb(255, 206, 255, 206); }
            else { panel1.BackColor = Color.FromArgb(100, 235, 100, 100); }
        }

        private void BtnReactivateFlight_Click(object sender, EventArgs e)
        {
            if (!eStrips.exclusionList.Contains(FdrModal.callsign)) { return; }

            eStrips.exclusionList.Remove(LblCallsign.Text);
            panel1.BackColor = Color.FromArgb(255, 206, 255, 206);
        }

        private void BtnDeactivateFlight_Click(object sender, EventArgs e)
        {
            if (eStrips.exclusionList.Contains(FdrModal.callsign)) { return; }

            eStrips.exclusionList.Add(LblCallsign.Text);
            panel1.BackColor = Color.FromArgb(100, 235, 100, 100);
        }
    }
}
