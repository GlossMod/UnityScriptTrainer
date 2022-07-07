using System;
using System.Collections.Generic;
using System.IO;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;
using System.Collections;
using ConfigurationManager;
using UnityEngine.Experimental.Rendering;
using System.Reflection;
using BepInEx.Logging;
using System.Text.RegularExpressions;

namespace ScriptTrainer
{
    [BepInPlugin("aoe.top.ScriptTrainer", "[戴森球计划] 内置修改器 By:小莫", "2.0")]
    public class ScriptTrainer : BaseUnityPlugin
    {


        private int AddItemNum = 1000;

        private string searchItem = "";


        // 窗口相关
        public static bool DisplayingWindow;

        private Rect HeaderTitleRect;
        private Rect windowRect;
        private Vector2 scrollPosition;

        private Rect DisplayArea;
        private Rect TableRect;


        // 启动按键
        private ConfigEntry<BepInEx.Configuration.KeyboardShortcut> ShowCounter { get; set; }
        private static ConfigEntry<int> userCount;


        // 注入脚本时会自动调用Start()方法 执行在Awake()方法后面
        public void Start()
        {
            // 允许用户自定义启动快捷键
            ShowCounter = Config.Bind("修改器快捷键", "Key", new BepInEx.Configuration.KeyboardShortcut(KeyCode.F9));

            // 自定义默认给予物品数量
            userCount = Config.Bind("启动游戏时默认给予数量", "val:", AddItemNum, new ConfigDescription("你可以根据自己的需求,自由的调整默认给予物品的数量", new AcceptableValueRange<int>(10, 100000)));
            AddItemNum = userCount.Value;

            // 清空之前的日志
            Debug.EmptyLog();
            // 日志输出
            Debug.Log("脚本已启动");

            //计算区域
            this.ComputeRect();
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
        }

        // GUI函数
        private void OnGUI()
        {
            if (DisplayingWindow)
            {
                //Texture2D texture2D = new Texture2D(1, 1, TextureFormat.RGBA32, false);
                //// rgba(116, 125, 140,1.0)
                //texture2D.SetPixel(0, 0, new Color32(116, 125, 140, 255));
                //texture2D.Apply();
                //GUIStyle myWindowStyle = new GUIStyle
                //{
                //    normal = new GUIStyleState  // 正常样式
                //    {
                //        textColor = new Color32(47, 53, 66, 1),
                //        background = texture2D
                //    },
                //    wordWrap = true,    // 自动换行
                //    alignment = TextAnchor.UpperCenter,  //对其方式
                //};
                //// 定义一个新窗口
                //windowRect = GUI.Window(20210219, windowRect, DoMyWindow, "", myWindowStyle);

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
            }
            // 保存修改
            this.ChangeToGame();
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

                    GUILayout.Label("[戴森球计划] 内置修改器 By:小莫", guistyle);
                }
                GUILayout.EndArea();
            }
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// 获取物品数量和搜索
        /// </summary>
        /// <param name="HeaderTableRect"></param>
        void AddItemHeader(Rect HeaderTableRect)
        {
            GUILayout.BeginArea(HeaderTableRect);
            {
                GUILayout.BeginHorizontal(new GUIStyle { alignment = TextAnchor.UpperLeft });
                {

                    // 用户自定义获取数量
                    {
                        GUILayout.Label("获取数量:", new GUIStyle
                        {
                            fixedWidth = 80,
                            fixedHeight = 40,
                            alignment = TextAnchor.MiddleRight,
                            normal = new GUIStyleState
                            {
                                textColor = Color.white
                            }
                        });
                        var ItemText = GUILayout.TextField(AddItemNum.ToString(), new GUIStyle
                        {
                            fixedWidth = 100,
                            fixedHeight = 40,
                            alignment = TextAnchor.MiddleLeft,
                            margin = new RectOffset(5, 0, 0, 0),
                            normal = new GUIStyleState
                            {
                                textColor = Color.white
                            }
                        });
                        ItemText = Regex.Replace(ItemText, @"[^0-9.]", "");
                        try
                        {
                            if (ItemText != null && ItemText.Length < 10 && ItemText.Length != 0)
                            {
                                AddItemNum = Int32.Parse(ItemText);
                            }
                            else
                            {
                                ItemText = 1000.ToString();
                            }
                        }
                        catch (Exception) { throw; }
                    }

                    // 搜索物品
                    {
                        GUILayout.Label("搜索物品:", new GUIStyle
                        {
                            fixedWidth = 80,
                            fixedHeight = 40,
                            alignment = TextAnchor.MiddleRight,
                            normal = new GUIStyleState
                            {
                                textColor = Color.white
                            }
                        });
                        searchItem = GUILayout.TextField(searchItem, new GUIStyle
                        {
                            fixedWidth = 100,
                            fixedHeight = 40,
                            alignment = TextAnchor.MiddleLeft,
                            margin = new RectOffset(5, 0, 0, 0),
                            normal = new GUIStyleState
                            {
                                textColor = Color.white
                            }
                        });
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndArea();
        }

        // 添加物品
        void AddItemTable(Rect AddItemTableRect)
        {

            if (GameMain.mainPlayer == null)
            {
                GUILayout.Label("请先进入游戏", new GUIStyle
                {
                    fontSize = 26,
                    fixedWidth = 700,
                    fixedHeight = 300,
                    alignment = TextAnchor.MiddleCenter
                });
                return;
            }
            Rect HeaderTableRect = new Rect(0, 40, 700, 40);
            AddItemHeader(HeaderTableRect);

            // 物品列表
            ItemProto[] dataArray = LDB.items.dataArray;

            AddItemTableRect.y += 30;

            GUILayout.BeginArea(AddItemTableRect);
            {

                scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false, GUILayout.Width(700), GUILayout.Height(300));
                {
                    GUILayout.BeginHorizontal(new GUIStyle { alignment = TextAnchor.UpperLeft });
                    for (int i = 0; i < dataArray.Length; i++)
                    {
                        var item = dataArray[i];

                        

                        if (searchItem == "")
                        {
                            // 普通模式
                            //if (XmGUI.Button(item.name))
                            if (XmGUI.Button(item.name, item.iconSprite.texture))
                            {

                                int num = AddItemNum;
                                int res = GameMain.mainPlayer.package.AddItemStacked(item.ID, num, 0, out int remainInc);
                                UIItemup.Up(item.ID, num);

                                Debug.addLog("remainInc:" + remainInc);

                                //// 添加物品代码
                                //int ID = 6001;
                                //int num = 1000;
                                //int res = GameMain.mainPlayer.package.AddItemStacked(ID, num);
                                //UIItemup.Up(ID, num);
                            }
                        }
                        else
                        {
                            // 如果用户输入搜索
                            if (item.name.Contains(searchItem))
                            {
                                if (XmGUI.Button(item.name, item.iconSprite.texture))
                                {
                                    int num = AddItemNum;
                                    //int res = GameMain.mainPlayer.package.AddItemStacked(item.ID, num);
                                    int res = GameMain.mainPlayer.package.AddItemStacked(item.ID, num, 0, out int remainInc);
                                    UIItemup.Up(item.ID, num);
                                }
                            }
                        }

                        int listNum = 5;    // 每行个数
                        if ((i + 1) % listNum == 0)
                        {
                            XmGUI.hr();
                        }
                    }
                    if (searchItem == "" || "沙土".Contains(searchItem))
                    {
                        if (XmGUI.Button("沙土"))
                        {
                            GameMain.mainPlayer.SetSandCount(GameMain.mainPlayer.sandCount + AddItemNum);
                        }
                    }

                    GUILayout.EndHorizontal();
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndArea();
        }

        // 解锁科技
        void AddlocktechTable(Rect AddlocktechRect)
        {
            if (GameMain.mainPlayer == null)
            {
                GUILayout.Label("请先进入游戏", new GUIStyle
                {
                    fontSize = 26,
                    fixedWidth = 700,
                    fixedHeight = 300,
                    alignment = TextAnchor.MiddleCenter
                });
                return;
            }

            // 科技列表
            TechProto[] dataArray = LDB.techs.dataArray;

            GUILayout.BeginArea(AddlocktechRect);
            {

                scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false, GUILayout.Width(700), GUILayout.Height(300));
                {
                    GUILayout.BeginHorizontal(new GUIStyle { alignment = TextAnchor.UpperLeft });
                    {
                        if (XmGUI.Button("解锁全部"))
                        {
                            for (int i = 0; i < dataArray.Length; i++)
                            {
                                var item = dataArray[i];
                                GameMain.history.UnlockTech(item.ID);
                            }
                        }
                    }
                    XmGUI.hr(); // 换行
                    for (int i = 0; i < dataArray.Length; i++)
                    {
                        var item = dataArray[i];

                        if (searchItem == "")
                        {
                            
                            // 普通模式
                            if (XmGUI.Button(item.name, item.iconSprite.texture))
                            {
                                //int num = AddItemNum;
                                //int res = GameMain.mainPlayer.package.AddItemStacked(item.ID, num);
                                //UIItemup.Up(item.ID, num);
                                // 解锁科技
                                GameMain.history.UnlockTech(item.ID);
                            }
                        }
                        else
                        {
                            // 如果用户输入搜索
                            if (item.name.Contains(searchItem))
                            {
                                if (XmGUI.Button(item.name, item.iconSprite.texture))
                                {
                                    //int num = AddItemNum;
                                    //int res = GameMain.mainPlayer.package.AddItemStacked(item.ID, num);
                                    //UIItemup.Up(item.ID, num);

                                    // 解锁科技
                                    GameMain.history.UnlockTech(item.ID);
                                }
                            }
                        }

                        int listNum = 5;
                        if ((i + 1) % listNum == 0)
                        {
                            XmGUI.hr();
                        }
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndArea();
        }


        // 无人机数量
        int droneCount = 3;
        int newDroneCount = 3;
        // 无人机速度
        float droneSpeed = 5;
        float newDroneSpeed = 5;
        // 跳跃高度
        float jumpSpeed = 32;
        float newjumpSpeed = 32;
        // 机甲移动速度
        float walkSpeed = 5;
        float newWalkSpeed = 5;
        // 挖掘速度倍率
        float miningSpeed = 1;
        float newMiningSpeed = 1;
        // 制作速度
        float replicateSpeed = 1;
        float newreplicateSpeed = 1;
        // 堆叠倍率
        int oldCount = 1;


        // 保存修改
        void ChangeToGame()
        {
            // 第一行
            {
                if (droneCount != newDroneCount)
                {
                    GameMain.mainPlayer.mecha.droneCount = newDroneCount;
                    Debug.Log("无人机数量修改为" + newDroneCount);
                    droneCount = newDroneCount;
                }
                if (droneSpeed != newDroneSpeed)
                {
                    GameMain.mainPlayer.mecha.droneSpeed = newDroneSpeed;
                    Debug.Log("无人机速度修改为" + newDroneSpeed);
                    droneSpeed = newDroneSpeed;
                }
                if (jumpSpeed != newjumpSpeed)
                {
                    GameMain.mainPlayer.mecha.jumpSpeed = newjumpSpeed;
                    Debug.Log("跳跃高度修改为" + newjumpSpeed);
                    jumpSpeed = newjumpSpeed;
                }
            }
            // 第二行
            {
                if (walkSpeed != newWalkSpeed)
                {
                    GameMain.mainPlayer.mecha.walkSpeed = newWalkSpeed;
                    Debug.Log("航行速度修改为" + newWalkSpeed);
                    walkSpeed = newWalkSpeed;
                }
                if (miningSpeed != newMiningSpeed)
                {
                    GameMain.mainPlayer.mecha.miningSpeed = newMiningSpeed;
                    Debug.Log("挖掘速度倍率修改为" + newMiningSpeed);
                    miningSpeed = newMiningSpeed;
                }
                if (replicateSpeed != newreplicateSpeed)
                {
                    GameMain.mainPlayer.mecha.researchPower = newreplicateSpeed;
                    Debug.Log("制作速度修改为" + newreplicateSpeed);
                    replicateSpeed = newreplicateSpeed;
                }
            }

        }
        // 其他
        void OtherTable(Rect OtherRect)
        {
            // GUILayout.Button(item.name, guistyle)
            
            Texture2D texture2D = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            texture2D.SetPixel(0, 0, new Color32(30, 144, 255, 255));    // rgba(30, 144, 255,1.0)
            texture2D.Apply();
            Texture2D texture2D2 = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            texture2D2.SetPixel(0, 0, new Color32(112, 161, 255, 255));  // rgba(112, 161, 255,1.0)
            texture2D2.Apply();

            // 按钮样式
            GUIStyle guistyle = new GUIStyle
            {
                normal = new GUIStyleState  // 正常样式
                {
                    textColor = Color.white,
                    background = texture2D
                },
                active = new GUIStyleState  // 点击样式
                {
                    textColor = Color.white,
                    background = texture2D2
                },
                wordWrap = true,
                alignment = TextAnchor.MiddleCenter,
                fixedHeight = 40,
                fixedWidth = 90,
                margin = new RectOffset(5, 7, 0, 5),
            };
            
            

            GUILayout.BeginArea(OtherRect);
            {
                scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false, GUILayout.Width(700), GUILayout.Height(300));
                {
                    GUILayout.BeginHorizontal(new GUIStyle { alignment = TextAnchor.UpperLeft });
                    {
                        // 无人机数量
                        {
                            GUILayout.Label("无人机数量:", new GUIStyle
                            {
                                fixedWidth = 80,
                                fixedHeight = 40,
                                alignment = TextAnchor.MiddleRight,
                                normal = new GUIStyleState
                                {
                                    textColor = Color.white
                                }
                            });
                            var ItemText = GUILayout.TextField(newDroneCount.ToString(), new GUIStyle
                            {
                                fixedWidth = 100,
                                fixedHeight = 40,
                                alignment = TextAnchor.MiddleLeft,
                                margin = new RectOffset(5, 0, 0, 0),
                                normal = new GUIStyleState
                                {
                                    textColor = Color.white
                                }
                            });
                            ItemText = Regex.Replace(ItemText, @"[^0-9.]", "");
                            try
                            {
                                if (ItemText != null && ItemText.Length < 10 && ItemText.Length != 0)
                                {
                                    newDroneCount = Int32.Parse(ItemText);
                                }
                                else
                                {
                                    ItemText = newDroneCount.ToString();
                                }
                            }
                            catch (Exception) { throw; }
                        }
                        // 无人机速度
                        {
                            GUILayout.Label("无人机速度:", new GUIStyle
                            {
                                fixedWidth = 80,
                                fixedHeight = 40,
                                alignment = TextAnchor.MiddleRight,
                                normal = new GUIStyleState
                                {
                                    textColor = Color.white
                                }
                            });
                            var ItemText = GUILayout.TextField(newDroneSpeed.ToString(), new GUIStyle
                            {
                                fixedWidth = 100,
                                fixedHeight = 40,
                                alignment = TextAnchor.MiddleLeft,
                                margin = new RectOffset(5, 0, 0, 0),
                                normal = new GUIStyleState
                                {
                                    textColor = Color.white
                                }
                            });
                            ItemText = Regex.Replace(ItemText, @"[^0-9.]", "");
                            try
                            {
                                if (ItemText != null && ItemText.Length < 10 && ItemText.Length != 0)
                                {
                                    newDroneSpeed = Int32.Parse(ItemText);
                                }
                                else
                                {
                                    ItemText = newDroneSpeed.ToString();
                                }
                            }
                            catch (Exception) { throw; }
                        }
                        // 跳跃高度
                        {
                            GUILayout.Label("跳跃高度:", new GUIStyle
                            {
                                fixedWidth = 80,
                                fixedHeight = 40,
                                alignment = TextAnchor.MiddleRight,
                                normal = new GUIStyleState
                                {
                                    textColor = Color.white
                                }
                            });
                            var ItemText = GUILayout.TextField(newjumpSpeed.ToString(), new GUIStyle
                            {
                                fixedWidth = 100,
                                fixedHeight = 40,
                                alignment = TextAnchor.MiddleLeft,
                                margin = new RectOffset(5, 0, 0, 0),
                                normal = new GUIStyleState
                                {
                                    textColor = Color.white
                                }
                            });
                            ItemText = Regex.Replace(ItemText, @"[^0-9.]", "");
                            try
                            {
                                if (ItemText != null && ItemText.Length < 10 && ItemText.Length != 0)
                                {
                                    newjumpSpeed = Int32.Parse(ItemText);
                                }
                                else
                                {
                                    ItemText = newjumpSpeed.ToString();
                                }
                            }
                            catch (Exception) { throw; }
                        }
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(new GUIStyle { alignment = TextAnchor.UpperLeft });
                    {

                        // 挖掘速度倍率
                        {
                            GUILayout.Label("挖掘速度:", new GUIStyle
                            {
                                fixedWidth = 80,
                                fixedHeight = 40,
                                alignment = TextAnchor.MiddleRight,
                                normal = new GUIStyleState
                                {
                                    textColor = Color.white
                                }
                            });
                            var ItemText = GUILayout.TextField(newMiningSpeed.ToString(), new GUIStyle
                            {
                                fixedWidth = 100,
                                fixedHeight = 40,
                                alignment = TextAnchor.MiddleLeft,
                                margin = new RectOffset(5, 0, 0, 0),
                                normal = new GUIStyleState
                                {
                                    textColor = Color.white
                                }
                            });
                            ItemText = Regex.Replace(ItemText, @"[^0-9.]", "");
                            try
                            {
                                if (ItemText != null && ItemText.Length < 10 && ItemText.Length != 0)
                                {
                                    newMiningSpeed = Int32.Parse(ItemText);
                                }
                                else
                                {
                                    ItemText = newMiningSpeed.ToString();
                                }
                            }
                            catch (Exception) { throw; }
                        }
                        // 机甲移动速度
                        {
                            GUILayout.Label("机甲移动速度:", new GUIStyle
                            {
                                fixedWidth = 80,
                                fixedHeight = 40,
                                alignment = TextAnchor.MiddleRight,
                                normal = new GUIStyleState
                                {
                                    textColor = Color.white
                                }
                            });
                            var ItemText = GUILayout.TextField(newWalkSpeed.ToString(), new GUIStyle
                            {
                                fixedWidth = 100,
                                fixedHeight = 40,
                                alignment = TextAnchor.MiddleLeft,
                                margin = new RectOffset(5, 0, 0, 0),
                                normal = new GUIStyleState
                                {
                                    textColor = Color.white
                                }
                            });
                            ItemText = Regex.Replace(ItemText, @"[^0-9.]", "");
                            try
                            {
                                if (ItemText != null && ItemText.Length < 10 && ItemText.Length != 0)
                                {
                                    newWalkSpeed = Int32.Parse(ItemText);
                                }
                                else
                                {
                                    ItemText = newWalkSpeed.ToString();
                                }
                            }
                            catch (Exception) { throw; }
                        }
                        // 物品堆叠
                        {
                            GUILayout.Label("堆叠倍率:", new GUIStyle
                            {
                                fixedWidth = 80,
                                fixedHeight = 40,
                                alignment = TextAnchor.MiddleRight,
                                normal = new GUIStyleState
                                {
                                    textColor = Color.white
                                }
                            });
                            var ItemText = XmGUI.TextField(oldCount.ToString());
                            ItemText = Regex.Replace(ItemText, @"[^0-9.]", "");
                            try
                            {
                                if (ItemText != null && ItemText.Length < 10 && ItemText.Length != 0)
                                {
                                    int newCount = Int32.Parse(ItemText);

                                    if (oldCount != newCount)
                                    {
                                        //Debug.Log($"oldCount的值是：{oldCount}；newCount的值是：{newCount}");
                                        ItemProto[] dataArray = LDB.items.dataArray;
                                        for (int j = 0; j < dataArray.Length; j++)
                                        {

                                            if (oldCount > 0 && newCount > 0)
                                            {
                                                //Debug.Log($"dataArray[j].StackSize是：{dataArray[j].StackSize}");
                                                if (dataArray[j].StackSize == 0)
                                                {
                                                    dataArray[j].StackSize = 100;
                                                }
                                                dataArray[j].StackSize = dataArray[j].StackSize / oldCount * newCount;

                                                //Debug.Log(String.Format("修改物品ID{0}的叠加倍率为{1}", j, dataArray[j].StackSize));
                                                StorageComponent.itemStackCount[dataArray[j].ID] = dataArray[j].StackSize;
                                            }

                                        }
                                        oldCount = newCount;

                                    }

                                }
                                else
                                {
                                    oldCount = 1;
                                    Debug.Log("发生错误");
                                }
                            }
                            catch (Exception) { throw; }

                        }
                        // 制作速度
                        //{
                        //    GUILayout.Label("制作速度:", new GUIStyle
                        //    {
                        //        fixedWidth = 80,
                        //        fixedHeight = 40,
                        //        alignment = TextAnchor.MiddleRight,
                        //        normal = new GUIStyleState
                        //        {
                        //            textColor = Color.white
                        //        }
                        //    });
                        //    var ItemText = GUILayout.TextField(newreplicateSpeed.ToString(), new GUIStyle
                        //    {
                        //        fixedWidth = 100,
                        //        fixedHeight = 40,
                        //        alignment = TextAnchor.MiddleLeft,
                        //        margin = new RectOffset(5, 0, 0, 0),
                        //        normal = new GUIStyleState
                        //        {
                        //            textColor = Color.white
                        //        }
                        //    });
                        //    ItemText = Regex.Replace(ItemText, @"[^0-9.]", "");
                        //    try
                        //    {
                        //        if (ItemText != null && ItemText.Length < 10 && ItemText.Length != 0)
                        //        {
                        //            newreplicateSpeed = Int32.Parse(ItemText);
                        //        }
                        //        else
                        //        {
                        //            ItemText = newreplicateSpeed.ToString();
                        //        }
                        //    }
                        //    catch (Exception) { throw; }
                        //}
                    }
                    GUILayout.EndHorizontal();

                    //GUILayout.BeginHorizontal(new GUIStyle { alignment = TextAnchor.UpperLeft });
                    //{
                    //    if (GUILayout.Button("潮汐锁定", guistyle))
                    //    {
                    //        GameMain.mainPlayer.planetData.singularity = EPlanetSingularity.TidalLocked;
                    //        // Load
                    //        GameMain.mainPlayer.planetData.Load();
                    //    }
                    //    if (GUILayout.Button("行星类型",guistyle))
                    //    {
                    //        GameMain.mainPlayer.planetData.type = EPlanetType.Ice;
                    //        GameMain.mainPlayer.planetData.Load();
                    //    }
                    //}
                    //GUILayout.EndHorizontal();
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndArea();
        }
        // 显示窗口
        void DoMyWindow(int winId)
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

                    // 添加物品
                    if (window.TabButtonStaty.GetWindowStat<windowsStat>("AddItem"))
                    {
                        AddItemTable(TableRect);
                    }

                    // 解锁科技
                    if (window.TabButtonStaty.GetWindowStat<windowsStat>("unlocktech"))
                    {
                        AddlocktechTable(TableRect);
                    }

                    // 其他
                    if (window.TabButtonStaty.GetWindowStat<windowsStat>("Other"))
                    {
                        OtherTable(TableRect);
                    }

                }
                GUILayout.EndArea();
            }
            GUILayout.EndHorizontal();

            GUI.DragWindow();
        }

    }


}