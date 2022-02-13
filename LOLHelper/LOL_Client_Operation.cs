using LOLHelper.Models;
using opLib;
using Stateless;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
            Home,
            队列中,
            接受对局,
            表明英雄,
            禁用英雄,
            配置符文,
            选择英雄
        }
        public enum ClientTrigger
        {
            Home,
            BP开始,
            表明英雄,
            选择英雄,
            配置符文,
            BP结束
        }




        public StateMachine<ClientStatus, ClientTrigger> LoLAuto;
        private readonly IOpInterface op_Client;
        private readonly IOpInterface op_Client_Operation;

        private volatile bool op_Running = false;

        private Position position;
        private int severalLayers;

        public LOL_Client_Operation(
            IOpInterface Op_Client,
            IOpInterface Op_Client_Operation
            )
        {
            op_Client = Op_Client;
            op_Client_Operation = Op_Client_Operation;
            LoLAuto = new StateMachine<ClientStatus, ClientTrigger>(ClientStatus.Home);


            LoLAuto.Configure(ClientStatus.Home)
            .Permit(ClientTrigger.Home, ClientStatus.队列中);




            LoLAuto.Configure(ClientStatus.队列中).OnEntry(t =>
            {
                //League of Legends.exe
                Console.WriteLine("队列中...");
            })
             .Permit(ClientTrigger.BP开始, ClientStatus.表明英雄);




            #region 弃用
            //LoLAuto.Configure(ClientStatus.接受对局).OnEntryAsync(async t =>
            //{
            //    await Task.Run(async () =>
            //    {
            //        Console.WriteLine("点击接受对局");
            //        await Task.Delay(1000);

            //        //等待所有人接受对局方法（一直找接受对局原型标志，找不到则判断是否进入对局或者是未接收对局）
            //        while (op_Client.FindMultiColor(522, 265, 552, 290,
            //            "00537b-000000|c3983c-000000|c69f4b-000000",
            //            "00537b,1|16|c3983c,12|8|c69f4b", 0.9, 0, out object x, out object y) == 1)
            //        {
            //            if (!op_Running)
            //                return;

            //            await Task.Delay(300);
            //        }

            //        //如果返回大厅
            //        if (op_Client.FindMultiColor(902, 37, 935, 56,
            //             "005176-000000|0ac5e1-000000|010a13-000000",
            //             "005176,4|0|0ac5e1,5|0|010a13", 0.9, 0, out object x1, out object y1) == 1)
            //            await LoLAuto.FireAsync(ClientTrigger.BP结束);
            //        else
            //            await LoLAuto.FireAsync(ClientTrigger.表明英雄);


            //    });
            //})
            // .Permit(ClientTrigger.表明英雄, ClientStatus.表明英雄)
            // .Permit(ClientTrigger.BP结束, ClientStatus.等待大厅);
            #endregion






            LoLAuto.Configure(ClientStatus.表明英雄).OnEntryAsync(async t =>
            {
                await Task.Run(async () =>
                {
                    Console.WriteLine("开始表明英雄");

                    #region 位置判断
                    switch ((await op_Client.LoopFindStrAsync(op_Client_Operation, 73, 76, 155, 387, 0, "上单|打野|中单|射手|辅助", "F4BA0B-202020", 20, false, op_Running)).Index)
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

                    #region 判断楼层
                    severalLayers = SeveralLayersOfJudgment();
                    Console.WriteLine("当前楼层" + severalLayers);
                    #endregion
                    await ExpressHeroesAsync();


                });
            })
            .Permit(ClientTrigger.选择英雄, ClientStatus.禁用英雄)
            .Permit(ClientTrigger.BP结束, ClientStatus.Home);








            LoLAuto.Configure(ClientStatus.禁用英雄).OnEntryAsync(async t =>
            {
                await Task.Run(async () =>
                {
                    Console.WriteLine("开始禁用英雄");
                    await DisableHeroesAsync();


                });
            })
            .Permit(ClientTrigger.选择英雄, ClientStatus.选择英雄)
            .Permit(ClientTrigger.BP结束, ClientStatus.队列中);









            LoLAuto.Configure(ClientStatus.选择英雄).OnEntryAsync(async t =>
            {
                await Task.Run(async () =>
                {
                    Console.WriteLine("选择英雄");
                    await ChooseHeroesAsync();
                });
            })
             .Permit(ClientTrigger.配置符文, ClientStatus.配置符文)
             .Permit(ClientTrigger.BP结束, ClientStatus.队列中);







            LoLAuto.Configure(ClientStatus.配置符文).OnEntryAsync(async t =>
            {
                Console.WriteLine("配置符文");
            })
            .Permit(ClientTrigger.BP结束, ClientStatus.队列中);





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
                new Rectangle(1,  74, 7, 143),//一楼
                new Rectangle(1, 141, 7, 203),//二楼
                new Rectangle(1, 211, 7, 263),//二楼
                new Rectangle(1, 266, 7, 328),//四楼
                new Rectangle(1, 331, 7, 398),//五楼
            };

            var currRectangle = poss.FirstOrDefault(p =>
                op_Client.FindMultiColor(p.X, p.Y, p.Width, p.Height,
                "a3c7c7-000000|1e252a-000000|99babb-000000",
                "a3c7c7,7|5|1e252a,13|1|99babb", 1, 0, out object x, out object y) == 1
            );

            switch (currRectangle)
            {
                case Rectangle r when r.Y > 70:
                    return 1;
                case Rectangle r when r.Y > 140:
                    return 2;
                case Rectangle r when r.Y > 210:
                    return 3;
                case Rectangle r when r.Y > 340:
                    return 4;
                case Rectangle r when r.Y > 410:
                    return 5;
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

                var hero = await Form1.Fsql.Select<HerosModel>().Where(p => p.Available && p.Position == position).OrderByDescending(p => p.Order).FirstAsync();

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

                var hero = await Form1.Fsql.Select<HerosModel>().Where(p => p.Available && p.Position == position).OrderByDescending(p => p.Order).FirstAsync();
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
                     3, true, op_Running);



                //如果英雄处于被选或者禁用状态
                if (!ret.Result)
                {
                    await Form1.Fsql.Update<HerosModel>().Where(p => p == hero).Set(p => p.Available, false).ExecuteAffrowsAsync();
                    Console.WriteLine("英雄已被选择，换下个英雄！");
                    return await ChooseHeroesAsync();
                }
                else
                {
                    await Form1.Fsql.Update<HerosModel>().Where(p => !p.Available).Set(p => p.Available, true).ExecuteAffrowsAsync();
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

                var hero = await Form1.Fsql.Select<HerosModel>().Where(p => p.Available).OrderBy(p => p.Order).FirstAsync();
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
                     3, true, op_Running);



                //如果英雄处于被选或者禁用状态
                if (!ret.Result)
                {
                    await Form1.Fsql.Update<HerosModel>().Where(p => p == hero).Set(p => p.Available, false).ExecuteAffrowsAsync();
                    Console.WriteLine("英雄已被选择，换下个英雄！");
                    return await DisableHeroesAsync();
                }
                else
                {
                    return true;
                }
            });
        }



        public async Task GameAsync()
        {
            await Task.Factory.StartNew(async () =>
            {
                while (true)
                {

                    try
                    {
                        //如果游戏进程已打开那么久不执行后续操作
                        if (Process.GetProcessesByName("League of Legends").FirstOrDefault() != null)
                            continue;

                        #region 如果处于队列中
                        if (op_Client.FindMultiColor(902, 37, 935, 56,
                             "005176-000000|0ac5e1-000000|010a13-000000",
                             "005176,4|0|0ac5e1,5|0|010a13", 0.9, 0, out object x, out object y) == 1)
                        {
                            if (LoLAuto.State is ClientStatus.Home)
                            {
                                await LoLAuto.FireAsync(ClientTrigger.Home);
                            }
                            if (LoLAuto.State != ClientStatus.队列中 || LoLAuto.State is ClientStatus.配置符文)
                            {
                                await LoLAuto.FireAsync(ClientTrigger.BP结束);
                                op_Running = false;
                            }
                        }
                        #endregion


                        #region 判断是否开始对局
                        IsCanRecvMatchStart();
                        #endregion


                        #region 如果表明英雄
                        if (op_Client.FindStr(op_Client_Operation, 249, 1, 743, 93, 1, "表明英雄", "E9DFCC-202020", false).Result)
                        {
                            if (LoLAuto.State is ClientStatus.队列中)
                            {
                                op_Running = true;
                                await LoLAuto.FireAsync(ClientTrigger.BP开始);

                            }
                        }
                        #endregion

                        #region 如果禁用英雄
                        if (op_Client.FindStr(op_Client_Operation, 249, 1, 743, 93, 1, "禁用英雄", "E9DFCC-202020", false).Result)
                        {
                            if (LoLAuto.State is ClientStatus.表明英雄)
                            {
                                op_Running = true;
                                await LoLAuto.FireAsync(ClientTrigger.选择英雄);
                            }
                        }
                        #endregion

                        #region 如果选择英雄
                        if (op_Client.Ocr(417, 6, 588, 36, "E9DFCC-202020", 0.85).Contains("选择你的英雄"))
                        {
                            if (LoLAuto.State is ClientStatus.禁用英雄)
                            {
                                op_Running = true;
                                await LoLAuto.FireAsync(ClientTrigger.选择英雄);
                            }
                        }
                        #endregion

                        #region 如果配置符文
                        if (op_Client.Ocr(417, 6, 588, 36, "E9DFCC-202020", 0.85).Contains("选择你的符文"))
                        {
                            if (LoLAuto.State is ClientStatus.选择英雄)
                            {
                                op_Running = true;
                                await LoLAuto.FireAsync(ClientTrigger.配置符文);
                            }
                        }
                        #endregion

                        await Task.Delay(300);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            });
        }

    }

}
