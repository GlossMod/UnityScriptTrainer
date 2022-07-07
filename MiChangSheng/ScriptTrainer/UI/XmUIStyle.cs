using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ScriptTrainer.UI
{
    class XmUIStyle
    {
        // 默认按钮样式
        private static GUIStyle myButtonStyle = GetButtonStyle();
        private static GUIStyle myLabelStyle = GetLabelStyle();
        public XmUIStyle()
        {
            myButtonStyle = GetButtonStyle();
            myLabelStyle = GetLabelStyle();
        }

        public static GUIStyle ButtonStyle
        {
            get { return myButtonStyle; }
        }
        public static GUIStyle LabelStyle
        {
            get { return myLabelStyle; }
        }

        /// <summary>
        /// 默认按钮样式
        /// </summary>
        /// <returns></returns>
        private static GUIStyle GetButtonStyle()
        {
            Texture2D texture2D = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            texture2D.SetPixel(0, 0, new Color32(30, 144, 255, 255));    // rgba(30, 144, 255,1.0)
            texture2D.Apply();
            Texture2D texture2D2 = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            texture2D2.SetPixel(0, 0, new Color32(112, 161, 255, 255));  // rgba(112, 161, 255,1.0)
            texture2D2.Apply();

            GUIStyle guistyle = new GUIStyle()
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
                fixedWidth = 80,
                margin = new RectOffset(5, 7, 0, 5),
            };
            return guistyle;
        }

        /// <summary>
        /// 默认label样式
        /// </summary>
        /// <returns></returns>
        private static GUIStyle GetLabelStyle()
        {
            GUIStyle guistyle = new GUIStyle()
            {
                fixedWidth = 50,
                fixedHeight = 40,
                alignment = TextAnchor.MiddleRight,
                wordWrap = true,
                normal = new GUIStyleState
                {
                    textColor = Color.white
                }
            };

            return guistyle;
        }

    }
}
