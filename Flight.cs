using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace eStrips
{
    internal struct Coordinate
    {
        public string name;
        public double latitude;
        public double longitude;
        public int altitude;

        public override string ToString()
        {
            return $"{name} | {latitude};{longitude}\n";
        }
    }

    internal struct Segment
    {
        public Coordinate Wpt1 { get; set; }
        public Coordinate Wpt2 { get; set; }
    }

    internal struct Flight
    {

        public string Callsign { get; set; }
        public int Heading { get; set; }
        public int Track { get; set; }
        public int Altitude { get; set; }
        public int Speed { get; set; }
        public Coordinate Position { get; set; }
        public string ASSR { get; set; }
        public string PSSR { get; set; }
        public string WptLbl { get; set; }
        public string AltLbl { get; set; }
        public string SpdLbl { get; set; }
        public Flightplan Flightplan { get; set; }
        public double ComputedCFL { get; set; }
        public int AppliedXFL { get; set; }

        // Functions
        public override string ToString()
        {
            return $"Callsign: {Callsign}, Heading: {Heading}, Track: {Track}, Altitude: {Altitude}, Speed: {Speed}, Coordinate: {Position}, ASSR: {ASSR}, PSSR: {PSSR}, WptLbl: {WptLbl}, AltLbl: {AltLbl}, SpdLbl: {SpdLbl}\n";
                //$"{Callsign}    |    {Flightplan.ToString()}";
        }

        public string[] ShowFlight()
        {

            if (AltLbl != "") { ComputedCFL = int.Parse(AltLbl); } 
            else { ComputedCFL = Flightplan.CruiseAlt; }
            Console.WriteLine(ComputedCFL);

            return new string[] { $"{Callsign}", $"{ComputedCFL.ToString().PadLeft(3, '0').Substring(0, 3)}", $"{Flightplan.CruiseAlt}", $"VEK             SAB", $"{WptLbl.Substring(0, 3)}", $"{Flightplan.CruiseAlt}", $"{Flightplan.AcType}", $"{Flightplan.CruiseSpd}", $"{Flightplan.Adep}", $"{Flightplan.Ades}", $"{ASSR}", $"{PSSR}", $"{string.Join(" ", Flightplan.Route)}" };
        }
    }

    internal struct Flightplan
    {
        public string Adep { get; set; }
        public string Ades { get; set; }
        public string Altn { get; set; }
        public string Etd { get; set; }
        public string AcType { get; set; }
        public string Wtc { get; set; }
        public string FlightType { get; set; }
        public string FlightRules { get; set; }
        public string Equipment { get; set; }
        public int CruiseAlt { get; set; }
        public string CruiseSpd { get; set; }
        public string Endurance { get; set; }
        public string EstimatedFlightTime { get; set; }
        public string[] Route { get; set; }
        public string Remarks { get; set; }

        public override string ToString()
        {
            return $"Adep: {Adep}, Ades: {Ades}, Altn: {Altn}, Etd: {Etd}, AcType: {AcType}, Wtc: {Wtc}, FlightType: {FlightType}, FlightRules: {FlightRules}, Equipment: {Equipment}, CruiseAlt: {CruiseAlt}, CruiseSpd: {CruiseSpd}, Endurance: {Endurance}, EstimatedFlightTime: {EstimatedFlightTime}, Remarks: {Remarks}";
        }
    }
}
