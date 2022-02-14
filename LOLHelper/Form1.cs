using LOLHelper.Models;
using Microsoft.Data.Sqlite;
using opLib;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static LOLHelper.LOL_Client_Operation;
using static System.Windows.Forms.ListView;

namespace LOLHelper
{
    public partial class Form1 : Form
    {

        public static readonly IOpInterface Op_Global = new OpInterface();
        private readonly IOpInterface op_Client;
        private readonly IOpInterface op_Client_Operation;
        private readonly LOL_Client_Operation lOL_Client_Operation;

        public Form1()
        {
            InitializeComponent();
            op_Client = new OpInterface();
            op_Client_Operation = new OpInterface();

            if (Op_Global.FindWindow("RCLIENT", "League of Legends") != 0)
            {
                op_Client.BindWindow(Op_Global.FindWindow("RCLIENT", "League of Legends"), "gdi", "normal", "normal", 0);
                int hwndOne = Op_Global.FindWindow("RCLIENT", "League of Legends");
                int hwndTow = Op_Global.FindWindowEx(hwndOne, "CefBrowserWindow", "");
                int hwndThree = Op_Global.FindWindowEx(hwndTow, "Chrome_WidgetWin_0", "");
                op_Client_Operation.BindWindow(hwndThree, "normal", "windows", "windows", 0);

                lOL_Client_Operation = new LOL_Client_Operation(op_Client, op_Client_Operation);

                op_Client.SetDict(0, Environment.CurrentDirectory + "\\lol.dict");
                op_Client.SetDict(1, Environment.CurrentDirectory + "\\lolclientstatus.dict");
            }




        }

        public static IFreeSql Fsql = new FreeSql.FreeSqlBuilder()
                .UseConnectionString(FreeSql.DataType.Sqlite, $"Data Source={Environment.CurrentDirectory + "\\Data.db"};")
                .UseAutoSyncStructure(true)
                .Build();

        private void Form1_Load(object sender, EventArgs e)
        {

            dataGridView1.DataSource = Fsql.Select<HerosBPModel>().ToList();


            dataGridView1.Columns[0].Width = 40;
            dataGridView1.Columns[0].HeaderText = "ID";
            dataGridView1.Columns[1].HeaderText = "英雄名称";
            dataGridView1.Columns[2].HeaderText = "位置";
            dataGridView1.Columns[3].HeaderText = "排序";
            dataGridView1.Columns[4].HeaderText = "启用";
            dataGridView1.Columns[4].Width = 68; 

            this.listView1.Columns.Add("", 24, HorizontalAlignment.Left);
            this.listView1.Columns.Add("名称", 120, HorizontalAlignment.Left);

            listView1.SmallImageList = imageList1;
            listView1.StateImageList = imageList1;
            listView1.LargeImageList = imageList1;
            //imageList1.ImageSize = new Size(50,50);

            //this.listView1.BeginUpdate();   //数据更新，UI暂时挂起，直到EndUpdate绘制控件，可以有效避免闪烁并大大提高加载速度  
            foreach (var item in Fsql.Select<HerosModel>().ToList())
            {
                imageList1.Images.Add(item.Key, Image.FromFile($"{Environment.CurrentDirectory}\\imgs\\{item.Name}.png"));

                ListViewItem lvi = new ListViewItem
                {
                    ImageKey = item.Key     //通过与imageList绑定，显示imageList中第i项图标  
                };


                lvi.SubItems.Add(item.Name);
                this.listView1.Items.Add(lvi);
            }

            // this.listView1.EndUpdate();  //结束数据处理，UI界面一次性绘制。 
            //dataGridView1.DataSource = Fsql.Select<HerosBPModel>().ToList();
            #region 插入测试数据
            //await Fsql.Insert(new HerosModel
            //{
            //    Available = false,
            //    Name = "沙漠死神",
            //    Order = 100,
            //    Position = Position.上单,
            //}).ExecuteAffrowsAsync();

            //await Fsql.Insert(new HerosModel
            //{
            //    Available = false,
            //    Name = "牧魂人",
            //    Order = 100,
            //    Position = Position.上单,
            //}).ExecuteAffrowsAsync();

            //await Fsql.Insert(new HerosModel
            //{
            //    Available = false,
            //    Name = "雪原双子",
            //    Order = 99,
            //    Position = Position.打野,
            //}).ExecuteAffrowsAsync();
            //await Fsql.Insert(new HerosModel
            //{
            //    Available = false,
            //    Name = "盲僧",
            //    Order = 99,
            //    Position = Position.打野,
            //}).ExecuteAffrowsAsync();
            //await Fsql.Insert(new HerosModel
            //{
            //    Available = false,
            //    Name = "战争之影",
            //    Order = 98,
            //    Position = Position.打野,
            //}).ExecuteAffrowsAsync();


            //await Fsql.Insert(new HerosModel
            //{
            //    Available = false,
            //    Name = "远古巫灵",
            //    Order = 99,
            //    Position = Position.中单,

            //}).ExecuteAffrowsAsync();

            //await Fsql.Insert(new HerosModel
            //{
            //    Available = false,
            //    Name = "光辉女郎",
            //    Order = 100,
            //    Position = Position.中单,
            //}).ExecuteAffrowsAsync();


            //await Fsql.Insert(new HerosModel
            //{
            //    Available = false,
            //    Name = "寒冰射手",
            //    Order = 100,
            //    Position = Position.射手,
            //}).ExecuteAffrowsAsync();

            //await Fsql.Insert(new HerosModel
            //{
            //    Available = false,
            //    Name = "赏金猎人",
            //    Order = 100,
            //    Position = Position.射手,
            //}).ExecuteAffrowsAsync();

            //await Fsql.Insert(new HerosModel
            //{
            //    Available = false,
            //    Name = "远古巫灵",
            //    Order = 99,
            //    Position = Position.辅助,

            //}).ExecuteAffrowsAsync();

            //await Fsql.Insert(new HerosModel
            //{
            //    Available = false,
            //    Name = "光辉女郎",
            //    Order = 100,
            //    Position = Position.辅助,
            //}).ExecuteAffrowsAsync();

            //await Fsql.Insert(new HerosModel
            //{
            //    Available = false,
            //    Name = "迅捷斥候",
            //    Order = 100,
            //    Position = Position.辅助,
            //}).ExecuteAffrowsAsync();

            //await Fsql.Insert(new HerosModel
            //{
            //    Available = false,
            //    Name = "机械先驱",
            //    Order = 100,
            //    Position = Position.辅助,
            //}).ExecuteAffrowsAsync();

            #endregion
            //await lOL_Client_Operation.GameAsync();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var client = new RestClient("https://www.op.gg/api/meta/champions?hl=zh_CN");
            var request = new RestRequest();
            HeroOPGGModel response = await client.GetAsync<HeroOPGGModel>(request);

            foreach (var item in response.data)
            {

                try
                {
                    Console.WriteLine("image_url");
                    var stream = await new RestClient(item.image_url)
                    .DownloadStreamAsync(new RestRequest { Method = Method.Get });

                    string imgPath = $"{Environment.CurrentDirectory}\\imgs\\{item.name}.png";
                    Console.WriteLine(imgPath);
                    Image ResourceImage = Image.FromStream(stream);
                    ResourceImage.Save(imgPath);


                    await Fsql.Insert(new HerosModel
                    {
                        Name = item.name,
                        ImagePath = imgPath,
                        Key = item.key
                    }).ExecuteIdentityAsync();
                }
                catch (Exception EX)
                {
                }
            }

            Console.WriteLine();
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Console.WriteLine(sender);

            listView1.Items.Clear();

            foreach (var item in Fsql.Select<HerosModel>().Where(p => p.Name.Contains(((TextBox)sender).Text)).ToList())
            {
                imageList1.Images.Add(item.Key, Image.FromFile($"{Environment.CurrentDirectory}\\imgs\\{item.Name}.png"));

                ListViewItem lvi = new ListViewItem
                {
                    ImageKey = item.Key     //通过与imageList绑定，显示imageList中第i项图标  
                };


                lvi.SubItems.Add(item.Name);
                this.listView1.Items.Add(lvi);
            }
            Console.WriteLine();
        }

















        private void dataGridView1_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {

        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (((DataGridView)sender).SelectedCells.Count > 0)
            {
                var rowIndex = ((DataGridView)sender).SelectedCells[0].RowIndex;
                Fsql.Update<HerosBPModel>()
                    .Set(p => p.Order, Convert.ToInt32(dataGridView1.Rows[rowIndex].Cells[3].FormattedValue))
                    .Where(p => p.ID == Convert.ToInt32(dataGridView1.Rows[rowIndex].Cells[0].FormattedValue))
                    .ExecuteAffrows();

                dataGridView1.DataSource = Fsql.Select<HerosBPModel>().ToList();
            }



        }

        private void 删除_Click(object sender, EventArgs e)
        {
            var rowIndex = dataGridView1.SelectedCells[0].RowIndex;
            Fsql.Delete<HerosBPModel>()
                .Where(p => p.ID == Convert.ToInt32(dataGridView1.Rows[rowIndex].Cells[0].FormattedValue))
                .ExecuteAffrows();

            dataGridView1.DataSource = Fsql.Select<HerosBPModel>().ToList();
        }
        private void 添加为上单_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices != null && listView1.SelectedIndices.Count > 0)
            {
                foreach (ListViewItem item in listView1.SelectedItems)
                {
                    try
                    {
                        Fsql.Insert(new HerosBPModel
                        {
                            Name = item.SubItems[1].Text,
                            Position = Position.上单
                        }).ExecuteIdentity();
                    }
                    catch (Exception)
                    {
                    }
                    Console.WriteLine();
                }

            }
            dataGridView1.DataSource = Fsql.Select<HerosBPModel>().ToList();
        }
        private void 添加为打野_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices != null && listView1.SelectedIndices.Count > 0)
            {
                foreach (ListViewItem item in listView1.SelectedItems)
                {
                    try
                    {
                        Fsql.Insert(new HerosBPModel
                        {
                            Name = item.SubItems[1].Text,
                            Position = Position.打野
                        }).ExecuteIdentity();
                    }
                    catch (Exception)
                    {
                    }
                    Console.WriteLine();
                }

            }
            dataGridView1.DataSource = Fsql.Select<HerosBPModel>().ToList();
        }

        private void 添加为中单_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices != null && listView1.SelectedIndices.Count > 0)
            {
                foreach (ListViewItem item in listView1.SelectedItems)
                {
                    try
                    {
                        Fsql.Insert(new HerosBPModel
                        {
                            Name = item.SubItems[1].Text,
                            Position = Position.中单
                        }).ExecuteIdentity();
                    }
                    catch (Exception)
                    {
                    }
                    Console.WriteLine();
                }

            }
            dataGridView1.DataSource = Fsql.Select<HerosBPModel>().ToList();
        }

        private void 添加为辅助_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices != null && listView1.SelectedIndices.Count > 0)
            {
                foreach (ListViewItem item in listView1.SelectedItems)
                {
                    try
                    {
                        Fsql.Insert(new HerosBPModel
                        {
                            Name = item.SubItems[1].Text,
                            Position = Position.辅助
                        }).ExecuteIdentity();
                    }
                    catch (Exception)
                    {
                    }
                    Console.WriteLine();
                }

            }
            dataGridView1.DataSource = Fsql.Select<HerosBPModel>().ToList();
        }

        private void 添加为射手_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices != null && listView1.SelectedIndices.Count > 0)
            {
                foreach (ListViewItem item in listView1.SelectedItems)
                {
                    try
                    {
                        Fsql.Insert(new HerosBPModel
                        {
                            Name = item.SubItems[1].Text,
                            Position = Position.射手
                        }).ExecuteIdentity();
                    }
                    catch (Exception)
                    {
                    }
                    Console.WriteLine();
                }

            }
            dataGridView1.DataSource = Fsql.Select<HerosBPModel>().ToList();
        }
    }
}
