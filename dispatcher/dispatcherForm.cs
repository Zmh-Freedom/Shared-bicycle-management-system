using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sunny.UI;
using shareBike;

namespace dispatcher_Form
{
    public partial class dispatcherForm : UIForm
    {

        #region 控件
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage fix_page;
        private System.Windows.Forms.TabPage diapatch_page;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label l_datetime2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cb_recycle;
        private System.Windows.Forms.CheckBox cb_putin;
        private System.Windows.Forms.CheckBox cb_dispatch;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label l_datetime1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Timer timer1;
        private System.ComponentModel.IContainer components = null;
        #endregion

        #region 数据库相关变量
        public DBDataContext dc;
        private UIDataGridView uiDataGridView1;
        private UIDataGridView uiDataGridView2;
        private UIPanel uiPanel2;
        private UIPanel uiPanel1;
        private DataGridViewTextBoxColumn Columnid;
        private DataGridViewTextBoxColumn Columntag;
        private DataGridViewTextBoxColumn ColumnScource;
        private DataGridViewTextBoxColumn ColumnFlag;
        private DataGridViewTextBoxColumn ColumnEndx;
        private DataGridViewTextBoxColumn Column1Endy;
        private DataGridViewTextBoxColumn ColumnStartTime;
        private DataGridViewTextBoxColumn ColumnEndTIme;
        private DataGridViewTextBoxColumn ColumnBike;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DataGridViewTextBoxColumn ColumnEndy;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        #endregion
        public dispatcherForm(string dispatcher_id)//构造函数
        {
            InitializeComponent();
            this.dispatcher_id = dispatcher_id;//调度员id
            timer1.Interval = 1000;//时间
            timer1.Start();
            
        }

        #region 调度员页面显示代码
        IQueryable<task> tasks1 = null;
        IQueryable<task> tasks2 = null;
        //显示任务
        private void tasksDisplay()
        {
            dc=new DBDataContext();
            uiDataGridView1.AutoGenerateColumns = false;
            uiDataGridView2.AutoGenerateColumns = false;
            
            tasks1 = from task in dc.task
                                      where task.tag==1
                                      &&task.flag!=2
                                      select task;
            
            tasks2 = from task in dc.task
                                      where (task.tag==2 && task.flag != 2 
                                      && cb_dispatch.Checked)
                                      ||(task.tag == 3 && task.flag != 2
                                      && cb_putin.Checked)
                                      || (task.tag == 4 && task.flag != 2
                                      && cb_recycle.Checked)
                                      select task;
            uiDataGridView2.DataSource = tasks2.ToList();
            uiDataGridView1.DataSource= tasks1.ToList();
            setView(uiDataGridView1 ,tasks1);
            setView(uiDataGridView2, tasks2);
            tabControl1.Refresh();

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
        private void setView(UIDataGridView view, IQueryable<task> task)
        {
            int i = 0;
            foreach(var x in task)
            {
                switch(x.tag)
                {
                    case 1:view.Rows[i].Cells[1].Value = "车辆检修";break;
                    case 2: view.Rows[i].Cells[1].Value = "车辆调度"; break;
                    case 3: view.Rows[i].Cells[1].Value = "投放"; break;
                    case 4: view.Rows[i].Cells[1].Value = "回收"; break;
                    
                }
                switch (x.source)
                {
                    case 1: view.Rows[i].Cells[2].Value = "用户"; break;
                    case 2: view.Rows[i].Cells[2].Value = "管理员"; break;
                    case 3: view.Rows[i].Cells[2].Value = "系统自动生成"; break;
                    
                }
                switch (x.flag)
                {
                    case 0: view.Rows[i].Cells[3].Value = "待处理"; break;
                    case 1: view.Rows[i].Cells[3].Value = "处理中"; break;
                    case 2: view.Rows[i].Cells[3].Value = "任务完成"; break;
                }
                view.Rows[i].Cells[4].Value = x.bike.current_x.ToString()+","+x.bike.current_y.ToString();
                view.Rows[i].Cells[5].Value=x.end_x.ToString() + "," + x.end_y.ToString();
                i++;
            }
            view.Update();
        }

        //获取选中的任务的task.id和bike.id
        private ArrayList getSelectedTasksBikeId(UIDataGridView view)
        {
            ArrayList selected_id = new ArrayList();
            for(int i=0; i < view.RowCount; i++)
            {
                if(view.Rows[i].Selected)
                {
                    int[] temp = new int[2];
                    temp[0] = (int)view.Rows[i].Cells[0].Value;
                    temp[1] = (int)view.Rows[i].Cells[8].Value;
                    selected_id.Add(temp);
                }
            }
            return selected_id;
        }

        //时间日期显示
        private void timer1_Tick(object sender, EventArgs e)
        {
            if(this.tabControl1.SelectedTab == fix_page)
                l_datetime1.Text = DateTime.Now.ToString();
            else
                l_datetime2.Text = DateTime.Now.ToString();
        }

        //下面三个筛选条件改变时刷新显示
        private void cb_dispatch_CheckedChanged(object sender, EventArgs e)
        {
            tasksDisplay();
        }
        private void cb_putin_CheckedChanged(object sender, EventArgs e)
        {
            tasksDisplay();
        }
        private void cb_recycle_CheckedChanged(object sender, EventArgs e)
        {
            tasksDisplay();
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            tasksDisplay();
        }
        #endregion

        #region 调度员处理任务代码
        //针对下面三种数据库操作的函数
        /*ArrayList dispatcher_Work(ArrayList tasks_bike_id, int bTimeTo, int bLocTo, int tFlagTo, DateTime tEndtimeTo, string tHandlerTo)
         * 未处理的在参数中的下标   (待处理的..，         检查条件，   bike.flag改成，.time.，坐标， task.flag)
         * 负数表示不用改，坐标0改(0,0)，1改task.loc
         * 
         * */

        //检修：检修完成按钮
        /*若bike.flag!=3: 
         * bike.flag=0, bike.total_time=0
         * task.flag=2
         * else:
         *      不处理，对话框提示正在使用的taskid和对应的bike.id（其他的要处理）
         */
        private void button4_Click(object sender, EventArgs e)
        {
            ArrayList selected_id = getSelectedTasksBikeId(uiDataGridView1);
            if (selected_id.Count == 0)
            {
                MessageBox.Show("请先选择任务！"); return;//未选择数据
            }
            DateTime tEndtimeTo = DateTime.Now;
            ArrayList occupied_id = dispatcherWork(selected_id, 0, 0, -1, 2, tEndtimeTo, dispatcher_id);
            if(occupied_id.Count > 0)
            {
                string mesg = "被占用车辆：";
                foreach(int i in occupied_id)
                {
                    mesg+=((int[])selected_id[i])[1].ToString() + " ";
                }
                MessageBox.Show(mesg);
            }
            tasksDisplay();
        }
        //检修：回收按钮
        /*若bike.flag!=3: 
         * bike.flag=5,  bike.total_time=0, bike.坐标=(0,0)
         * task.flag=2
         * else:
         *      不处理，对话框提示正在使用的taskid和对应的bike.id（其他的要处理）
         */
        private void button3_Click(object sender, EventArgs e)
        {
            ArrayList selected_id = getSelectedTasksBikeId(uiDataGridView1);
            if (selected_id.Count == 0)
            {
                MessageBox.Show("请先选择任务！"); return;//未选择数据
            }
            DateTime tEndtimeTo = DateTime.Now;
            ArrayList occupied_id = dispatcherWork(selected_id, 5, 0, 0, 2, tEndtimeTo, dispatcher_id);
            if (occupied_id.Count > 0)
            {
                string mesg = "被占用车辆：";
                foreach (int i in occupied_id)
                {
                    mesg += ((int[])selected_id[i])[1].ToString() + " ";
                }
                MessageBox.Show(mesg);
            }
            tasksDisplay();
        }
        //调度：开始处理按钮
        /*若bike.flag!=3: 
         * bike.flag=4, 
         * task.flag=1
         * ArrayList processing.Add(task.id)(传入的-传出的)
         * else:
         *      不处理，对话框提示正在使用的taskid和对应的bike.id(其他的要处理）
         */
        private void button1_Click(object sender, EventArgs e)
        {
            ArrayList selected_id = getSelectedTasksBikeId(uiDataGridView2);
            if (selected_id.Count == 0)
            {
                MessageBox.Show("请先选择任务！"); return;//未选择数据
            }
            DateTime tEndtimeTo = DateTime.Now;
            ArrayList occupied_id = dispatcherWork(selected_id, 4, -1, -1, 1, tEndtimeTo, dispatcher_id);
            if (occupied_id.Count > 0)//车辆被占用或任务已结束
            {
                string mesg = "被占用车辆：";
                foreach (int i in occupied_id)
                {
                    mesg += ((int[])selected_id[i])[1].ToString() + " ";
                }
                MessageBox.Show(mesg);
            }
            //添加“我在处理”任务数组
            for(int i = 0; i < selected_id.Count; i++)
            {
                if (!occupied_id.Contains(i))
                {
                    int t_task_id=((int[])selected_id[i])[0];
                    if(!processing.Contains(t_task_id))
                        processing.Add(t_task_id);
                }
            }
            tasksDisplay();
        }
        //调度：处理完成按钮
        /* 若t_task.flag != 1 || t_task.handler != tHandlerTo: 提示有任务不在处理中(task.id)
         * else : if调度和投放：              bike.flag=0, bike.坐标=task.end(x,y), 
         *        if回收：同（检修：回收按钮）bike.flag=5,  bike.total_time=0, bike.坐标=(0,0)
         *       task.flag=2
         */
        private void button2_Click(object sender, EventArgs e)
        {
            ArrayList selected_id = getSelectedTasksBikeId(uiDataGridView2);
            if (selected_id.Count == 0)
            {
                MessageBox.Show("请先选择任务！"); return;//未选择数据
            }
            DateTime tEndtimeTo = DateTime.Now;
            ArrayList occupied_id = dispatchEnd(selected_id, tEndtimeTo, dispatcher_id);
            if (occupied_id.Count > 0)
            {
                string mesg = "任务不在处理中：";
                foreach (int i in occupied_id)
                {
                    mesg += ((int[])selected_id[i])[0].ToString() + " ";
                }
                MessageBox.Show(mesg);
            }
            tasksDisplay();
        }
        #endregion



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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.diapatch_page = new System.Windows.Forms.TabPage();
            this.uiPanel1 = new Sunny.UI.UIPanel();
            this.label5 = new System.Windows.Forms.Label();
            this.cb_recycle = new System.Windows.Forms.CheckBox();
            this.cb_putin = new System.Windows.Forms.CheckBox();
            this.cb_dispatch = new System.Windows.Forms.CheckBox();
            this.l_datetime2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.uiDataGridView2 = new Sunny.UI.UIDataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnEndy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.fix_page = new System.Windows.Forms.TabPage();
            this.uiPanel2 = new Sunny.UI.UIPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.l_datetime1 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.uiDataGridView1 = new Sunny.UI.UIDataGridView();
            this.Columnid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Columntag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnScource = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnFlag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnEndx = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1Endy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnStartTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnEndTIme = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnBike = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tabControl1.SuspendLayout();
            this.diapatch_page.SuspendLayout();
            this.uiPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiDataGridView2)).BeginInit();
            this.fix_page.SuspendLayout();
            this.uiPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiDataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.diapatch_page);
            this.tabControl1.Controls.Add(this.fix_page);
            this.tabControl1.Location = new System.Drawing.Point(0, 35);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1385, 607);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // diapatch_page
            // 
            this.diapatch_page.BackColor = System.Drawing.Color.Snow;
            this.diapatch_page.Controls.Add(this.uiPanel1);
            this.diapatch_page.Controls.Add(this.uiDataGridView2);
            this.diapatch_page.Controls.Add(this.button2);
            this.diapatch_page.Controls.Add(this.button1);
            this.diapatch_page.Location = new System.Drawing.Point(4, 40);
            this.diapatch_page.Name = "diapatch_page";
            this.diapatch_page.Padding = new System.Windows.Forms.Padding(3);
            this.diapatch_page.Size = new System.Drawing.Size(1377, 563);
            this.diapatch_page.TabIndex = 1;
            this.diapatch_page.Text = "调度任务页";
            // 
            // uiPanel1
            // 
            this.uiPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uiPanel1.Controls.Add(this.label5);
            this.uiPanel1.Controls.Add(this.cb_recycle);
            this.uiPanel1.Controls.Add(this.cb_putin);
            this.uiPanel1.Controls.Add(this.cb_dispatch);
            this.uiPanel1.Controls.Add(this.l_datetime2);
            this.uiPanel1.Controls.Add(this.label1);
            this.uiPanel1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiPanel1.Location = new System.Drawing.Point(36, 22);
            this.uiPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiPanel1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiPanel1.Name = "uiPanel1";
            this.uiPanel1.Size = new System.Drawing.Size(1321, 72);
            this.uiPanel1.TabIndex = 20;
            this.uiPanel1.Text = null;
            this.uiPanel1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(20, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(155, 36);
            this.label5.TabIndex = 13;
            this.label5.Text = "任务类型：";
            // 
            // cb_recycle
            // 
            this.cb_recycle.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cb_recycle.AutoSize = true;
            this.cb_recycle.BackColor = System.Drawing.Color.Transparent;
            this.cb_recycle.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cb_recycle.Location = new System.Drawing.Point(583, 28);
            this.cb_recycle.Name = "cb_recycle";
            this.cb_recycle.Size = new System.Drawing.Size(84, 28);
            this.cb_recycle.TabIndex = 7;
            this.cb_recycle.Text = "回收";
            this.cb_recycle.UseVisualStyleBackColor = false;
            this.cb_recycle.CheckedChanged += new System.EventHandler(this.cb_recycle_CheckedChanged);
            // 
            // cb_putin
            // 
            this.cb_putin.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cb_putin.AutoSize = true;
            this.cb_putin.BackColor = System.Drawing.Color.Transparent;
            this.cb_putin.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cb_putin.Location = new System.Drawing.Point(401, 28);
            this.cb_putin.Name = "cb_putin";
            this.cb_putin.Size = new System.Drawing.Size(84, 28);
            this.cb_putin.TabIndex = 8;
            this.cb_putin.Text = "投放";
            this.cb_putin.UseVisualStyleBackColor = false;
            this.cb_putin.CheckedChanged += new System.EventHandler(this.cb_putin_CheckedChanged);
            // 
            // cb_dispatch
            // 
            this.cb_dispatch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cb_dispatch.AutoSize = true;
            this.cb_dispatch.BackColor = System.Drawing.Color.Transparent;
            this.cb_dispatch.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cb_dispatch.Location = new System.Drawing.Point(233, 28);
            this.cb_dispatch.Name = "cb_dispatch";
            this.cb_dispatch.Size = new System.Drawing.Size(84, 28);
            this.cb_dispatch.TabIndex = 9;
            this.cb_dispatch.Text = "调度";
            this.cb_dispatch.UseVisualStyleBackColor = false;
            this.cb_dispatch.CheckedChanged += new System.EventHandler(this.cb_dispatch_CheckedChanged);
            // 
            // l_datetime2
            // 
            this.l_datetime2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.l_datetime2.AutoSize = true;
            this.l_datetime2.BackColor = System.Drawing.Color.Transparent;
            this.l_datetime2.Location = new System.Drawing.Point(1064, 33);
            this.l_datetime2.Name = "l_datetime2";
            this.l_datetime2.Size = new System.Drawing.Size(82, 31);
            this.l_datetime2.TabIndex = 12;
            this.l_datetime2.Text = "label2";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(1051, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(134, 31);
            this.label1.TabIndex = 11;
            this.label1.Text = "时间日期：";
            // 
            // uiDataGridView2
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.uiDataGridView2.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.uiDataGridView2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uiDataGridView2.BackgroundColor = System.Drawing.Color.White;
            this.uiDataGridView2.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.uiDataGridView2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.uiDataGridView2.ColumnHeadersHeight = 32;
            this.uiDataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.uiDataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5,
            this.ColumnEndy,
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewTextBoxColumn7,
            this.dataGridViewTextBoxColumn8});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.uiDataGridView2.DefaultCellStyle = dataGridViewCellStyle3;
            this.uiDataGridView2.EnableHeadersVisualStyles = false;
            this.uiDataGridView2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiDataGridView2.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            this.uiDataGridView2.Location = new System.Drawing.Point(36, 111);
            this.uiDataGridView2.Name = "uiDataGridView2";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.uiDataGridView2.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.uiDataGridView2.RowHeadersWidth = 62;
            this.uiDataGridView2.RowHeight = 30;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiDataGridView2.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.uiDataGridView2.RowTemplate.Height = 30;
            this.uiDataGridView2.SelectedIndex = -1;
            this.uiDataGridView2.ShowGridLine = true;
            this.uiDataGridView2.Size = new System.Drawing.Size(1321, 341);
            this.uiDataGridView2.StripeOddColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.uiDataGridView2.TabIndex = 19;
            this.uiDataGridView2.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.uiDataGridView1_RowsAdded);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "id";
            this.dataGridViewTextBoxColumn1.HeaderText = "ID";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.HeaderText = "任务类型";
            this.dataGridViewTextBoxColumn2.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn3.HeaderText = "任务来源";
            this.dataGridViewTextBoxColumn3.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn4.HeaderText = "任务状态";
            this.dataGridViewTextBoxColumn4.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn5.HeaderText = "单车位置";
            this.dataGridViewTextBoxColumn5.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            // 
            // ColumnEndy
            // 
            this.ColumnEndy.HeaderText = "目标位置";
            this.ColumnEndy.MinimumWidth = 8;
            this.ColumnEndy.Name = "ColumnEndy";
            this.ColumnEndy.Width = 150;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "start_time";
            this.dataGridViewTextBoxColumn6.HeaderText = "创建时间";
            this.dataGridViewTextBoxColumn6.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.Width = 150;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn7.DataPropertyName = "end_time";
            this.dataGridViewTextBoxColumn7.HeaderText = "完成时间";
            this.dataGridViewTextBoxColumn7.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.DataPropertyName = "bid";
            this.dataGridViewTextBoxColumn8.HeaderText = "单车ID";
            this.dataGridViewTextBoxColumn8.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.Width = 150;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Font = new System.Drawing.Font("宋体", 15F);
            this.button2.Location = new System.Drawing.Point(901, 466);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(148, 55);
            this.button2.TabIndex = 17;
            this.button2.Text = "处理完成";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Font = new System.Drawing.Font("宋体", 15F);
            this.button1.Location = new System.Drawing.Point(252, 466);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(148, 55);
            this.button1.TabIndex = 18;
            this.button1.Text = "开始处理";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // fix_page
            // 
            this.fix_page.BackColor = System.Drawing.Color.MintCream;
            this.fix_page.Controls.Add(this.uiPanel2);
            this.fix_page.Controls.Add(this.uiDataGridView1);
            this.fix_page.Controls.Add(this.button3);
            this.fix_page.Controls.Add(this.button4);
            this.fix_page.Location = new System.Drawing.Point(4, 40);
            this.fix_page.Name = "fix_page";
            this.fix_page.Padding = new System.Windows.Forms.Padding(3);
            this.fix_page.Size = new System.Drawing.Size(1377, 563);
            this.fix_page.TabIndex = 0;
            this.fix_page.Text = "检修任务页";
            // 
            // uiPanel2
            // 
            this.uiPanel2.Controls.Add(this.label6);
            this.uiPanel2.Controls.Add(this.l_datetime1);
            this.uiPanel2.Controls.Add(this.label8);
            this.uiPanel2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiPanel2.Location = new System.Drawing.Point(34, 11);
            this.uiPanel2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiPanel2.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiPanel2.Name = "uiPanel2";
            this.uiPanel2.Size = new System.Drawing.Size(1310, 73);
            this.uiPanel2.TabIndex = 18;
            this.uiPanel2.Text = null;
            this.uiPanel2.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(32, 18);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(155, 36);
            this.label6.TabIndex = 11;
            this.label6.Text = "任务信息：";
            // 
            // l_datetime1
            // 
            this.l_datetime1.AutoSize = true;
            this.l_datetime1.BackColor = System.Drawing.Color.Transparent;
            this.l_datetime1.Location = new System.Drawing.Point(893, 22);
            this.l_datetime1.Name = "l_datetime1";
            this.l_datetime1.Size = new System.Drawing.Size(82, 31);
            this.l_datetime1.TabIndex = 16;
            this.l_datetime1.Text = "label2";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Location = new System.Drawing.Point(781, 22);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(134, 31);
            this.label8.TabIndex = 15;
            this.label8.Text = "时间日期：";
            // 
            // uiDataGridView1
            // 
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.uiDataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            this.uiDataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.uiDataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle7.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.uiDataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.uiDataGridView1.ColumnHeadersHeight = 32;
            this.uiDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.uiDataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Columnid,
            this.Columntag,
            this.ColumnScource,
            this.ColumnFlag,
            this.ColumnEndx,
            this.Column1Endy,
            this.ColumnStartTime,
            this.ColumnEndTIme,
            this.ColumnBike});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.uiDataGridView1.DefaultCellStyle = dataGridViewCellStyle8;
            this.uiDataGridView1.EnableHeadersVisualStyles = false;
            this.uiDataGridView1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiDataGridView1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            this.uiDataGridView1.Location = new System.Drawing.Point(34, 96);
            this.uiDataGridView1.Name = "uiDataGridView1";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle9.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.uiDataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.uiDataGridView1.RowHeadersWidth = 62;
            this.uiDataGridView1.RowHeight = 30;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiDataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle10;
            this.uiDataGridView1.RowTemplate.Height = 30;
            this.uiDataGridView1.SelectedIndex = -1;
            this.uiDataGridView1.ShowGridLine = true;
            this.uiDataGridView1.Size = new System.Drawing.Size(1310, 363);
            this.uiDataGridView1.StripeOddColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.uiDataGridView1.TabIndex = 17;
            this.uiDataGridView1.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.uiDataGridView1_RowsAdded);
            // 
            // Columnid
            // 
            this.Columnid.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Columnid.DataPropertyName = "id";
            this.Columnid.HeaderText = "ID";
            this.Columnid.MinimumWidth = 8;
            this.Columnid.Name = "Columnid";
            // 
            // Columntag
            // 
            this.Columntag.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Columntag.HeaderText = "任务类型";
            this.Columntag.MinimumWidth = 8;
            this.Columntag.Name = "Columntag";
            // 
            // ColumnScource
            // 
            this.ColumnScource.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnScource.HeaderText = "任务来源";
            this.ColumnScource.MinimumWidth = 8;
            this.ColumnScource.Name = "ColumnScource";
            // 
            // ColumnFlag
            // 
            this.ColumnFlag.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnFlag.HeaderText = "任务状态";
            this.ColumnFlag.MinimumWidth = 8;
            this.ColumnFlag.Name = "ColumnFlag";
            // 
            // ColumnEndx
            // 
            this.ColumnEndx.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnEndx.HeaderText = "单车位置";
            this.ColumnEndx.MinimumWidth = 8;
            this.ColumnEndx.Name = "ColumnEndx";
            // 
            // Column1Endy
            // 
            this.Column1Endy.HeaderText = "目标位置";
            this.Column1Endy.MinimumWidth = 8;
            this.Column1Endy.Name = "Column1Endy";
            this.Column1Endy.Width = 150;
            // 
            // ColumnStartTime
            // 
            this.ColumnStartTime.DataPropertyName = "start_time";
            this.ColumnStartTime.HeaderText = "创建时间";
            this.ColumnStartTime.MinimumWidth = 8;
            this.ColumnStartTime.Name = "ColumnStartTime";
            this.ColumnStartTime.Width = 150;
            // 
            // ColumnEndTIme
            // 
            this.ColumnEndTIme.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnEndTIme.DataPropertyName = "end_time";
            this.ColumnEndTIme.HeaderText = "完成时间";
            this.ColumnEndTIme.MinimumWidth = 8;
            this.ColumnEndTIme.Name = "ColumnEndTIme";
            // 
            // ColumnBike
            // 
            this.ColumnBike.DataPropertyName = "bid";
            this.ColumnBike.HeaderText = "单车ID";
            this.ColumnBike.MinimumWidth = 8;
            this.ColumnBike.Name = "ColumnBike";
            this.ColumnBike.Width = 150;
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("宋体", 15F);
            this.button3.Location = new System.Drawing.Point(942, 481);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(148, 55);
            this.button3.TabIndex = 13;
            this.button3.Text = "回收";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("宋体", 15F);
            this.button4.Location = new System.Drawing.Point(241, 481);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(148, 55);
            this.button4.TabIndex = 14;
            this.button4.Text = "检修完成";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // dispatcherForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1388, 642);
            this.Controls.Add(this.tabControl1);
            this.Name = "dispatcherForm";
            this.Text = "调度员界面";
            this.tabControl1.ResumeLayout(false);
            this.diapatch_page.ResumeLayout(false);
            this.uiPanel1.ResumeLayout(false);
            this.uiPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiDataGridView2)).EndInit();
            this.fix_page.ResumeLayout(false);
            this.uiPanel2.ResumeLayout(false);
            this.uiPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiDataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion



        #region 调度员的数据库相关操作

        //调度员操作
        /* 未处理的taskid和bike.id在参数中的序号  (待处理的..，         检查条件，   bike.flag改成，.time.，坐标， task.flag, 结束时间, 调度员)
        * 负数表示不用改，坐标0改(0,0)，1改task.loc
        * Noflag==4,要检查任务类型
        * */
        private ArrayList dispatcherWork(ArrayList tasks_bike_id, int bFlagTo, int bTimeTo, int bLocTo, int tFlagTo, DateTime tEndtimeTo, string tHandlerTo)
        {
            int Noflag = 3;
            ArrayList result = new ArrayList();
            try
            {
                //对每条任务
                for (int i = 0; i < tasks_bike_id.Count; i++)
                {
                    //检查Noflag
                    var t_bike = (from p in dc.bike
                                  where p.id == ((int[])(tasks_bike_id[i]))[1]
                                  select p);
                    if (t_bike.First().flag == Noflag)
                    {
                        result.Add(i);
                        continue;
                    }
                    var t_task = (from p in dc.task
                                  where p.id == ((int[])(tasks_bike_id[i]))[0]
                                  select p);
                    if (tFlagTo == 1 && t_task.First().flag == 2)//调度任务开始处理, 已经完成的不再开始
                    {
                        result.Add(i);
                        continue;
                    }
                    //开始处理
                    //bike
                    if (t_task.First().flag == 3)
                        t_bike.First().flag = 4;
                    else
                        t_bike.First().flag = (byte?)bFlagTo;//bike.flag
                    if (bTimeTo >= 0) t_bike.First().total_time = bTimeTo;//bike.total_time
                    if (bLocTo == 0) t_bike.First().current_x = t_bike.First().current_y = 0;//bike位置
                    else if (bLocTo == 1)
                    {
                        t_bike.First().current_x = t_task.First().end_x;
                        t_bike.First().current_y = t_task.First().end_y;
                    }
                    //公共部分
                    //task
                    t_task.First().flag = (byte?)tFlagTo;
                    if (tFlagTo != 1) t_task.First().end_time = tEndtimeTo;//1是处理中
                    t_task.First().handler = tHandlerTo;
                    //提交
                    dc.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }

            return result;
        }

        private ArrayList dispatchEnd(ArrayList tasks_bike_id, DateTime tEndtimeTo, string tHandlerTo)
        {
            ArrayList result = new ArrayList();
            try
            {
                //对每条任务
                for (int i = 0; i < tasks_bike_id.Count; i++)
                {
                    var t_bike = (from p in dc.bike
                                  where p.id == ((int[])(tasks_bike_id[i]))[1]
                                  select p);
                    var t_task = (from p in dc.task//任务必须是处理中且处理人是我
                                  where p.id == ((int[])(tasks_bike_id[i]))[0]
                                  select p);
                    if (t_task.First().flag != 1 || t_task.First().handler != tHandlerTo)
                    {
                        result.Add(i);
                        continue;
                    }
                    //bike
                    if (t_task.First().tag == 2 || t_task.First().tag == 3)//调度、投放
                    {
                        t_bike.First().flag = 0;
                        t_bike.First().current_x = t_task.First().end_x;
                        t_bike.First().current_y = t_task.First().end_y;
                    }
                    else if (t_task.First().tag == 4)//回收
                    {
                        t_bike.First().flag = 5;
                        t_bike.First().total_time = 0;
                        t_bike.First().current_x = t_bike.First().current_y = 0;
                    }
                    //公共部分
                    //task
                    t_task.First().flag = 2;
                    t_task.First().end_time = tEndtimeTo;
                    //提交
                    dc.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            return result;
        }

        #endregion
    }
}
