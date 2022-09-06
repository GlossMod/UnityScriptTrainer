
using System;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace ScriptTrainer
{
    public class MainWindow: MonoBehaviour
    {
        #region[声明]
        // Trainer Base
        public static GameObject obj = null;
        public static MainWindow instance;
        public static bool initialized = false;
        public static bool optionToggle = false;
        private static TooltipGUI toolTipComp = null;
        
        // UI
        public static AssetBundle testAssetBundle = null;
        public static GameObject canvas = null;
        private static bool isVisible = false;
        private static GameObject uiPanel = null;
        private static readonly int width = Mathf.Min(Screen.width, 740);
        private static readonly int height = (Screen.height < 400) ? Screen.height : (450);

        // 按钮位置
        private static int elementX = -width / 2 + 60;
        private static int elementY = height / 2 - 80;
        #endregion

        internal static GameObject Create(string name)
        {
            obj = new GameObject(name);
            DontDestroyOnLoad(obj);

            var component = new MainWindow();

            toolTipComp = new TooltipGUI();
            toolTipComp.enabled = false;

            return obj;
        }

        public MainWindow()
        {
            instance = this;
        }

        public static void Initialize()
        {
            #region[初始化资源]
            if (testAssetBundle == null)
            {
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\AssetBundles\\testassetbundle"))
                {
                    //testAssetBundle = Il2CppAssetBundleManager.LoadFromFile(AppDomain.CurrentDomain.BaseDirectory + "\\AssetBundles\\testassetbundle");
                    testAssetBundle = AssetBundle.LoadFromFile(AppDomain.CurrentDomain.BaseDirectory + "\\AssetBundles\\testassetbundle");
                    if (testAssetBundle == null)
                    {
                        Debug.Log("AssetBundle 加载失败");
                        return;
                    }
                    // 输出 资源名称
                    foreach (var asset in testAssetBundle.GetAllAssetNames())
                    {
                        Debug.Log("   Asset Name: " + asset.ToString());
                    }

                    Debug.Log("完成");
                }
                else
                {
                    Debug.LogWarning("跳过 AssetBundle 加载 - testassetBundle 不存在于:" + AppDomain.CurrentDomain.BaseDirectory + "\\AssetBundles\\testassetbundle");
                    Debug.LogWarning("请确保“AssetBundles”文件夹已放入游戏根目录");
                }
            }
            #endregion

            instance.CreateUI();

            initialized = true;
        }

        #region[创建UI]
        private void CreateUI()
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
                AddTitle("【疯狂游戏大亨2】内置修改器 By:小莫");

                GameObject closeButton = UIControls.createUIButton(uiPanel, "#B71C1CFF", "X", () =>
                {
                    optionToggle = false;
                    canvas.SetActive(optionToggle);
                }, new Vector3(width / 2 + 10, height / 2 + 10, 0));
                closeButton.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 20);
                // 字体颜色为白色
                closeButton.GetComponentInChildren<Text>().color = UIControls.HTMLString2Color("#FFFFFFFF");

                #endregion

                #region[添加功能按钮]
                {
                    AddButton("添加现金", () =>
                    {
                        Scripts.AddMoney();
                    });
                    AddButton("添加粉丝", () =>
                    {
                        Scripts.AddFans();
                    });
                    
                    AddButton("工作效率", () =>
                    {
                        Scripts.AddMotivation();
                    });
                   
                    AddButton("超级员工", () =>
                    {
                        Scripts.SuperStaff();

                    });
                    AddButton("全能开发", () =>
                    {
                        Scripts.AllKnow();
                    });
                }
                ButtonHr();
                {
                    AddToggle("员工最低工资", 150, (bool state) =>
                    {
                        Scripts.NoSalary(state);
                    });

                    AddButton("按钮4", () =>
                    {
                        Debug.Log("按钮2被点击");
                    });
                }

                #endregion


                #endregion

                isVisible = true;

                //log.LogMessage("Complete!");
                canvas.SetActive(optionToggle);
                Debug.Log("初始化完成!");
            }
        }

        #region[添加组件]
        public GameObject AddButton(string Text, UnityAction action)
        {
            string backgroundColor = "#8C9EFFFF";
            Vector3 localPosition = new Vector3(elementX, elementY, 0);

            elementX += 90;
            return UIControls.createUIButton(uiPanel, backgroundColor, Text, action, localPosition);
        }

        // 添加复选框
        public GameObject AddToggle(string Text, int width, UnityAction<bool> action)
        {
            // 计算x轴偏移
            elementX += width / 2 - 30;

            Sprite toggleBgSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#3E3E42FF"));
            Sprite toggleSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#18FFFFFF"));
            GameObject uiToggle = UIControls.createUIToggle(uiPanel, toggleBgSprite, toggleSprite);
            uiToggle.GetComponentInChildren<Text>().color = Color.white;
            uiToggle.GetComponentInChildren<Toggle>().isOn = false;
            uiToggle.GetComponent<RectTransform>().localPosition = new Vector3(elementX, elementY, 0);
            
            uiToggle.GetComponentInChildren<Text>().text = Text;            
            uiToggle.GetComponentInChildren<Toggle>().onValueChanged.AddListener(action);


            elementX += width / 2 + 10;

            return uiToggle;
        }

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
            Text text =  uiText.GetComponent<Text>();
            text.text = Title;
            text.alignment = TextAnchor.MiddleCenter;
            text.fontSize = 16;

            return uiText;
        }
        public void ButtonHr()
        {
            elementX = -width / 2 + 60;
            elementY -= 50;
        }

        #endregion

        

        #endregion



        
    }
}
