using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sunny.UI;
using System.Windows.Forms.DataVisualization.Charting;
namespace shareDemo2
{
    public partial class trend : UIForm
    {
        public trend(int sel1, int sel2, IQueryable<orderform> order)
        {
            InitializeComponent();
            Cratechart0();
            ct.Series.Clear();
            if (sel2 == 1)
            {
                Cratechart1(sel1, sel2, order);
            }
            else Cratechart2(sel1, sel2, order);
        }
        public trend(int sel1, int sel2, IQueryable<orderform> order1, IQueryable<orderform> order2)
        {
            InitializeComponent();
            Cratechart0();
            Cratechart1(sel1, sel2, order1);
            Cratechart2(sel1, sel2, order2);
        }
        int n_series = -1;
        int trend_start = 0;
        int trend_end = 0;
        public void Cratechart0()
        {
            ct.ChartAreas.Add(new ChartArea() { Name = "ca1" }); //背景框
            ct.ChartAreas[0].Axes[0].MajorGrid.Enabled = false; //X轴上网格
            ct.ChartAreas[0].Axes[1].MajorGrid.Enabled = false; //y轴上网格
            ct.ChartAreas[0].Axes[0].MajorGrid.LineDashStyle = ChartDashStyle.Dash; //网格类型 短横线
            ct.ChartAreas[0].Axes[0].MajorGrid.LineColor = Color.Gray;
            ct.ChartAreas[0].Axes[0].MajorTickMark.Enabled = false; // x轴上突出的小点
            ct.ChartAreas[0].Axes[1].MajorTickMark.Enabled = false; //
            ct.ChartAreas[0].Axes[1].IsInterlaced = true; //显示交错带
            ct.ChartAreas[0].Axes[1].MajorGrid.LineDashStyle = ChartDashStyle.Dash; //网格类型 短横线
            ct.ChartAreas[0].Axes[1].MajorGrid.LineColor = Color.Blue;
            ct.ChartAreas[0].Axes[1].MajorGrid.LineWidth = 3;
            ct.ChartAreas[0].BackColor = System.Drawing.Color.Transparent; //设置区域内背景透明
        }
        public void Series_set(int i)
        {
            ct.Series.Add(new Series());
            n_series++;
            ct.Series[n_series].Label = "#VAL";                //设置显示X Y的值    
            ct.Series[n_series].ChartArea = ct.ChartAreas[0].Name; //设置图表背景框ChartArea 
            ct.Series[n_series].ChartType = SeriesChartType.Line; //图类型(折线)
            if (i == 1)
            {
                ct.Series[n_series].Color = Color.Orange; //线条颜色
                ct.Series[n_series].MarkerColor = Color.Orange; //标记点中心颜色
                ct.Series[n_series].BorderWidth = 4; //线条粗细
            }
            else
            {
                ct.Series[n_series].Color = Color.Green; //线条颜色
                ct.Series[n_series].MarkerColor = Color.Green; //标记点中心颜色
                ct.Series[n_series].BorderWidth = 2; //线条粗细
            }
            ct.Series[n_series].MarkerBorderColor = Color.Red; //标记点边框颜色
            ct.Series[n_series].MarkerBorderWidth = 3; //标记点边框大小            
            ct.Series[n_series].MarkerSize = 5; //标记点大小
            ct.Series[n_series].MarkerStyle = MarkerStyle.Circle; //标记点类型
        }
        public void Cratechart1(int sel1, int sel2, IQueryable<orderform> order)
        {

            Series_set(1);
            List<string> xDate1 = new List<string>();
            List<int> yDate1 = new List<int>();
            DateTime date = DateTime.Today;
            trend_start = 0;
            switch (sel1)
            {
                case 0:
                    for (int i = 0; i < 8; i++)
                    {
                        xDate1.Add(date.AddHours(i * 3).ToString("t"));
                        yDate1.Add((from x in order
                                    where x.start_time.Value.Hour >= date.AddHours(i * 3).Hour && x.start_time.Value.Hour <= date.AddHours((i + 1) * 3).Hour
                                    select x).Count() / 3);
                        trend_start += yDate1[i];
                    }
                    ct.Series[n_series].Name = "开始用车/平均每日";
                    break;
                case 1:
                    for (int i = 0; i < 8; i++)
                    {
                        xDate1.Add(date.AddHours(i * 3).ToString("t"));
                        yDate1.Add((from x in order
                                    where x.start_time.Value.Hour >= date.AddHours(i * 3).Hour && x.start_time.Value.Hour <= date.AddHours((i + 1) * 3).Hour
                                    select x).Count() / 3);
                        trend_start += yDate1[i];
                    }
                    ct.Series[n_series].Name = "开始用车/平均每周";
                    break;
                case 2:
                    for (int i = 0; i < 8; i++)
                    {
                        xDate1.Add(date.AddHours(i * 3).ToString("t"));
                        yDate1.Add((from x in order
                                    where x.start_time.Value.Hour >= date.AddHours(i * 3).Hour && x.start_time.Value.Hour <= date.AddHours((i + 1) * 3).Hour
                                    select x).Count() / 3);
                        trend_start += yDate1[i];
                    }
                    ct.Series[n_series].Name = "开始用车/平均每月";
                    break;
                case 3:
                    for (int i = 0; i < 8; i++)
                    {
                        xDate1.Add(date.AddHours(i * 3).ToString("t"));
                        yDate1.Add((from x in order
                                    where x.start_time.Value.Hour >= date.AddHours(i * 3).Hour && x.start_time.Value.Hour <= date.AddHours((i + 1) * 3).Hour
                                    select x).Count());
                        trend_start += yDate1[i];
                    }
                    ct.Series[n_series].Name = "开始用车/平均一日";
                    break;
                case 4:
                    for (int i = 0; i < 8; i++)
                    {
                        xDate1.Add(date.AddHours(i * 3).ToString("t"));
                        yDate1.Add((from x in order
                                    where x.start_time.Value.Hour >= date.AddHours(i * 3).Hour && x.start_time.Value.Hour <= date.AddHours((i + 1) * 3).Hour
                                    select x).Count());
                        trend_start += yDate1[i];
                    }
                    ct.Series[n_series].Name = "开始用车/一个月";
                    break;
            }
            ct.Series[n_series].Points.DataBindXY(xDate1, yDate1);
        }

        public void Cratechart2(int sel1, int sel2, IQueryable<orderform> order)
        {
            Series_set(2);
            List<string> xDate1 = new List<string>();
            List<int> yDate1 = new List<int>();
            DateTime date = DateTime.Today;
            switch (sel1)
            {
                case 0:
                    for (int i = 0; i < 8; i++)
                    {
                        xDate1.Add(date.AddHours(i * 3).ToString("t"));
                        yDate1.Add((from x in order
                                    where x.end_time.Value.Hour >= date.AddHours(i * 3).Hour && x.start_time.Value.Hour <= date.AddHours((i + 1) * 3).Hour
                                    select x).Count() / 3);
                        trend_end += yDate1[i];
                    }
                    ct.Series[n_series].Name = "结束用车/平均每日";
                    break;
                case 1:
                    for (int i = 0; i < 8; i++)
                    {
                        xDate1.Add(date.AddHours(i * 3).ToString("t"));
                        yDate1.Add((from x in order
                                    where x.end_time.Value.Hour >= date.AddHours(i * 3).Hour && x.start_time.Value.Hour <= date.AddHours((i + 1) * 3).Hour
                                    select x).Count() / 3);
                        trend_end += yDate1[i];
                    }
                    ct.Series[n_series].Name = "结束用车/平均每周";
                    break;
                case 2:
                    for (int i = 0; i < 8; i++)
                    {
                        xDate1.Add(date.AddHours(i * 3).ToString("t"));
                        yDate1.Add((from x in order
                                    where x.end_time.Value.Hour >= date.AddHours(i * 3).Hour && x.start_time.Value.Hour <= date.AddHours((i + 1) * 3).Hour
                                    select x).Count() / 3);
                        trend_end += yDate1[i];
                    }
                    ct.Series[n_series].Name = "结束用车/平均每月";
                    break;
                case 3:
                    for (int i = 0; i < 8; i++)
                    {
                        xDate1.Add(date.AddHours(i * 3).ToString("t"));
                        yDate1.Add((from x in order
                                    where x.end_time.Value.Hour >= date.AddHours(i * 3).Hour && x.start_time.Value.Hour <= date.AddHours((i + 1) * 3).Hour
                                    select x).Count());
                        trend_end += yDate1[i];
                    }
                    ct.Series[n_series].Name = "结束用车/平均一日";
                    break;
                case 4:
                    for (int i = 0; i < 8; i++)
                    {
                        xDate1.Add(date.AddHours(i * 3).ToString("t"));
                        yDate1.Add((from x in order
                                    where x.end_time.Value.Hour >= date.AddHours(i * 3).Hour && x.start_time.Value.Hour <= date.AddHours((i + 1) * 3).Hour
                                    select x).Count());
                        trend_end += yDate1[i];
                    }
                    ct.Series[n_series].Name = "结束用车/一个月";
                    break;
            }
            ct.Series[n_series].Points.DataBindXY(xDate1, yDate1);
            return;
        }
    }
}
