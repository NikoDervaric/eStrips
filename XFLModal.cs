using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eStrips
{
    public partial class XFLModal : Form
    {
        public static XFLModal instance;
        public static string callsign;

        public XFLModal(string csn)
        {
            InitializeComponent();
            instance = this;
            TopMost = true;
            callsign = csn;
            LblCallsign.Text = callsign;
            instance.StartPosition = FormStartPosition.Manual;
            instance.Left = Cursor.Position.X;
            instance.Top = Cursor.Position.Y;
            SetCursorOnBtnDelete();

            //  Sets XFL value in CbXFLSelect
            CbXFLSelect.Text = eStrips.validFlights[callsign].AppliedXFL.ToString();

            //  Sets RFL value in LblRFL
            LblRFL.Text = eStrips.validFlights[callsign].Flightplan.CruiseAlt.ToString().Substring(0,3);
        }

        private void XFLModal_Deactivate(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SetCursorOnBtnDelete()
        {
            // Set the Current cursor, move the cursor's Position,
            // and set its clipping rectangle to the form. 

            this.Cursor = new Cursor(Cursor.Current.Handle);
            Cursor.Position = new System.Drawing.Point(Cursor.Position.X + 67, Cursor.Position.Y + 85);
            Cursor.Clip = new Rectangle(this.Location, this.Size);
        }

        private void BtnConfirmXFL_Click(object sender, EventArgs e)
        {
            eStrips.validFlights[callsign] = new Flight
            {
                Callsign = callsign,
                Heading = eStrips.validFlights[callsign].Heading,
                Track = eStrips.validFlights[callsign].Track,
                Altitude = eStrips.validFlights[callsign].Altitude,
                Speed = eStrips.validFlights[callsign].Speed,
                Position = eStrips.validFlights[callsign].Position,
                ASSR = eStrips.validFlights[callsign].ASSR,
                PSSR = eStrips.validFlights[callsign].PSSR,
                WptLbl = eStrips.validFlights[callsign].WptLbl,
                AltLbl = eStrips.validFlights[callsign].AltLbl,
                SpdLbl = eStrips.validFlights[callsign].SpdLbl,

                Flightplan = new Flightplan
                {
                    Adep = eStrips.validFlights[callsign].Flightplan.Adep,
                    Ades = eStrips.validFlights[callsign].Flightplan.Ades,
                    Altn = eStrips.validFlights[callsign].Flightplan.Altn,
                    Etd = eStrips.validFlights[callsign].Flightplan.Etd,
                    AcType = eStrips.validFlights[callsign].Flightplan.AcType,
                    Wtc = eStrips.validFlights[callsign].Flightplan.Wtc,
                    FlightType = eStrips.validFlights[callsign].Flightplan.Wtc,
                    FlightRules = eStrips.validFlights[callsign].Flightplan.FlightRules,
                    Equipment = eStrips.validFlights[callsign].Flightplan.Equipment,
                    CruiseAlt = eStrips.validFlights[callsign].Flightplan.CruiseAlt,
                    CruiseSpd = eStrips.validFlights[callsign].Flightplan.CruiseSpd,
                    Endurance = eStrips.validFlights[callsign].Flightplan.Endurance,
                    EstimatedFlightTime = eStrips.validFlights[callsign].Flightplan.EstimatedFlightTime,
                    Route = eStrips.validFlights[callsign].Flightplan.Route,
                    Remarks = eStrips.validFlights[callsign].Flightplan.Remarks
                },
                ChangedXFL = int.Parse(CbXFLSelect.Text)
            };

            this.Close();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
