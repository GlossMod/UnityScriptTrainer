using BepInEx;
using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


namespace ScriptTrainer
{
    [BepInPlugin("aoe.top.plugins.SW_ScriptTrainer", "暖雪 内置修改器", "1.0.0.0")]
    public class ScriptTrainer: BaseUnityPlugin
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
                }
            }


           
            //SetSize(windowRect);
            //if (DisplayingWindow)
            //{
            //    OpenBlocker();
            //}
            //else
            //{
            //    CloseBlocker();
            //}
           
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

                Input.ResetInputAxes();
                Cursor.visible = true;

                rb.SetSize(windowRect);

                rb. OpenBlocker();

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
            TableRect = new Rect(0, 40, (float)num - 30, 600);
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

                    GUILayout.Label("[暖雪] 内置修改器 By:小莫", guistyle);
                }
                GUILayout.EndArea();
            }
            GUILayout.EndHorizontal();
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
                    // 获取道具
                    if (window.TabButtonStaty.GetWindowStat<windowsStat>("GetProps"))
                    {
                        GetPropsTable(TableRect);
                    }
                    // 获取武器
                    if (window.TabButtonStaty.GetWindowStat<windowsStat>("GetWeapon"))
                    {
                        GetWeaponTable(TableRect);
                    }
                }
                GUILayout.EndArea();

            }
            GUILayout.EndHorizontal();

            GUI.DragWindow();
        }

        /// <summary>
        /// 基础功能
        /// </summary>
        /// <param name="tableRect"></param>
        private void BasicScriptsTable(Rect TableRect)
        {
            PlayerAnimControl player = PlayerAnimControl.instance;

            GUILayout.BeginArea(TableRect);
            {
                scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false, GUILayout.Width(700), GUILayout.Height(300));
                {
                    XmGUI.Title("基础功能");

                    GUILayout.BeginHorizontal(new GUIStyle { alignment = TextAnchor.UpperLeft });
                    {
                        {
                            XmGUI.Label("官方作弊");

                            XmGUI.Switch(player.isCheat, () => { player.isCheat = true; }, () => { player.isCheat = false; });
                            
                                                    
                        }
                        {
                            XmGUI.Label("灵魂", 40, 40);
                            var Souls = XmGUI.TextField(player.Souls.ToString());
                            player.Souls = Script.CheckIsInt(Souls);
                        }
                        {
                            XmGUI.Label("红魂", 40, 40);
                            var Souls = XmGUI.TextField(player.RedSouls.ToString());
                            player.RedSouls = Script.CheckIsInt(Souls);
                        }
                        {
                            if (XmGUI.Button("血量回满", 80, 20))
                            {
                                player.playerParameter.HP = player.playerParameter.MAX_HP;
                            }
                        }
                        
                        XmGUI.hr();
                        {
                            XmGUI.Label("移动速度", 75, 40);
                            var ItemText = XmGUI.TextField(player.playerParameter.RUN_SPEED_RATE.ToString());
                            player.playerParameter.RUN_SPEED_RATE = Script.CheckIsFloat(ItemText);
                        }
                        {
                            XmGUI.Label("攻击速度", 75, 40);
                            var ItemText = XmGUI.TextField(player.playerParameter.EXTRA_ATTACK_SPEED.ToString());
                            player.playerParameter.EXTRA_ATTACK_SPEED = Script.CheckIsFloat(ItemText);
                        }
                        {
                            XmGUI.Label("生命值");

                            if (XmGUI.Button("-100"))
                            {
                                player.attrChange.SubMaxHp(100);
                            }
                            XmGUI.Label(player.playerParameter.MAX_HP.ToString());
                           
                            if (XmGUI.Button("+100"))
                            {
                                player.attrChange.AddMaxHp(100);
                            }
                        }
                        XmGUI.hr();
                        {
                            if (XmGUI.Button("获取技能"))
                            {
                                SkillDropPool.instance.Pop(player.transform.position, true);
                            }
                        }
                    

                    }
                    GUILayout.EndHorizontal();

                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndArea();
        }

        private string PropsSearch = "";
        /// <summary>
        /// 获取道具
        /// </summary>
        /// <param name="TableRect"></param>
        private void GetPropsTable(Rect TableRect)
        {
            PlayerAnimControl player = PlayerAnimControl.instance;

            GUILayout.BeginArea(TableRect);
            {
                scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false, GUILayout.Width(700), GUILayout.Height(400));
                {
                    
                    
                    GUILayout.BeginHorizontal(new GUIStyle { alignment = TextAnchor.UpperLeft });
                    {
                        XmGUI.Title("获取道具");
                        XmGUI.Label("搜素", 40, 40);
                        PropsSearch = XmGUI.TextField(PropsSearch);
                        XmGUI.hr();
                        int num = 0;
                        foreach (PN item in Enum.GetValues(typeof(PN)))
                        {
                            
                            Potion p = new Potion();
                            p.PotionName = item;
                            if (item != PN.None)
                            {
                                if (PropsSearch != "")
                                {
                                    string name = TextControl.instance.PotionTitle(p, false);
                                    if (name.Contains(PropsSearch))
                                    {
                                        if (XmGUI.Button(TextControl.instance.PotionTitle(p, false)))
                                        {

                                            PotionDropPool.instance.Pop(item, 2, player.transform.position);
                                        }
                                    }
                                }
                                else
                                {
                                    if (XmGUI.Button(TextControl.instance.PotionTitle(p, false)))
                                    {

                                        PotionDropPool.instance.Pop(item, 2, player.transform.position);
                                    }
                                }

                                num++;
                                if (num > 7)
                                {
                                    num = 0;
                                    XmGUI.hr();
                                }
                            }
                        }
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndArea();

        }

        string WeaponSearch = "";
        /// <summary>
        /// 获取武器
        /// </summary>
        /// <param name="TableRect"></param>
        private void GetWeaponTable(Rect TableRect)
        {
            PlayerAnimControl player = PlayerAnimControl.instance;

            GUILayout.BeginArea(TableRect);
            {
                scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false, GUILayout.Width(700), GUILayout.Height(400));
                {

                    
                    GUILayout.BeginHorizontal(new GUIStyle { alignment = TextAnchor.UpperLeft });
                    {
                        XmGUI.Title("获取武器");
                        XmGUI.Label("搜素", 40, 40);
                        WeaponSearch = XmGUI.TextField(WeaponSearch);
                        XmGUI.hr();
                        int num = 0;

                        // MagicSwordName
                        foreach (MagicSwordName item in Enum.GetValues(typeof(MagicSwordName)))
                        {
                            MagicSword ms = new MagicSword();
                            ms.magicSwordName = item;
                            var m = TextControl.instance.MagicSwordInfo(ms);

                            if (WeaponSearch != "")
                            {
                                if (m[0].Contains(WeaponSearch))
                                {
                                    if (XmGUI.Button(m[0]))
                                    {
                                        List<MagicSwordEntry> entrys = MagicSwordControl.instance.RandomEntrys(MagicSwordName.BaoNu, 3);

                                        MagicSwordPool.instance.Pop((int)item, 3, entrys, player.transform.position);
                                    }
                                }
                            }
                            else
                            {
                                if (XmGUI.Button(m[0]))
                                {
                                    List<MagicSwordEntry> entrys = MagicSwordControl.instance.RandomEntrys(MagicSwordName.BaoNu, 3);

                                    MagicSwordPool.instance.Pop((int)item, 3, entrys, player.transform.position);
                                }
                            }

                            

                            num++;
                            if (num > 7)
                            {
                                num = 0;
                                XmGUI.hr();
                            }
                        }

                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndArea();
        }






















    }
}
