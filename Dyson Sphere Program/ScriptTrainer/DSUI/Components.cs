
using UnityEngine;
using UnityEngine.UI;
using UnityGameUI;

namespace ScriptTrainer
{
    public static class Components
    {
        #region[声明]
        private const float kWidth = 160f;
        private const float kThickHeight = 30f;
        private const float kThinHeight = 20f;
        private static Vector2 s_ThickElementSize = new Vector2(160f, 30f);
        private static Vector2 s_ThinElementSize = new Vector2(160f, 20f);
        private static Vector2 s_ImageElementSize = new Vector2(100f, 100f);
        private static Color s_DefaultSelectableColor = new Color(1f, 1f, 1f, 1f);
        private static Color s_PanelColor = new Color(1f, 1f, 1f, 0.392f);
        private static Color s_TextColor = new Color(0.19607843f, 0.19607843f, 0.19607843f, 1f);

        public struct Resources
        {
            public Sprite standard;
            public Sprite background;
            public Sprite inputField;
            public Sprite knob;
            public Sprite checkmark;
            public Sprite dropdown;
            public Sprite mask;
        }
        #endregion


        // 创建背景 复制的游戏中物品清单样式
        public static UIInventoryWindow createUIPanel(GameObject canvas,string title, float width, float height)
        {
            UIInventoryWindow background = Object.Instantiate<UIInventoryWindow>(UIRoot.instance.uiGame.inventoryWindow, canvas.transform);

            //background._Create();
            //background._Init(null);
            //background._Open();

            // 设置宽高
            RectTransform rectTransform = background.GetComponent<RectTransform>();
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);

            Transform panel_bg = background.transform.Find("panel-bg"); // 背景

            // 删除多余的内容
            Object.Destroy(background.transform.Find("storage-grid").gameObject);
            Object.Destroy(background.transform.Find("delivery-panel").gameObject);
            Object.Destroy(panel_bg.transform.Find("glass").gameObject);
            Object.Destroy(panel_bg.transform.Find("deco").gameObject);
            Object.Destroy(panel_bg.transform.Find("deco (1)").gameObject);
            Object.Destroy(panel_bg.transform.Find("deco (2)").gameObject);
            Object.Destroy(panel_bg.transform.Find("deco (3)").gameObject);
            Object.Destroy(panel_bg.transform.Find("tip-text").gameObject);
            //Object.Destroy(panel_bg.transform.Find("btn-box").gameObject);

            // 设置标题
            var title_text = panel_bg.transform.Find("title-text");
            Object.Destroy(title_text.GetComponentInChildren<Localizer>());
            title_text.GetComponentInChildren<Text>().text = title;
            
            // 显示背景
            background.gameObject.SetActive(true);
            return background;
        }

        public static Transform createUIButton(GameObject canvas)
        {
            //UIEscMenu escMenu = Object.Instantiate<UIEscMenu>(UIRoot.instance.uiGame.escMenu);
            //Transform button =  escMenu.transform.Find("button (1)");

            Transform button = Object.Instantiate(UIRoot.instance.uiGame.escMenu.transform.Find("button (1)"));
            button.SetParent(canvas.transform);
            button.GetComponentInChildren<Text>().fontSize = 14;     // 设置字体大小

            Object.Destroy(button.GetComponentInChildren<Localizer>());
            
            button.name = "button";
            return button;
        }

        //// 创建文本框
        //public static Transform createInputField(GameObject canvas)
        //{
        //    UIMechaEditor mechaEditor = Object.Instantiate<UIMechaEditor>(UIRoot.instance.uiMechaEditor);

        //    // 获取一个输入框
        //    Transform input = mechaEditor.transform.Find("Left Panel/scroll-view/Viewport/Left Panel Content/save-group/fold-group-0/input-name");
        //    input.SetParent(canvas.transform);

        //    Object.Destroy(input.GetComponentInChildren<Localizer>());


        //    input.name = "input";

        //    return input;
        //}

        // 创建 加减框
        public static Transform createCount(GameObject canvas)
        {
            // Replicator Window

            //UIReplicatorWindow uIReplicatorWindow = Object.Instantiate<UIReplicatorWindow>(UIRoot.instance.uiGame.replicator);
            //Transform count = uIReplicatorWindow.transform.Find("recipe-tree/count");

            Transform count = Object.Instantiate(UIRoot.instance.uiGame.replicator.transform.Find("recipe-tree/count"));


            count.SetParent(canvas.transform);
            
            return count;

        }

        // 创建 滚动条
        //public static Transform createItemScrollbar(GameObject canvas)
        //{
        //    UIPerformancePanel performancePanelUI = Object.Instantiate<UIPerformancePanel>(UIRoot.instance.uiGame.statWindow.performancePanelUI);

        //    //performancePanelUI._Init(null);
        //    performancePanelUI._Create();
        //    performancePanelUI._Init(null);
        //    performancePanelUI._Open();

        //    ScrollRect cpuScrollRect = performancePanelUI.cpuScrollRect;

        //    cpuScrollRect.transform.SetParent(canvas.transform);
        //    cpuScrollRect.gameObject.name = "Scroll View";

        //    RectTransform rt = cpuScrollRect.GetComponent<RectTransform>();
        //    rt.localPosition = new Vector3(0, 0, 0);

        //    Transform Viewport = cpuScrollRect.transform.Find("Viewport");  
        //    Transform Content = Viewport.transform.Find("Content");
        //    Transform Scrollbar_Horizontal = cpuScrollRect.transform.Find("Scrollbar Horizontal");
        //    Transform Scrollbar_Vertical = cpuScrollRect.transform.Find("Scrollbar Vertical");

        //    //Object.Destroy(cpuScrollRect.transform.Find("Scrollbar Horizontal").gameObject);
        //    Object.Destroy(Content.transform.Find("label").gameObject);
        //    Object.Destroy(Content.transform.Find("value-1").gameObject);
        //    Object.Destroy(Content.transform.Find("value-2").gameObject);


        //    RectTransform rt_Viewport = Viewport.GetComponent<RectTransform>();
        //    rt_Viewport.anchorMin = Vector2.zero;
        //    rt_Viewport.anchorMax = Vector2.one;
        //    rt_Viewport.sizeDelta = Vector2.zero;
        //    rt_Viewport.pivot = Vector2.up;

        //    RectTransform rt_Content = Content.GetComponent<RectTransform>();
        //    rt_Content.anchorMin = Vector2.up;
        //    rt_Content.anchorMax = Vector2.one;
        //    //rt_Content.sizeDelta = new Vector2(0f, 300f);
        //    rt_Content.pivot = Vector2.up;

        //    // 设定滚动条
        //    ScrollRect sr = cpuScrollRect.GetComponent<ScrollRect>();
        //    sr.content = rt_Content;
        //    sr.viewport =  rt_Viewport;
        //    sr.horizontalScrollbar = Scrollbar_Horizontal.GetComponent<Scrollbar>();
        //    sr.verticalScrollbar = Scrollbar_Vertical.GetComponent<Scrollbar>();
        //    sr.horizontalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;
        //    sr.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;
        //    sr.horizontalScrollbarSpacing = -3f;
        //    sr.verticalScrollbarSpacing = -3f;
            

        //    return cpuScrollRect.transform;
        //}
    }
}
