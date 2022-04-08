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
namespace shareDemo2.Trend
{
    public partial class Trend : Form
    {
        public Trend()
        {
            InitializeComponent();
            
        }
        public void Cratechart1(int sel1,int sel2,IQueryable<DateTime>order)
        {
            DataTable dt = new DataTable();
            List<DateTime> xDate1 = new List<DateTime>() 
                            { DateTime.Now.AddDays(-2),DateTime.Now.AddDays(-1)};
            List<int> xDate2 = new List<int>() { 1, 1};
            /*xDate2[0] = (from x in order
                      where x.Date == xDate1[0].Date
                      select x).Count();
            xDate2[1] = (from x in order
                         where x.Date == xDate1[1].Date
                         select x).Count();
            xDate2[2] = (from x in order
                         where x.Date == xDate1[2].Date
                         select x).Count();*/
            /*var gourps = from x in order.GroupBy(x => x.Date)
                         select new
                         {
                             cout = x.Count(),
                             x.First().Date,
                         };*/

            ct.ChartAreas.Add(new ChartArea() { Name = "ca1" }); //背景框
            ct.ChartAreas[0].Axes[0].MajorGrid.Enabled = false; //X轴上网格
            ct.ChartAreas[0].Axes[1].MajorGrid.Enabled = false; //y轴上网格
            ct.ChartAreas[0].Axes[0].MajorGrid.LineDashStyle = ChartDashStyle.Dash; //网格类型 短横线
            ct.ChartAreas[0].Axes[0].MajorGrid.LineColor = Color.Gray;
            ct.ChartAreas[0].Axes[0].MajorTickMark.Enabled = false; // x轴上突出的小点
            ct.ChartAreas[0].Axes[1].MajorTickMark.Enabled = false; //
            ct.ChartAreas[0].Axes[1].IsInterlaced = true; //显示交错带
            ct.ChartAreas[0].Axes[0].LabelStyle.Format = "#年"; //设置X轴显示样式
            ct.ChartAreas[0].Axes[1].MajorGrid.LineDashStyle = ChartDashStyle.Dash; //网格类型 短横线
            ct.ChartAreas[0].Axes[1].MajorGrid.LineColor = Color.Blue;
            ct.ChartAreas[0].Axes[1].MajorGrid.LineWidth = 3;

            ct.ChartAreas[0].BackColor = System.Drawing.Color.Transparent; //设置区域内背景透明
            List<int> txData2 = new List<int>() { 2011, 2012, 2013, 2014, 2015, 2016 };
            List<int> tyData2 = new List<int>() { 9, 6, 7, 4, 5, 4 };
            List<int> txData3 = new List<int>() { 2012 };
            List<int> tyData3 = new List<int>() { 7 };
            ct.Series.Add(new Series()); //添加一个图表序列
            ct.Series[0].XValueType = ChartValueType.String; //设置X轴上的值类型
            ct.Series[0].Label = "#VAL"; //设置显示X Y的值
            ct.Series[0].ToolTip = "#VALX年\r#VAL"; //鼠标移动到对应点显示数值
            ct.Series[0].ChartArea = ct.ChartAreas[0].Name; //设置图表背景框ChartArea 
            ct.Series[0].ChartType = SeriesChartType.Line; //图类型(折线)
            ct.Series[0].Points.DataBindXY(txData2, tyData2); //添加数据
            ct.Series[0].Color = Color.Red; //线条颜色
            ct.Series[0].BorderWidth = 3; //线条粗细
            ct.Series[0].MarkerBorderColor = Color.Red; //标记点边框颜色
            ct.Series[0].MarkerBorderWidth = 3; //标记点边框大小
            ct.Series[0].MarkerColor = Color.Red; //标记点中心颜色
            ct.Series[0].MarkerSize = 5; //标记点大小
            ct.Series[0].MarkerStyle = MarkerStyle.Circle; //标记点类型
        }
    }
    
}
