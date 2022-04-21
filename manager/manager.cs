using shareDemo2;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Sunny.UI;
using System.Text.RegularExpressions;
using System.Collections;

namespace shareDemo3
{
    public partial class managerForm : UIForm
    {
        public managerForm(string id)
        {
            InitializeComponent();

            #region 控件自适应窗体大小初始化
            x = this.Width;
            y = this.Height;
            setTag(this);
            #endregion

            #region 数据库初始化
            dc = new DBDataContext();
            order1 = dc.orderform;
            order2 = dc.orderform;
            #endregion

            #region 绘图初始化
            blueBrush = new SolidBrush(Color.Blue);
            silverBrush = new SolidBrush(Color.Silver);
            goldBrush = new SolidBrush(Color.Gold);
            greenBrush = new SolidBrush(Color.Green);
            redBrush = new SolidBrush(Color.LightCoral);
            bike_stratBrush = new SolidBrush(Color.DarkOrange);
            bike_endBrush = new SolidBrush(Color.LightGreen);

            blueShadowBrush = new SolidBrush(Color.FromArgb(50, Color.SteelBlue));
            lightOrangeShadowBrush = new SolidBrush(Color.FromArgb(10, Color.Yellow));
            orangeShadowBrush = new SolidBrush(Color.FromArgb(85, Color.Orange));
            silverShadowBrush = new SolidBrush(Color.FromArgb(85, Color.Silver));
            
            bikesDisplay(pictureBox1.Image);
            UpDateCapturePageLabel();
            bikesDisplay(pictureBox7.Image);
            UpDateStoreLabel();
            AIAreaDisplay();
            PointList = new System.Collections.ArrayList();   
            #endregion

            #region 逻辑代码初始化
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            Sel1(comboBox1.SelectedIndex);
            Sel2(comboBox1.SelectedIndex);
            //Bike_date();
            myid = id;
            #endregion

        }
        #region 地图快照页代码
        //更新快照页标签
        private void UpDateCapturePageLabel()
        {
            label2.Text = "当前可用 " + numAvailable.ToString();
            label3.Text = "正在使用 " + numUsing.ToString();
            label4.Text = "等待检查 " + numToOverhaul.ToString();
            label5.Text = "等待调度 " + numOutServiceArea.ToString();
        }

        //展示单车
        public void bikesDisplay(Image myPaper)
        {
            try
            {
                numAvailable = 0;
                numOutServiceArea = 0;
                numToOverhaul = 0;
                numUsing = 0;

                //绘制单车
                g = Graphics.FromImage(myPaper);
                IQueryable<bike> qbikes = from p in dc.bike
                                          where p.flag < 4
                                          select p;
                foreach (var q in qbikes)
                {
                    if (q.flag == 0)
                    {
                        g.FillEllipse(blueBrush, q.current_x.Value - 8, q.current_y.Value - 8, 16, 16);
                        numAvailable += 1;
                    }
                    else if (q.flag == 1)
                    {
                        g.FillEllipse(silverBrush, q.current_x.Value - 8, q.current_y.Value - 8, 16, 16);
                        numOutServiceArea += 1;
                    }
                    else if (q.flag == 2)
                    {
                        g.FillEllipse(goldBrush, q.current_x.Value - 8, q.current_y.Value - 8, 16, 16);
                        numToOverhaul += 1;
                    }
                    else if (q.flag == 3)
                    {
                        g.FillEllipse(greenBrush, q.current_x.Value - 8, q.current_y.Value - 8, 16, 16);
                        numUsing += 1;
                    }
                }
                IQueryable<task> bikesToHere = from p in dc.task
                                               where (p.tag == 2 || p.tag == 3)
                                               && p.flag < 2
                                               select p;
                foreach (var q in bikesToHere)
                {
                    g.FillEllipse(redBrush, q.end_x.Value - 8, q.end_y.Value - 8, 16, 16);
                }
                //绘制服务区阴影
                IQueryable<fence> qfences = from p in dc.fence
                                            where p.tag == 1 || p.tag == 3
                                            select p;
                foreach (var q in qfences)
                {
                    if (q.tag == 1)
                        g.FillRectangle(blueShadowBrush, (float)q.origin_x, (float)q.origin_y, (float)q.width, (float)q.height);
                    else
                    {
                        if (q.score > 60)
                        {
                            g.DrawRectangle(Pens.Red,
                                            (float)q.origin_x,
                                            (float)q.origin_y,
                                            (float)q.width,
                                            (float)q.height);
                        }
                        else if (q.score > 25)
                        {
                            g.DrawRectangle(Pens.Orange,
                                            (float)q.origin_x,
                                            (float)q.origin_y,
                                            (float)q.width,
                                            (float)q.height);
                        }
                        else
                        {
                            g.DrawRectangle(Pens.Green,
                                            (float)q.origin_x,
                                            (float)q.origin_y,
                                            (float)q.width,
                                            (float)q.height);
                        }
                    }

                }

                g.Flush();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //捕捉鼠标移动
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            toolStripStatusLabel2.Text = (e.X - 5).ToString();
            toolStripStatusLabel4.Text = (e.Y - 4).ToString();
        }

        //利用定时器更新页面时间
        private void timer1_Tick(object sender, EventArgs e)
        {
            label6.Text = DateTime.Now.ToString();
        }

        //响应地图快照页"刷新"按钮
        private void button4_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = shareDemo2.Properties.Resources.map11;
            bikesDisplay(pictureBox1.Image);
            pictureBox1.Refresh();
            UpDateCapturePageLabel();
        }
        #endregion

        #region 用车热图页代码
        //用于框选的绘图事件
        private void PictureBox6_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(System.Drawing.Pens.Blue, myRect);
        }

        //响应鼠标按下
        private void pictureBox6_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            myRect = new Rectangle(e.X, e.Y, 0, 0);
            origin_SelectX = e.X - 5;
            origin_SelectY = e.Y - 4;
        }

        //响应鼠标抬起
        private void pictureBox6_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
            selectWidth = e.X - myRect.Left;
            selectHeight = e.Y - myRect.Top;
        }

        //响应鼠标移动
        private void pictureBox6_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                myRect.Width = e.X - myRect.Left;
                myRect.Height = e.Y - myRect.Top;
            }
            pictureBox6.Refresh();
        }

        //响应"刷新"按钮
        private void button9_Click(object sender, EventArgs e)
        {

        }

        //响应趋势分析按钮
        private void button1_Click(object sender, EventArgs e)
        {
            Trendset(1);
        }
        private void button13_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                MessageBox.Show("不支持近三天的周趋势图查询");
                return;
            }
            Trendset(2);
        }
        #region 趋势图
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            repaintHotPageMap(); Bike_date();
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            repaintHotPageMap(); Bike_date();
        }
        private void Trendset(int tag)
        {

            int sel1 = comboBox1.SelectedIndex;
            int sel2 = comboBox2.SelectedIndex;
            //RepaintMap();
            switch (sel2)
            {
                case 1:
                    var Order1 = Sel1(sel1);
                    trend Trend1 = new shareDemo2.trend(sel1, sel2, Order1, tag);
                    Trend1.Show();
                    break;
                case 2:
                    var Order2 = Sel2(sel1);
                    UIForm Trend2 = new shareDemo2.trend(sel1, sel2, Order2, tag);
                    Trend2.Show();
                    break;
                case 0:
                    var Order3 = Sel1(sel1);
                    var Order4 = Sel2(sel1);
                    UIForm Trend3 = new shareDemo2.trend(sel1, sel2, Order3, Order4, tag);
                    Trend3.Show();
                    break;
            }
        }
        public IQueryable<DateTime> Sel1(int sel)
        {
            IQueryable<DateTime> order1 = null;
            switch (sel)
            {
                case 0:
                    order1 = from m in dc.orderform
                             where m.start_time.Value.Date <= DateTime.Now.Date && m.start_time.Value > DateTime.Now.AddDays(-3).Date
                             && m.start_x >= origin_SelectX && m.start_x <= origin_SelectX + selectWidth
                             && m.start_y >= origin_SelectY && m.start_y <= origin_SelectY + selectHeight
                             select m.start_time.Value; break;
                case 1:
                    order1 = from m in dc.orderform
                             where m.start_time.Value <= DateTime.Now && m.start_time.Value > DateTime.Now.AddDays(-7 * 3)
                             && m.start_x >= origin_SelectX && m.start_x <= origin_SelectX + selectWidth
                             && m.start_y >= origin_SelectY && m.start_y <= origin_SelectY + selectHeight
                             select m.start_time.Value; break;
                case 2:
                    order1 = from m in dc.orderform
                             where m.start_time.Value <= DateTime.Now && m.start_time.Value > DateTime.Now.AddMonths(-3)
                             && m.start_x >= origin_SelectX && m.start_x <= origin_SelectX + selectWidth
                             && m.start_y >= origin_SelectY && m.start_y <= origin_SelectY + selectHeight
                             select m.start_time.Value; break;
                case 3:
                    order1 = from m in dc.orderform
                             where m.start_time.Value.Day == DateTime.Now.AddYears(-1).Day
                             && m.start_time.Value.Year == DateTime.Now.AddYears(-1).Year
                             && m.start_x >= origin_SelectX && m.start_x <= origin_SelectX + selectWidth
                             && m.start_y >= origin_SelectY && m.start_y <= origin_SelectY + selectHeight
                             select m.start_time.Value; break;
                case 4:
                    order1 = from m in dc.orderform
                             where m.start_time.Value.Month == DateTime.Now.AddYears(-1).Month
                             && m.start_time.Value.Year == DateTime.Now.AddYears(-1).Year
                             && m.start_x >= origin_SelectX && m.start_x <= origin_SelectX + selectWidth
                             && m.start_y >= origin_SelectY && m.start_y <= origin_SelectY + selectHeight
                             select m.start_time.Value; break;
            }
            return order1;
        }
        public IQueryable<DateTime> Sel2(int sel)
        {
            IQueryable<DateTime> order2 = null;
            switch (sel)
            {
                case 0:
                    order2 = from m in dc.orderform
                             where m.end_time.Value.Date <= DateTime.Now.Date && m.end_time.Value > DateTime.Now.AddDays(-3).Date
                             && m.end_x >= origin_SelectX && m.end_x <= origin_SelectX + selectWidth
                             && m.end_y >= origin_SelectY && m.end_y <= origin_SelectY + selectHeight
                             select m.end_time.Value; break;
                case 1:
                    order2 = from m in dc.orderform
                             where m.end_time.Value <= DateTime.Now && m.end_time.Value > DateTime.Now.AddDays(-7 * 3)
                             && m.end_x >= origin_SelectX && m.end_x <= origin_SelectX + selectWidth
                             && m.end_y >= origin_SelectY && m.end_y <= origin_SelectY + selectHeight
                             select m.end_time.Value; break;
                case 2:
                    order2 = from m in dc.orderform
                             where m.end_time.Value <= DateTime.Now && m.end_time.Value > DateTime.Now.AddMonths(-3)
                             && m.end_x >= origin_SelectX && m.end_x <= origin_SelectX + selectWidth
                             && m.end_y >= origin_SelectY && m.end_y <= origin_SelectY + selectHeight
                             select m.end_time.Value; break;
                case 3:
                    order2 = from m in dc.orderform
                             where m.end_time.Value.Day == DateTime.Now.AddYears(-1).Day
                             && m.start_time.Value.Month == DateTime.Now.AddYears(-1).Month
                             && m.end_time.Value.Year == DateTime.Now.AddYears(-1).Year
                             && m.end_x >= origin_SelectX && m.end_x <= origin_SelectX + selectWidth
                             && m.end_y >= origin_SelectY && m.end_y <= origin_SelectY + selectHeight
                             select m.end_time.Value; break;
                case 4:
                    order2 = from m in dc.orderform
                             where m.end_time.Value.Month == DateTime.Now.AddYears(-1).Month
                             && m.end_time.Value.Year == DateTime.Now.AddYears(-1).Year
                             && m.end_x >= origin_SelectX && m.end_x <= origin_SelectX + selectWidth
                             && m.end_y >= origin_SelectY && m.end_y <= origin_SelectY + selectHeight
                             select m.end_time.Value; break;
            }
            return order2;
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
                                 where m.start_time.Value <= DateTime.Now && m.start_time.Value > DateTime.Now.AddDays(-3)
                                 select m; break;
                case 1:
                    orderform1 = from m in dc.orderform
                                 where m.start_time.Value <= DateTime.Now && m.start_time.Value > DateTime.Now.AddDays(-7 * 3)
                                 select m; break;
                case 2:
                    orderform1 = from m in dc.orderform
                                 where m.start_time.Value <= DateTime.Now && m.start_time.Value > DateTime.Now.AddMonths(-3)
                                 select m; break;
                case 3:
                    orderform1 = from m in dc.orderform
                                 where m.start_time.Value.Day == DateTime.Now.AddYears(-1).Day
                                 && m.start_time.Value.Month == DateTime.Now.AddYears(-1).Month
                                 && m.start_time.Value.Year == DateTime.Now.AddYears(-1).Year
                                 select m; break;
                case 4:
                    orderform1 = from m in dc.orderform
                                 where m.start_time.Value.Month == DateTime.Now.AddYears(-1).Month
                                 && m.start_time.Value.Year == DateTime.Now.AddYears(-1).Year
                                 select m; break;
            }
            switch (sel1)
            {
                case 0:
                    orderform2 = from m in dc.orderform
                                 where m.end_time.Value <= DateTime.Now && m.end_time.Value > DateTime.Now.AddDays(-3)
                                 select m; break;
                case 1:
                    orderform2 = from m in dc.orderform
                                 where m.end_time.Value <= DateTime.Now && m.end_time.Value > DateTime.Now.AddDays(-7 * 3)
                                 select m; break;
                case 2:
                    orderform2 = from m in dc.orderform
                                 where m.end_time.Value <= DateTime.Now && m.end_time.Value > DateTime.Now.AddMonths(-3)
                                 select m; break;
                case 3:
                    orderform2 = from m in dc.orderform
                                 where m.end_time.Value.Day == DateTime.Now.AddYears(-1).Day
                                 && m.start_time.Value.Month == DateTime.Now.AddYears(-1).Month
                                 && m.end_time.Value.Year == DateTime.Now.AddYears(-1).Year
                                 select m; break;
                case 4:
                    orderform2 = from m in dc.orderform
                                 where m.end_time.Value.Month == DateTime.Now.AddYears(-1).Month
                                 && m.end_time.Value.Year == DateTime.Now.AddYears(-1).Year
                                 select m; break;
            }
            if (sel2 == 1) Draw_trendbike(orderform1, 0);
            else if (sel2 == 2)
                Draw_trendbike(orderform2, 1);
            else
            {
                Draw_trendbike(orderform1, 0);
                Draw_trendbike(orderform2, 1);
            }
        }
        private void Draw_trendbike(IQueryable<orderform> orders, int sel2)
        {
            Trend = Graphics.FromImage(pictureBox6.Image);
            try
            {
                switch (sel2)
                {
                    case 0:
                        foreach (var q in orders)
                        {
                            Trend.FillEllipse(bike_stratBrush, (float)q.start_x, (float)q.start_y - 3, 6, 6);
                            //trend_start++;
                        }
                        break;
                    case 1:
                        foreach (var q in orders)
                        {
                            Trend.FillEllipse(bike_endBrush, (float)q.end_x, (float)q.end_y - 3, 6, 6);
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
        private void repaintHotPageMap()
        {
            pictureBox6.Image = shareDemo2.Properties.Resources.map11;
            pictureBox6.Refresh();
        }
        #endregion

        #endregion

        #region 下达任务页代码
        //响应下达任务页鼠标按下
        private void pictureBox7_MouseDown(object sender, MouseEventArgs e)
        {
            if (comboBox3.SelectedIndex == 0)
            {
                PointList.Add(e.Location);
            }
            else if (comboBox3.SelectedIndex == 1)
            {
                recMouseDown = true;
                recoveryRect = new Rectangle(e.X, e.Y, 0, 0);
                recovery_SelectX = e.X;
                recovery_SelectY = e.Y;
            }
            else if (comboBox3.SelectedIndex == 2)
            {
                if (srcDone)
                {
                    dstMouseDown = true;
                    dstRect = new Rectangle(e.X, e.Y, 0, 0);
                    dst_SelectX = e.X;
                    dst_SelectY = e.Y;
                }
                else
                {
                    srcMouseDown = true;
                    srcRect = new Rectangle(e.X, e.Y, 0, 0);
                    src_SelectX = e.X;
                    src_SelectY = e.Y;
                }
            }
        }

        //更新下达任务页库存标签
        public void UpDateStoreLabel()
        {
            IQueryable<bike> bikesToDown = from p in dc.bike
                                           where p.flag == 5
                                           select p;

            label9.Text = "仓库中单车数: " + bikesToDown.Count();
        }

        //响应下达任务页鼠标抬起
        private void pictureBox7_MouseUp(object sender, MouseEventArgs e)
        {
            if (comboBox3.SelectedIndex == 0)
            {
                pictureBox7.Refresh();
            }
            else if (comboBox3.SelectedIndex == 1)
            {
                recMouseDown = false;
                recoveryWidth = e.X - recoveryRect.Left;
                recoveryHeight = e.Y - recoveryRect.Top;
            }
            else if (comboBox3.SelectedIndex == 2)
            {
                if (srcDone)
                {
                    dstMouseDown = false;
                    dstWidth = e.X - dstRect.Left;
                    dstHeight = e.Y - dstRect.Top;
                    srcDone = false;
                }
                else
                {
                    srcDone = true;
                    srcMouseDown = false;
                    srcWidth = e.X - srcRect.Left;
                    srcHeight = e.Y - srcRect.Top;
                }
            }
        }

        //实现框选的绘图事件
        private void pictureBox7_Paint(object sender, PaintEventArgs e)
        {
            if (comboBox3.SelectedIndex == 0)
            {
                foreach (Point p in PointList)
                {
                    e.Graphics.FillEllipse(redBrush, p.X - 8, p.Y - 8, 16, 16);
                }
            }
            else if (comboBox3.SelectedIndex == 1)
            {
                e.Graphics.DrawRectangle(System.Drawing.Pens.Blue, recoveryRect);
            }
            else if (comboBox3.SelectedIndex == 2)
            {
                e.Graphics.DrawRectangle(System.Drawing.Pens.LightCoral, dstRect);
                e.Graphics.DrawRectangle(System.Drawing.Pens.Blue, srcRect);
            }
        }

        //响应下达任务页鼠标移动事件
        private void pictureBox7_MouseMove(object sender, MouseEventArgs e)
        {
            if (comboBox3.SelectedIndex == 1)
            {
                if (recMouseDown)
                {
                    recoveryRect.Width = e.X - recoveryRect.Left;
                    recoveryRect.Height = e.Y - recoveryRect.Top;
                }
            }
            else if (comboBox3.SelectedIndex == 2)
            {
                if (srcMouseDown)
                {
                    srcRect.Width = e.X - srcRect.Left;
                    srcRect.Height = e.Y - srcRect.Top;
                }
                if (dstMouseDown)
                {
                    dstRect.Width = e.X - dstRect.Left;
                    dstRect.Height = e.Y - dstRect.Top;
                }
            }
            pictureBox7.Refresh();
        }

        //响应下达任务页"确认"按钮
        private void button5_Click(object sender, EventArgs e)
        {
            if (comboBox3.SelectedIndex == 0)
            {
                try
                {
                    List<bike> bikesToDown = (from p in dc.bike
                                              where p.flag == 5
                                              select p).ToList();
                    int i = 0;

                    foreach (Point theLocation in PointList)
                    {
                        bike theToDown = bikesToDown.ElementAt(i);
                        theToDown.flag = 4;
                        i++;
                        task t = new task();
                        t.end_x = theLocation.X;
                        t.end_y = theLocation.Y;
                        t.source = 2;
                        t.flag = 0;
                        t.start_time = dateTimePicker1.Value;
                        t.tag = 3;
                        t.bid = theToDown.id;
                        dc.task.InsertOnSubmit(t);
                    }
                    dc.SubmitChanges();
                    PointList.Clear();
                    MessageBox.Show("您已成功下达投放任务，调度员会尽快完成！");
                    UpDateStoreLabel();
                    repaintTaskDonwnMap();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("仓库单车已全部投放 " + ex.Message);
                    UpDateStoreLabel();
                    repaintTaskDonwnMap();
                }
            }
            else if (comboBox3.SelectedIndex == 1)
            {
                try
                {
                    IQueryable<bike> bikesToRecovery = from p in dc.bike
                                                       where (p.flag == 0 || p.flag == 2)
                                                       && recovery_SelectX < p.current_x && p.current_x < recovery_SelectX + recoveryWidth
                                                       && recovery_SelectY < p.current_y && p.current_y < recovery_SelectY + recoveryHeight
                                                       select p;
                    foreach (var q in bikesToRecovery)
                    {
                        q.flag = 1;
                        task t = new task();
                        t.tag = 4;
                        t.source = 2;
                        t.flag = 0;
                        t.start_time = dateTimePicker1.Value;
                        t.bid = q.id;
                        dc.task.InsertOnSubmit(t);
                    }
                    dc.SubmitChanges();
                    MessageBox.Show("您已成功下达回收任务，调度员会尽快完成！");
                    repaintTaskDonwnMap();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("附近没车可回收 " + ex.Message);
                }
            }
            else if (comboBox3.SelectedIndex == 2)
            {
                try
                {
                    IQueryable<bike> bikesToMove = from p in dc.bike
                                                   where (p.flag == 0 || p.flag == 2)
                                                   && src_SelectX < p.current_x && p.current_x < src_SelectX + srcWidth
                                                   && src_SelectY < p.current_y && p.current_y < src_SelectY + srcHeight
                                                   select p;
                    int bikesAmount = bikesToMove.Count();
                    char DownPolicy;
                    int i = 0;
                    if (dstWidth >= 2 * dstHeight)
                    {
                        DownPolicy = 'H';
                    }
                    else
                    {
                        DownPolicy = 'I';
                    }

                    foreach (var q in bikesToMove)
                    {
                        q.flag = 1;
                        task t = new task();
                        t.bid = q.id;
                        t.tag = 2;
                        t.source = 2;
                        t.flag = 0;
                        t.start_time = dateTimePicker1.Value;
                        if (DownPolicy == 'H')
                        {
                            t.end_y = dst_SelectY + 13;
                            t.end_x = (int?)(dst_SelectX + i * 11 + 13);
                            i++;
                        }
                        else
                        {
                            t.end_x = dst_SelectX + 13;
                            t.end_y = (int?)(dst_SelectY + i * 11 + 13);
                            i++;
                        }
                        dc.task.InsertOnSubmit(t);
                    }
                    dc.SubmitChanges();
                    MessageBox.Show("您已成功下达调度任务，调度员会尽快完成！");
                    repaintTaskDonwnMap();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("附近没有单车可供调度 " + ex.Message);
                }
            }
        }

        //重绘下达任务页地图
        public void repaintTaskDonwnMap()
        {
            pictureBox7.Image = shareDemo2.Properties.Resources.map11;
            bikesDisplay(pictureBox7.Image);
            pictureBox7.Refresh();
        }

        //响应下达任务页"任务管理"按钮
        private void button6_Click(object sender, EventArgs e)
        {
            taskManage taskManageForm = new taskManage(this);
            taskManageForm.ShowDialog();
        }
        //生成"仓库管理"界面
        private void button7_Click(object sender, EventArgs e)
        {
            Warehouse WareUI = new Warehouse(this);
            WareUI.ShowDialog();
        }
        #endregion

        #region 智能调度页代码
        //绘制支持智能调度的阴影区域
        private void AIAreaDisplay()
        {
            pictureBox9.Image = shareDemo2.Properties.Resources.map11;
            g = Graphics.FromImage(pictureBox9.Image);
            try
            {
                IQueryable<fence> qfences = from p in dc.fence
                                            where p.tag == 2
                                            select p;
                foreach (var q in qfences)
                {
                    if (q.score > 0)
                    {
                        g.FillRectangle(orangeShadowBrush, new Rectangle((int)q.origin_x, (int)q.origin_y, (int)q.width, (int)q.height));

                    }
                    else
                    {
                        g.FillRectangle(silverShadowBrush, new Rectangle((int)q.origin_x, (int)q.origin_y, (int)q.width, (int)q.height));
                    }
                }
                g.Flush();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //响应智能调度页"增加区域"按钮
        private void button3_Click(object sender, EventArgs e)
        {
            if (AddAIAreaButtonDown)
            {
                try
                {
                    if (aiHeight < 3 || aiWidth < 3)
                    {
                        MessageBox.Show("所选区域过小，无法建立智能调度区域！");
                        return;
                    }
                    fence f = new fence();
                    f.origin_x = ai_SelectX;
                    f.origin_y = ai_SelectY;
                    f.height = aiHeight;
                    f.width = aiWidth;
                    f.tag = 2;
                    f.score = 100;
                    dc.fence.InsertOnSubmit(f);
                    dc.SubmitChanges();
                    //新建智能调度区视图
                    createView(ai_SelectX, ai_SelectY, aiWidth, aiHeight, "weekday", 1, 3 * 7);//7天策略
                    createView(ai_SelectX, ai_SelectY, aiWidth, aiHeight, "hour", 1, 7 * 24);//24小时策略
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                AIAreaDisplay();
                pictureBox9.Refresh();
                MessageBox.Show("成功增加1个支持智能调度的区域");
                button3.Text = "增加区域";
                AddAIAreaButtonDown = false;
            }
            else
            {
                MessageBox.Show("框选区域后点击确定，以增加支持智能调度的区域");
                button3.Text = "确定";
                AddAIAreaButtonDown = true;
            }
        }

        //响应鼠标按下事件
        private void pictureBox9_MouseDown(object sender, MouseEventArgs e)
        {
            if (AddAIAreaButtonDown)
            {
                DrawAIAreaMouseDown = true;
                aiRect = new Rectangle(e.X, e.Y, 0, 0);
                ai_SelectX = e.X;
                ai_SelectY = e.Y;
            }
        }

        //响应点击区域事件
        private void pictureBox9_MouseClick(object sender, MouseEventArgs e)
        {
            IQueryable<fence> AIareas = from p in dc.fence
                                        where p.tag == 2
                                        && p.origin_x < e.X && e.X < p.origin_x + p.width
                                        && p.origin_y < e.Y && e.Y < p.origin_y + p.height
                                        select p;
            if (AIareas.Count() > 0)
            {
                fence f = AIareas.First();
                tempAIarea = f;
                aiRect = new Rectangle((int)f.origin_x, (int)f.origin_y, (int)f.width, (int)f.height);
                getSelectedArea = true;
            }
        }

        //响应鼠标移动事件
        private void pictureBox9_MouseMove(object sender, MouseEventArgs e)
        {
            if (AddAIAreaButtonDown && DrawAIAreaMouseDown)
            {
                aiRect.Width = e.X - aiRect.Left;
                aiRect.Height = e.Y - aiRect.Top;
            }
            pictureBox9.Refresh();
        }

        //响应鼠标抬起事件
        private void pictureBox9_MouseUp(object sender, MouseEventArgs e)
        {
            if (AddAIAreaButtonDown)
            {
                DrawAIAreaMouseDown = false;
                aiWidth = e.X - aiRect.Left;
                aiHeight = e.Y - aiRect.Top;
            }
        }

        //响应框选的绘图事件
        private void pictureBox9_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(System.Drawing.Pens.Orange, aiRect);
        }

        //响应智能调度页"开启/关闭"按钮
        private void button2_Click(object sender, EventArgs e)
        {
            if (getSelectedArea)
            {
                if (tempAIarea.score > 0)
                {
                    tempAIarea.score = 0;
                    dc.SubmitChanges();
                    MessageBox.Show("您已成功关闭该区域的智能调度功能！");
                }
                else
                {
                    tempAIarea.score = 100;
                    dc.SubmitChanges();
                    MessageBox.Show("您已成功开启该区域的智能调度功能！");
                }
                AIAreaDisplay();
                pictureBox9.Refresh();
            }
            else
            {
                MessageBox.Show("您未选中任何区域！");
            }
        }

        //响应智能调度页"删除区域"按钮
        private void button8_Click(object sender, EventArgs e)
        {
            if (getSelectedArea)
            {
                try
                {
                    dropView(tempAIarea.id, "weekday", 1);//删除7天策略视图
                    dropView(tempAIarea.id, "hour", 1);//删除24小时策略视图
                    dc.fence.DeleteOnSubmit(tempAIarea);
                    dc.SubmitChanges();
                    AIAreaDisplay();
                    pictureBox9.Refresh();
                    MessageBox.Show("您成功删除了该智能调度区域！");
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private Button button9;
        private Button button10;
        private Button button11;
        private Button button12;
        private TextBox tb_name;
        private TextBox tb_password;
        private TextBox tb_id;
        private Label label15;
        private Label label14;
        private Label label16;
        private Label label17;
        private Label label18;
        private Label label19;
        private CheckedListBox clb_user;
        private Label label20;
        private Button button13;
        private Label label21;
        private Timer timer2;
        private UILabel uiLabel2;
        private PictureBox pictureBox11;
        private UILabel uiLabel1;
        private PictureBox pictureBox10;
        private UILabel uiLabel3;
        private UIPanel uiPanel1;
        private SaveFileDialog saveFileDialog1;
        private ComboBox cb_user_type;

        //创建智能调度时段-需求视图
        //由于参数设置为getdate（），会自动更新
        /*参数说明（地理范围（x,y,宽，高）；时间间隔（类型，长度)；时间长度）
        *   
        *   时间间隔类型interval_type：minute，hour，day，month，year
        *   时间长度time_length = n：即n个interval_type
        */
        /*使用时机：
         *      创建视图由围栏增、开的button调用；
         *      关、删时drop视图
         * 视图名："view"_fenceid_时间间隔类型_长度
        */

        public void createView(int x, int y, int width, int height, string interval_type, int interval_length, int time_length)
        {
            try
            {
                string command = "";
                //生成sql命令

                string time0;
                time0 = "DATEADD(" + interval_type + ",-" + time_length.ToString() + ",GETDATE())";

                string view_name = "";
                //查fence_id
                var fence_id = from p in dc.fence
                               where p.origin_x == x && p.origin_y == y && p.width == width && p.height == height
                               select p.id;
                view_name = "view_" + fence_id.First().ToString() + "_" + interval_type + "_" + interval_length.ToString();

                string startORend = "start";

                string cycle;
                if (interval_type == "year")
                    cycle = "10000000";
                else if (interval_type == "month")
                    cycle = "12";
                else if (interval_type == "weekday")
                    cycle = "7";
                else if (interval_type == "day")
                    cycle = "30";
                else if (interval_type == "hour")
                    cycle = "24";
                else if (interval_type == "minute")
                    cycle = "60";
                else
                    cycle = "60";

                Console.WriteLine(cycle);

                string period = "(DATEPART(" + interval_type + "," + startORend + "_time)-DATEPART(" + interval_type + "," + time0 + ")+" + cycle + ")%" + cycle + "/" + interval_length.ToString();

                command = "create view " + view_name
                    + "(period,need)as select  " + period + ",count(*) from shareBike.dbo.orderform where "
                    + startORend + "_x between " + x.ToString() + " and " + (x + width).ToString() + " and "
                    + startORend + "_y between " + y.ToString() + " and " + (y + height).ToString() + " and "
                    + startORend + "_time between " + time0 + " and GETDATE()"
                    + " group by " + period;
                //若存在，先删除
                dc.ExecuteCommand("drop view if exists " + view_name);
                //创建视图
                dc.ExecuteCommand(command);
                // Console.WriteLine(command);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //删除智能调度视图 interval_type小写英文
        private void dropView(int fence_id, string interval_type, int interval_length)
        {
            try
            {
                string command = "drop view if exists view_" + fence_id.ToString() + "_" + interval_type + "_" + interval_length.ToString();
                dc.ExecuteCommand(command);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //显示累计生成任务量
        private void timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                IQueryable<task> taskFromAI = from p in dc.task
                                              where p.source == 3
                                              select p;
                label21.Text = "累计生成任务 " + taskFromAI.Count().ToString() + "条";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region 人事管理页代码
        //插入数据按钮
        private void button12_Click(object sender, EventArgs e)
        {
            //如果输人内容为空
            if (string.IsNullOrWhiteSpace(tb_name.Text.Trim()) || string.IsNullOrWhiteSpace(tb_password.Text.Trim())
                || string.IsNullOrWhiteSpace(tb_id.Text.Trim()))
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
            bool isSingle = register(cb_user_type.SelectedIndex, id, name, password);//0顾客，1管理员，2调度员
            if (!isSingle)
            {
                MessageBox.Show("账号重复，请检查后再试.");
            }
            //查询并显示
            ArrayList array = searchUser(cb_user_type.SelectedIndex, "", "", "");
            userDisplay(array);
        }
        //查询数据按钮
        private void button9_Click_1(object sender, EventArgs e)
        {
            ArrayList array = searchUser(cb_user_type.SelectedIndex, tb_id.Text.Trim(), tb_password.Text.Trim(), tb_name.Text.Trim());
            userDisplay(array);
        }
        //显示数据
        private void userDisplay(ArrayList t_user)
        {
            //清空现有项
            for (int i = 0; i < clb_user.Items.Count; i++)
            {
                clb_user.Items.Clear();
            }
            //清空下面三个输入框
            tb_id.Clear();
            tb_password.Clear();
            tb_name.Clear();
            //显示
            for (int j = 0; j < t_user.Count / 3; j++)
            {
                string item = "";
                for (int i = 0; i < 3; i++)
                {
                    item += t_user[3 * j + i].ToString() + "                                 ";
                }
                clb_user.Items.Add(item);
            }

        }
        //修改数据按钮
        private void button11_Click(object sender, EventArgs e)
        {
            if (clb_user.CheckedItems.Count != 1)
            {
                MessageBox.Show("请选中一条数据！"); return;
            }
            //如果输人内容为空
            if (string.IsNullOrWhiteSpace(tb_name.Text.Trim()) || string.IsNullOrWhiteSpace(tb_password.Text.Trim())
                || string.IsNullOrWhiteSpace(tb_id.Text.Trim()))
            {
                MessageBox.Show("请输入完整的信息."); return;
            }
            //检查格式
            Regex regex = new Regex(@"^(((13[0-9]{1})|(15[0-35-9]{1})|(17[0-9]{1})|(18[0-9]{1}))+\d{8})$");
            if (!regex.IsMatch(tb_id.Text.Trim()))
            {
                MessageBox.Show("请输入手机号注册."); return;
            }
            if (tb_password.Text.Trim().Length > 10 || tb_name.Text.Trim().Length > 10)
            {
                MessageBox.Show("密码和昵称长度最多10个字符."); return;
            }
            //检查唯一性
            string oldID = clb_user.SelectedItem.ToString();
            Match match = Regex.Match(oldID, @"\d+");//取第一串数字
            oldID = match.Groups[0].Value;
            if (oldID != tb_id.Text.Trim())
            {
                ArrayList searched = searchUser(cb_user_type.SelectedIndex, tb_id.Text.Trim(), "", "");//查新id
                if(searched.Count > 0)
                {
                    MessageBox.Show("新id重复，请检查后再试."); return;
                }
            }
            //指定id修改用户数据(先删再注册)
            ArrayList selected = new ArrayList();
            selected.Add(oldID);
            deleteUser(cb_user_type.SelectedIndex, selected);
            register(cb_user_type.SelectedIndex, tb_id.Text.Trim(), tb_name.Text.Trim(), tb_password.Text.Trim());
            //查询并显示
            ArrayList array = searchUser(cb_user_type.SelectedIndex, "", "", "");
            userDisplay(array);
        }
        //删除数据按钮
        private void button10_Click(object sender, EventArgs e)
        {
            ArrayList selected = new ArrayList();
            for (int i = 0; i < clb_user.Items.Count; i++)//取所有选中的数据
            {
                if (clb_user.GetItemChecked(i))
                {
                    string str = clb_user.SelectedItem.ToString();
                    Match match = Regex.Match(str, @"\d+");//取第一串数字
                    str = match.Groups[0].Value;
                    if (cb_user_type.SelectedIndex == 1 && str == myid)
                    {
                        MessageBox.Show("不能删除自己的数据！  id:" + myid); return;
                    }
                    selected.Add(str);
                }
            }
            //删
            deleteUser(cb_user_type.SelectedIndex, selected);
            //查询并显示
            ArrayList array = searchUser(cb_user_type.SelectedIndex, "", "", "");
            userDisplay(array);
        }


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
                    Dispatcher new_dispatcher = new Dispatcher() { id = t_id.ToString().PadLeft(5, '0'), password = t_password, nickname = t_name };
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

        //删除用户数据//0顾客，1管理员，2调度员
        private void deleteUser(int user_type, ArrayList user_id)
        {
            try
            {
                foreach (string theID in user_id)
                {
                    if (user_type == 0)
                    {
                        var users = (from p in dc.customer
                                     where theID == (p.id.ToString())
                                     select p);
                        //dc.customer.DeleteAllOnSubmit(users);
                        dc.customer.DeleteOnSubmit(users.First());
                    }
                    else if (user_type == 1)
                    {
                        var users = (from p in dc.manager
                                     where theID == (p.id.ToString())
                                     select p);
                        dc.manager.DeleteAllOnSubmit(users);
                    }
                    else if (user_type == 2)
                    {
                        var users = (from p in dc.dispatcher
                                     where theID == (p.id.ToString())
                                     select p);
                        dc.dispatcher.DeleteAllOnSubmit(users);
                    }
                }
                dc.SubmitChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //查询用户数据//0顾客，1管理员，2调度员
        private ArrayList searchUser(int user_type, string user_id, string user_password, string user_name)
        {
            ArrayList result = new ArrayList();
            try
            {
                if (user_type == 0)//顾客
                {
                    if (user_id.Length == 0 && user_password.Length == 0 && user_name.Length == 0)
                    {
                        var t_user = (from p in dc.customer
                                      select p);
                        if (t_user != null)
                        {
                            foreach (var t in t_user)
                            {
                                result.Add(t.id);
                                result.Add(t.password);
                                result.Add(t.nickname);
                            }
                        }
                    }
                    if (user_id.Length > 0)
                    {
                        var t_user = (from p in dc.customer
                                      where p.id == user_id
                                      select p);
                        if (t_user != null)
                        {
                            foreach (var t in t_user)
                            {
                                result.Add(t.id);
                                result.Add(t.password);
                                result.Add(t.nickname);
                            }
                        }

                    }
                    else if (user_password.Length > 0)
                    {
                        var t_user = (from p in dc.customer
                                      where p.password == user_password
                                      select p);
                        foreach (var t in t_user)
                        {
                            result.Add(t.id);
                            result.Add(t.password);
                            result.Add(t.nickname);
                        }
                    }

                    else if (user_name.Length > 0)
                    {
                        var t_user = (from p in dc.customer
                                      where p.nickname == user_name
                                      select p);
                        foreach (var t in t_user)
                        {
                            result.Add(t.id);
                            result.Add(t.password);
                            result.Add(t.nickname);
                        }
                    }

                }
                else if (user_type == 1)//管理员
                {
                    if (user_id.Length == 0 && user_password.Length == 0 && user_name.Length == 0)
                    {
                        var t_user = (from p in dc.manager
                                      select p);
                        foreach (var t in t_user)
                        {
                            result.Add(t.id);
                            result.Add(t.password);
                            result.Add(t.nickname);
                        }
                    }
                    if (user_id.Length > 0)
                    {
                        var t_user = (from p in dc.manager
                                      where p.id == user_id
                                      select p);
                        foreach (var t in t_user)
                        {
                            result.Add(t.id);
                            result.Add(t.password);
                            result.Add(t.nickname);
                        }
                    }

                    else if (user_password.Length > 0)
                    {
                        var t_user = (from p in dc.manager
                                      where p.password == user_password
                                      select p);
                        foreach (var t in t_user)
                        {
                            result.Add(t.id);
                            result.Add(t.password);
                            result.Add(t.nickname);
                        }
                    }

                    else if (user_name.Length > 0)
                    {
                        var t_user = (from p in dc.manager
                                      where p.nickname == user_name
                                      select p);
                        foreach (var t in t_user)
                        {
                            result.Add(t.id);
                            result.Add(t.password);
                            result.Add(t.nickname);
                        }
                    }

                }
                else if (user_type == 2)//调度员
                {
                    if (user_id.Length == 0 && user_password.Length == 0 && user_name.Length == 0)
                    {
                        var t_user = (from p in dc.dispatcher
                                      select p);
                        foreach (var t in t_user)
                        {
                            result.Add(t.id);
                            result.Add(t.password);
                            result.Add(t.nickname);
                        }
                    }

                    if (user_id.Length > 0)
                    {
                        var t_user = (from p in dc.dispatcher
                                      where p.id == user_id
                                      select p);
                        foreach (var t in t_user)
                        {
                            result.Add(t.id);
                            result.Add(t.password);
                            result.Add(t.nickname);
                        }
                    }

                    else if (user_password.Length > 0)
                    {
                        var t_user = (from p in dc.dispatcher
                                      where p.password == user_password
                                      select p);
                        foreach (var t in t_user)
                        {
                            result.Add(t.id);
                            result.Add(t.password);
                            result.Add(t.nickname);
                        }
                    }

                    else if (user_name.Length > 0)
                    {
                        var t_user = (from p in dc.dispatcher
                                      where p.nickname == user_name
                                      select p);
                        foreach (var t in t_user)
                        {
                            result.Add(t.id);
                            result.Add(t.password);
                            result.Add(t.nickname);
                        }
                    }

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

        #region 窗体代码

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
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.pictureBox9 = new System.Windows.Forms.PictureBox();
            this.label21 = new System.Windows.Forms.Label();
            this.button8 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.pictureBox7 = new System.Windows.Forms.PictureBox();
            this.button7 = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label10 = new System.Windows.Forms.Label();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.button6 = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.uiLabel3 = new Sunny.UI.UILabel();
            this.uiLabel2 = new Sunny.UI.UILabel();
            this.pictureBox11 = new System.Windows.Forms.PictureBox();
            this.uiLabel1 = new Sunny.UI.UILabel();
            this.pictureBox10 = new System.Windows.Forms.PictureBox();
            this.button13 = new System.Windows.Forms.Button();
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.pictureBox8 = new System.Windows.Forms.PictureBox();
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
            this.uiPanel1 = new Sunny.UI.UIPanel();
            this.clb_user = new System.Windows.Forms.CheckedListBox();
            this.button9 = new System.Windows.Forms.Button();
            this.cb_user_type = new System.Windows.Forms.ComboBox();
            this.button10 = new System.Windows.Forms.Button();
            this.label20 = new System.Windows.Forms.Label();
            this.button11 = new System.Windows.Forms.Button();
            this.label19 = new System.Windows.Forms.Label();
            this.button12 = new System.Windows.Forms.Button();
            this.label18 = new System.Windows.Forms.Label();
            this.tb_name = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.tb_password = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.tb_id = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.tabPage6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).BeginInit();
            this.tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox10)).BeginInit();
            this.statusStrip2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.uiPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.splitContainer4);
            this.tabPage6.Location = new System.Drawing.Point(4, 33);
            this.tabPage6.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage6.Size = new System.Drawing.Size(1594, 826);
            this.tabPage6.TabIndex = 5;
            this.tabPage6.Text = "智能调度";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(3, 2);
            this.splitContainer4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitContainer4.Name = "splitContainer4";
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.AutoScroll = true;
            this.splitContainer4.Panel1.Controls.Add(this.pictureBox9);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.BackColor = System.Drawing.Color.AliceBlue;
            this.splitContainer4.Panel2.Controls.Add(this.label21);
            this.splitContainer4.Panel2.Controls.Add(this.button8);
            this.splitContainer4.Panel2.Controls.Add(this.button2);
            this.splitContainer4.Panel2.Controls.Add(this.button3);
            this.splitContainer4.Panel2.Controls.Add(this.label13);
            this.splitContainer4.Size = new System.Drawing.Size(1588, 822);
            this.splitContainer4.SplitterDistance = 1339;
            this.splitContainer4.TabIndex = 0;
            // 
            // pictureBox9
            // 
            this.pictureBox9.Image = global::shareDemo2.Properties.Resources.map11;
            this.pictureBox9.Location = new System.Drawing.Point(-7, -6);
            this.pictureBox9.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox9.Name = "pictureBox9";
            this.pictureBox9.Size = new System.Drawing.Size(8960, 3328);
            this.pictureBox9.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox9.TabIndex = 0;
            this.pictureBox9.TabStop = false;
            this.pictureBox9.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox9_Paint);
            this.pictureBox9.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox9_MouseClick);
            this.pictureBox9.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox9_MouseDown);
            this.pictureBox9.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox9_MouseMove);
            this.pictureBox9.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox9_MouseUp);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(11, 151);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(136, 48);
            this.label21.TabIndex = 5;
            this.label21.Text = "累计生成任务量\r\n10秒刷新一次";
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(19, 280);
            this.button8.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(119, 56);
            this.button8.TabIndex = 4;
            this.button8.Text = "删除区域";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(19, 216);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(119, 56);
            this.button2.TabIndex = 3;
            this.button2.Text = "开启/关闭";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(19, 342);
            this.button3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(119, 59);
            this.button3.TabIndex = 2;
            this.button3.Text = "增加区域";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(7, 31);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(226, 96);
            this.label13.TabIndex = 0;
            this.label13.Text = "图示为支持智能调度的区域\r\n\r\n橙色表示功能开启\r\n灰色表示功能关闭\r\n";
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.splitContainer3);
            this.tabPage5.Location = new System.Drawing.Point(4, 33);
            this.tabPage5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage5.Size = new System.Drawing.Size(1594, 826);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "下达任务";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(3, 2);
            this.splitContainer3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.AutoScroll = true;
            this.splitContainer3.Panel1.Controls.Add(this.pictureBox7);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.BackColor = System.Drawing.Color.MintCream;
            this.splitContainer3.Panel2.Controls.Add(this.button7);
            this.splitContainer3.Panel2.Controls.Add(this.label12);
            this.splitContainer3.Panel2.Controls.Add(this.dateTimePicker1);
            this.splitContainer3.Panel2.Controls.Add(this.label10);
            this.splitContainer3.Panel2.Controls.Add(this.comboBox3);
            this.splitContainer3.Panel2.Controls.Add(this.button6);
            this.splitContainer3.Panel2.Controls.Add(this.label9);
            this.splitContainer3.Panel2.Controls.Add(this.button5);
            this.splitContainer3.Size = new System.Drawing.Size(1588, 822);
            this.splitContainer3.SplitterDistance = 1315;
            this.splitContainer3.TabIndex = 0;
            // 
            // pictureBox7
            // 
            this.pictureBox7.Image = global::shareDemo2.Properties.Resources.map11;
            this.pictureBox7.Location = new System.Drawing.Point(-7, -6);
            this.pictureBox7.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox7.Name = "pictureBox7";
            this.pictureBox7.Size = new System.Drawing.Size(8960, 3328);
            this.pictureBox7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox7.TabIndex = 0;
            this.pictureBox7.TabStop = false;
            this.pictureBox7.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox7_Paint);
            this.pictureBox7.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox7_MouseDown);
            this.pictureBox7.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox7_MouseMove);
            this.pictureBox7.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox7_MouseUp);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(74, 70);
            this.button7.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(111, 54);
            this.button7.TabIndex = 10;
            this.button7.Text = "仓库管理";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(69, 257);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(136, 24);
            this.label12.TabIndex = 9;
            this.label12.Text = "创建任务的时间";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "yyyy/MM/dd HH:mm:ss";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(18, 293);
            this.dateTimePicker1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(241, 31);
            this.dateTimePicker1.TabIndex = 8;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(67, 157);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(82, 24);
            this.label10.TabIndex = 7;
            this.label10.Text = "任务类型";
            // 
            // comboBox3
            // 
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Items.AddRange(new object[] {
            "投放",
            "回收",
            "调度"});
            this.comboBox3.Location = new System.Drawing.Point(71, 196);
            this.comboBox3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(121, 32);
            this.comboBox3.TabIndex = 6;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(79, 425);
            this.button6.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(111, 62);
            this.button6.TabIndex = 5;
            this.button6.Text = "任务管理";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(60, 30);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(136, 24);
            this.label9.TabIndex = 4;
            this.label9.Text = "仓库中单车数量";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(79, 355);
            this.button5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(111, 62);
            this.button5.TabIndex = 3;
            this.button5.Text = "确认";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.splitContainer2);
            this.tabPage2.Controls.Add(this.statusStrip2);
            this.tabPage2.Location = new System.Drawing.Point(4, 33);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage2.Size = new System.Drawing.Size(1594, 826);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "用车热图";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(3, 2);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.AutoScroll = true;
            this.splitContainer2.Panel1.Controls.Add(this.pictureBox6);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.BackColor = System.Drawing.Color.SeaShell;
            this.splitContainer2.Panel2.Controls.Add(this.uiLabel3);
            this.splitContainer2.Panel2.Controls.Add(this.uiLabel2);
            this.splitContainer2.Panel2.Controls.Add(this.pictureBox11);
            this.splitContainer2.Panel2.Controls.Add(this.uiLabel1);
            this.splitContainer2.Panel2.Controls.Add(this.pictureBox10);
            this.splitContainer2.Panel2.Controls.Add(this.button13);
            this.splitContainer2.Panel2.Controls.Add(this.label8);
            this.splitContainer2.Panel2.Controls.Add(this.label7);
            this.splitContainer2.Panel2.Controls.Add(this.comboBox2);
            this.splitContainer2.Panel2.Controls.Add(this.comboBox1);
            this.splitContainer2.Panel2.Controls.Add(this.button1);
            this.splitContainer2.Size = new System.Drawing.Size(1588, 791);
            this.splitContainer2.SplitterDistance = 1375;
            this.splitContainer2.TabIndex = 1;
            // 
            // pictureBox6
            // 
            this.pictureBox6.Image = global::shareDemo2.Properties.Resources.map11;
            this.pictureBox6.Location = new System.Drawing.Point(-7, -6);
            this.pictureBox6.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(8960, 3328);
            this.pictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox6.TabIndex = 0;
            this.pictureBox6.TabStop = false;
            this.pictureBox6.Paint += new System.Windows.Forms.PaintEventHandler(this.PictureBox6_Paint);
            this.pictureBox6.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox6_MouseDown);
            this.pictureBox6.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox6_MouseMove);
            this.pictureBox6.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox6_MouseUp);
            // 
            // uiLabel3
            // 
            this.uiLabel3.BackColor = System.Drawing.Color.Snow;
            this.uiLabel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.uiLabel3.Font = new System.Drawing.Font("微软雅黑", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel3.Location = new System.Drawing.Point(30, 130);
            this.uiLabel3.Name = "uiLabel3";
            this.uiLabel3.Size = new System.Drawing.Size(155, 98);
            this.uiLabel3.Style = Sunny.UI.UIStyle.LayuiGreen;
            this.uiLabel3.TabIndex = 10;
            this.uiLabel3.Text = "用车热图分析";
            this.uiLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.uiLabel3.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // uiLabel2
            // 
            this.uiLabel2.BackColor = System.Drawing.Color.Transparent;
            this.uiLabel2.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel2.Location = new System.Drawing.Point(55, 473);
            this.uiLabel2.Name = "uiLabel2";
            this.uiLabel2.Size = new System.Drawing.Size(51, 25);
            this.uiLabel2.TabIndex = 9;
            this.uiLabel2.Text = "关锁";
            this.uiLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.uiLabel2.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // pictureBox11
            // 
            this.pictureBox11.BackColor = System.Drawing.Color.LightGreen;
            this.pictureBox11.Location = new System.Drawing.Point(108, 481);
            this.pictureBox11.Name = "pictureBox11";
            this.pictureBox11.Size = new System.Drawing.Size(53, 10);
            this.pictureBox11.TabIndex = 8;
            this.pictureBox11.TabStop = false;
            // 
            // uiLabel1
            // 
            this.uiLabel1.BackColor = System.Drawing.Color.Transparent;
            this.uiLabel1.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel1.Location = new System.Drawing.Point(55, 451);
            this.uiLabel1.Name = "uiLabel1";
            this.uiLabel1.Size = new System.Drawing.Size(51, 25);
            this.uiLabel1.TabIndex = 7;
            this.uiLabel1.Text = "开锁";
            this.uiLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.uiLabel1.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // pictureBox10
            // 
            this.pictureBox10.BackColor = System.Drawing.Color.DarkOrange;
            this.pictureBox10.Location = new System.Drawing.Point(108, 459);
            this.pictureBox10.Name = "pictureBox10";
            this.pictureBox10.Size = new System.Drawing.Size(53, 10);
            this.pictureBox10.TabIndex = 6;
            this.pictureBox10.TabStop = false;
            // 
            // button13
            // 
            this.button13.Location = new System.Drawing.Point(47, 598);
            this.button13.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(118, 47);
            this.button13.TabIndex = 5;
            this.button13.Text = "周趋势分析";
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Click += new System.EventHandler(this.button13_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(43, 349);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(118, 24);
            this.label8.TabIndex = 4;
            this.label8.Text = "热图展示类型";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(42, 252);
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
            this.comboBox2.Location = new System.Drawing.Point(43, 385);
            this.comboBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
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
            this.comboBox1.Location = new System.Drawing.Point(43, 289);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 32);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(47, 535);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(118, 47);
            this.button1.TabIndex = 0;
            this.button1.Text = "小时趋势分析";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // statusStrip2
            // 
            this.statusStrip2.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel5});
            this.statusStrip2.Location = new System.Drawing.Point(3, 793);
            this.statusStrip2.Name = "statusStrip2";
            this.statusStrip2.Size = new System.Drawing.Size(1588, 31);
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
            this.tabPage1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage1.Size = new System.Drawing.Size(1914, 1006);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "地图快照";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 2);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.AutoScroll = true;
            this.splitContainer1.Panel1.Controls.Add(this.pictureBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.Azure;
            this.splitContainer1.Panel2.Controls.Add(this.textBox1);
            this.splitContainer1.Panel2.Controls.Add(this.button4);
            this.splitContainer1.Panel2.Controls.Add(this.label11);
            this.splitContainer1.Panel2.Controls.Add(this.pictureBox8);
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
            this.splitContainer1.Size = new System.Drawing.Size(1908, 971);
            this.splitContainer1.SplitterDistance = 1620;
            this.splitContainer1.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::shareDemo2.Properties.Resources.map11;
            this.pictureBox1.Location = new System.Drawing.Point(-7, -6);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(8960, 3328);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(18, 298);
            this.textBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(200, 190);
            this.textBox1.TabIndex = 13;
            this.textBox1.Text = "框中为潜在投放区域\r\n即使用APP时，周围无车\r\n\r\n红色 超过55人次\r\n橙色 超过25人次\r\n绿色 低于25人次";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(22, 500);
            this.button4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(116, 49);
            this.button4.TabIndex = 12;
            this.button4.Text = "刷新";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(49, 259);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(118, 24);
            this.label11.TabIndex = 11;
            this.label11.Text = "车辆即将到达";
            // 
            // pictureBox8
            // 
            this.pictureBox8.Image = global::shareDemo2.Properties.Resources.lightred;
            this.pictureBox8.Location = new System.Drawing.Point(22, 263);
            this.pictureBox8.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox8.Name = "pictureBox8";
            this.pictureBox8.Size = new System.Drawing.Size(15, 15);
            this.pictureBox8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox8.TabIndex = 10;
            this.pictureBox8.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(21, 55);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 24);
            this.label6.TabIndex = 9;
            this.label6.Text = "当前时间";
            // 
            // pictureBox5
            // 
            this.pictureBox5.Image = global::shareDemo2.Properties.Resources.gray;
            this.pictureBox5.Location = new System.Drawing.Point(22, 224);
            this.pictureBox5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(15, 15);
            this.pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox5.TabIndex = 8;
            this.pictureBox5.TabStop = false;
            // 
            // pictureBox4
            // 
            this.pictureBox4.Image = global::shareDemo2.Properties.Resources.yellow;
            this.pictureBox4.Location = new System.Drawing.Point(22, 184);
            this.pictureBox4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(15, 15);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox4.TabIndex = 7;
            this.pictureBox4.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::shareDemo2.Properties.Resources.green;
            this.pictureBox3.Location = new System.Drawing.Point(22, 146);
            this.pictureBox3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(15, 15);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox3.TabIndex = 6;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::shareDemo2.Properties.Resources.blue;
            this.pictureBox2.Location = new System.Drawing.Point(22, 106);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(15, 15);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 5;
            this.pictureBox2.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(45, 222);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 24);
            this.label5.TabIndex = 4;
            this.label5.Text = "等待调度";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(45, 181);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 24);
            this.label4.TabIndex = 3;
            this.label4.Text = "等待检查";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(45, 140);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 24);
            this.label3.TabIndex = 2;
            this.label3.Text = "正在使用";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(45, 101);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 24);
            this.label2.TabIndex = 1;
            this.label2.Text = "当前可用";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(33, 18);
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
            this.statusStrip1.Location = new System.Drawing.Point(3, 973);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1908, 31);
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
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage6);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControl1.Location = new System.Drawing.Point(-1, 35);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1922, 1043);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.Snow;
            this.tabPage3.BackgroundImage = global::shareDemo2.Properties.Resources._2_1;
            this.tabPage3.Controls.Add(this.uiPanel1);
            this.tabPage3.Location = new System.Drawing.Point(4, 33);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage3.Size = new System.Drawing.Size(1914, 1006);
            this.tabPage3.TabIndex = 6;
            this.tabPage3.Text = "人事管理";
            // 
            // uiPanel1
            // 
            this.uiPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uiPanel1.AutoSize = true;
            this.uiPanel1.BackColor = System.Drawing.Color.Transparent;
            this.uiPanel1.Controls.Add(this.clb_user);
            this.uiPanel1.Controls.Add(this.button9);
            this.uiPanel1.Controls.Add(this.cb_user_type);
            this.uiPanel1.Controls.Add(this.button10);
            this.uiPanel1.Controls.Add(this.label20);
            this.uiPanel1.Controls.Add(this.button11);
            this.uiPanel1.Controls.Add(this.label19);
            this.uiPanel1.Controls.Add(this.button12);
            this.uiPanel1.Controls.Add(this.label18);
            this.uiPanel1.Controls.Add(this.tb_name);
            this.uiPanel1.Controls.Add(this.label17);
            this.uiPanel1.Controls.Add(this.tb_password);
            this.uiPanel1.Controls.Add(this.label16);
            this.uiPanel1.Controls.Add(this.tb_id);
            this.uiPanel1.Controls.Add(this.label14);
            this.uiPanel1.Controls.Add(this.label15);
            this.uiPanel1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiPanel1.Location = new System.Drawing.Point(252, 0);
            this.uiPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiPanel1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiPanel1.Name = "uiPanel1";
            this.uiPanel1.Size = new System.Drawing.Size(1400, 1010);
            this.uiPanel1.TabIndex = 24;
            this.uiPanel1.Text = null;
            this.uiPanel1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.uiPanel1.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.uiPanel1.Resize += new System.EventHandler(this.Form1_Resize);
            // 
            // clb_user
            // 
            this.clb_user.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.clb_user.CausesValidation = false;
            this.clb_user.FormattingEnabled = true;
            this.clb_user.Location = new System.Drawing.Point(288, 240);
            this.clb_user.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.clb_user.Name = "clb_user";
            this.clb_user.Size = new System.Drawing.Size(861, 364);
            this.clb_user.TabIndex = 10;
            // 
            // button9
            // 
            this.button9.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button9.CausesValidation = false;
            this.button9.Location = new System.Drawing.Point(998, 683);
            this.button9.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(108, 47);
            this.button9.TabIndex = 20;
            this.button9.Text = "查询数据";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click_1);
            // 
            // cb_user_type
            // 
            this.cb_user_type.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cb_user_type.CausesValidation = false;
            this.cb_user_type.FormattingEnabled = true;
            this.cb_user_type.Items.AddRange(new object[] {
            "顾客",
            "管理员",
            "调度员"});
            this.cb_user_type.Location = new System.Drawing.Point(426, 169);
            this.cb_user_type.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cb_user_type.Name = "cb_user_type";
            this.cb_user_type.Size = new System.Drawing.Size(136, 39);
            this.cb_user_type.TabIndex = 8;
            this.cb_user_type.Text = "顾客";
            // 
            // button10
            // 
            this.button10.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button10.CausesValidation = false;
            this.button10.Location = new System.Drawing.Point(777, 683);
            this.button10.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(108, 47);
            this.button10.TabIndex = 21;
            this.button10.Text = "删除数据";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // label20
            // 
            this.label20.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label20.AutoSize = true;
            this.label20.BackColor = System.Drawing.Color.Transparent;
            this.label20.CausesValidation = false;
            this.label20.Location = new System.Drawing.Point(286, 177);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(134, 31);
            this.label20.TabIndex = 9;
            this.label20.Text = "用户类型：";
            // 
            // button11
            // 
            this.button11.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button11.CausesValidation = false;
            this.button11.Location = new System.Drawing.Point(545, 683);
            this.button11.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(108, 47);
            this.button11.TabIndex = 22;
            this.button11.Text = "修改数据";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // label19
            // 
            this.label19.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label19.AutoSize = true;
            this.label19.BackColor = System.Drawing.Color.Transparent;
            this.label19.CausesValidation = false;
            this.label19.Location = new System.Drawing.Point(355, 205);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(35, 31);
            this.label19.TabIndex = 12;
            this.label19.Text = "id";
            // 
            // button12
            // 
            this.button12.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button12.CausesValidation = false;
            this.button12.Location = new System.Drawing.Point(309, 683);
            this.button12.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(108, 47);
            this.button12.TabIndex = 23;
            this.button12.Text = "插入数据";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // label18
            // 
            this.label18.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label18.AutoSize = true;
            this.label18.BackColor = System.Drawing.Color.Transparent;
            this.label18.CausesValidation = false;
            this.label18.Location = new System.Drawing.Point(318, 769);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(35, 31);
            this.label18.TabIndex = 11;
            this.label18.Text = "id";
            // 
            // tb_name
            // 
            this.tb_name.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tb_name.CausesValidation = false;
            this.tb_name.Location = new System.Drawing.Point(952, 761);
            this.tb_name.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tb_name.Name = "tb_name";
            this.tb_name.Size = new System.Drawing.Size(178, 39);
            this.tb_name.TabIndex = 17;
            // 
            // label17
            // 
            this.label17.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label17.AutoSize = true;
            this.label17.BackColor = System.Drawing.Color.Transparent;
            this.label17.CausesValidation = false;
            this.label17.Location = new System.Drawing.Point(566, 205);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(62, 31);
            this.label17.TabIndex = 14;
            this.label17.Text = "密码";
            // 
            // tb_password
            // 
            this.tb_password.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tb_password.CausesValidation = false;
            this.tb_password.Location = new System.Drawing.Point(654, 761);
            this.tb_password.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tb_password.Name = "tb_password";
            this.tb_password.Size = new System.Drawing.Size(178, 39);
            this.tb_password.TabIndex = 18;
            // 
            // label16
            // 
            this.label16.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label16.AutoSize = true;
            this.label16.BackColor = System.Drawing.Color.Transparent;
            this.label16.CausesValidation = false;
            this.label16.Location = new System.Drawing.Point(591, 769);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(62, 31);
            this.label16.TabIndex = 13;
            this.label16.Text = "密码";
            // 
            // tb_id
            // 
            this.tb_id.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tb_id.CausesValidation = false;
            this.tb_id.Location = new System.Drawing.Point(363, 761);
            this.tb_id.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tb_id.Name = "tb_id";
            this.tb_id.Size = new System.Drawing.Size(178, 39);
            this.tb_id.TabIndex = 19;
            // 
            // label14
            // 
            this.label14.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.CausesValidation = false;
            this.label14.Location = new System.Drawing.Point(788, 205);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(62, 31);
            this.label14.TabIndex = 16;
            this.label14.Text = "昵称";
            // 
            // label15
            // 
            this.label15.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.CausesValidation = false;
            this.label15.Location = new System.Drawing.Point(884, 767);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(62, 31);
            this.label15.TabIndex = 15;
            this.label15.Text = "昵称";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Interval = 10000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // managerForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1920, 1080);
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "managerForm";
            this.Text = "管理员";
            this.ZoomScaleRect = new System.Drawing.Rectangle(22, 22, 1600, 900);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.tabPage6.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel1.PerformLayout();
            this.splitContainer4.Panel2.ResumeLayout(false);
            this.splitContainer4.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).EndInit();
            this.tabPage5.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox10)).EndInit();
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
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.uiPanel1.ResumeLayout(false);
            this.uiPanel1.PerformLayout();
            this.ResumeLayout(false);

        }



        #endregion

        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.TabPage tabPage5;
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
        private SplitContainer splitContainer3;
        private PictureBox pictureBox7;
        private Button button5;
        private Label label9;
        private Button button6;
        private Label label10;
        private ComboBox comboBox3;
        private Label label11;
        private PictureBox pictureBox8;
        private DateTimePicker dateTimePicker1;
        private Label label12;
        private SplitContainer splitContainer4;
        private PictureBox pictureBox9;
        private Label label13;
        private Button button3;
        private Button button4;
        private Button button2;
        private TextBox textBox1;
        private Button button7;
        private Button button8;

        #endregion

        #region 控件大小随窗体大小等比例缩放
        private float x;//定义当前窗体的宽度
        private float y;//定义当前窗体的高度
        private void setTag(Control cons)
        {
            foreach (Control con in cons.Controls)
            {
                con.Tag = con.Width + ";" + con.Height + ";" + con.Left + ";" + con.Top + ";" + con.Font.Size;
                if (con.Controls.Count > 0)
                {
                    setTag(con);
                }
            }
        }
        private void setControls(float newx, float newy, Control cons)
        {
            //遍历窗体中的控件，重新设置控件的值
            foreach (Control con in cons.Controls)
            {
                //获取控件的Tag属性值，并分割后存储字符串数组
                if (con.Tag != null)
                {
                    string[] mytag = con.Tag.ToString().Split(new char[] { ';' });
                    //根据窗体缩放的比例确定控件的值
                    con.Width = Convert.ToInt32(System.Convert.ToSingle(mytag[0]) * newx);//宽度
                    con.Height = Convert.ToInt32(System.Convert.ToSingle(mytag[1]) * newy);//高度
                    con.Left = Convert.ToInt32(System.Convert.ToSingle(mytag[2]) * newx);//左边距
                    con.Top = Convert.ToInt32(System.Convert.ToSingle(mytag[3]) * newy);//顶边距
                    Single currentSize = System.Convert.ToSingle(mytag[4]) * newy;//字体大小
                    con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
                    if (con.Controls.Count > 0)
                    {
                        setControls(newx, newy, con);
                    }
                }
            }
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            float newx = (this.Width) / x;
            float newy = (this.Height) / y;
            setControls(newx, newy, this);
        }

        #endregion
    }
}
