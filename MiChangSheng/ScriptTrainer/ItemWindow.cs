
using GUIPackage;
using JSONClass;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityGameUI;
using Object = UnityEngine.Object;

namespace ScriptTrainer
{
    internal class ItemWindow: MonoBehaviour
    {
        private static GameObject Panel;
        private static int initialX;
        private static int initialY;
        private static int elementX;
        private static int elementY;

        #region[数据分页相关]
        private static GameObject ItemPanel;
        private static List<GameObject> ItemButtons = new List<GameObject>();
        private static List<item> DataList
        {
            get {
                List<item> itemList = new List<item>();
                foreach (KeyValuePair<string, JSONObject> item in jsonData.instance.ItemJsonData)
                {
                    itemList.Add(new item(item.Value["id"].I));
                }

                return itemList;
            }
        }
        private static List<item> itemList = new List<item>();
        private static int page = 1;
        private static int maxPage = 1;
        private static int conunt = 15;
        private static ItemType type = (ItemType)999;
        private static string searchText = "";
        private static GameObject uiText;
        private static string uiText_text
        {
            get
            {
                return $"{page} / {maxPage}";
            }
        }
        #endregion

        public ItemWindow(GameObject panel, int x , int y)
        {
            Panel = panel;
            initialX = elementX = x + 50;
            initialY = elementY = y;
            Initialize();
        }

        public void Initialize()
        {
            //Debug.Log(DataList.Count.ToString());
            // 创建搜索框
            searchBar(Panel);
            typeBar(Panel);
            elementY += 10;
            hr();

            // 创建物品列表
            container();


            // 创建分页
            pageBar(Panel);
        }

        #region[创建详细]

        // 搜索框
        private void searchBar(GameObject panel)
        {
            elementY += 10;
            
            // label
            Sprite txtBgSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#7AB900FF"));
            GameObject uiText = UIControls.createUIText(panel, txtBgSprite, "#FFFFFFFF");
            uiText.GetComponent<Text>().text = "搜索";
            uiText.GetComponent<RectTransform>().localPosition = new Vector3(elementX, elementY, 0);
            //uiText.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 30);
            uiText.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

            // 坐标偏移
            elementX += 10;

            // 输入框
            int w = 260;
            Sprite inputFieldSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#212121FF"));
            GameObject uiInputField = UIControls.createUIInputField(panel, inputFieldSprite, "#FFFFFFFF");
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
                ItemWindow.uiText.GetComponent<Text>().text = uiText_text;
                //Destroy(ItemPanel);
            });
        }

        // 分类下拉框
        private void typeBar(GameObject panel)
        {
            elementX += 350;

            // label
            Sprite txtBgSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#7AB900FF"));
            GameObject uiText = UIControls.createUIText(panel, txtBgSprite, "#FFFFFFFF");
            uiText.GetComponent<Text>().text = "分类";
            uiText.GetComponent<RectTransform>().localPosition = new Vector3(elementX, elementY, 0);
            //uiText.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 30);
            uiText.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

            // 坐标偏移
            elementX += 60;

           
            
            List<string> options = new List<string> { "全部" };
            // 遍历 ItemTypes
            foreach (var item in Enum.GetValues(typeof(ItemTypes)))
            {
                options.Add(item.ToString());
            }

            // 创建下拉框
            Sprite dropdownBgSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#212121FF"));      // 背景颜色
            Sprite dropdownScrollbarSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#8C9EFFFF"));   // 滚动条颜色 (如果有的话
            Sprite dropdownDropDownSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#212121FF"));    // 框右侧小点的颜色
            Sprite dropdownCheckmarkSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#8C9EFFFF"));   // 选中时的颜色
            Sprite dropdownMaskSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#E65100FF"));        // 不知道是哪的颜色
            Color LabelColor = UIControls.HTMLString2Color("#EFEBE9FF");
            GameObject uiDropDown = UIControls.createUIDropDown(panel, dropdownBgSprite, dropdownScrollbarSprite, dropdownDropDownSprite, dropdownCheckmarkSprite, dropdownMaskSprite, options, LabelColor);
            Object.DontDestroyOnLoad(uiDropDown);
            uiDropDown.GetComponent<RectTransform>().localPosition = new Vector3(elementX, elementY, 0);

            // 下拉框选中时触发方法
            uiDropDown.GetComponent<Dropdown>().onValueChanged.AddListener((int call) =>
            {                
                type = call == 0 ? (ItemType)999 : (ItemType)call - 1;
                page = 1;
                container();
                ItemWindow.uiText.GetComponent<Text>().text = uiText_text;
            });

        }
        
        // 分页
        private void pageBar(GameObject panel)
        {
            // 背景
            GameObject pageObj = UIControls.createUIPanel(panel, "40", "500");
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
                if (page >= maxPage) page = maxPage;
                container();
                uiText.GetComponent<Text>().text = uiText_text;
            });
            nextBtn.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 20);
            nextBtn.GetComponent<RectTransform>().localPosition = new Vector3(100, 0, 0);
        }

        private static void container()
        {
            //Debug.Log($"x:{elementX}, y:{elementY}");
            elementX = -200;
            elementY = 125;

            // 先清空旧的 ItemPanel
            foreach (var item in ItemButtons)
            {
                UnityEngine.Object.Destroy(item);
            }
            ItemButtons.Clear();


            ItemPanel = UIControls.createUIPanel(Panel, "300", "600");
            ItemPanel.GetComponent<Image>().color = UIControls.HTMLString2Color("#424242FF");
            ItemPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(10, 0);

            int num = 0;
            foreach (var item in GetItemData())
            {
                var btn = createItemButton(item.itemNameCN, ItemPanel, item.itemIcon, item.quality,  () =>
                {
                    UIWindows.SpawnInputDialog($"您想获取多少个{item.itemNameCN}？", "添加", "1", (string count) =>
                    {
                        Debug.Log($"已添加{count}个{item.itemNameCN}到背包");
                        
                        KBEngine.Avatar player = Tools.instance.getPlayer();    // 获取玩家 
                        player.addItem(item.itemID, Tools.CreateItemSeid(item.itemID), count.ConvertToIntDef(1));
                        //Singleton.inventory.AddItem(item.itemID);
                    });
                });
                ItemButtons.Add(btn);

                num++;
                if (num % 3 == 0)
                {
                    hr();
                }
            }
        }
        
        private static GameObject createItemButton(string text, GameObject panel, Texture2D itemIcon, int quality, UnityAction action)
        {
            // 按钮宽 200 高 50
            int buttonWidth = 190;
            int buttonHeight = 50;

            // 根据品质设置背景颜色
            string qualityColor = "#FFFFFFFF";
            switch (quality)
            {
                case 1: qualityColor = "#81C784FF"; break;
                case 2: qualityColor = "#29B6F6FF"; break;
                case 3: qualityColor = "#B388FFFF"; break;
                case 4: qualityColor = "#FFA726FF"; break;
                case 5: qualityColor = "#FF8A80FF"; break;
            }


            // 创建一个背景
            GameObject background = UIControls.createUIPanel(panel, buttonHeight.ToString(), buttonWidth.ToString(), null);            
            background.GetComponent<Image>().color = UIControls.HTMLString2Color("#455A64FF");
            background.GetComponent<RectTransform>().localPosition = new Vector3(elementX, elementY, 0);

            // 创建图标  60x60
            Sprite BgSprite = UIControls.createSpriteFrmTexture(itemIcon);
            //Sprite BgSprite = ResManager.inst.LoadSprite("Item Icon/" + itemIcon);
            //if (BgSprite == null)
            //{
            //    BgSprite = ResManager.inst.LoadSprite("Item Icon/1");
            //}
            GameObject icon = UIControls.createUIImage(background, BgSprite);
            icon.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 40);
            icon.GetComponent<RectTransform>().anchoredPosition = new Vector2(30, 0);

            // 创建文字
            //Sprite txtBgSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#455A64FF"));
            //GameObject uiText = UIControls.createUIText(background, txtBgSprite, text);
            //uiText.GetComponent<Text>().fontSize = 20;
            Sprite txtBgSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#7AB900FF"));
            GameObject uiText = UIControls.createUIText(background, txtBgSprite, qualityColor);
            uiText.GetComponent<Text>().text = text;
            uiText.GetComponent<RectTransform>().localPosition = new Vector3(0, 5, 0);

            // 创建按钮
            string backgroundColor_btn = "#8C9EFFFF";
            GameObject button = UIControls.createUIButton(background, backgroundColor_btn, "获取", action, new Vector3());
            button.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 20);
            button.GetComponent<RectTransform>().localPosition = new Vector3(-50, -10, 0);


            elementX +=200;

            //return button;
            return background;
        }

        private static void hr()
        {
            elementX = initialX;
            elementY -= 60;
        }

        #endregion

        #region[获取数据相关函数]
        private static List<item> GetItemData()
        {
            var ItemData = DataList;
            if (searchText != "")
            {
                ItemData = FilterItemData(ItemData);
            }
            if (type != (ItemType)999){
                ItemData = FilterItemDataByType(ItemData);
            }

            // 对 DataList 进行分页
            List<item> list = new List<item>();
            int start = page * conunt;
            int end = start + conunt;
            for (int i = start; i < end; i++)
            {
                if (i < ItemData.Count)
                {
                    list.Add(ItemData[i]);
                }
                else
                {
                    break;
                }
            }
            maxPage = ItemData.Count / conunt;

            return list;
        }

        // 搜索过滤
        private static List<item> FilterItemData(List<item> dataList)
        {
            if (searchText == "")
            {
                return dataList;
            }

            List<item> list = new List<item>();

            foreach (var item in dataList)
            {
                if (item.itemNameCN.Contains(searchText))
                {
                    list.Add(item);
                }
            }

            return list;
        }

        // 类型过滤
        private static List<item> FilterItemDataByType(List<item> dataList)
        {
            if (type == (ItemType)999)
            {
                return dataList;
            }

            List<item> list = new List<item>();
            
            foreach (var item in dataList)
            {
                if ((int)item.itemType == (int)type)
                {
                    list.Add(item);
                }
            }

            return list;
        }

        #endregion
    }
}
