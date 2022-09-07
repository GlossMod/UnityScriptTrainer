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
        public static bool setingsMaxPopularity = false;
        public static bool setingsMaxHype = false;
        public static bool setingsInfiniteSpace = false;
        public static bool setingsInfiniteServerplatz = false;
        #endregion


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

        [HarmonyPostfix]
        [HarmonyPatch(typeof(genres), nameof(genres.GetAlignKnown))]
        public static void GetAlignKnown(ref bool __result)
        {
            // 已知所有设计方向
            if (setingsAllKnow) __result = true;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(genres), nameof(genres.GetFocusKnown))]
        public static void GetFocusKnown(ref bool __result)
        {
            // 已知所有设计重点
            if (setingsAllKnow) __result = true;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(genres), nameof(genres.GetFloatBeliebtheit))]
        public static void GetFloatBeliebtheit(ref float __result)
        {
            if (setingsMaxPopularity) __result = 100f;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(gameScript), nameof(gameScript.AddHype))]
        public static void AddHype(gameScript __instance)
        {
            if (setingsMaxHype) __instance.hype = 300f;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(roomScript), nameof(roomScript.UpdateListInventar))]
        public static void UpdateListInventar(roomScript __instance)
        {
            if (setingsInfiniteSpace)
            {
                // 使用 Traverse 来访问私有字段 并修改它的值
                //__instance.lagerplatz = 999999;
                Traverse.Create(__instance).Field("lagerplatz").SetValue(999999999);
            }
            // 同理修改服务器的
            if (setingsInfiniteServerplatz) Traverse.Create(__instance).Field("serverplatz").SetValue(999999999);
        }
    }
}
