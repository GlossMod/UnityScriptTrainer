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
        public static bool DisplayingWindow;
        MainWindow mw;
        
        // 启动按键
        private ConfigEntry<BepInEx.Configuration.KeyboardShortcut> ShowCounter { get; set; }

        public void Awake()
        {
            #region[注入游戏方法]
            
            Harmony.CreateAndPatchAll(typeof(ScriptPatch), null);

            //var harmony = new Harmony("aoe.top.plugins.ScriptTrainer");
            // These are required since UnHollower doesn't support Interfaces yet - Only needed if you need these events.
            //var originalOnBeginDrag = AccessTools.Method(typeof(characterScript), "GetWorkSpeed");
            //var postOnBeginDrag = AccessTools.Method(typeof(ScriptPatch), "GetWorkSpeed");
            //harmony.Patch(originalOnBeginDrag, postfix: new HarmonyMethod(postOnBeginDrag));
            //harmony.CreateClassProcessor(typeof(ScriptPatch));

            #endregion
        }

        public void Start()
        {
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
