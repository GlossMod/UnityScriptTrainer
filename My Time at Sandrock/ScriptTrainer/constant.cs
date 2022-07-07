using Pathea.ActorNs;
using Pathea.FrameworkNs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptTrainer
{
    public static class constant
    {
        public static bool enduranceSwitch = false;     // 无限体力开关
        public static bool FallInjurySwitch = false;    // 掉落伤害开关
        public static string search = ""; // 搜索词
        public static int count = 1;  // 物品数量
        public static float version = 1.0f;  //当前版本

        public static bool isCheck = false; // 是否已检查过

        public static Actor actor = Module<ActorMgr>.Self.GetActor(8000);

    }
}
