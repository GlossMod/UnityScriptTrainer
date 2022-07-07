using System.Collections.Generic;
using GUIPackage;
using UnityEngine;

namespace ScriptTrainer
{
    internal class ItemData : MonoBehaviour
    {
        public static List<item> itemList = new List<item>();  // 物品列表

        public static void LoadIfNeeded()
        {
            if (itemList?.Count == 0)
            {
                foreach (KeyValuePair<string, JSONObject> item in jsonData.instance.ItemJsonData)
                {
                    itemList.Add(new item(item.Value["id"].I));
                }
            }
        }

        /// <summary>
        /// 通过类型筛选物品
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<item> GetDataForType(ItemTypes type)
        {
            if (type == (ItemTypes)999)
            {
                return itemList;
            }

            List<item> list = new List<item>();
            foreach (item item in itemList)
            {
                if ((int)item.itemType == (int)type)
                {
                    list.Add(item);
                }
            }
            return list;
        }
        public static List<item> GetDataForType(item.ItemType type, List<item> itemList)
        {
            List<item> list = new List<item>();

            foreach (item item in itemList)
            {
                if (item.itemType == type)
                {
                    list.Add(item);
                }
            }
            return list;
        }

        /// <summary>
        /// 通过品质筛选物品
        /// </summary>
        /// <param name="quality"></param>
        /// <returns></returns>
        public static List<item> GetDataForQuality(int quality)
        {
            List<item> list = new List<item>();

            foreach (item item in itemList)
            {
                if (item.quality == quality)
                {
                    list.Add(item);
                }
            }
            return list;
        }
        public static List<item> GetDataForQuality(int quality, List<item> itemList)
        {
            List<item> list = new List<item>();

            foreach (item item in itemList)
            {
                if (item.quality == quality)
                {
                    list.Add(item);
                }
            }
            return list;
        }

        /// <summary>
        /// 通过名称筛选物品
        /// </summary>
        /// <param name="list"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public static List<item> FilterItemData(List<item> list, string search = "")
        {
            if (search == "")
            {
                return list;
            }

            List<item> newList = new List<item>();
            foreach (item item in list)
            {
                if (search != "")
                {
                    if (item.itemName.Contains(search))
                    {
                        newList.Add(item);
                    }
                }
            }
            return newList;
        }



        /// <summary>
        /// 给物品列表分页
        /// </summary>
        /// <param name="list">筛选后的物品列表</param>
        /// <param name="page">当前页数</param>
        /// <param name="limit">每页数量</param>
        /// <returns></returns>
        public static void Pagination(List<item> list, KBEngine.Avatar player, int count, int page = 1, int limit = 24)
        {
            GUILayout.BeginArea(new Rect(20, 45, 700, 320));
            {
                GUILayout.BeginHorizontal(new GUIStyle { alignment = TextAnchor.UpperCenter });
                {
                    int beginNum = (page - 1) * limit;
                    int num = 0;
                    for (int i = beginNum; i < page * limit; i++)
                    {
                        if (i >= list.Count - 1)
                        {
                            break;
                        }

                        // QualityColors


                        item item = list[i];

                        if (XmGUI.Button(item.itemNameCN, item.itemIcon))
                        {
                            Debug.addLog(string.Format("获取物品：{0}，数量{1}", item.itemName, count));
                            player.addItem(item.itemID, Tools.CreateItemSeid(item.itemID), count);
                            Singleton.inventory.AddItem(item.itemID);
                        }
                        num++;
                        if (num >= 5)
                        {
                            XmGUI.hr();
                            num = 0;
                        }
                    }
                }
                GUILayout.EndHorizontal();
                GUILayout.BeginArea(new Rect(100, 270, 700, 300));
                {
                    XmGUI.PaginationList(page, list.Count, limit, PaginationDo);
                }
                GUILayout.EndArea();
            }
            GUILayout.EndArea();

        }

        public static void ItemTypeWindow()
        {
            List<myItemType> typeList = new List<myItemType> {
                new myItemType() {type = (ItemTypes)999, name = "全部"},
                new myItemType() {type =  ItemTypes.武器, name = "武器"},
                new myItemType() {type =  ItemTypes.衣服, name = "衣服"},
                new myItemType() {type =  ItemTypes.饰品, name = "饰品"},
                new myItemType() {type =  ItemTypes.技能书, name = "技能书"},
                new myItemType() {type =  ItemTypes.功法, name = "功法"},
                new myItemType() {type =  ItemTypes.丹药, name = "丹药"},
                new myItemType() {type =  ItemTypes.药材, name = "药材"},
                new myItemType() {type =  ItemTypes.任务, name = "任务"},
                new myItemType() {type =  ItemTypes.矿石, name = "矿石"},
                new myItemType() {type =  ItemTypes.丹炉, name = "丹炉"},
                new myItemType() {type =  ItemTypes.丹方, name = "丹方"},
                new myItemType() {type =  ItemTypes.药渣, name = "药渣"},
                new myItemType() {type =  ItemTypes.书籍, name = "书籍"},
                new myItemType() {type =  ItemTypes.秘籍, name = "秘籍"},
                new myItemType() {type =  ItemTypes.灵舟, name = "灵舟"},
                new myItemType() {type =  ItemTypes.秘药, name = "秘药"},
                new myItemType() {type =  ItemTypes.其他, name = "其他"},
            };
            GUILayout.BeginArea(new Rect(640, 0, 700, 600));
            {
                scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false, GUILayout.Width(100), GUILayout.Height(350));
                {
                    foreach (myItemType item in typeList)
                    {
                        Texture2D texture2D = new Texture2D(1, 1, TextureFormat.RGBA32, false);
                        if (item.type == type)
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
                            fixedWidth = 65,
                            margin = new RectOffset(5, 7, 0, 0),
                        };

                        if (GUILayout.Button(item.name, guistyle))
                        {
                            type = item.type;
                            nowPage = 1;
                        }
                    }
                }
                GUILayout.EndScrollView();

            }
            GUILayout.EndArea();
        }

        /// <summary>
        /// 获取物品列表
        /// </summary>
        public static void ItemWindow(KBEngine.Avatar player, int count, string search)
        {
            ItemTypeWindow();   // 物品类型

            List<item> myItem = GetDataForType(type);
            myItem = FilterItemData(myItem, search);
            Pagination(myItem, player, count, nowPage);
        }
        /// <summary>
        /// 分页栏点击
        /// </summary>
        public static void PaginationDo(int page)
        {
            nowPage = page;
        }

        private static int nowPage = 1;
        private static ItemTypes type = (ItemTypes)999;
        private static Vector2 scrollPosition;



    }

    public class myItemType
    {
        public ItemTypes type { get; set; }
        public string name { get; set; }

    }
}