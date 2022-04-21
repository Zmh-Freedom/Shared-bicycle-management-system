using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using shareBike;
namespace shareBike
{
    public partial class taskManage : Form
    {
        private shareBike.managerForm father;
        public taskManage(shareBike.managerForm myfather)
        {
            InitializeComponent();
            father = myfather;
            updateDataSrc();            
        }
        private void updateDataSrc()
        {
            IQueryable<task> taskToDo = from p in father.dc.task
                                        where p.flag == 0
                                        select p;
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = taskToDo.ToList();
        }

        //响应"删除任务"按钮
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int amount = dataGridView1.SelectedRows.Count;
                if (amount > 0)
                {
                    int i = 0;
                    while (i < amount)
                    {
                        task toDelete = (from u in father.dc.task
                                       where u.id == (int)dataGridView1.SelectedRows[i].Cells["ColumnID"].Value
                                       select u).First();
                        
                        if(toDelete.tag == 3)
                        {
                            toDelete.bike.flag = 5;
                        }
                        else
                        {
                            toDelete.bike.flag = 0;
                        }
                        father.dc.SubmitChanges();
                        father.dc.task.DeleteOnSubmit(toDelete);
                        i++;
                    }
                    father.dc.SubmitChanges();
                    updateDataSrc();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void taskManage_FormClosing(object sender, FormClosingEventArgs e)
        {
            father.repaintTaskDonwnMap();
            father.UpDateStoreLabel();
        }
    }
}
