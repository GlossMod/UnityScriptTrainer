using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace ScriptTrainer
{
    [BepInPlugin("aoe.top.plugins.ScriptTrainer", "觅长生 内置修改器", "2.3")]
    public class main : BaseUnityPlugin
    {
        // 窗口相关
        MainWindow mw;

        private static BepInEx.Logging.ManualLogSource log;
        // 启动按键
        public static ConfigEntry<BepInEx.Configuration.KeyboardShortcut> ShowCounter { get; set; }

        public void Awake()
        {
            mw = new MainWindow();
            log = Logger;
        }

        public void Start()
        {
            #region[注入游戏补丁]
            Harmony.CreateAndPatchAll(typeof(ScriptPatch), null);
            //Harmony.CreateAndPatchAll(typeof(UnityGameUI.WindowDragHandler), null);
            #endregion

            ShowCounter = Config.Bind("修改器快捷键", "Key", new KeyboardShortcut(KeyCode.Home));
            log.LogMessage("脚本已启动");

        }

        public void Update()
        {
            if (!MainWindow.initialized)
            {
                MainWindow.Initialize();
            }

            // 切换UI开关
            //if (ShowCounter.IsDown())
            // if (new KeyboardShortcut(KeyCode.F9).IsDown())
            if (ShowCounter.Value.IsDown())
            {
                if (!MainWindow.initialized)
                {
                    Debug.Log("创建窗口失败,未找到玩家");
                    Debug.Log("请先加载存档进入游戏");
                    return;
                }

                MainWindow.optionToggle = !MainWindow.optionToggle;
                log.LogMessage("窗口开关状态：" + MainWindow.optionToggle.ToString());
                MessageMag.Instance.Send("窗口开关状态", null);
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
