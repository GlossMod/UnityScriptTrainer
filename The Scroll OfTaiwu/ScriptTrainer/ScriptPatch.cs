using System;
using GameData.Domains;
using GameData.Domains.Character;
using HarmonyLib;

namespace ScriptTrainer
{
    public class ScriptPatch
    {
        #region[全局参数]
        public static bool SettingMaxInventoryLoad = true;

        #endregion

        //[HarmonyPostfix]
        //[HarmonyPatch(typeof(GameData.Domains.Global.Inscription.InscribedCharacter), "CalcAttraction")]
        //public static void CalcAttraction(ref int __result)
        //{
        //    if (SettingMaxInventoryLoad)
        //    {
        //        __result = 100;
        //    }
        //}

        //[HarmonyPostfix]
        //[HarmonyPatch("GameData.Domains.Character.Character", "GetMaxInventoryLoad")]
        //public static void GetMaxInventoryLoad(ref int __result)
        //{
        //    if (SettingMaxInventoryLoad)
        //    {
        //        __result = 99999;
        //    }
        //}


        //[HarmonyPostfix]
        //[HarmonyPatch(typeof(UnityEngine.UI.CanvasScaler), "Update")]        
        //public static void Update()
        //{
        //    //BepInExLoader.log.LogMessage("Bootstrapper Update() Fired!");

        //    if (Input.GetKeyDown(KeyCode.F9))
        //    {
        //        if (!MainWindow.initialized)
        //        {
        //            MainWindow.Initialize();
        //        }

        //        MainWindow.optionToggle = !MainWindow.optionToggle;
        //        GLog.Log("窗口开关状态：" + MainWindow.optionToggle.ToString());
        //        //MessageMag.Instance.Send("窗口开关状态", null);

        //        MainWindow.canvas.SetActive(MainWindow.optionToggle);
        //        Event.current.Use();
        //    }
        //}
    }
}
