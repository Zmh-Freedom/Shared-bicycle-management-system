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

namespace login_register_Form1
{
    public partial class login_Form : Form
    {
        public login_Form()
        {
            InitializeComponent();
            lb_identity.SelectedIndex = 0;  //身份默认选顾客
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
                    CustomerForm new_form = new CustomerForm();
                    new_form.ShowDialog();
                    this.Hide();
                    Application.ExitThread();
                }
                else if (user_type == 1)//管理员
                {
                    managerForm new_form = new managerForm();
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_id = new System.Windows.Forms.TextBox();
            this.tb_password = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lb_identity = new System.Windows.Forms.ListBox();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft YaHei UI", 15F);
            this.button1.Location = new System.Drawing.Point(372, 248);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(142, 38);
            this.button1.TabIndex = 0;
            this.button1.Text = "登录";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei UI", 24F);
            this.label1.Location = new System.Drawing.Point(268, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(182, 52);
            this.label1.TabIndex = 1;
            this.label1.Text = "用户登录";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei UI", 13F);
            this.label2.Location = new System.Drawing.Point(185, 151);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 30);
            this.label2.TabIndex = 2;
            this.label2.Text = "账号：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei UI", 13F);
            this.label3.Location = new System.Drawing.Point(185, 190);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 30);
            this.label3.TabIndex = 3;
            this.label3.Text = "密码：";
            // 
            // tb_id
            // 
            this.tb_id.Location = new System.Drawing.Point(300, 153);
            this.tb_id.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_id.Name = "tb_id";
            this.tb_id.Size = new System.Drawing.Size(215, 25);
            this.tb_id.TabIndex = 4;
            // 
            // tb_password
            // 
            this.tb_password.Location = new System.Drawing.Point(300, 194);
            this.tb_password.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_password.Name = "tb_password";
            this.tb_password.Size = new System.Drawing.Size(215, 25);
            this.tb_password.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft YaHei UI", 13F);
            this.label4.Location = new System.Drawing.Point(186, 109);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(123, 30);
            this.label4.TabIndex = 5;
            this.label4.Text = "用户类型：";
            // 
            // lb_identity
            // 
            this.lb_identity.FormattingEnabled = true;
            this.lb_identity.ItemHeight = 15;
            this.lb_identity.Items.AddRange(new object[] {
            "顾客",
            "管理员",
            "调度员"});
            this.lb_identity.Location = new System.Drawing.Point(335, 109);
            this.lb_identity.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lb_identity.Name = "lb_identity";
            this.lb_identity.Size = new System.Drawing.Size(134, 19);
            this.lb_identity.TabIndex = 6;
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft YaHei UI", 15F);
            this.button2.Location = new System.Drawing.Point(185, 248);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(142, 38);
            this.button2.TabIndex = 0;
            this.button2.Text = "注册";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // login_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(711, 338);
            this.Controls.Add(this.lb_identity);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tb_password);
            this.Controls.Add(this.tb_id);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "login_Form";
            this.Text = "login_Form";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button button1;
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox tb_id;
        private TextBox tb_password;
        private Label label4;
        private ListBox lb_identity;
        private Button button2;
    }
}
