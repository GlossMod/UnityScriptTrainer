using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace ScriptTrainer;

[BepInPlugin("aoe.top.plugins.SW_ScriptTrainer", "暖雪 内置修改器", "1.0.0.0")]
public class Main : BaseUnityPlugin
{

    #region 全局配置
    MainWindow mw;
    private static BepInEx.Logging.ManualLogSource log;

    // 启动按键
    private ConfigEntry<BepInEx.Configuration.KeyboardShortcut> ShowCounter { get; set; }

    #endregion

    private void Awake()
    {
        log = Logger;
        // Plugin startup logic
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        mw = new MainWindow();
    }

    private void Start()
    {
        #region[注入游戏补丁]
        Harmony.CreateAndPatchAll(typeof(ScriptPatch), null);
        // Harmony.CreateAndPatchAll(typeof(UnityGameUI.WindowDragHandler), null);
        #endregion

        ShowCounter = Config.Bind("修改器快捷键", "Key", new KeyboardShortcut(KeyCode.Home));
        Logger.LogInfo("脚本已启动");
    }

    private void Update()
    {
        if (!MainWindow.initialized)
        {
            MainWindow.Initialize();
        }

        if (ShowCounter.Value.IsDown())
        {
            // Logger.LogInfo("启动键按下");
            MainWindow.optionToggle = !MainWindow.optionToggle;
            Logger.LogInfo("窗口开关状态：" + MainWindow.optionToggle.ToString());
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

    public static void Logs(string text)
    {
        log.LogInfo(text);
    }
}

