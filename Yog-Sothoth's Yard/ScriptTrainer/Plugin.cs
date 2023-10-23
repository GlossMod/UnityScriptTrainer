using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace ScriptTrainer;

[BepInPlugin(STConfig.PLUGIN_GUID, STConfig.PLUGIN_NAME, STConfig.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    Harmony harmony;
    MainWindow mainWindow;
    private void Awake()
    {
        STConfig.ShowCounter = Config.Bind("修改器快捷键", "Key", new KeyboardShortcut(KeyCode.Home));

        mainWindow = new MainWindow();

        #region[注入游戏补丁]
        harmony = Harmony.CreateAndPatchAll(typeof(ScriptPatch), null);
        #endregion
    }

    private void Update()
    {

        if (STConfig.ShowCounter.Value.IsDown())
        {
            // 打开修改器
            MainWindow.OptionToggle = !MainWindow.OptionToggle;
            Logger.LogInfo($"修改器: {MainWindow.OptionToggle}");
        }
    }

    private void OnDestroy()
    {
        harmony.UnpatchSelf();

        // 销毁
        if (MainWindow.canvas)
        {
            MainWindow.canvas.SetActive(false);
            Destroy(MainWindow.canvas);
            MainWindow.canvas = null;
        }
    }
}
