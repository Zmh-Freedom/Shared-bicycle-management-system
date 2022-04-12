namespace shareDemo2
{
    partial class trend
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
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.uiPanel1 = new Sunny.UI.UIPanel();
            this.ct = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.uiPanel2 = new Sunny.UI.UIPanel();
            this.uiLabel2 = new Sunny.UI.UILabel();
            this.pictureBoxTrend = new System.Windows.Forms.PictureBox();
            this.uiLabel1 = new Sunny.UI.UILabel();
            this.uiPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ct)).BeginInit();
            this.uiPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTrend)).BeginInit();
            this.SuspendLayout();
            // 
            // uiPanel1
            // 
            this.uiPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uiPanel1.Controls.Add(this.uiPanel2);
            this.uiPanel1.Controls.Add(this.ct);
            this.uiPanel1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiPanel1.Location = new System.Drawing.Point(5, 41);
            this.uiPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiPanel1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiPanel1.Name = "uiPanel1";
            this.uiPanel1.Size = new System.Drawing.Size(992, 663);
            this.uiPanel1.TabIndex = 0;
            this.uiPanel1.Text = null;
            this.uiPanel1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ct
            // 
            legend1.Name = "Legend1";
            this.ct.Legends.Add(legend1);
            this.ct.Location = new System.Drawing.Point(49, 179);
            this.ct.Name = "ct";
            this.ct.Size = new System.Drawing.Size(872, 426);
            this.ct.TabIndex = 0;
            this.ct.Text = "chart1";
            // 
            // uiPanel2
            // 
            this.uiPanel2.Controls.Add(this.uiLabel2);
            this.uiPanel2.Controls.Add(this.pictureBoxTrend);
            this.uiPanel2.Controls.Add(this.uiLabel1);
            this.uiPanel2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiPanel2.Location = new System.Drawing.Point(72, 24);
            this.uiPanel2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiPanel2.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiPanel2.Name = "uiPanel2";
            this.uiPanel2.Size = new System.Drawing.Size(202, 117);
            this.uiPanel2.TabIndex = 8;
            this.uiPanel2.Text = null;
            this.uiPanel2.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiLabel2
            // 
            this.uiLabel2.BackColor = System.Drawing.Color.Transparent;
            this.uiLabel2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel2.Location = new System.Drawing.Point(18, 68);
            this.uiLabel2.Name = "uiLabel2";
            this.uiLabel2.Size = new System.Drawing.Size(59, 33);
            this.uiLabel2.TabIndex = 8;
            this.uiLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBoxTrend
            // 
            this.pictureBoxTrend.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxTrend.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBoxTrend.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.pictureBoxTrend.Image = global::shareDemo2.Properties.Resources.bike2;
            this.pictureBoxTrend.Location = new System.Drawing.Point(93, 9);
            this.pictureBoxTrend.Name = "pictureBoxTrend";
            this.pictureBoxTrend.Size = new System.Drawing.Size(108, 93);
            this.pictureBoxTrend.TabIndex = 7;
            this.pictureBoxTrend.TabStop = false;
            // 
            // uiLabel1
            // 
            this.uiLabel1.BackColor = System.Drawing.Color.Transparent;
            this.uiLabel1.Font = new System.Drawing.Font("华文琥珀", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel1.Location = new System.Drawing.Point(8, 9);
            this.uiLabel1.Name = "uiLabel1";
            this.uiLabel1.Size = new System.Drawing.Size(79, 42);
            this.uiLabel1.TabIndex = 6;
            this.uiLabel1.Text = "总计";
            this.uiLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trend
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1002, 707);
            this.Controls.Add(this.uiPanel1);
            this.Name = "trend";
            this.Text = "trend";
            this.uiPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ct)).EndInit();
            this.uiPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTrend)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UIPanel uiPanel1;
        private System.Windows.Forms.DataVisualization.Charting.Chart ct;
        private Sunny.UI.UIPanel uiPanel2;
        private Sunny.UI.UILabel uiLabel2;
        private System.Windows.Forms.PictureBox pictureBoxTrend;
        private Sunny.UI.UILabel uiLabel1;
    }
}