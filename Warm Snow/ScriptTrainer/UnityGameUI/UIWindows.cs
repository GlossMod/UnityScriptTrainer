using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace UnityGameUI
{
    internal class UIWindows
    {
        public UIWindows()
        {

        }

        // 输入对话框
        public static void SpawnInputDialog(string prompt, string title, string defaultText, Action<string> onFinish)
        {
            GameObject canvas = UIControls.createUICanvas();    // 创建画布
            Object.DontDestroyOnLoad(canvas);
            // 设置置顶显示
            canvas.GetComponent<Canvas>().overrideSorting = true;
            canvas.GetComponent<Canvas>().sortingOrder = 100;

            GameObject uiPanel = UIControls.createUIPanel(canvas, "70", "300", null);  // 创建面板
            uiPanel.GetComponent<Image>().color = UIControls.HTMLString2Color("#37474FFF"); // 设置背景颜色
           
            // 创建标题
            Sprite txtBgSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#7AB900FF"));
            GameObject uiText = UIControls.createUIText(uiPanel, txtBgSprite, "#FFFFFFFF");
            uiText.GetComponent<Text>().text = prompt;
            uiText.GetComponent<RectTransform>().localPosition = new Vector3(0, 10, 0);

            // 创建输入框
            Sprite inputFieldSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#212121FF"));
            GameObject uiInputField = UIControls.createUIInputField(uiPanel, inputFieldSprite, "#FFFFFFFF");
            uiInputField.GetComponent<InputField>().text = defaultText;
            uiInputField.GetComponent<RectTransform>().localPosition = new Vector3(-50, -10, 0);

            // 创建确定按钮
            GameObject uiButton = UIControls.createUIButton(uiPanel, "#8C9EFFFF", title, () =>
            {
                onFinish(uiInputField.GetComponent<InputField>().text);
                Object.Destroy(canvas);
            }, new Vector3(100, -10, 0));

            // 创建关闭按钮
            GameObject closeButton = UIControls.createUIButton(uiPanel, "#B71C1CFF", "X", () =>
            {
                Object.Destroy(canvas);
            }, new Vector3(350 / 2 - 10, 70 / 2 - 10, 0));
            // 设置closeButton宽高
            closeButton.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 20);
            // 字体颜色为白色
            closeButton.GetComponentInChildren<Text>().color = UIControls.HTMLString2Color("#FFFFFFFF");
        }

        // 带下拉框的对话框
        public static void SpawnDropdownDialog(string prompt, string title, List<string> options,  Action<int> onFinish)
        {
            GameObject canvas = UIControls.createUICanvas();    // 创建画布
            Object.DontDestroyOnLoad(canvas);
            // 设置置顶显示
            canvas.GetComponent<Canvas>().overrideSorting = true;
            canvas.GetComponent<Canvas>().sortingOrder = 100;
            

            GameObject uiPanel = UIControls.createUIPanel(canvas, "70", "300", null);  // 创建面板
            uiPanel.GetComponent<Image>().color = UIControls.HTMLString2Color("#37474FFF"); // 设置背景颜色
            

            // 创建标题
            Sprite txtBgSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#7AB900FF"));
            GameObject uiText = UIControls.createUIText(uiPanel, txtBgSprite, "#FFFFFFFF");
            uiText.GetComponent<Text>().text = prompt;
            uiText.GetComponent<RectTransform>().localPosition = new Vector3(0, 10, 0);

            // 创建下拉框
            Sprite dropdownBgSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#212121FF"));      // 背景颜色
            Sprite dropdownScrollbarSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#8C9EFFFF"));   // 滚动条颜色 (如果有的话
            Sprite dropdownDropDownSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#212121FF"));    // 框右侧小点的颜色
            Sprite dropdownCheckmarkSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#8C9EFFFF"));   // 选中时的颜色
            Sprite dropdownMaskSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#E65100FF"));        // 不知道是哪的颜色
            Color LabelColor = UIControls.HTMLString2Color("#EFEBE9FF");
            GameObject uiDropDown = UIControls.createUIDropDown(uiPanel, dropdownBgSprite, dropdownScrollbarSprite, dropdownDropDownSprite, dropdownCheckmarkSprite, dropdownMaskSprite, options, LabelColor);
            Object.DontDestroyOnLoad(uiDropDown);
            uiDropDown.GetComponent<RectTransform>().localPosition = new Vector3(-50, -10, 0);

            int m_call = 0;

            // 下拉框选中时触发方法
            uiDropDown.GetComponent<Dropdown>().onValueChanged.AddListener((int call) =>
            {
                m_call = call;
            });

            // 创建确定按钮
            GameObject uiButton = UIControls.createUIButton(uiPanel, "#8C9EFFFF", title, () =>
            {
                onFinish(m_call);
                Object.Destroy(canvas);
            }, new Vector3(100, -10, 0));

            // 创建关闭按钮
            GameObject closeButton = UIControls.createUIButton(uiPanel, "#B71C1CFF", "X", () =>
            {
                Object.Destroy(canvas);
            }, new Vector3(350 / 2 - 10, 70 / 2 - 10, 0));
            // 设置closeButton宽高
            closeButton.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 20);
            // 字体颜色为白色
            closeButton.GetComponentInChildren<Text>().color = Color.white;

        }

    }
}
