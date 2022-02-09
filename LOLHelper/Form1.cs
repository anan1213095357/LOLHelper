using LOLHelper.Models;
using opLib;
using System;
using System.Collections.Generic;
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

            lOL_Client_Operation = new LOL_Client_Operation(op_Client, op_Client_Operation, new HeroesOptions
            {
                FavoriteList = new List<Hero>()
                {
                    new Hero { Name = "雪原双子", Order = 99 ,Position = Position.打野},
                    new Hero { Name = "远古巫灵", Order = 99 ,Position = Position.辅助},
                    new Hero { Name = "远古巫灵", Order = 99 ,Position = Position.辅助},
                    new Hero { Name = "远古巫灵", Order = 99 ,Position = Position.中单},
                    new Hero { Name = "发条魔灵", Order = 98 ,Position = Position.辅助},
                    new Hero { Name = "复仇烈焰", Order = 97 ,Position = Position.辅助},
                    new Hero { Name = "复仇烈焰", Order = 97 ,Position = Position.中单},
                },
                HateList = new List<Hero>()
                {
                    new Hero { Name = "无极剑圣", Order = 99 },
                    new Hero { Name = "永恒梦魇", Order = 98 },
                    new Hero { Name = "虚空掠夺者", Order = 97 },
                    new Hero { Name = "无双剑姬", Order = 96 },
                    new Hero { Name = "破败之王", Order = 95 },
                    new Hero { Name = "疾风剑豪", Order = 94 },
                }
            });

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
            var r = await Fsql.Insert(new HerosModel
            {
                Available = false,
                Name = "远古巫灵",
                Order = 99,
                Position = Position.辅助,
                Skills = new SkillModel[] {
                    new SkillModel
                    {
                        Skill_D = SkillEnum.闪现,
                        Skill_F = SkillEnum.引燃
                    },
                    new SkillModel
                    {
                        Skill_D = SkillEnum.闪现,
                        Skill_F = SkillEnum.屏障
                    }
                }
            }).ExecuteAffrowsAsync();

            Console.WriteLine(r);

            await lOL_Client_Operation.GameAsync();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await lOL_Client_Operation.DisableHeroesAsync();
        }
    }
}
