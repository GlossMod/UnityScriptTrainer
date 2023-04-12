using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptTrainer
{
    internal class ScriptPatch
    {
        #region[全局参数]
        public static ConfigEntry<int> userCount { get; set; }  // 物品叠加倍率

        #endregion

        [HarmonyPostfix]
        [HarmonyPatch(typeof(StorageComponent), "LoadStatic")]

        public static void LoadStatic_Postfix()
        {
            ItemTweaks(userCount.Value);
        }


        // 物品叠加
        public static void ItemTweaks(int count)
        {
            ItemProto[] dataArray = LDB.items.dataArray;

            foreach (var item in dataArray)
            {
                StorageComponent.itemStackCount[item.ID] = item.StackSize * count;
            }
            userCount.Value = count;

            userCount.SetSerializedValue(count.ToString());
        }
    }
}
