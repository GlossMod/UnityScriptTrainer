using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityGameUI;
// using Object = UnityEngine.Object;

namespace ScriptTrainer;
public static class Components
{
    #region[添加组件]

    // 添加标题
    public static GameObject AddTitle(string Title, GameObject panel)
    {
        GameObject TitleBackground = UIControls.createUIPanel(panel, "30", (MainWindow.width - 20).ToString(), null);
        TitleBackground.GetComponent<Image>().color = UIControls.HTMLString2Color("#2D2D30FF");
        TitleBackground.GetComponent<RectTransform>().localPosition = new Vector3(0, MainWindow.height / 2 - 30, 0);

        Sprite txtBgSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#7AB900FF"));
        GameObject uiText = UIControls.createUIText(TitleBackground, txtBgSprite, "#FFFFFFFF");
        uiText.GetComponent<RectTransform>().sizeDelta = new Vector2(MainWindow.width - 10, 30);
        uiText.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        Text text = uiText.GetComponent<Text>();
        text.text = Title;
        text.alignment = TextAnchor.MiddleCenter;
        text.fontSize = 16;

        return uiText;
    }

    public static GameObject AddButton(string Text, GameObject panel, UnityAction action)
    {
        string backgroundColor = "#8C9EFFFF";
        Vector3 localPosition = new Vector3(MainWindow.ElementX, MainWindow.ElementY, 0);
        MainWindow.ElementX += 90;

        GameObject button = UIControls.createUIButton(panel, backgroundColor, Text, action, localPosition);

        // 按钮样式
        button.AddComponent<Shadow>().effectColor = UIControls.HTMLString2Color("#000000FF");   // 添加阴影
        button.GetComponent<Shadow>().effectDistance = new Vector2(2, -2);              // 设置阴影偏移
        button.GetComponentInChildren<Text>().fontSize = 14;     // 设置字体大小           
        button.GetComponent<RectTransform>().sizeDelta = new Vector2(80, 30);    // 设置按钮大小


        return button;
    }

    // 添加复选框
    public static GameObject AddToggle(string Text, int width, GameObject panel, UnityAction<bool> action)
    {
        // 计算x轴偏移
        MainWindow.ElementX += width / 2 - 30;

        Sprite toggleBgSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#3E3E42FF"));
        Sprite toggleSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#18FFFFFF"));
        GameObject uiToggle = UIControls.createUIToggle(panel, toggleBgSprite, toggleSprite);
        uiToggle.GetComponentInChildren<Text>().color = Color.white;
        uiToggle.GetComponentInChildren<Toggle>().isOn = false;
        uiToggle.GetComponent<RectTransform>().localPosition = new Vector3(MainWindow.ElementX, MainWindow.ElementY, 0);

        uiToggle.GetComponentInChildren<Text>().text = Text;
        uiToggle.GetComponentInChildren<Toggle>().onValueChanged.AddListener(action);


        MainWindow.ElementX += width / 2 + 10;

        return uiToggle;
    }

    // 添加输入框
    public static GameObject AddInputField(string Text, int width, string defaultText, GameObject panel, UnityAction<string> action, ref int ElementX, ref int ElementY)
    {
        // 计算x轴偏移
        ElementX += width / 2 - 30;

        // label
        Sprite txtBgSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#7AB900FF"));
        GameObject uiText = UIControls.createUIText(panel, txtBgSprite, "#FFFFFFFF");
        uiText.GetComponent<Text>().text = Text;
        uiText.GetComponent<RectTransform>().localPosition = new Vector3(ElementX, ElementY, 0);
        //uiText.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 30);
        uiText.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;


        // 坐标偏移
        ElementX += 10;

        // 输入框
        Sprite inputFieldSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#212121FF"));
        GameObject uiInputField = UIControls.createUIInputField(panel, inputFieldSprite, "#FFFFFFFF");
        uiInputField.GetComponent<InputField>().text = defaultText;
        uiInputField.GetComponent<RectTransform>().localPosition = new Vector3(ElementX, ElementY, 0);
        uiInputField.GetComponent<RectTransform>().sizeDelta = new Vector2(width - 60, 30);

        // 文本框失去焦点时触发方法
        uiInputField.GetComponent<InputField>().onEndEdit.AddListener(action);

        ElementX += width / 2 + 10;
        return uiInputField;
    }

    // 添加下拉框
    public static GameObject AddDropdown(string Text, int width, List<string> options, GameObject panel, UnityAction<int> action, ref int ElementX, ref int ElementY)
    {
        // 计算x轴偏移
        ElementX += width / 2 - 30;

        // label
        Sprite txtBgSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#7AB900FF"));
        GameObject uiText = UIControls.createUIText(panel, txtBgSprite, "#FFFFFFFF");
        uiText.GetComponent<Text>().text = Text;
        uiText.GetComponent<RectTransform>().localPosition = new Vector3(ElementX, ElementY, 0);
        //uiText.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 30);
        uiText.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

        // 坐标偏移
        ElementX += 60;

        // 创建下拉框
        Sprite dropdownBgSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#212121FF"));      // 背景颜色
        Sprite dropdownScrollbarSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#8C9EFFFF"));   // 滚动条颜色 (如果有的话
        Sprite dropdownDropDownSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#212121FF"));    // 框右侧小点的颜色
        Sprite dropdownCheckmarkSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#8C9EFFFF"));   // 选中时的颜色
        Sprite dropdownMaskSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#E65100FF"));        // 不知道是哪的颜色
        Color LabelColor = UIControls.HTMLString2Color("#EFEBE9FF");
        GameObject uiDropDown = UIControls.createUIDropDown(panel, dropdownBgSprite, dropdownScrollbarSprite, dropdownDropDownSprite, dropdownCheckmarkSprite, dropdownMaskSprite, options, LabelColor);
        Object.DontDestroyOnLoad(uiDropDown);
        uiDropDown.GetComponent<RectTransform>().localPosition = new Vector3(ElementX, ElementY, 0);

        // 下拉框选中时触发方法
        uiDropDown.GetComponent<Dropdown>().onValueChanged.AddListener(action);

        ElementX += width / 2 + 60;
        return uiDropDown;
    }

    // 添加小标题
    public static GameObject AddH3(string text, GameObject panel, ref int ElementX, ref int ElementY, Color color = default(Color))
    {
        ElementX += 40;

        Sprite txtBgSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#7AB900FF"));
        GameObject uiText = UIControls.createUIText(panel, txtBgSprite, "#FFFFFFFF");
        uiText.GetComponent<Text>().text = text;
        uiText.GetComponent<RectTransform>().localPosition = new Vector3(ElementX, ElementY, 0);
        //uiText.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 30);  // 设置宽度

        // 设置字体样式为h3小标题
        uiText.GetComponent<Text>().fontSize = 14;
        uiText.GetComponent<Text>().fontStyle = FontStyle.Bold;

        // 设置字体颜色
        if (color != default(Color)) uiText.GetComponent<Text>().color = color;

        Hr();
        ElementY += 20;
        ElementX += 10;
        return uiText;
    }

    // 换行
    public static void Hr(int offsetX = 0, int offsetY = 0)
    {
        ResetCoordinates(true);
        MainWindow.ElementX += offsetX;
        MainWindow.ElementY -= 50 + offsetY;

    }
    // 重置坐标
    public static void ResetCoordinates(bool x, bool y = false)
    {
        if (x) MainWindow.ElementX = MainWindow.InitialX;
        if (y) MainWindow.ElementY = MainWindow.InitialY;
    }

    #endregion

}
