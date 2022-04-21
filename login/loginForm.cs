using shareBike;
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
    public partial class loginForm : UIForm
    {
        #region 控件
        private Button button1;
        private Label label2;
        private Label label3;
        private TextBox tb_id;
        private TextBox tb_password;
        private Label label4;
        private UILabel uiLabel1;
        private UIComboBox uiComboBox1;
        private PictureBox pictureBox2;
        private PictureBox pictureBox1;
        private Panel panel1;
        private Button button2;
        #endregion
        public loginForm()
        {
            InitializeComponent();
            uiComboBox1.SelectedIndex = 0;
            label2.BackColor = Color.Transparent;
            label3.BackColor = Color.Transparent;
            label4.BackColor = Color.Transparent;
            panel1.BackColor = Color.FromArgb(100, Color.White);
            pictureBox1.BackColor = Color.Transparent; 
            pictureBox2.BackColor = Color.Transparent;
            uiLabel1.BackColor = Color.FromArgb(100, Color.White);
            TitleColor = Color.FromArgb(100, Color.White);
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
        #region 登录代码
        //登录
        private void button1_Click(object sender, EventArgs e)
        {
            //如果输人内容为空
            if (!(tb_id_hasText && tb_password_hasText))
            {
                MessageBox.Show("请输入用户名或密码."); return;
            }
            //用户id
            string id = tb_id.Text.Trim();
            //用户类型
            int user_type = uiComboBox1.SelectedIndex;//0顾客，1管理员，2调度员
            //检索数据库获取密码
            string password = getPassword(user_type, id);
            //对比密码
            if (password == tb_password.Text)
            {
                //跳转到主界面
                if(user_type == 0)
                {
                    this.Hide();
                    CustomerForm new_form = new CustomerForm(tb_id.Text);
                    new_form.ShowDialog();
                    this.Show();

                }
                else if (user_type == 1)//管理员
                {
                    this.Hide();
                    managerForm new_form = new managerForm(id);
                    new_form.ShowDialog();
                    this.Show();
                    
                }
                else if(user_type == 2)//调度员
                {
                    this.Hide();
                    dispatcher_Form.dispatcherForm new_form = new dispatcher_Form.dispatcherForm(id);
                    new_form.ShowDialog();
                    this.Show();
                }
            }
            else
            {
                MessageBox.Show("用户名或密码错误"); return;
            }
        }
        #endregion

        # region 跳转到注册页代码
        //注册
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            registerForm register = new registerForm();
            register.ShowDialog();
            this.Show();
            //Application.ExitThread();
        }
        #endregion
        

        #region Windows Form Designer generated code
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
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
            this.button2 = new System.Windows.Forms.Button();
            this.uiLabel1 = new Sunny.UI.UILabel();
            this.uiComboBox1 = new Sunny.UI.UIComboBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.button1.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(413, 561);
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
            this.label2.Font = new System.Drawing.Font("微软雅黑", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(103, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 21);
            this.label2.TabIndex = 2;
            this.label2.Text = "账号";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(103, 175);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 21);
            this.label3.TabIndex = 3;
            this.label3.Text = "密码";
            // 
            // tb_id
            // 
            this.tb_id.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.tb_id.Location = new System.Drawing.Point(103, 105);
            this.tb_id.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_id.Name = "tb_id";
            this.tb_id.Size = new System.Drawing.Size(329, 39);
            this.tb_id.TabIndex = 4;
            this.tb_id.Text = "请输入手机号";
            this.tb_id.Enter += new System.EventHandler(this.tb_id_Enter);
            this.tb_id.Leave += new System.EventHandler(this.tb_id_Leave);
            // 
            // tb_password
            // 
            this.tb_password.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.tb_password.Location = new System.Drawing.Point(103, 198);
            this.tb_password.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_password.Name = "tb_password";
            this.tb_password.Size = new System.Drawing.Size(329, 39);
            this.tb_password.TabIndex = 4;
            this.tb_password.Text = "10位以内字符数字组合";
            this.tb_password.Enter += new System.EventHandler(this.tb_password_Enter);
            this.tb_password.Leave += new System.EventHandler(this.tb_password_Leave);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(67, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(150, 35);
            this.label4.TabIndex = 5;
            this.label4.Text = "用户类型：";
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.White;
            this.button2.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button2.Location = new System.Drawing.Point(173, 561);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(142, 52);
            this.button2.TabIndex = 0;
            this.button2.Text = "注册";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // uiLabel1
            // 
            this.uiLabel1.BackColor = System.Drawing.Color.Transparent;
            this.uiLabel1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.uiLabel1.Font = new System.Drawing.Font("微软雅黑", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel1.Location = new System.Drawing.Point(108, 160);
            this.uiLabel1.Name = "uiLabel1";
            this.uiLabel1.Size = new System.Drawing.Size(518, 70);
            this.uiLabel1.TabIndex = 14;
            this.uiLabel1.Text = "共享单车使用和调度系统";
            this.uiLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.uiLabel1.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // uiComboBox1
            // 
            this.uiComboBox1.DataSource = null;
            this.uiComboBox1.FillColor = System.Drawing.Color.White;
            this.uiComboBox1.FilterMaxCount = 50;
            this.uiComboBox1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiComboBox1.Items.AddRange(new object[] {
            "顾客",
            "管理员",
            "调度员"});
            this.uiComboBox1.Location = new System.Drawing.Point(275, 28);
            this.uiComboBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiComboBox1.MinimumSize = new System.Drawing.Size(63, 0);
            this.uiComboBox1.Name = "uiComboBox1";
            this.uiComboBox1.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.uiComboBox1.RectSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.None;
            this.uiComboBox1.Size = new System.Drawing.Size(142, 35);
            this.uiComboBox1.TabIndex = 13;
            this.uiComboBox1.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiComboBox1.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackgroundImage = global::shareBike.Properties.Resources.password;
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox2.Location = new System.Drawing.Point(53, 189);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(45, 45);
            this.pictureBox2.TabIndex = 12;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImage = global::shareBike.Properties.Resources.user;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(53, 96);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(45, 45);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 11;
            this.pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.uiComboBox1);
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.tb_password);
            this.panel1.Controls.Add(this.tb_id);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(138, 250);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(455, 290);
            this.panel1.TabIndex = 15;
            // 
            // loginForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackgroundImage = global::shareBike.Properties.Resources._1_2;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(722, 739);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.uiLabel1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "loginForm";
            this.RectColor = System.Drawing.Color.Transparent;
            this.Style = Sunny.UI.UIStyle.Custom;
            this.Text = "";
            this.TitleColor = System.Drawing.Color.Transparent;
            this.ZoomScaleRect = new System.Drawing.Rectangle(22, 22, 722, 739);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion


        #region 输入提示逻辑代码
 //tb_id获得焦点事件触发
        private void tb_id_Enter(object sender, EventArgs e)
        {
            if (tb_id_hasText == false)
                tb_id.Text = "";
            tb_id.ForeColor = Color.Black;
        }
        //tb_id失去焦点事件触发
        private void tb_id_Leave(object sender, EventArgs e)
        {
            if (tb_id.Text == "")
            {
                tb_id.Text = "输入手机号登录";
                tb_id.ForeColor = Color.LightGray;
                tb_id_hasText = false;
            }
            else
                tb_id_hasText = true;
        }
        //tb_password获得焦点事件触发
        private void tb_password_Enter(object sender, EventArgs e)
        {
            if (tb_password_hasText == false)
                tb_password.Text = "";
            tb_password.ForeColor = Color.Black;
        }
        //tb_password失去焦点事件触发
        private void tb_password_Leave(object sender, EventArgs e)
        {
            if (tb_password.Text == "")
            {
                tb_password.Text = "10位以内字符数字组合";
                tb_password.ForeColor = Color.LightGray;
                tb_password_hasText = false;
            }
            else
                tb_password_hasText = true;
        }
        #endregion

       
        #region 数据库操作代码
        //查询密码，找不到返回空串
        private string getPassword(int user_type, string user_id)//0顾客，1管理员，2调度员
        {
            try
            {
                DBDataContext dc = new DBDataContext();//创建数据库对象
                switch (user_type)
                {
                    case 0://顾客
                        {
                            IQueryable<string> query = from p in dc.customer
                                                       where p.id == user_id
                                                       select p.password;
                            return query.First();
                        }
                    case 1://管理员
                        {
                            IQueryable<string> query = from p in dc.manager
                                                       where p.id == user_id
                                                       select p.password;
                            return query.First();
                        }
                    case 2://调度员
                        {
                            IQueryable<string> query = from p in dc.dispatcher
                                                       where p.id == user_id
                                                       select p.password;
                            return query.First();
                        }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "";
        }

        #endregion

    }
}
