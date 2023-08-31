namespace eStrips
{
    partial class RouteView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RouteView));
            this.RouteBrowser = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // RouteBrowser
            // 
            this.RouteBrowser.AllowNavigation = false;
            this.RouteBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RouteBrowser.Location = new System.Drawing.Point(0, 0);
            this.RouteBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.RouteBrowser.Name = "RouteBrowser";
            this.RouteBrowser.ScriptErrorsSuppressed = true;
            this.RouteBrowser.Size = new System.Drawing.Size(800, 450);
            this.RouteBrowser.TabIndex = 1;
            this.RouteBrowser.Url = new System.Uri("https://skyvector.com", System.UriKind.Absolute);
            // 
            // RouteView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.RouteBrowser);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RouteView";
            this.Text = "Route View";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.RouteView_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser RouteBrowser;
    }
}