using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityGameUI;
using Object = UnityEngine.Object;

namespace ScriptTrainer
{

    // MonoBehaviour
    // ManualBehaviour
    internal class MainWindow : ManualBehaviour
    {
        #region 声明变量

        // Trainer Base
        public static GameObject obj = null;
        public static MainWindow instance;
        public static bool initialized = false;
        public static bool _optionToggle = false;

        // UI
        public static AssetBundle testAssetBundle = null;
        public static GameObject canvas = null;
        public static readonly int width = Mathf.Min(Screen.width, 740);
        public static readonly int height = (Screen.height < 400) ? Screen.height : (450);



        // 变量
        public static Player player = GameMain.mainPlayer;
        public static GameData gameData;

        // 窗口开关
        public static bool optionToggle
        {
            get => _optionToggle;
            set
            {
                _optionToggle = value;

                gameData = GameMain.data;
                player = GameMain.mainPlayer;

                if (initialized) canvas.SetActive(optionToggle);
                else Init();
            }
        }

        // 组件位置
        private static int initialX { get => -width / 2 + 110; }
        private static int initialY { get => height / 2 - 85; }

        private static int elementX = initialX;
        private static int elementY = initialY;

        #endregion

        public MainWindow()
        {
            Init();
        }

        private static void Init()
        {
            if (initialized)
            {
                return;
            }

            if (!GameMain.instance)
            {
                Debug.Log("等待游戏初始化完成");
                return;
            }

            Console.WriteLine((bool)GameMain.instance);



            CreateUI();

            canvas.SetActive(optionToggle);
            initialized = true;
        }


        // 创建UI
        // ReSharper disable Unity.PerformanceAnalysis
        private static void CreateUI()
        {
            if (canvas == null)
            {
                Debug.Log("开始创建UI");

                canvas = UIControls.createUICanvas();
                Object.DontDestroyOnLoad(canvas);
                canvas.name = "ScriptTrainer";

                // 创建背景              
                UIInventoryWindow background = Components.createUIPanel(canvas, "内置修改器", width, height);
                background.gameObject.name = "background";

                // 关闭按钮 panel-bg/btn-box/close-btn
                Transform close_btn = background.transform.Find("panel-bg/btn-box/close-btn");

                close_btn.GetComponent<UIButton>().onClick += (int i) =>
                {
                    optionToggle = false;
                    canvas.SetActive(optionToggle);
                };


                #region[基础功能]

                GameObject BasicScripts = UIControls.createUIPanel(background.gameObject, "330", "630", null);
                BasicScripts.GetComponent<Image>().color = UIControls.HTMLString2Color("#00000033");
                BasicScripts.name = "BasicScripts";

                // ==================== 数值功能 ====================
                AddCount(BasicScripts, "物品堆叠倍率", ScriptPatch.userCount.Value, (int num) =>
                {
                    ScriptPatch.ItemTweaks(num);
                }, 1, 1);

                AddCount(BasicScripts, "无人机数量", player.mecha.droneCount, (int num) =>
                {
                    player.mecha.droneCount = num;

                }, 1, 1, 256);

                AddCount(BasicScripts, "无人机速度", (int)player.mecha.droneSpeed, (int num) =>
                {
                    player.mecha.droneSpeed = num;

                }, 1, 1, 256);

                hr();
                AddCount(BasicScripts, "跳跃高度", (int)player.mecha.jumpSpeed, (int num) =>
                {
                    player.mecha.jumpSpeed = num;

                }, 10, 10);
                AddCount(BasicScripts, "移动速度", (int)player.mecha.walkSpeed, (int num) =>
                {
                    player.mecha.walkSpeed = num;

                }, 10, 10);

                AddCount(BasicScripts, "机甲采矿速度", (int)player.mecha.miningSpeed, (int num) =>
                {
                    player.mecha.miningSpeed = num;
                }, 1, 1);
                hr();
                AddCount(BasicScripts, "机甲制作速度", (int)player.mecha.replicateSpeed, (int num) =>
                {
                    player.mecha.replicateSpeed = num;
                }, 1, 1);
                hr();

                AddCount(BasicScripts, "研究速度", (int)player.mecha.researchPower, (int num) =>
                {
                    player.mecha.researchPower = num;
                });

                // ==================== 按钮功能 ====================
                hr();
                AddButton(BasicScripts, "解锁所有科技", () =>
                {
                    TechProto[] techs = LDB.techs.dataArray;

                    foreach (var item in techs)
                    {
                        GameMain.history.UnlockTech(item.ID);
                    }

                }, 100);

                AddButton(BasicScripts, "解锁成就", () =>
                {
                    var a = LDB.achievements.dataArray;


                    foreach (var item in LDB.achievements.dataArray)
                    {
                        DSPGame.achievementSystem.UnlockAchievement(item.ID);
                    }
                });

                AddButton(BasicScripts, "解锁里程碑", () =>
                {

                    foreach (var item in LDB.milestones.dataArray)
                    {
                        GameMain.data.milestoneSystem.UnlockMilestone(item.ID, GameMain.gameTick);
                    }
                });

                #endregion


                #region[添加物品]
                ResetCoordinates(true, true);

                GameObject ItemScripts = UIControls.createUIPanel(background.gameObject, "330", "630", null);
                ItemScripts.GetComponent<Image>().color = UIControls.HTMLString2Color("#00000033");
                ItemScripts.name = "ItemScripts";

                new ItemWindow(ItemScripts);



                #endregion
                #region[修改配方]
                ResetCoordinates(true, true);

                GameObject RecipeScripts = UIControls.createUIPanel(background.gameObject, "330", "630", null);
                RecipeScripts.GetComponent<Image>().color = UIControls.HTMLString2Color("#00000033");
                RecipeScripts.name = "RecipeScripts";

                new RecipeWindow(RecipeScripts);



                #endregion

                #region[创建导航栏]

                DSNavigation dSNavigation = new DSNavigation(background.gameObject);
                dSNavigation.AddTab("基础功能", BasicScripts, true);
                dSNavigation.AddTab("添加物品", ItemScripts);
                dSNavigation.AddTab("配方修改", RecipeScripts);
                dSNavigation.Create();

                #endregion

                #region[著名]
                ByWindow by = new ByWindow();
                by.init(background.gameObject);



                #endregion


                Debug.Log("修改器UI初始化完成");
                Debug.Log($"使用{ScriptTrainer.ShowCounter.Value}键打开修改器");

                canvas.SetActive(optionToggle);
            }
        }







        #region[添加组件]
        // 创建标题
        public static GameObject AddTitle(GameObject panel, string Title)
        {

            Sprite txtBgSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#7AB900FF"));
            GameObject uiText = UIControls.createUIText(panel, txtBgSprite, "#FFFFFFFF");
            //uiText.GetComponent<RectTransform>().sizeDelta = new Vector2(width - 10, 30);
            uiText.GetComponent<RectTransform>().localPosition = new Vector3(110, -30, 0);
            Text text = uiText.GetComponent<Text>();
            text.text = Title;
            text.alignment = TextAnchor.MiddleCenter;
            text.fontSize = 16;
            uiText.name = "title-text";
            return uiText;
        }

        // 创建按钮
        public static Transform AddButton(GameObject panel, string text, UnityAction action, int width = 80, int height = 30)
        {
            Transform button = Components.createUIButton(panel);
            RectTransform rt = button.GetComponent<RectTransform>();

            rt.localPosition = new Vector3(elementX, elementY, 0);
            rt.sizeDelta = new Vector2(width, height);    // 设置按钮大小

            elementX += width + 10;


            button.GetComponentInChildren<Text>().text = text;
            button.GetComponent<Button>().onClick.AddListener(action);

            return button;
        }

        // 添加数字 加减按钮
        public static Transform AddCount(GameObject panel, string label, int value, UnityAction<int> action, int step = 1, int? min = null, int? max = null)
        {
            // 计算x轴偏移
            elementX += 120 / 2 - 30;
            // label
            Sprite txtBgSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#7AB900FF"));
            GameObject uiText = UIControls.createUIText(panel, txtBgSprite, "#FFFFFFFF");
            uiText.GetComponent<Text>().text = label;
            uiText.GetComponent<RectTransform>().localPosition = new Vector3(elementX, elementY, 0);
            uiText.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

            // 坐标偏移
            elementX += 60;

            // 加减按钮
            Transform count = Components.createCount(panel);
            RectTransform rt = count.GetComponent<RectTransform>();
            rt.localPosition = new Vector3(elementX, elementY, 0);

            // times-value 显示值
            Transform times_value = count.Find("times-value");
            times_value.GetComponent<Text>().text = value.ToString();

            // - 按钮
            UIButton minusUIButton = count.Find("-").GetComponent<UIButton>();

            minusUIButton.onClick += (int a) =>
            {
                value -= step;

                if (min != null) if (value <= min) value = (int)min;

                times_value.GetComponent<Text>().text = value.ToString();
                action(value);
            };
            minusUIButton.onRightClick += (int a) =>
            {
                value -= step * 10;

                if (min != null) if (value <= min) value = (int)min;

                times_value.GetComponent<Text>().text = value.ToString();
                action(value);
            };


            // + 按钮
            UIButton plusUIButton = count.Find("+").GetComponent<UIButton>();
            plusUIButton.onClick += (int a) =>
            {
                value += step;

                if (max != null) if (value >= max) value = (int)max;


                times_value.GetComponent<Text>().text = value.ToString();
                action(value);
            };
            plusUIButton.onRightClick += (int a) =>
            {
                value += step * 10;

                if (max != null) if (value >= max) value = (int)max;


                times_value.GetComponent<Text>().text = value.ToString();
                action(value);
            };

            elementX += 120;

            return count;

        }



        // 重置坐标
        public static void ResetCoordinates(bool x, bool y = false)
        {
            if (x) elementX = initialX;
            if (y) elementY = initialY;
        }

        // 换行
        public static void hr(int offsetX = 0, int offsetY = 0)
        {
            ResetCoordinates(true);
            elementX += offsetX;
            elementY -= 50 + offsetY;

        }

        #endregion

    }
}
