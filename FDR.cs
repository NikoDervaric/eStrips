using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eStrips
{
    public partial class FDR : Form
    {
        public FDR()
        {
            InitializeComponent();
        }

        private void FDR_Load(object sender, EventArgs e)
        {
            LblCallsign.Text = FdrModal.callsign;
        }
    }
}
