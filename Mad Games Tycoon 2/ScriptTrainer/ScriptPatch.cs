using HarmonyLib;
using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace ScriptTrainer;


public class ScriptPatch
{
    #region[全局参数]
    public static float workEfficiency = 100f;
    public static float setingsHostSalesRatio = 1f;
    public static bool settingsWorkEfficiency = false;
    public static bool settingsNoSalary = false;
    public static bool setingsAllKnow = false;
    // public static bool setingsMaxPopularity = false;
    public static float setingsMaxHype1 = 1f;
    public static float setingsMaxHype2 = 1f;
    public static bool setingsInfiniteSpace = false;
    public static bool setingsInfiniteServerplatz = false;
    public static bool setingsAcquisitionCompany = false;
    #endregion


    [HarmonyPostfix]
    [HarmonyPatch(typeof(characterScript), nameof(characterScript.AddMotivation))]
    public static void AddMotivation(characterScript __instance)
    {
        // 修改工作效率
        if (settingsWorkEfficiency)
        {
            __instance.s_motivation = workEfficiency;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(characterScript), nameof(characterScript.GetGehalt))]
    public static void GetGehalt(ref int __result)
    {
        // 不发薪水
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

    // [HarmonyPostfix]
    // [HarmonyPatch(typeof(genres), nameof(genres.GetFloatBeliebtheit))]
    // public static void GetFloatBeliebtheit(ref float __result)
    // {
    //     if (setingsMaxPopularity) __result = 100f;
    // }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(gameScript), nameof(gameScript.GetHype))]
    public static void GGetHype(ref float __result, gameScript __instance)
    {
        // 游戏热度最高
        if (Mathf.RoundToInt(setingsMaxHype1) != 1 && __instance.IsMyGame())
        {
            __instance.hype = setingsMaxHype1;
            __result = setingsMaxHype1;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(platformScript), "GetHype")]
    public static void PGetHype(ref float __result, platformScript __instance)
    {
        // 主机热度最高
        if (Mathf.RoundToInt(setingsMaxHype2) != 1 && !__instance.OwnerIsNPC())
        {
            __instance.hype = setingsMaxHype2;
            __result = setingsMaxHype2;
        }

        if (!__instance.OwnerIsNPC() && Mathf.RoundToInt(setingsHostSalesRatio) != 1)
        {
            // 控制主机销量
            __instance.performancePoints = Mathf.RoundToInt(setingsHostSalesRatio);
            Debug.Log(__instance.performancePoints);
        }
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

    // [HarmonyPostfix]
    // [HarmonyPatch(typeof(publisherScript), nameof(publisherScript.GetFirmenwert))]
    // public static void GetMoneyExklusiv(ref long __result, publisherScript __instance)
    // {
    //     // 免费收购公司
    //     if (setingsAcquisitionCompany)
    //     {

    //         __instance.lockToBuy = 0;
    //         __result = 100;
    //     }
    // }

}
