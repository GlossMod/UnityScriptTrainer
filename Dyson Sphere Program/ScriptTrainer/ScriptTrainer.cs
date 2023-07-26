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
    [BepInPlugin("aoe.top.DysonScriptTrainer", "Dyson Script Trainer By:小莫", "3.1")]


    public class ScriptTrainer : BaseUnityPlugin
    {

        #region[初始化参数]
        public static ConfigEntry<KeyboardShortcut> ShowCounter { get; set; }    // 启动按键

        #region 配方修改配置




        public static ConfigEntry<int> AssemblerSpeed { get; set; }              // 组装机速度
        public static ConfigEntry<int> AssemblerOutRate { get; set; }             // 组装机产出倍率

        public static ConfigEntry<int> SmelterSpeed { get; set; }                // 熔炉速度
        public static ConfigEntry<int> SmelterOutRate { get; set; }              // 熔炉产出倍率

        public static ConfigEntry<int> RefinerySpeed { get; set; }               // 精炼厂速度
        public static ConfigEntry<int> RefineryOutRate { get; set; }             // 精炼厂产出倍率

        public static ConfigEntry<int> ChemicalPlantSpeed { get; set; }          // 化工厂速度
        public static ConfigEntry<int> ChemicalPlantOutRate { get; set; }        // 化工厂产出倍率

        public static ConfigEntry<int> ParticleColliderSpeed { get; set; }       // 粒子对撞机速度
        public static ConfigEntry<int> ParticleColliderOutRate { get; set; }     // 粒子对撞机产出倍率

        public static ConfigEntry<int> ExchangeSpeed { get; set; }           //  交易站速度
        public static ConfigEntry<int> ExchangeOutRate { get; set; }         // 交易站产出倍率

        public static ConfigEntry<int> PhotonStoreSpeed { get; set; }        // 光子储存速度
        public static ConfigEntry<int> PhotonStoreOutRate { get; set; }      // 光子储存产出倍率

        public static ConfigEntry<int> FractionateSpeed { get; set; }                // 分馏塔速度
        public static ConfigEntry<int> FractionateOutRate { get; set; }              // 分馏塔产出倍率

        public static ConfigEntry<int> ResearchSpeed { get; set; }                // 研究速度
        public static ConfigEntry<int> ResearchOutRate { get; set; }              // 研究产出倍率


        #endregion


        private MainWindow window;

        #endregion

        public void InitRecipeConfig()
        {
            AssemblerSpeed = Config.Bind("配方修改", "AssemblerSpeed", 1);
            AssemblerOutRate = Config.Bind("配方修改", "AssemblerOutRate", 1);
            SmelterSpeed = Config.Bind("配方修改", "SmelterSpeed", 1);
            SmelterOutRate = Config.Bind("配方修改", "SmelterOutRate", 1);
            RefinerySpeed = Config.Bind("配方修改", "RefinerySpeed", 1);
            RefineryOutRate = Config.Bind("配方修改", "RefineryOutRate", 1);
            ChemicalPlantSpeed = Config.Bind("配方修改", "ChemicalPlantSpeed", 1);
            ChemicalPlantOutRate = Config.Bind("配方修改", "ChemicalPlantOutRate", 1);
            ParticleColliderSpeed = Config.Bind("配方修改", "ParticleColliderSpeed", 1);
            ParticleColliderOutRate = Config.Bind("配方修改", "ParticleColliderOutRate", 1);
            ExchangeSpeed = Config.Bind("配方修改", "ExchangeSpeed", 1);
            ExchangeOutRate = Config.Bind("配方修改", "ExchangeOutRate", 1);
            PhotonStoreSpeed = Config.Bind("配方修改", "PhotonStoreSpeed", 1);
            PhotonStoreOutRate = Config.Bind("配方修改", "PhotonStoreOutRate", 1);
            FractionateSpeed = Config.Bind("配方修改", "FractionateSpeed", 1);
            FractionateOutRate = Config.Bind("配方修改", "FractionateOutRate", 1);
            ResearchSpeed = Config.Bind("配方修改", "ResearchSpeed", 1);
            ResearchOutRate = Config.Bind("配方修改", "ResearchOutRate", 1);


        }
        // 注入脚本时会自动调用Start()方法 执行在Awake()方法后面
        public void Start()
        {
            // 允许用户自定义启动快捷键
            ShowCounter = Config.Bind("修改器快捷键", "Key", new KeyboardShortcut(KeyCode.Home));

            // 默认物品叠加倍率
            ScriptPatch.userCount = Config.Bind("叠加倍率", "count", 1);
            //ScriptPatch.ItemTweaks(ScriptPatch.userCount.Value);

            InitRecipeConfig();
            #region[注入游戏补丁]
            Harmony.CreateAndPatchAll(typeof(ScriptPatch), null);
            #endregion


            window = new MainWindow();

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