namespace SketchIt
{
    partial class SplashScreenForm
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
            this.ctlCanvas = new SketchIt.Windows.SketchContainer();
            this.SuspendLayout();
            // 
            // ctlCanvas
            // 
            this.ctlCanvas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.ctlCanvas.DesignMode = false;
            this.ctlCanvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctlCanvas.Location = new System.Drawing.Point(2, 2);
            this.ctlCanvas.Name = "ctlCanvas";
            this.ctlCanvas.Size = new System.Drawing.Size(726, 500);
            this.ctlCanvas.Sketch = null;
            this.ctlCanvas.TabIndex = 4;
            this.ctlCanvas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ctlCanvas_MouseDown);
            // 
            // SplashScreenForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(730, 504);
            this.Controls.Add(this.ctlCanvas);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "SplashScreenForm";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SketchIt";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.SplashScreenForm_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private Windows.SketchContainer ctlCanvas;
    }
}