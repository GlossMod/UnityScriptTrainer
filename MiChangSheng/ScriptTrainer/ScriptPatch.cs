using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ScriptTrainer
{
    // 补丁注入相关
    public static class ScriptPatch
    {
        #region[全局参数]
        public static bool settingsWorkEfficiency = false;
        public static bool settingsNoSalary = false;
        public static bool setingsAllKnow = false;
        public static bool setingsMaxPopularity = false;
        public static bool setingsMaxHype = false;
        public static bool setingsInfiniteSpace = false;
        public static bool setingsInfiniteServerplatz = false;
        #endregion

        
    }
}
