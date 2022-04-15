﻿using shareDemo2;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Sunny.UI;
namespace shareDemo3
{
    public partial class managerForm : UIForm
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
            

            Date_Warehouse(dc.bike);
            Bike_date();
        }

        //展示单车
        #region
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
            //RepaintMap();
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
        private UIDataGridView uiDataGridView1;
        private UIPanel uiPanel1;
        private UIButton uiButton1;
        private UIPanel datePanel2;
        private PictureBox pictureBox8;
        private UILabel uiLabel1;
        private UILabel uiLabel2;
        private UIButton uiButton2;
        private UIPanel uiPanel2;
        #endregion

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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.pictureBox7 = new System.Windows.Forms.PictureBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.uiPanel1 = new Sunny.UI.UIPanel();
            this.uiPanel2 = new Sunny.UI.UIPanel();
            this.uiLabel2 = new Sunny.UI.UILabel();
            this.datePanel2 = new Sunny.UI.UIPanel();
            this.uiLabel1 = new Sunny.UI.UILabel();
            this.uiButton2 = new Sunny.UI.UIButton();
            this.pictureBox8 = new System.Windows.Forms.PictureBox();
            this.uiButton1 = new Sunny.UI.UIButton();
            this.uiDataGridView1 = new Sunny.UI.UIDataGridView();
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
            this.tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).BeginInit();
            this.tabPage4.SuspendLayout();
            this.uiPanel1.SuspendLayout();
            this.uiPanel2.SuspendLayout();
            this.datePanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiDataGridView1)).BeginInit();
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
            this.SuspendLayout();
            // 
            // tabPage6
            // 
            this.tabPage6.Location = new System.Drawing.Point(4, 33);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(1424, 891);
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
            this.tabPage5.Size = new System.Drawing.Size(1424, 891);
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
            this.splitContainer3.Size = new System.Drawing.Size(1418, 885);
            this.splitContainer3.SplitterDistance = 992;
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
            this.tabPage4.Controls.Add(this.uiPanel1);
            this.tabPage4.Location = new System.Drawing.Point(4, 33);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(1424, 891);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "仓库数据";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // uiPanel1
            // 
            this.uiPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uiPanel1.Controls.Add(this.uiPanel2);
            this.uiPanel1.Controls.Add(this.datePanel2);
            this.uiPanel1.Controls.Add(this.uiDataGridView1);
            this.uiPanel1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.uiPanel1.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.uiPanel1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiPanel1.Location = new System.Drawing.Point(4, 0);
            this.uiPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiPanel1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiPanel1.Name = "uiPanel1";
            this.uiPanel1.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            this.uiPanel1.Size = new System.Drawing.Size(1424, 891);
            this.uiPanel1.Style = Sunny.UI.UIStyle.LayuiGreen;
            this.uiPanel1.TabIndex = 1;
            this.uiPanel1.Text = "datePanel1";
            this.uiPanel1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiPanel2
            // 
            this.uiPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uiPanel2.Controls.Add(this.uiLabel2);
            this.uiPanel2.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.uiPanel2.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.uiPanel2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiPanel2.Location = new System.Drawing.Point(18, 4);
            this.uiPanel2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiPanel2.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiPanel2.Name = "uiPanel2";
            this.uiPanel2.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            this.uiPanel2.Size = new System.Drawing.Size(1370, 131);
            this.uiPanel2.Style = Sunny.UI.UIStyle.LayuiGreen;
            this.uiPanel2.TabIndex = 5;
            this.uiPanel2.Text = null;
            this.uiPanel2.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiLabel2
            // 
            this.uiLabel2.BackColor = System.Drawing.Color.Transparent;
            this.uiLabel2.Font = new System.Drawing.Font("微软雅黑", 26F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel2.Location = new System.Drawing.Point(86, 18);
            this.uiLabel2.Name = "uiLabel2";
            this.uiLabel2.Size = new System.Drawing.Size(242, 90);
            this.uiLabel2.Style = Sunny.UI.UIStyle.LayuiGreen;
            this.uiLabel2.TabIndex = 3;
            this.uiLabel2.Text = "仓库数据";
            this.uiLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // datePanel2
            // 
            this.datePanel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.datePanel2.Controls.Add(this.uiLabel1);
            this.datePanel2.Controls.Add(this.uiButton2);
            this.datePanel2.Controls.Add(this.pictureBox8);
            this.datePanel2.Controls.Add(this.uiButton1);
            this.datePanel2.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.datePanel2.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.datePanel2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.datePanel2.Location = new System.Drawing.Point(1184, 157);
            this.datePanel2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.datePanel2.MinimumSize = new System.Drawing.Size(1, 1);
            this.datePanel2.Name = "datePanel2";
            this.datePanel2.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            this.datePanel2.Size = new System.Drawing.Size(204, 633);
            this.datePanel2.Style = Sunny.UI.UIStyle.LayuiGreen;
            this.datePanel2.TabIndex = 2;
            this.datePanel2.Text = null;
            this.datePanel2.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiLabel1
            // 
            this.uiLabel1.Font = new System.Drawing.Font("微软雅黑", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel1.Location = new System.Drawing.Point(3, 0);
            this.uiLabel1.Name = "uiLabel1";
            this.uiLabel1.Size = new System.Drawing.Size(72, 64);
            this.uiLabel1.Style = Sunny.UI.UIStyle.LayuiGreen;
            this.uiLabel1.TabIndex = 1;
            this.uiLabel1.Text = "总数";
            this.uiLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiButton2
            // 
            this.uiButton2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton2.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            this.uiButton2.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            this.uiButton2.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(171)))), ((int)(((byte)(160)))));
            this.uiButton2.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(109)))));
            this.uiButton2.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(109)))));
            this.uiButton2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton2.Location = new System.Drawing.Point(26, 322);
            this.uiButton2.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton2.Name = "uiButton2";
            this.uiButton2.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            this.uiButton2.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(171)))), ((int)(((byte)(160)))));
            this.uiButton2.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(109)))));
            this.uiButton2.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(109)))));
            this.uiButton2.Size = new System.Drawing.Size(152, 76);
            this.uiButton2.Style = Sunny.UI.UIStyle.LayuiGreen;
            this.uiButton2.TabIndex = 4;
            this.uiButton2.Text = "删除单车";
            this.uiButton2.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton2.Click += new System.EventHandler(this.uiButton2_Click);
            // 
            // pictureBox8
            // 
            this.pictureBox8.BackgroundImage = global::shareDemo2.Properties.Resources.bike2;
            this.pictureBox8.Location = new System.Drawing.Point(92, 3);
            this.pictureBox8.Name = "pictureBox8";
            this.pictureBox8.Size = new System.Drawing.Size(101, 103);
            this.pictureBox8.TabIndex = 0;
            this.pictureBox8.TabStop = false;
            // 
            // uiButton1
            // 
            this.uiButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            this.uiButton1.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            this.uiButton1.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(171)))), ((int)(((byte)(160)))));
            this.uiButton1.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(109)))));
            this.uiButton1.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(109)))));
            this.uiButton1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton1.Location = new System.Drawing.Point(23, 182);
            this.uiButton1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton1.Name = "uiButton1";
            this.uiButton1.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            this.uiButton1.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(171)))), ((int)(((byte)(160)))));
            this.uiButton1.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(109)))));
            this.uiButton1.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(109)))));
            this.uiButton1.Size = new System.Drawing.Size(155, 76);
            this.uiButton1.Style = Sunny.UI.UIStyle.LayuiGreen;
            this.uiButton1.TabIndex = 1;
            this.uiButton1.Text = "添加单车";
            this.uiButton1.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton1.Click += new System.EventHandler(this.uiButton1_Click);
            // 
            // uiDataGridView1
            // 
            dataGridViewCellStyle11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.uiDataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle11;
            this.uiDataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uiDataGridView1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.uiDataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle12.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.uiDataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle12;
            this.uiDataGridView1.ColumnHeadersHeight = 32;
            this.uiDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle13.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(234)))), ((int)(((byte)(232)))));
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.uiDataGridView1.DefaultCellStyle = dataGridViewCellStyle13;
            this.uiDataGridView1.EnableHeadersVisualStyles = false;
            this.uiDataGridView1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiDataGridView1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(164)))), ((int)(((byte)(152)))));
            this.uiDataGridView1.Location = new System.Drawing.Point(18, 157);
            this.uiDataGridView1.Name = "uiDataGridView1";
            this.uiDataGridView1.ReadOnly = true;
            this.uiDataGridView1.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            dataGridViewCellStyle14.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.uiDataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle14;
            this.uiDataGridView1.RowHeadersWidth = 62;
            this.uiDataGridView1.RowHeight = 30;
            dataGridViewCellStyle15.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle15.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(234)))), ((int)(((byte)(232)))));
            dataGridViewCellStyle15.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiDataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle15;
            this.uiDataGridView1.RowTemplate.Height = 30;
            this.uiDataGridView1.SelectedIndex = -1;
            this.uiDataGridView1.ShowGridLine = true;
            this.uiDataGridView1.Size = new System.Drawing.Size(1096, 633);
            this.uiDataGridView1.StripeOddColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.uiDataGridView1.Style = Sunny.UI.UIStyle.LayuiGreen;
            this.uiDataGridView1.TabIndex = 0;
            this.uiDataGridView1.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.uiDataGridView1_RowsAdded);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.splitContainer2);
            this.tabPage2.Controls.Add(this.statusStrip2);
            this.tabPage2.Location = new System.Drawing.Point(4, 33);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1424, 891);
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
            this.splitContainer2.Panel2.BackColor = System.Drawing.Color.FloralWhite;
            this.splitContainer2.Panel2.Controls.Add(this.label8);
            this.splitContainer2.Panel2.Controls.Add(this.label7);
            this.splitContainer2.Panel2.Controls.Add(this.comboBox2);
            this.splitContainer2.Panel2.Controls.Add(this.comboBox1);
            this.splitContainer2.Panel2.Controls.Add(this.button1);
            this.splitContainer2.Size = new System.Drawing.Size(1418, 854);
            this.splitContainer2.SplitterDistance = 1147;
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
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
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
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
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
            this.statusStrip2.Location = new System.Drawing.Point(3, 857);
            this.statusStrip2.Name = "statusStrip2";
            this.statusStrip2.Size = new System.Drawing.Size(1418, 31);
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
            this.tabPage1.Size = new System.Drawing.Size(1424, 891);
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
            this.splitContainer1.Size = new System.Drawing.Size(1418, 854);
            this.splitContainer1.SplitterDistance = 931;
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
            this.statusStrip1.Location = new System.Drawing.Point(3, 857);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1418, 31);
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
            this.tabControl1.Location = new System.Drawing.Point(-1, 35);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1432, 928);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 33);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1424, 891);
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
            // managerForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1430, 966);
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
            this.tabPage4.ResumeLayout(false);
            this.uiPanel1.ResumeLayout(false);
            this.uiPanel2.ResumeLayout(false);
            this.datePanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiDataGridView1)).EndInit();
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
        #region 趋势图设计
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RepaintMap();Bike_date(); 
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            RepaintMap();Bike_date(); 
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Trendset();
        }
        private void Trendset()
        {
            
            int sel1 = comboBox1.SelectedIndex;
            int sel2 = comboBox2.SelectedIndex;
            //RepaintMap();
            switch (sel2)
            {
                case 1:
                    var order1 = Sel1(sel1);
                    UIForm Trend1 = new shareDemo2.trend(sel1, sel2, order1);
                    Trend1.Show();
                    break;
                case 2:
                    var order2 = Sel2(sel1);
                    UIForm Trend2 = new shareDemo2.trend(sel1, sel2, order2);
                    Trend2.Show();
                    break;
                case 0:
                    var order3 = Sel1(sel1);
                    var order4 = Sel2(sel1);
                    UIForm Trend3 = new shareDemo2.trend(sel1, sel2, order3,order4);
                    Trend3.Show();
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
        private void Bike_date()
        {
            int sel1 = comboBox1.SelectedIndex;
            int sel2 = comboBox2.SelectedIndex;
            IQueryable<orderform> orderform1 = dc.orderform;
            IQueryable<orderform> orderform2 = dc.orderform;
            switch (sel1)
            {
                case 0:
                    orderform1 = from m in dc.orderform
                                 where m.start_time.Value <= DateTime.Now && m.end_time.Value >= DateTime.Now.AddDays(-3)
                                 select m;
                    orderform2 = from m in dc.orderform
                                 where m.end_time.Value <= DateTime.Now && m.end_time.Value >= DateTime.Now.AddDays(-3)
                                 select m; 
                    break;
                case 1:
                    orderform1 = from m in dc.orderform
                                 where m.start_time.Value <= DateTime.Now && m.end_time.Value >= DateTime.Now.AddDays(-7)
                                 select m;
                    orderform2 = from m in dc.orderform
                                 where m.end_time.Value <= DateTime.Now && m.end_time.Value >= DateTime.Now.AddDays(-7)
                                 select m; 
                    break;
                case 2:
                    orderform1 = from m in dc.orderform
                                 where m.start_time.Value <= DateTime.Now && m.end_time.Value >= DateTime.Now.AddMonths(-3)
                                 select m;
                    orderform2 = from m in dc.orderform
                                 where m.end_time.Value <= DateTime.Now && m.end_time.Value >= DateTime.Now.AddMonths(-3)
                                 select m;
                    break;
                case 3:
                    orderform1 = from m in dc.orderform
                                 where m.end_time.Value.Day == DateTime.Now.AddYears(-1).Day
                                 && m.end_time.Value.Year == DateTime.Now.AddMonths(-1).Year
                                 select m;
                    orderform2 = from m in dc.orderform
                                 where m.end_time.Value.Day == DateTime.Now.AddYears(-1).Day
                                 && m.end_time.Value.Year == DateTime.Now.AddMonths(-1).Year
                                 select m; 
                    break;
                case 4:
                    orderform1 = from m in dc.orderform
                                 where m.end_time.Value.Month == DateTime.Now.AddYears(-1).Month
                                 && m.end_time.Value.Year == DateTime.Now.AddMonths(-1).Year
                                 select m;
                    orderform2 = from m in dc.orderform
                                 where m.end_time.Value.Month == DateTime.Now.AddYears(-1).Month
                                 && m.end_time.Value.Year == DateTime.Now.AddMonths(-1).Year
                                 select m;
                    break;
            }
            if (sel2 == 1) Dream_trendbike(orderform1, 0);
                    else if (sel2 == 2)
                        Dream_trendbike(orderform2, 1);
                    else
                    {
                        Dream_trendbike(orderform1, 0);
                        Dream_trendbike(orderform2, 1);
                    }
        }
        private void Dream_trendbike(IQueryable<orderform>orders,int sel2)
        {
            Trend = Graphics.FromImage(pictureBox6.Image);
            try
            {
                switch (sel2)
                {
                    case 0:
                        foreach (var q in orders)
                        {
                            Trend.FillEllipse(bike_stratBrush, (float)q.start_x - 8, (float)q.start_y - 8, 16, 16);
                            //trend_start++;
                        }
                        break;
                    case 1:
                        foreach (var q in orders)
                        {
                            Trend.FillEllipse(bike_endBrush, (float)q.end_x - 8, (float)q.end_y - 8, 16, 16);
                            //trend_end++;
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
        #endregion
        #region 仓库数据设计
        private void Date_Warehouse(IQueryable<bike> Bike)
        {
            
            int n = 0;
            IQueryable<bike> bikes = from x in Bike
                                     where x.flag==5
                                     select x;
            uiDataGridView1.DataSource = bikes;
            uiDataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            
        }
        private void add_bike()
        {
            bike Bike = new bike() { flag=5, total_time=0 };
            dc.bike.InsertOnSubmit(Bike);
            dc.SubmitChanges();
        }

        #endregion

        private void uiDataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            e.Equals(e.RowIndex);
            int a = e.ColumnIndex;
            int b=(int)uiDataGridView1.Rows[a].Cells[0].Value;
        }

        private void uiDataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            var dgv = (DataGridView)sender;
            for (var i = 0; i < e.RowCount; i++)
            {
                dgv.Rows[e.RowIndex + i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Rows[e.RowIndex + i].HeaderCell.Value = (e.RowIndex + i + 1).ToString();
            }

            for (var i = e.RowIndex + e.RowCount; i < dgv.Rows.Count; i++)
            {
                dgv.Rows[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Rows[i].HeaderCell.Value = (i + 1).ToString();
            }
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            add_bike();
            Date_Warehouse(dc.bike);
        }

        private void uiButton2_Click(object sender, EventArgs e)
        {
            int a = uiDataGridView1.SelectedIndex;
            int b = (int)uiDataGridView1.Rows[a].Cells[0].Value;
            IQueryable<bike> bikes = from x in dc.bike
                                     where x.id == b
                                     select x;
            dc.bike.DeleteAllOnSubmit(bikes);
            dc.SubmitChanges();
            Date_Warehouse(dc.bike);
        }
    }


}
