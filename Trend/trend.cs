using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Sunny.UI;

namespace shareDemo2.Trend
{
    public partial class Trend : Form
    {
        
        public Trend()
        {
            InitializeComponent();
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
       
        int n_series = -1;
        public void Cratechart1(int sel1,int sel2,IQueryable<DateTime>order)
        {

            List<string> xDate1 = new List<string>();
            List<int> yDate1 = new List<int>();
            switch (sel1)
            {
                case 0:
                    for (int i = 0; i < 3; i++)
                    {
                        xDate1.Add(DateTime.Now.AddDays(i - 2).ToString("D"));
                        yDate1.Add((from x in order
                                    where x.Day == DateTime.Now.AddDays(i-2).Day
                                    select x).Count()) ;
                    }
                    break;
                case 1:
                    xDate1 = new List<string>() { "上上周", "上周", "这周" };
                    int week = DateTime.Now.DayOfWeek - DayOfWeek.Monday;
                    for (int i = 2; i >=0; i--)
                    {
                        yDate1.Add((from x in order
                                    where x.Date >= DateTime.Now.AddDays(-week + (-i * 7)) &&
                                    x.Date <= DateTime.Now.AddDays(-i * 7)
                                    select x).Count());
                    }
                    break;
                case 2:
                    for (int i = 0; i < 3; i++)
                    {
                        xDate1.Add(DateTime.Now.AddMonths(i - 2).ToString("Y"));
                        yDate1.Add((from x in order
                                    where x.Month == DateTime.Now.AddMonths(i - 2).Month
                                    select x).Count());
                    }
                    break;
                case 3:
                    for (int i = 0; i < 24; i++)
                    {
                        DateTime d1 = new System.DateTime(2022, 1, 1, 0, 0, 0);
                        int temp2 = (from x in order
                                     where x.Hour == i
                                     select x).Count();
                        if(temp2!=0||i==0||i==12||i==23)
                        {
                            xDate1.Add(d1.AddHours(i).ToString("t"));
                            yDate1.Add(temp2);
                        }

                    }
                    break;
                case 4:
                    int y = DateTime.Now.AddYears(-1).Year;
                    int m = DateTime.Now.AddYears(-1).Month;
                    DateTime d = new System.DateTime(y, m, 1);
                    for (int i = 0; i < DateTime.DaysInMonth(y, m); i++)
                    {
                        int temp3 = (from x in order
                                     where x.Day == i + 1
                                     select x).Count();
                        if (temp3 != 0 || i == 0 || i == 14 || i == DateTime.DaysInMonth(y, m) - 1)
                        {
                            xDate1.Add(d.AddDays(i).ToString("M"));
                            //ct.ChartAreas[0].Axes[0].LabelStyle.Format = "#日";
                            yDate1.Add(temp3);
                        }

                    }
                    break;
            }
            ct.Series.Add(new Series());
            n_series++;
            ct.Series[n_series].Label = "#VAL";                //设置显示X Y的值    
            ct.Series[n_series].Name = "开始用车";
            ct.Series[n_series].ChartArea = ct.ChartAreas[0].Name; //设置图表背景框ChartArea 
            ct.Series[n_series].ChartType = SeriesChartType.Line; //图类型(折线)
            ct.Series[n_series].Color = Color.Yellow; //线条颜色
            ct.Series[n_series].BorderWidth = 3; //线条粗细
            ct.Series[n_series].MarkerBorderColor = Color.Red; //标记点边框颜色
            ct.Series[n_series].MarkerBorderWidth = 4; //标记点边框大小
            ct.Series[n_series].MarkerColor = Color.Red; //标记点中心颜色
            ct.Series[n_series].MarkerSize = 5; //标记点大小
            ct.Series[n_series].MarkerStyle = MarkerStyle.Circle; //标记点类型
            ct.Series[n_series].Points.DataBindXY(xDate1, yDate1);
            
        }
        public void Cratechart2(int sel1, int sel2, IQueryable<DateTime> order)
        {
            List<string> xDate1 = new List<string>();
            List<int> yDate1 = new List<int>();
            switch (sel1)
            {
                case 0:
                    for (int i = 0; i < 3; i++)
                    {
                        xDate1.Add(DateTime.Now.AddDays(i - 2).ToString("D"));
                        yDate1.Add((from x in order
                                    where x.Day == DateTime.Now.AddDays(i - 2).Day
                                    select x).Count());
                    }
                    break;
                case 1:
                    xDate1 = new List<string>() {"上上周","上周","这周" };
                    int week = DateTime.Now.DayOfWeek-DayOfWeek.Monday;
                    for(int i = 2; i >= 0; i--)
                    {
                        yDate1.Add((from x in order
                                    where x.Date >= DateTime.Now.AddDays(-week+(-i*7)) &&
                                    x.Date <= DateTime.Now.AddDays(-i * 7)
                                    select x).Count());
                    }
                    break;
                case 2:
                    for (int i = 0; i < 3; i++)
                    {
                        xDate1.Add(DateTime.Now.AddMonths(i - 2).ToString("Y"));
                        yDate1.Add((from x in order
                                    where x.Month == DateTime.Now.AddMonths(i - 2).Month
                                    select x).Count());
                    }
                    break;
                case 3:
                    for (int i = 0; i < 24; i++)
                    {
                        DateTime d1 = new System.DateTime(2022, 1, 1, 0, 0, 0);
                        int temp2 = (from x in order
                                     where x.Hour == i
                                     select x).Count();
                        if (temp2 != 0 || i == 0 || i == 12 || i == 23)
                        {
                            xDate1.Add(d1.AddHours(i).ToString("t"));
                            yDate1.Add(temp2);
                        }

                    }
                    break;
                case 4:
                    int y = DateTime.Now.AddYears(-1).Year;
                    int m = DateTime.Now.AddYears(-1).Month;
                    DateTime d = new System.DateTime(y, m, 1);
                    for (int i = 0; i < DateTime.DaysInMonth(y, m); i++)
                    {
                        int temp3 = (from x in order
                                     where x.Day == i+1
                                     select x).Count();
                        if(temp3!=0 || i == 0 || i == 14 || i == DateTime.DaysInMonth(y, m)-1)
                        {
                            xDate1.Add(d.AddDays(i).ToString("d"));
                            yDate1.Add(temp3);
                        }
                        
                    }
                    break;
            }
            
            ct.Series.Add(new Series());
            n_series++;ct.Series[n_series].Points.DataBindXY(xDate1, yDate1);
            ct.Series[n_series].Label = "#VAL";                //设置显示X Y的值    
            ct.Series[n_series].Name = "结束用车";
            ct.Series[n_series].ChartArea = ct.ChartAreas[0].Name; //设置图表背景框ChartArea 
            ct.Series[n_series].ChartType = SeriesChartType.Line; //图类型(折线)
            ct.Series[n_series].Color = Color.Blue; //线条颜色
            ct.Series[n_series].BorderWidth = 2; //线条粗细
            ct.Series[n_series].MarkerBorderColor = Color.Red; //标记点边框颜色
            ct.Series[n_series].MarkerBorderWidth = 3; //标记点边框大小
            ct.Series[n_series].MarkerColor = Color.Blue; //标记点中心颜色
            ct.Series[n_series].MarkerSize = 5; //标记点大小
            ct.Series[n_series].MarkerStyle = MarkerStyle.Circle; //标记点类型
            ct.Series[n_series].AxisLabel = "#辆";
        }
         
    }
    
}
