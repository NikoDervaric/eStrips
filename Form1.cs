using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Timers;
using System.Threading;
using System.Security.Permissions;
using System.Runtime.CompilerServices;
using System.Windows.Shapes;
using static System.Windows.Forms.LinkLabel;
using System.Collections;
using System.Xml.Linq;
using System.Collections.ObjectModel;

namespace eStrips
{
    public partial class eStrips : Form
    {
        private System.Timers.Timer timer;
        private const string serverAddress = "127.0.0.1";       // Replace with your server IP address
        private const int serverPort = 1130;                    // Replace with your server port number
        private const int sendInterval = 5000;                 // Interval between sending messages (in milliseconds)

        private List<Flight> validFlights = new List<Flight>();
        private readonly Dictionary<string, Point> Airac;
        private readonly Sector mainSector;

        //LoAs
        //WRITE THE LOASSS
        // Dictionary<FIX, Dictionary<AIRPORT, WAYPOINT(LAT, LON, LEVEL)>>
        private readonly Dictionary<string, int> Departures = new Dictionary<string, int>();
        private readonly Dictionary<string, int> Arrivals = new Dictionary<string, int>();
        //private readonly Dictionary<string, int> Overflights;

        public eStrips()
        {
            InitializeComponent();

            mainSector = LoadMainSector();
            Log("Loaded sector.");
            Airac = LoadAirac();
            Log("Loaded AIRAC.");
            LoadLoAs();
            Log("Loaded LOAs.");

            StyleStripTable();

            //PopulateDataGridView();
            timer = new System.Timers.Timer(sendInterval);

            // Hook up the Elapsed event handler
            timer.Elapsed += TimerElapsed;

            // Set the timer to continuously repeat
            timer.AutoReset = true;

            // Start the timer
            timer.Start();
        }
        //  STYLING
        private void StyleStripTable()
        {
            //stripDataTable.RowHeadersVisible = false;
            stripDataTable.RowHeadersWidth = 25;

            //Change cell font
            foreach (DataGridViewColumn c in stripDataTable.Columns)
            {
                c.DefaultCellStyle.Font = new Font("Segoe UI Semibold", 15, GraphicsUnit.Pixel);
                c.DefaultCellStyle.BackColor = Color.FromArgb(255, 180, 180, 180);
            }

            stripDataTable.Columns["callsign_column"].DefaultCellStyle.Font = new Font("Segoe UI Semibold", 23, GraphicsUnit.Pixel);
            stripDataTable.Columns["planned_cleared_levels_column"].DefaultCellStyle.Font = new Font("Segoe UI Semibold", 23, GraphicsUnit.Pixel);
            stripDataTable.Columns["xfl_column"].DefaultCellStyle.Font = new Font("Segoe UI Semibold", 23, GraphicsUnit.Pixel);

            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle();
            headerStyle.Font = new Font("Segoe UI Semibold", 15, FontStyle.Bold);
            stripDataTable.Columns[4].HeaderCell.Style = headerStyle;

            stripDataTable.Columns["planned_cleared_levels_column"].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 230, 80);
            stripDataTable.Columns["xfl_column"].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 230, 80);

            stripDataTable.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            stripDataTable.GridColor = Color.Black;

            stripDataTable.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 15, GraphicsUnit.Pixel);

            stripDataTable.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 180, 180, 180);
            stripDataTable.DefaultCellStyle.SelectionForeColor = Color.Black;
        }

        //  LOGGING
        private static void Log(string str)
        {
            Console.WriteLine(DateTime.Now.ToString("h:mm:ss:ff") + " | " + str);
        }
        
        // TIMING AND BASIC PROGRAM FUNCTIONALITY
        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            // Function to execute periodically
            string response = SendCommand("#TR");

            // Process the response or perform any other actions
            // Note: Make sure to update UI controls from the main UI thread using Invoke if necessary
            Invoke(new Action(() =>
            {
                // Update UI controls or display the response

                //List<Flight> validFlights = ValidateFlights(response);
                PopulateDataGridView(validFlights);
                stripDataTable.Sort(stripDataTable.Columns["planned_cleared_levels_column"], ListSortDirection.Descending);
            }));
        }

        //  Open route on double-click
        private void stripDataTable_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 12)
            {
                DataGridViewTextBoxCell route = (DataGridViewTextBoxCell) stripDataTable.Rows[e.RowIndex].Cells[e.ColumnIndex];
                DataGridViewTextBoxCell adep = (DataGridViewTextBoxCell)stripDataTable.Rows[e.RowIndex].Cells[e.ColumnIndex - 4];
                DataGridViewTextBoxCell ades = (DataGridViewTextBoxCell)stripDataTable.Rows[e.RowIndex].Cells[e.ColumnIndex - 3];

                string[] parts = route.Value.ToString().Split(' ');

                string fullRoute = adep.Value.ToString() + "%20" + string.Join("%20", parts) + "%20" + ades.Value.ToString();
                //Log(fullRoute);
                
                System.Diagnostics.Process.Start($"https://skyvector.com/?ll=46.12757152789378,14.788330080764661&chart=304&zoom=5&fpl={fullRoute}");
            }
            else { return; }
        }

        //  Generate squawk on clicking PSSR field
        private void stripDataTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 10)
            {
                DataGridViewTextBoxCell pssr = (DataGridViewTextBoxCell)stripDataTable.Rows[e.RowIndex].Cells[e.ColumnIndex];
                DataGridViewTextBoxCell callsign = (DataGridViewTextBoxCell)stripDataTable.Rows[e.RowIndex].Cells[e.ColumnIndex - 10];

                string[] squawk = SendCommand($"#TRSQK;{callsign.Value}").Split(';');
                if (squawk[2] == "Traffic not assumed.") 
                {
                    Log("Here:" + callsign.Value.ToString());
                    pssr.Value = ""; 
                }
                else
                {
                    
                    pssr.Value = squawk[2];
                    SendCommand($"#LBSQK;{callsign.Value};{squawk[2]}");
                }
            }
        }

        public static Dictionary<string, Point> LoadAirac()
        {
            string[] lines = File.ReadAllLines(@"..\..\airac.txt");
            var dictionary = new Dictionary<string, Point>();

            foreach (var line in lines)
            {
                string[] parts = line.Split(';');

                if (parts.Length == 3)
                {
                    string name = parts[0];
                    double latitude;
                    double longitude;

                    if (double.TryParse(parts[1], out latitude) && double.TryParse(parts[2], out longitude))
                    {
                        Point point = new Point { X = latitude, Y = longitude };
                        dictionary[name] = point;
                    }
                    else
                    {
                        Console.WriteLine($"Invalid latitude or longitude in line: {line}");
                    }
                }
                else
                {
                    Console.WriteLine($"Invalid line format: {line}");
                }
            }

            return dictionary;
        }

        public Sector LoadMainSector()
        {
            string filePath = "../../sectors/LJLA.msct";
            var lines = File.ReadAllLines(filePath);
            var sector = new Sector { Points = new List<Point>() };

            foreach (var line in lines)
            {
                var coordinates = line.Split(',');
                if (coordinates.Length == 2 && double.TryParse(coordinates[0], out double latitude) && double.TryParse(coordinates[1], out double longitude))
                {
                    sector.Points.Add(new Point { X = latitude, Y = longitude });
                }
                else
                {
                    throw new FormatException("Invalid coordinate format in file.");
                }
            }

            return sector;
        }

        /*public static bool HasIntersection(List<Line> lines, Sector sector)
        {
            foreach (var line in lines)
            {
                if (IntersectsSector(line, sector))
                {
                    //Log($"{line.Start.X};{line.Start.Y} | {line.End.X};{line.End.Y}");
                    return true;
                }
            }

            return false;
        }

        private static bool IntersectsSector(Line line, Sector sector)
        {
            foreach (var SectorLine in GetSectorLines(sector))
            {
                if (Intersects(line, SectorLine))
                    return true;
            }

            return false;
        }

        private static List<Line> GetSectorLines(Sector sector)
        {
            var lines = new List<Line>();
            var count = sector.Points.Count;

            for (int i = 0; i < count; i++)
            {
                var startPoint = sector.Points[i];
                var endPoint = sector.Points[(i + 1) % count];

                lines.Add(new Line { Start = startPoint, End = endPoint });
            }

            return lines;
        }

        private static bool Intersects(Line line1, Line line2, double tolerance = 0.001)
        {
            //  Returns Point of intersection if do intersect otherwise default Point (null)
            double x1 = line1.Start.X, y1 = line1.Start.Y;
            double x2 = line1.End.X, y2 = line1.End.Y;

            double x3 = line2.Start.X, y3 = line2.Start.Y;
            double x4 = line2.End.X, y4 = line2.End.Y;

            // equations of the form x=c (two vertical lines) with overlapping
            if (Math.Abs(x1 - x2) < tolerance && Math.Abs(x3 - x4) < tolerance && Math.Abs(x1 - x3) < tolerance)
            {
                return true;
            }

            //equations of the form y=c (two horizontal lines) with overlapping
            if (Math.Abs(y1 - y2) < tolerance && Math.Abs(y3 - y4) < tolerance && Math.Abs(y1 - y3) < tolerance)
            {
                return true;
            }

            //equations of the form x=c (two vertical parallel lines)
            if (Math.Abs(x1 - x2) < tolerance && Math.Abs(x3 - x4) < tolerance)
            {
                //return default (no intersection)
                return false;
            }

            //equations of the form y=c (two horizontal parallel lines)
            if (Math.Abs(y1 - y2) < tolerance && Math.Abs(y3 - y4) < tolerance)
            {
                //return default (no intersection)
                return false;
            }

            //general equation of line is y = mx + c where m is the slope
            //assume equation of line 1 as y1 = m1x1 + c1 
            //=> -m1x1 + y1 = c1 ----(1)
            //assume equation of line 2 as y2 = m2x2 + c2
            //=> -m2x2 + y2 = c2 -----(2)
            //if line 1 and 2 intersect then x1=x2=x & y1=y2=y where (x,y) is the intersection point
            //so we will get below two equations 
            //-m1x + y = c1 --------(3)
            //-m2x + y = c2 --------(4)

            double x, y;

            //lineA is vertical x1 = x2
            //slope will be infinity
            //so lets derive another solution
            if (Math.Abs(x1 - x2) < tolerance)
            {
                //compute slope of line 2 (m2) and c2
                double m2 = (y4 - y3) / (x4 - x3);
                double c2 = -m2 * x3 + y3;

                //equation of vertical line is x = c
                //if line 1 and 2 intersect then x1=c1=x
                //subsitute x=x1 in (4) => -m2x1 + y = c2
                // => y = c2 + m2x1 
                return true;
            }
            //lineB is vertical x3 = x4
            //slope will be infinity
            //so lets derive another solution
            else if (Math.Abs(x3 - x4) < tolerance)
            {
                //compute slope of line 1 (m1) and c2
                double m1 = (y2 - y1) / (x2 - x1);
                double c1 = -m1 * x1 + y1;

                //equation of vertical line is x = c
                //if line 1 and 2 intersect then x3=c3=x
                //subsitute x=x3 in (3) => -m1x3 + y = c1
                // => y = c1 + m1x3 
                return true;
            }
            //lineA & lineB are not vertical 
            //(could be horizontal we can handle it with slope = 0)
            else
            {
                //compute slope of line 1 (m1) and c2
                double m1 = (y2 - y1) / (x2 - x1);
                double c1 = -m1 * x1 + y1;

                //compute slope of line 2 (m2) and c2
                double m2 = (y4 - y3) / (x4 - x3);
                double c2 = -m2 * x3 + y3;

                //solving equations (3) & (4) => x = (c1-c2)/(m2-m1)
                //plugging x value in equation (4) => y = c2 + m2 * x
                x = (c1 - c2) / (m2 - m1);
                y = c2 + m2 * x;

                //verify by plugging intersection point (x, y)
                //in orginal equations (1) & (2) to see if they intersect
                //otherwise x,y values will not be finite and will fail this check
                if (!(Math.Abs(-m1 * x + y - c1) < tolerance
                    && Math.Abs(-m2 * x + y - c2) < tolerance))
                {
                    //return default (no intersection)
                    return false;
                }
            }

            //return default (no intersection)
            return false;
        }
        */
        
        private void LoadLoAs()
        {
            string[] lines = File.ReadAllLines(@"..\..\loa.txt");

            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                string key = parts[1] + parts[2];
                int loaFL = int.Parse(parts[3]);

                if (parts[0] == "A")
                {
                    Arrivals.Add(key, loaFL);
                }
                else if (parts[0] == "D")
                {
                    Departures.Add(key, loaFL);
                }
            }
        }

        public static string SendCommand(string command)
        {
            string sendCommand = command + "\n";
            try
            {
                using (TcpClient client = new TcpClient(serverAddress, serverPort))
                {
                    using (NetworkStream stream = client.GetStream())
                    {
                        byte[] data = Encoding.ASCII.GetBytes(sendCommand);
                        stream.Write(data, 0, data.Length);

                        // Receive the response
                        byte[] responseBuffer = new byte[4096];
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            int bytesRead;
                            do
                            {
                                bytesRead = stream.Read(responseBuffer, 0, responseBuffer.Length);
                                memoryStream.Write(responseBuffer, 0, bytesRead);
                            } while (stream.DataAvailable);

                            // Convert the received data to a string
                            string response = Encoding.ASCII.GetString(memoryStream.ToArray());
                            return response;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the process
                Console.WriteLine("An error occurred: " + ex.Message);
                Console.WriteLine("Make sure you are connected in Aurora and that 3rd party is enabled in settings.");
                return null;
            }
        }

        //TABLE CHECKING AND DATA POPULATION
        private void PopulateDataGridView(List<Flight> validFlights)  
        {
            stripDataTable.Rows.Clear();

            string response = SendCommand("#TR");
            ValidateFlights(response);

            // Add the data rows to the DataGridView
            foreach (Flight flight in validFlights)
            {
                stripDataTable.Rows.Add(flight.ShowFlight());
            }
        }

        //FLIGHT FILTERING AND PROCESSING
        private void ValidateFlights(string radarResponse)
        {
            validFlights.Clear();

            string[] trafficInRange = radarResponse.Split(';');
            trafficInRange = trafficInRange.Skip(1).ToArray();

            //Log("=====ValidateFlights()=====");
            foreach (string traffic in trafficInRange)
            {

                string fplTmp = SendCommand($"#FP;{traffic}");
                string[] fpl = fplTmp.Split(';');

                if (fpl[0] == "@ERR") continue;

                if (fpl[8] == "I" && fpl[11] != "VFR")
                {
                    string tmpPos = SendCommand($"#TRPOS;{traffic}");
                    string[] pos = tmpPos.Split(';');

                    if (!int.TryParse(pos[4], out int _) || !int.TryParse(pos[5], out int _)) { continue; }

                    Flight flight = new Flight
                    {
                        Callsign = traffic,
                        Heading = int.Parse(pos[2]),
                        Track = int.Parse(pos[3]),
                        Altitude = int.Parse(pos[4]),
                        Speed = int.Parse(pos[5]),
                        Position = new Waypoint($"{pos[6]};{pos[7]}", double.Parse(pos[6]), double.Parse(pos[7]), int.Parse(pos[4])),
                        ASSR = pos[9],
                        PSSR = pos[8],
                        WptLbl = pos[10].PadRight(5, ' '),
                        AltLbl = pos[11],
                        SpdLbl = pos[12],

                        Flightplan = new Flightplan
                        {
                            Adep = fpl[2],
                            Ades = fpl[3],
                            Altn = fpl[4],
                            Etd = fpl[5],
                            AcType = fpl[6],
                            Wtc = fpl[7],
                            FlightType = fpl[8],
                            FlightRules = fpl[9],
                            Equipment = fpl[10],
                            CruiseAlt = int.Parse(fpl[11].Substring(1)),
                            CruiseSpd = fpl[12],
                            Endurance = fpl[13],
                            EstimatedFlightTime = fpl[14],
                            Route = fpl[15].Split(' '),
                            Remarks = fpl[16]
                        },
                        AppliedXFL = 0
                    };
                    Log(flight.Callsign);
                    flight.Route = ProcessRoute(flight);
                    flight.AppliedXFL = ApplyLoA(flight);
                    Log(flight.AppliedXFL.ToString());

                    validFlights.Add(flight);

                    /*if (HasIntersection(flight.Route, mainSector)) 
                    {
                        Log(flight.Callsign + " has intersection");
                        validFlights.Add(flight);
                    }
                    else { continue; }*/
                }
                else { continue; }
            }
        }

        private List<Line> ProcessRoute(Flight flight)
        {
            string[] unprocessed_route = flight.Flightplan.Route;
            List<Line> route_segments = new List<Line>();
            List<Point> tmp_route = new List<Point>();

            foreach (string wpt in unprocessed_route)
            {
                string[] parts = wpt.Split('/');

                if (Airac.ContainsKey(parts[0]))
                {
                    Point point = Airac[parts[0]];
                    tmp_route.Add(point);
                }
                /*else
                {
                    Console.WriteLine($"Name '{wpt}' not found in the dictionary.");
                }*/
            }

            var count = tmp_route.Count;

            for (int i = 0; i < count; i++)
            {
                Point point1 = tmp_route[i];
                Point point2 = tmp_route[(i + 1) % count];

                route_segments.Add(new Line { Start = point1, End = point2 });
            }

            return route_segments;
        }

        private int ApplyLoA(Flight flight)
        {
            string ADEP = flight.Flightplan.Adep;
            string ADES = flight.Flightplan.Ades;

            foreach (string wpt in flight.Flightplan.Route)
            {
                string depKey = wpt + ADEP;
                string arrKey = wpt + ADES;

                if (Departures.ContainsKey(depKey))
                {
                    Log("LOA APPLIED to C/S: " + flight.Callsign + " DEP: " + ADEP + " WPT: " + wpt);
                    return Departures[depKey];
                }
                else if (Arrivals.ContainsKey(arrKey))
                {
                    Log("LOA APPLIED to C/S: " + flight.Callsign + " ARR: " + ADES + " WPT: " + wpt + " FL: " + Arrivals[arrKey]);
                    return Arrivals[arrKey];
                }
                else
                {
                    return flight.Flightplan.CruiseAlt;
                }
            }
            return 0;
        }

        //CLOSING
        private void EStrips_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Stop the timer
            timer.Stop();
            timer.Dispose();
        }

        private void eStrips_Load(object sender, EventArgs e)
        {
            TopMost = true;
            string response = SendCommand("#TR");

            //List<Flight> validFlights = ValidateFlights(response);
            ValidateFlights(response);
            PopulateDataGridView(validFlights);
            stripDataTable.Sort(stripDataTable.Columns["planned_cleared_levels_column"], ListSortDirection.Descending);
        }
    }
}