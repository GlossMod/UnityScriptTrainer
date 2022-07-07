using ScriptTrainer.UI;
using System;
using UnityEngine;

namespace ScriptTrainer
{
    public class XmGUI : MonoBehaviour
    {
        public delegate void PaginationDo(int page);

        public static void Title(string text)
        {
            GUILayout.BeginHorizontal();
            {
                GUIStyle guistyle = new GUIStyle()
                {
                    fixedWidth = 100,
                    fixedHeight = 40,
                    alignment = TextAnchor.MiddleLeft,
                    margin = new RectOffset(5, 0, 0, 0),
                    normal = new GUIStyleState
                    {
                        textColor = Color.white
                    }
                };
                // UILabel newLabel = new UILabel();
                GUILayout.Label(text, guistyle);
            }
            GUILayout.EndHorizontal();
        }

        public static bool Button(string text)
        {
            return GUILayout.Button(text, XmUIStyle.ButtonStyle);
        }
        public static bool Button(string text, int width, int height)
        {
            GUIStyle guistyle = XmUIStyle.ButtonStyle;
            guistyle.fixedWidth = width;
            guistyle.fixedHeight = height;
            guistyle.margin = new RectOffset(5, 7, (40 - height) / 2, 5);

            return GUILayout.Button(text, guistyle);
        }
        public static bool Button(Rect position, string text)
        {
            GUIStyle guistyle = XmUIStyle.ButtonStyle;
            return GUI.Button(position, text, guistyle);
        }
        public static bool Button(string text, bool stat)
        {
            Texture2D texture2D = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            if (stat)
            {
                texture2D.SetPixel(0, 0, new Color32(51, 51, 51, 255));    // rgba(30, 144, 255,1.0)
            }
            else
            {
                texture2D.SetPixel(0, 0, new Color32(69, 69, 69, 255));    // rgba(30, 144, 255,1.0)

            }
            texture2D.Apply();

            GUIStyle guistyle = new GUIStyle()
            {
                normal = new GUIStyleState  // 正常样式
                {
                    textColor = Color.white,
                    background = texture2D
                },
                wordWrap = true,
                alignment = TextAnchor.MiddleCenter,
                fixedHeight = 40,
                fixedWidth = 80,
                margin = new RectOffset(5, 7, 0, 0),
            };

            return GUILayout.Button(text, guistyle);
        }
        public static bool Button(string text, Texture2D normal, Texture2D active)
        {
            GUIStyle guistyle = XmUIStyle.ButtonStyle;
            guistyle.normal.background = normal;
            guistyle.active.background = active;

            return GUILayout.Button(text, guistyle);
        }
        public static bool Button(Texture2D img)
        {
            GUIStyle guistyle = XmUIStyle.ButtonStyle;
            guistyle.normal.background = img;
            guistyle.fixedWidth = 50;
            guistyle.fixedHeight = 50;
            //float num = EditorGUILayout.FloatField(new GUIContent("float number", comment1), 5.0f);
            return GUILayout.Button("", guistyle);
        }
        public static bool Button(string text, Texture2D img)
        {
            GUIStyle guistyle = XmUIStyle.ButtonStyle;
            guistyle.normal.background = img;
            guistyle.fixedWidth = 50;
            guistyle.fixedHeight = 50;
            //float num = EditorGUILayout.FloatField(new GUIContent("float number", comment1), 5.0f);
            Label(text, 65, 40);
            return GUILayout.Button("", guistyle);
        }

        public static void Label(string text)
        {
            GUIStyle guistyle = XmUIStyle.LabelStyle;
            GUILayout.Label(text, guistyle);
        }

        public static void Label(string text, RectOffset margin)
        {
            GUIStyle guistyle = XmUIStyle.LabelStyle;
            guistyle.margin = margin;

            GUILayout.Label(text, guistyle);
        }

        public static void Label(string text, int width, int height)
        {
            GUIStyle guistyle = XmUIStyle.LabelStyle;
            guistyle.fixedWidth = width;
            guistyle.fixedHeight = height;
            //GUIStyle guistyle = new GUIStyle()
            //{
            //    fixedWidth = width,
            //    fixedHeight = height,
            //    alignment = TextAnchor.MiddleRight,
            //    normal = new GUIStyleState
            //    {
            //        textColor = Color.white
            //    }
            //};
            GUILayout.Label(text, guistyle);
        }

        public static string TextField(string text)
        {
            GUIStyle guistyle = new GUIStyle()
            {
                fixedWidth = 60,
                fixedHeight = 40,
                alignment = TextAnchor.MiddleLeft,
                margin = new RectOffset(5, 0, 0, 0),
                normal = new GUIStyleState
                {
                    textColor = Color.white
                }
            };
            return GUILayout.TextField(text, guistyle);
        }
        public static string TextField(string text, int width, int height)
        {
            GUIStyle guistyle = new GUIStyle()
            {
                fixedWidth = width,
                fixedHeight = height,
                alignment = TextAnchor.MiddleLeft,
                margin = new RectOffset(5, 0, 0, 0),
                normal = new GUIStyleState
                {
                    textColor = Color.white
                }
            };
            return GUILayout.TextField(text, guistyle);
        }

        /// <summary>
        /// 分页栏
        /// </summary>
        /// <param name="page">当前页数</param>
        /// <param name="count">总数量</param>
        /// <param name="limit">每页数量</param>
        /// <param name="PaginationDo">点击执行函数</param>
        public static void PaginationList(int page, int count, int limit, PaginationDo PaginationDo)
        {
            GUILayout.BeginHorizontal(new GUIStyle { alignment = TextAnchor.MiddleCenter });
            {
                int groups = 5; //一共出现几个按钮
                int PageFloat = (int)Math.Floor((double)(groups / 2));  // 页码浮动量
                int maxPage = (int)Math.Ceiling((double)(count / limit));   // 总页数
                int i = page - PageFloat;       // 序号
                if (page + PageFloat > maxPage) i = maxPage - (PageFloat * 2);
                if (i < 1) i = 1;

                Texture2D textureNormal = new Texture2D(1, 1, TextureFormat.RGBA32, false);
                textureNormal.SetPixel(0, 0, new Color32(59, 59, 59, 255));
                textureNormal.Apply();

                Texture2D textureActive = new Texture2D(1, 1, TextureFormat.RGBA32, false);
                textureActive.SetPixel(0, 0, new Color32(49, 49, 49, 255));  // rgba(112, 161, 255,1.0)
                textureActive.Apply();

                GUIStyle guistyle = new GUIStyle()
                {
                    normal = new GUIStyleState  // 正常样式
                    {
                        textColor = Color.white,
                        background = textureNormal
                    },
                    active = new GUIStyleState  // 点击样式
                    {
                        textColor = Color.white,
                        background = textureActive
                    },
                    wordWrap = true,
                    alignment = TextAnchor.MiddleCenter,
                    fixedHeight = 40,
                    fixedWidth = 40,
                    margin = new RectOffset(5, 7, 0, 0),
                };

                if (GUILayout.Button("首页", guistyle))
                {
                    PaginationDo(1);
                }
                if (GUILayout.Button("<<", guistyle))
                {
                    page--;
                    if (page <= 0) page = 1;
                    PaginationDo(page);
                }
                do
                {
                    Texture2D texture2D2 = new Texture2D(1, 1, TextureFormat.RGBA32, false);
                    if (page == i)
                    {
                        texture2D2.SetPixel(0, 0, new Color32(51, 122, 183, 255));
                    }
                    else
                    {
                        texture2D2.SetPixel(0, 0, new Color32(59, 59, 59, 255));
                    }
                    texture2D2.Apply();
                    guistyle.normal.background = texture2D2;

                    if (GUILayout.Button(i.ToString(), guistyle))
                    {
                        PaginationDo(i);
                    }
                    i++;

                } while ((i <= (page + PageFloat) || page - PageFloat <= 0 && i < (page + PageFloat + (PageFloat + 2 - page))) && i <= maxPage);

                guistyle.normal.background = textureNormal;

                if (GUILayout.Button(">>", guistyle))
                {
                    page++;
                    if (page >= maxPage) page = maxPage;
                    PaginationDo(page);
                }
                if (GUILayout.Button("尾页", guistyle))
                {
                    PaginationDo(maxPage);
                }
            }
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// 换行
        /// </summary>
        public static void hr()
        {
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(new GUIStyle { alignment = TextAnchor.UpperLeft });
        }
    }
}