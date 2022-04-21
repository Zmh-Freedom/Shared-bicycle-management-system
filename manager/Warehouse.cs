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
namespace shareBike
{
    public partial class Warehouse : UIForm
    {
        private shareBike.managerForm father;
        public Warehouse(shareBike.managerForm myfather)
        {
            InitializeComponent();
            father = myfather;
            Date_Warehouse(father.dc.bike);
        }
        //搜索在仓库中的单车
        private void Date_Warehouse(IQueryable<bike> Bike)
        {
            int n = 0;
            IQueryable<bike> bikes = from x in Bike
                                     where x.flag == 5
                                     select x;
            uiDataGridView1.AutoGenerateColumns = false;
            uiDataGridView1.DataSource = bikes.ToList();
            uiDataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        //添加单车
        private void add_bike()
        {
            int n = (int)uiDoubleUpDown1.Value;
            for(int i=0; i < n; i++)
            {
                bike Bike = new bike() { flag = 5, total_time = 0 };
                father.dc.bike.InsertOnSubmit(Bike);
                father.dc.SubmitChanges();
            }
        }
        //添加序号
        private void uiDataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            var dgv = (DataGridView)sender;
            for (var i = 0; i < e.RowCount; i++)
            {
                dgv.Rows[e.RowIndex + i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Rows[e.RowIndex + i].HeaderCell.Value = (e.RowIndex + i + 1).ToString();
            }

            for (var i = e.RowIndex + e.RowCount; i < dgv.Rows.Count; i++)
            {
                dgv.Rows[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Rows[i].HeaderCell.Value = (i + 1).ToString();
            }
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            add_bike();
            Date_Warehouse(father.dc.bike);
        }
        //删除单车
        private void uiButton2_Click(object sender, EventArgs e)
        {
            List<int> list = new List<int>();
            for(int i=0;i<uiDataGridView1.RowCount;i++)
            {
                if(uiDataGridView1.Rows[i].Selected)
                {
                    list.Add((int)uiDataGridView1.Rows[i].Cells[0].Value);
                }
            }
            int a = uiDataGridView1.SelectedIndex;
            int b = (int)uiDataGridView1.Rows[a].Cells[0].Value;
            IQueryable<bike> bikes = from x in father.dc.bike
                                     where list.Contains(x.id)
                                     select x;
            father.dc.bike.DeleteAllOnSubmit(bikes);
            father.dc.SubmitChanges();
            Date_Warehouse(father.dc.bike);
        }

        private void Warehouse_FormClosing(object sender, FormClosingEventArgs e)
        {
            father.UpDateStoreLabel();
        }
    }
}
