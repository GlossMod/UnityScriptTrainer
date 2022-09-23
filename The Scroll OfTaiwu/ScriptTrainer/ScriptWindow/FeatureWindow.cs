using Config;
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
    public class FeatureWindow
    {
        #region[获取数据]

        public static List<CharacterFeatureItem> CharacterFeature
        {
            get
            {
                List<CharacterFeatureItem> list = new List<CharacterFeatureItem>();
                foreach (var item in Config.CharacterFeature.Instance)
                {
                    if (searchText != "")
                    {
                        if (item.Name.Contains(searchText))
                        {
                            list.Add(item);
                        }
                    }
                    else
                    {
                        list.Add(item);
                    }

                };
                return list;
            }
        }
        
        public static int PlayerCharId
        {
            get
            {
                return SingletonObject.getInstance<BasicGameData>().TaiwuCharId;
            }
        }

        #endregion

        #region[全局参数]
        private static GameObject Panel;
        private static int initialX;
        private static int initialY;
        private static int elementX;
        private static int elementY;

        private static int type = 0;
        private static int page = 1;
        private static int conunt = 15;
        // Config.MapArea.Instance.Count / conunt 向下取整

        private static int maxPage
        {
            get
            {
                return (int)Math.Ceiling((double)CharacterFeature.Count / conunt);
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

        private static GameObject ItemPanel;
        private static List<GameObject> ItemButtons = new List<GameObject>();

        #endregion

        public FeatureWindow(GameObject panel, int x, int y)
        {
            Panel = panel;
            initialX = elementX = x + 60;
            initialY = elementY = y;
            Initialize();
        }
        
        public static void Initialize()
        {
            searchBar();
            container();
            pageBar();
        }

        // 主体
        public static void container()
        {
            ResetCoordinates(true, true);

            elementY -= 40;

            foreach (var item in ItemButtons)
            {
                Object.Destroy(item);
            }
            ItemButtons.Clear();

            int start = (page - 1) * conunt;
            int end = start + conunt;

            for (int i = start; i < end;)
            {
                var item = CharacterFeature[i];
                GameObject btn = AddButton($"{item.Name} {item.Level}", item.Level, Panel, () =>
                {
                    // 添加
                    var Character =  Config.Character.Instance[PlayerCharId];
                    Character.FeatureIds.Add(item.TemplateId);

                    Debug.Log($"Character {Character.GivenName} - {Character.Surname}");

                    //  GameData.Domains.Character.Character
                    // private bool OfflineAddFeature(short featureId, bool removeMutexFeature)


                    //Character = Config.CharacterFeature.Instance[PlayerCharId];
                }, () =>
                {
                    // 移除
                });
                i++;
                if (i % 3 == 0)
                {
                    hr(0,10);
                }
                ItemButtons.Add(btn);
            }
        }

        // 搜索框
        private static void searchBar()
        {
            elementY += 10;

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
                FeatureWindow.uiText.GetComponent<Text>().text = uiText_text;
                //Destroy(ItemPanel);
            });
        }

        // 分页
        public static void pageBar()
        {
            // 背景
            GameObject pageObj = UIControls.createUIPanel(Panel, "40", "500");
            pageObj.GetComponent<Image>().color = UIControls.HTMLString2Color("#424242FF");
            pageObj.GetComponent<RectTransform>().localPosition = new Vector3(0, elementY, 0);

            // 当前页数 / 总页数
            Sprite txtBgSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#7AB900FF"));

            if (uiText == null)
            {
                uiText = UIControls.createUIText(pageObj, txtBgSprite, "#ffFFFFFF");
                uiText.GetComponent<Text>().text = uiText_text;
                uiText.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
                // 设置字体
                uiText.GetComponent<Text>().fontSize = 20;
                uiText.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            }


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
        }

        #region[元素方法]
        public static GameObject AddButton(string Text, int Level , GameObject panel, UnityAction action, UnityAction action2)
        {
            // 按钮宽 190 高 50
            int buttonWidth = 190;
            int buttonHeight = 50;

            // 创建一个背景
            GameObject background = UIControls.createUIPanel(panel, buttonHeight.ToString(), buttonWidth.ToString(), null);
            background.GetComponent<Image>().color = UIControls.HTMLString2Color("#101010ff");
            background.GetComponent<RectTransform>().localPosition = new Vector3(elementX, elementY, 0);

            // 名称
            Sprite txtBgSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#FFD54Fff"));
            GameObject uiText = UIControls.createUIText(background, txtBgSprite);
            uiText.GetComponent<Text>().text = Text;
            uiText.GetComponent<Text>().color = UIControls. HTMLString2Color("FFD54Fff");
            uiText.GetComponent<RectTransform>().localPosition = new Vector3(0, 5, 0);
            // 居中对齐
            uiText.GetComponent<Text>().alignment = TextAnchor.UpperCenter;

            // 添加按钮
            GameObject btn = UIControls.createUIButton(background, "#FFD54Fff", "获取", action);
            btn.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 20);
            btn.GetComponent<RectTransform>().localPosition = new Vector3(40, -10, 0);

            // 移除按钮
            GameObject btn2 = UIControls.createUIButton(background, "#78909Cff", "移除", action2);
            btn2.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 20);
            btn2.GetComponent<RectTransform>().localPosition = new Vector3(-40, -10, 0);


            elementX += 200;


            //string backgroundColor = "#8C9EFFFF";
            //Vector3 localPosition = new Vector3(elementX, elementY, 0);
            //elementX += 90;

            //GameObject button = UIControls.createUIButton(panel, backgroundColor, Text, action, localPosition);

            //// 按钮样式
            //button.AddComponent<Shadow>().effectColor = UIControls.HTMLString2Color("#000000FF");   // 添加阴影
            //button.GetComponent<Shadow>().effectDistance = new Vector2(2, -2);              // 设置阴影偏移
            //button.GetComponentInChildren<Text>().fontSize = 14;     // 设置字体大小           
            //button.GetComponent<RectTransform>().sizeDelta = new Vector2(80, 30);    // 设置按钮大小


            return background;
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
}
