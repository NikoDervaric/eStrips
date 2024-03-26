using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net.Sockets;
using System.Timers;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Xml.Linq;

namespace eStrips
{
    public partial class eStrips : Form
    {
        private static string cwd = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        private System.Timers.Timer timer;
        public static int sendInterval = 15000;                     // Interval between sending messages (in milliseconds)

        public static Dictionary<string, Flight> validFlights = new Dictionary<string, Flight>();
        private readonly Dictionary<string, Point> Airac;
        private readonly Sector mainSector;
        private readonly List<Sector> sectors = new List<Sector>();

        public static List<string> allFlights = new List<string>();

        //Exclusion List and string
        public static List<string> exclusionList = new List<string>();
        public static string exclusionCallsign = string.Empty;

        //LoAs
        //WRITE THE LOASSS
        // Dictionary<FIX, Dictionary<AIRPORT, WAYPOINT(LAT, LON, LEVEL)>>
        private readonly Dictionary<string, int> EntryLevels = new Dictionary<string, int>();
        private readonly Dictionary<string, int> ExitLevels = new Dictionary<string, int>();
        private readonly Dictionary<string, int> Overflights = new Dictionary<string, int>();

        public eStrips()
        {
            InitializeComponent();

            mainSector = LoadMainSector();
            Airac = LoadAirac();
            Logging.Log("Loaded AIRAC");
            LoadLoAs();
            Logging.Log("Loaded LoAs");
            sectors = LoadSectors();

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

            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle
            {
                Font = new Font("Segoe UI Semibold", 15, FontStyle.Bold)
            };
            stripDataTable.Columns[4].HeaderCell.Style = headerStyle;

            stripDataTable.Columns["planned_cleared_levels_column"].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 230, 80);
            stripDataTable.Columns["xfl_column"].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 230, 80);

            stripDataTable.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            stripDataTable.GridColor = Color.Black;

            stripDataTable.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 15, GraphicsUnit.Pixel);

            stripDataTable.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 180, 180, 180);
            stripDataTable.DefaultCellStyle.SelectionForeColor = Color.Black;

            stripDataTable.Columns["planned_cleared_levels_column"].DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 255, 230, 80);
            stripDataTable.Columns["xfl_column"].DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 255, 230, 80);
        }

        private void stripDataTable_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Check if it's the columns you want to compare
            if (e.ColumnIndex == 1 || e.ColumnIndex == 2)
            {
                DataGridViewRow row = stripDataTable.Rows[e.RowIndex];

                // Get values of the two columns
                object value1 = row.Cells["planned_cleared_levels_column"].Value;
                object value2 = row.Cells["xfl_column"].Value;

                // Compare values
                if (value1 != null && value2 != null && value1.Equals(value2))
                {
                    // Set the cell's fore color to red
                    e.CellStyle.BackColor = Color.FromArgb(255, 180, 180, 180);
                }
            }
        }

        // TIMING AND BASIC PROGRAM FUNCTIONALITY
        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            // Function to execute periodically
            string response = Netcode.SendCommand("#TR");

            // Process the response or perform any other actions
            // Note: Make sure to update UI controls from the main UI thread using Invoke if necessary
            Invoke(new Action(() =>
            {
                // Update UI controls or display the response

                //List<Flight> validFlights = ValidateFlights(response);
                PopulateDataGridView();
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
                string url = $"https://skyvector.com/?ll=46.12757152789378,14.788330080764661&chart=304&zoom=5&fpl={fullRoute}";

                /*RouteView wx = new RouteView(url);
                wx.Show();*/

                System.Diagnostics.Process.Start(url);
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

                string callsignString = callsign.Value.ToString();
                Flight tmp = validFlights[callsignString];

                string[] squawk = Netcode.SendCommand($"#TRSQK;{callsign.Value}").Split(';');
                if (squawk[2] == "Traffic not assumed.")
                {
                    pssr.Value = "";
                }
                else if (validFlights[callsignString].PSSR != "" || squawk[2] == "Traffic not assumed.")
                {
                    tmp.PSSR = squawk[2];
                    validFlights.Remove(callsignString);
                    validFlights.Add(callsignString, tmp);
                    Logging.Log($"Value: {pssr.Value.GetType()}");
                    pssr.Value = squawk[2];
                    Netcode.SendCommand($"#LBSQK;{callsignString};{squawk[2]}");
                }
                else
                {
                    pssr.Value = string.Empty;
                }
            }
            else if (e.ColumnIndex == 0)
            {
                DataGridViewTextBoxCell callsign = (DataGridViewTextBoxCell)stripDataTable.Rows[e.RowIndex].Cells[e.ColumnIndex];

                Logging.Log("Callsign: " + callsign.Value.ToString());
                FlightHandler form = new FlightHandler(callsign.Value.ToString());
                form.Show();
            }
        }

        public static Dictionary<string, Point> LoadAirac()
        {
            string[] lines = File.ReadAllLines(@"airac.txt");
            var dictionary = new Dictionary<string, Point>();

            foreach (var line in lines)
            {
                if (line.Substring(0, 2) == "//") { continue; }

                string[] parts = line.Split(';');

                if (parts.Length == 3)
                {
                    string name = parts[0];

                    if (double.TryParse(parts[1], out double latitude) && double.TryParse(parts[2], out double longitude))
                    {
                        //Log(latitude + " | " + longitude);
                        Point point = new Point(Math.Round(latitude, 5), Math.Round(longitude, 5));
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
            string[] filePath = Directory.GetFiles($"{cwd}/sectors", "*.msct");
            var lines = File.ReadAllLines(filePath[0]);
            var sector = new Sector { Points = new List<Point>() };

            foreach (var line in lines)
            {
                if (line.Substring(0, 2) == "//") { continue; }

                var coordinates = line.Split(';');
                if (coordinates.Length == 2 && double.TryParse(coordinates[0], out double _) && double.TryParse(coordinates[1], out double _))
                {
                    sector.Points.Add(new Point ( Math.Round(Convert.ToDouble(coordinates[0]), 5), Math.Round(Convert.ToDouble(coordinates[1]), 5) ));
                }
                else
                {
                    throw new FormatException("Invalid coordinate format in file.");
                }
            }
            Logging.Log($"Loaded {filePath[0]}");
            return sector;
        }

        public List<Sector> LoadSectors()
        {
            string[] filePath = Directory.GetFiles($"{cwd}/sectors", "*.sct");
            List<Sector> sectors = new List<Sector>();

            foreach (string sectorName in filePath)
            {
                var lines = File.ReadAllLines(sectorName);
                string fileName = Path.GetFileNameWithoutExtension(sectorName);
                Sector newSect = new Sector { Name = fileName, Points = new List<Point>() };

                foreach (var line in lines)
                {
                    if (line.Substring(0, 2) == "//") { continue; }
                    newSect.Name = fileName;
                    var coordinates = line.Split(';');
                    if (coordinates.Length == 2 && double.TryParse(coordinates[0], out double _) && double.TryParse(coordinates[1], out double _))
                    {
                        newSect.Points.Add(new Point(Math.Round(Convert.ToDouble(coordinates[0]), 5), Math.Round(Convert.ToDouble(coordinates[1]), 5)));
                    }
                    else
                    {
                        throw new FormatException("Invalid coordinate format in file.");
                    }
                }

                sectors.Add(newSect);
                Logging.Log($"Loaded sector: {fileName}");
            }

            return sectors;
        }

        private void LoadLoAs()
        {
            string[] lines = File.ReadAllLines($"{cwd}/loa.txt");

            foreach (string line in lines)
            {
                if (line.Substring(0, 2) == "//") { continue; }
                string[] parts = line.Split(',');
                string key = parts[1] + parts[2] + parts[3];
                int loaFL = int.Parse(parts[4]);

                if (parts[0] == "XFL")
                {
                    ExitLevels.Add(key, loaFL);
                    Logging.Log($"Loaded: {key}");
                }
                else if (parts[0] == "EFL")
                {
                    EntryLevels.Add(key, loaFL);
                    Logging.Log($"Loaded: {key}");
                }
                else { continue; }
            }
        }

        // CheckIntersections checks which lines intersects the sector
        public static void CheckIntersections(List<Line> lines, Sector sector)
        {
            foreach (Line line in lines)
            {
                if (IntersectsSector(line, sector))
                {
                    Console.WriteLine(line + " intersects sector.");
                }
            }
        }

        // The explanation of the algorithm for line intersection is available at these links:
        // https://www.youtube.com/watch?v=bbTqI0oqL5U
        // https://gist.github.com/SuryaPratapK/4b632447abbc0e95f6e81da321b855fb
        // https://www.geeksforgeeks.org/check-if-two-given-line-segments-intersect/

        // IntersectsSector checks if line intersects sector
        public static bool IntersectsSector(Line line, Sector sector)
        {
            // individually check if line intersects any sector lines
            for (int i = 0; i < sector.Points.Count; i++)
            {
                Point start = sector.Points[i];
                Point end = sector.Points[(i + 1) % sector.Points.Count];

                if (LinesIntersect(line, new Line( start, end)))
                {
                    // if line intersects any of the sector lines that means it
                    // also intersects the sector
                    return true;
                }
            }

            return false;
        }

        // LinesIntersect check if line l1 intersects line l2
        private static bool LinesIntersect(Line l1, Line l2)
        {
            Point p1 = l1.Start; Point q1 = l1.End; // line 1
            Point p2 = l2.Start; Point q2 = l2.End; // line 2

            // Find the four orientations
            int o1 = Orientation(p1, q1, p2);
            int o2 = Orientation(p1, q1, q2);
            int o3 = Orientation(p2, q2, p1);
            int o4 = Orientation(p2, q2, q1);

            if (o1 != o2 && o3 != o4)
            {
                // TODO: check if line only intersects with point from sector
                //Console.WriteLine("Line intersects sector.");
                return true;
            }

            // Special cases for colinear points
            if (o1 == 0 && OnSegment(p1, p2, q1))
            {
                Console.WriteLine("Parallel intersection.");
                return true;
            }

            if (o2 == 0 && OnSegment(p1, q2, q1))
            {
                Console.WriteLine("Parallel intersection.");
                return true;
            }

            if (o1 == 0 && OnSegment(p2, p1, q2))
            {
                Console.WriteLine("Parallel intersection.");
                return true;
            }

            if (o1 == 0 && OnSegment(p2, q1, q2))
            {
                Console.WriteLine("Parallel intersection.");
                return true;
            }

            return false;
        }

        // Checks if point q lies on line segment 'pr'
        private static bool OnSegment(Point p, Point q, Point r)
        {
            if (q.X <= Math.Max(p.X, r.X) && q.X >= Math.Min(p.X, r.X) &&
                q.Y <= Math.Max(p.Y, r.Y) && q.Y >= Math.Min(p.Y, r.Y))
                return true;

            return false;
        }

        // 0 --> p, q and r are colinear
        // 1 --> Clockwise
        // 2 --> Counterclockwise
        private static int Orientation(Point p, Point q, Point r)
        {
            double val = (q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y);

            // val == 0 should be good if the numbers were integers
            // Because we are using decimal number, we leave some room beacause
            // decimal numbers can be inaccurately stored in memory
            if (Math.Abs(val) < 0.0001) return 0; // colinear

            return (val > 0) ? 1 : 2;
        }

        // TABLE CHECKING AND DATA POPULATION
        private void ApplyFLColors()
        {
            foreach (DataGridViewRow row in stripDataTable.Rows)
            {
                if (row.Cells["planned_cleared_levels_column"] == row.Cells["xfl_column"]) 
                {
                    row.Cells["planned_cleared_levels_column"].Style.BackColor = Color.FromArgb(255, 180, 180, 180);
                    row.Cells["xfl_column"].Style.BackColor = Color.FromArgb(255, 180, 180, 180);
                }
            }
        }

        private void PopulateDataGridView()
        {
            stripDataTable.Rows.Clear();

            string response = Netcode.SendCommand("#TR");
            ValidateFlights(response);

            // Add the data rows to the DataGridView
            foreach(KeyValuePair<string, Flight> flight in validFlights)
            {
                if (exclusionList.Contains(flight.Value.Callsign)) { continue; }
                stripDataTable.Rows.Add(flight.Value.ShowFlight());
            }
            ApplyFLColors();
        }

        // NEW
        static List<Line> SplitLines(List<Line> inputLines)
        {
            List<Line> outputLines = new List<Line>();

            foreach (Line line in inputLines)
            {
                // Splitting the line into 5 parts
                List<Line> parts = SplitRoute(line, 15);

                // Converting each part into a string representation and adding to the output list
                foreach (var part in parts)
                {
                    outputLines.Add(new Line(new Point(part.Start.X, part.Start.Y), new Point(part.End.X, part.End.Y)));
                }
            }

            return outputLines;
        }

        // NEW
        static List<Line> SplitRoute(Line line, int parts)
        {
            List<Line> dividedLines = new List<Line>();

            double deltaX = (line.End.X - line.Start.X) / parts;
            double deltaY = (line.End.Y - line.Start.Y) / parts;

            for (int i = 0; i < parts; i++)
            {
                Point newStart = new Point(line.Start.X + i * deltaX, line.Start.Y + i * deltaY);
                Point newEnd = new Point(line.Start.X + (i + 1) * deltaX, line.Start.Y + (i + 1) * deltaY);
                dividedLines.Add(new Line(newStart, newEnd));
            }

            return dividedLines;
        }

        //FLIGHT FILTERING AND PROCESSING
        // Applies the sector flight is coming from
        private string DefineInSector(Flight flight)
        {
            // Determine in sector / previous
            List<Line> flightRoute = SplitLines(flight.SystemRoute);

            for (int i = 0; i < flightRoute.Count; i++)
            {
                foreach (Sector sect in sectors)
                {
                    if (IntersectsSector(flightRoute[i], sect) && sect.Name != "LJLA")
                    {
                        // Returns first sector the route intersects
                        flight.InboundSector = sect.Name;
                        if (flight.Flightplan.Adep.StartsWith("LJ")) { sect.Name = "LJLA"; }
                        return sect.Name;
                    }
                }
            }
            return mainSector.Name;
        }

        // Applies the sector the flight will be entering
        private List<string> DefineOutSector(Flight flight, string inSector)
        {
            List<string> crossingSectors = new List<string>();
            List<Line> flightRoute = SplitLines(flight.SystemRoute);

            foreach (Sector sect in sectors)
            {
                for (int i = 0; i < flightRoute.Count; i++)
                {
                    //Logging.Log(flightRoute[i].ToString());
                    if (IntersectsSector(flightRoute[i], sect) && !crossingSectors.Contains(sect.Name) && sect.Name != inSector)
                    {
                        crossingSectors.Add($"{sect.Name}");
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            if (crossingSectors.Count == 2 && (crossingSectors[0].StartsWith("LD") || crossingSectors[0].StartsWith("LI")) && crossingSectors[1] == "LJLA")
            {
                string tmp = crossingSectors[0];
                crossingSectors[0] = "LJLA";
                crossingSectors[1] = tmp;
            }

            if (crossingSectors.Count < 2) { crossingSectors.Add("LJLA"); }
            if (crossingSectors[0] == "LJLA" && crossingSectors.Count() >= 2) { crossingSectors.RemoveAt(0); }

            string cs = string.Empty;
            foreach (string s in crossingSectors) { cs += s + ", "; }

            Logging.Log($"Sector count {flight.Callsign}: {cs}");

            return crossingSectors;
        }

        private Tuple<string, string> ApplySectors(Flight flight)
        {
            string inSector = DefineInSector(flight);
            List<string> outSectorList = DefineOutSector(flight, inSector);

            return Tuple.Create(inSector, outSectorList[0]);
        }

        private int GetQNH(string station)
        {
            string metar = Netcode.SendCommand($"#METAR;{station}").Split(';')[1];
            string[] metarSplit = metar.Split(' ');

            foreach (string element in metarSplit)
            {
                Match match = Regex.Match(element, @"Q(\d{3,4})");
                if (match.Success)
                {
                    if (int.TryParse(match.Groups[1].Value, out int qnhValue))
                    {
                        return qnhValue;
                    }
                }
            }

            return 1013;
        }

        // Applies XFLs and EFLs to validated flights
        private void ApplyLoa(Flight flight)
        {
            flight.SystemRoute = ProcessRoute(flight);

            foreach (Line line in flight.SystemRoute)
            {
                if (IntersectsSector(line, mainSector))
                {
                    Tuple<string, string> AppliedSectors = ApplySectors(flight);

                    Logging.Log(flight.Callsign);
                    flight.InboundSector = AppliedSectors.Item1;
                    flight.OutboundSector = AppliedSectors.Item2;
                    //Logging.Log("From (O): " + flight.InboundSector + " | To (I): " + flight.OutboundSector);

                    //flight.Loa_efls = ApplyEFLLoA(flight);
                    flight.Loa_xfls = ApplyXFLLoA(flight);

                    double tempCFL = (flight.Altitude + (1013 - GetQNH("LJLJ")) * 28) / 100;
                    flight.ComputedFL = ((int)Math.Round(tempCFL / 10)) * 10;

                    validFlights.Add(flight.Callsign, flight);
                    break;
                }
                else { continue; }
            }
        }
        
        private void ValidateFlights(string radarResponse)
        {
            validFlights.Clear();

            string[] trafficInRange = radarResponse.Split(';');
            trafficInRange = trafficInRange.Skip(1).ToArray();

            Logging.Log("=====ValidateFlights()=====");
            foreach (string traffic in trafficInRange)
            {
                string fplTmp = Netcode.SendCommand($"#FP;{traffic}");
                string[] fpl = fplTmp.Split(';');

                if (fpl[0] == "@ERR") continue;

                if (fpl[8] == "I" && fpl[11] != "VFR")
                {
                    string tmpPos = Netcode.SendCommand($"#TRPOS;{traffic}");
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

                    ApplyLoa(flight);
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
                else
                {
                    //Log($"Name '{wpt}' not found in the dictionary.");
                    continue;
                }
            }

            for (int i = 0; i < tmp_route.Count - 1; i++)
            {
                Point point1 = tmp_route[i];
                Point point2 = tmp_route[(i + 1) % tmp_route.Count];

                route_segments.Add(new Line(point1, point2));
            }

            return route_segments;
        }

        private List<int> ApplyEFLLoA(Flight flight)
        {
            List<int> loa_efls = new List<int>();

            string ADEP = flight.Flightplan.Adep;
            string ADES = flight.Flightplan.Ades;

            string DKey_ADEP = 'D' + ADEP + flight.OutboundSector;
            string AKey_ADES = 'A' + ADES + flight.OutboundSector;

            if (EntryLevels.ContainsKey(DKey_ADEP))
            {
                loa_efls.Add(EntryLevels[DKey_ADEP]);
            }
            else if (EntryLevels.ContainsKey(AKey_ADES))
            {
                loa_efls.Add(EntryLevels[AKey_ADES]);
            }

            return loa_efls;
        }

        private List<int> ApplyXFLLoA(Flight flight)
        {
            List<int> loa_xfls = new List<int>();

            string ADEP = flight.Flightplan.Adep;
            string ADES = flight.Flightplan.Ades;

            string DKey_ADEP = 'D' + ADEP + flight.OutboundSector;
            string AKey_ADES = 'A' + ADES + flight.OutboundSector;

            if (ExitLevels.ContainsKey(DKey_ADEP)) 
            {
                //Log(DKey_ADEP); //
                loa_xfls.Add(ExitLevels[DKey_ADEP]);
            }

            if(ExitLevels.ContainsKey(AKey_ADES))
            {
                Logging.Log(AKey_ADES); //GOOD
                loa_xfls.Add(ExitLevels[AKey_ADES]);
            }

            return loa_xfls;
        }

        //
        private void EStrips_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Stop the timer
            timer.Stop();
            timer.Dispose();
        }

        private void eStrips_Load(object sender, EventArgs e)
        {
            TopMost = true;
            string response = Netcode.SendCommand("#TR");

            ValidateFlights(response);
            PopulateDataGridView();
            stripDataTable.Sort(stripDataTable.Columns["planned_cleared_levels_column"], ListSortDirection.Descending);
        }

        private void BtnMeteo_MouseClick(object sender, MouseEventArgs e)
        {
            FormCollection fc = Application.OpenForms;
            // Check if form is open
            foreach (Form frm in fc)
            {
                if (frm.Name == "WxWindow")
                {
                    return;
                }
            }

            WxWindow wx = new WxWindow();
            wx.Show();
        }

        private void BtnLoa_MouseClick(object sender, MouseEventArgs e)
        {
            System.Diagnostics.Process.Start($"https://si.ivao.aero/wp-content/uploads/2023/08/IVAO-FLAS.pdf");
        }

        private void BtnCharts_MouseClick(object sender, MouseEventArgs e)
        {
            System.Diagnostics.Process.Start($"https://www.sloveniacontrol.si/acrobat/aip/Operations/history-en-GB.html");
        }

        private void BtnFlights_Click(object sender, EventArgs e)
        {
            FdrModal form = new FdrModal();
            form.Show();
        }

    }
}