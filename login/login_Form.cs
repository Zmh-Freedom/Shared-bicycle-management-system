using shareDemo2;
using shareDemo3;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sunny.UI;
namespace login_register_Form1
{
    public partial class login_Form : UIForm
    {
        public login_Form()
        {
            InitializeComponent();
            lb_identity.SelectedIndex = 0;  //身份默认选顾客
            panel1.BackColor = Color.FromArgb(100, Color.White);
            label2.BackColor = Color.FromArgb(100, Color.White);
            label3.BackColor = Color.FromArgb(100, Color.White);    
            label4.BackColor = Color.FromArgb(100, Color.White);
            uiLabel1.BackColor = Color.FromArgb(100, Color.White);
        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }
        //登录
        private void button1_Click(object sender, EventArgs e)
        {
            //如果输人内容为空
            if (string.IsNullOrWhiteSpace(tb_id.Text.Trim()) || string.IsNullOrWhiteSpace(tb_password.Text.Trim()))
            {
                MessageBox.Show("请输入用户名或密码."); return;
            }
            //用户id
            string id = tb_id.Text.Trim();
            //用户类型
            int user_type = lb_identity.SelectedIndex;//0顾客，1管理员，2调度员
            //检索数据库获取密码
            string password = DBmanager.DBmanager.getPassword(user_type, id);
            //对比密码
            if (password == tb_password.Text)
            {
                //跳转到主界面
                if(user_type == 0)
                {
                    CustomerForm new_form = new CustomerForm(tb_id.Text);
                    new_form.ShowDialog();
                    this.Hide();
                    Application.ExitThread();
                }
                else if (user_type == 1)//管理员
                {
                    managerForm new_form = new managerForm(id);
                    new_form.ShowDialog();
                    this.Hide();
                    Application.ExitThread();
                }
                else if(user_type == 2)//调度员
                {
                    dispatcher_Form.dispatcher_Form new_form = new dispatcher_Form.dispatcher_Form(id);
                    new_form.ShowDialog();
                    this.Hide();
                    Application.ExitThread();
                }
            }
            else
            {
                MessageBox.Show("用户名或密码错误"); return;
            }
        }

        //注册
        private void button2_Click(object sender, EventArgs e)
        {
            register_Form register = new register_Form();
            this.Hide();
            register.ShowDialog();
            Application.ExitThread();
        }

        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private Panel panel1;
        private UILabel uiLabel1;

        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_id = new System.Windows.Forms.TextBox();
            this.tb_password = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lb_identity = new System.Windows.Forms.ListBox();
            this.button2 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.uiLabel1 = new Sunny.UI.UILabel();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Salmon;
            this.button1.Font = new System.Drawing.Font("Microsoft YaHei UI", 15F);
            this.button1.Location = new System.Drawing.Point(341, 515);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(142, 52);
            this.button1.TabIndex = 0;
            this.button1.Text = "登录";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(91, 101);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 27);
            this.label2.TabIndex = 2;
            this.label2.Text = "账号";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(91, 174);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 27);
            this.label3.TabIndex = 3;
            this.label3.Text = "密码";
            // 
            // tb_id
            // 
            this.tb_id.BackColor = System.Drawing.SystemColors.Menu;
            this.tb_id.Location = new System.Drawing.Point(85, 130);
            this.tb_id.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_id.Name = "tb_id";
            this.tb_id.Size = new System.Drawing.Size(312, 39);
            this.tb_id.TabIndex = 4;
            // 
            // tb_password
            // 
            this.tb_password.BackColor = System.Drawing.SystemColors.Menu;
            this.tb_password.Location = new System.Drawing.Point(85, 203);
            this.tb_password.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_password.Name = "tb_password";
            this.tb_password.Size = new System.Drawing.Size(312, 39);
            this.tb_password.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft YaHei UI", 13F);
            this.label4.Location = new System.Drawing.Point(37, 39);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(119, 35);
            this.label4.TabIndex = 5;
            this.label4.Text = "用户类型";
            // 
            // lb_identity
            // 
            this.lb_identity.FormattingEnabled = true;
            this.lb_identity.ItemHeight = 31;
            this.lb_identity.Items.AddRange(new object[] {
            "顾客",
            "管理员",
            "调度员"});
            this.lb_identity.Location = new System.Drawing.Point(230, 39);
            this.lb_identity.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lb_identity.Name = "lb_identity";
            this.lb_identity.Size = new System.Drawing.Size(134, 35);
            this.lb_identity.TabIndex = 6;
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft YaHei UI", 15F);
            this.button2.Location = new System.Drawing.Point(112, 515);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(142, 52);
            this.button2.TabIndex = 0;
            this.button2.Text = "注册";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Controls.Add(this.lb_identity);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.tb_id);
            this.panel1.Controls.Add(this.tb_password);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(93, 191);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(427, 289);
            this.panel1.TabIndex = 9;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackgroundImage = global::shareDemo2.Properties.Resources.password;
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox2.Location = new System.Drawing.Point(43, 203);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(42, 39);
            this.pictureBox2.TabIndex = 8;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::shareDemo2.Properties.Resources.user;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(43, 130);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(42, 39);
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // uiLabel1
            // 
            this.uiLabel1.BackColor = System.Drawing.Color.Transparent;
            this.uiLabel1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.uiLabel1.Font = new System.Drawing.Font("微软雅黑", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel1.Location = new System.Drawing.Point(135, 96);
            this.uiLabel1.Name = "uiLabel1";
            this.uiLabel1.Size = new System.Drawing.Size(348, 70);
            this.uiLabel1.TabIndex = 10;
            this.uiLabel1.Text = "共享单车使用和调度系统";
            this.uiLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // login_Form
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackgroundImage = global::shareDemo2.Properties.Resources._1_2;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(613, 619);
            this.Controls.Add(this.uiLabel1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "login_Form";
            this.Text = "";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Button button1;
        private Label label2;
        private Label label3;
        private TextBox tb_id;
        private TextBox tb_password;
        private Label label4;
        private ListBox lb_identity;
        private Button button2;

    }
}
