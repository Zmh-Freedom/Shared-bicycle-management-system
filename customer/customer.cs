using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Sunny.UI;
namespace shareDemo2
{
    public partial class CustomerForm : UIForm
    {
        public CustomerForm(string ID)
        {
            InitializeComponent();
            #region 数据库初始化
            dc = new DBDataContext();
            user_id = ID;
            foreach(var x in dc.customer)
            {
                if(x.id== user_id)
                {
                    user_nickname= x.nickname;
                    break;
                }
            }
            #endregion

            #region 绘图初始化
            mysbrush1 = new SolidBrush(Color.Blue);
            mysbrush2 = new SolidBrush(Color.FromArgb(50, Color.SteelBlue));
            bikesDisplay();
            pbox = new PictureBox();
            setDisplayNowTag();
            pictureBox1.Controls.Add(pbox);
            #endregion

            #region 其他窗口组件初始化
            label1.Text = user_nickname;
            #endregion
        }
        #region 被调用逻辑函数
        //设置nowLocation图片标志的显示
        private void setDisplayNowTag()
        {
            pbox.Image = Properties.Resources.nowlocation;
            pbox.Size = new Size(30, 40);//设置PictureBox控件大小
            pbox.BackColor = Color.Transparent;//设置PictureBox控件的背景色
            pbox.SizeMode = PictureBoxSizeMode.StretchImage;
            pbox.Location = new Point(300, 300);
        }

        //设置me图片标志的显示
        private void setDisplayMeTag()
        {
            pbox.Image = Properties.Resources.me;
            pbox.Size = new Size(30, 50);//设置PictureBox控件大小

            //freeMove = false;
            bias_x = 15;
            bias_y = 45;
        }

        //设置bike图片标志的显示
        private void setDisplayBikeTag()
        {
            setDisplayMeTag();
            pbox.Image = Properties.Resources.bike;
            pbox.Size = new Size(40, 30);
            bias_x = 15;
            bias_y = 25;
        }

        //重绘地图
        private void RepaintMap()
        {
            pictureBox1.Image = Properties.Resources.map1;
            bikesDisplay();
            pictureBox1.Refresh();
        }

        //显示单车
        private void bikesDisplay()
        {
            try
            {
                g = Graphics.FromImage(pictureBox1.Image);
                IQueryable<bike> qbikes = from p in dc.bike
                                          where p.flag < 3
                                          select p;
                foreach (var q in qbikes)
                {
                    g.FillEllipse(mysbrush1, q.current_x.Value - 10, q.current_y.Value - 10, 20, 20);
                }

                //绘制服务区阴影
                IQueryable<fence> qfences = from p in dc.fence
                                            where p.tag == 1
                                            select p;
                foreach (var q in qfences)
                {
                    g.FillRectangle(mysbrush2, (float)q.origin_x, (float)q.origin_y, (float)q.width, (float)q.height);
                }
                g.Flush();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //检查附近是否有单车
        private bool IsBikeArround(int x, int y)
        {
            x += bias_x;
            y += bias_y;
            try
            {
                int num = (from p in dc.bike
                           where x - 150 < p.current_x && p.current_x < x + 150 && y - 150 < p.current_y && p.current_y < y + 150
                           select p).Count();
                if (num > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            return true;
        }

        //检查是否在潜在投放区域中
        private bool IsPotentialArea(int x, int y)
        {
            try
            {
                IQueryable<fence> area = from p in dc.fence
                                         where p.tag == 3 && x < p.origin_x + 300 && y < p.origin_y + 300
                                         select p;

                if (area.Count() > 0)
                {
                    area.First().score += 1;//如果该潜在投放区域已存在，点数加1
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        //添加潜在投放区域
        private bool AddPotentialArea(int x, int y)
        {
            try
            {
                fence f = new fence();
                f.tag = 3;
                f.origin_x = x - 100;
                f.origin_y = y - 100;
                f.height = 200;
                f.width = 200;
                f.score = 1;
                dc.fence.InsertOnSubmit(f);
                dc.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        //检查是否有异常的订单
        private bool IsAnyUnpaidorIngOrder()
        {
            try
            {
                IQueryable<orderform> ingorder = from p in dc.orderform
                                               where p.cid == user_id && p.flag == 1
                                               select p;
                if (ingorder.Count()>0)
                {
                    MessageBox.Show("您有正在进行的订单，不能再解锁单车");
                    return true;
                }

                IQueryable<orderform> qoders = from p in dc.orderform
                                               where p.cid == user_id && p.flag == 2
                                               select p;
                if (qoders.Count() > 0)
                {
                    double sumcost = 0;
                    foreach (var q in qoders)
                    {
                        sumcost += q.cost.GetValueOrDefault();
                    }
                    if (MessageBox.Show(string.Format("您有未支付的订单共{0}元，支付后才能使用，确定支付吗？", sumcost),
                        "待支付", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        foreach (var p in qoders)
                        {
                            p.flag = 0;
                        }
                        return false;
                    }
                    else
                    {
                        MessageBox.Show("您取消了支付，暂不能使用单车！");
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return true;
            }
        }

        //解锁单车
        private bike UnlockBike(int x, int y)
        {
            try
            {
                IQueryable<bike> qbikes = from p in dc.bike
                                          where x - 10 < p.current_x && p.current_x < x + 10 && y - 10 < p.current_y && p.current_y < y + 10
                                          select p;
                bike now_bike = qbikes.First();//就近返回第一辆单车
                if(now_bike.flag == 2)
                {
                    if(MessageBox.Show("这辆单车可能不安全，请检查后再骑行,确认没问题吗","温馨提示",
                        MessageBoxButtons.YesNo,MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        old_flag = now_bike.flag.Value;
                        now_bike.flag = 3;//标记为正在使用
                        dc.SubmitChanges();
                        return now_bike;
                    }
                    else
                    {
                        return null;
                    }
                }
                old_flag = now_bike.flag.Value;
                now_bike.flag = 3;//标记为正在使用
                dc.SubmitChanges();
                return now_bike;
            }
            catch (Exception ex)
            {
                MessageBox.Show("附近没车");
                return null;
            }
        }

        //创建订单
        private orderform CreateOrder(bike the_bike)
        {
            try
            {
                int num = dc.orderform.Count();
                orderform order = new orderform();
                order.bid = the_bike.id;
                order.cid = user_id;
                order.start_x = now_x;
                order.start_y = now_y;
                //startTime = DateTime.Now;
                startTime = dateTimePicker1.Value;
                order.start_time = startTime;
                order.flag = 1;
                dc.orderform.InsertOnSubmit(order);
                dc.SubmitChanges();
                return order;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        //锁车
        private void LockBike(int feeMore = 0,bool outService = false)
        {
            if (outService)
            {
                now_bike.flag = 1;
            }
            else
            {
                now_bike.flag = 0;
            }
            now_bike.current_x = now_x;
            now_bike.current_y = now_y;

            //int last = (int)(DateTime.Now - startTime).TotalSeconds;
            int last = Convert.ToInt32(textBox1.Text);
            now_bike.total_time += last;

            now_order.end_time = startTime.AddMinutes(last);
            now_order.end_x = now_x;
            now_order.end_y = now_y;

            double cost = 1.5 + feeMore;
            if (last > 30)
            {
                cost += Math.Ceiling((last - 30) / 30.0);
            }
            now_order.cost = cost;
            if (MessageBox.Show(string.Format("骑车费用为{0},确定支付吗", cost),
                "收费界面", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                now_order.flag = 0;
                MessageBox.Show("支付成功");
            }
            else
            {
                now_order.flag = 2;
                MessageBox.Show("您暂时忽略了支付，下次需要先支付该笔订单才能使用单车。");
            }
            dc.SubmitChanges();
        }

        //检查还车区域是否在服务区内
        private bool IsInServiceArea(int x, int y)
        {
            int num = (from p in dc.fence
                       where p.tag == 1 && p.origin_x<x && x< p.origin_x + p.width 
                       && p.origin_y<y && y< p.origin_y + p.height
                       select p).Count();
            if (num > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //返回一个智能调度区内的位置
        private Point GetNewLocation()
        {
            fence q = (from p in dc.fence
                      where p.tag == 2
                      select p).First();
            return new Point((int)q.origin_x, (int)q.origin_y);
        }

        //创建因非法停车导致的调度任务
        private void CreateDispatchTask()
        {
            try
            {
                task t = new task();
                t.tag = 2;
                t.source = 1;
                t.flag = 0;
                t.start_time = DateTime.Now;
                Point NewLocation = GetNewLocation();
                t.end_x = NewLocation.X;
                t.end_y = NewLocation.Y;
                t.bid = now_bike.id;
                dc.task.InsertOnSubmit(t);
                dc.SubmitChanges();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region 交互响应函数
        //移动
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (freeMove)//选择出发点时可以自由移动
            {
                var screenPoint = PointToScreen(pictureBox1.Location);//获取图片panel的相对位置
                now_x = Control.MousePosition.X - screenPoint.X;
                now_y = Control.MousePosition.Y - screenPoint.Y;
                //MessageBox.Show(string.Format("{0} {1}",now_x,now_y));
                pbox.Location = new Point(now_x - bias_x, now_y - bias_y);
            }
            else//步行和骑车时不能瞬间移动超过90个像素的距离
            {
                var screenPoint = PointToScreen(pictureBox1.Location);//获取图片panel的相对位置
                int new_x = Control.MousePosition.X - screenPoint.X;
                int new_y = Control.MousePosition.Y - screenPoint.Y;
                if (Math.Sqrt(Math.Pow(now_x - new_x, 2) +
                   Math.Pow(now_y - new_y, 2)) < 90)
                {
                    now_x = new_x;
                    now_y = new_y;
                    pbox.Location = new Point(now_x - bias_x, now_y - bias_y);
                }
            }
        }

        //响应从这出发按钮
        private void button1_Click(object sender, EventArgs e)
        {
            setDisplayMeTag();
            if (!IsBikeArround(now_x, now_y))
            {
                if (!IsPotentialArea(now_x, now_y))
                {
                    AddPotentialArea(now_x, now_y);
                }
            }
        }

        //响应解锁单车按钮
        private void button2_Click(object sender, EventArgs e)
        {
            if (!IsAnyUnpaidorIngOrder())
            {
                if ((now_bike = UnlockBike(now_x, now_y)) != null)
                {
                    RepaintMap();
                    setDisplayBikeTag();
                    now_order = CreateOrder(now_bike);
                    timer1.Enabled = true;
                    timer2.Enabled = true;
                    old_x = now_x;
                    old_y = now_y;
                }
            }
        }

        //响应锁车按钮
        private void button3_Click(object sender, EventArgs e)
        {
            if (!IsInServiceArea(now_x, now_y))
            {
                if (MessageBox.Show("在服务区外停车收取20元调度费，是否继续？", "还车提示",
               MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    LockBike(20,true);
                    pbox.Image = Properties.Resources.nowlocation;
                    pbox.Size = new Size(30, 40);
                    bias_x = 15;
                    bias_y = 40;
                    pbox.Location = new Point(now_x - bias_x, now_y - bias_y);
                    RepaintMap();
                    //freeMove = true;
                    timer1.Enabled = false;
                    timer2.Enabled = false;
                    label2.ResetText();
                    CreateDispatchTask();
                }
            }
            else
            {
                LockBike();
                pbox.Image = Properties.Resources.nowlocation;
                pbox.Size = new Size(30, 40);
                bias_x = 15;
                bias_y = 40;
                pbox.Location = new Point(now_x - bias_x, now_y - bias_y);
                RepaintMap();
                freeMove = true;
                timer1.Enabled = false;
                timer2.Enabled = false;
                label2.ResetText();
            }
        }

        //响应报修按钮
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                IQueryable<bike> qbikes = from p in dc.bike
                                          where now_x - 10 < p.current_x && p.current_x < now_x + 10 && now_y - 10 < p.current_y && p.current_y < now_y + 10 && p.flag <2
                                          select p;
                bike the_bike = qbikes.First();//就近返回第一辆单车
                the_bike.flag = 1;//标记为待检查
                task t = new task();
                t.tag = 1;
                t.source = 1;
                t.flag = 0;
                t.start_time = DateTime.Now;
                t.bid = the_bike.id;
                dc.task.InsertOnSubmit(t);
                dc.SubmitChanges();
                MessageBox.Show("您已成功报修，工作人员会尽快前来检查！");
            }
            catch (Exception ex)
            {
                MessageBox.Show("附近没车");
            }
        }
        #endregion

        #region 定时更新函数
        //更新骑行时间的标签
        private void timer2_Tick_1(object sender, EventArgs e)
        {
            label2.Text = "已骑行" + ((int)(DateTime.Now - startTime).TotalSeconds).ToString() + "min";
        }

        //检查是否忘记锁车
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (now_x == old_x && now_y == old_y)
            {
                int last = (int)(DateTime.Now - startTime).TotalSeconds;

                now_order.flag = 2;
                now_order.cost = 24;
                now_order.end_x = now_x;
                now_order.end_y = now_y;
                now_order.end_time = startTime.AddMinutes(last);

                now_bike.current_x = now_x;
                now_bike.current_y = now_y;
                now_bike.flag = 0;
                dc.SubmitChanges();
                pbox.Image = Properties.Resources.nowlocation;
                pbox.Size = new Size(30, 40);
                bias_x = 15;
                bias_y = 40;
                pbox.Location = new Point(now_x - bias_x, now_y - bias_y);
                RepaintMap();
                freeMove = true;
                timer1.Enabled = false;
                timer2.Enabled = false;
                label2.ResetText();
            }
            else
            {
                old_x = now_x;
                old_y = now_y;
            }
        }
        #endregion
    }
}
