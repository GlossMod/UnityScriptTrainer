using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityGameUI;
using Object = UnityEngine.Object;

namespace ScriptTrainer
{
   

    internal class ByWindow
    {
        #region[UI]
        // 组件位置
        private static int initialX { get => 0; }
        private static int initialY { get => 0; }

        private static int elementX = initialX;
        private static int elementY = initialY;
        #endregion


        
        // ReSharper disable Unity.PerformanceAnalysis
        public void init(GameObject canvas)
        {
            GameObject By = UIControls.createUIPanel(canvas, "40", "250", null);
            By.name = "dst-by";
            By.GetComponent<RectTransform>().localPosition = new Vector3(550, -415, 0);
            By.GetComponent<Image>().color = UIControls.HTMLString2Color("#00000000");

            // 作者名称
            Sprite txtBgSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#7AB900FF"));
            GameObject uiText = UIControls.createUIText(By, txtBgSprite, "#FFFFFFFF");
            //uiText.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 30);
            uiText.GetComponent<RectTransform>().localPosition = new Vector3(-35, 0, 0);
            Text text = uiText.GetComponent<Text>();
            text.text = "作者;小莫";
            text.alignment = TextAnchor.MiddleLeft;
            text.fontSize = 14;

            // 相关按钮
            Transform button = Components.createUIButton(By);
            RectTransform rt = button.GetComponent<RectTransform>();
            rt.localPosition = new Vector3(elementX, elementY, 0);
            rt.sizeDelta = new Vector2(80, 30);    // 设置按钮大小
            button.GetComponentInChildren<Text>().text = "获取更新";
            button.GetComponent<UIButton>().onClick += (int a) =>
            {
                System.Diagnostics.Process.Start("https://mod.3dmgame.com/mod/173023");
            };
            Transform button2 = Components.createUIButton(By);
            RectTransform rt2 = button2.GetComponent<RectTransform>();
            rt2.localPosition = new Vector3(elementX + 90, elementY, 0);
            rt2.sizeDelta = new Vector2(80, 30);    // 设置按钮大小
            button2.GetComponentInChildren<Text>().text = "投喂小莫";
            button2.GetComponent<UIButton>().onClick += (int a) =>
            {
                System.Diagnostics.Process.Start("https://www.aoe.top/donate");
            };

        }
    }
}
