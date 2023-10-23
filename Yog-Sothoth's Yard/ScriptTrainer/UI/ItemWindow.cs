
using System.Collections.Generic;
using BagSystem;
using Foundation.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityGameUI;
using Navigation = UnityGameUI.Navigation;

namespace ScriptTrainer;

internal class ItemWindow : MonoBehaviour
{
    private static int initialX;
    private static int initialY;
    private static int elementX;
    private static int elementY;
    // 列表数据
    private static List<Example.Item> GetItemData
    {
        get
        {
            List<Example.Item> ItemData = ItemManager.Instance.GetAllItem().Items;
            if (searchText != "") ItemData = FilterItemData(ItemManager.Instance.GetAllItem().Items);

            Debug.Log(ItemData.Count);

            // 对 DataList 进行分页
            List<Example.Item> list = new List<Example.Item>();
            int start = (page - 1) * conunt;
            int end = start + conunt;
            for (int i = start; i < end; i++)
            {
                if (i < ItemData.Count)
                {
                    list.Add(ItemData[i]);
                }
                else break;
            }
            maxPage = ItemData.Count / conunt;

            Debug.Log(start);


            return list;
        }
    }
    private static List<Example.Item> itemList = new List<Example.Item>();
    private static string searchText = "";
    private static GameObject uiText;
    private static int page = 1;
    private static int maxPage = 1;
    private static int conunt = 15;
    private static string uiText_text
    {
        get
        {
            return $"{page} / {maxPage}";
        }
    }
    private static List<GameObject> ItemButtons = new List<GameObject>();
    private static GameObject ItemPanel;

    static GameObject Panel;
    public ItemWindow(GameObject panel, int x, int y)
    {
        Panel = panel;
        initialX = elementX = x + 50;
        initialY = elementY = y;
        // elementX += 70;

        searchBar(panel);
        elementY += 10;
        hr();

        // 创建物品列表
        container();
        // 创建分页
        pageBar(panel);
    }

    // 物品列表
    private static void container()
    {
        // 先清空旧的 ItemPanel
        foreach (var item in ItemButtons)
        {
            UnityEngine.Object.Destroy(item);
        }
        ItemButtons.Clear();

        // elementX += 70;
        elementX = -200;
        elementY = 125;

        ItemPanel = UIControls.createUIPanel(Panel, "300", "600");
        ItemPanel.GetComponent<Image>().color = UIControls.HTMLString2Color("#424242FF");
        ItemPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(10, 0);

        int num = 0;
        foreach (var item in GetItemData)
        {
            var btn = createItemButton(GameAPI.GetLanguageStr(item.NameID), ItemPanel, GameAPI.GetSpriteByName(item.Icon), () =>
            {
                Debug.Log(item.ItemID);
                // 添加物品到背包
                // ItemManager.Instance.AddItem(item.ItemID, 1);
                //    BagManager.Instance.AddItem(10013, finalValue, true);
                UIWindows.SpawnInputDialog($"您想获取多少个{GameAPI.GetLanguageStr(item.NameID)}? ", "添加", "1", (string count) =>
                {
                    Debug.Log($"已添加{count}个{GameAPI.GetLanguageStr(item.NameID)}到背包");
                    BagManager.Instance.AddItem(item.ItemID, count.ConvertToIntDef(1), false);
                });
            });

            ItemButtons.Add(btn);

            num++;
            if (num % 3 == 0)
            {
                hr();
                // elementX += 70;
            }

        }


    }


    // 搜索框
    private void searchBar(GameObject panel)
    {
        elementY += 30;

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
            // Debug.Log(text);
            page = 1;
            searchText = text;
            container();
            ItemWindow.uiText.GetComponent<Text>().text = uiText_text;
            //Destroy(ItemPanel);
        });
    }

    // 搜索过滤
    private static List<Example.Item> FilterItemData(List<Example.Item> dataList)
    {
        if (searchText == "") return dataList;

        List<Example.Item> list = new List<Example.Item>();

        foreach (var item in dataList)
        {
            // Debug.Log($" {GameAPI.GetLanguageStr(item.NameID)} in {searchText} | {GameAPI.GetLanguageStr(item.NameID).Contains(searchText)}");
            if (GameAPI.GetLanguageStr(item.NameID).Contains(searchText))
            {
                list.Add(item);
            }
        }

        // Debug.Log(list.Count);

        return list;
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
            // Debug.Log("上一页");
            page--;
            if (page <= 0) page = 1;
            container();
            uiText.GetComponent<Text>().text = uiText_text;
        }, new Vector3(-100, 0, 0));
        prevBtn.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 20);
        // prevBtn.GetComponent<RectTransform>().localPosition = new Vector3(-100, 0, 0);

        // 下一页            
        GameObject nextBtn = UIControls.createUIButton(pageObj, backgroundColor, "下一页", () =>
        {
            Debug.Log("下一页");
            page++;
            if (page >= maxPage) page = maxPage;
            container();
            uiText.GetComponent<Text>().text = uiText_text;
        }, new Vector3(100, 0, 0));
        nextBtn.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 20);
        // nextBtn.GetComponent<RectTransform>().localPosition = new Vector3(100, 0, 0);
    }

    // 图标按钮
    private static GameObject createItemButton(string text, GameObject panel, Sprite itemIcon, UnityAction action)
    {
        // 根据品质设置背景颜色
        string qualityColor = "#FFFFFFFF";


        // 创建一个背景
        GameObject background = UIControls.createUIButton(panel, "#00000633", "", action);
        background.GetComponent<RectTransform>().localPosition = new Vector3(elementX, elementY, 0);
        background.GetComponent<RectTransform>().sizeDelta = new Vector2(180, 45);
        background.name = "background";

        // 名称
        Sprite txtBgSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#7AB900FF"));
        GameObject uiText = UIControls.createUIText(background, txtBgSprite, qualityColor);
        uiText.GetComponent<Text>().text = text;
        uiText.GetComponent<RectTransform>().localPosition = new Vector3(0, -10, 0);


        // 图标
        // Sprite BgSprite = UIControls.createSpriteFrmTexture(itemIcon);
        GameObject button = UIControls.createUIButton(background, "#00000033", "", action);
        button.GetComponent<Image>().sprite = itemIcon;
        RectTransform rt = button.GetComponent<RectTransform>();
        rt.localPosition = new Vector3(50, 0, 0);
        rt.sizeDelta = new Vector2(40, 40);

        elementX += 200;

        return background;

    }


    private static void hr()
    {
        elementX = initialX;
        elementY -= 60;
    }
}

