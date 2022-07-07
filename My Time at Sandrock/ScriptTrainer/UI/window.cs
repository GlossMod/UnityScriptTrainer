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
            // GlobalParameter.instance.EndingBranchStoryChips
            new windowsStat(){Key = "Player", Value = false , Text = "玩家选项"},
            new windowsStat(){Key = "GetItem", Value = false , Text = "获取道具"},
            //new windowsStat(){Key = "playerItem", Value = false, Text = "物品"},
            //new windowsStat(){Key = "NPC", Value = false, Text = "NPC选项"}
        };


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
                    Script.CursorReset(false);
                }
            }
            GUILayout.EndArea();
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
                            Debug.Log("切换窗口为：" + item.Text);
                        }
                    }
                }
                GUILayout.EndArea();
            }
        }

        private static string text = "无法检查更新";
        public static void CheckUpdatesButon()
        {
            GUILayout.BeginArea(new Rect(600,10,100,100));
            {

                if (!constant.isCheck)
                {
                    constant.isCheck = true;
                    new CheckUpdates(out text);
                }                

                if (XmGUI.Button(text, 100, 20))
                {
                    // 打开网页 https://mod.3dmgame.com/mod/185597
                    Application.OpenURL("https://mod.3dmgame.com/mod/185597");
                    
                }
            }
            GUILayout.EndArea();
        }
    }

}