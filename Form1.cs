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

namespace eStrips
{
    public partial class eStrips : Form
    {
        private static string cwd = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static string[] portFile = File.ReadAllLines($"{cwd}/.key");

        private System.Timers.Timer timer;
        private const string serverAddress = "127.0.0.1";           // Replace with your server IP address
        private static int serverPort = int.Parse(portFile[0]);     // Replace with your server port number
        private const int sendInterval = 15000;                      // Interval between sending messages (in milliseconds)

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
            Log("Loaded AIRAC");
            LoadLoAs();
            Log("Loaded LoAs");
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

            stripDataTable.Columns["planned_cleared_levels_column"].DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 255, 230, 80);
            stripDataTable.Columns["xfl_column"].DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 255, 230, 80);
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
            else if (e.ColumnIndex == 0)
            {
                DataGridViewTextBoxCell callsign = (DataGridViewTextBoxCell)stripDataTable.Rows[e.RowIndex].Cells[e.ColumnIndex];
                
                Log("Callsing: " + callsign.Value.ToString());
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
            Log($"Loaded {filePath[0]}");
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
                Log($"Loaded sector: {fileName}");
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
                    Log($"Loaded: {key}");
                }
                else if (parts[0] == "EFL")
                {
                    EntryLevels.Add(key, loaFL);
                    Log($"Loaded: {key}");
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
        private void PopulateDataGridView()
        {
            stripDataTable.Rows.Clear();

            string response = SendCommand("#TR");
            ValidateFlights(response);

            // Add the data rows to the DataGridView
            foreach(KeyValuePair<string, Flight> flight in validFlights)
            {
                if (exclusionList.Contains(flight.Value.Callsign)) { continue; }
                stripDataTable.Rows.Add(flight.Value.ShowFlight());
            }
        }

        //FLIGHT FILTERING AND PROCESSING
        // Applies the sector flight is coming from
        private Tuple<string, int> DefineInSector(Flight flight)
        {
            // Determine outbound sector / previous sector
            string outboundSector = string.Empty;
            int outboundSegmentIndex = 0;

            for (int i = 0; i < flight.Route.Count; i++)
            {
                foreach (Sector sect in sectors)
                {
                    if (IntersectsSector(flight.Route[i], sect))
                    {
                        outboundSector = sect.Name;
                        outboundSegmentIndex = i;
                        return new Tuple<string, int>(outboundSector, i);
                    }
                }
            }
            Log($"OUT index: {outboundSegmentIndex}");
            return new Tuple<string, int>("LJLA", 0);
        }

        // Applies the sector the flight will be entering
        private Tuple<string, int> DefineOutSector(Flight flight)    
        {
            List<Tuple<string, int>> sectorTuples = new List<Tuple<string, int>>();
            List<string> sectorNames = new List<string>();
            int outboundIndex = 0;

            //Add all sectors that have an intersecting segment.
            for (int i = 0; i < flight.Route.Count; i++)
            {
                foreach (Sector sect in sectors)
                {
                    if (IntersectsSector(flight.Route[i], sect) && !sectorNames.Contains(sect.Name))
                    {
                        sectorTuples.Add(new Tuple<string, int>(sect.Name, i));
                    }
                    else { continue; }
                }
            }

            /*
               Remove duplicates based on the sector name.
               - Group the tuples by the sector name.
               - Select the first tuple from each group (keeps the first occurrence).
               - Convert the result to a list.
            */      
            List<Tuple<string, int>> uniqueSectorTuples = sectorTuples.GroupBy(tuple => tuple.Item1)
                .Select(group => group.Last())
                .ToList();

            //  Testing only
            foreach (var tuple in uniqueSectorTuples)
            {
                Log("Sector: " + tuple.Item1 + " | Index: " + tuple.Item2);

            }

            Log($"Sector count: {uniqueSectorTuples.Count()}");

            //  Returns the first sector if flight doesn't leave FIR
            if (uniqueSectorTuples.Count == 1 && uniqueSectorTuples[0].Item1 == "LJLA") { return sectorTuples[0]; }

            //  Returns if traffic is departing or arriving into FIR
            if (uniqueSectorTuples.Count == 2 && uniqueSectorTuples[0].Item1 != uniqueSectorTuples[1].Item1) { return uniqueSectorTuples[1]; } /*&& 
                (sectorTuples[0].Item1 == "LJLA" || sectorTuples[1].Item1 == "LJLA")*/

            if (uniqueSectorTuples.Count > 2)
            {
                //Log("IN HERE");
                int i = 0;

                while (uniqueSectorTuples[i].Item1 == uniqueSectorTuples[i+1].Item1)
                {
                    i++;
                    if (uniqueSectorTuples[i].Item1 != uniqueSectorTuples[i+1].Item1) { break; }
                    continue;
                }
                outboundIndex = i;
                //Log("HERE");
                return new Tuple<string, int>(uniqueSectorTuples[i].Item1, outboundIndex);
            }
            return new Tuple<string, int>("LJLA", 0);
        }

        private Tuple<string, string> ApplySectors(Flight flight)
        {   
            Tuple<string, int> outTuple = DefineInSector(flight);
            Tuple<string, int> inTuple = DefineOutSector(flight);

            if (outTuple.Item2 > inTuple.Item2)
            {
                Log("SWAPPED");
                return new Tuple<string, string>(inTuple.Item1, outTuple.Item1);
            }
            else
            {
                Log("NOT swapped");
                return new Tuple<string, string>(outTuple.Item1, inTuple.Item1);
            }
        }

        private int GetQNH(string station)
        {
            string metar = SendCommand($"#METAR;{station}").Split(';')[1];
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
            flight.Route = ProcessRoute(flight);

            foreach (Line line in flight.Route)
            {
                if (IntersectsSector(line, mainSector))
                {
                    Log(flight.Callsign + " has intersection");
                    Tuple<string, string> AppliedSectors = ApplySectors(flight);

                    flight.OutboundSector = AppliedSectors.Item1;
                    flight.InboundSector = AppliedSectors.Item2;
                    Log("From (O): " + flight.OutboundSector + " | To (I): " + flight.InboundSector + "\n");

                    flight.Loa_efls = ApplyEFLLoA(flight);
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

                    ApplyLoa(flight);
                }
                else { continue; }

            }
            Log("===========================================");
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

            string DKey_ADEP = 'D' + ADEP + flight.InboundSector;
            string AKey_ADES = 'A' + ADES + flight.InboundSector;

            if (ExitLevels.ContainsKey(DKey_ADEP)) 
            {
                Log(DKey_ADEP);
                loa_xfls.Add(ExitLevels[DKey_ADEP]);
            }

            if(ExitLevels.ContainsKey(AKey_ADES))
            {
                Log(AKey_ADES);
                loa_xfls.Add(ExitLevels[AKey_ADES]);
            }

            return loa_xfls;
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