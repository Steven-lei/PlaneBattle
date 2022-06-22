using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaneBattle
{
    internal class GameSetting
    {
        public static int screen_width { set; get; }
        public static int screen_height { set; get; }

        public static void InitGame(int screenwidth, int screenheight)
        {
            screen_width = screenwidth;
            screen_height = screenheight;
        }
    }
}
