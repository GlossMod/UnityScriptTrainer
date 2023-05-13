using System;
using System.Collections.Generic;
using System.IO;
using Steamworks;
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
        public static GameObject obj = null;
        public static MainWindow instance;
        public static bool initialized = false;
        public static bool _optionToggle = false;
        private static TooltipGUI toolTipComp = null;
        public static KBEngine.Avatar player;   //获取玩家

        // UI
        public static AssetBundle testAssetBundle = null;
        public static GameObject canvas = null;
        private static bool isVisible = false;
        private static GameObject uiPanel = null;
        private static readonly int width = Mathf.Min(Screen.width, 740);
        private static readonly int height = (Screen.height < 400) ? Screen.height : (450);

        // 窗口开关
        public static bool optionToggle
        {
            get => _optionToggle;
            set
            {
                _optionToggle = value;

                player = Tools.instance.getPlayer();    // 获取玩家
                NpcWindow.RefreshNpcData();    // 刷新获取Npc

                if (!initialized)
                {
                    instance.CreateUI();
                }
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

        private static int elementX = initialX;
        private static int elementY = initialY;
        #endregion



        public MainWindow()
        {
            instance = this;
        }

        public static void Initialize()
        {
            #region[初始化资源]
            if (testAssetBundle == null)
            {

                // 获取当前steam登录的用户
                // CSteamID steamID = SteamUser.GetSteamID();


            }
            #endregion

            initialized = true;

            instance.CreateUI();

        }

        #region[创建UI]
        private void CreateUI()
        {
            try
            {
                player = Tools.instance.getPlayer();    // 获取玩家 
            }
            catch (Exception e)
            {
                initialized = false;
                return;
            }



            if (canvas == null && player != null)
            {
                Debug.Log("创建 UI 元素");

                canvas = UIControls.createUICanvas();
                Object.DontDestroyOnLoad(canvas);

                // 设置背景
                GameObject background = UIControls.createUIPanel(canvas, (height + 40).ToString(), (width + 40).ToString(), null);
                background.GetComponent<Image>().color = UIControls.HTMLString2Color("#2D2D30FF");
                background.AddComponent<WindowDragHandler>();

                // 将面板添加到画布, 请参阅 createUIPanel 了解我们将高度/宽度作为字符串传递的原因
                uiPanel = UIControls.createUIPanel(background, height.ToString(), width.ToString(), null);
                // 设置背景颜色
                uiPanel.GetComponent<Image>().color = UIControls.HTMLString2Color("#424242FF");


                // 鼠标拖动窗口移动
                //EventTrigger comp1 = background.AddComponent<EventTrigger>();                                
                //WindowDragHandler comp2 = background.AddComponent<WindowDragHandler>();
                //EventTrigger.Entry entry1 = new EventTrigger.Entry();
                //entry1.eventID = EventTriggerType.Drag;
                ////entry1.callback.AddListener((data) => { OnDrag((PointerEventData)data); });
                //comp1.triggers.Add(entry1);


                //EventTrigger comp1 = background.AddComponent<EventTrigger>();
                //WindowDragHandler comp2 = background.AddComponent<WindowDragHandler>();




                #region[面板元素]


                #region[创建标题 和 关闭按钮]
                AddTitle(background,"【觅长生】内置修改器 By:小莫");

                GameObject closeButton = UIControls.createUIButton(uiPanel, "#B71C1CFF", "X", () =>
                {
                    optionToggle = false;
                    canvas.SetActive(optionToggle);
                }, new Vector3(width / 2 + 10, height / 2 + 10, 0));
                closeButton.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 20);
                // 字体颜色为白色
                closeButton.GetComponentInChildren<Text>().color = UIControls.HTMLString2Color("#FFFFFFFF");
                #endregion



                #region[基础功能]
                GameObject BasicScripts = UIControls.createUIPanel(uiPanel, "410", "600", null);
                BasicScripts.GetComponent<Image>().color = UIControls.HTMLString2Color("#424242FF");
                BasicScripts.GetComponent<RectTransform>().anchoredPosition = new Vector2(-70, -20);

                AddH3("常用功能：", BasicScripts);
                {
                    AddButton("添加灵石", BasicScripts, () =>
                    {
                        Scripts.AddMoney();
                    });

                    AddButton("宁州声望", BasicScripts, () =>
                    {
                        Scripts.AddShengWang(0, "宁州");
                    });
                    AddButton("海域声望", BasicScripts, () =>
                    {
                        Scripts.AddShengWang(19,"海域");
                    });
                    AddButton("龙族声望", BasicScripts, () =>
                    {
                        
                        Scripts.AddShengWang(23, "龙族");
                    });
                    AddButton("白帝楼声望", BasicScripts, () =>
                    {
                        Scripts.AddShengWang(24,"白帝楼");
                    });
                    AddButton("风雨楼声望", BasicScripts, () =>
                    {
                        Scripts.AddShengWang(10, "风雨楼");
                    });
                }
                hr(10);
                {
                    AddButton("添加修为", BasicScripts, () =>
                    {
                        Scripts.AddExp();
                    });
                    AddButton("修为全满", BasicScripts, () =>
                    {
                        Scripts.MaxExp();
                    });
                    AddButton("血量全满", BasicScripts, () =>
                    {
                        Scripts.MaxHp();
                    });
                }
                hr();
                AddH3("抽卡功能：", BasicScripts);
                {
                    AddButton("抽1张卡", BasicScripts, () =>
                    {

                        RoundManager.instance.DrawCard(player);
                    });
                    AddButton("抽3张卡", BasicScripts, () =>
                    {

                        for (int i = 0; i < 3; i++)
                        {
                            RoundManager.instance.DrawCard(player);
                        }
                    });
                    AddButton("3张相同卡", BasicScripts, () =>
                    {

                        int a = UnityEngine.Random.Range(0, 4);
                        for (int i = 0; i < 3; i++)
                        {
                            RoundManager.instance.DrawCard(player, a);
                        }
                    });
                }
                hr(10);
                {
                    SCard[] CardList = {
                        // "金", "木", "水", "火", "土"
                        new SCard{ name = "金", CardID =  0  },
                        new SCard{ name = "木", CardID =  1  },
                        new SCard{ name = "水", CardID =  2  },
                        new SCard{ name = "火", CardID =  3  },
                        new SCard{ name = "土", CardID =  4  },
                    };
                    foreach (var item in CardList)
                    {
                        AddButton(string.Format("抽3张{0}卡", item.name), BasicScripts, () =>
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                RoundManager.instance.DrawCard(player, item.CardID);
                            }
                        });
                    }
                }

                #endregion

                #region[玩家属性]
                ResetCoordinates(true, true);
                GameObject PlayerAttributes = UIControls.createUIPanel(uiPanel, "410", "600", null);
                PlayerAttributes.GetComponent<Image>().color = UIControls.HTMLString2Color("#424242FF");
                PlayerAttributes.GetComponent<RectTransform>().anchoredPosition = new Vector2(-70, -20);

                AddH3("玩家属性：", PlayerAttributes);
                {

                    AddInputField("年龄", 150, player.age.ToString(), PlayerAttributes, (string text) =>
                    {
                        player.age = (uint)text.ConvertToIntDef(16);
                        Debug.Log("年龄已修改为" + player.age);
                    });
                    AddInputField("寿元", 150, player.shouYuan.ToString(), PlayerAttributes, (string text) =>
                    {
                        player.shouYuan = (uint)text.ConvertToIntDef(200);
                        Debug.Log("寿元已修改为" + player.shouYuan);
                    });
                    AddInputField("资质", 150, player.ZiZhi.ToString(), PlayerAttributes, (string text) =>
                    {
                        player.ZiZhi = text.ConvertToIntDef(100);
                        Debug.Log("资质已修改为" + player.ZiZhi);
                    });
                    AddInputField("神识", 150, player.shengShi.ToString(), PlayerAttributes, (string text) =>
                    {
                        player.shengShi = text.ConvertToIntDef(100);
                        Debug.Log("神识已修改为" + player.shengShi);
                    });
                }
                hr(10);
                {

                    AddInputField("悟性", 150, player.wuXin.ToString(), PlayerAttributes, (string text) =>
                    {
                        player.wuXin = (uint)text.ConvertToIntDef(100);
                        Debug.Log("悟性已修改为" + player.wuXin);
                    });
                    AddInputField("遁速", 150, player.dunSu.ToString(), PlayerAttributes, (string text) =>
                    {
                        player.dunSu = text.ConvertToIntDef(100);
                        Debug.Log("遁速已修改为" + player.dunSu);
                    });
                    AddInputField("心境", 150, player.xinjin.ToString(), PlayerAttributes, (string text) =>
                    {
                        player.xinjin = text.ConvertToIntDef(100);
                        Debug.Log("心境已修改为" + player.xinjin);
                    });
                    AddInputField("丹毒", 150, player.Dandu.ToString(), PlayerAttributes, (string text) =>
                    {
                        player.Dandu = text.ConvertToIntDef(100);
                        Debug.Log("丹毒已修改为" + player.Dandu);
                    });
                }
                hr(10);
                {
                    AddInputField("灵感", 150, player.LingGan.ToString(), PlayerAttributes, (string text) =>
                    {
                        player.LingGan = text.ConvertToIntDef(100);
                        Debug.Log("灵感已修改为" + player.LingGan);
                    });
                    AddInputField("修为", 150, player.exp.ToString(), PlayerAttributes, (string text) =>
                    {
                        player.exp = (uint)text.ConvertToIntDef(100000);
                        Debug.Log("修为已修改为" + player.exp);
                    });
                    AddInputField("生命值", 150, player.HP_Max.ToString(), PlayerAttributes, (string text) =>
                    {
                        player.HP_Max = text.ConvertToIntDef(1000);
                        player.HP = player.HP_Max;
                        Debug.Log("最大生命值已修改为" + player.HP_Max);
                    });
                }
                hr();
                AddH3("灵根属性：", PlayerAttributes);
                {
                    //string[] LingGengList = { "金", "木", "水", "火", "土" };
                    LingGeng[] LingGengList =
                    {
                        new LingGeng{name= "金", id=0},
                        new LingGeng{name="木", id=1},
                        new LingGeng{name="水", id=2},
                        new LingGeng{name="火", id=3},
                        new LingGeng{name="土", id=4},
                    };

                    foreach (var item in LingGengList)
                    {
                        AddInputField(item.name, 150, player.LingGeng[item.id].ToString(), PlayerAttributes, (string text) =>
                        {
                            player.LingGeng[item.id] = text.ConvertToIntDef(100);
                            Debug.Log(item.name + "灵根已修改为" + player.LingGeng[item.id]);
                        });
                        if (item.id == 3)
                        {
                            hr(10);
                        }
                    }

                }
                hr();

                #endregion

                #region[宗门修改]
                ResetCoordinates(true, true);
                GameObject MenPaiScripts = UIControls.createUIPanel(uiPanel, "410", "600", null);
                MenPaiScripts.GetComponent<Image>().color = UIControls.HTMLString2Color("#424242FF");
                MenPaiScripts.GetComponent<RectTransform>().anchoredPosition = new Vector2(-70, -20);

                AddH3("宗门属性：", MenPaiScripts);
                {
                    {
                        List<string> shiliList = new List<string>();
                        foreach (var item in jsonData.instance.CyShiLiNameData.list)
                        {
                            shiliList.Add(item["name"].Str);

                            //AddButton(item["name"].Str, MenPaiScripts, () =>
                            //{

                            //});
                            //Debug.Log(item["name"].Str);
                        }

                        AddDropdown("修改宗门", 150, shiliList, MenPaiScripts, (int index) =>
                        {
                            player.joinMenPai(jsonData.instance.CyShiLiNameData.list[index]["id"].I);
                            //player.menpai = index;
                            Debug.Log("宗门已修改为" + shiliList[index]);
                        });
                    }
                    {
                        List<string> ChengHao = new List<string> { "无" };
                        foreach (var item in jsonData.instance.ChengHaoJsonData.list)
                        {
                            ChengHao.Add(item["Name"].Str);
                        }
                        AddDropdown("修改职位", 150, ChengHao, MenPaiScripts, (int index) =>
                        {


                            int shiLiID = player.menPai;
                            int chengHaoLevel = jsonData.instance.ChengHaoJsonData.list[index]["id"].I;

                            /**
                             * 不知道为什么，这里需要设置ID为1才能生效
                             * 查了一下游戏源码，GetMenPaiChengHao()方法里面只获取过ID为1的门派
                             * 也就是竹山宗，可能是BUG，也可能是我没找对地方
                             */
                            PlayerEx.SetShiLiChengHaoLevel(1, chengHaoLevel);
                            // 设置ID后会自动修改宗门每月俸禄
                            player.SetChengHaoId(chengHaoLevel);

                            Debug.Log("职位已修改为" + PlayerEx.GetMenPaiChengHao());
                        });
                    }
                }
                hr(10);
                {
                    AddButton("添加声望", MenPaiScripts, () =>
                    {
                        Scripts.AddMenPaiShengWang();
                    });

                    //AddInputField("门派声望", 150, PlayerEx.GetMenPaiShengWang().ToString(), MenPaiScripts, (string text) =>
                    //{
                    //    Debug.Log("门派声望已修改为" + text);
                    //    PlayerEx.AddMenPaiShengWang();
                    //});
                }

                #endregion

                #region[悟道修改]
                ResetCoordinates(true, true);
                GameObject WuDaoScripts = UIControls.createUIPanel(uiPanel, "410", "600", null);
                WuDaoScripts.GetComponent<Image>().color = UIControls.HTMLString2Color("#424242FF");
                WuDaoScripts.GetComponent<RectTransform>().anchoredPosition = new Vector2(-70, -20);

                AddH3("悟道属性", WuDaoScripts);
                {
                    int num = 0;
                    List<string> wudaoList = new List<string>();
                    foreach (var item in jsonData.instance.WuDaoAllTypeJson.list)
                    {
                        AddInputField(item["name"].Str, 150, player.wuDaoMag.getWuDaoEx(item["id"].I).ToString(), WuDaoScripts, (string text) =>
                        {
                            player.wuDaoMag.SetWuDaoEx(item["id"].I, text.ConvertToIntDef(1000));

                            Debug.Log($"悟道{item["name"].Str}修改为{text}");
                        });

                        num++;
                        // 一行4个
                        if (num % 4 == 0)
                        {
                            hr(10);
                        }
                    }
                }
                AddH3("悟道修改", WuDaoScripts);
                {
                    AddButton("一键全满", WuDaoScripts, () =>
                    {
                        foreach (JSONObject item in jsonData.instance.WuDaoAllTypeJson.list)
                        {
                            player.wuDaoMag.SetWuDaoEx(item["id"].I, 150000);
                        }
                        Debug.Pop("所有悟道已全部设置为最高");
                    });
                    AddInputField("悟道点", 150, player._WuDaoDian.ToString(), WuDaoScripts, (string text) =>
                    {
                        player.WuDaoDian = text.ConvertToIntDef(100);
                        Debug.Log($"已将悟道点修改为{player.WuDaoDian}");
                    });
                    hr(10);
                    {
                        int num = 0;
                        foreach (var item in jsonData.instance.WuDaoAllTypeJson.list)
                        {
                            AddButton(item["name"].Str, WuDaoScripts, () =>
                            {
                                player.wuDaoMag.SetWuDaoEx(item["id"].I, 150000);

                                Debug.Pop($"已设置{item["name"].Str}悟道为最高");
                            });
                            num++;
                            if (num % 6 == 0)
                            {
                                hr(10);
                            }
                        }
                    }
                }

                #endregion

                #region[获取物品]
                ResetCoordinates(true, true);
                GameObject ItemScripts = UIControls.createUIPanel(uiPanel, "410", "600", null);
                ItemScripts.GetComponent<Image>().color = UIControls.HTMLString2Color("#424242FF");
                ItemScripts.GetComponent<RectTransform>().anchoredPosition = new Vector2(-70, -20);

                ItemWindow itemWindow = new ItemWindow(ItemScripts, elementX, elementY);


                #endregion

                #region[Npc修改]
                ResetCoordinates(true, true);
                GameObject NpcScripts = UIControls.createUIPanel(uiPanel, "410", "600", null);
                NpcScripts.GetComponent<Image>().color = UIControls.HTMLString2Color("#424242FF");
                NpcScripts.GetComponent<RectTransform>().anchoredPosition = new Vector2(-70, -20);

                NpcWindow npcWindow = new NpcWindow(NpcScripts, elementX, elementY);

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
                    new Navigation("BasicScripts","基础功能", BasicScripts, true),
                    new Navigation("PlayerAttributes","玩家属性", PlayerAttributes, false),
                    new Navigation("MenPaiScripts","宗门修改", MenPaiScripts, false),
                    new Navigation("WuDaoScripts", "悟道修改", WuDaoScripts, false),
                    new Navigation("NpcScripts", "NPC修改", NpcScripts, false),
                    new Navigation("ItemScripts", "获取物品", ItemScripts, false),

                };

                UINavigation.Initialize(nav, NavPanel);

                #endregion

                #endregion

                isVisible = true;

                //log.LogMessage("Complete!");
                canvas.SetActive(optionToggle);
                Debug.Log("修改器初始化完成!");
                Debug.Log($"按{main.ShowCounter.Value.ToString()}可开关修改器菜单");
            }
            else
            {
                //Debug.Log("UI创建失败,未找到玩家");
                initialized = false;
            }
        }

        #region[添加组件]

        // 添加标题
        public static GameObject AddTitle(GameObject parent,string Title)
        {
            GameObject TitleBackground = UIControls.createUIPanel(parent, "30", (width - 20).ToString(), null);
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

        public static GameObject AddButton(string Text, GameObject panel, UnityAction action)
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
        public static GameObject AddToggle(string Text, int width, UnityAction<bool> action)
        {
            // 计算x轴偏移
            elementX += width / 2 - 30;

            Sprite toggleBgSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#3E3E42FF"));
            Sprite toggleSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#18FFFFFF"));
            GameObject uiToggle = UIControls.createUIToggle(uiPanel, toggleBgSprite, toggleSprite);
            uiToggle.GetComponentInChildren<Text>().color = Color.white;
            uiToggle.GetComponentInChildren<Toggle>().isOn = false;
            uiToggle.GetComponent<RectTransform>().localPosition = new Vector3(elementX, elementY, 0);

            uiToggle.GetComponentInChildren<Text>().text = Text;
            uiToggle.GetComponentInChildren<Toggle>().onValueChanged.AddListener(action);


            elementX += width / 2 + 10;

            return uiToggle;
        }

        // 添加输入框
        public static GameObject AddInputField(string Text, int width, string defaultText, GameObject panel, UnityAction<string> action)
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
        public GameObject AddH3(string text, GameObject panel)
        {
            elementX += 40;

            Sprite txtBgSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#7AB900FF"));
            GameObject uiText = UIControls.createUIText(panel, txtBgSprite, "#FFFFFFFF");
            uiText.GetComponent<Text>().text = text;
            uiText.GetComponent<RectTransform>().localPosition = new Vector3(elementX, elementY, 0);

            // 设置字体样式为h3小标题
            uiText.GetComponent<Text>().fontSize = 14;
            uiText.GetComponent<Text>().fontStyle = FontStyle.Bold;
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



        #endregion
    }

    public struct SCard
    {
        public string name;
        public int CardID;
    }

    public struct LingGeng
    {
        public string name;
        public int id;
    }
}
