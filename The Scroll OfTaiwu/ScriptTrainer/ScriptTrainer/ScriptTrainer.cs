using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaiwuModdingLib.Core.Plugin;
using UnityEngine;
using HarmonyLib;
using Object = UnityEngine.Object;

namespace ScriptTrainer
{

    [PluginConfig("ScriptTrainer", "小莫", "1.0.0")]
    public class ScriptTrainer : TaiwuRemakePlugin
    {
        //MainWindow mw;
        private static GameObject gameObject;

        public override void Dispose()
        {
            //// 销毁
            Object.Destroy(gameObject);
            Debug.Log("ScriptTrainer 销毁");
        }

        public override void Initialize()
        {
            // 加载时调用
            gameObject = new GameObject($"taiwu.ScriptTrainer{DateTime.Now.Ticks}");
            gameObject.AddComponent<MainWindow>();
            Debug.Log("ScriptTrainer 初始化完成");
        }
    }
}
