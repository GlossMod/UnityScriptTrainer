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
        public static bool setingsAllKnow = false;
        #endregion

        //[HarmonyPostfix]
        //[HarmonyPatch(typeof(characterScript), "GetWorkSpeed")]
        //public static void GetWorkSpeed(ref float __result)
        //{
        //    __result *= workEfficiency;

        //    return true;
        //}

        [HarmonyPostfix]
        [HarmonyPatch(typeof(characterScript), nameof(characterScript.AddMotivation))]
        public static void AddMotivation(characterScript __instance)
        {
            if (settingsWorkEfficiency)
            {
                __instance.s_motivation = workEfficiency;
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(characterScript), nameof(characterScript.GetGehalt))]
        public static void GetGehalt(ref int __result)
        {
            if (settingsNoSalary) __result = 0;
        }

        // 已知设计方向
        [HarmonyPostfix]
        [HarmonyPatch(typeof(genres), nameof(genres.GetAlignKnown))]
        public static void GetAlignKnown(ref bool __result)
        {
            if (setingsAllKnow) __result = true;
        }

        // 已知设计重点
        [HarmonyPostfix]
        [HarmonyPatch(typeof(genres), nameof(genres.GetFocusKnown))]
        public static void GetFocusKnown(ref bool __result)
        {
            if (setingsAllKnow) __result = true;
        }



        //[HarmonyPostfix]
        //[HarmonyPatch(typeof(genres), nameof(genres.GetFocusKnown))]
        //public static bool GetFocusKnown(ref bool __result)
        //{
        //    if (setingsAllKnow) __result = true;
        //    return __result;
        //}

    }
}
