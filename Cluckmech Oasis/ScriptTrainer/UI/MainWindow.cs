using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityGameUI;
using Navigation = UnityGameUI.Navigation;

namespace ScriptTrainer;

internal class MainWindow : MonoBehaviour
{
    #region[声明]
    public static GameObject obj = null;
    public static MainWindow instance;
    public static bool initialized = false;
    public static bool _optionToggle = false;
    // private static TooltipGUI toolTipComp = null;

    // UI
    public static GameObject canvas = null;
    private static bool isVisible = false;
    private static GameObject uiPanel = null;
    public static readonly int width = Mathf.Min(Screen.width, 740);
    public static readonly int height = (Screen.height < 400) ? Screen.height : (450);

    // 窗口开关
    public static bool optionToggle
    {
        get => _optionToggle;
        set
        {
            _optionToggle = value;

            // player = PlayerAnimControl.instance;   // 获取玩家
            // NpcWindow.RefreshNpcData();    // 刷新获取Npc

            if (!initialized)
            {
                instance.CreateUI();
            }
        }
    }

    // 按钮位置
    public static int InitialX
    {
        get
        {
            return -width / 2 + 120;
        }
    }
    public static int InitialY
    {
        get
        {
            return height / 2 - 60;
        }
    }

    public static int ElementX = InitialX;
    public static int ElementY = InitialY;

    #endregion

    #region[初始化]
    // public static PlayerAnimControl player
    // {
    //     get
    //     {
    //         return PlayerAnimControl.instance;
    //     }
    // }
    #endregion

    public MainWindow()
    {
        instance = this;
    }

    public static void Initialize()
    {
        initialized = true;
        instance.CreateUI();
    }

    #region [创建UI]

    private void CreateUI()
    {

        if (canvas == null)
        {
            Debug.Log("创建 UI 元素");
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



            #region[面板元素]
            Components.AddTitle("【鸡械绿洲】内置修改器 By:小莫", background);
            GameObject closeButton = UIControls.createUIButton(background, "#B71C1CFF", "X", () =>
                {
                    optionToggle = false;
                    canvas.SetActive(optionToggle);
                }, new Vector3(width / 2 + 10, height / 2 + 10, 0));
            closeButton.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 20);
            // 字体颜色为白色
            closeButton.GetComponentInChildren<Text>().color = UIControls.HTMLString2Color("#FFFFFFFF");
            #endregion

            #region [基础功能]
            GameObject basicScripts = UIControls.createUIPanel(background, "410", "600", null);
            basicScripts.GetComponent<Image>().color = UIControls.HTMLString2Color("#424242FF");
            basicScripts.GetComponent<RectTransform>().anchoredPosition = new Vector2(-70, -20);
            new BasicScripts(basicScripts);

            Components.ResetCoordinates(true, true);

            GameObject settingsScripts = UIControls.createUIPanel(background, "410", "600", null);
            settingsScripts.GetComponent<Image>().color = UIControls.HTMLString2Color("#424242FF");
            settingsScripts.GetComponent<RectTransform>().anchoredPosition = new Vector2(-70, -20);
            new SettingsScripts(settingsScripts);

            // Components.ResetCoordinates(true, true);

            // GameObject weaponScripts = UIControls.createUIPanel(background, "410", "600", null);
            // weaponScripts.GetComponent<Image>().color = UIControls.HTMLString2Color("#424242FF");
            // weaponScripts.GetComponent<RectTransform>().anchoredPosition = new Vector2(-70, -20);
            // // new WeaponScript(weaponScripts);

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
                    new Navigation("BasicScripts","基础功能", basicScripts, true),
                    new Navigation("ItemScripts","游戏设置", settingsScripts, false),
                    // new Navigation("ItemScripts","获取武器", weaponScripts, false),
                    // new Navigation("MenPaiScripts","宗门修改", MenPaiScripts, false),
                    // new Navigation("WuDaoScripts", "悟道修改", WuDaoScripts, false),
                    // new Navigation("NpcScripts", "NPC修改", NpcScripts, false),
                    // new Navigation("ItemScripts", "获取物品", ItemScripts, false),

            };

            UINavigation.Initialize(nav, NavPanel);

            #endregion

            isVisible = true;
            canvas.SetActive(optionToggle);
            Debug.Log("修改器初始化完成!");
            Debug.Log("按Home可开关修改器菜单");

        }
        else
        {
            // Debug.Log("玩家未加载!");
            initialized = false;
        }

    }

    #endregion



}