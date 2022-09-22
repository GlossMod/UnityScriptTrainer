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
    // 物品列表
    public class ItemWindow: MonoBehaviour
    {
        #region[物品列表]
        // 武器
        public static List<ItemData> Weapon
        {
            get
            {
                List<ItemData> list = new List<ItemData>();
                foreach (var item in Config.Weapon.Instance) list.Add((ItemData)item);
                return list;
            }
        }

        // 护甲
        public static List<ItemData> Armor
        {
            get
            {
                List<ItemData> list = new List<ItemData>();
                foreach (var item in Config.Armor.Instance) list.Add((ItemData)item);
                return list;
            }
        }

        // 饰品
        public static List<ItemData> Accessory
        {
            get
            {
                List<ItemData> list = new List<ItemData>();
                foreach (var item in Config.Accessory.Instance) list.Add((ItemData)item);
                return list;
            }
        }

        // 衣服
        public static List<ItemData> Clothing
        {
            get
            {
                List<ItemData> list = new List<ItemData>();
                foreach (var item in Config.Clothing.Instance) list.Add((ItemData)item);
                return list;
            }
        }

        // 坐骑
        public static List<ItemData> Carrier
        {
            get
            {
                List<ItemData> list = new List<ItemData>();
                foreach (var item in Config.Carrier.Instance) list.Add((ItemData)item);
                return list;
            }
        }

        // 材料
        public static List<ItemData> Material
        {
            get
            {
                List<ItemData> list = new List<ItemData>();
                foreach (var item in Config.Material.Instance) list.Add((ItemData)item);
                return list;
            }
        }

        // 工艺道具
        public static List<ItemData> CraftTool
        {
            get
            {
                List<ItemData> list = new List<ItemData>();
                foreach (var item in Config.CraftTool.Instance) list.Add((ItemData)item);
                return list;
            }
        }

        // 食物
        public static List<ItemData> Food
        {
            get
            {
                List<ItemData> list = new List<ItemData>();
                foreach (var item in Config.Food.Instance) list.Add((ItemData)item);
                return list;
            }
        }

        // 药物
        public static List<ItemData> Medicine
        {
            get
            {
                List<ItemData> list = new List<ItemData>();
                foreach (var item in Config.Medicine.Instance) list.Add((ItemData)item);
                return list;
            }
        }

        // 茶酒
        public static List<ItemData> TeaWine
        {
            get
            {
                List<ItemData> list = new List<ItemData>();
                foreach (var item in Config.TeaWine.Instance) list.Add((ItemData)item);
                return list;
            }
        }

        // 技能书
        public static List<ItemData> SkillBook
        {
            get
            {
                List<ItemData> list = new List<ItemData>();
                foreach (var item in Config.SkillBook.Instance) list.Add((ItemData)item);
                return list;
            }
        }

        // 蟋蟀
        public static List<ItemData> Cricket
        {
            get
            {
                List<ItemData> list = new List<ItemData>();
                foreach (var item in Config.Cricket.Instance) list.Add((ItemData)item);
                return list;
            }
        }

        // 其他
        public static List<ItemData> Misc
        {
            get
            {
                List<ItemData> list = new List<ItemData>();
                foreach (var item in Config.Misc.Instance) list.Add((ItemData)item);
                return list;
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
        private static int maxPage = 1;
        private static int conunt = 15;        
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

        public ItemWindow(GameObject panel, int x, int y)
        {
            Panel = panel;
            initialX = elementX = x + 50;
            initialY = elementY = y;
            Initialize();
        }
        
        private void Initialize()
        {
            searchBar(Panel);
            typeBar(Panel);
            elementY += 10;
            hr();
            container();
            pageBar(Panel);
        }

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


            // 类型
            List<string> options = new List<string> {
                "武器", "护甲", "饰品", "衣服", "坐骑", "材料", "工艺道具", "食物", "药物", "茶酒", "技能书", "蟋蟀", "其他"
            };
           

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
                type = call;
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

        // 主体内容
        private static void container()
        {
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
                // 品质

                string[] Grade = new string[]
                {
                    "下", "中", "上", "奇", "密", "极", "超", "绝", "神"
                };


                var btn = createItemButton($"{item.Name} - {Grade[item.Gradel]}", ItemPanel, item.Icon, item.Gradel, () =>
                {
                    UIWindows.SpawnInputDialog($"您想获取多少个{item.Name}？", "添加", "1", (string count) =>
                    {
                        Scripts.GetItem(item.ItemType, item.TemplateId, count.ConvertToIntDef(1));

                        Debug.Log($"已添加{count}个{item.Name}到背包");

                        //KBEngine.Avatar player = Tools.instance.getPlayer();    // 获取玩家 
                        //player.addItem(item.itemID, Tools.CreateItemSeid(item.itemID), count.ConvertToIntDef(1));
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
        // 创建按钮
        private static GameObject createItemButton(string text, GameObject panel, string itemIcon, int quality, UnityAction action)
        {
            // 按钮宽 200 高 50
            int buttonWidth = 190;
            int buttonHeight = 50;

            // 创建一个背景
            GameObject background = UIControls.createUIPanel(panel, buttonHeight.ToString(), buttonWidth.ToString(), null);
            background.GetComponent<Image>().color = UIControls.HTMLString2Color("#101010ff");
            background.GetComponent<RectTransform>().localPosition = new Vector3(elementX, elementY, 0);

            // 创建图标  40 x 40
           
            GameObject icon = UIControls.createUIImage(background, GetLoadedSprite(itemIcon));
            icon.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 40);
            icon.GetComponent<RectTransform>().anchoredPosition = new Vector2(30, 0);

            // 创建文字
            Sprite txtBgSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#7AB900FF"));
            GameObject uiText = UIControls.createUIText(background, txtBgSprite);
            uiText.GetComponent<Text>().text = text;
            uiText.GetComponent<Text>().color = Colors.Instance.GradeColors[quality];   // 根据品质修改字体颜色
            uiText.GetComponent<RectTransform>().localPosition = new Vector3(0, 5, 0);

            // 创建按钮
            string backgroundColor_btn = "#8C9EFFFF";
            GameObject button = UIControls.createUIButton(background, backgroundColor_btn, "获取", action, new Vector3());
            button.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 20);
            button.GetComponent<RectTransform>().localPosition = new Vector3(-50, -10, 0);


            elementX += 200;

            //return button;
            return background;
        }

        private static void hr()
        {
            elementX = initialX;
            elementY -= 60;
        }
        
        #region[获取数据]

        private static List<ItemData> GetItemData()
        {
            List<ItemData> m_item = new List<ItemData>() ;

            int start = (page - 1) * conunt;
            int end = start + conunt;
            switch (type)
            {
                case 0:
                    m_item = SetItemData(Weapon);
                    break;
                case 1:
                    m_item = SetItemData(Armor);
                    break;
                case 2:
                    m_item = SetItemData(Accessory);
                    break;
                case 3:
                    m_item = SetItemData(Clothing);
                    break;
                case 4:
                    m_item = SetItemData(Carrier);
                    break;
                case 5:
                    m_item = SetItemData(Material);
                    break;
                case 6:
                    m_item = SetItemData(CraftTool);
                    break;
                case 7:
                    m_item = SetItemData(Food);
                    break;
                case 8:
                    m_item = SetItemData(Medicine);
                    break;
                case 9:
                    m_item = SetItemData(TeaWine);
                    break;
                case 10:
                    m_item = SetItemData(SkillBook);
                    break;
                case 11:
                    m_item = SetItemData(Cricket);
                    break;
                case 12:
                    m_item = SetItemData(Misc);
                    break;
            }

            return m_item;
        }
        
       
        private static List<ItemData> SetItemData<T>(List<T> m_data_list)
        {
            List<ItemData> item = new List<ItemData>();
            List<ItemData> r_item = new List<ItemData>();
            foreach (var itemData in m_data_list)
            {

                if (searchText != "" && (itemData as ItemData).Name.Contains(searchText))
                {
                    item.Add(itemData as ItemData);
                }

                if (searchText == "")
                {
                    item.Add(itemData as ItemData);
                }
            }
            // 分页
            int start = (page - 1) * conunt;
            int end = start + conunt;
            if (end > item.Count)
            {
                end = item.Count;
            }
            
            for (int i = start; i < end; i++)
            {
                r_item.Add(item[i]);
            }

            // (int)Math.Ceiling((double)Config.MapArea.Instance.Count / conunt);
            maxPage = (int)Math.Ceiling((double)item.Count / conunt);

            return r_item;
        }

        #endregion

        #region[获取图标]

        public static void LoadAllPacker()
        {
            var _runTimeNamesCache = Traverse.Create(AtlasInfo.Instance).Field<Dictionary<string, List<string>>>("_runTimeNamesCache").Value;
            Dictionary<string, List<string>>.Enumerator enumerator = _runTimeNamesCache.GetEnumerator();
            while (enumerator.MoveNext())
            {
                KeyValuePair<string, List<string>> pair = enumerator.Current;
                AtlasInfo.Instance.LoadPacker(pair.Key);
            }
        }

        private static Sprite GetLoadedSprite(string spriteName)
        {
            Dictionary<string, Sprite> map;
            Sprite sprite = default(Sprite);

            // LoadConstantPackers
            var _loadedPackerCache = Traverse.Create(AtlasInfo.Instance).Field<Dictionary<string, Dictionary<string, Sprite>>>("_loadedPackerCache").Value;

            var _runTimeNamesCache = Traverse.Create(AtlasInfo.Instance).Field<Dictionary<string, List<string>>>("_runTimeNamesCache").Value;
            Dictionary<string, List<string>>.Enumerator enumerator = _runTimeNamesCache.GetEnumerator();

            while (enumerator.MoveNext())
            {
                KeyValuePair<string, List<string>> pair = enumerator.Current;
                if (pair.Value.Contains(spriteName))
                {
                    

                    if (_loadedPackerCache.TryGetValue(pair.Key, out map) && map.TryGetValue(spriteName, out sprite))
                    {
                        return sprite;
                    }
                }
            }
            if (sprite == default(Sprite))
            {
                Debug.Log($"未找到图标 {spriteName}");
            }
            
            return sprite;
        }

        #endregion
    }


    public class ItemData
    {
        public short TemplateId;    // ID
        public string Name;         // 名称
        public string Icon;         // 图标
        public string Desc;         // 描述
        public sbyte Gradel;        // 品质
        public short ItemSubType;   // 子类型
        public sbyte ItemType;

        public ItemData(short TemplateId, string Name, string Icon, string Desc, sbyte Grade, short ItemSubType, sbyte ItemType)
        {
            this.TemplateId = TemplateId;
            this.Name = Name;
            this.Icon = Icon;
            this.Desc = Desc;
            this.Gradel = Grade;
            this.ItemSubType = ItemSubType;
            this.ItemType = ItemType;
        }

        #region[类型强制转换]

        public static explicit operator ItemData(MiscItem item)
        {            
            return new ItemData(item.TemplateId, item.Name, item.Icon, item.Desc, item.Grade, item.ItemSubType, item.ItemType);
        }
        
        public static explicit operator ItemData(CricketItem item)
        {
            return new ItemData(item.TemplateId, item.Name, item.Icon, item.Desc, item.Grade, item.ItemSubType, item.ItemType);
        }

        public static explicit operator ItemData(SkillBookItem item)
        {
            return new ItemData(item.TemplateId, item.Name, item.Icon, item.Desc, item.Grade, item.ItemSubType, item.ItemType);
        }
         public static explicit operator ItemData(WeaponItem item)
        {
            return new ItemData(item.TemplateId, item.Name, item.Icon, item.Desc, item.Grade, item.ItemSubType, item.ItemType);
        }
         public static explicit operator ItemData(ArmorItem item)
        {
            return new ItemData(item.TemplateId, item.Name, item.Icon, item.Desc, item.Grade, item.ItemSubType, item.ItemType);
        }
         public static explicit operator ItemData(AccessoryItem item)
        {
            return new ItemData(item.TemplateId, item.Name, item.Icon, item.Desc, item.Grade, item.ItemSubType, item.ItemType);
        }
         public static explicit operator ItemData(ClothingItem item)
        {
            return new ItemData(item.TemplateId, item.Name, item.Icon, item.Desc, item.Grade, item.ItemSubType, item.ItemType);
        }
         public static explicit operator ItemData(CarrierItem item)
        {
            return new ItemData(item.TemplateId, item.Name, item.Icon, item.Desc, item.Grade, item.ItemSubType, item.ItemType);
        }
         public static explicit operator ItemData(MaterialItem item)
        {
            return new ItemData(item.TemplateId, item.Name, item.Icon, item.Desc, item.Grade, item.ItemSubType, item.ItemType);
        }
         public static explicit operator ItemData(CraftToolItem item)
        {
            return new ItemData(item.TemplateId, item.Name, item.Icon, item.Desc, item.Grade, item.ItemSubType, item.ItemType);
        }
         public static explicit operator ItemData(FoodItem item)
        {
            return new ItemData(item.TemplateId, item.Name, item.Icon, item.Desc, item.Grade, item.ItemSubType, item.ItemType);
        }
         public static explicit operator ItemData(MedicineItem item)
        {
            return new ItemData(item.TemplateId, item.Name, item.Icon, item.Desc, item.Grade, item.ItemSubType, item.ItemType);
        }
         public static explicit operator ItemData(TeaWineItem item)
        {
            return new ItemData(item.TemplateId, item.Name, item.Icon, item.Desc, item.Grade, item.ItemSubType, item.ItemType);
        }

        #endregion
    }
}
