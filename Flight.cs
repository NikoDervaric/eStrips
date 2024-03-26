using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

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
        public List<int> Loa_efls { get; set; }
        public int AppliedEFL { get; set; }
        public int ChangedEFL { get; set; }
        public List<int> Loa_xfls { get; set; }
        public int AppliedXFL { get; set; }
        public int ChangedXFL { get; set; }
        public string InboundSector { get; set; }
        public string OutboundSector { get; set; }
        public List<Line> SystemRoute { get; set; }
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

            ChangedXFL = 0;
            AppliedXFL = ApplyXFL();
            if (ChangedXFL != 0) { AppliedXFL = ChangedXFL; }
            //AppliedEFL = ApplyEFL();

            return new string[] { $"{Callsign}", $"{AppliedEFL.ToString().PadLeft(3, '0').Substring(0, 3)}", $"{AppliedXFL.ToString().PadLeft(3, '0').Substring(0, 3)}", 
                                    $"{InboundSector} -> {OutboundSector}", $"{WptLbl.Substring(0, 3)}", $"{Flightplan.CruiseAlt}", $"{Flightplan.AcType}", $"{Flightplan.CruiseSpd}", 
                                    $"{Flightplan.Adep}", $"{Flightplan.Ades}", $"{ASSR}", $"{PSSR}", $"{string.Join(" ", Flightplan.Route)}" };
        }

        // XFL > Cruise FL = Cruise FL
        // CFL > XFL => XFL = CFL        *Confirm box for XFL
        // Najbolj restriktivnega
        public int ApplyXFL()
        {
            if (Loa_xfls.Count == 0) { AppliedXFL = Flightplan.CruiseAlt; }
            else { AppliedXFL = Loa_xfls.Min(); }
            //Logging.Log($"XFL = XFL: {AppliedXFL}");

            // If XFL is greater than cruise altitude, the XFL will be set to cruise alt
            if (AppliedXFL > Flightplan.CruiseAlt && AppliedXFL != ComputedFL) { AppliedXFL = Flightplan.CruiseAlt; }
            if (ChangedXFL != 0) { AppliedXFL = ChangedXFL; }

            //if (Flightplan.Ades == "LJLJ") { return ""; }

            Netcode.SendCommand($"#LBALR;{AppliedXFL}");
            return AppliedXFL;
        }

        public int ApplyEFL()
        {
            string cwd = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string[] filePath = Directory.GetFiles($"{cwd}/sectors", "*.msct");
            var lines = File.ReadAllLines(filePath[0]);
            var sector = new Sector { Points = new List<Point>() };

            foreach (var line in lines)
            {
                if (line.Substring(0, 2) == "//") { continue; }

                var coordinates = line.Split(';');
                if (coordinates.Length == 2 && double.TryParse(coordinates[0], out double _) && double.TryParse(coordinates[1], out double _))
                {
                    sector.Points.Add(new Point(Math.Round(Convert.ToDouble(coordinates[0]), 5), Math.Round(Convert.ToDouble(coordinates[1]), 5)));
                }
                else
                {
                    throw new FormatException("Invalid coordinate format in file.");
                }
            }

            Point p1 = new Point(Position.latitude, Position.longitude);
            Point p2 = new Point(Position.latitude, Position.longitude);
            Line FlightPos = new Line(p1, p2);

            if (!eStrips.IntersectsSector(FlightPos, sector))
            {
                AppliedEFL = ComputedFL;

                for (int i = 0; i < Loa_efls.Count;)
                {
                    if (Loa_efls[i] < AppliedEFL) { AppliedEFL = Loa_efls[i]; }
                }

                if (int.TryParse(AltLbl, out int CFL) && (CFL < ComputedFL || CFL < Flightplan.CruiseAlt))
                {
                    AppliedEFL = CFL;
                }
            }
            return AppliedEFL;
        }
    }
}
