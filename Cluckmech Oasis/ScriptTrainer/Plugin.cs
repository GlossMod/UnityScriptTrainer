using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace ScriptTrainer
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        #region 全局配置
        MainWindow mw;
        private static BepInEx.Logging.ManualLogSource log;

        // 启动按键
        private ConfigEntry<BepInEx.Configuration.KeyboardShortcut> ShowCounter { get; set; }

        #endregion




        private void Awake()
        {
            // Plugin startup logic

            #region[注入游戏补丁]
            Harmony.CreateAndPatchAll(typeof(ScriptPatch), null);
            // Harmony.CreateAndPatchAll(typeof(UnityGameUI.WindowDragHandler), null);
            #endregion

            ShowCounter = Config.Bind("修改器快捷键", "Key", new KeyboardShortcut(KeyCode.Home));
            Logger.LogInfo("脚本已启动");
            mw = new MainWindow();
        }

        private void Update()
        {
            if (!MainWindow.initialized)
            {
                Logger.LogInfo("初始化窗口");
                MainWindow.Initialize();
            }

            if (ShowCounter.Value.IsDown())
            {
                // Logger.LogInfo("启动键按下");
                MainWindow.optionToggle = !MainWindow.optionToggle;
                Logger.LogInfo("窗口开关状态: " + MainWindow.optionToggle.ToString());
                // MessageMag.Instance.Send("窗口开关状态", null);
                MainWindow.canvas.SetActive(MainWindow.optionToggle);
                Event.current.Use();
            }
        }
        public void OnDestroy()
        {
            // 移除 MainWindow.testAssetBundle 加载时的资源
            //AssetBundle.UnloadAllAssetBundles(true);

            // 销毁
            MainWindow.canvas.SetActive(false);
            MainWindow.canvas = null;
        }
    }
}
