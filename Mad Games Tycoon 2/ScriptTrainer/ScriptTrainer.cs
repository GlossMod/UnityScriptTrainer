using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace ScriptTrainer
{
    [BepInPlugin("aoe.top.plugins.ScriptTrainer", "疯狂游戏大亨2 内置修改器", "1.0.0.0")]
    public class ScriptTrainer: BaseUnityPlugin
    {
        // 窗口相关
        MainWindow mw;
        
        // 启动按键
        private ConfigEntry<BepInEx.Configuration.KeyboardShortcut> ShowCounter { get; set; }

        public void Awake()
        {
            
        }

        public void Start()
        {
            #region[注入游戏补丁]
            Harmony.CreateAndPatchAll(typeof(ScriptPatch), null);
            #endregion

            ShowCounter = Config.Bind("修改器快捷键", "Key", new KeyboardShortcut(KeyCode.F9));
            Debug.Log("脚本已启动");
            mw = new MainWindow();

        }

        public void Update()
        {
            if (!MainWindow.initialized) {
                MainWindow.Initialize(); 
            }

            // 切换UI开关
            if (ShowCounter.Value.IsDown())
            {
                MainWindow.optionToggle = !MainWindow.optionToggle;
                Debug.Log("窗口开关状态：" + MainWindow.optionToggle.ToString());

                MainWindow.canvas.SetActive(MainWindow.optionToggle);
                Event.current.Use();
            }
        }

        public void OnDestroy()
        {
            // 移除 MainWindow.testAssetBundle 加载时的资源
            AssetBundle.UnloadAllAssetBundles(true);

        }
    }
}
