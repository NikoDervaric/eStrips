namespace eStrips
{
    partial class eStrips
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(eStrips));
            this.stripDataTable = new System.Windows.Forms.DataGridView();
            this.callsign_column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.planned_cleared_levels_column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.xfl_column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.route_column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dct_column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rfl_column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.actype_column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.speed_column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.adep_column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ades_column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pssr_column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.assr_column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.frf_column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BtnMeteo = new System.Windows.Forms.Button();
            this.BtnLoa = new System.Windows.Forms.Button();
            this.BtnCharts = new System.Windows.Forms.Button();
            this.BtnFlights = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.stripDataTable)).BeginInit();
            this.SuspendLayout();
            // 
            // stripDataTable
            // 
            this.stripDataTable.AllowUserToAddRows = false;
            this.stripDataTable.AllowUserToDeleteRows = false;
            this.stripDataTable.BackgroundColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.stripDataTable.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.stripDataTable.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this.stripDataTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.stripDataTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.callsign_column,
            this.planned_cleared_levels_column,
            this.xfl_column,
            this.route_column,
            this.dct_column,
            this.rfl_column,
            this.actype_column,
            this.speed_column,
            this.adep_column,
            this.ades_column,
            this.pssr_column,
            this.assr_column,
            this.frf_column});
            this.stripDataTable.Location = new System.Drawing.Point(0, 60);
            this.stripDataTable.Margin = new System.Windows.Forms.Padding(0);
            this.stripDataTable.MultiSelect = false;
            this.stripDataTable.Name = "stripDataTable";
            this.stripDataTable.ReadOnly = true;
            this.stripDataTable.RowTemplate.Height = 35;
            this.stripDataTable.RowTemplate.ReadOnly = true;
            this.stripDataTable.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.stripDataTable.Size = new System.Drawing.Size(1039, 594);
            this.stripDataTable.TabIndex = 0;
            this.stripDataTable.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.stripDataTable_CellClick);
            this.stripDataTable.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.stripDataTable_CellDoubleClick);
            // 
            // callsign_column
            // 
            this.callsign_column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.callsign_column.HeaderText = "CALLSIGN";
            this.callsign_column.MaxInputLength = 10;
            this.callsign_column.MinimumWidth = 120;
            this.callsign_column.Name = "callsign_column";
            this.callsign_column.ReadOnly = true;
            this.callsign_column.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.callsign_column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.callsign_column.Width = 120;
            // 
            // planned_cleared_levels_column
            // 
            this.planned_cleared_levels_column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.planned_cleared_levels_column.HeaderText = "P/CFL";
            this.planned_cleared_levels_column.MaxInputLength = 3;
            this.planned_cleared_levels_column.MinimumWidth = 55;
            this.planned_cleared_levels_column.Name = "planned_cleared_levels_column";
            this.planned_cleared_levels_column.ReadOnly = true;
            this.planned_cleared_levels_column.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.planned_cleared_levels_column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.planned_cleared_levels_column.ToolTipText = "Planned/Cleared Flightlevel";
            this.planned_cleared_levels_column.Width = 55;
            // 
            // xfl_column
            // 
            this.xfl_column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.xfl_column.HeaderText = "XFL";
            this.xfl_column.MaxInputLength = 4;
            this.xfl_column.MinimumWidth = 55;
            this.xfl_column.Name = "xfl_column";
            this.xfl_column.ReadOnly = true;
            this.xfl_column.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.xfl_column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.xfl_column.Width = 55;
            // 
            // route_column
            // 
            this.route_column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.route_column.HeaderText = "ROUTE";
            this.route_column.MaxInputLength = 20;
            this.route_column.MinimumWidth = 120;
            this.route_column.Name = "route_column";
            this.route_column.ReadOnly = true;
            this.route_column.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.route_column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.route_column.Width = 120;
            // 
            // dct_column
            // 
            this.dct_column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dct_column.HeaderText = "→";
            this.dct_column.MaxInputLength = 3;
            this.dct_column.MinimumWidth = 45;
            this.dct_column.Name = "dct_column";
            this.dct_column.ReadOnly = true;
            this.dct_column.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dct_column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dct_column.Width = 45;
            // 
            // rfl_column
            // 
            this.rfl_column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.rfl_column.HeaderText = "RFL";
            this.rfl_column.MaxInputLength = 3;
            this.rfl_column.MinimumWidth = 55;
            this.rfl_column.Name = "rfl_column";
            this.rfl_column.ReadOnly = true;
            this.rfl_column.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.rfl_column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.rfl_column.ToolTipText = "Req. cruise level";
            this.rfl_column.Width = 55;
            // 
            // actype_column
            // 
            this.actype_column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.actype_column.HeaderText = "TYPE";
            this.actype_column.MaxInputLength = 4;
            this.actype_column.Name = "actype_column";
            this.actype_column.ReadOnly = true;
            this.actype_column.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.actype_column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.actype_column.Width = 65;
            // 
            // speed_column
            // 
            this.speed_column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.speed_column.HeaderText = "SPD";
            this.speed_column.MaxInputLength = 4;
            this.speed_column.MinimumWidth = 60;
            this.speed_column.Name = "speed_column";
            this.speed_column.ReadOnly = true;
            this.speed_column.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.speed_column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.speed_column.Width = 60;
            // 
            // adep_column
            // 
            this.adep_column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.adep_column.HeaderText = "ADEP";
            this.adep_column.MaxInputLength = 4;
            this.adep_column.MinimumWidth = 60;
            this.adep_column.Name = "adep_column";
            this.adep_column.ReadOnly = true;
            this.adep_column.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.adep_column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.adep_column.Width = 60;
            // 
            // ades_column
            // 
            this.ades_column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ades_column.HeaderText = "ADES";
            this.ades_column.MaxInputLength = 4;
            this.ades_column.MinimumWidth = 60;
            this.ades_column.Name = "ades_column";
            this.ades_column.ReadOnly = true;
            this.ades_column.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ades_column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ades_column.Width = 60;
            // 
            // pssr_column
            // 
            this.pssr_column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.pssr_column.HeaderText = "PSSR";
            this.pssr_column.MaxInputLength = 4;
            this.pssr_column.MinimumWidth = 60;
            this.pssr_column.Name = "pssr_column";
            this.pssr_column.ReadOnly = true;
            this.pssr_column.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.pssr_column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.pssr_column.ToolTipText = "Previous SSR code";
            this.pssr_column.Width = 60;
            // 
            // assr_column
            // 
            this.assr_column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.assr_column.HeaderText = "ASSR";
            this.assr_column.MaxInputLength = 4;
            this.assr_column.MinimumWidth = 60;
            this.assr_column.Name = "assr_column";
            this.assr_column.ReadOnly = true;
            this.assr_column.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.assr_column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.assr_column.ToolTipText = "Assigned SSR code";
            this.assr_column.Width = 60;
            // 
            // frf_column
            // 
            this.frf_column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.frf_column.HeaderText = "FRF";
            this.frf_column.MinimumWidth = 200;
            this.frf_column.Name = "frf_column";
            this.frf_column.ReadOnly = true;
            this.frf_column.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.frf_column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.frf_column.ToolTipText = "Further route of flight";
            this.frf_column.Width = 200;
            // 
            // BtnMeteo
            // 
            this.BtnMeteo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(193)))), ((int)(((byte)(202)))));
            this.BtnMeteo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnMeteo.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtnMeteo.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnMeteo.ForeColor = System.Drawing.SystemColors.ControlText;
            this.BtnMeteo.Location = new System.Drawing.Point(14, 12);
            this.BtnMeteo.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.BtnMeteo.Name = "BtnMeteo";
            this.BtnMeteo.Size = new System.Drawing.Size(120, 35);
            this.BtnMeteo.TabIndex = 1;
            this.BtnMeteo.Text = "METEO";
            this.BtnMeteo.UseVisualStyleBackColor = false;
            this.BtnMeteo.MouseClick += new System.Windows.Forms.MouseEventHandler(this.BtnMeteo_MouseClick);
            // 
            // BtnLoa
            // 
            this.BtnLoa.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(193)))), ((int)(((byte)(202)))));
            this.BtnLoa.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnLoa.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtnLoa.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnLoa.ForeColor = System.Drawing.SystemColors.ControlText;
            this.BtnLoa.Location = new System.Drawing.Point(147, 12);
            this.BtnLoa.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.BtnLoa.Name = "BtnLoa";
            this.BtnLoa.Size = new System.Drawing.Size(120, 35);
            this.BtnLoa.TabIndex = 2;
            this.BtnLoa.Text = "LoA";
            this.BtnLoa.UseVisualStyleBackColor = false;
            this.BtnLoa.MouseClick += new System.Windows.Forms.MouseEventHandler(this.BtnLoa_MouseClick);
            // 
            // BtnCharts
            // 
            this.BtnCharts.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(193)))), ((int)(((byte)(202)))));
            this.BtnCharts.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnCharts.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtnCharts.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnCharts.ForeColor = System.Drawing.SystemColors.ControlText;
            this.BtnCharts.Location = new System.Drawing.Point(280, 12);
            this.BtnCharts.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.BtnCharts.Name = "BtnCharts";
            this.BtnCharts.Size = new System.Drawing.Size(120, 35);
            this.BtnCharts.TabIndex = 3;
            this.BtnCharts.Text = "CHARTS";
            this.BtnCharts.UseVisualStyleBackColor = false;
            this.BtnCharts.MouseClick += new System.Windows.Forms.MouseEventHandler(this.BtnCharts_MouseClick);
            // 
            // BtnFlights
            // 
            this.BtnFlights.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(193)))), ((int)(((byte)(202)))));
            this.BtnFlights.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnFlights.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtnFlights.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnFlights.ForeColor = System.Drawing.SystemColors.ControlText;
            this.BtnFlights.Location = new System.Drawing.Point(413, 12);
            this.BtnFlights.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.BtnFlights.Name = "BtnFlights";
            this.BtnFlights.Size = new System.Drawing.Size(120, 35);
            this.BtnFlights.TabIndex = 4;
            this.BtnFlights.Text = "FDR";
            this.BtnFlights.UseVisualStyleBackColor = false;
            this.BtnFlights.Click += new System.EventHandler(this.BtnFlights_Click);
            // 
            // eStrips
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(1039, 653);
            this.Controls.Add(this.BtnFlights);
            this.Controls.Add(this.BtnCharts);
            this.Controls.Add(this.BtnLoa);
            this.Controls.Add(this.BtnMeteo);
            this.Controls.Add(this.stripDataTable);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1055, 650);
            this.Name = "eStrips";
            this.Text = "eStrips - IVAO Slovenia";
            this.Load += new System.EventHandler(this.eStrips_Load);
            ((System.ComponentModel.ISupportInitialize)(this.stripDataTable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView stripDataTable;
        private System.Windows.Forms.DataGridViewTextBoxColumn callsign_column;
        private System.Windows.Forms.DataGridViewTextBoxColumn planned_cleared_levels_column;
        private System.Windows.Forms.DataGridViewTextBoxColumn xfl_column;
        private System.Windows.Forms.DataGridViewTextBoxColumn route_column;
        private System.Windows.Forms.DataGridViewTextBoxColumn dct_column;
        private System.Windows.Forms.DataGridViewTextBoxColumn rfl_column;
        private System.Windows.Forms.DataGridViewTextBoxColumn actype_column;
        private System.Windows.Forms.DataGridViewTextBoxColumn speed_column;
        private System.Windows.Forms.DataGridViewTextBoxColumn adep_column;
        private System.Windows.Forms.DataGridViewTextBoxColumn ades_column;
        private System.Windows.Forms.DataGridViewTextBoxColumn pssr_column;
        private System.Windows.Forms.DataGridViewTextBoxColumn assr_column;
        private System.Windows.Forms.DataGridViewTextBoxColumn frf_column;
        private System.Windows.Forms.Button BtnMeteo;
        private System.Windows.Forms.Button BtnLoa;
        private System.Windows.Forms.Button BtnCharts;
        private System.Windows.Forms.Button BtnFlights;
    }
}

