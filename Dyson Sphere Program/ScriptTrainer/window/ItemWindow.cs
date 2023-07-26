using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityGameUI;
using static System.Collections.Specialized.BitVector32;
using Object = UnityEngine.Object;
// ReSharper disable All


namespace ScriptTrainer
{
    public class ItemWindow
    {
        #region[全局参数]
        public  static ItemProto[] dataArray = LDB.items.dataArray; // 物品列表
        private static int ItemConst = 100;
        private static List<GameObject> ItemButtons = new List<GameObject>();


        // 组件位置
        private static int initialX { get => -MainWindow.width / 2 + 100 + 60; }
        private static int initialY { get => MainWindow.height / 2 - 85 - 10; }

        private static int elementX = initialX;
        private static int elementY = initialY;

        #endregion

        #region[分页相关]
        private static int page = 1;        // 当前页数
        private static int conunt = 18;     // 每页多少个
        private static int maxPage = 1;     // 最大页数
        private static string search = "";  // 搜索

        private static string uiText_text   // 显示文本
        {
            get
            {
                return $"{page} / {maxPage}";
            }
        }

        private static Transform times_value;
        private static GameObject itemWindow;
        #endregion

        public ItemWindow(GameObject canvas)
        {

            itemWindow = UIControls.createUIPanel(canvas, "330", "630");
            itemWindow.GetComponent<Image>().color = UIControls.HTMLString2Color("#00000000");
            itemWindow.name = "itemWindow";
            
            AddItem(itemWindow);
            pageBar(canvas);
            SearchItem(canvas);
        }

        // 物品列表
        public static void AddItem(GameObject canvas)
        {
            // 先清空旧的 ItemPanel
            foreach (var item in ItemButtons)
            {
                UnityEngine.Object.Destroy(item);
            }
            ItemButtons.Clear();

            ResetCoordinates(true, true);

            int i = 0;

            foreach (var item in GetItemList(search))
            {
                i++;

                GameObject btn = createItemButton(canvas, item, () =>
                {
                    GameMain.mainPlayer.package.AddItemStacked(item.ID, ItemConst, 0, out int remainInc);
                    
                    UIItemup.Up(item.ID, ItemConst);

                });

                ItemButtons.Add(btn);

                // 一行10个
                if (i % 3 == 0) hr();

            }
        }

        // 分页按钮
        public static void pageBar(GameObject canvas)
        {
            // 加减按钮
            Transform count = Components.createCount(canvas);
            RectTransform rt = count.GetComponent<RectTransform>();
            rt.localPosition = new Vector3(0, -190, 0);

            // times-value 显示值
            times_value = count.Find("times-value");
            times_value.GetComponent<Text>().text = uiText_text;

            // - 按钮
            Transform minus = count.Find("-");
            minus.GetComponent<UIButton>().onClick += (int a) =>
            {
                page--;
                if (page <= 1) page = 1;

                AddItem(itemWindow);

                times_value.GetComponent<Text>().text = uiText_text;
            };

            // + 按钮
            Transform plus = count.Find("+");
            plus.GetComponent<UIButton>().onClick += (int a) =>
            {
                page++;

                if (page >= maxPage) page = maxPage;

                AddItem(itemWindow);

                times_value.GetComponent<Text>().text = uiText_text;
            };

        }

        // 搜索框
        public static void SearchItem(GameObject canvas)
        {
            // label
            Sprite txtBgSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#7AB900FF"));
            GameObject uiText = UIControls.createUIText(canvas, txtBgSprite, "#FFFFFFFF");
            uiText.GetComponent<Text>().text = "搜索";
            uiText.GetComponent<RectTransform>().localPosition = new Vector3(-230, -190, 0);
            uiText.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

            // 输入框
            Sprite inputFieldSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#84FFFF06"));
            GameObject uiInputField = UIControls.createUIInputField(canvas, inputFieldSprite, "#FFFFFFFF");
            uiInputField.GetComponent<InputField>().text = search;
            uiInputField.GetComponent<RectTransform>().localPosition = new Vector3(-230 + 30, -190, 0);
            uiInputField.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 30);
            // 设置输入框边框颜色
            
            
            

            // 文本框失去焦点时触发方法
            uiInputField.GetComponent<InputField>().onEndEdit.AddListener((string text) =>
            {
                Console.WriteLine(text);
                page = 1;
                search = text;
                AddItem(itemWindow);
                times_value.GetComponent<Text>().text = uiText_text;
                //Destroy(ItemPanel);
            });
        }


        // 按钮
        private static GameObject createItemButton(GameObject canvas, ItemProto item, UnityAction action)
        {
            // 创建一个背景
            GameObject background = UIControls.createUIButton(canvas, "#00000633", "", action);
            background.GetComponent<RectTransform>().localPosition = new Vector3(elementX, elementY, 0);
            background.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 45);
            background.name = "background";

            // 名称
            Sprite txtBgSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#7AB900FF"));
            GameObject uiText = UIControls.createUIText(background, txtBgSprite, "#FFFFFFFF");
            uiText.GetComponent<Text>().text = item.name;
            uiText.GetComponent<RectTransform>().localPosition = new Vector3(0, -10, 0);

            // 图标
            Sprite BgSprite = item.iconSprite;
            GameObject button = UIControls.createUIButton(background, "#00000033", "", action);
            button.GetComponent<Image>().sprite = BgSprite;
            RectTransform rt = button.GetComponent<RectTransform>();
            rt.localPosition = new Vector3(60, 0, 0);
            rt.sizeDelta = new Vector2(40, 40);

            elementX += 210;

            return background;
        }

        // 获取物品列表
        public static List<ItemProto> GetItemList(string name = "")
        {
            List<ItemProto> list = new List<ItemProto>();

            foreach (var item in dataArray)
            {
                if (name == "")
                {
                    list.Add(item);
                }
                else
                {
                    // 判断 item.name 是否包含 name
                    if (item.name.Contains(name))
                    {
                        list.Add(item);
                    }

                }
            }

            // 根据 list 长度 和 conunt 计算页数 并向下取整
            maxPage = (int)Math.Ceiling((double)list.Count / conunt);

            // 根据 page 和 conunt 进行分页返回
            int start = (page - 1) * conunt;
            int end = start + conunt;
            if (end > list.Count) end = list.Count;
            
            return list.GetRange(start, end - start);

        }


        // 重置坐标
        public static void ResetCoordinates(bool x, bool y = false)
        {
            if (x) elementX = initialX;
            if (y) elementY = initialY;
        }

        // 换行
        public static void hr(int offsetX = 0, int offsetY = 0)
        {
            ResetCoordinates(true);
            elementX += offsetX;
            elementY -= 50 + offsetY;

        }

    }
}
