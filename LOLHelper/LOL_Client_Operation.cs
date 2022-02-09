using opLib;
using Stateless;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LOLHelper
{
    public class LOL_Client_Operation
    {
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool PostMessageW(IntPtr hWnd, uint Msg, int wParam, IntPtr lParam);
        public class HeroesOptions
        {
            public List<Hero> FavoriteList { get; set; }
            public List<Hero> HateList { get; set; }
        }

        public enum ClientStatus
        {
            启动客户端,
            等待大厅,
            接受对局,
            表明英雄,
            禁用英雄,
            配置符文,
            选择英雄
        }
        public enum ClientTrigger
        {
            启动客户端,
            匹配已找到,
            表明英雄,
            选择英雄,
            配置符文,
            BP结束
        }




        public StateMachine<ClientStatus, ClientTrigger> LoLAuto;
        private readonly IOpInterface op_Client;
        private readonly IOpInterface op_Client_Operation;
        private readonly HeroesOptions heroesOptions;

        private Position position;

        public LOL_Client_Operation(
            IOpInterface Op_Client,
            IOpInterface Op_Client_Operation,
            HeroesOptions heroesOptions

            )
        {
            op_Client = Op_Client;
            op_Client_Operation = Op_Client_Operation;
            this.heroesOptions = heroesOptions;
            LoLAuto = new StateMachine<ClientStatus, ClientTrigger>(ClientStatus.启动客户端);


            LoLAuto.Configure(ClientStatus.启动客户端)
            .Permit(ClientTrigger.启动客户端, ClientStatus.等待大厅);




            LoLAuto.Configure(ClientStatus.等待大厅).OnEntryAsync(async t =>
            {
                await Task.Factory.StartNew(async () =>
                {
                    Console.WriteLine("等待对局开始");
                    while (IsCanRecvMatchStart() != 1)
                        await Task.Delay(100);
                    await LoLAuto.FireAsync(ClientTrigger.匹配已找到);
                });
            })
            .Permit(ClientTrigger.匹配已找到, ClientStatus.接受对局);



            LoLAuto.Configure(ClientStatus.接受对局).OnEntryAsync(async t =>
            {
                await Task.Factory.StartNew(async () =>
                {
                    Console.WriteLine("点击接受对局");
                    await Task.Delay(1000);

                    //等待所有人接受对局方法
                    while (op_Client.FindMultiColor(522, 265, 552, 290,
                        "00537b-000000|c3983c-000000|c69f4b-000000",
                        "00537b,1|16|c3983c,12|8|c69f4b", 1, 0, out object x, out object y) == 1)
                        await Task.Delay(300);

                    //如果返回大厅
                    if (op_Client.FindMultiColor(761, 34, 778, 51,
                        "010f1d-000000|0ac9e4-000000|0ac5e0-000000",
                        "010f1d,3|3|0ac9e4,4|10|0ac5e0", 1, 0, out object x1, out object y1) == 1)

                        await LoLAuto.FireAsync(ClientTrigger.BP结束);
                    else
                        await LoLAuto.FireAsync(ClientTrigger.表明英雄);


                });
            })
             .Permit(ClientTrigger.表明英雄, ClientStatus.表明英雄)
             .Permit(ClientTrigger.BP结束, ClientStatus.等待大厅);







            LoLAuto.Configure(ClientStatus.表明英雄).OnEntryAsync(async t =>
            {
                await Task.Factory.StartNew(async () =>
                {
                    Console.WriteLine("开始表明英雄");

                    #region 判断是否处于选择英雄状态
                    //判断是否进入BP成功
                    await Task.Delay(8000);
                    var ret = await op_Client.LoopFindStrAsync(op_Client_Operation, 249, 1, 743, 93, 1, "表明英雄", "E9DFCC-202020", 20, false);
                    if (!ret.Result)
                        await LoLAuto.FireAsync(ClientTrigger.BP结束);
                    #endregion

                    #region 位置判断
                    await Task.Delay(1000);
                    switch ((await op_Client.LoopFindStrAsync(op_Client_Operation, 73, 76, 155, 387, 0, "上单|打野|中单|射手|辅助", "F4BA0B-202020", 20, false)).Index)
                    {
                        case 0:
                            position = Position.上单;
                            break;
                        case 1:
                            position = Position.打野;
                            break;
                        case 2:
                            position = Position.中单;
                            break;
                        case 3:
                            position = Position.射手;
                            break;
                        case 4:
                            position = Position.辅助;
                            break;
                    }
                    #endregion

                    await ExpressHeroesAsync();
                    await Task.Delay(3000);
                    await LoLAuto.FireAsync(ClientTrigger.选择英雄);


                });
            })
            .Permit(ClientTrigger.选择英雄, ClientStatus.禁用英雄)
            .Permit(ClientTrigger.BP结束, ClientStatus.等待大厅);








            LoLAuto.Configure(ClientStatus.禁用英雄).OnEntryAsync(async t =>
            {
                await Task.Factory.StartNew(async () =>
                {
                    Console.WriteLine("开始禁用英雄");

                    var ret = await op_Client.LoopFindStrAsync(op_Client_Operation, 249, 1, 743, 93, 1, "禁用英雄", "E9DFCC-202020", 20, false);
                    if (!ret.Result)
                        await LoLAuto.FireAsync(ClientTrigger.BP结束);



                    await Task.Delay(1000);
                    if (await DisableHeroesAsync())
                        await LoLAuto.FireAsync(ClientTrigger.选择英雄);
                    else
                        await LoLAuto.FireAsync(ClientTrigger.BP结束);
                });
            })
            .Permit(ClientTrigger.选择英雄, ClientStatus.选择英雄)
            .Permit(ClientTrigger.BP结束, ClientStatus.等待大厅);









            LoLAuto.Configure(ClientStatus.选择英雄).OnEntryAsync(async t =>
            {
                await Task.Factory.StartNew(async () =>
                {
                    Console.WriteLine("选择英雄");

                    var ret = await op_Client.LoopFindStrAsync(op_Client_Operation, 249, 1, 743, 93, 1, "选择英雄", "E9DFCC-202020", 60, false);
                    if (!ret.Result)
                        await LoLAuto.FireAsync(ClientTrigger.BP结束);



                    await Task.Delay(1000);
                    if (await ChooseHeroesAsync())
                        await LoLAuto.FireAsync(ClientTrigger.选择英雄);
                    else
                        await LoLAuto.FireAsync(ClientTrigger.BP结束);
                });
            })
             .Permit(ClientTrigger.配置符文, ClientStatus.配置符文)
             .Permit(ClientTrigger.BP结束, ClientStatus.等待大厅);







            LoLAuto.Configure(ClientStatus.配置符文).OnEntryAsync(async t =>
            {
                await Task.Factory.StartNew(async () =>
                {
                    Console.WriteLine("配置符文");
                    await Task.Delay(1000);
                    await LoLAuto.FireAsync(ClientTrigger.BP结束);
                });
            })
            .Permit(ClientTrigger.BP结束, ClientStatus.等待大厅);





        }

        /// <summary>
        /// 是否开始对局
        /// </summary>
        /// <returns></returns>
        public int IsCanRecvMatchStart()
        {
            int ret = op_Client.FindMultiColor(505, 439, 533, 462,
                "a3c7c7-000000|1e252a-000000|99babb-000000",
                "a3c7c7,7|5|1e252a,13|1|99babb", 1, 0, out object x, out object y);

            if (ret == 1)
            {
                op_Client_Operation.MoveTo((int)x, (int)y);
                op_Client_Operation.LeftClick();
                op_Client_Operation.MoveTo(100, 100);
            }
            return ret;
        }

        /// <summary>
        /// 楼层判断
        /// </summary>
        /// <returns></returns>
        private int SeveralLayersOfJudgment()
        {
            Rectangle[] poss = new Rectangle[] {
                new Rectangle(1, 141, 7, 203),//二楼
                new Rectangle(0, 342, 8, 386),//四楼
            };

            var currRectangle = poss.FirstOrDefault(p =>
                op_Client.FindMultiColor(p.X, p.Y, p.Width, p.Height,
                "a3c7c7-000000|1e252a-000000|99babb-000000",
                "a3c7c7,7|5|1e252a,13|1|99babb", 1, 0, out object x, out object y) == 1
            );

            switch (currRectangle)
            {
                case Rectangle r when r.X > 0:
                    return 1;
                case Rectangle r when r.X > 0:
                    return 2;
                case Rectangle r when r.X > 0:
                    return 3;
                case Rectangle r when r.X > 0:
                    return 4;
            }
            return 0;
        }

        public async Task ExpressHeroesAsync()
        {
            await Task.Run(async () =>
            {
                op_Client_Operation.MoveTo(728, 80);
                op_Client_Operation.LeftClick();
                await Task.Delay(200);
                op_Client_Operation.MoveTo(729, 82);
                op_Client_Operation.LeftClick();
                await Task.Delay(200);

                int hwndOne = Form1.Op_Global.FindWindow("RCLIENT", "League of Legends");
                int hwndTow = Form1.Op_Global.FindWindowEx(hwndOne, "CefBrowserWindow", "");
                int hwndThree = Form1.Op_Global.FindWindowEx(hwndTow, "Chrome_WidgetWin_0", "");

                var hero = heroesOptions.FavoriteList.Where(p => p.Available && p.Position == position).OrderByDescending(p => p.Order).FirstOrDefault();

                Encoding.Unicode.GetChars(Encoding.Unicode.GetBytes(hero.Name))
                     .Select(p => p).ToList()
                     .ForEach(p => PostMessageW(new IntPtr(hwndThree), 0x0102, p, new IntPtr(0)));

                await Task.Delay(200);
                op_Client_Operation.MoveTo(308, 128);
                op_Client_Operation.LeftClick();
            });
        }

        public async Task<bool> ChooseHeroesAsync()
        {
            return await Task.Run(async () =>
            {
                op_Client_Operation.MoveTo(728, 80);
                op_Client_Operation.LeftClick();
                await Task.Delay(200);
                op_Client_Operation.MoveTo(729, 82);
                op_Client_Operation.LeftClick();
                await Task.Delay(200);

                int hwndOne = Form1.Op_Global.FindWindow("RCLIENT", "League of Legends");
                int hwndTow = Form1.Op_Global.FindWindowEx(hwndOne, "CefBrowserWindow", "");
                int hwndThree = Form1.Op_Global.FindWindowEx(hwndTow, "Chrome_WidgetWin_0", "");

                var hero = heroesOptions.FavoriteList.Where(p => p.Available && p.Position == position).OrderByDescending(p => p.Order).FirstOrDefault();
                if (hero == null)
                {
                    return false;
                }
                Encoding.Unicode.GetChars(Encoding.Unicode.GetBytes(hero.Name))
                     .Select(p => p).ToList()
                     .ForEach(p => PostMessageW(new IntPtr(hwndThree), 0x0102, p, new IntPtr(0)));

                await Task.Delay(200);
                op_Client_Operation.MoveTo(308, 128);
                op_Client_Operation.LeftClick();


                await Task.Delay(200);
                //判断是否可以选择英雄
                var ret = await op_Client.LoopFindColorAsync(op_Client_Operation,
                     514, 476, 531, 495,
                     "1e2328-000000|b0d6d8-000000|b2d9db-000000",
                     "1e2328,0|4|b0d6d8,2|7|b2d9db",
                     3, true);



                //如果英雄处于被选或者禁用状态
                if (!ret.Result)
                {
                    heroesOptions.FavoriteList.FirstOrDefault(p => p == hero).Available = false;
                    Console.WriteLine("英雄已被选择，换下个英雄！");
                    return await ChooseHeroesAsync();
                }
                else
                {
                    heroesOptions.FavoriteList.ForEach(p => p.Available = true);
                    return true;
                }
            });
        }

        public async Task<bool> DisableHeroesAsync()
        {
            return await Task.Run(async () =>
            {
                op_Client_Operation.MoveTo(728, 80);
                op_Client_Operation.LeftClick();
                await Task.Delay(200);
                op_Client_Operation.MoveTo(729, 82);
                op_Client_Operation.LeftClick();
                await Task.Delay(200);

                int hwndOne = Form1.Op_Global.FindWindow("RCLIENT", "League of Legends");
                int hwndTow = Form1.Op_Global.FindWindowEx(hwndOne, "CefBrowserWindow", "");
                int hwndThree = Form1.Op_Global.FindWindowEx(hwndTow, "Chrome_WidgetWin_0", "");

                var hero = heroesOptions.HateList.Where(p => p.Available).OrderByDescending(p => p.Order).FirstOrDefault();
                if (hero == null)
                {
                    return false;
                }
                Encoding.Unicode.GetChars(Encoding.Unicode.GetBytes(hero.Name))
                     .Select(p => p).ToList()
                     .ForEach(p => PostMessageW(new IntPtr(hwndThree), 0x0102, p, new IntPtr(0)));

                await Task.Delay(200);
                op_Client_Operation.MoveTo(308, 128);
                op_Client_Operation.LeftClick();


                await Task.Delay(200);
                //判断是否可以禁用英雄
                var ret = await op_Client.LoopFindColorAsync(op_Client_Operation,
                     498, 479, 533, 496,
                     "be1e37-000000|be1e37-000000|a51f35-000000",
                     "be1e37,0|3|be1e37,4|0|a51f35",
                     3, true);



                //如果英雄处于被选或者禁用状态
                if (!ret.Result)
                {
                    heroesOptions.HateList.FirstOrDefault(p => p == hero).Available = false;
                    Console.WriteLine("英雄已被选择，换下个英雄！");
                    return await DisableHeroesAsync();
                }
                else
                {
                    heroesOptions.HateList.ForEach(p => p.Available = true);
                    return true;
                }
            });
        }



        public async Task GameAsync()
        {
            await LoLAuto.FireAsync(ClientTrigger.启动客户端);
        }

    }

}
