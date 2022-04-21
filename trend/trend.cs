using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sunny.UI;
using System.Windows.Forms.DataVisualization.Charting;
namespace shareBike
{
    public partial class trend : UIForm
    {
        public trend(int sel1, int sel2, IQueryable<DateTime> order, int tag)
        {//重载1：只有一个要显示的折线图
            InitializeComponent();
            Cratechart0();
            Order = order;
            ct.Series.Clear();
            uiPanel3.Hide();
            if (sel2 == 1)
            {
                Series_set(1);
                ct.Series[n_series].Name = "开锁";
                uiLabel1.Text = "开锁总计";
            }

            else
            {
                Series_set(2);
                ct.Series[n_series].Name = "关锁";
                uiLabel1.Text = "关锁总计";
            }
            if (tag == 1)
                Cratechart1(sel1, sel2);//绘制折线图
            else
                Cratechart1(sel1, sel2);

            uiLabel4.Text = trendNumber.ToString();
            //添加折线图的Labe
            setLabe(sel1);
        }
        public trend(int sel1, int sel2, IQueryable<DateTime> order1, IQueryable<DateTime> order2, int tag)
        {//重载2：有两个要显示的折线图
            InitializeComponent();
            Cratechart0();//初始化chart的显示格式
            if (tag == 1)
            {
                Series_set(1);
                Order = order1;
                Cratechart1(sel1, sel2);
                ct.Series[n_series].Name = "开锁";
                uiLabel4.Text = Convert.ToString(trendNumber);
                Series_set(2);
                Order = order2;
                Cratechart1(sel1, sel2);
                ct.Series[n_series].Name = "关锁";
                uiLabel5.Text = Convert.ToString(trendNumber);
            }
            else
            {
                Series_set(1);
                Order = order1;
                Cratechart2(sel1, sel2);
                ct.Series[n_series].Name = "开锁";
                uiLabel4.Text = Convert.ToString(trendNumber);
                Series_set(2);
                Order = order2;
                Cratechart2(sel1, sel2);
                ct.Series[n_series].Name = "关锁";
                uiLabel5.Text = Convert.ToString(trendNumber);
            }
            uiLabel1.Text = "开锁总计";
            uiLabel2.Text = "关锁总计";
            //添加折线图的Labe
            setLabe(sel1);
        }
        #region 全局变量
        int n_series = -1;
        int trendNumber = 0;
        private UILabel uiLabel3;
        private UILabel uiLabel5;
        private UILabel uiLabel4;
        IQueryable<DateTime> Order = null;
        #endregion

        private void setLabe(int sel1)
        {
            switch (sel1)
            {
                case 0:
                    uiLabel3.Text = "过去三天";
                    break;
                case 1:
                    uiLabel3.Text = "过去三周";
                    break;
                case 2:
                    uiLabel3.Text = "过去3个月";
                    break;
                case 3:
                    uiLabel3.Text = "去年同日";
                    break;
                case 4:
                    uiLabel3.Text = "去年同月";
                    break;
            }
        }
        private void Cratechart0()
        {
            //设计chart图的网格
            //ct.ChartAreas.Add(new ChartArea() { Name = "ca1" }); //背景框
            ct.ChartAreas[0].Axes[0].MajorGrid.Enabled = false; //X轴上网格
            ct.ChartAreas[0].Axes[1].MajorGrid.Enabled = false; //y轴上网格
            ct.ChartAreas[0].Axes[0].MajorGrid.LineDashStyle = ChartDashStyle.Dash; //网格类型 短横线
            ct.ChartAreas[0].Axes[0].MajorGrid.LineColor = Color.Gray;
            ct.ChartAreas[0].Axes[0].MajorTickMark.Enabled = false; // x轴上突出的小点
            ct.ChartAreas[0].Axes[1].MajorTickMark.Enabled = false; //
            ct.ChartAreas[0].Axes[1].IsInterlaced = false; //显示交错带
            ct.ChartAreas[0].Axes[1].MajorGrid.LineDashStyle = ChartDashStyle.Dash; //网格类型 短横线
            ct.ChartAreas[0].Axes[1].MajorGrid.LineColor = Color.Blue;
            ct.ChartAreas[0].Axes[1].MajorGrid.LineWidth = 3;
            ct.ChartAreas[0].BackColor = System.Drawing.Color.Transparent; //设置区域内背景透明
            ct.ChartAreas[0].AxisX.Title = "时段";
            ct.ChartAreas[0].AxisY.Title = "数量/辆";
        }
        public void Series_set(int i)
        {
            //设置线的样式
            ct.Series.Add(new Series());
            n_series++;
            ct.Series[n_series].Label = "#VAL";                //设置显示X Y的值    
            ct.Series[n_series].ChartArea = ct.ChartAreas[0].Name; //设置图表背景框ChartArea 
            ct.Series[n_series].ChartType = SeriesChartType.Line; //图类型(折线)
            if (i == 1)
            {
                ct.Series[n_series].Color = Color.Orange; //线条颜色
                ct.Series[n_series].MarkerColor = Color.Orange; //标记点中心颜色
                ct.Series[n_series].BorderWidth = 4; //线条粗细
            }
            else
            {
                ct.Series[n_series].Color = Color.Green; //线条颜色
                ct.Series[n_series].MarkerColor = Color.Green; //标记点中心颜色
                ct.Series[n_series].BorderWidth = 2; //线条粗细
            }
            ct.Series[n_series].MarkerBorderColor = Color.Red; //标记点边框颜色
            ct.Series[n_series].MarkerBorderWidth = 3; //标记点边框大小            
            ct.Series[n_series].MarkerSize = 5; //标记点大小
            ct.Series[n_series].MarkerStyle = MarkerStyle.Circle; //标记点类型
        }
        public void Cratechart1(int sel1, int sel2)
        {
            //绘制开锁/关锁的小时折线
            List<string> xDate1 = new List<string>();
            List<int> yDate1 = new List<int>();
            DateTime date = DateTime.Today;
            trendNumber = 0;
            for (int i = 0; i < 24; i++)
            {
                int s_hour = date.AddHours(i).Hour;
                int e_hour = date.AddHours(i + 1).Hour;
                xDate1.Add(date.AddHours(i).ToString("HH") + "点");
                yDate1.Add((from x in Order
                            where x.Hour == s_hour
                            select x).Count());
                trendNumber += yDate1[i];
            }
            //ct.Series[n_series].Name = "解锁单车";
            ct.Series[n_series].Points.DataBindXY(xDate1, yDate1);//添加数据来源
            ct.Series[n_series].ToolTip = "#VALX点\r#VAL辆";
        }


        public void Cratechart2(int sel1, int sel2)
        {
            //绘制开锁/关锁的周折线
            List<string> xDate1 = new List<string>() { "星期一", "星期二", "星期三", "星期四", "星期五", "星期六", " 星期日" };
            List<int> yDate1 = new List<int>();
            DateTime date = DateTime.Today;
            trendNumber = 0;
            //设置除数
            for (int i = 0; i < 7; i++)
            {
                int set = i;
                if (i == 6) set = -1;
                yDate1.Add((from x in Order
                            where x.DayOfWeek - DayOfWeek.Monday == set
                            select x).Count());
                trendNumber += yDate1[i];
            }
            // ct.Series[n_series].Name = "解锁单车";
            ct.Series[n_series].Points.DataBindXY(xDate1, yDate1);//添加数据来源
            ct.Series[n_series].ToolTip = "#VALX\r#VAL辆";
        }
        #region 窗口
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
            this.uiPanel1 = new Sunny.UI.UIPanel();
            this.uiLabel3 = new Sunny.UI.UILabel();
            this.uiPanel3 = new Sunny.UI.UIPanel();
            this.uiLabel5 = new Sunny.UI.UILabel();
            this.uiLabel2 = new Sunny.UI.UILabel();
            this.uiPanel2 = new Sunny.UI.UIPanel();
            this.uiLabel4 = new Sunny.UI.UILabel();
            this.uiLabel1 = new Sunny.UI.UILabel();
            this.ct = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBoxTrend = new System.Windows.Forms.PictureBox();
            this.uiPanel1.SuspendLayout();
            this.uiPanel3.SuspendLayout();
            this.uiPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ct)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTrend)).BeginInit();
            this.SuspendLayout();
            // 
            // uiPanel1
            // 
            this.uiPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uiPanel1.Controls.Add(this.uiLabel3);
            this.uiPanel1.Controls.Add(this.uiPanel3);
            this.uiPanel1.Controls.Add(this.uiPanel2);
            this.uiPanel1.Controls.Add(this.ct);
            this.uiPanel1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiPanel1.Location = new System.Drawing.Point(5, 41);
            this.uiPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiPanel1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiPanel1.Name = "uiPanel1";
            this.uiPanel1.Size = new System.Drawing.Size(1247, 652);
            this.uiPanel1.TabIndex = 0;
            this.uiPanel1.Text = null;
            this.uiPanel1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiLabel3
            // 
            this.uiLabel3.BackColor = System.Drawing.Color.Azure;
            this.uiLabel3.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel3.Location = new System.Drawing.Point(860, 94);
            this.uiLabel3.Name = "uiLabel3";
            this.uiLabel3.Size = new System.Drawing.Size(249, 34);
            this.uiLabel3.TabIndex = 10;
            this.uiLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiPanel3
            // 
            this.uiPanel3.Controls.Add(this.uiLabel5);
            this.uiPanel3.Controls.Add(this.pictureBox1);
            this.uiPanel3.Controls.Add(this.uiLabel2);
            this.uiPanel3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiPanel3.Location = new System.Drawing.Point(286, 27);
            this.uiPanel3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiPanel3.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiPanel3.Name = "uiPanel3";
            this.uiPanel3.Size = new System.Drawing.Size(217, 101);
            this.uiPanel3.TabIndex = 9;
            this.uiPanel3.Text = null;
            this.uiPanel3.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiLabel5
            // 
            this.uiLabel5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.uiLabel5.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel5.Location = new System.Drawing.Point(16, 56);
            this.uiLabel5.Name = "uiLabel5";
            this.uiLabel5.Size = new System.Drawing.Size(87, 37);
            this.uiLabel5.TabIndex = 8;
            this.uiLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiLabel2
            // 
            this.uiLabel2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.uiLabel2.BackColor = System.Drawing.Color.Transparent;
            this.uiLabel2.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel2.Location = new System.Drawing.Point(3, 14);
            this.uiLabel2.Name = "uiLabel2";
            this.uiLabel2.Size = new System.Drawing.Size(143, 42);
            this.uiLabel2.TabIndex = 6;
            this.uiLabel2.Text = "关锁总计";
            this.uiLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiPanel2
            // 
            this.uiPanel2.Controls.Add(this.uiLabel4);
            this.uiPanel2.Controls.Add(this.pictureBoxTrend);
            this.uiPanel2.Controls.Add(this.uiLabel1);
            this.uiPanel2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiPanel2.Location = new System.Drawing.Point(18, 27);
            this.uiPanel2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiPanel2.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiPanel2.Name = "uiPanel2";
            this.uiPanel2.Size = new System.Drawing.Size(207, 101);
            this.uiPanel2.TabIndex = 8;
            this.uiPanel2.Text = null;
            this.uiPanel2.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiLabel4
            // 
            this.uiLabel4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.uiLabel4.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel4.Location = new System.Drawing.Point(15, 56);
            this.uiLabel4.Name = "uiLabel4";
            this.uiLabel4.Size = new System.Drawing.Size(83, 37);
            this.uiLabel4.TabIndex = 8;
            this.uiLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiLabel1
            // 
            this.uiLabel1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.uiLabel1.BackColor = System.Drawing.Color.Transparent;
            this.uiLabel1.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel1.Location = new System.Drawing.Point(3, 14);
            this.uiLabel1.Name = "uiLabel1";
            this.uiLabel1.Size = new System.Drawing.Size(131, 42);
            this.uiLabel1.TabIndex = 6;
            this.uiLabel1.Text = "开锁总计";
            this.uiLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ct
            // 
            this.ct.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea1.AxisX.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
            chartArea1.Name = "ChartArea1";
            this.ct.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.ct.Legends.Add(legend1);
            this.ct.Location = new System.Drawing.Point(18, 136);
            this.ct.Name = "ct";
            this.ct.Size = new System.Drawing.Size(1210, 513);
            this.ct.TabIndex = 0;
            this.ct.Text = "chart1";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImage = global::shareBike.Properties.Resources.bike2;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.pictureBox1.Location = new System.Drawing.Point(152, 26);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(62, 57);
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBoxTrend
            // 
            this.pictureBoxTrend.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.pictureBoxTrend.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxTrend.BackgroundImage = global::shareBike.Properties.Resources.bike2;
            this.pictureBoxTrend.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBoxTrend.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.pictureBoxTrend.Location = new System.Drawing.Point(142, 26);
            this.pictureBoxTrend.Name = "pictureBoxTrend";
            this.pictureBoxTrend.Size = new System.Drawing.Size(65, 57);
            this.pictureBoxTrend.TabIndex = 7;
            this.pictureBoxTrend.TabStop = false;
            // 
            // trend
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1260, 700);
            this.Controls.Add(this.uiPanel1);
            this.Name = "trend";
            this.Text = "趋势分析";
            this.uiPanel1.ResumeLayout(false);
            this.uiPanel3.ResumeLayout(false);
            this.uiPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ct)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTrend)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UIPanel uiPanel1;
        private System.Windows.Forms.DataVisualization.Charting.Chart ct;
        private Sunny.UI.UIPanel uiPanel2;
        private System.Windows.Forms.PictureBox pictureBoxTrend;
        private Sunny.UI.UILabel uiLabel1;
        private Sunny.UI.UIPanel uiPanel3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Sunny.UI.UILabel uiLabel2;

        #endregion
    }
}
