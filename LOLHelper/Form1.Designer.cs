namespace LOLHelper
{
    partial class Form1
    {
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
            this.button1 = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.选项 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.添加为上单 = new System.Windows.Forms.ToolStripMenuItem();
            this.添加为打野 = new System.Windows.Forms.ToolStripMenuItem();
            this.添加为中单 = new System.Windows.Forms.ToolStripMenuItem();
            this.添加为辅助 = new System.Windows.Forms.ToolStripMenuItem();
            this.添加为射手 = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.BP表格右键菜单 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.删除 = new System.Windows.Forms.ToolStripMenuItem();
            this.skillShow1 = new LOLHelper.SkillShow();
            this.选项.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.BP表格右键菜单.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 454);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(156, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "初始化英雄数据（OPGG）";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listView1
            // 
            this.listView1.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.listView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listView1.ContextMenuStrip = this.选项;
            this.listView1.FullRowSelect = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(17, 39);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(122, 207);
            this.listView1.TabIndex = 4;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // 选项
            // 
            this.选项.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加为上单,
            this.添加为打野,
            this.添加为中单,
            this.添加为辅助,
            this.添加为射手});
            this.选项.Name = "contextMenuStrip1";
            this.选项.Size = new System.Drawing.Size(137, 114);
            this.选项.Text = "选项";
            // 
            // 添加为上单
            // 
            this.添加为上单.Name = "添加为上单";
            this.添加为上单.Size = new System.Drawing.Size(136, 22);
            this.添加为上单.Text = "添加为上单";
            this.添加为上单.Click += new System.EventHandler(this.添加为上单_Click);
            // 
            // 添加为打野
            // 
            this.添加为打野.Name = "添加为打野";
            this.添加为打野.Size = new System.Drawing.Size(136, 22);
            this.添加为打野.Text = "添加为打野";
            this.添加为打野.Click += new System.EventHandler(this.添加为打野_Click);
            // 
            // 添加为中单
            // 
            this.添加为中单.Name = "添加为中单";
            this.添加为中单.Size = new System.Drawing.Size(136, 22);
            this.添加为中单.Text = "添加为中单";
            this.添加为中单.Click += new System.EventHandler(this.添加为中单_Click);
            // 
            // 添加为辅助
            // 
            this.添加为辅助.Name = "添加为辅助";
            this.添加为辅助.Size = new System.Drawing.Size(136, 22);
            this.添加为辅助.Text = "添加为辅助";
            this.添加为辅助.Click += new System.EventHandler(this.添加为辅助_Click);
            // 
            // 添加为射手
            // 
            this.添加为射手.Name = "添加为射手";
            this.添加为射手.Size = new System.Drawing.Size(136, 22);
            this.添加为射手.Text = "添加为射手";
            this.添加为射手.Click += new System.EventHandler(this.添加为射手_Click);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(17, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(122, 21);
            this.textBox1.TabIndex = 6;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ContextMenuStrip = this.BP表格右键菜单;
            this.dataGridView1.Location = new System.Drawing.Point(145, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(428, 234);
            this.dataGridView1.TabIndex = 9;
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            this.dataGridView1.RowStateChanged += new System.Windows.Forms.DataGridViewRowStateChangedEventHandler(this.dataGridView1_RowStateChanged);
            this.dataGridView1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseClick);
            // 
            // BP表格右键菜单
            // 
            this.BP表格右键菜单.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.删除});
            this.BP表格右键菜单.Name = "contextMenuStrip1";
            this.BP表格右键菜单.Size = new System.Drawing.Size(101, 26);
            this.BP表格右键菜单.Text = "contextMenuStrip1";
            // 
            // 删除
            // 
            this.删除.Name = "删除";
            this.删除.Size = new System.Drawing.Size(100, 22);
            this.删除.Text = "删除";
            this.删除.Click += new System.EventHandler(this.删除_Click);
            // 
            // skillShow1
            // 
            this.skillShow1.Location = new System.Drawing.Point(17, 252);
            this.skillShow1.Name = "skillShow1";
            this.skillShow1.Size = new System.Drawing.Size(106, 75);
            this.skillShow1.TabIndex = 10;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(588, 349);
            this.Controls.Add(this.skillShow1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "英雄联盟自动BP 换符文";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.选项.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.BP表格右键菜单.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ContextMenuStrip 选项;
        private System.Windows.Forms.ToolStripMenuItem 添加为上单;
        private System.Windows.Forms.ToolStripMenuItem 添加为打野;
        private System.Windows.Forms.ToolStripMenuItem 添加为中单;
        private System.Windows.Forms.ToolStripMenuItem 添加为辅助;
        private System.Windows.Forms.ToolStripMenuItem 添加为射手;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ContextMenuStrip BP表格右键菜单;
        private System.Windows.Forms.ToolStripMenuItem 删除;
        private SkillShow skillShow1;
    }
}

