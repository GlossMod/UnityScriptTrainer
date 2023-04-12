using System;
using System.Collections.Generic;
using System.IO;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;
using System.Collections;
//using ConfigurationManager;
using UnityEngine.Experimental.Rendering;
using System.Reflection;
using BepInEx.Logging;
using System.Text.RegularExpressions;

namespace ScriptTrainer
{
    [BepInPlugin("aoe.top.DysonScriptTrainer", "Dyson Script Trainer By:小莫", "3.0")]
    public class ScriptTrainer : BaseUnityPlugin
    {

        #region[初始化参数]
        public static ConfigEntry<BepInEx.Configuration.KeyboardShortcut> ShowCounter { get; set; }    // 启动按键
       

        private MainWindow window ;

        #endregion


        // 注入脚本时会自动调用Start()方法 执行在Awake()方法后面
        public void Start()
        {
            // 允许用户自定义启动快捷键
            ShowCounter = Config.Bind("修改器快捷键", "Key", new BepInEx.Configuration.KeyboardShortcut(KeyCode.Home));

            // 默认物品叠加倍率
            ScriptPatch.userCount = Config.Bind("叠加倍率", "count", 1);
            //ScriptPatch.ItemTweaks(ScriptPatch.userCount.Value);

            #region[注入游戏补丁]
            Harmony.CreateAndPatchAll(typeof(ScriptPatch), null);
            #endregion


            window =  new MainWindow();

            // 日志输出
            Debug.Log("修改器启动");
        }

        public void Update()
        {
            // 按下快捷键
            if (ShowCounter.Value.IsDown())
            {
                // 打开修改器
                MainWindow.optionToggle = !MainWindow.optionToggle;
                //MainWindow.canvas.SetActive(MainWindow.optionToggle);

                Console.WriteLine("修改器:" + MainWindow.optionToggle);
            }
        }


        public void OnDestroy()
        {
            // 移除 MainWindow.testAssetBundle 加载时的资源
            //AssetBundle.UnloadAllAssetBundles(true);

            // 销毁
            if (MainWindow.canvas)
            {
                MainWindow.canvas.SetActive(false);
                Destroy(MainWindow.canvas);
                MainWindow.canvas = null;
            }
           
        }
    }
}