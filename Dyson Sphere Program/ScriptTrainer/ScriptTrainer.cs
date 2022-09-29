using System;
using System.Collections.Generic;
using System.IO;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;
using System.Collections;
using ConfigurationManager;
using UnityEngine.Experimental.Rendering;
using System.Reflection;
using BepInEx.Logging;
using System.Text.RegularExpressions;

namespace ScriptTrainer
{
    [BepInPlugin("aoe.top.ScriptTrainer", "[戴森球计划] 内置修改器 By:小莫", "2.0")]
    public class ScriptTrainer : BaseUnityPlugin
    {

        // 启动按键
        private ConfigEntry<BepInEx.Configuration.KeyboardShortcut> ShowCounter { get; set; }
        private static ConfigEntry<int> userCount;


        // 注入脚本时会自动调用Start()方法 执行在Awake()方法后面
        public void Start()
        {
            // 允许用户自定义启动快捷键
            ShowCounter = Config.Bind("修改器快捷键", "Key", new BepInEx.Configuration.KeyboardShortcut(KeyCode.F9));

            // 日志输出
            Debug.Log("脚本已启动");
        }

        public void Update()
        {
           
        }


    }


}