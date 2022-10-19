using Config;
using FrameWork;
using GameData.Common;
using GameData.Domains.Character;
using GameData.GameDataBridge;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityGameUI;
using Navigation = UnityGameUI.Navigation;
using Object = UnityEngine.Object;

namespace ScriptTrainer
{
    internal class MainWindow : MonoBehaviour
    {
        #region[声明]
        // Trainer Base
        public  GameObject obj = null;
        public  MainWindow instance;
        public  bool initialized = false;
        public  bool _optionToggle = false;
        private  TooltipGUI toolTipComp = null;
        public static KeyCode Hot_Key = KeyCode.F9;

        // UI
        public  AssetBundle testAssetBundle = null;
        public  GameObject canvas = null;
        private  bool isVisible = false;
        private  GameObject uiPanel = null;
        private static readonly int width = Mathf.Min(Screen.width, 740);
        private static readonly int height = (Screen.height < 400) ? Screen.height : (450);

        // 界面开关
        public bool optionToggle
        {
            get { return _optionToggle; }
            set {
                // 设置鼠标显示
                if (value)
                {
                    //Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;                    
                }
                else
                {
                    Cursor.visible = false;
                }
                NpcWindow.Initialize();
                MapAreaWindow.container();

                _optionToggle = value;
            }
        }

        // 按钮位置
        private static int initialX
        {
            get
            {
                return -width / 2 + 120;
            }
        }
        private static int initialY
        {
            get
            {
                return height / 2 - 60;
            }
        }
        private  int elementX = initialX;
        private  int elementY = initialY;
        #endregion

        #region[初始化]

        internal GameObject Create(string name)
        {
            obj = new GameObject(name);
            DontDestroyOnLoad(obj);

            obj.AddComponent<MainWindow>();

            return obj;
        }

        public MainWindow()
        {
            instance = this;
        }

        public void Start()
        {
            Initialize();
        }

        public void Update()
        {
            if (!initialized)
            {
                Initialize();
            }

            if (Input.GetKeyDown(Hot_Key))
            {
                optionToggle = !optionToggle;
                Debug.Log("窗口开关状态：" + optionToggle.ToString());
                 
                // 加载图标资源
                ItemWindow.LoadAllPacker();                

                canvas.SetActive(optionToggle);
                Event.current.Use();
            }
        }
        public void OnDestroy()
        {
            if (canvas != null)
            {
                Object.Destroy(canvas);
                initialized = false;
            }            
            //Debug.Log("MainWindow 销毁完毕");
        }

        public  void Initialize()
        {
            initialized = true;
            instance.CreateUI();
        }

        #endregion

        #region[创建UI]
        private void CreateUI()
        {
            try
            {
                if (canvas == null)
                {
                    Debug.Log("创建 UI 元素");

                    canvas = UIControls.createUICanvas();
                    Object.DontDestroyOnLoad(canvas);

                    // 设置背景
                    GameObject background = UIControls.createUIPanel(canvas, (height + 40).ToString(), (width + 40).ToString(), null);
                    background.GetComponent<Image>().color = UIControls.HTMLString2Color("#2D2D30FF");

                    
                    // 将面板添加到画布, 请参阅 createUIPanel 了解我们将高度/宽度作为字符串传递的原因
                    uiPanel = UIControls.createUIPanel(canvas, height.ToString(), width.ToString(), null);
                    // 设置背景颜色
                    uiPanel.GetComponent<Image>().color = UIControls.HTMLString2Color("#424242FF");

                    // 这就是我们将如何挂钩鼠标事件以进行窗口拖动
                    //EventTrigger comp1 = background.AddComponent<EventTrigger>();
                    //WindowDragHandler comp2 = background.AddComponent<WindowDragHandler>();


                    #region[面板元素]


                    #region[创建标题 和 关闭按钮]
                    AddTitle("【太吾绘卷】内置修改器 By:小莫 1.5.2");
                     
                    GameObject closeButton = UIControls.createUIButton(uiPanel, "#B71C1CFF", "X", () =>
                    {
                        optionToggle = false;
                        canvas.SetActive(optionToggle);
                    }, new Vector3(width / 2 + 10, height / 2 + 10, 0));
                    closeButton.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 20);
                    // 字体颜色为白色
                    closeButton.GetComponentInChildren<Text>().color = UIControls.HTMLString2Color("#FFFFFFFF");
                    #endregion



                    #region[常用功能]
                    GameObject BasicScripts = UIControls.createUIPanel(uiPanel, "410", "600", null);
                    BasicScripts.GetComponent<Image>().color = UIControls.HTMLString2Color("#424242FF");
                    BasicScripts.GetComponent<RectTransform>().anchoredPosition = new Vector2(-70, -20);

                    AddH3("常用功能", BasicScripts);
                    {
                        AddButton("斗蛐蛐获胜", BasicScripts, () =>
                        {
                            GMFunc.CricketForceWin();
                        });
                        AddButton("治愈伤病", BasicScripts, () =>
                        {
                            for (int i = 0; i <= 6; i++)
                            {
                                Scripts.ChangeInjury(true, (sbyte)i, -6);
                                Scripts.ChangeInjury(false, (sbyte)i, -6);
                            }
                        });
                        AddButton("解除中毒", BasicScripts, () =>
                        {
                            for (int i = 0; i <= 5; i++)
                            {
                                Scripts.ChangePoisoned((sbyte)i, -1000);
                            }
                        });
                        AddButton("解锁地图", BasicScripts, () =>
                        {
                            GMFunc.ShowAllMapBlocks();
                        });
                        AddButton("解锁驻站", BasicScripts, () =>
                        {
                            GMFunc.UnlockAllStation();
                        });
                        AddButton("解锁技艺", BasicScripts, () =>
                        {
                            Scripts.UnlockAllSkills();
                        });
                        hr(10);
                        AddButton("获取更新", BasicScripts, () =>
                        {
                            // 使用默认浏览器打开网站 https://mod.3dmgame.com/mod/188315
                            System.Diagnostics.Process.Start("https://mod.3dmgame.com/mod/188315");
                        });
                        //AddButton("解锁武学", BasicScripts, () =>
                        //{
                        //    Scripts.UnlockAllArts();                        
                        //});
                        //AddButton("探索势力", BasicScripts, () =>
                        //{
                        //    //GMFunc.QuestAllFactionInfos();
                        //    //var a = GMFunc.GetCombatSkillEditor();
                        //    //a.SetActive(true);
                        //    //GMFunc.QuestAllFactionInfos();
                        //    GMFunc.QuestAllGroupInfos();
                        //});
                    }
                    hr();
                    AddH3("添加资源：", BasicScripts);
                    {
                        AddButton("添加食材", BasicScripts, () =>
                        {
                            Scripts.AddPlayerFood();
                        });
                        AddButton("添加木材", BasicScripts, () =>
                        {
                            Scripts.AddPlayerWood();
                        });
                        AddButton("添加金铁", BasicScripts, () =>
                        {
                            Scripts.AddPlayerGoldIron();
                        });
                        AddButton("添加玉石", BasicScripts, () =>
                        {
                            Scripts.AddPlayerJadeStone();
                        });
                        AddButton("添加织物", BasicScripts, () =>
                        {
                            Scripts.AddPlayerCloth();
                        });
                        AddButton("添加药材", BasicScripts, () =>
                        {
                            Scripts.AddPlayerMedicine();
                        });
                    }
                    hr(10);
                    {
                        AddButton("添加现金", BasicScripts, () =>
                        {
                            Scripts.AddPlayerMoney();
                        });
                        AddButton("添加威望", BasicScripts, () =>
                        {
                            Scripts.AddPlayerAuthority();
                        });
                    }
                    hr();
                    AddH3("免费版,倒卖奸商没马", BasicScripts, Color.cyan);
                    //AddH3("其他功能", BasicScripts);
                    //{
                    //    AddButton("测试按钮", BasicScripts, () =>
                    //    {
                    //        Scripts.Test();
                    //    });
                    //}

                    #endregion

                    #region[玩家功能]
                    ResetCoordinates(true, true);
                    GameObject PlayerScripts = UIControls.createUIPanel(uiPanel, "410", "600", null);
                    PlayerScripts.GetComponent<Image>().color = UIControls.HTMLString2Color("#424242FF");
                    PlayerScripts.GetComponent<RectTransform>().anchoredPosition = new Vector2(-70, -20);

                    AddH3("基础修改", PlayerScripts);
                    {
                        AddButton("修改年龄", PlayerScripts, () =>
                        {
                            Scripts.ChangeAge();
                        });
                        AddButton("修改血量", PlayerScripts, () =>
                        {
                            Scripts.ChangeHp();
                        });
                        AddButton("编辑心情", PlayerScripts, () =>
                        {
                            UIWindows.SpawnInputDialog("您想将心情修改为多少？", "修改", "100", (string count) =>
                            {
                                GMFunc.EditHappiness(Scripts.playerId, count.ConvertToIntDef(100));
                            });
                        });
                        //AddButton("修改名誉", PlayerScripts, () =>
                        //{
                        //    // Preset Fame= 126
                        //    UIWindows.SpawnInputDialog("您想将名誉修改为多少？", "修改", "100", (string count) =>
                        //    {
                        //        GameDataBridge.AddDataModification<sbyte>(4, 0, (ulong)Scripts.playerId, 126U, (sbyte)count.ConvertToIntDef(100));

                        //    });
                        //});
                    }
                    hr();
                    AddH3("基础属性", PlayerScripts);
                    {
                        short[] attributes = new short[6] { 100, 100, 100, 100, 100, 100 };
                        AddInputField("脊力", 150 , attributes[0].ToString(), PlayerScripts, (string text) =>
                        {
                            attributes[0] = (short)text.ConvertToIntDef(100);
                            Scripts.ChangeMainAttributes(attributes);
                        });
                        AddInputField("灵敏", 150 , attributes[1].ToString(), PlayerScripts, (string text) =>
                        {
                            attributes[1] = (short)text.ConvertToIntDef(100);
                            Scripts.ChangeMainAttributes(attributes);
                        });
                        AddInputField("定力", 150 , attributes[2].ToString(), PlayerScripts, (string text) =>
                        {
                            attributes[2] = (short)text.ConvertToIntDef(100);
                            Scripts.ChangeMainAttributes(attributes);
                        });
                        AddInputField("体质", 150 , attributes[3].ToString(), PlayerScripts, (string text) =>
                        {
                            attributes[3] = (short)text.ConvertToIntDef(100);
                            Scripts.ChangeMainAttributes(attributes);
                        });
                        hr(10);
                        AddInputField("根骨", 150 , attributes[4].ToString(), PlayerScripts, (string text) =>
                        {
                            attributes[4] = (short)text.ConvertToIntDef(100);
                            Scripts.ChangeMainAttributes(attributes);
                        });
                        AddInputField("悟性", 150 , attributes[5].ToString(), PlayerScripts, (string text) =>
                        {
                            attributes[5] = (short)text.ConvertToIntDef(100);
                            Scripts.ChangeMainAttributes(attributes);
                        });                        
                    }
                    hr();
                    AddH3("编辑内力", PlayerScripts);
                    {
                        int[] allocation_Items = new int[4] { 100, 100, 100, 100 };
                        AddInputField("摧破", 150, allocation_Items[0].ToString(), PlayerScripts, (string text) =>
                        {
                            allocation_Items[0] = text.ConvertToIntDef(100);
                            Scripts.ChangeNeiLi(allocation_Items);
                        });
                        AddInputField("轻灵", 150, allocation_Items[1].ToString(), PlayerScripts, (string text) =>
                        {
                            allocation_Items[1] = text.ConvertToIntDef(100);
                            Scripts.ChangeNeiLi(allocation_Items);
                        });
                        AddInputField("护体", 150, allocation_Items[2].ToString(), PlayerScripts, (string text) =>
                        {
                            allocation_Items[2] = text.ConvertToIntDef(100);
                            Scripts.ChangeNeiLi(allocation_Items);
                        });
                        AddInputField("奇巧", 150, allocation_Items[3].ToString(), PlayerScripts, (string text) =>
                        {
                            allocation_Items[3] = text.ConvertToIntDef(100);
                            Scripts.ChangeNeiLi(allocation_Items);
                        });
                        hr(10);
                        AddInputField("丹田", 150, "100", PlayerScripts, (string text) =>
                        {
                            GMFunc.EditExtraNeili(Scripts.playerId, text.ConvertToIntDef(100));
                        });
                        AddInputField("境界", 150, "10", PlayerScripts, (string text) =>
                        {
                            GMFunc.EditConsummateLevel(Scripts.playerId, text.ConvertToIntDef(10));
                        });
                        //AddButton("最大内力", PlayerScripts, () =>
                        //{
                        //    // 109 GameDataBridge.AddDataModification<int>(4, 0, (ulong)((long)charId), 27U, value);
                        //    UIWindows.SpawnInputDialog("您想将最大内力设为多少？", "修改", "100", (string count) =>
                        //    {
                        //        //GMFunc.EditConsummateLevel(Scripts.playerId, count.ConvertToIntDef(100));
                        //        GameDataBridge.AddDataModification<int>(4, 0, (ulong)Scripts.playerId, 109U, count.ConvertToIntDef(100));
                        //    });
                        //});
                    }

                    #endregion

                    #region[NPC功能]
                    ResetCoordinates(true, true);
                    GameObject NpcScripts = UIControls.createUIPanel(uiPanel, "410", "600", null);
                    NpcScripts.GetComponent<Image>().color = UIControls.HTMLString2Color("#424242FF");
                    NpcScripts.GetComponent<RectTransform>().anchoredPosition = new Vector2(-70, -20);

                    NpcWindow npcWindow = new NpcWindow(NpcScripts, elementX, elementY);

                    #endregion

                    #region[添加物品]
                    ResetCoordinates(true, true);
                    GameObject ItemScripts = UIControls.createUIPanel(uiPanel, "410", "600", null);
                    ItemScripts.GetComponent<Image>().color = UIControls.HTMLString2Color("#424242FF");
                    ItemScripts.GetComponent<RectTransform>().anchoredPosition = new Vector2(-70, -20);

                    ItemWindow itemWindow = new ItemWindow(ItemScripts, elementX, elementY);

                    #endregion

                    #region[地区恩义]
                    ResetCoordinates(true, true);
                    GameObject MapAreaScripts = UIControls.createUIPanel(uiPanel, "410", "600", null);
                    MapAreaScripts.GetComponent<Image>().color = UIControls.HTMLString2Color("#424242FF");
                    MapAreaScripts.GetComponent<RectTransform>().anchoredPosition = new Vector2(-70, -20);

                    MapAreaWindow mapAreaWindow = new MapAreaWindow(MapAreaScripts, elementX, elementY);


                    #endregion

                    #region[编辑特性]
                    //ResetCoordinates(true, true);
                    //GameObject FeatureScripts = UIControls.createUIPanel(uiPanel, "410", "600", null);
                    //FeatureScripts.GetComponent<Image>().color = UIControls.HTMLString2Color("#424242FF");
                    //FeatureScripts.GetComponent<RectTransform>().anchoredPosition = new Vector2(-70, -20);



                    //FeatureWindow featureWindow = new FeatureWindow(FeatureScripts, elementX, elementY);

                    #endregion

                    #endregion


                    #region[创建导航栏]
                    // 分割线
                    GameObject DividingLine = UIControls.createUIPanel(uiPanel, (height - 40).ToString(), "10", null);
                    DividingLine.GetComponent<Image>().color = UIControls.HTMLString2Color("#2D2D30FF");
                    DividingLine.GetComponent<RectTransform>().anchoredPosition = new Vector3(width / 2 - 200 + 80, -20, 0);

                    //// 按钮
                    GameObject NavPanel = UIControls.createUIPanel(uiPanel, (height - 40).ToString(), "40", null);
                    NavPanel.GetComponent<Image>().color = UIControls.HTMLString2Color("#424242FF");
                    NavPanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(width / 2 - 100, -20, 0);

                    Navigation[] nav = new Navigation[]
                    {
                        new Navigation("BasicScripts","常用功能", BasicScripts, true),
                        new Navigation("PlayerScripts","玩家功能", PlayerScripts, false),
                        new Navigation("NpcScripts","Npc功能", NpcScripts, false),
                        new Navigation("ItemScripts","添加物品", ItemScripts, false),
                        new Navigation("MapAreaScripts","地区恩义", MapAreaScripts, false),
                        //new Navigation("FeatureScripts", "编辑特性", FeatureScripts, false),
                        //new Navigation("ItemScripts", "获取物品", ItemScripts, false),

                    };

                    UINavigation.Initialize(nav, NavPanel);

                    #endregion


                    isVisible = true;

                    //log.LogMessage("Complete!");
                    canvas.SetActive(optionToggle);
                    Debug.Log("修改器初始化完成!");
                    Debug.Log("按F9可开关修改器菜单");
                }
                else
                {
                    //Debug.Log("UI创建失败,未找到玩家");
                    initialized = false;
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                initialized = false;
            }
            
        }

        #endregion


        #region[添加组件]

        // 添加标题
        public GameObject AddTitle(string Title)
        {
            GameObject TitleBackground = UIControls.createUIPanel(canvas, "30", (width - 20).ToString(), null);
            TitleBackground.GetComponent<Image>().color = UIControls.HTMLString2Color("#2D2D30FF");
            TitleBackground.GetComponent<RectTransform>().localPosition = new Vector3(0, height / 2 - 30, 0);

            Sprite txtBgSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#7AB900FF"));
            GameObject uiText = UIControls.createUIText(TitleBackground, txtBgSprite, "#FFFFFFFF");
            uiText.GetComponent<RectTransform>().sizeDelta = new Vector2(width - 10, 30);
            uiText.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            Text text = uiText.GetComponent<Text>();
            text.text = Title;
            text.alignment = TextAnchor.MiddleCenter;
            text.fontSize = 16;

            return uiText;
        }

        public GameObject AddButton(string Text, GameObject panel, UnityAction action)
        {
            string backgroundColor = "#8C9EFFFF";
            Vector3 localPosition = new Vector3(elementX, elementY, 0);
            elementX += 90;

            GameObject button = UIControls.createUIButton(panel, backgroundColor, Text, action, localPosition);

            // 按钮样式
            button.AddComponent<Shadow>().effectColor = UIControls.HTMLString2Color("#000000FF");   // 添加阴影
            button.GetComponent<Shadow>().effectDistance = new Vector2(2, -2);              // 设置阴影偏移
            button.GetComponentInChildren<Text>().fontSize = 14;     // 设置字体大小           
            button.GetComponent<RectTransform>().sizeDelta = new Vector2(80, 30);    // 设置按钮大小


            return button;
        }

        // 添加复选框
        public GameObject AddToggle(string Text, int width, GameObject panel, UnityAction<bool> action)
        {
            // 计算x轴偏移
            elementX += width / 2 - 30;

            Sprite toggleBgSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#3E3E42FF"));
            Sprite toggleSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#18FFFFFF"));
            GameObject uiToggle = UIControls.createUIToggle(panel, toggleBgSprite, toggleSprite);
            uiToggle.GetComponentInChildren<Text>().color = Color.white;
            uiToggle.GetComponentInChildren<Toggle>().isOn = false;
            uiToggle.GetComponent<RectTransform>().localPosition = new Vector3(elementX, elementY, 0);

            uiToggle.GetComponentInChildren<Text>().text = Text;
            uiToggle.GetComponentInChildren<Toggle>().onValueChanged.AddListener(action);


            elementX += width / 2 + 10;

            return uiToggle;
        }

        // 添加输入框
        public GameObject AddInputField(string Text, int width, string defaultText, GameObject panel, UnityAction<string> action)
        {
            // 计算x轴偏移
            elementX += width / 2 - 30;

            // label
            Sprite txtBgSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#7AB900FF"));
            GameObject uiText = UIControls.createUIText(panel, txtBgSprite, "#FFFFFFFF");
            uiText.GetComponent<Text>().text = Text;
            uiText.GetComponent<RectTransform>().localPosition = new Vector3(elementX, elementY, 0);
            //uiText.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 30);
            uiText.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;


            // 坐标偏移
            elementX += 10;

            // 输入框
            Sprite inputFieldSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#212121FF"));
            GameObject uiInputField = UIControls.createUIInputField(panel, inputFieldSprite, "#FFFFFFFF");
            uiInputField.GetComponent<InputField>().text = defaultText;
            uiInputField.GetComponent<RectTransform>().localPosition = new Vector3(elementX, elementY, 0);
            uiInputField.GetComponent<RectTransform>().sizeDelta = new Vector2(width - 60, 30);

            // 文本框失去焦点时触发方法
            uiInputField.GetComponent<InputField>().onEndEdit.AddListener(action);

            elementX += width / 2 + 10;
            return uiInputField;
        }

        // 添加下拉框
        public GameObject AddDropdown(string Text, int width, List<string> options, GameObject panel, UnityAction<int> action)
        {
            // 计算x轴偏移
            elementX += width / 2 - 30;

            // label
            Sprite txtBgSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#7AB900FF"));
            GameObject uiText = UIControls.createUIText(panel, txtBgSprite, "#FFFFFFFF");
            uiText.GetComponent<Text>().text = Text;
            uiText.GetComponent<RectTransform>().localPosition = new Vector3(elementX, elementY, 0);
            //uiText.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 30);
            uiText.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

            // 坐标偏移
            elementX += 60;

            // 创建下拉框
            Sprite dropdownBgSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#212121FF"));      // 背景颜色
            Sprite dropdownScrollbarSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#8C9EFFFF"));   // 滚动条颜色 (如果有的话
            Sprite dropdownDropDownSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#212121FF"));    // 框右侧小点的颜色
            Sprite dropdownCheckmarkSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#8C9EFFFF"));   // 选中时的颜色
            Sprite dropdownMaskSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#E65100FF"));        // 不知道是哪的颜色
            Color LabelColor = UIControls.HTMLString2Color("#EFEBE9FF");
            GameObject uiDropDown = UIControls.createUIDropDown(panel, dropdownBgSprite, dropdownScrollbarSprite, dropdownDropDownSprite, dropdownCheckmarkSprite, dropdownMaskSprite, options, LabelColor);
            Object.DontDestroyOnLoad(uiDropDown);
            uiDropDown.GetComponent<RectTransform>().localPosition = new Vector3(elementX, elementY, 0);

            // 下拉框选中时触发方法
            uiDropDown.GetComponent<Dropdown>().onValueChanged.AddListener(action);

            elementX += width / 2 + 60;
            return uiDropDown;
        }

        // 添加小标题
        public GameObject AddH3(string text, GameObject panel, Color color = default(Color))
        {
            elementX += 40;

            Sprite txtBgSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#7AB900FF"));
            GameObject uiText = UIControls.createUIText(panel, txtBgSprite, "#FFFFFFFF");
            uiText.GetComponent<Text>().text = text;
            uiText.GetComponent<RectTransform>().localPosition = new Vector3(elementX, elementY, 0);            
            //uiText.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 30);  // 设置宽度

            // 设置字体样式为h3小标题
            uiText.GetComponent<Text>().fontSize = 14;
            uiText.GetComponent<Text>().fontStyle = FontStyle.Bold;

            // 设置字体颜色
            if (color != default(Color)) uiText.GetComponent<Text>().color = color;

            hr();
            elementY += 20;
            elementX += 10;
            return uiText;
        }

        // 换行
        public void hr(int offsetX = 0, int offsetY = 0)
        {
            ResetCoordinates(true);
            elementX += offsetX;
            elementY -= 50 + offsetY;

        }
        // 重置坐标
        public void ResetCoordinates(bool x, bool y = false)
        {
            if (x) elementX = initialX;
            if (y) elementY = initialY;
        }

        #endregion



    }

}
