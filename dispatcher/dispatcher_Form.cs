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

namespace dispatcher_Form
{
    public partial class dispatcher_Form : Form
    {
        string dispatcher_id;
        //本调度员正在处理的id数组
        private ArrayList processing = new ArrayList();//int
        /*检索时先检索这里，且类型符合的；再检索其他未处理的
        处理时不删掉
        显示的时候默认是选中状态。
        */
        public dispatcher_Form(string dispatcher_id)//构造函数
        {
            InitializeComponent();
            this.dispatcher_id = dispatcher_id;//调度员id
            timer1.Interval = 1000;//时间
            timer1.Start();
            tasks_display();//任务表显示
            cb_dispatch.Checked = true;//默认三种任务都显示
            cb_putin.Checked = true;
            cb_recycle.Checked = true;
        }
        //显示任务
        private void tasks_display()
        {
            if (this.tabControl1.SelectedTab == fix_page)//检修页面
            {
                //清空现有项
                for (int i = 0; i < clb_fix_tasks.Items.Count; i++)
                {
                    clb_fix_tasks.Items.Clear();
                }
                //筛选条件
                int condition = 1000;
                //查询任务表
                ArrayList tasks = DBmanager.DBmanager.getTask(processing, condition);
                //显示（添加项）
                if (tasks != null)
                    foreach (string task in tasks)
                    {
                        clb_fix_tasks.Items.Add(task);
                    }
            }
            else//调度页面
            {
                //清空现有项
                for (int i = 0; i < clb_dispatch_tasks.Items.Count; i++)
                {
                    clb_dispatch_tasks.Items.Clear();
                }
                //检查筛选条件
                int condition = 0;
                if (cb_dispatch.Checked)
                    condition += 100;
                if (cb_putin.Checked)
                    condition += 10;
                if (cb_recycle.Checked)
                    condition += 1;
                //查询任务表
                ArrayList tasks = DBmanager.DBmanager.getTask(processing, condition);
                //显示（添加项）
                if (tasks != null)
                    for(int i=0; i< tasks.Count; ++i)
                    {
                        clb_dispatch_tasks.Items.Add(tasks[i]);
                        if(i<processing.Count)//我在处理的默认选中
                            clb_dispatch_tasks.SetItemCheckState(i, CheckState.Checked);
                    }
            }
                
        }
        
        //获取选中的任务的task.id和bike.id
        private ArrayList get_selected_tasks_bike_id()
        {
            ArrayList selected_id = new ArrayList();
            CheckedListBox clb;//选CheckedListBox
            if (this.tabControl1.SelectedTab == fix_page)//检修页面
                clb = clb_fix_tasks;
            else//调度页面
                clb = clb_dispatch_tasks;
            //记录选中的项
            for (int i = 0; i < clb.Items.Count; i++)
            {
                if (clb.GetItemChecked(i))
                {
                    string str = clb.GetItemText(clb.Items[i]);
                    Match match = Regex.Match(str, @"\d+");
                    int t_id = int.Parse(match.Value);//字符串中第一串数字(task.id)
                    match = match.NextMatch();
                    int b_id = int.Parse(match.Value);//第二串数字（bike.id）
                    int[] t_b_id = new int[2];
                    t_b_id[0] = t_id;
                    t_b_id[1] = b_id;
                    selected_id.Add(t_b_id);
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
            tasks_display();
        }
        private void cb_putin_CheckedChanged(object sender, EventArgs e)
        {
            tasks_display();
        }
        private void cb_recycle_CheckedChanged(object sender, EventArgs e)
        {
            tasks_display();
        }

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
            ArrayList selected_id = get_selected_tasks_bike_id();
            if (selected_id.Count == 0)
            {
                MessageBox.Show("请先选择任务！"); return;//未选择数据
            }
            DateTime tEndtimeTo = DateTime.Now;
            ArrayList occupied_id = DBmanager.DBmanager.dispatcher_Work(selected_id, 0, 0, -1, 2, tEndtimeTo, dispatcher_id);
            if(occupied_id.Count > 0)
            {
                string mesg = "被占用车辆：";
                foreach(int i in occupied_id)
                {
                    mesg+=((int[])selected_id[i])[1].ToString() + " ";
                }
                MessageBox.Show(mesg);
            }
            tasks_display();
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
            ArrayList selected_id = get_selected_tasks_bike_id();
            if (selected_id.Count == 0)
            {
                MessageBox.Show("请先选择任务！"); return;//未选择数据
            }
            DateTime tEndtimeTo = DateTime.Now;
            ArrayList occupied_id = DBmanager.DBmanager.dispatcher_Work(selected_id, 5, 0, 0, 2, tEndtimeTo, dispatcher_id);
            if (occupied_id.Count > 0)
            {
                string mesg = "被占用车辆：";
                foreach (int i in occupied_id)
                {
                    mesg += ((int[])selected_id[i])[1].ToString() + " ";
                }
                MessageBox.Show(mesg);
            }
            tasks_display();
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
            ArrayList selected_id = get_selected_tasks_bike_id();
            if (selected_id.Count == 0)
            {
                MessageBox.Show("请先选择任务！"); return;//未选择数据
            }
            DateTime tEndtimeTo = DateTime.Now;
            ArrayList occupied_id = DBmanager.DBmanager.dispatcher_Work(selected_id, 4, -1, -1, 1, tEndtimeTo, dispatcher_id);
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
            tasks_display();
        }
        //调度：处理完成按钮
        /* 若t_task.flag != 1 || t_task.handler != tHandlerTo: 提示有任务不在处理中(task.id)
         * else : if调度和投放：              bike.flag=0, bike.坐标=task.end(x,y), 
         *        if回收：同（检修：回收按钮）bike.flag=5,  bike.total_time=0, bike.坐标=(0,0)
         *       task.flag=2
         */
        private void button2_Click(object sender, EventArgs e)
        {
            ArrayList selected_id = get_selected_tasks_bike_id();
            if (selected_id.Count == 0)
            {
                MessageBox.Show("请先选择任务！"); return;//未选择数据
            }
            DateTime tEndtimeTo = DateTime.Now;
            ArrayList occupied_id = DBmanager.DBmanager.dispatch_End(selected_id, tEndtimeTo, dispatcher_id);
            if (occupied_id.Count > 0)
            {
                string mesg = "任务不在处理中：";
                foreach (int i in occupied_id)
                {
                    mesg += ((int[])selected_id[i])[0].ToString() + " ";
                }
                MessageBox.Show(mesg);
            }
            tasks_display();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            tasks_display();
        }
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
            this.components = new System.ComponentModel.Container();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.fix_page = new System.Windows.Forms.TabPage();
            this.l_datetime1 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.clb_fix_tasks = new System.Windows.Forms.CheckedListBox();
            this.diapatch_page = new System.Windows.Forms.TabPage();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.clb_dispatch_tasks = new System.Windows.Forms.CheckedListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.l_datetime2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cb_recycle = new System.Windows.Forms.CheckBox();
            this.cb_putin = new System.Windows.Forms.CheckBox();
            this.cb_dispatch = new System.Windows.Forms.CheckBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tabControl1.SuspendLayout();
            this.fix_page.SuspendLayout();
            this.diapatch_page.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.fix_page);
            this.tabControl1.Controls.Add(this.diapatch_page);
            this.tabControl1.Location = new System.Drawing.Point(0, -2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1041, 571);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // fix_page
            // 
            this.fix_page.Controls.Add(this.l_datetime1);
            this.fix_page.Controls.Add(this.label8);
            this.fix_page.Controls.Add(this.button3);
            this.fix_page.Controls.Add(this.button4);
            this.fix_page.Controls.Add(this.label7);
            this.fix_page.Controls.Add(this.label6);
            this.fix_page.Controls.Add(this.clb_fix_tasks);
            this.fix_page.Location = new System.Drawing.Point(4, 25);
            this.fix_page.Name = "fix_page";
            this.fix_page.Padding = new System.Windows.Forms.Padding(3);
            this.fix_page.Size = new System.Drawing.Size(1033, 542);
            this.fix_page.TabIndex = 0;
            this.fix_page.Text = "检修任务页";
            this.fix_page.UseVisualStyleBackColor = true;
            // 
            // l_datetime1
            // 
            this.l_datetime1.AutoSize = true;
            this.l_datetime1.Location = new System.Drawing.Point(809, 40);
            this.l_datetime1.Name = "l_datetime1";
            this.l_datetime1.Size = new System.Drawing.Size(55, 15);
            this.l_datetime1.TabIndex = 16;
            this.l_datetime1.Text = "label2";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(697, 40);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(82, 15);
            this.label8.TabIndex = 15;
            this.label8.Text = "时间日期：";
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("宋体", 15F);
            this.button3.Location = new System.Drawing.Point(598, 450);
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
            this.button4.Location = new System.Drawing.Point(248, 450);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(148, 55);
            this.button4.TabIndex = 14;
            this.button4.Text = "检修完成";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 12F);
            this.label7.Location = new System.Drawing.Point(58, 93);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(869, 20);
            this.label7.TabIndex = 12;
            this.label7.Text = "任务号   类型    状态   来源    单车   起始位置   结束位置      创建时间      完成时间";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 12F);
            this.label6.Location = new System.Drawing.Point(53, 49);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(109, 20);
            this.label6.TabIndex = 11;
            this.label6.Text = "任务信息：";
            // 
            // clb_fix_tasks
            // 
            this.clb_fix_tasks.FormattingEnabled = true;
            this.clb_fix_tasks.Items.AddRange(new object[] {
            "task1",
            "task2",
            "task3"});
            this.clb_fix_tasks.Location = new System.Drawing.Point(37, 116);
            this.clb_fix_tasks.Name = "clb_fix_tasks";
            this.clb_fix_tasks.Size = new System.Drawing.Size(957, 304);
            this.clb_fix_tasks.TabIndex = 10;
            // 
            // diapatch_page
            // 
            this.diapatch_page.Controls.Add(this.button2);
            this.diapatch_page.Controls.Add(this.button1);
            this.diapatch_page.Controls.Add(this.label2);
            this.diapatch_page.Controls.Add(this.clb_dispatch_tasks);
            this.diapatch_page.Controls.Add(this.label5);
            this.diapatch_page.Controls.Add(this.l_datetime2);
            this.diapatch_page.Controls.Add(this.label1);
            this.diapatch_page.Controls.Add(this.cb_recycle);
            this.diapatch_page.Controls.Add(this.cb_putin);
            this.diapatch_page.Controls.Add(this.cb_dispatch);
            this.diapatch_page.Location = new System.Drawing.Point(4, 25);
            this.diapatch_page.Name = "diapatch_page";
            this.diapatch_page.Padding = new System.Windows.Forms.Padding(3);
            this.diapatch_page.Size = new System.Drawing.Size(1033, 542);
            this.diapatch_page.TabIndex = 1;
            this.diapatch_page.Text = "调度任务页";
            this.diapatch_page.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("宋体", 15F);
            this.button2.Location = new System.Drawing.Point(598, 450);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(148, 55);
            this.button2.TabIndex = 17;
            this.button2.Text = "处理完成";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("宋体", 15F);
            this.button1.Location = new System.Drawing.Point(248, 450);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(148, 55);
            this.button1.TabIndex = 18;
            this.button1.Text = "开始处理";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F);
            this.label2.Location = new System.Drawing.Point(58, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(869, 20);
            this.label2.TabIndex = 16;
            this.label2.Text = "任务号   类型    状态   来源    单车   起始位置   结束位置      创建时间      完成时间";
            // 
            // clb_dispatch_tasks
            // 
            this.clb_dispatch_tasks.FormattingEnabled = true;
            this.clb_dispatch_tasks.Items.AddRange(new object[] {
            "task1",
            "task2",
            "task3"});
            this.clb_dispatch_tasks.Location = new System.Drawing.Point(37, 116);
            this.clb_dispatch_tasks.Name = "clb_dispatch_tasks";
            this.clb_dispatch_tasks.Size = new System.Drawing.Size(957, 304);
            this.clb_dispatch_tasks.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 12F);
            this.label5.Location = new System.Drawing.Point(57, 50);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(109, 20);
            this.label5.TabIndex = 13;
            this.label5.Text = "任务类型：";
            // 
            // l_datetime2
            // 
            this.l_datetime2.AutoSize = true;
            this.l_datetime2.Location = new System.Drawing.Point(826, 73);
            this.l_datetime2.Name = "l_datetime2";
            this.l_datetime2.Size = new System.Drawing.Size(55, 15);
            this.l_datetime2.TabIndex = 12;
            this.l_datetime2.Text = "label2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(826, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 15);
            this.label1.TabIndex = 11;
            this.label1.Text = "时间日期：";
            // 
            // cb_recycle
            // 
            this.cb_recycle.AutoSize = true;
            this.cb_recycle.Font = new System.Drawing.Font("宋体", 12F);
            this.cb_recycle.Location = new System.Drawing.Point(619, 50);
            this.cb_recycle.Name = "cb_recycle";
            this.cb_recycle.Size = new System.Drawing.Size(71, 24);
            this.cb_recycle.TabIndex = 7;
            this.cb_recycle.Text = "回收";
            this.cb_recycle.UseVisualStyleBackColor = true;
            this.cb_recycle.CheckedChanged += new System.EventHandler(this.cb_recycle_CheckedChanged);
            // 
            // cb_putin
            // 
            this.cb_putin.AutoSize = true;
            this.cb_putin.Font = new System.Drawing.Font("宋体", 12F);
            this.cb_putin.Location = new System.Drawing.Point(437, 50);
            this.cb_putin.Name = "cb_putin";
            this.cb_putin.Size = new System.Drawing.Size(71, 24);
            this.cb_putin.TabIndex = 8;
            this.cb_putin.Text = "投放";
            this.cb_putin.UseVisualStyleBackColor = true;
            this.cb_putin.CheckedChanged += new System.EventHandler(this.cb_putin_CheckedChanged);
            // 
            // cb_dispatch
            // 
            this.cb_dispatch.AutoSize = true;
            this.cb_dispatch.Font = new System.Drawing.Font("宋体", 12F);
            this.cb_dispatch.Location = new System.Drawing.Point(269, 50);
            this.cb_dispatch.Name = "cb_dispatch";
            this.cb_dispatch.Size = new System.Drawing.Size(71, 24);
            this.cb_dispatch.TabIndex = 9;
            this.cb_dispatch.Text = "调度";
            this.cb_dispatch.UseVisualStyleBackColor = true;
            this.cb_dispatch.CheckedChanged += new System.EventHandler(this.cb_dispatch_CheckedChanged);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // dispatcher_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1040, 562);
            this.Controls.Add(this.tabControl1);
            this.Name = "dispatcher_Form";
            this.Text = "dispatcher_Form1";
            this.tabControl1.ResumeLayout(false);
            this.fix_page.ResumeLayout(false);
            this.fix_page.PerformLayout();
            this.diapatch_page.ResumeLayout(false);
            this.diapatch_page.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage fix_page;
        private System.Windows.Forms.TabPage diapatch_page;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label l_datetime2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cb_recycle;
        private System.Windows.Forms.CheckBox cb_putin;
        private System.Windows.Forms.CheckBox cb_dispatch;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckedListBox clb_fix_tasks;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckedListBox clb_dispatch_tasks;
        private System.Windows.Forms.Label l_datetime1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Timer timer1;
    }
}
