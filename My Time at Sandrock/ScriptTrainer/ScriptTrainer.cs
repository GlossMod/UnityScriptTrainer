using BepInEx;
using BepInEx.Configuration;
using Pathea;
using Pathea.ActorNs;
using Pathea.CookingNs;
using Pathea.FrameworkNs;
using Pathea.HomeNs;
using Pathea.IllustrationNs;
using Pathea.MissionNs;
using Pathea.ProficiencyNs;
using Pathea.SendGiftNs;
using Pathea.StoreNs;
using UnityEngine;

namespace ScriptTrainer
{
    [BepInPlugin("aoe.top.plugins.Sandrock_ScriptTrainer", "沙石镇时光 内置修改器", "1.0.0.0")]
    public class ScriptTrainer : BaseUnityPlugin
    {
        // 窗口相关
        public static bool DisplayingWindow;

        private Rect HeaderTitleRect;
        private Rect windowRect;
        private Vector2 scrollPosition;

        private Rect DisplayArea;
        private Rect TableRect;

        // 光标相关
        RayBlocker rb;

        // 启动按键
        private ConfigEntry<BepInEx.Configuration.KeyboardShortcut> ShowCounter { get; set; }

        // 注入脚本时会自动调用Start()方法 执行在Awake()方法后面
        public void Start()
        {
            // 允许用户自定义启动快捷键
            ShowCounter = Config.Bind("修改器快捷键", "Key", new BepInEx.Configuration.KeyboardShortcut(KeyCode.F9));
                       
            // 日志输出
            Debug.Log("脚本已启动");

            //计算区域
            this.ComputeRect();

            rb = RayBlocker.CreateRayBlock();
        }

        public void Update()
        {
            if (ShowCounter.Value.IsDown())
            {
                //Debug.Log("按下按键");
                DisplayingWindow = !DisplayingWindow;
                
                if (DisplayingWindow)
                {
                    Debug.Log("打开窗口");

                }
                else
                {
                    Debug.Log("关闭窗口");
                    rb.CloseBlocker();
                }
                Script.CursorReset(DisplayingWindow);
            }
        }


        // GUI函数
        private void OnGUI()
        {
            if (DisplayingWindow)
            {

                Texture2D texture2D = new Texture2D(1, 1, TextureFormat.RGBA32, false);
                // rgba(116, 125, 140,1.0)
                texture2D.SetPixel(0, 0, new Color32(51, 51, 51, 255));
                texture2D.Apply();
                GUIStyle myWindowStyle = new GUIStyle
                {
                    normal = new GUIStyleState  // 正常样式
                    {
                        textColor = new Color32(47, 53, 66, 1),
                        background = texture2D
                    },
                    wordWrap = true,    // 自动换行
                                        //alignment = TextAnchor.UpperCenter,  //对齐方式
                };
                // 定义一个新窗口
                int winId = 20210630;
                windowRect = GUI.Window(winId, windowRect, DoMyWindow, "", myWindowStyle);


                new UI.XmUIStyle(); // 修正样式

                float windowW = 210f;
                window.RightWindow(new Rect(windowRect.x + windowRect.width, windowRect.y + 40, windowW, windowRect.height));   // 右侧菜单
                window.CloseButton(new Rect(windowRect.x + windowRect.width, windowRect.y, 80, 40)); // 关闭按钮

                rb.SetSize(windowRect);

                rb.OpenBlocker();

            }
            else
            {
                rb.CloseBlocker();
            }

        }

        // 初始样式
        void ComputeRect()
        {

            // 主窗口居中
            int num = Mathf.Min(Screen.width, 740);
            int num2 = (Screen.height < 400) ? Screen.height : (450);
            int num3 = Mathf.RoundToInt((Screen.width - num) / 2f);
            int num4 = Mathf.RoundToInt((Screen.height - num2) / 2f);
            windowRect = new Rect(num3, num4, num, num2);

            DisplayArea = new Rect(15, 15, (float)num - 30, (float)num2 - 30);

            // 头部
            HeaderTitleRect = new Rect(5, 5, (float)num - 40, (float)num2 - 40);

            // 中间窗口
            TableRect = new Rect(0, 40, (float)num - 30, 1000);
        }

        // 头部标题
        void HeaderTitle(Rect HeaderTitleRect)
        {

            //// 结尾
            //GUILayout.EndHorizontal();
            //GUILayout.EndArea();

            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginArea(HeaderTitleRect);
                {
                    Texture2D texture2D = new Texture2D(1, 1, TextureFormat.RGBA32, false);
                    // rgba(255, 99, 72,1.0)
                    texture2D.SetPixel(0, 0, new Color32(51, 51, 51, 255));
                    texture2D.Apply();
                    GUIStyle guistyle = new GUIStyle
                    {
                        normal = new GUIStyleState
                        {
                            textColor = Color.white,
                            background = texture2D
                        },
                        wordWrap = true,
                        alignment = TextAnchor.MiddleCenter,
                        fixedHeight = 30,
                        fontSize = 16
                    };

                    GUILayout.Label("[沙石镇时光] 内置修改器 By:小莫", guistyle);                   
                }
                GUILayout.EndArea();
            }
            GUILayout.EndHorizontal();

            window.CheckUpdatesButon();
        }

        private void DoMyWindow(int id)
        {
            GUILayout.BeginHorizontal();
            {
                Texture2D texture2D = new Texture2D(1, 1, TextureFormat.RGBA32, false);
                texture2D.SetPixel(0, 0, new Color32(69, 69, 69, 255));
                texture2D.Apply();
                GUIStyle guistyle = new GUIStyle
                {
                    normal = new GUIStyleState  // 正常样式
                    {
                        textColor = new Color32(47, 53, 66, 1),
                        background = texture2D
                    },
                    wordWrap = true,    // 自动换行
                    alignment = TextAnchor.UpperCenter,  //对齐方式
                };

                GUILayout.BeginArea(DisplayArea, guistyle);
                {
                    // 渲染头部标题
                    HeaderTitle(HeaderTitleRect);

                    // 基础功能
                    if (window.TabButtonStaty.GetWindowStat<windowsStat>("BasicScripts"))
                    {
                        BasicScriptsTable(TableRect);
                    }
                    // 玩家选项
                    if (window.TabButtonStaty.GetWindowStat<windowsStat>("Player"))
                    {
                        PlayerTable(TableRect);
                    }
                    // 获取武器
                    if (window.TabButtonStaty.GetWindowStat<windowsStat>("GetItem"))
                    {
                        GetItemTable(TableRect);
                    }
                }
                GUILayout.EndArea();

                GUILayout.EndHorizontal();
            }            

            GUI.DragWindow();
        }

        /// <summary>
        /// 基础功能
        /// </summary>
        /// <param name="tableRect"></param>
        private void BasicScriptsTable(Rect TableRect)
        {

            GUILayout.BeginArea(TableRect);
            {
                scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false, GUILayout.Width(700), GUILayout.Height(300));
                {
                    Player player = Module<Player>.Self;

                    MiscCmd cmd = new MiscCmd();

                    XmGUI.Title("基础功能");
                    GUILayout.BeginHorizontal(new GUIStyle { alignment = TextAnchor.UpperLeft });
                    {
                        if (XmGUI.Button("获取所有道具"))
                        {
                            cmd.AddAllItem();
                        }
                        if (XmGUI.Button("清空背包"))
                        {
                            cmd.ClearBag();
                        }
                        if (XmGUI.Button("添加随机订单"))
                        {
                            Module<OrderMissionManager>.Self.DeliverOrder();
                            Module<OrderMissionManager>.Self.RefreshRandom();
                        }
                        if (XmGUI.Button("清除订单板"))
                        {
                            Module<SceneItemManager>.Self.ClearMissionBoard(OrderMissionType.Item);
                            Module<OrderMissionManager>.Self.ClearRecentDeliver();
                        }
                        if (XmGUI.Button("存档"))
                        {
                            Module<Pathea.SaveNs.Save>.Self.RequestAuto();
                        }
                        if (XmGUI.Button("刷新商店商品"))
                        {
                            foreach (Store store in Module<StoreModule>.Self.GetAllStore())
                            {
                                store.Refresh();
                            }
                        }
                        if (XmGUI.Button("解锁全部菜谱"))
                        {
                            Module<CookingModule>.Self.UnlockAllCookingFormulas();
                        }
                        
                        XmGUI.hr();

                        if (XmGUI.Button("知识最大经验"))
                        {
                            Module<ProficiencyModule>.Self.SetAllProficiencyExpMax();
                        }
                        if (XmGUI.Button("重置知识点"))
                        {
                            Module<ProficiencyModule>.Self.ResetAllProficiencyApplyPoint();
                        }
                        if (XmGUI.Button("解锁所有收集"))
                        {
                            foreach (IllustrationConfig illustrationConfig in Module<IllustrationModule>.Self.Configs)
                            {
                                Module<IllustrationModule>.Self.SetState(illustrationConfig.id, StateType.Unlocked);
                            }
                        }
                        

                        XmGUI.hr();

                        {
                            XmGUI.Label("不消耗体力");
                            // actor.AddBuff(11);

                            XmGUI.Switch(constant.enduranceSwitch, () => {
                                player.actor.AddBuff(11);
                                constant.enduranceSwitch = true;
                            }, () => {
                                player.actor.RemoveBuff(11);
                                constant.enduranceSwitch = false;
                            });
                        }

                        {
                            XmGUI.Label("无掉落伤害");
                            XmGUI.Switch(constant.FallInjurySwitch, () => {
                                constant.actor.AddFallInjuryLocker(new Locker());
                                constant.FallInjurySwitch = true;
                            }, () => {
                                constant.actor.RemoveFallInjuryLocker(new Locker());
                                constant.FallInjurySwitch = false;
                            });
                        }
                        {
                            XmGUI.Label("无限送礼");
                            XmGUI.Switch(Module<SendGiftModule>.Self.Config.UnlimitedTodayGift, () => {
                                Module<SendGiftModule>.Self.Config.UnlimitedTodayGift = true;
                            }, () => {
                                Module<SendGiftModule>.Self.Config.UnlimitedTodayGift = false;
                            });
                        }
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndArea();
        }

        /// <summary>
        /// 玩家选项
        /// </summary>
        /// <param name="TableRect"></param>
        private void PlayerTable(Rect TableRect)
        {
            GUILayout.BeginArea(TableRect);
            {
                scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false, GUILayout.Width(700), GUILayout.Height(300));
                { 
                    Player player = Module<Player>.Self;

                    XmGUI.Title("玩家选项");
                    GUILayout.BeginHorizontal(new GUIStyle { alignment = TextAnchor.UpperLeft });
                    {
                        {
                            if (XmGUI.Button("增加经验"))
                            {
                                player.AddExp(1000, null, true);
                            }
                        }
                        {
                            //  player.AddExp(exp, null, true);
                            XmGUI.Label("修改现金");
                            var Souls = XmGUI.TextField(player.bag.Gold.ToString());
                            Script.ChangeGold(Script.CheckIsLong(Souls), player.bag.Gold, player);
                            //player.bag.ChangeGold();
                        }
                        {
                            XmGUI.Label("农场等级");
                            var Souls = XmGUI.TextField(Module<HomeModule>.Self.FarmLevel.ToString());
                            Module<HomeModule>.Self.FarmLevel = Script.CheckIsInt(Souls);
                        }
                        
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndArea();
        }

        /// <summary>
        /// 获取物品
        /// </summary>
        /// <param name="TableRect"></param>
        private void GetItemTable(Rect TableRect)
        {
            GUILayout.BeginArea(TableRect);
            {
                scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false, GUILayout.Width(700), GUILayout.Height(700));
                {

                    GUILayout.BeginHorizontal(new GUIStyle { alignment = TextAnchor.UpperLeft });
                    {
                        XmGUI.Label("数量", 30, 40);
                        constant.count = Script.CheckIsInt(XmGUI.TextField(constant.count.ToString(), 40, 40));
                        XmGUI.Label("搜索");
                        constant.search = XmGUI.TextField(constant.search);

                        ItemData itemdata = new ItemData();

                        itemdata.ItemWindow(constant.count, constant.search);
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndArea();
        }

    }
}
