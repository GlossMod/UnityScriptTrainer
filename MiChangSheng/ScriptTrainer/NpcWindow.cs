
using GUIPackage;
using JSONClass;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityGameUI;
using Object = UnityEngine.Object;

namespace ScriptTrainer
{
    public class NpcWindow
    {
        public static UINPCData npc;            // 获取Npc
        private static GameObject Panel;
        private static GameObject NpcPanel;        
        private static int initialX;
        private static int initialY;
        private static int elementX;
        private static int elementY;

        public NpcWindow(GameObject panel, int x, int y)
        {
            npc = UINPCJiaoHu.Inst.NowJiaoHuNPC;
            Panel = panel;
            initialX = elementX = x;
            initialY = elementY = y;
            Initialize();
        }
        
        public static void Initialize()
        {
            //Object.Destroy(Panel);
            
            ResetCoordinates(true);
            RefreshNpcData();
            
        }

        public static void RefreshNpcData()
        {
            ResetCoordinates(true, true);
            Object.Destroy(NpcPanel);
            NpcPanel = UIControls.createUIPanel(Panel, "410", "600", null);
            NpcPanel.GetComponent<Image>().color = UIControls.HTMLString2Color("#424242FF");

            npc = UINPCJiaoHu.Inst.NowJiaoHuNPC;
            if (npc == null)
            {
                Sprite txtBgSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#7AB900FF"));
                GameObject uiText = UIControls.createUIText(NpcPanel, txtBgSprite, "#FFFFFFFF");
                uiText.GetComponent<Text>().text = "未选择或未找到NPC";
                uiText.GetComponent<RectTransform>().localPosition = new Vector3(elementX, elementY, 0);

                // 设置字体样式为h3小标题
                uiText.GetComponent<Text>().fontSize = 14;
                uiText.GetComponent<Text>().fontStyle = FontStyle.Bold;

                return;
            }

            
            AddH3("Npc资料", NpcPanel);
            {
                AddLabel($"名称：{npc.Name}", NpcPanel);
                AddLabel($"年龄：{npc.Age}", NpcPanel);
                AddLabel($"寿元：{npc.ShouYuan}", NpcPanel);
                AddLabel($"气血：{npc.HP}", NpcPanel);
                AddLabel($"资质：{npc.ZiZhi}", NpcPanel);                
            }
            hr(10);
            {
                AddLabel($"悟性：{npc.WuXing}", NpcPanel);
                AddLabel($"遁速：{npc.DunSu}", NpcPanel);
                AddLabel($"神识：{npc.ShenShi}", NpcPanel);
                AddLabel($"性格：{npc.XingGe}", NpcPanel);
            }
            hr(10);
            {
                // 修改修为
                {
                    List<string> options = new List<string> {
                        "","炼气初期", "炼气中期","炼气后期",
                        "固基初期","固基中期","固基后期",
                        "结丹初期","结丹中期","结丹后期",
                        "化婴初期","化婴中期","化婴后期",
                        "化神初期","化神中期","化神后期",
                    };
                    AddDropdown("修改修为", 150, options, NpcPanel, (int level) =>
                    {
                        if (level > 0)
                        {
                            npc.Level = level;

                            NPCFactory npcf = new NPCFactory();
                            npcf.SetNpcLevel(npc.ID, level);
                            Debug.Log($"已修改{npc.Name}的修为为{options[level]}");
                        }
                    });
                }

                AddButton("添加好感度", Panel, () =>
                {
                    UIWindows.SpawnInputDialog($"您想与{npc.Name}添加多少好感度", "添加", "100", (string count) =>
                    {
                        NPCEx.AddFavor(npc.ID, count.ConvertToIntDef(100));
                    });
                });
                AddButton("添加情分", Panel, () =>
                {
                    UIWindows.SpawnInputDialog($"您想与{npc.Name}添加多少情分", "添加", "100", (string count) =>
                    {
                        NPCEx.AddQingFen(npc.ID, count.ConvertToIntDef(100), true);
                    });
                });
                AddButton("设为道侣", Panel, () =>
                {
                    MainWindow.player.DaoLvId.Add(npc.ID);
                    Debug.Log($"你已和{npc.Name}成为道侣");
                });
            }
        }

        #region[Npc操作]
        
        private static void ChangeNpcAge(int NpcID, int age)
        {

            //NPCFactory npcf = new NPCFactory();
            //npcf.SetNpcLevel(npc.ID, level);

            //UINPCData npc = new UINPCData(NpcID);
            //npc.Age = age;
        }
        #endregion

        #region[添加组件]
        // 添加按钮
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

        // 添加文本
        public static GameObject AddLabel(string text, GameObject panel)
        {
            elementX += 40;

            Sprite txtBgSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#7AB900FF"));
            GameObject uiText = UIControls.createUIText(panel, txtBgSprite, "#FFFFFFFF");
            uiText.GetComponent<Text>().text = text;
            uiText.GetComponent<RectTransform>().localPosition = new Vector3(elementX, elementY, 0);
            uiText.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

            elementX += 70;

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
