namespace shareDemo2.Trend
{
    partial class Trend
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ct = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ct)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ct);
            this.panel1.Location = new System.Drawing.Point(10, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1127, 613);
            this.panel1.TabIndex = 0;
            // 
            // ct
            // 
            chartArea1.Name = "ChartArea1";
            this.ct.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.ct.Legends.Add(legend1);
            this.ct.Location = new System.Drawing.Point(0, 0);
            this.ct.Name = "ct";
            this.ct.Size = new System.Drawing.Size(1127, 613);
            this.ct.TabIndex = 0;
            this.ct.Text = "chart1";
            // 
            // Trend
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1159, 625);
            this.Controls.Add(this.panel1);
            this.Name = "Trend";
            this.Text = "trend";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ct)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataVisualization.Charting.Chart ct;
    }
}