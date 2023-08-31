using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eStrips
{
    public partial class RouteView : Form
    {
        public static RouteView instance;
        public static string url;
        public RouteView(string route)
        {
            InitializeComponent();
            url = route;
            instance = this;
            instance.StartPosition = FormStartPosition.Manual;
            instance.Left = Cursor.Position.X;
            instance.Top = Cursor.Position.Y;
        }

        private void RouteView_Load(object sender, EventArgs e)
        {
            RouteBrowser.Navigate(url);
        }
    }
}
