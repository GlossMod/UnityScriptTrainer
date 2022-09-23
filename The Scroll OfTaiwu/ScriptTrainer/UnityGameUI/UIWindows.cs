using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
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

    }
}
