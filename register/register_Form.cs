using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace login_register_Form1
{
    public partial class register_Form : Form
    {
        public register_Form()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "注册")//注册
            {
                //如果输人内容为空
                if (string.IsNullOrWhiteSpace(tb_name.Text.Trim()) || string.IsNullOrWhiteSpace(tb_password.Text.Trim())
                    || string.IsNullOrWhiteSpace(tb_id.Text.Trim()))
                {
                    MessageBox.Show("请输入完整的信息."); return;
                }
                //用户信息入库，并返回分配的新ID
                string password = tb_password.Text.Trim();//密码
                string name = tb_name.Text.Trim();//昵称
                string id = tb_id.Text.Trim();//id手机号
                bool isSingle = DBmanager.DBmanager.register(id,name, password);
                if (!isSingle)
                {
                    MessageBox.Show("账号重复，请检查后再试."); return;
                }
                button1.Text = "登录"; return;
            }

            //登录
            else if (button1.Text == "登录")
            {
                //跳转到主界面
                login_Form customForm = new login_Form();//待修改:新建主界面(传参用户类型)
                this.Hide();
                customForm.ShowDialog();
                Application.ExitThread();
            }
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
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_name = new System.Windows.Forms.TextBox();
            this.tb_password = new System.Windows.Forms.TextBox();
            this.tb_id = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            this.button1.Location = new System.Drawing.Point(284, 280);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(150, 38);
            this.button1.TabIndex = 0;
            this.button1.Text = "注册";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei UI", 23F);
            this.label1.Location = new System.Drawing.Point(276, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(178, 51);
            this.label1.TabIndex = 1;
            this.label1.Text = "用户注册";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            this.label2.Location = new System.Drawing.Point(170, 176);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 27);
            this.label2.TabIndex = 2;
            this.label2.Text = "设置昵称：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            this.label3.Location = new System.Drawing.Point(170, 219);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 27);
            this.label3.TabIndex = 3;
            this.label3.Text = "设置密码：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            this.label4.Location = new System.Drawing.Point(190, 129);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 27);
            this.label4.TabIndex = 4;
            this.label4.Text = "手机号：";
            // 
            // tb_name
            // 
            this.tb_name.Location = new System.Drawing.Point(336, 177);
            this.tb_name.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_name.Name = "tb_name";
            this.tb_name.Size = new System.Drawing.Size(159, 25);
            this.tb_name.TabIndex = 6;
            // 
            // tb_password
            // 
            this.tb_password.Location = new System.Drawing.Point(336, 222);
            this.tb_password.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_password.Name = "tb_password";
            this.tb_password.Size = new System.Drawing.Size(159, 25);
            this.tb_password.TabIndex = 6;
            // 
            // tb_id
            // 
            this.tb_id.Location = new System.Drawing.Point(336, 129);
            this.tb_id.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_id.Name = "tb_id";
            this.tb_id.Size = new System.Drawing.Size(159, 25);
            this.tb_id.TabIndex = 6;
            // 
            // register_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(711, 338);
            this.Controls.Add(this.tb_id);
            this.Controls.Add(this.tb_password);
            this.Controls.Add(this.tb_name);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "register_Form";
            this.Text = "register_Form";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button button1;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private TextBox tb_name;
        private TextBox tb_password;
        private TextBox tb_id;
    }
}
