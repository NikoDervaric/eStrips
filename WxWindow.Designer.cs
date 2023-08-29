namespace eStrips
{
    partial class WxWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WxWindow));
            this.WxBrowser = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // WxBrowser
            // 
            this.WxBrowser.AllowNavigation = false;
            this.WxBrowser.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.WxBrowser.Location = new System.Drawing.Point(-204, -162);
            this.WxBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.WxBrowser.Name = "WxBrowser";
            this.WxBrowser.ScrollBarsEnabled = false;
            this.WxBrowser.Size = new System.Drawing.Size(699, 534);
            this.WxBrowser.TabIndex = 0;
            this.WxBrowser.Url = new System.Uri("https://meteo.arso.gov.si/uploads/probase/www/observ/radar/si0-rm-anim.gif", System.UriKind.Absolute);
            // 
            // WxWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(495, 372);
            this.Controls.Add(this.WxBrowser);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "WxWindow";
            this.ShowInTaskbar = false;
            this.Text = "WxWindow";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser WxBrowser;
    }
}