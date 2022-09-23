/**
 * 
 * 准备操作后端用的辅助dll
 * 目前交互问题不知道怎么解决
 * 负重、特性、修炼、等很多功能全部在后端的dll里面
 * Assembly-CSharp.dll 中很多方法不知道该怎么实现 
 * 现在有点头疼, 不知道有没有大佬能帮我想想办法
 * 暂时先把代码提交一些
 * 周末继续弄
 * 
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityGameUI;
using Navigation = UnityGameUI.Navigation;
using Object = UnityEngine.Object;

namespace STGameData
{
    public class STGameData: MonoBehaviour
    {
        #region[声明]
        // Trainer Base
        public GameObject obj = null;
        public STGameData instance;
        public bool initialized = false;
        public bool _optionToggle = false;
        private TooltipGUI toolTipComp = null;

        // UI
        public AssetBundle testAssetBundle = null;
        public GameObject canvas = null;
        private bool isVisible = false;
        private GameObject uiPanel = null;
        private static readonly int width = Mathf.Min(Screen.width, 740);
        private static readonly int height = (Screen.height < 400) ? Screen.height : (450);

        // 界面开关
        public bool optionToggle
        {
            get { return _optionToggle; }
            set
            {
                // 设置鼠标显示
                if (value)
                {
                    //Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
                else
                {
                    Cursor.visible = false;
                }

                _optionToggle = value;
            }
        }

        // 按钮位置
        private static int initialX
        {
            get
            {
                return -width / 2 + 120;
            }
        }
        private static int initialY
        {
            get
            {
                return height / 2 - 60;
            }
        }
        private int elementX = initialX;
        private int elementY = initialY;
        #endregion

        public void Start()
        {
            Initialize();
        }

        public void Update()
        {
            if (!initialized)
            {
                Initialize();
            }

            if (Input.GetKeyDown(KeyCode.F10))
            {
                optionToggle = !optionToggle;
                Debug.Log("后端窗口开关状态：" + optionToggle.ToString());

                canvas.SetActive(optionToggle);
                Event.current.Use();
            }
        }

        public void Initialize()
        {
            initialized = true;
            instance.CreateUI();            
        }


        #region[创建UI]
        private void CreateUI()
        {
            try
            {
                if (canvas == null)
                {
                    Debug.Log("创建 UI 元素");

                    canvas = UIControls.createUICanvas();
                    Object.DontDestroyOnLoad(canvas);

                    // 设置背景
                    GameObject background = UIControls.createUIPanel(canvas, (height + 40).ToString(), (width + 40).ToString(), null);
                    background.GetComponent<Image>().color = UIControls.HTMLString2Color("#2D2D30FF");


                    // 将面板添加到画布, 请参阅 createUIPanel 了解我们将高度/宽度作为字符串传递的原因
                    uiPanel = UIControls.createUIPanel(canvas, height.ToString(), width.ToString(), null);
                    // 设置背景颜色
                    uiPanel.GetComponent<Image>().color = UIControls.HTMLString2Color("#424242FF");

                    // 这就是我们将如何挂钩鼠标事件以进行窗口拖动
                    //EventTrigger comp1 = background.AddComponent<EventTrigger>();
                    //WindowDragHandler comp2 = background.AddComponent<WindowDragHandler>();


                    #region[面板元素]


                    #region[创建标题 和 关闭按钮]
                    AddTitle("【太吾绘卷】内置修改器 By:小莫 1.2 后端");

                    GameObject closeButton = UIControls.createUIButton(uiPanel, "#B71C1CFF", "X", () =>
                    {
                        optionToggle = false;
                        canvas.SetActive(optionToggle);
                    }, new Vector3(width / 2 + 10, height / 2 + 10, 0));
                    closeButton.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 20);
                    // 字体颜色为白色
                    closeButton.GetComponentInChildren<Text>().color = UIControls.HTMLString2Color("#FFFFFFFF");
                    #endregion

                    
                    #region[编辑特性]
                    ResetCoordinates(true, true);
                    GameObject FeatureScripts = UIControls.createUIPanel(uiPanel, "410", "600", null);
                    FeatureScripts.GetComponent<Image>().color = UIControls.HTMLString2Color("#424242FF");
                    FeatureScripts.GetComponent<RectTransform>().anchoredPosition = new Vector2(-70, -20);



                    //FeatureWindow featureWindow = new FeatureWindow(FeatureScripts, elementX, elementY);

                    #endregion

                    #endregion


                    #region[创建导航栏]
                    // 分割线
                    GameObject DividingLine = UIControls.createUIPanel(uiPanel, (height - 40).ToString(), "10", null);
                    DividingLine.GetComponent<Image>().color = UIControls.HTMLString2Color("#2D2D30FF");
                    DividingLine.GetComponent<RectTransform>().anchoredPosition = new Vector3(width / 2 - 200 + 80, -20, 0);

                    //// 按钮
                    GameObject NavPanel = UIControls.createUIPanel(uiPanel, (height - 40).ToString(), "40", null);
                    NavPanel.GetComponent<Image>().color = UIControls.HTMLString2Color("#424242FF");
                    NavPanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(width / 2 - 100, -20, 0);

                    Navigation[] nav = new Navigation[]
                    {
                        //new Navigation("BasicScripts","常用功能", BasicScripts, true),
                        //new Navigation("PlayerScripts","玩家功能", PlayerScripts, false),
                        //new Navigation("ItemScripts","添加物品", ItemScripts, false),
                        //new Navigation("MapAreaScripts","地区恩义", MapAreaScripts, false),
                        new Navigation("FeatureScripts", "编辑特性", FeatureScripts, true),
                        //new Navigation("ItemScripts", "获取物品", ItemScripts, false),

                    };

                    UINavigation.Initialize(nav, NavPanel);

                    #endregion


                    isVisible = true;

                    //log.LogMessage("Complete!");
                    canvas.SetActive(optionToggle);
                    Debug.Log("修改器初始化完成!");
                    Debug.Log("按F9可开关修改器菜单");
                }
                else
                {
                    //Debug.Log("UI创建失败,未找到玩家");
                    initialized = false;
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                initialized = false;
            }

        }

        #endregion








        #region[添加组件]

        // 添加标题
        public GameObject AddTitle(string Title)
        {
            GameObject TitleBackground = UIControls.createUIPanel(canvas, "30", (width - 20).ToString(), null);
            TitleBackground.GetComponent<Image>().color = UIControls.HTMLString2Color("#2D2D30FF");
            TitleBackground.GetComponent<RectTransform>().localPosition = new Vector3(0, height / 2 - 30, 0);

            Sprite txtBgSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#7AB900FF"));
            GameObject uiText = UIControls.createUIText(TitleBackground, txtBgSprite, "#FFFFFFFF");
            uiText.GetComponent<RectTransform>().sizeDelta = new Vector2(width - 10, 30);
            uiText.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            Text text = uiText.GetComponent<Text>();
            text.text = Title;
            text.alignment = TextAnchor.MiddleCenter;
            text.fontSize = 16;

            return uiText;
        }

        public GameObject AddButton(string Text, GameObject panel, UnityAction action)
        {
            string backgroundColor = "#8C9EFFFF";
            Vector3 localPosition = new Vector3(elementX, elementY, 0);
            elementX += 90;

            GameObject button = UIControls.createUIButton(panel, backgroundColor, Text, action, localPosition);

            // 按钮样式
            button.AddComponent<Shadow>().effectColor = UIControls.HTMLString2Color("#000000FF");   // 添加阴影
            button.GetComponent<Shadow>().effectDistance = new Vector2(2, -2);              // 设置阴影偏移
            button.GetComponentInChildren<Text>().fontSize = 14;     // 设置字体大小           
            button.GetComponent<RectTransform>().sizeDelta = new Vector2(80, 30);    // 设置按钮大小


            return button;
        }

        // 添加复选框
        public GameObject AddToggle(string Text, int width, GameObject panel, UnityAction<bool> action)
        {
            // 计算x轴偏移
            elementX += width / 2 - 30;

            Sprite toggleBgSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#3E3E42FF"));
            Sprite toggleSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#18FFFFFF"));
            GameObject uiToggle = UIControls.createUIToggle(panel, toggleBgSprite, toggleSprite);
            uiToggle.GetComponentInChildren<Text>().color = Color.white;
            uiToggle.GetComponentInChildren<Toggle>().isOn = false;
            uiToggle.GetComponent<RectTransform>().localPosition = new Vector3(elementX, elementY, 0);

            uiToggle.GetComponentInChildren<Text>().text = Text;
            uiToggle.GetComponentInChildren<Toggle>().onValueChanged.AddListener(action);


            elementX += width / 2 + 10;

            return uiToggle;
        }

        // 添加输入框
        public GameObject AddInputField(string Text, int width, string defaultText, GameObject panel, UnityAction<string> action)
        {
            // 计算x轴偏移
            elementX += width / 2 - 30;

            // label
            Sprite txtBgSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#7AB900FF"));
            GameObject uiText = UIControls.createUIText(panel, txtBgSprite, "#FFFFFFFF");
            uiText.GetComponent<Text>().text = Text;
            uiText.GetComponent<RectTransform>().localPosition = new Vector3(elementX, elementY, 0);
            //uiText.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 30);
            uiText.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;


            // 坐标偏移
            elementX += 10;

            // 输入框
            Sprite inputFieldSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#212121FF"));
            GameObject uiInputField = UIControls.createUIInputField(panel, inputFieldSprite, "#FFFFFFFF");
            uiInputField.GetComponent<InputField>().text = defaultText;
            uiInputField.GetComponent<RectTransform>().localPosition = new Vector3(elementX, elementY, 0);
            uiInputField.GetComponent<RectTransform>().sizeDelta = new Vector2(width - 60, 30);

            // 文本框失去焦点时触发方法
            uiInputField.GetComponent<InputField>().onEndEdit.AddListener(action);

            elementX += width / 2 + 10;
            return uiInputField;
        }

        // 添加下拉框
        public GameObject AddDropdown(string Text, int width, List<string> options, GameObject panel, UnityAction<int> action)
        {
            // 计算x轴偏移
            elementX += width / 2 - 30;

            // label
            Sprite txtBgSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#7AB900FF"));
            GameObject uiText = UIControls.createUIText(panel, txtBgSprite, "#FFFFFFFF");
            uiText.GetComponent<Text>().text = Text;
            uiText.GetComponent<RectTransform>().localPosition = new Vector3(elementX, elementY, 0);
            //uiText.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 30);
            uiText.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

            // 坐标偏移
            elementX += 60;

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
            uiDropDown.GetComponent<Dropdown>().onValueChanged.AddListener(action);

            elementX += width / 2 + 60;
            return uiDropDown;
        }

        // 添加小标题
        public GameObject AddH3(string text, GameObject panel)
        {
            elementX += 40;

            Sprite txtBgSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#7AB900FF"));
            GameObject uiText = UIControls.createUIText(panel, txtBgSprite, "#FFFFFFFF");
            uiText.GetComponent<Text>().text = text;
            uiText.GetComponent<RectTransform>().localPosition = new Vector3(elementX, elementY, 0);

            // 设置字体样式为h3小标题
            uiText.GetComponent<Text>().fontSize = 14;
            uiText.GetComponent<Text>().fontStyle = FontStyle.Bold;
            hr();
            elementY += 20;
            elementX += 10;
            return uiText;
        }

        // 换行
        public void hr(int offsetX = 0, int offsetY = 0)
        {
            ResetCoordinates(true);
            elementX += offsetX;
            elementY -= 50 + offsetY;

        }
        // 重置坐标
        public void ResetCoordinates(bool x, bool y = false)
        {
            if (x) elementX = initialX;
            if (y) elementY = initialY;
        }

        #endregion


    }
}
