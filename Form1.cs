using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Demo
{
    public partial class Form1 : Form
    {
        //窗体的构造函数，一些初始化在这里进行
        public Form1()
        {
            InitializeComponent();
            bike_Display();
            this.Load += MainForm_Load;
            this.SizeChanged += MainForm_SizeChanged;
            
        }
        Bike[] _bike = new Bike[100];
        Coordinate coordinate =new  Coordinate();//创建一个坐标全局变量
        int n=0;
        bool Ride=false;//骑行状态
        
        //函数名：从这出发
        //函数功能：用户进入该窗口后，点击地图上的一个点，再点击该按钮，显示红色圆点，确定所选择点为出发点
        //函数流程：捕捉坐标；检查附近是否有单车
        //函数输出：若半径 300m区域内无车，将该地记录到潜在投放区域表（围栏表）
        private void button1_Click(object sender, EventArgs e)
        {
            this.pictureBox1.Click -= new System.EventHandler(this.pictureBox1_Click);
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click2);//换用另一种点击图片事件
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //函数名：解锁单车
        //函数功能：用户位置距离单车10m时，单击按钮实现解锁单车
        //函数流程：检查用户是否有未支付账单
        //函数输出：改变单车状态；记录下解锁点与时刻；更新骑行时间标签；开始计时
        private void button2_Click(object sender, EventArgs e)
        {
            bool flag = false;
            int i = 0;
            foreach (Bike bike in _bike)
            {
                int x1 = (int)bike.bike.current_x, x2 = coordinate.pbox.Left;
                int y1= (int)bike.bike.current_y, y2 = coordinate.pbox.Top;
                int r=(x1-x2)*(x1-x2)+(y1-y2)*(y1-y2);
                if (r < 300)
                {
                    coordinate.bike = bike;
                    coordinate.bike.pbox.Hide();
                    flag = true;
                    break;
                }
                i++;
                if (i >= n) break;
            }

            if(flag)
            {
                coordinate.pbox.Image = Properties.Resources.bike;
                pictureBox1.Controls.Add(coordinate.pbox);
            }
            else
            {
                MessageBox.Show("附近没有单车", "提示", MessageBoxButtons.OKCancel);
            }
            
        }

        //函数名：结束骑行
        //函数流程：点击按钮后检查是否在可还车区域(围栏表)
        //函数输出：非法区域时对话框提示单独收取调度费；弹出支付对话框(支付和忽略按钮)；停止计时；记录还车点与时刻
        //          非法区域强行还车，单车状态标记为待调度，发送调度任务给调度员
        private void button3_Click(object sender, EventArgs e)
        {
            //if (!Ride) return;
            coordinate.pbox.Image = Properties.Resources.zb;
            coordinate.bike.pbox.Left = coordinate.pbox.Left;
            coordinate.bike.pbox.Top = coordinate.pbox.Top;
            coordinate.bike.pbox.Show();
            coordinate.bike.bike.current_x = coordinate.pbox.Left;
            coordinate.bike.bike.current_y = coordinate.pbox.Top;
            this.pictureBox1.Click -= new System.EventHandler(this.pictureBox1_Click2);
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
        }


        //函数名：报修
        //函数输出：点击后改变单车状态为待检查；发送检查任务给调度员
        private void button4_Click(object sender, EventArgs e)
        {

        }

        //函数名：移动
        //函数功能：通过点击当前点附近的区域实现移动；在步行和骑行时均采用该逻辑
        //函数输出：改变当前点的指示标志在地图上的位置；改变XY坐标变量now_x，now_y

        
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //先获取鼠标在窗口上的坐标，在获取此时图片相对于panel的坐标，然后将两个坐标相减即可
            int x = Control.MousePosition.X;
            int y = Control.MousePosition.Y;
            var screenPoint = PointToScreen(pictureBox1.Location);//获取图片panel的相对位置
            x -= screenPoint.X;
            y -= screenPoint.Y;
            //
            Point myPT = new Point(x, y);//获取鼠标单击位置
            coordinate.pbox.Location = myPT;//将坐标标志的picturebox组件赋予坐标值
            pictureBox1.Controls.Add(coordinate.pbox);//添加到pictureBox1中
        }
        private void pictureBox1_Click2(object sender, EventArgs e)
        {
            int x = Control.MousePosition.X;
            int y = Control.MousePosition.Y;
            var screenPoint = PointToScreen(pictureBox1.Location);//获取图片panel的相对位置
            x -= screenPoint.X;
            y -= screenPoint.Y;
            int xx= coordinate.pbox.Location.X;
            int yy= coordinate.pbox.Location.Y;
            double a, b,r;
            a =(x - xx) ;
            b =(y - yy) ;
            r=Math.Sqrt(a*a+ b*b);
            a=a/r;
            b=b/r;
            //进行相对一步一步移动
            for(int i=0; i< r; i+=5)
            {
                coordinate.pbox.BackColor = Color.Transparent;//设置PictureBox控件的背景色
                
                coordinate.pbox.Location=new Point(xx+ (int)(a*i)-15, (int)yy+(int)(b*i)-15);
                pictureBox1.Controls.Add(coordinate.pbox);
                coordinate.pbox.BringToFront();
                Delay(2);//每一移动一次睡眠10ms，体现除动画效果
            }
        }
        //函数名：展示单车
        //函数输入：单车表，访问所有可用状态的单车的坐标
        //函数输出：以蓝色圆点标志显示所有可用单车
        
        private void bike_Display()
        {
            //PictureBox[] Biek_Boxes = new PictureBox[10];
            //读取数据库的单车信息
            DataClasses1DataContext dc = new DataClasses1DataContext();
            IQueryable<bike> bikes = dc.bike;
            foreach(bike b in bikes)
            {//给Bike类型的单车数组赋值
                _bike[n] = new Bike();//初始化
                _bike[n].pbox.Location=new Point((int)b.current_x,(int)b.current_y);//位置赋值
                _bike[n].bike=b;
                _bike[n].id = b.id;//赋值id
                _bike[n]. pbox.Click += new System.EventHandler(this.pictureBox1_Click2);
                pictureBox1.Controls.Add(_bike[n].pbox);
                n++;
            }
        }
        private void _Bike()
        {
            _bike[0] = new Bike();
            _bike[1] = new Bike();
            _bike[2] = new Bike();
        }
        class Bike                          //单车的类
        {
            public PictureBox pbox = new PictureBox();
            public string id { get; set; }
            public bike bike { get; set; }
            public Bike()
            {
                pbox.BackColor = Color.Transparent;//设置PictureBox控件的背景色
                pbox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
                pbox.Size = new System.Drawing.Size(30, 30);//设置PictureBox控件大小
                pbox.Image = Properties.Resources.bike;//设置PictureBox控件要显示的图像
                pbox.Location =new Point(0,0);
                //pbox.Click+= new System.EventHandler(this.pictureBox1_Click2);
            }

        }
        class Coordinate                    //坐标的类
        {
            public PictureBox pbox = new PictureBox();//
            public Bike bike { get; set; }
            public Coordinate()
            {
                pbox.BackColor = Color.Transparent;//设置PictureBox控件的背景色
                pbox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
                pbox.Size = new System.Drawing.Size(30, 30);//设置PictureBox控件大小
                pbox.Image = Properties.Resources.zb;//设置PictureBox控件要显示的图像
                pbox.BringToFront();
                pbox.Location = new Point(0,0);
            }
        }           
        //需要记录的控件位置以及宽度和高度
        Dictionary<string, Rect> normalControl = new Dictionary<string, Rect>();
        private void MainForm_Load(object sender, EventArgs e)
        {
            //记录相关对象以及原始尺寸
            normalWidth = this.panel4.Width;
            normalHeight = this.panel4.Height;
            foreach (Control item in this.panel4.Controls)
            {
                normalControl.Add(item.Name, new Rect(item.Left, item.Top, item.Width, item.Height));

            }
        }
        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            //根据原始比例进行新尺寸计算
            int w = this.panel4.Width;
            int h = this.panel4.Height;
            foreach (Control item in this.panel4.Controls)
            {
                int newX = (int)(w * 1.0 / normalWidth * normalControl[item.Name].x);
                int newY = (int)(h * 1.0 / normalHeight * normalControl[item.Name].y);
                int newWidth = (int)(w * 1.0 / normalWidth * normalControl[item.Name].Width);
                int newHeight = (int)(h * 1.0 / normalHeight * normalControl[item.Name].Height);
                item.Left = newX;
                item.Top = newY;
                item.Width = newWidth;
                item.Height = newHeight;
                //normalControl.Add(item.Name, new Rect(item.Left, item.Top, item.Width, item.Height));

            }
        }
        class Rect
        {
            public Rect(int x, int y, int w, int h)
            {
                this.x = x; this.y = y; this.Width = w; this.Height = h;
            }
            public int x { get; set; }
            public int y { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }

        }
        public static void Delay(int milliSecond)
        {
            int start = Environment.TickCount;
            while (Math.Abs(Environment.TickCount - start) < milliSecond)//毫秒
            {
                Application.DoEvents();//可执行某无聊的操作
            }
        }
    }
}
