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

namespace eStrips
{
    public partial class eStrips : Form
    {
        private System.Timers.Timer timer;
        private const string serverAddress = "127.0.0.1";   // Replace with your server IP address
        private const int serverPort = 1130;                // Replace with your server port number
        private const int sendInterval = 15000;              // Interval between sending messages (in milliseconds)

        //LoAs
        private Dictionary<string, Dictionary<string, int>> arrivalLoa = new Dictionary<string, Dictionary<string, int>>();
        private Dictionary<string, Dictionary<string, int>> departureLoa = new Dictionary<string, Dictionary<string, int>>();
        private Dictionary<string, Dictionary<string, int>> overflightLoa = new Dictionary<string, Dictionary<string, int>>();

        public eStrips()
        {
            InitializeComponent();

            Coordinate[] Airac = LoadAirac();

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
        }

        //  LOGGING
        private void Log(string str)
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
                List<Flight> validFlights = ValidateFlights(response);
                PopulateDataGridView(validFlights);
                stripDataTable.Sort(stripDataTable.Columns["planned_cleared_levels_column"], ListSortDirection.Descending);
            }));
        }

        //  TODO - Generate squawk upon clicking PSSR field
        private void stripDataTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 10)
            {

            }
        }

        private Coordinate[] LoadAirac()
        {
            string[] lines = File.ReadAllLines(@"..\..\airac.txt");
            Coordinate[] waypoints = new Coordinate[lines.Length];

            for (int i = 0; i < lines.Length; i++)
            {
                string[] parts = lines[i].Split(';');

                if (parts.Length >= 3)
                {
                    waypoints[i].name = parts[0].Trim();
                    waypoints[i].latitude = double.Parse(parts[1].Trim());
                    waypoints[i].longitude = double.Parse(parts[2].Trim());

                    //Waypoint logging
                    //Log(waypoints[i].ToString());
                }
                else
                {
                    // Handle invalid lines with insufficient data if needed
                    waypoints[i].name = "Invalid Waypoint";
                    waypoints[i].latitude = 0.0;
                    waypoints[i].longitude = 0.0;
                }
            }
            return waypoints;

        }

        private void LoadLoAs()
        {
            string[] lines = File.ReadAllLines(@"..\..\loa.txt");

            for (int i = 0; i < lines.Length; i++)
            {
                string[] parts = lines[i].Split(';');

                if (parts[0] == "A")
                {
                    //arrivalLoa.Add(parts[1], parts[2], int.Parse(parts[3]));
                }
                else
                {

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
        public int CallsignIndex(string callsign)
        {
            int rowIndex = -1;  // Initialize with -1 to indicate value not found

            foreach (DataGridViewRow row in stripDataTable.Rows)
            {
                // Replace "ColumnName" with the actual name or index of the column you want to search in
                if (row.Cells["callsign_column"].Value != null && row.Cells["callsign_column"].Value.ToString() == callsign)
                {
                    rowIndex = row.Index;  // Store the row index where the value is found
                    break;
                }
            }
            return rowIndex;
        }

        private bool CallsignAlreadyExists(string callsign)
        {
            foreach (DataGridViewRow row in stripDataTable.Rows)
            {
                // Replace "ColumnName" with the actual name or index of the column you want to check
                if (row.Cells["callsign_column"].Value != null && row.Cells["callsign_column"].Value.ToString() == callsign)
                {
                    return true;
                }
            }

            return false;
        }

        private void PopulateDataGridView(List<Flight> validFlights)//string[][] flights)   
        {

            // Add the data rows to the DataGridView
            foreach (Flight flight in validFlights)
            {   
                // Updates the callsigns information!!
                if (CallsignAlreadyExists(flight.Callsign))
                {
                    stripDataTable.Rows.RemoveAt(CallsignIndex(flight.Callsign));
                    stripDataTable.Rows.Add(flight.ShowFlight());
                    continue;
                }

                stripDataTable.Rows.Add(flight.ShowFlight());
                //stripDataTable.Columns["planned_cleared_levels_column"].SortMode = DataGridViewColumnSortMode.Programmatic;
                //stripDataTable.Columns["planned_cleared_levels_column"].HeaderCell.SortGlyphDirection = SortOrder.Descending;
            }
        }

        //FLIGHT FILTERING AND PROCESSING
        private List<Flight> ValidateFlights(string radarResponse)
        {
            List<Flight> validFlights = new List<Flight>();

            string[] trafficInRange = radarResponse.Split(';');
            trafficInRange = trafficInRange.Skip(1).ToArray();

            Log("=====ValidateFlights()=====");
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
                        Position = new Coordinate {
                            name = $"{pos[6]};{pos[7]}",
                            latitude = double.Parse(pos[6]),
                            longitude = double.Parse(pos[7]),
                            altitude = int.Parse(pos[4])
                        },
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
                        }
                    };

                    validFlights.Add(flight);
                }
                else { continue; }
            }
            return validFlights;
        }

        private Coordinate[] ProcessRoute(Flight flight)
        {

            Coordinate[] route = null;

            return route;
        }

        //TODO
        private void CalculateTrajectory(List<Flight> validFlights)
        {
            foreach (Flight flight in validFlights)
            {
                // TODO
            }
        }

        //TODO ApplyLoa
        private void ApplyLoA(List<Flight> validFlights)
        {

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

            List<Flight> validFlights = ValidateFlights(response);
            PopulateDataGridView(validFlights);
            stripDataTable.Sort(stripDataTable.Columns["planned_cleared_levels_column"], ListSortDirection.Descending);
        }

    }
}