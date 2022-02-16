using opLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LOLHelper
{

    public static class Utility
    {
        public class FindColorResult
        {
            public bool Result { get; set; }
            public Point? Pos { get; set; }
        }

        public class FindStrResult
        {
            public bool Result { get; set; }
            public int Index { get; set; }
            public Point? Pos { get; set; }
        }

        public static async Task<FindColorResult> LoopFindColorAsync(this IOpInterface op, IOpInterface opAction,
            int x, int y, int width, int height, string f, string o, int time, bool isClick, bool opRuning)
        {
            return await Task.Run(async () =>
            {
                int tempTime = 0;
                object posx, posy;
                while (op.FindMultiColor(x, y, width, height, f, o, 1, 0, out posx, out posy) != 1)
                {
                    if (tempTime++ > time || !opRuning)
                    {
                        return new FindColorResult { Result = false };
                    }
                    await Task.Delay(1000);
                }
                if (isClick)
                {
                    opAction.MoveTo((int)posx, (int)posy);
                    opAction.LeftClick();
                }
                return new FindColorResult { Result = true, Pos = new Point((int)posx, (int)posy) };
            });


        }



        public static async Task<FindStrResult> LoopFindStrAsync(this IOpInterface op, IOpInterface opAction,
            int x, int y, int width, int height, int dictIndex, string str, string color, int time, bool isClick, bool opRuning)
        {
            return await Task.Run(async () =>
            {
                op.UseDict(dictIndex);
                int tempTime = 0;
                object retx = -1, rety = -1;
                int findIndex = -1;
                while (findIndex == -1)
                {
                    findIndex = op.FindStr(x, y, width, height, str, color, 0.85, out retx, out rety);
                    if (tempTime++ > time || !opRuning)
                    {
                        return new FindStrResult { Result = false, Index = -1 };
                    }
                    Console.WriteLine(str + ":" + findIndex);
                    await Task.Delay(1000);
                }
                if (isClick)
                {
                    opAction.MoveTo((int)retx, (int)rety);
                    opAction.LeftClick();
                }
                return new FindStrResult { Result = true, Index = findIndex ,Pos = new Point((int)retx, (int)rety) };
            });


        }


        public static FindStrResult FindStr(this IOpInterface op, IOpInterface opAction,
            int x, int y, int width, int height, int dictIndex, string str, string color, bool isClick)
        {
            op.UseDict(dictIndex);
            object retx, rety;
            int findIndex;
            findIndex = op.FindStr(x, y, width, height, str, color, 0.85, out retx, out rety);
            if (isClick)
            {
                opAction.MoveTo((int)retx, (int)rety);
                opAction.LeftClick();
            }
            if (findIndex >= 0)
                return new FindStrResult { Result = true, Index = findIndex };
            else
                return new FindStrResult { Result = false };

        }


    }
}
