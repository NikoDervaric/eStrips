using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace eStrips
{
    public struct Flight
    {
        public string Callsign { get; set; }
        public int Heading { get; set; }
        public int Track { get; set; }
        public int Altitude { get; set; }
        public int Speed { get; set; }
        public Waypoint Position { get; set; }
        public string ASSR { get; set; }
        public string PSSR { get; set; }
        public string WptLbl { get; set; }
        public string AltLbl { get; set; }
        public string SpdLbl { get; set; }
        public Flightplan Flightplan { get; set; }
        public int ComputedFL { get; set; }
        public int AppliedEFL { get; set; }
        public List<int> Loa_xfls { get; set; }
        public int AppliedXFL { get; set; }
        public int ChangedXFL { get; set; }
        public string InboundSector { get; set; }
        public string OutboundSector { get; set; }
        public List<Line> Route { get; set; }
        public List<Line> FRF { get; set; }

        // Functions
        public override string ToString()
        {
            return $"Callsign: {Callsign}, Heading: {Heading}, Track: {Track}, Altitude: {Altitude}, Speed: {Speed}, Coordinate: {Position}, ASSR: {ASSR}, PSSR: {PSSR}, WptLbl: {WptLbl}, AltLbl: {AltLbl}, SpdLbl: {SpdLbl}\n";
                //$"{Callsign}    |    {Flightplan.ToString()}";
        }

        public string[] ShowFlight()
        {
            if (int.TryParse(AltLbl, out int CFL) && CFL < Flightplan.CruiseAlt) { AppliedEFL = CFL; }
            else { AppliedEFL = ComputedFL; }

            AppliedXFL = ApplyXFL();

            return new string[] { $"{Callsign}", $"{AppliedEFL.ToString().PadLeft(3, '0').Substring(0, 3)}", $"{AppliedXFL.ToString().PadLeft(3, '0').Substring(0, 3)}", 
                                    $"                   ", $"{WptLbl.Substring(0, 3)}", $"{Flightplan.CruiseAlt}", $"{Flightplan.AcType}", $"{Flightplan.CruiseSpd}", 
                                    $"{Flightplan.Adep}", $"{Flightplan.Ades}", $"{ASSR}", $"{PSSR}", $"{string.Join(" ", Flightplan.Route)}" };
        }

        // XFL > Cruise FL = Cruise FL
        // CFL > XFL => XFL = CFL        *Confirm box for XFL
        // Najbolj restriktivnega

        public int ApplyXFL()
        {
            AppliedXFL = ComputedFL;

            // If XFL is greater than cruise altitude, the XFL will be set to cruise alt
            if (AppliedXFL > Flightplan.CruiseAlt) { AppliedXFL = Flightplan.CruiseAlt; }
            
            for (int i = 0; i < Loa_xfls.Count;)
            {
                if (Loa_xfls[i] < AppliedXFL) { AppliedXFL = Loa_xfls[i]; }
            }

            //  TODO!
            //if (ChangedXFL != null) { ChangedXFL = 0; }

            return AppliedXFL;
        }
    }
}
