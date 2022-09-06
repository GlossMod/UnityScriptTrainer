using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ScriptTrainer
{
    public  class ScriptPatch
    {
        #region[全局参数]
        public static float workEfficiency = 100f;
        public static bool settingsWorkEfficiency = false;
        public static bool settingsNoSalary = false;
        #endregion

        //[HarmonyPostfix]
        //[HarmonyPatch(typeof(characterScript), "GetWorkSpeed")]
        //public static void MyGetWorkSpeed(ref float __result)
        //{
        //    __result *= workEfficiency;

        //    //return true;
        //}        

        [HarmonyPostfix]
        [HarmonyPatch(typeof(characterScript), "AddMotivation")]
        public static void AddMotivation(characterScript __instance)
        {
            if (settingsWorkEfficiency)
            {
                __instance.s_motivation = workEfficiency;
            }
        }
        
    }
}
