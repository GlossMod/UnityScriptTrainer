using Config;
using GameData.Domains.Map;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityGameUI;
using Object = UnityEngine.Object;

namespace ScriptTrainer
{
    public class MapAreaWindow : MonoBehaviour
    {
        #region[全局参数]
        private static GameObject Panel;
        private static int initialX;
        private static int initialY;
        private static int elementX;
        private static int elementY;

        // private static int type = 0;
        private static int page = 1;
        private static int conunt = 36;
        // Config.MapArea.Instance.Count / conunt 向下取整
        public static List<MapAreaList> MapArea
        {
            get
            {
                try
                {
                    List<MapAreaList> list = new List<MapAreaList>();
                    //Debug.Log($"玩家：{Scripts.playerId}");
                    if (Scripts.playerId != 0)
                    {
                        WorldMapModel instance = SingletonObject.getInstance<WorldMapModel>();
                        for (int i = 0; i < instance.Areas.Length; i++)
                        {
                            var item = instance.Areas[i];
                            if (searchText != "")
                            {
                                if (item.GetConfig().Name.Contains(searchText))
                                {
                                    list.Add(new MapAreaList(item.GetConfig().Name, i));
                                }
                            }
                            else
                            {
                                list.Add(new MapAreaList(item.GetConfig().Name, i));
                            }
                        }
                    }
                    return list;
                }
                catch (Exception e)
                {
                    Debug.Log(e.ToString());
                    return new List<MapAreaList>();
                }


            }
        }

        private static int maxPage
        {
            get
            {
                return (int)Math.Ceiling((double)MapArea.Count / conunt);
            }
        }
        private static string searchText = "";
        private static GameObject uiText;
        private static string uiText_text
        {
            get
            {
                return $"{page} / {maxPage}";
            }
        }

        // private static GameObject ItemPanel;
        private static List<GameObject> ItemButtons = new List<GameObject>();

        #endregion

        public MapAreaWindow(GameObject panel, int x, int y)
        {
            Panel = panel;
            initialX = elementX = x + 20;
            initialY = elementY = y;
            Initialize();
        }

        private static void Initialize()
        {
            //int page = 1;
            //int conunt = 36;
            //int maxPage = Config.MapArea.Instance.Count / conunt;
            searchBar();
            container();

        }


        public static void container()
        {
            ResetCoordinates(true, true);
            elementY -= 30;
            elementX += 10;

            foreach (var item in ItemButtons)
            {
                Object.Destroy(item);
            }
            ItemButtons.Clear();

            if (Scripts.playerId == 0)
            {
                var btn = AddH3("请先进入游戏", Panel);
                ItemButtons.Add(btn);
                return;
            }


            int start = (page - 1) * conunt;
            int end = start + conunt;
            end = end >= MapArea.Count ? MapArea.Count : end;

            //AddH3("修改地区恩义", Panel);
            for (int i = start; i < end;)
            {
                var item = MapArea[i];

                GameObject btn = AddButton(item.name, Panel, () =>
                {
                    UIWindows.SpawnInputDialog($"{item.name}的恩义设为多少?", "修改", "100", (string count) =>
                    {
                        Scripts.ChangeSpiritualDebt(item.areaId, count.ConvertToIntDef(100) * 10);

                    });

                });
                i++;
                if (i % 6 == 0)
                {
                    hr(10);
                }
                ItemButtons.Add(btn);
            }


            var pageObg = pageBar();
            ItemButtons.Add(pageObg);
        }

        #region[筛选]

        // 搜索
        private static void searchBar()
        {
            elementY += 10;
            elementX += 50;

            // label
            Sprite txtBgSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#7AB900FF"));
            GameObject uiText = UIControls.createUIText(Panel, txtBgSprite, "#FFFFFFFF");
            uiText.GetComponent<Text>().text = "搜索";
            uiText.GetComponent<RectTransform>().localPosition = new Vector3(elementX, elementY, 0);
            //uiText.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 30);
            uiText.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

            // 坐标偏移
            elementX += 10;

            // 输入框
            int w = 260;
            Sprite inputFieldSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#212121FF"));
            GameObject uiInputField = UIControls.createUIInputField(Panel, inputFieldSprite, "#FFFFFFFF");
            uiInputField.GetComponent<InputField>().text = searchText;
            uiInputField.GetComponent<RectTransform>().localPosition = new Vector3(elementX + 100, elementY, 0);
            uiInputField.GetComponent<RectTransform>().sizeDelta = new Vector2(w, 30);

            // 文本框失去焦点时触发方法
            uiInputField.GetComponent<InputField>().onEndEdit.AddListener((string text) =>
            {
                //Debug.Log(text);
                page = 1;
                searchText = text;
                container();
                MapAreaWindow.uiText.GetComponent<Text>().text = uiText_text;
                //Destroy(ItemPanel);
            });
        }

        // 分页
        private static GameObject pageBar()
        {

            // 背景
            GameObject pageObj = UIControls.createUIPanel(Panel, "40", "350");
            pageObj.GetComponent<Image>().color = UIControls.HTMLString2Color("#424242FF");
            pageObj.GetComponent<RectTransform>().localPosition = new Vector3(0, elementY, 0);

            // 当前页数 / 总页数
            Sprite txtBgSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#7AB900FF"));

            uiText = UIControls.createUIText(pageObj, txtBgSprite, "#ffFFFFFF");
            uiText.GetComponent<Text>().text = uiText_text;
            uiText.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            // 设置字体
            uiText.GetComponent<Text>().fontSize = 20;
            uiText.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;


            // 上一页
            string backgroundColor = "#8C9EFFFF";
            GameObject prevBtn = UIControls.createUIButton(pageObj, backgroundColor, "上一页", () =>
            {

                page--;
                if (page <= 0) page = 1;
                container();
                uiText.GetComponent<Text>().text = uiText_text;
            }, new Vector3());
            prevBtn.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 20);
            prevBtn.GetComponent<RectTransform>().localPosition = new Vector3(-100, 0, 0);

            // 下一页            
            GameObject nextBtn = UIControls.createUIButton(pageObj, backgroundColor, "下一页", () =>
            {
                page++;
                if (page >= maxPage) page = (int)maxPage;
                container();
                uiText.GetComponent<Text>().text = uiText_text;
            });
            nextBtn.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 20);
            nextBtn.GetComponent<RectTransform>().localPosition = new Vector3(100, 0, 0);

            return pageObj;
        }

        #endregion

        #region[组件]
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

        // 添加小标题
        public static GameObject AddH3(string text, GameObject panel)
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
        public static void hr(int offsetX = 0, int offsetY = 0)
        {
            ResetCoordinates(true);
            elementX += offsetX;
            elementY -= 50 + offsetY;

        }

        // 重置坐标
        public static void ResetCoordinates(bool x, bool y = false)
        {
            if (x) elementX = initialX;
            if (y) elementY = initialY;
        }

        #endregion



    }

    public class MapAreaList
    {
        public string name;
        public int areaId;

        public MapAreaList(string name, int areaId)
        {
            this.name = name;
            this.areaId = areaId;
        }
    }
}
