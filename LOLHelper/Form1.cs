using LOLHelper.Models;
using opLib;
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

            if (Op_Global.FindWindow("RCLIENT", "League of Legends") == 0)
            {
                MessageBox.Show("未找到游戏窗口！");
                Environment.Exit(0);
            }

            op_Client.BindWindow(Op_Global.FindWindow("RCLIENT", "League of Legends"), "gdi", "normal", "normal", 0);
            int hwndOne = Op_Global.FindWindow("RCLIENT", "League of Legends");
            int hwndTow = Op_Global.FindWindowEx(hwndOne, "CefBrowserWindow", "");
            int hwndThree = Op_Global.FindWindowEx(hwndTow, "Chrome_WidgetWin_0", "");
            op_Client_Operation.BindWindow(hwndThree, "normal", "windows", "windows", 0);

            lOL_Client_Operation = new LOL_Client_Operation(op_Client, op_Client_Operation);

            op_Client.SetDict(0, Environment.CurrentDirectory + "\\lol.dict");
            op_Client.SetDict(1, Environment.CurrentDirectory + "\\lolclientstatus.dict");


        }

        public static IFreeSql Fsql = new FreeSql.FreeSqlBuilder()
                .UseConnectionString(FreeSql.DataType.Sqlite, $"Data Source={Environment.CurrentDirectory + "\\Data.db"};")
                .UseAutoSyncStructure(true)
                .Build();

        private async void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = Fsql.Select<HerosModel>().ToList();
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
            await lOL_Client_Operation.GameAsync();
        }

    }
}
