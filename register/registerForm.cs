using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using shareBike;
using Sunny.UI;
namespace login_register_Form1
{
    public partial class registerForm : UIForm
    {
        
        #region 控件

        private Button button1;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private TextBox tb_name;
        private TextBox tb_password;
        private TextBox tb_id;
        private Panel panel1;
        private UIAvatar uiAvatar1;
        private System.ComponentModel.IContainer components = null;
        #endregion
        public registerForm()
        {
            InitializeComponent();
            dc=new DBDataContext();
            panel1.BackColor = Color.FromArgb(100, Color.White);
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

        #region 用户注册代码
        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "注册")//注册
            {
                //如果输人内容为空
                if (!(tb_id_hasText && tb_name_hasText && tb_password_hasText))
                {
                    MessageBox.Show("请输入完整的信息."); return;
                }
                //用户信息入库
                string password = tb_password.Text.Trim();//密码
                string name = tb_name.Text.Trim();//昵称
                string id = tb_id.Text.Trim();//id手机号
                //检查格式
                Regex regex = new Regex(@"^(((13[0-9]{1})|(15[0-35-9]{1})|(17[0-9]{1})|(18[0-9]{1}))+\d{8})$");
                if (!regex.IsMatch(id))
                {
                    MessageBox.Show("请输入手机号注册."); return;
                }
                if (password.Length > 10 || name.Length > 10)
                {
                    MessageBox.Show("密码和昵称长度最多10个字符."); return;
                }
                bool isSingle = register(0, id, name, password);//0顾客，1管理员，2调度员
                if (!isSingle)
                {
                    MessageBox.Show("账号重复，请检查后再试."); return;
                }
                //button1.Text = "登录";
                MessageBox.Show("注册成功");
                this.Close();
                return;
            }

            //登录
            else if (button1.Text == "登录")
            {
                //跳转到主界面
                //CustomerForm new_form = new CustomerForm(tb_id.Text);
                this.Close();
                //new_form.ShowDialog();
                //Application.ExitThread();
            }
        }
        #endregion

    

        #region Windows Form Designer generated code
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.uiAvatar1 = new Sunny.UI.UIAvatar();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.White;
            this.button1.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            this.button1.Location = new System.Drawing.Point(195, 389);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(167, 38);
            this.button1.TabIndex = 0;
            this.button1.Text = "注册";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei UI", 23F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(226, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(209, 60);
            this.label1.TabIndex = 1;
            this.label1.Text = "用户注册";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            this.label2.Location = new System.Drawing.Point(70, 247);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(134, 31);
            this.label2.TabIndex = 2;
            this.label2.Text = "设置昵称：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            this.label3.Location = new System.Drawing.Point(70, 304);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(134, 31);
            this.label3.TabIndex = 3;
            this.label3.Text = "设置密码：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            this.label4.Location = new System.Drawing.Point(90, 191);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(110, 31);
            this.label4.TabIndex = 4;
            this.label4.Text = "手机号：";
            // 
            // tb_name
            // 
            this.tb_name.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.tb_name.Location = new System.Drawing.Point(226, 240);
            this.tb_name.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_name.Name = "tb_name";
            this.tb_name.Size = new System.Drawing.Size(279, 39);
            this.tb_name.TabIndex = 6;
            this.tb_name.Text = "10位以内字符数字组合";
            this.tb_name.Enter += new System.EventHandler(this.tb_name_Enter);
            this.tb_name.Leave += new System.EventHandler(this.tb_name_Leave);
            // 
            // tb_password
            // 
            this.tb_password.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.tb_password.Location = new System.Drawing.Point(226, 301);
            this.tb_password.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_password.Name = "tb_password";
            this.tb_password.Size = new System.Drawing.Size(279, 39);
            this.tb_password.TabIndex = 6;
            this.tb_password.Text = "10位以内字符数字组合";
            this.tb_password.Enter += new System.EventHandler(this.tb_password_Enter);
            this.tb_password.Leave += new System.EventHandler(this.tb_password_Leave);
            // 
            // tb_id
            // 
            this.tb_id.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.tb_id.Location = new System.Drawing.Point(226, 184);
            this.tb_id.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_id.Name = "tb_id";
            this.tb_id.Size = new System.Drawing.Size(279, 39);
            this.tb_id.TabIndex = 6;
            this.tb_id.Text = "请输入手机号";
            this.tb_id.Enter += new System.EventHandler(this.tb_id_Enter);
            this.tb_id.Leave += new System.EventHandler(this.tb_id_Leave);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.uiAvatar1);
            this.panel1.Controls.Add(this.tb_id);
            this.panel1.Controls.Add(this.tb_password);
            this.panel1.Controls.Add(this.tb_name);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Location = new System.Drawing.Point(67, 66);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(561, 470);
            this.panel1.TabIndex = 7;
            // 
            // uiAvatar1
            // 
            this.uiAvatar1.BackColor = System.Drawing.Color.Transparent;
            this.uiAvatar1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiAvatar1.Location = new System.Drawing.Point(144, 67);
            this.uiAvatar1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiAvatar1.Name = "uiAvatar1";
            this.uiAvatar1.Size = new System.Drawing.Size(76, 74);
            this.uiAvatar1.TabIndex = 7;
            this.uiAvatar1.Text = "uiAvatar1";
            this.uiAvatar1.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // registerForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackgroundImage = global::shareBike.Properties.Resources.background1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(706, 592);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "registerForm";
            this.Text = "";
            this.ZoomScaleRect = new System.Drawing.Rectangle(22, 22, 738, 466);
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
        //tb_name获得焦点事件触发
        private void tb_name_Enter(object sender, EventArgs e)
        {
            if (tb_name_hasText == false)
                tb_name.Text = "";
            tb_name.ForeColor = Color.Black;
        }
        //tb_name失去焦点事件触发
        private void tb_name_Leave(object sender, EventArgs e)
        {
            if (tb_name.Text == "")
            {
                tb_name.Text = "10位以内字符数字组合";
                tb_name.ForeColor = Color.LightGray;
                tb_name_hasText = false;
            }
            else
                tb_name_hasText = true;
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
        //注册：将新顾客入库(成功返回true，失败返回false)//0顾客，1管理员，2调度员
        private bool register(int user_type, string t_id, string t_name, string t_password)
        {
            try
            {
                if (user_type == 0)
                {
                    //查询id是否存在
                    int idCount = (from p in dc.customer
                                   where t_id == p.id
                                   select p).Count();
                    if (idCount > 0) return false;
                    //生成新记录
                    customer new_customer = new customer() { id = t_id.ToString().PadLeft(5, '0'), password = t_password, nickname = t_name };
                    dc.customer.InsertOnSubmit(new_customer);
                    dc.SubmitChanges();
                }
                else if (user_type == 1)
                {

                    //查询id是否存在
                    int idCount = (from p in dc.manager
                                   where t_id == p.id
                                   select p).Count();
                    if (idCount > 0) return false;
                    //生成新记录
                    manager new_manager = new manager() { id = t_id.ToString().PadLeft(5, '0'), password = t_password, nickname = t_name };
                    dc.manager.InsertOnSubmit(new_manager);
                    dc.SubmitChanges();
                }
                else if (user_type == 2)
                {

                    //查询id是否存在
                    int idCount = (from p in dc.dispatcher
                                   where t_id == p.id
                                   select p).Count();
                    if (idCount > 0) return false;
                    //生成新记录
                    dispatcher new_dispatcher = new dispatcher() { id = t_id.ToString().PadLeft(5, '0'), password = t_password, nickname = t_name };
                    dc.dispatcher.InsertOnSubmit(new_dispatcher);
                    dc.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return true;
        }
        #endregion
    }
}
