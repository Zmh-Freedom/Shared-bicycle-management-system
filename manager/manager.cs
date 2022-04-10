using shareDemo2;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using shareDemo2.Trend;
using System.Windows.Forms.DataVisualization.Charting;
namespace shareDemo3
{
    public partial class managerForm : Form
    {
        public managerForm()
        {
            InitializeComponent();
            #region 数据库初始化
            dc = new DBDataContext();
            #endregion

            #region 绘图初始化
            blueBrush = new SolidBrush(Color.Blue);
            silverBrush = new SolidBrush(Color.Silver);
            goldBrush = new SolidBrush(Color.Gold);
            greenBrush = new SolidBrush(Color.Green);
            bike_stratBrush = new SolidBrush(Color.Yellow);
            bike_endBrush = new SolidBrush(Color.YellowGreen);
            shadowBrush = new SolidBrush(Color.FromArgb(50, Color.SteelBlue));
            bikesDisplay();
            #endregion

            #region 逻辑代码初始化
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            #endregion
            //图表初始化
            
            Cratechart0();
        }

        //展示单车
        private void bikesDisplay()
        {
            try
            {
                numAvailable = 0;
                numOutServiceArea = 0;
                numToOverhaul = 0;
                numUsing = 0;

                //绘制单车
                g = Graphics.FromImage(pictureBox1.Image);
                IQueryable<bike> qbikes = from p in dc.bike
                                          where p.flag < 4
                                          select p;
                foreach (var q in qbikes)
                {
                    if(q.flag == 0)
                    {
                        g.FillEllipse(blueBrush, q.current_x.Value - 8, q.current_y.Value - 8, 16, 16);
                        numAvailable += 1;
                    }
                    else if(q.flag == 1)
                    {
                        g.FillEllipse(silverBrush, q.current_x.Value - 8, q.current_y.Value - 8, 16, 16);
                        numOutServiceArea += 1;
                    }
                    else if(q.flag == 2)
                    {
                        g.FillEllipse(goldBrush, q.current_x.Value - 8, q.current_y.Value - 8, 16, 16);
                        numToOverhaul += 1;
                    }
                    else if(q.flag == 3)
                    {
                        g.FillEllipse(greenBrush, q.current_x.Value - 8, q.current_y.Value - 8, 16, 16);
                        numUsing += 1;
                    }
                }

                //绘制服务区阴影
                IQueryable<fence> qfences = from p in dc.fence
                                            where p.tag == 1
                                            select p;
                foreach (var q in qfences)
                {
                    g.FillRectangle(shadowBrush, new Rectangle(q.origin_x, q.origin_y, q.width, q.height));
                }
                g.Flush();

                label2.Text = "当前可用 "+numAvailable.ToString();
                label3.Text = "正在使用 "+numUsing.ToString();
                label4.Text = "等待检查 "+numToOverhaul.ToString();
                label5.Text = "服务区外 "+numOutServiceArea.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            toolStripStatusLabel2.Text = (e.X-5).ToString();
            toolStripStatusLabel4.Text = (e.Y-4).ToString();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label6.Text = DateTime.Now.ToString();
        }
       
        private void PictureBox6_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(System.Drawing.Pens.Blue, myRect);
        }
        private void pictureBox6_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            myRect = new Rectangle(e.X,e.Y,0,0);
            pictureBox6.Refresh();
            origin_SelectX = e.X - 5;
            origin_SelectY = e.Y - 4;
            RepaintMap();
        }

        private void pictureBox6_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
            selectWidth = e.X - myRect.Left;
            selectHeight = e.Y - myRect.Top;
        }
        private void pictureBox6_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                myRect.Width = e.X - myRect.Left;
                myRect.Height = e.Y - myRect.Top;
            }
            pictureBox6.Refresh();
        }

        private SplitContainer splitContainer3;
        private PictureBox pictureBox7;
        private System.Windows.Forms.DataVisualization.Charting.Chart ct;


        #region desiner
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.pictureBox7 = new System.Windows.Forms.PictureBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.statusStrip2 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.ct = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            this.statusStrip2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ct)).BeginInit();
            this.SuspendLayout();
            // 
            // tabPage6
            // 
            this.tabPage6.Location = new System.Drawing.Point(4, 33);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(1123, 662);
            this.tabPage6.TabIndex = 5;
            this.tabPage6.Text = "智能调度";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.splitContainer3);
            this.tabPage5.Location = new System.Drawing.Point(4, 33);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(914, 559);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "下达任务";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(3, 3);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.AutoScroll = true;
            this.splitContainer3.Panel1.Controls.Add(this.pictureBox7);
            this.splitContainer3.Size = new System.Drawing.Size(908, 553);
            this.splitContainer3.SplitterDistance = 637;
            this.splitContainer3.TabIndex = 0;
            // 
            // pictureBox7
            // 
            this.pictureBox7.Image = global::shareDemo2.Properties.Resources.map1;
            this.pictureBox7.Location = new System.Drawing.Point(-7, -3);
            this.pictureBox7.Name = "pictureBox7";
            this.pictureBox7.Size = new System.Drawing.Size(1844, 884);
            this.pictureBox7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox7.TabIndex = 0;
            this.pictureBox7.TabStop = false;
            // 
            // tabPage4
            // 
            this.tabPage4.Location = new System.Drawing.Point(4, 33);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(1123, 662);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "仓库数据";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.splitContainer2);
            this.tabPage2.Controls.Add(this.statusStrip2);
            this.tabPage2.Location = new System.Drawing.Point(4, 33);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1123, 662);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "用车热图";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(3, 3);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.AutoScroll = true;
            this.splitContainer2.Panel1.Controls.Add(this.pictureBox6);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.ct);
            this.splitContainer2.Panel2.Controls.Add(this.label8);
            this.splitContainer2.Panel2.Controls.Add(this.label7);
            this.splitContainer2.Panel2.Controls.Add(this.comboBox2);
            this.splitContainer2.Panel2.Controls.Add(this.comboBox1);
            this.splitContainer2.Panel2.Controls.Add(this.button1);
            this.splitContainer2.Size = new System.Drawing.Size(1117, 625);
            this.splitContainer2.SplitterDistance = 495;
            this.splitContainer2.TabIndex = 1;
            // 
            // pictureBox6
            // 
            this.pictureBox6.Image = global::shareDemo2.Properties.Resources.map1;
            this.pictureBox6.Location = new System.Drawing.Point(-7, -6);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(1844, 884);
            this.pictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox6.TabIndex = 0;
            this.pictureBox6.TabStop = false;
            this.pictureBox6.Paint += new System.Windows.Forms.PaintEventHandler(this.PictureBox6_Paint);
            this.pictureBox6.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox6_MouseDown);
            this.pictureBox6.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox6_MouseMove);
            this.pictureBox6.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox6_MouseUp);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(26, 129);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(118, 24);
            this.label8.TabIndex = 4;
            this.label8.Text = "热图展示类型";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(25, 31);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(118, 24);
            this.label7.TabIndex = 3;
            this.label7.Text = "热图时间范围";
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "全部显示",
            "显示解锁",
            "显示还车"});
            this.comboBox2.Location = new System.Drawing.Point(26, 165);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(121, 32);
            this.comboBox2.TabIndex = 2;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "过去三天",
            "过去三周",
            "过去3个月",
            "去年同日",
            "去年同月"});
            this.comboBox1.Location = new System.Drawing.Point(26, 68);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 32);
            this.comboBox1.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(29, 247);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(118, 47);
            this.button1.TabIndex = 0;
            this.button1.Text = "趋势分析";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // statusStrip2
            // 
            this.statusStrip2.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel5});
            this.statusStrip2.Location = new System.Drawing.Point(3, 628);
            this.statusStrip2.Name = "statusStrip2";
            this.statusStrip2.Size = new System.Drawing.Size(1117, 31);
            this.statusStrip2.TabIndex = 0;
            // 
            // toolStripStatusLabel5
            // 
            this.toolStripStatusLabel5.Name = "toolStripStatusLabel5";
            this.toolStripStatusLabel5.Size = new System.Drawing.Size(208, 24);
            this.toolStripStatusLabel5.Text = "框选区域后进行趋势分析";
            // 
            // tabPage1
            // 
            this.tabPage1.AutoScroll = true;
            this.tabPage1.Controls.Add(this.splitContainer1);
            this.tabPage1.Controls.Add(this.statusStrip1);
            this.tabPage1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabPage1.Location = new System.Drawing.Point(4, 33);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1123, 662);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "地图快照";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.AutoScroll = true;
            this.splitContainer1.Panel1.Controls.Add(this.pictureBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.label6);
            this.splitContainer1.Panel2.Controls.Add(this.pictureBox5);
            this.splitContainer1.Panel2.Controls.Add(this.pictureBox4);
            this.splitContainer1.Panel2.Controls.Add(this.pictureBox3);
            this.splitContainer1.Panel2.Controls.Add(this.pictureBox2);
            this.splitContainer1.Panel2.Controls.Add(this.label5);
            this.splitContainer1.Panel2.Controls.Add(this.label4);
            this.splitContainer1.Panel2.Controls.Add(this.label3);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Size = new System.Drawing.Size(1117, 625);
            this.splitContainer1.SplitterDistance = 674;
            this.splitContainer1.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::shareDemo2.Properties.Resources.map1;
            this.pictureBox1.Location = new System.Drawing.Point(-7, -6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1844, 884);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 55);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 24);
            this.label6.TabIndex = 9;
            this.label6.Text = "当前时间";
            // 
            // pictureBox5
            // 
            this.pictureBox5.Image = global::shareDemo2.Properties.Resources.gray;
            this.pictureBox5.Location = new System.Drawing.Point(7, 294);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(15, 15);
            this.pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox5.TabIndex = 8;
            this.pictureBox5.TabStop = false;
            // 
            // pictureBox4
            // 
            this.pictureBox4.Image = global::shareDemo2.Properties.Resources.yellow;
            this.pictureBox4.Location = new System.Drawing.Point(7, 235);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(15, 15);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox4.TabIndex = 7;
            this.pictureBox4.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::shareDemo2.Properties.Resources.green;
            this.pictureBox3.Location = new System.Drawing.Point(7, 177);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(15, 15);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox3.TabIndex = 6;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::shareDemo2.Properties.Resources.blue;
            this.pictureBox2.Location = new System.Drawing.Point(7, 112);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(15, 15);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 5;
            this.pictureBox2.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(30, 292);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 24);
            this.label5.TabIndex = 4;
            this.label5.Text = "服务区外";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(30, 232);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 24);
            this.label4.TabIndex = 3;
            this.label4.Text = "等待检查";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 172);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 24);
            this.label3.TabIndex = 2;
            this.label3.Text = "正在使用";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 107);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 24);
            this.label2.TabIndex = 1;
            this.label2.Text = "当前可用";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(18, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "账户名";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3,
            this.toolStripStatusLabel4});
            this.statusStrip1.Location = new System.Drawing.Point(3, 628);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1117, 31);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "X";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(19, 24);
            this.toolStripStatusLabel1.Text = "x";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(195, 24);
            this.toolStripStatusLabel2.Text = "toolStripStatusLabel2";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(20, 24);
            this.toolStripStatusLabel3.Text = "y";
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(195, 24);
            this.toolStripStatusLabel4.Text = "toolStripStatusLabel4";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage6);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControl1.Location = new System.Drawing.Point(-1, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1131, 699);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 33);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1123, 662);
            this.tabPage3.TabIndex = 6;
            this.tabPage3.Text = "人事管理";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // ct
            // 
            this.ct.Enabled = false;
            legend2.Name = "Legend1";
            this.ct.Legends.Add(legend2);
            this.ct.Location = new System.Drawing.Point(26, 416);
            this.ct.Name = "ct";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.ct.Series.Add(series2);
            this.ct.Size = new System.Drawing.Size(846, 495);
            this.ct.TabIndex = 5;
            this.ct.Text = "chart1";
            // 
            // managerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1129, 695);
            this.Controls.Add(this.tabControl1);
            this.Name = "managerForm";
            this.Text = "管理员界面";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.tabPage5.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            this.statusStrip2.ResumeLayout(false);
            this.statusStrip2.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ct)).EndInit();
            this.ResumeLayout(false);

        }



        #endregion

        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.StatusStrip statusStrip2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.PictureBox pictureBox6;
        private System.Windows.Forms.TabPage tabPage3;
        private ComboBox comboBox1;
        private Button button1;
        private ComboBox comboBox2;
        private Label label8;
        private Label label7;



        #endregion
        
        private void button1_Click(object sender, EventArgs e)
        {
            ct.Series.Clear();
            n_series = -1;
            int sel1=comboBox1.SelectedIndex;
            int sel2=comboBox2.SelectedIndex;
            trend_start = 0;
            trend_end = 0;
            RepaintMap();
            switch (sel2)
            {
                case 1: var order1 = Sel1(sel1); 
                    Cratechart1(sel1,sel2,order1);
                    Dream_trendbike(order1, 1);
                    break;
                case 2: var order2 = Sel2(sel1);
                    Cratechart2(sel1, sel2, order2);
                    Dream_trendbike(order2, 2);
                    break;
                case 0: var order3 = Sel1(sel1);
                    var order4 = Sel2(sel1);
                    Cratechart1(sel1, sel2, order3);
                    Cratechart2(sel1, sel2, order4);
                    Dream_trendbike(order3, 1);
                    Dream_trendbike(order4, 2);
                    break;

            }
        }
        public IQueryable<orderform> Sel1(int sel)
        {
            switch (sel)
            {
                case 0:
                    var Order1 = from m in dc.orderform
                                 where m.start_time.Value <= DateTime.Now && m.start_time.Value >= DateTime.Now.AddDays(-3)
                                 && m.start_x >= origin_SelectX && m.start_x <= origin_SelectX + selectWidth
                                 && m.start_y >= origin_SelectY && m.start_y <= origin_SelectY + selectHeight
                                 select m; return Order1; 
                case 1:
                    var Order2 = from m in dc.orderform
                                 where m.start_time.Value <= DateTime.Now && m.start_time.Value >= DateTime.Now.AddDays(-7)
                                 && m.start_x >= origin_SelectX && m.start_x <= origin_SelectX + selectWidth
                                 && m.start_y >= origin_SelectY && m.start_y <= origin_SelectY + selectHeight
                                 select m; return Order2;
                case 2:
                    var Order3 = from m in dc.orderform
                                 where m.start_time.Value <= DateTime.Now && m.start_time.Value >= DateTime.Now.AddMonths(-3)
                                 && m.start_x >= origin_SelectX && m.start_x <= origin_SelectX + selectWidth
                                 && m.start_y >= origin_SelectY && m.start_y <= origin_SelectY + selectHeight
                                 select m; return Order3;
                case 3:
                    var Order4 = from m in dc.orderform
                                 where m.start_time.Value.Day == DateTime.Now.AddYears(-1).Day
                                 &&m.start_time.Value.Year == DateTime.Now.AddYears(-1).Year
                                 && m.start_x >= origin_SelectX && m.start_x <= origin_SelectX + selectWidth
                                 && m.start_y >= origin_SelectY && m.start_y <= origin_SelectY + selectHeight
                                 select m; return Order4;
                case 4:
                    var Order5 = from m in dc.orderform
                                 where m.start_time.Value.Month == DateTime.Now.AddYears(-1).Month
                                 && m.start_time.Value.Year == DateTime.Now.AddYears(-1).Year
                                 && m.start_x >= origin_SelectX && m.start_x <= origin_SelectX + selectWidth
                                 && m.start_y >= origin_SelectY && m.start_y <= origin_SelectY + selectHeight
                                 select m; return Order5;
            }
            return null;
        }
        public IQueryable<orderform> Sel2(int sel)
        {
            switch (sel)
            {
                case 0:
                    var Order1 = from m in dc.orderform
                                 where m.end_time.Value <= DateTime.Now && m.end_time.Value >= DateTime.Now.AddDays(-3)
                                 && m.end_x >= origin_SelectX && m.end_x <= origin_SelectX + selectWidth
                                 && m.end_y >= origin_SelectY && m.end_y <= origin_SelectY + selectHeight
                                 select m; return Order1;
                case 1:
                    var Order2 = from m in dc.orderform
                                 where m.end_time.Value <= DateTime.Now && m.end_time.Value >= DateTime.Now.AddDays(-7)
                                 && m.end_x >= origin_SelectX && m.end_x <= origin_SelectX + selectWidth
                                 && m.end_y >= origin_SelectY && m.end_y <= origin_SelectY + selectHeight
                                 select m; return Order2;
                case 2:
                    var Order3 = from m in dc.orderform
                                 where m.end_time.Value <= DateTime.Now && m.end_time.Value >= DateTime.Now.AddMonths(-3)
                                 && m.end_x >= origin_SelectX && m.end_x <= origin_SelectX + selectWidth
                                 && m.end_y >= origin_SelectY && m.end_y <= origin_SelectY + selectHeight
                                 select m; return Order3;
                case 3:
                    var Order4 = from m in dc.orderform
                                 where m.end_time.Value.Day == DateTime.Now.AddYears(-1).Day
                                 && m.end_time.Value.Year == DateTime.Now.AddYears(-1).Year
                                 && m.end_x >= origin_SelectX && m.end_x <= origin_SelectX + selectWidth
                                 && m.end_y >= origin_SelectY && m.end_y <= origin_SelectY + selectHeight
                                 select m; return Order4;
                case 4:
                    var Order5 = from m in dc.orderform
                                 where m.end_time.Value.Month == DateTime.Now.AddYears(-1).Month
                                 && m.end_time.Value.Year == DateTime.Now.AddYears(-1).Year
                                 && m.end_x >= origin_SelectX && m.end_x <= origin_SelectX + selectWidth
                                 && m.end_y >= origin_SelectY && m.end_y <= origin_SelectY + selectHeight
                                 select m; return Order5;
            }
            return null;
        }
        public void Cratechart0()
        {
            ct.ChartAreas.Add(new ChartArea() { Name = "ca1" }); //背景框
            ct.ChartAreas[0].Axes[0].MajorGrid.Enabled = false; //X轴上网格
            ct.ChartAreas[0].Axes[1].MajorGrid.Enabled = false; //y轴上网格
            ct.ChartAreas[0].Axes[0].MajorGrid.LineDashStyle = ChartDashStyle.Dash; //网格类型 短横线
            ct.ChartAreas[0].Axes[0].MajorGrid.LineColor = Color.Gray;
            ct.ChartAreas[0].Axes[0].MajorTickMark.Enabled = false; // x轴上突出的小点
            ct.ChartAreas[0].Axes[1].MajorTickMark.Enabled = false; //
            ct.ChartAreas[0].Axes[1].IsInterlaced = true; //显示交错带
            ct.ChartAreas[0].Axes[1].MajorGrid.LineDashStyle = ChartDashStyle.Dash; //网格类型 短横线
            ct.ChartAreas[0].Axes[1].MajorGrid.LineColor = Color.Blue;
            ct.ChartAreas[0].Axes[1].MajorGrid.LineWidth = 3;
            ct.ChartAreas[0].BackColor = System.Drawing.Color.Transparent; //设置区域内背景透明
        }
        
        public void Cratechart1(int sel1, int sel2, IQueryable<orderform> order)
        {

            
            List<string> xDate1 = new List<string>();
            List<int> yDate1 = new List<int>();
            switch (sel1)
            {
                case 0:
                    for (int i = 0; i < 3; i++)
                    {
                        xDate1.Add(DateTime.Now.AddDays(i - 2).ToString("M"));
                        yDate1.Add((from x in order
                                    where x.start_time.Value.Day == DateTime.Now.AddDays(i - 2).Day
                                    select x).Count());
                    }
                    break;
                case 1:
                    xDate1 = new List<string>() { "上上周", "上周", "这周" };
                    int week = DateTime.Now.DayOfWeek - DayOfWeek.Monday;
                    for (int i = 2; i >= 0; i--)
                    {
                        yDate1.Add((from x in order
                                    where x.start_time.Value.Date >= DateTime.Now.AddDays(-week + (-i * 7)) &&
                                    x.start_time.Value.Date <= DateTime.Now.AddDays(-i * 7)
                                    select x).Count());
                    }
                    break;
                case 2:
                    for (int i = 0; i < 3; i++)
                    {
                        xDate1.Add(DateTime.Now.AddMonths(i - 2).ToString("Y"));
                        yDate1.Add((from x in order
                                    where x.start_time.Value.Month == DateTime.Now.AddMonths(i - 2).Month
                                    select x).Count());
                    }
                    break;
                case 3:
                    for (int i = 0; i < 24; i++)
                    {
                        DateTime d1 = new System.DateTime(2022, 1, 1, 0, 0, 0);
                        int temp2 = (from x in order
                                     where x.start_time.Value.Hour == i
                                     select x).Count();
                        if (temp2 != 0 || i == 0 || i == 12 || i == 23)
                        {
                            xDate1.Add(d1.AddHours(i).ToString("t"));
                            yDate1.Add(temp2);
                        }

                    }
                    break;
                case 4:
                    int y = DateTime.Now.AddYears(-1).Year;
                    int m = DateTime.Now.AddYears(-1).Month;
                    DateTime d = new System.DateTime(y, m, 1);
                    for (int i = 0; i < DateTime.DaysInMonth(y, m); i++)
                    {
                        int temp3 = (from x in order
                                     where x.start_time.Value.Day == i + 1
                                     select x).Count();
                        if (temp3 != 0 || i == 0 || i == 14 || i == DateTime.DaysInMonth(y, m) - 1)
                        {
                            xDate1.Add(d.AddDays(i).ToString("M"));
                            //ct.ChartAreas[0].Axes[0].LabelStyle.Format = "#日";
                            yDate1.Add(temp3);
                        }

                    }
                    break;
            }
            ct.Series.Add(new Series());
            n_series++;
            ct.Series[n_series].Label = "#VAL";                //设置显示X Y的值    
            ct.Series[n_series].Name = "开始用车";
            ct.Series[n_series].ChartArea = ct.ChartAreas[0].Name; //设置图表背景框ChartArea 
            ct.Series[n_series].ChartType = SeriesChartType.Line; //图类型(折线)
            ct.Series[n_series].Color = Color.Yellow; //线条颜色
            ct.Series[n_series].BorderWidth = 3; //线条粗细
            ct.Series[n_series].MarkerBorderColor = Color.Red; //标记点边框颜色
            ct.Series[n_series].MarkerBorderWidth = 4; //标记点边框大小
            ct.Series[n_series].MarkerColor = Color.Yellow; //标记点中心颜色
            ct.Series[n_series].MarkerSize = 5; //标记点大小
            ct.Series[n_series].MarkerStyle = MarkerStyle.Circle; //标记点类型
            ct.Series[n_series].Points.DataBindXY(xDate1, yDate1);
            return;
        }
        public void Cratechart2(int sel1, int sel2, IQueryable<orderform> order)
        {
            List<string> xDate1 = new List<string>();
            List<int> yDate1 = new List<int>();
            switch (sel1)
            {
                case 0:
                    for (int i = 0; i < 3; i++)
                    {
                        xDate1.Add(DateTime.Now.AddDays(i - 2).ToString("M"));
                        yDate1.Add((from x in order
                                    where x.start_time.Value.Day == DateTime.Now.AddDays(i - 2).Day
                                    select x).Count());
                    }
                    break;
                case 1:
                    xDate1 = new List<string>() { "上上周", "上周", "这周" };
                    int week = DateTime.Now.DayOfWeek - DayOfWeek.Monday;
                    for (int i = 2; i >= 0; i--)
                    {
                        yDate1.Add((from x in order
                                    where x.start_time.Value.Date >= DateTime.Now.AddDays(-week + (-i * 7)) &&
                                    x.start_time.Value.Date <= DateTime.Now.AddDays(-i * 7)
                                    select x).Count());
                    }
                    break;
                case 2:
                    for (int i = 0; i < 3; i++)
                    {
                        xDate1.Add(DateTime.Now.AddMonths(i - 2).ToString("Y"));
                        yDate1.Add((from x in order
                                    where x.start_time.Value.Month == DateTime.Now.AddMonths(i - 2).Month
                                    select x).Count());
                    }
                    break;
                case 3:
                    for (int i = 0; i < 24; i++)
                    {
                        DateTime d1 = new System.DateTime(2022, 1, 1, 0, 0, 0);
                        int temp2 = (from x in order
                                     where x.start_time.Value.Hour == i
                                     select x).Count();
                        if (temp2 != 0 || i == 0 || i == 12 || i == 23)
                        {
                            xDate1.Add(d1.AddHours(i).ToString("t"));
                            yDate1.Add(temp2);
                        }

                    }
                    break;
                case 4:
                    int y = DateTime.Now.AddYears(-1).Year;
                    int m = DateTime.Now.AddYears(-1).Month;
                    DateTime d = new System.DateTime(y, m, 1);
                    for (int i = 0; i < DateTime.DaysInMonth(y, m); i++)
                    {
                        int temp3 = (from x in order
                                     where x.start_time.Value.Day == i + 1
                                     select x).Count();
                        if (temp3 != 0 || i == 0 || i == 14 || i == DateTime.DaysInMonth(y, m) - 1)
                        {
                            xDate1.Add(d.AddDays(i).ToString("d"));
                            yDate1.Add(temp3);
                        }

                    }
                    break;
            }

            ct.Series.Add(new Series());
            n_series++; ct.Series[n_series].Points.DataBindXY(xDate1, yDate1);
            ct.Series[n_series].Label = "#VAL";                //设置显示X Y的值    
            ct.Series[n_series].Name = "结束用车";
            ct.Series[n_series].ChartArea = ct.ChartAreas[0].Name; //设置图表背景框ChartArea 
            ct.Series[n_series].ChartType = SeriesChartType.Line; //图类型(折线)
            ct.Series[n_series].Color = Color.YellowGreen; //线条颜色
            ct.Series[n_series].BorderWidth = 2; //线条粗细
            ct.Series[n_series].MarkerBorderColor = Color.Red; //标记点边框颜色
            ct.Series[n_series].MarkerBorderWidth = 3; //标记点边框大小
            ct.Series[n_series].MarkerColor = Color.YellowGreen; //标记点中心颜色
            ct.Series[n_series].MarkerSize = 5; //标记点大小
            ct.Series[n_series].MarkerStyle = MarkerStyle.Circle; //标记点类型
            ct.Series[n_series].AxisLabel = "#辆";
            return;
        }
        public void Dream_trendbike(IQueryable<orderform>orders,int sel2)
        {
            Trend = Graphics.FromImage(pictureBox6.Image);
            try
            {
                switch (sel2)
                {
                    case 1:
                        foreach (var q in orders)
                        {
                            Trend.FillEllipse(bike_stratBrush, (float)q.start_x - 8, (float)q.start_y - 8, 16, 16);
                            trend_start++;
                        }
                        break;
                    case 2:
                        foreach (var q in orders)
                        {
                            Trend.FillEllipse(bike_endBrush, (float)q.end_x - 8, (float)q.end_y - 8, 16, 16);
                            trend_end++;
                        }
                        break;
                }
                Trend.Flush();
                pictureBox6.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
        private void RepaintMap()
        {
            pictureBox6.Image = shareDemo2.Properties.Resources.map1;
            bikesDisplay();
            pictureBox6.Refresh();
        }
    }


}
