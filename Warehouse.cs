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
namespace shareDemo2
{
    public partial class Warehouse : UIForm
    {
        private shareDemo3.managerForm father;
        public Warehouse(shareDemo3.managerForm myfather)
        {
            InitializeComponent();
            father = myfather;
            Date_Warehouse(father.dc.bike);
        }
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
        private void add_bike()
        {
            bike Bike = new bike() { flag = 5, total_time = 0 };
            father.dc.bike.InsertOnSubmit(Bike);
            father.dc.SubmitChanges();
        }
        private void uiDataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            e.Equals(e.RowIndex);
            int a = e.ColumnIndex;
            int b = (int)uiDataGridView1.Rows[a].Cells[0].Value;
        }

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

        private void uiButton2_Click(object sender, EventArgs e)
        {
            int a = uiDataGridView1.SelectedIndex;
            int b = (int)uiDataGridView1.Rows[a].Cells[0].Value;
            IQueryable<bike> bikes = from x in father.dc.bike
                                     where x.id == b
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
