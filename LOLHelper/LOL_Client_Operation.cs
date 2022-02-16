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


        private readonly IOpInterface op_Client;
        private readonly IOpInterface op_Client_Operation;

        private volatile bool op_Running = false;
        private Position position;

        public LOL_Client_Operation(
            IOpInterface Op_Client,
            IOpInterface Op_Client_Operation
            )
        {
            op_Client = Op_Client;
            op_Client_Operation = Op_Client_Operation;
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
        /// 表明英雄
        /// </summary>
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

                var hero = await Form1.Fsql.Select<HerosBPModel>().Where(p => p.Available && p.Position == position).OrderByDescending(p => p.Order).FirstAsync();

                Encoding.Unicode.GetChars(Encoding.Unicode.GetBytes(hero.Name))
                     .Select(p => p).ToList()
                     .ForEach(p => PostMessageW(new IntPtr(hwndThree), 0x0102, p, new IntPtr(0)));

                await Task.Delay(200);
                op_Client_Operation.MoveTo(308, 128);
                op_Client_Operation.LeftClick();
            });
        }
        
        /// <summary>
        /// 选择英雄
        /// </summary>
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

                var hero = await Form1.Fsql.Select<HerosBPModel>().Where(p => p.Available && p.Position == position).OrderByDescending(p => p.Order).FirstAsync();
                Console.WriteLine(hero.Name);
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
                    await Form1.Fsql.Update<HerosBPModel>().Where(p => p == hero).Set(p => p.Available, false).ExecuteAffrowsAsync();
                    Console.WriteLine("英雄已被选择，换下个英雄！");
                    return await ChooseHeroesAsync();
                }
                else
                {
                    await Form1.Fsql.Update<HerosBPModel>().Where(p => !p.Available).Set(p => p.Available, true).ExecuteAffrowsAsync();
                    return true;
                }
            });
        }

        /// <summary>
        /// 禁用英雄
        /// </summary>
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

                var hero = await Form1.Fsql.Select<HerosBPModel>().Where(p => p.Available).OrderBy(p => p.Order).FirstAsync();
                Console.WriteLine(hero.Name);
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
                    await Form1.Fsql.Update<HerosBPModel>().Where(p => p == hero).Set(p => p.Available, false).ExecuteAffrowsAsync();
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
            //检测对局是否开始
            _ = Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    if (Process.GetProcessesByName("League of Legends").FirstOrDefault() != null)
                        continue;

                    if (IsCanRecvMatchStart() == 1)
                    {
                        op_Running = false;
                        await Task.Delay(3000);
                    }
                    await Task.Delay(10);
                }
            });

            //BP状态检测
            await Task.Factory.StartNew(async () =>
            {
                while (true)
                {

                    try
                    {
                        //如果游戏进程已打开那么久不执行后续操作
                        if (Process.GetProcessesByName("League of Legends").FirstOrDefault() != null)
                            continue;

                        #region 如果表明英雄
                        if (op_Client.FindStr(op_Client_Operation, 249, 1, 743, 93, 1, "表明英雄", "E9DFCC-202020", false).Result)
                        {
                            Console.WriteLine("开始表明英雄");
                            op_Running = true;

                            #region 位置楼层判断
                            var ret = await op_Client.LoopFindStrAsync(op_Client_Operation, 73, 76, 155, 387, 0, "上单|打野|中单|射手|辅助", "F4BA0B-202020", 20, false, op_Running);
                            switch (ret.Index)
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
                            var severalLayers = -1;
                            switch (ret.Pos.Value.Y)
                            {
                                case int y when y > 340:
                                    severalLayers = 5;
                                    break;
                                case int y when y > 210:
                                    severalLayers = 4;
                                    break;
                                case int y when y > 140:
                                    severalLayers = 3;
                                    break;
                                case int y when y > 70:
                                    severalLayers = 2;
                                    break;
                                case int y when y > 0:
                                    severalLayers = 1;
                                    break;
                            }
                            Console.WriteLine($"当前位置 {position} 处于 {severalLayers} 楼");
                            _ = Form1.Fsql.Insert(new SeveralLayersRecordModel
                            {
                                SeveralLayers = severalLayers,
                                Position = position,
                                Time = DateTime.Now
                            }).ExecuteAffrowsAsync();
                            #endregion
                            await ExpressHeroesAsync();
                        }
                        #endregion

                        #region 如果禁用英雄
                        if (op_Client.FindStr(op_Client_Operation, 249, 1, 743, 93, 1, "禁用英雄", "E9DFCC-202020", false).Result)
                        {

                            Console.WriteLine("开始禁用英雄");
                            op_Running = true;
                            await DisableHeroesAsync();

                        }
                        #endregion

                        #region 如果选择英雄
                        if (op_Client.Ocr(417, 6, 588, 36, "E9DFCC-202020", 0.85).Contains("选择你的英雄"))
                        {
                            Console.WriteLine("选择英雄");
                            op_Running = true;
                            await ChooseHeroesAsync();

                        }
                        #endregion

                        #region 如果配置符文
                        if (op_Client.Ocr(417, 6, 588, 36, "E9DFCC-202020", 0.85).Contains("选择你的符文"))
                        {
                            op_Running = true;
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
