using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptTrainer
{
    public static class Debug
    {
        public static void Log(string msg)
        {
            UIPopTip.Inst.Pop(msg, PopTipIconType.叹号);
        }
        public static void Pop(string msg)
        {
            UIPopTip.Inst.Pop(msg, PopTipIconType.上箭头);
        }
    }
}
