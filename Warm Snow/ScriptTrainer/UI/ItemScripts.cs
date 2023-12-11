using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameUI;

namespace ScriptTrainer;
public class ItemScripts
{

    public static PlayerAnimControl player = PlayerAnimControl.instance;
    private static string searchText = "";
    private static List<GameObject> ItemButtons = new List<GameObject>();
    private static GameObject ItemPanel;
    private static GameObject Panel;

    private static int page = 1;
    private static int maxPage = 1;
    private static int conunt = 36;
    private static GameObject uiText;
    private static string uiText_text
    {
        get
        {
            return $"{page} / {maxPage}";
        }
    }

    #region[物品列表]

    static List<PN> ItemList
    {
        get
        {
            // 移除为 PN.None 的内容
            var list = new List<PN>();
            List<PN> r_item = new List<PN>();
            foreach (PN i in Enum.GetValues(typeof(PN)))
            {
                Potion potion = default(Potion);
                potion.PotionName = i;
                string text = TextControl.instance.PotionTitle(potion, false);

                if (searchText != "" && text.Contains(searchText))
                {
                    list.Add((PN)i);
                }
                if (searchText == "" && (PN)i != PN.None)
                {
                    list.Add((PN)i);
                }
            }

            int start = (page - 1) * conunt;
            int end = start + conunt;
            if (end > list.Count) end = list.Count;

            for (int i = start; i < end; i++)
            {
                r_item.Add(list[i]);
            }

            maxPage = (int)Math.Ceiling((double)list.Count / conunt);

            return r_item;
        }
    }

    #endregion

    public ItemScripts(GameObject panel)
    {
        ItemScripts.Panel = panel;
        MainWindow.ElementY += 10;
        #region[获取物品]
        Components.AddInputField("搜索", 150, searchText, panel, (string text) =>
        {
            page = 1;
            Debug.Log(text);
            searchText = text;
            container();
            ItemScripts.uiText.GetComponent<Text>().text = uiText_text;
        });

        container();
        pageBar(panel);

        #endregion

    }

    // 主体内容
    private static void container()
    {
        PlayerAnimControl instance = PlayerAnimControl.instance;

        Components.ResetCoordinates(true);
        MainWindow.ElementY = 125;

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

        foreach (PN item in ItemList)
        {
            if (item != PN.None)
            {

                Potion potion = default(Potion);
                potion.PotionName = item;
                string text = TextControl.instance.PotionTitle(potion, false);

                Components.AddButton(text, ItemPanel, () =>
                {
                    PotionDropPool.instance.Pop(item, 2, instance.transform.position, true, true);
                });

                num++;
                if (num % 6 == 0)
                {
                    Components.Hr();
                }
            }
        }
    }


    // 分页
    private void pageBar(GameObject panel)
    {
        // 背景
        GameObject pageObj = UIControls.createUIPanel(panel, "40", "500");
        pageObj.GetComponent<Image>().color = UIControls.HTMLString2Color("#424242FF");
        pageObj.GetComponent<RectTransform>().localPosition = new Vector3(0, MainWindow.ElementY, 0);

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
}
