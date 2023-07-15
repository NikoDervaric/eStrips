﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace eStrips
{
    public class Line
    {
        public Point Start { get; set; }
        public Point End { get; set; }
    }

    public class Point
    {
        public double X { get; set; }
        public double Y { get; set; }
    }

    public class Sector
    {
        public List<Point> Points { get; set; }
    }

    public class Waypoint
    {
        public string name { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public int altitude { get; set; }

        public Waypoint(string name, double  latitude, double longitude, int altitude)
        {
            this.name = name;
            this.latitude = latitude;
            this.longitude = longitude;
            this.altitude = altitude;
        }

        public override string ToString()
        {
            return $"{name} | {latitude};{longitude}\n @ A/FL{altitude}";
        }
    }

    internal struct Segment
    {
        public Waypoint W1 { get; set; }
        public Waypoint W2 { get; set; }
    }

    internal struct Flight
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
        public double ComputedCFL { get; set; }
        public int AppliedEFL { get; set; }
        public int AppliedXFL { get; set; }
        public string PrevSector { get; set; }
        public string NextSector { get; set; }
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

            if (int.TryParse(AltLbl, out int _)) { ComputedCFL = int.Parse(AltLbl); }
            else if (WptLbl.Substring(0, 3) == "ILS" || AltLbl == "LND") { ComputedCFL = 0; }
            else { ComputedCFL = Flightplan.CruiseAlt; }

            /*if (AppliedEFL == 0) { ComputedCFL = AppliedEFL; }*/
            if (AppliedXFL == 0)
            {
                AppliedXFL = Flightplan.CruiseAlt;
                Console.WriteLine(Callsign + " AppliedXFL: " + AppliedXFL);
            }

            return new string[] { $"{Callsign}", $"{ComputedCFL.ToString().PadLeft(3, '0').Substring(0, 3)}", $"{AppliedXFL.ToString().PadLeft(3, '0').Substring(0, 3)}", 
                                    $"                   ", $"{WptLbl.Substring(0, 3)}", $"{Flightplan.CruiseAlt}", $"{/*PrevSector*/Flightplan.AcType}", $"{Flightplan.CruiseSpd}", 
                                    $"{Flightplan.Adep}", $"{Flightplan.Ades}", $"{ASSR}", $"{PSSR}", $"{string.Join(" ", Flightplan.Route)}" };
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