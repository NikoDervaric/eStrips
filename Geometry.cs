using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace eStrips
{
    public class Point
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }

    public class Line
    {
        public Point Start { get; set; }
        public Point End { get; set; }

        public Line(Point start, Point end)
        {
            Start = start;
            End = end;
        }

        public override string ToString()
        {
            return $"{Start} ->- {End}";
        }
    }

    public class Waypoint
    {
        public string name { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public int altitude { get; set; }

        public Waypoint(string name, double latitude, double longitude, int altitude)
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
}
