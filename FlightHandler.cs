﻿using System;
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
    public partial class FlightHandler : Form
    {
        public static FlightHandler instance;
        public static string callsign = eStrips.exclusionCallsign;

        public FlightHandler()
        {
            InitializeComponent();
            instance = this;
            instance.TopMost = true;
            instance.StartPosition = FormStartPosition.Manual;
            instance.Left = Cursor.Position.X;
            instance.Top = Cursor.Position.Y;
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            eStrips.exclusionList.Add(callsign);
            instance.Close();
        }

        private void BtnFdr_Click(object sender, EventArgs e)
        {
            FDR form = new FDR();
            form.Show();
            Close();
        }

        private void FlightHandler_Deactivate(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
