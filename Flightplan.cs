using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eStrips
{
    public struct Flightplan
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
