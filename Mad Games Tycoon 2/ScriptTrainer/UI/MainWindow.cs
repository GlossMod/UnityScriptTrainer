
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityGameUI;
using Navigation = UnityGameUI.Navigation;

namespace ScriptTrainer;
public class MainWindow : MonoBehaviour
{
    #region 声明变量

    // Trainer Base
    public static MainWindow instance;
    public static bool initialized = false;
    // UI
    public static GameObject canvas = null;
    public static readonly int width = Mathf.Min(Screen.width, 740);
    public static readonly int height = (Screen.height < 400) ? Screen.height : (450);

    // UI
    private static GameObject uiPanel = null;



    // 窗口开关
    public static bool OptionToggle
    {
        get => canvas.activeSelf;
        set
        {
            if (initialized) canvas.SetActive(value);
            else instance.Init();
        }
    }

    // 组件位置
    public static int InitialX { get => -width / 2 + 110; }
    public static int InitialY { get => height / 2 - 85; }

    public static int ElementX = InitialX;
    public static int ElementY = InitialY;

    #endregion

    public MainWindow()
    {
        Debug.Log("MainWindow");

        instance = this;
        instance.Init();
    }

    private void Init()
    {
        if (initialized)
        {
            return;
        }

        CreateUI();

        canvas.SetActive(OptionToggle);
        initialized = true;
    }


    #region[创建UI]
    public void CreateUI()
    {
        if (canvas == null)
        {
            Debug.Log("开始创建UI");

            canvas = UIControls.createUICanvas();
            Object.DontDestroyOnLoad(canvas);
            canvas.name = "ScriptTrainer";


            // 创建背景              
            GameObject background = UIControls.createUIPanel(canvas, (height + 40).ToString(), (width + 40).ToString(), null);
            background.gameObject.name = "background";
            background.gameObject.layer = 5;   // UI层

            // 将面板添加到画布, 请参阅 createUIPanel 了解我们将高度/宽度作为字符串传递的原因
            uiPanel = UIControls.createUIPanel(background, height.ToString(), width.ToString(), null);
            // 设置背景颜色
            uiPanel.GetComponent<Image>().color = UIControls.HTMLString2Color("#424242FF");

            // 设置背景
            background.GetComponent<Image>().color = UIControls.HTMLString2Color("#2D2D30FF");
            // 这就是我们将如何挂钩鼠标事件以进行窗口拖动
            background.AddComponent<WindowDragHandler>();

            #region[创建标题 和 关闭按钮]
            Components.AddTitle("【狂游戏大亨2】内置修改器 By:小莫 2.0.0", background);

            GameObject closeButton = UIControls.createUIButton(background, "#B71C1CFF", "X", () =>
            {
                OptionToggle = false;
                // canvas.SetActive(OptionToggle);
            }, new Vector3(width / 2 + 10, height / 2 + 10, 0));
            closeButton.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 20);
            // 字体颜色为白色
            closeButton.GetComponentInChildren<Text>().color = UIControls.HTMLString2Color("#FFFFFFFF");
            #endregion

            #region[常用功能]
            GameObject BasicScripts = UIControls.createUIPanel(uiPanel, "410", "600", null);
            BasicScripts.GetComponent<Image>().color = UIControls.HTMLString2Color("#424242FF");
            BasicScripts.GetComponent<RectTransform>().anchoredPosition = new Vector2(-70, -20);

            BasicScripts itemWindow = new BasicScripts(BasicScripts);


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
                new Navigation("BasicScripts","常用功能", BasicScripts, true),
                // new Navigation("PlayerScripts","玩家功能", PlayerScripts, false),
                // new Navigation("NpcScripts","Npc功能", NpcScripts, false),
                // new Navigation("ItemScripts","添加物品", ItemScripts, false),
                // new Navigation("MapAreaScripts","地区恩义", MapAreaScripts, false),
                //new Navigation("FeatureScripts", "编辑特性", FeatureScripts, false),
                //new Navigation("ItemScripts", "获取物品", ItemScripts, false),

            };

            UINavigation.Initialize(nav, NavPanel);

            #endregion




            canvas.SetActive(false);

        }
    }

    #endregion




}
