namespace eStrips
{
    partial class XFLModal
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.LblCallsign = new System.Windows.Forms.Label();
            this.LblRFL = new System.Windows.Forms.Label();
            this.LblRFLText = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.CbXFLSelect = new System.Windows.Forms.ComboBox();
            this.BtnConfirmXFL = new System.Windows.Forms.Button();
            this.BtnClose = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.panel1.Controls.Add(this.LblCallsign);
            this.panel1.Controls.Add(this.LblRFL);
            this.panel1.Controls.Add(this.LblRFLText);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(284, 35);
            this.panel1.TabIndex = 0;
            // 
            // LblCallsign
            // 
            this.LblCallsign.AutoSize = true;
            this.LblCallsign.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.LblCallsign.Font = new System.Drawing.Font("Lucida Sans Unicode", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblCallsign.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(226)))), ((int)(((byte)(82)))));
            this.LblCallsign.Location = new System.Drawing.Point(110, 0);
            this.LblCallsign.Name = "LblCallsign";
            this.LblCallsign.Size = new System.Drawing.Size(144, 34);
            this.LblCallsign.TabIndex = 3;
            this.LblCallsign.Text = "AHO597V";
            // 
            // LblRFL
            // 
            this.LblRFL.AutoSize = true;
            this.LblRFL.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.LblRFL.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblRFL.Location = new System.Drawing.Point(49, 9);
            this.LblRFL.Name = "LblRFL";
            this.LblRFL.Size = new System.Drawing.Size(35, 17);
            this.LblRFL.TabIndex = 1;
            this.LblRFL.Text = "000";
            // 
            // LblRFLText
            // 
            this.LblRFLText.AutoSize = true;
            this.LblRFLText.Font = new System.Drawing.Font("Lucida Sans", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblRFLText.Location = new System.Drawing.Point(2, 9);
            this.LblRFLText.Name = "LblRFLText";
            this.LblRFLText.Size = new System.Drawing.Size(36, 16);
            this.LblRFLText.TabIndex = 0;
            this.LblRFLText.Text = "RFL:";
            this.LblRFLText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Location = new System.Drawing.Point(46, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(45, 29);
            this.panel2.TabIndex = 2;
            // 
            // CbXFLSelect
            // 
            this.CbXFLSelect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
            this.CbXFLSelect.DropDownHeight = 100;
            this.CbXFLSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CbXFLSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CbXFLSelect.FormattingEnabled = true;
            this.CbXFLSelect.IntegralHeight = false;
            this.CbXFLSelect.Items.AddRange(new object[] {
            "470",
            "460",
            "450",
            "440",
            "430",
            "420",
            "410",
            "400",
            "390",
            "380",
            "370",
            "360",
            "350",
            "340",
            "330",
            "320",
            "310",
            "300",
            "290",
            "280",
            "270",
            "260",
            "250",
            "240",
            "230",
            "220",
            "200",
            "190",
            "180",
            "170",
            "160",
            "150",
            "140",
            "130",
            "120",
            "110",
            "A100",
            "A95",
            "A90",
            "A85",
            "A80",
            "A75",
            "A70",
            "A65",
            "A60",
            "A55",
            "A50",
            "A45",
            "A40",
            "A35",
            "A30",
            "A25",
            "A20",
            "A15",
            "A10"});
            this.CbXFLSelect.Location = new System.Drawing.Point(5, 41);
            this.CbXFLSelect.MaxDropDownItems = 2;
            this.CbXFLSelect.MaximumSize = new System.Drawing.Size(275, 0);
            this.CbXFLSelect.MaxLength = 4;
            this.CbXFLSelect.MinimumSize = new System.Drawing.Size(50, 0);
            this.CbXFLSelect.Name = "CbXFLSelect";
            this.CbXFLSelect.Size = new System.Drawing.Size(50, 21);
            this.CbXFLSelect.TabIndex = 1;
            // 
            // BtnConfirmXFL
            // 
            this.BtnConfirmXFL.Location = new System.Drawing.Point(5, 98);
            this.BtnConfirmXFL.Name = "BtnConfirmXFL";
            this.BtnConfirmXFL.Size = new System.Drawing.Size(75, 23);
            this.BtnConfirmXFL.TabIndex = 2;
            this.BtnConfirmXFL.Text = "Confirm";
            this.BtnConfirmXFL.UseVisualStyleBackColor = true;
            this.BtnConfirmXFL.Click += new System.EventHandler(this.BtnConfirmXFL_Click);
            // 
            // BtnClose
            // 
            this.BtnClose.Location = new System.Drawing.Point(95, 98);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(75, 23);
            this.BtnClose.TabIndex = 3;
            this.BtnClose.Text = "Close";
            this.BtnClose.UseVisualStyleBackColor = true;
            this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // XFLModal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
            this.ClientSize = new System.Drawing.Size(284, 161);
            this.ControlBox = false;
            this.Controls.Add(this.BtnClose);
            this.Controls.Add(this.BtnConfirmXFL);
            this.Controls.Add(this.CbXFLSelect);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "XFLModal";
            this.Text = "Edit XFL";
            this.Deactivate += new System.EventHandler(this.XFLModal_Deactivate);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label LblRFLText;
        private System.Windows.Forms.Label LblRFL;
        private System.Windows.Forms.Label LblCallsign;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox CbXFLSelect;
        private System.Windows.Forms.Button BtnConfirmXFL;
        private System.Windows.Forms.Button BtnClose;
    }
}