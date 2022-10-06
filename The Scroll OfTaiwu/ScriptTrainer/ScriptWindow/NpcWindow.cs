using System;
using Config;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityGameUI;
using Object = UnityEngine.Object;

namespace ScriptTrainer
{
    internal class NpcWindow
    {
        #region[全局参数]
        private static GameObject Panel;
        private static GameObject NpcPanel;
        private static int initialX;
        private static int initialY;
        private static int elementX;
        private static int elementY;

        private static int type = 0;
        private static int page = 1;
        private static int conunt = 15;
        // Config.MapArea.Instance.Count / conunt 向下取整

      
        private static GameObject ItemPanel;
        private static List<GameObject> ItemButtons = new List<GameObject>();

        #endregion
        public NpcWindow(GameObject panel, int x, int y)
        {
            Panel = panel;
            initialX = elementX = x ;
            initialY = elementY = y;
            Initialize();
        }

        public static void Initialize()
        {
            ResetCoordinates(true, true);
            Object.Destroy(NpcPanel);
            NpcPanel = UIControls.createUIPanel(Panel, "410", "600", null);
            NpcPanel.GetComponent<Image>().color = UIControls.HTMLString2Color("#424242FF");

            // Scripts.CurCharacterId
            AddH3($"当前选择角色：{Scripts.CurCharacterId}", NpcPanel);
            if (Scripts.CurCharacterId == -1)
            {
                AddH3("未选择角色", NpcPanel);
                return;
            }
            AddH3("基础功能", NpcPanel);
            {
                AddButton("修改年龄", NpcPanel, () =>
                {
                    Scripts.ChangeAge(Scripts.CurCharacterId);
                });
                AddButton("修改血量", NpcPanel, () =>
                {
                    Scripts.ChangeHp(Scripts.CurCharacterId);
                });
                AddButton("好感度", NpcPanel, () =>
                {
                    Scripts.ChangeFavor(Scripts.CurCharacterId, Scripts.playerId);
                });
                AddButton("编辑心情", NpcPanel, () =>
                {
                    UIWindows.SpawnInputDialog($"您想将{Scripts.CurCharacterId}的心情改为多少？", "修改", "100", (string count) =>
                    {
                        GMFunc.EditHappiness(Scripts.CurCharacterId, count.ConvertToIntDef(100));
                    });
                });
                AddButton("设置关系", NpcPanel, () =>
                {
                    Scripts.Relationship(Scripts.playerId, Scripts.CurCharacterId);
                });
                AddButton("可为同道", NpcPanel, () =>
                {
                    GMFunc.SetCharacterOrganization(Scripts.CurCharacterId, 16);
                });
               
                AddButton("解锁技艺", NpcPanel, () =>
                {
                    Scripts.UnlockAllSkills(Scripts.CurCharacterId);
                });
                hr(10);
                AddButton("绑架他/她", NpcPanel, () =>
                {
                    Scripts.Kidnap(Scripts.CurCharacterId);
                });
                AddButton("共度春宵", NpcPanel, () =>
                {
                    GMFunc.MakeCharacterHaveSex(Scripts.playerId, Scripts.CurCharacterId, false, 3);
                });
                AddButton("出手重伤", NpcPanel, () =>
                {
                    for (int i = 0; i <= 6; i++)
                    {
                        Scripts.ChangeInjury(true, (sbyte)i, 6, Scripts.CurCharacterId);
                        Scripts.ChangeInjury(false, (sbyte)i, 6, Scripts.CurCharacterId);
                    }
                    for (int i = 0; i <= 5; i++)
                    {
                        Scripts.ChangePoisoned((sbyte)i, 1000, Scripts.CurCharacterId);
                    }
                });
                AddButton("治病解毒", NpcPanel, () =>
                {
                    for (int i = 0; i <= 6; i++)
                    {
                        Scripts.ChangeInjury(true, (sbyte)i, -6, Scripts.CurCharacterId);
                        Scripts.ChangeInjury(false, (sbyte)i, -6, Scripts.CurCharacterId);
                    }
                    for (int i = 0; i <= 5; i++)
                    {
                        Scripts.ChangePoisoned((sbyte)i, -1000, Scripts.CurCharacterId);
                    }
                });
                              
                //AddButton("设为双性恋", NpcPanel, () =>
                //{
                //    GMFunc.EditBisexual(Scripts.CurCharacterId, true);
                //});

            }
            hr();
            AddH3("基础属性", NpcPanel);
            {
                short[] attributes = new short[6] { 100, 100, 100, 100, 100, 100 };
                AddInputField("脊力", 150, attributes[0].ToString(), NpcPanel, (string text) =>
                {
                    attributes[0] = (short)text.ConvertToIntDef(100);
                    Scripts.ChangeMainAttributes(attributes, Scripts.CurCharacterId);
                });
                AddInputField("灵敏", 150, attributes[1].ToString(), NpcPanel, (string text) =>
                {
                    attributes[1] = (short)text.ConvertToIntDef(100);
                    Scripts.ChangeMainAttributes(attributes, Scripts.CurCharacterId);
                });
                AddInputField("定力", 150, attributes[2].ToString(), NpcPanel, (string text) =>
                {
                    attributes[2] = (short)text.ConvertToIntDef(100);
                    Scripts.ChangeMainAttributes(attributes, Scripts.CurCharacterId);
                });
                AddInputField("体质", 150, attributes[3].ToString(), NpcPanel, (string text) =>
                {
                    attributes[3] = (short)text.ConvertToIntDef(100);
                    Scripts.ChangeMainAttributes(attributes, Scripts.CurCharacterId);
                });
                hr(10);
                AddInputField("根骨", 150, attributes[4].ToString(), NpcPanel, (string text) =>
                {
                    attributes[4] = (short)text.ConvertToIntDef(100);
                    Scripts.ChangeMainAttributes(attributes, Scripts.CurCharacterId);
                });
                AddInputField("悟性", 150, attributes[5].ToString(), NpcPanel, (string text) =>
                {
                    attributes[5] = (short)text.ConvertToIntDef(100);
                    Scripts.ChangeMainAttributes(attributes, Scripts.CurCharacterId);
                });
            }
        }


        #region[添加组件]

       

        public static GameObject AddButton(string Text, GameObject panel, UnityAction action)
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
        public static GameObject AddToggle(string Text, int width, GameObject panel, UnityAction<bool> action)
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
        public static GameObject AddInputField(string Text, int width, string defaultText, GameObject panel, UnityAction<string> action)
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
        public static GameObject AddDropdown(string Text, int width, List<string> options, GameObject panel, UnityAction<int> action)
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
        public static GameObject AddH3(string text, GameObject panel)
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
        public static void hr(int offsetX = 0, int offsetY = 0)
        {
            ResetCoordinates(true);
            elementX += offsetX;
            elementY -= 50 + offsetY;

        }
        // 重置坐标
        public static void ResetCoordinates(bool x, bool y = false)
        {
            if (x) elementX = initialX;
            if (y) elementY = initialY;
        }

        #endregion

    }
}
