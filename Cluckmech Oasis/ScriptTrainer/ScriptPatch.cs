using HarmonyLib;
using UnityEngine;

namespace ScriptTrainer;

internal class ScriptPatch
{
    // 无限CD
    public static bool settingsNoCD = false;
    // 无需动力
    public static bool settingsNoMotivation = false;

    // 模块_玩家_建造.背包cd初始化
    [HarmonyPostfix]
    [HarmonyPatch(typeof(模块_玩家_建造), "Update")]
    public static void AddMotivation(模块_玩家_建造 __instance)
    {
        // 无限CD
        if (settingsNoCD)
        {
            foreach (var item in __instance.背包cd剩余时间)
            {
                item.cd时间 = 0f;
            }
        }

        // 无需动力
        if (settingsNoMotivation)
        {
            GameObject 当前选中炮台 = Traverse.Create(__instance).Field("当前选中炮台").GetValue<GameObject>();
            模块_基本属性 模块_基本属性 = 当前选中炮台.GetComponent<模块_基本属性>();
            模块_基本属性.花费 = 0;
        }
    }


}