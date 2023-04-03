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

    [PluginConfig("ScriptTrainer", "小莫", "1.5.3")]
    public class ScriptTrainer : TaiwuRemakePlugin
    {
        //MainWindow mw;
        private static GameObject gameObject;
        private Harmony harmony;


        public override void Dispose()
        {
            //// 销毁
            Object.Destroy(gameObject);
            //harmony.UnpatchSelf();
            Debug.Log("ScriptTrainer 销毁");
        }

        public override void Initialize()
        {

            // 加载时调用
            gameObject = new GameObject($"taiwu.ScriptTrainer{DateTime.Now.Ticks}");
            gameObject.AddComponent<MainWindow>();
            // 加载后端的操作dll
            //gameObject.AddComponent<STGameData.STGameData>();
            //#region[注入游戏补丁]
            //harmony = Harmony.CreateAndPatchAll(typeof(ScriptPatch), "ScriptTrainer");
            ////Harmony.CreateAndPatchAll(typeof(UnityGameUI.WindowDragHandler), null);
            //#endregion

            Debug.Log("ScriptTrainer 初始化完成");
        }

        public override void OnModSettingUpdate()
        {
            string hot_Key = default(string) ;
            ModManager.GetSetting(base.ModIdStr, "Hot_key", ref hot_Key);
            if (!Enum.TryParse<KeyCode>(hot_Key, out MainWindow.Hot_Key))
            {
                MainWindow.Hot_Key = KeyCode.F9;
            }
        }
    }
}
