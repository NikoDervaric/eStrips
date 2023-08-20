namespace eStrips
{
    partial class FlightHandler
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FlightHandler));
            this.BtnDelete = new System.Windows.Forms.Button();
            this.LblPopup = new System.Windows.Forms.Label();
            this.BtnFdr = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BtnDelete
            // 
            this.BtnDelete.Location = new System.Drawing.Point(12, 40);
            this.BtnDelete.Margin = new System.Windows.Forms.Padding(3, 3, 3, 25);
            this.BtnDelete.Name = "BtnDelete";
            this.BtnDelete.Size = new System.Drawing.Size(98, 30);
            this.BtnDelete.TabIndex = 0;
            this.BtnDelete.Text = "Close Flight";
            this.BtnDelete.UseVisualStyleBackColor = true;
            this.BtnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // LblPopup
            // 
            this.LblPopup.AutoSize = true;
            this.LblPopup.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblPopup.Location = new System.Drawing.Point(85, 9);
            this.LblPopup.Name = "LblPopup";
            this.LblPopup.Size = new System.Drawing.Size(70, 17);
            this.LblPopup.TabIndex = 1;
            this.LblPopup.Text = "Edit Flight";
            // 
            // BtnFdr
            // 
            this.BtnFdr.Enabled = false;
            this.BtnFdr.Location = new System.Drawing.Point(140, 40);
            this.BtnFdr.Margin = new System.Windows.Forms.Padding(3, 3, 3, 25);
            this.BtnFdr.Name = "BtnFdr";
            this.BtnFdr.Size = new System.Drawing.Size(98, 30);
            this.BtnFdr.TabIndex = 2;
            this.BtnFdr.Text = "FDR";
            this.BtnFdr.UseVisualStyleBackColor = true;
            this.BtnFdr.Click += new System.EventHandler(this.BtnFdr_Click);
            // 
            // FlightHandler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(250, 80);
            this.Controls.Add(this.BtnFdr);
            this.Controls.Add(this.LblPopup);
            this.Controls.Add(this.BtnDelete);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FlightHandler";
            this.ShowInTaskbar = false;
            this.Text = "Flight Edit";
            this.Deactivate += new System.EventHandler(this.FlightHandler_Deactivate);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnDelete;
        private System.Windows.Forms.Label LblPopup;
        private System.Windows.Forms.Button BtnFdr;
    }
}