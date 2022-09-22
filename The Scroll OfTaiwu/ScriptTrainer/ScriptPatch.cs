using System;
using Config;
using HarmonyLib;
using UnityEngine;

namespace ScriptTrainer
{
    public class ScriptPatch
    {
        #region[全局参数]
        public static bool SettingMaxInventoryLoad = false;

        #endregion

        //[HarmonyPostfix]
        //[HarmonyPatch(typeof(Character))]
        //public static void GetKidnapMaxSlotCount()
        //{

        //    Character.GetKidnapMaxSlotCount();
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
