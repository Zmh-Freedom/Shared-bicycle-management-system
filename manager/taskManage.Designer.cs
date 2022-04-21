
namespace shareBike
{
    partial class taskManage
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.taskBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ColumnID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnTag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnTIME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSrc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnEX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnEY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnBid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.taskBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnID,
            this.ColumnTag,
            this.ColumnTIME,
            this.ColumnSrc,
            this.ColumnEX,
            this.ColumnEY,
            this.ColumnBid});
            this.dataGridView1.Location = new System.Drawing.Point(21, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 62;
            this.dataGridView1.Size = new System.Drawing.Size(816, 396);
            this.dataGridView1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(364, 414);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(135, 53);
            this.button1.TabIndex = 1;
            this.button1.Text = "删除所选任务";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // taskBindingSource
            // 
            this.taskBindingSource.DataSource = typeof(shareBike.task);
            // 
            // ColumnID
            // 
            this.ColumnID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.ColumnID.DataPropertyName = "id";
            this.ColumnID.HeaderText = "任务id";
            this.ColumnID.MinimumWidth = 8;
            this.ColumnID.Name = "ColumnID";
            this.ColumnID.Width = 98;
            // 
            // ColumnTag
            // 
            this.ColumnTag.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.ColumnTag.DataPropertyName = "tag";
            this.ColumnTag.HeaderText = "类型";
            this.ColumnTag.MinimumWidth = 8;
            this.ColumnTag.Name = "ColumnTag";
            this.ColumnTag.Width = 80;
            // 
            // ColumnTIME
            // 
            this.ColumnTIME.DataPropertyName = "start_time";
            this.ColumnTIME.HeaderText = "创建时间";
            this.ColumnTIME.MinimumWidth = 8;
            this.ColumnTIME.Name = "ColumnTIME";
            this.ColumnTIME.Width = 116;
            // 
            // ColumnSrc
            // 
            this.ColumnSrc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.ColumnSrc.DataPropertyName = "source";
            this.ColumnSrc.HeaderText = "来源";
            this.ColumnSrc.MinimumWidth = 8;
            this.ColumnSrc.Name = "ColumnSrc";
            this.ColumnSrc.Width = 80;
            // 
            // ColumnEX
            // 
            this.ColumnEX.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.ColumnEX.DataPropertyName = "end_x";
            this.ColumnEX.HeaderText = "终点X";
            this.ColumnEX.MinimumWidth = 8;
            this.ColumnEX.Name = "ColumnEX";
            this.ColumnEX.Width = 89;
            // 
            // ColumnEY
            // 
            this.ColumnEY.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.ColumnEY.DataPropertyName = "end_y";
            this.ColumnEY.HeaderText = "终点Y";
            this.ColumnEY.MinimumWidth = 8;
            this.ColumnEY.Name = "ColumnEY";
            this.ColumnEY.Width = 89;
            // 
            // ColumnBid
            // 
            this.ColumnBid.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.ColumnBid.DataPropertyName = "bid";
            this.ColumnBid.HeaderText = "单车id";
            this.ColumnBid.MinimumWidth = 8;
            this.ColumnBid.Name = "ColumnBid";
            this.ColumnBid.Width = 98;
            // 
            // taskManage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(864, 505);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "taskManage";
            this.Text = "管理尚未执行的任务";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.taskManage_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.taskBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.BindingSource taskBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTag;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTIME;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSrc;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnEX;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnEY;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnBid;
    }
}