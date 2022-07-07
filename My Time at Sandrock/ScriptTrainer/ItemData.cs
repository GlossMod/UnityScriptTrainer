using Pathea;
using Pathea.FrameworkNs;
using Pathea.ItemNs;
using Pathea.UISystemV2.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ScriptTrainer
{
    class ItemData : MonoBehaviour
    {
        public static List<ItemPrototype> itemList;
        private static int nowPage = 1;
        private static Vector2 scrollPosition;
        public ItemData()
        {
            itemList = Module<ItemPrototypeModule>.Self.GetDataListByName("");
           
        }

        /// <summary>
        /// 通过名称筛选物品
        /// </summary>
        /// <param name="list"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public static List<ItemPrototype> FilterItemData(string search = "")
        {
            return Module<ItemPrototypeModule>.Self.GetDataListByName(search);
        }


        /// <summary>
        /// 给物品列表分页
        /// </summary>
        /// <param name="list">筛选后的物品列表</param>
        /// <param name="page">当前页数</param>
        /// <param name="limit">每页数量</param>
        /// <returns></returns>
        public void Pagination(List<ItemPrototype> list, int count, int page = 1, int limit = 35)
        {
            GUILayout.BeginArea(new Rect(20, 45, 700, 700));
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
                        var item = list[i];
                        var itemName = Module<ItemPrototypeModule>.Self.GetItemName(item.id);
                        Texture2D icon = Resources.Load<Texture2D>(item.iconPath);

                        if (XmGUI.Button(itemName))
                        {
                            Debug.LogWarning(string.Format("获取物品：{0}，数量{1}", itemName, count));

                            new MiscCmd().AddItemToBag(item.id, count);
                        }

                        num++;
                        if (num >= 7)
                        {
                            XmGUI.hr();
                            num = 0;
                        }
                    }                   
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginArea(new Rect(40,250, 700, 300));
                {
                    XmGUI.PaginationList(page, list.Count, limit, PaginationDo);
                }
                GUILayout.EndArea();
            }
            GUILayout.EndArea();            
        }

        public void ItemWindow(int count, string search)
        {
            List<ItemPrototype> myItem = itemList;
            myItem = FilterItemData(search);
            Pagination(myItem, count, nowPage);
        }

        /// <summary>
        /// 分页栏点击
        /// </summary>
        public static void PaginationDo(int page)
        {
            nowPage = page;
        }


        
    }
}
