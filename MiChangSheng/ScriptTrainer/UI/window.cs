using Fungus;
using GUIPackage;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptTrainer
{


    public class window : MonoBehaviour
    {
        //public static bool MenPaiWindowStat = false;
        //public static bool ShiLiChengHaoStat = false;
        public delegate void WindowContent();

        public static List<windowsStat> windowStaty = new List<windowsStat>
        {
            new windowsStat(){ Key = "MenPaiWindowStat",Value =  false },
            new windowsStat(){ Key = "ShiLiChengHaoStat",Value = false }
        };

        public static List<windowsStat> TabButtonStaty = new List<windowsStat>
        {
            new windowsStat(){Key = "BasicScripts", Value = true , Text = "基础功能"},
            new windowsStat(){Key = "PlayerAttributes", Value = false , Text = "玩家属性"},
            new windowsStat(){Key = "PlayerWuDao", Value = false , Text = "悟道"},
            new windowsStat(){Key = "playerItem", Value = false, Text = "物品"},
            new windowsStat(){Key = "NPC", Value = false, Text = "NPC选项"}
        };


        /// <summary>
        /// 显示左侧窗口
        /// </summary>
        /// <param name="position">坐标</param>
        /// <param name="content">窗体内容</param>
        /// <param name="title">窗体标题</param>
        public static void LeftWindow(Rect position)
        {
            if (windowStaty.GetWindowStat<windowsStat>())
            {
                GUILayout.BeginHorizontal();
                {
                    Texture2D texture2D = new Texture2D(1, 1, TextureFormat.RGBA32, false);
                    texture2D.SetPixel(0, 0, new Color32(51, 51, 51, 255));
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
                    GUILayout.BeginArea(position, guistyle);
                    {
                        if (windowStaty.GetWindowStat<windowsStat>("MenPaiWindowStat")) ShowContent(new Rect(15, 15, position.width - 30, position.height - 30), MenPaiWindow);
                        if (windowStaty.GetWindowStat<windowsStat>("ShiLiChengHaoStat")) ShowContent(new Rect(15, 15, position.width - 30, position.height - 30), ShiLiChengHao);

                        //ShowContent(new Rect(15, 15, position.width - 30, position.height - 30), content);

                    }
                    GUILayout.EndArea();
                }
                GUILayout.EndHorizontal();
            }
        }
        /// <summary>
        /// 显示内容
        /// </summary>
        /// <param name="position"></param>
        /// <param name="content"></param>
        public static void ShowContent(Rect position, WindowContent content)
        {
            Texture2D texture2D = new Texture2D(1, 1, TextureFormat.RGBA32, true);
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

            GUILayout.BeginArea(position, guistyle);
            {
                GUILayout.BeginHorizontal(new GUIStyle { alignment = TextAnchor.UpperLeft });
                {
                    content();
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndArea();
        }
        /// <summary>
        /// 门派列表窗口
        /// </summary>
        public static void MenPaiWindow()
        {
            KBEngine.Avatar player = Tools.instance.getPlayer();    // 获取玩家
            int num = 0;

            foreach (JSONObject jsonobject in jsonData.instance.CyShiLiNameData.list)
            {
                if (XmGUI.Button(jsonobject["name"].Str))
                {
                    player.menPai = (ushort)jsonobject["id"].I;
                    player.onMenPaiChanged((ushort)jsonobject["id"].I);
                    windowStaty.ChangeWindowStat<windowsStat>("MenPaiWindowStat", false);

                    Debug.addLog(String.Format("修改玩家门派为：{0}|{1}", jsonobject["name"].Str, jsonobject["id"].I));
                }
                num++;
                if (num >= 2)
                {
                    XmGUI.hr();
                    num = 0;
                }
            }
        }
        /// <summary>
        /// 职位/称号 列表窗口
        /// </summary>
        public static void ShiLiChengHao()
        {
            KBEngine.Avatar player = Tools.instance.getPlayer();    // 获取玩家
            int num = 0;
            foreach (JSONObject jsonobject in jsonData.instance.ChengHaoJsonData.list)
            {
                if (XmGUI.Button(jsonobject["Name"].Str))
                {
                    //PlayerEx.SetShiLiChengHaoLevel(player.menPai, jsonobject["id"].I + 1);
                    //player.ShiLiChengHaoLevel.SetField(player.menPai.ToString(), jsonobject["id"].I + 1);

                    windowStaty.ChangeWindowStat<windowsStat>("ShiLiChengHaoStat", false);

                    string menPaiName = Tools.Code64(jsonData.instance.ShiLiHaoGanDuName[player.menPai.ToString()]["ChinaText"].str);
                    Debug.addLog(String.Format("修改玩家职位/称号，门派：{0}|{1},称号：{2}|{3}", menPaiName, PlayerEx.Player.menPai, jsonobject["Name"].Str, jsonobject["id"].I));

                    Debug.addLog(PlayerEx.GetMenPaiChengHao());
                }
                num++;
                if (num >= 2)
                {

                    XmGUI.hr();
                    num = 0;
                }
            }
        }
        /// <summary>
        /// 修改玩家血量
        /// </summary>
        public static void PlayerHP()
        {
            KBEngine.Avatar player = Tools.instance.getPlayer();    // 获取玩家

            var HP = XmGUI.TextField(player.HP.ToString(), 50, 40);
            player.HP = Script.CheckIsInt(HP);
            XmGUI.Label("/", 10, 40);
            var MaxHP = XmGUI.TextField(oldHP_Max.ToString(), 50, 40);
            if (MaxHP != oldHP_Max.ToString())
            {
                Debug.addLog(String.Format("MaxHP:{0},oldHP_Max:{1}", MaxHP, oldHP_Max));
                player._HP_Max = Script.CheckIsInt(MaxHP);
                oldHP_Max = player.HP_Max;
            }
        }
        /// <summary>
        /// 修改玩家神识
        /// </summary>
        public static void PlayerShengShi(KBEngine.Avatar player)
        {
            //KBEngine.Avatar player = Tools.instance.getPlayer();    // 获取玩家

            var ItemText = XmGUI.TextField(oldShengShi.ToString());
            if (ItemText != oldShengShi.ToString())
            {
                player.shengShi = Script.CheckIsInt(ItemText);
                oldShengShi = player.shengShi;
            }
        }
        /// <summary>
        /// 修改玩家遁速
        /// </summary>
        /// <param name="player"></param>
        public static void PlayerDunSu(KBEngine.Avatar player)
        {
            var ItemText = XmGUI.TextField(oldDunSu.ToString());
            if (ItemText != oldDunSu.ToString())
            {
                player.dunSu = Script.CheckIsInt(ItemText);
                oldDunSu = player.dunSu;
            }
        }

        /// <summary>
        /// 显示右侧TAB按钮组
        /// </summary>
        /// <param name="position"></param>
        public static void RightWindow(Rect position)
        {
            if (TabButtonStaty.GetWindowStat<windowsStat>())
            {

                GUILayout.BeginArea(position);
                {
                    foreach (var item in TabButtonStaty)
                    {
                        if (XmGUI.Button(item.Text, item.Value))
                        {
                            TabButtonStaty.ChangeWindowStat<windowsStat>(item.Key, true);
                            Debug.addLog("切换窗口为：" + item.Text);
                        }
                    }
                }
                GUILayout.EndArea();
            }
        }

        /// <summary>
        /// 关闭按钮
        /// </summary>
        /// <param name="position"></param>
        public static void CloseButton(Rect position)
        {
            GUILayout.BeginArea(position);
            {
                Texture2D normal = new Texture2D(1, 1, TextureFormat.RGBA32, false);
                normal.SetPixel(0, 0, new Color32(255, 107, 129, 255));    // rgb(255, 107, 129)
                normal.Apply();
                Texture2D active = new Texture2D(1, 1, TextureFormat.RGBA32, false);
                active.SetPixel(0, 0, new Color32(112, 161, 255, 255));  // rgba(112, 161, 255,1.0)
                active.Apply();

                if (XmGUI.Button("关闭", normal, active))
                {
                    ScriptTrainer.DisplayingWindow = false;
                }
            }
            GUILayout.EndArea();
        }

        public static void NPCExp(UINPCData npc)
        {
            var Exp = XmGUI.TextField(npc.Exp.ToString(), 50, 40);
            npc.Exp = Script.CheckIsInt(Exp);
            XmGUI.Label("/", 10, 40);
            XmGUI.Label(jsonData.instance.LevelUpDataJsonData[npc.Level.ToString()]["MaxExp"].I.ToString(), 50, 40);
            if (XmGUI.Button("满", 20, 20))
            {
                npc.Exp = jsonData.instance.LevelUpDataJsonData[npc.Level.ToString()]["MaxExp"].I;
            }
        }

        private static int oldHP_Max = Tools.instance.getPlayer().HP_Max;   // 玩家最大血量
        private static int oldShengShi = Tools.instance.getPlayer().shengShi;   // 玩家神识
        private static int oldDunSu = Tools.instance.getPlayer().dunSu;         // 玩家遁速

        
    }

}