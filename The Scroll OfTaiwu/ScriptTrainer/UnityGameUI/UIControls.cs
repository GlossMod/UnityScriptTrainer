
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace UnityGameUI
{
    // UI 控件
    internal class UIControls : MonoBehaviour
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

        public UIControls()
        {

        }

        #region[元素]

        // 创建根元素
        private static GameObject CreateUIElementRoot(string name, Vector2 size)
        {
            GameObject gameObject = new GameObject(name);
            RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
            rectTransform.sizeDelta = size;
            return gameObject;
        }

        // 创建UI对象
        private static GameObject CreateUIObject(string name, GameObject parent)
        {
            GameObject gameObject = new GameObject(name);
            gameObject.AddComponent<RectTransform>();
            UIControls.SetParentAndAlign(gameObject, parent);
            return gameObject;
        }

        // 设置默认文本
        private static void SetDefaultTextValues(Text lbl)
        {
            lbl.color = UIControls.s_TextColor;
            //lbl.AssignDefaultFont();
            //lbl.FontTextureChanged();
            lbl.font = (Font)UnityEngine.Resources.GetBuiltinResource<Font>("Arial.ttf");
            // 设置字体为宋体
            //lbl.font = (Font)UnityEngine.Resources.GetBuiltinResource<Font>("simkai.ttf");
        }

        // 设置默认颜色过度值
        private static void SetDefaultColorTransitionValues(Selectable slider)
        {
            ColorBlock colors = slider.colors;
            colors.highlightedColor = new Color(0.882f, 0.882f, 0.882f);
            colors.pressedColor = new Color(0.698f, 0.698f, 0.698f);
            colors.disabledColor = new Color(0.521f, 0.521f, 0.521f);
        }

        // 设置父级对其
        private static void SetParentAndAlign(GameObject child, GameObject parent)
        {
            if (!(parent == null))
            {
                child.transform.SetParent(parent.transform, false);
                UIControls.SetLayerRecursively(child, parent.layer);
            }
        }

        // 递归设置层
        private static void SetLayerRecursively(GameObject go, int layer)
        {
            go.layer = layer;
            Transform transform = go.transform;
            for (int i = 0; i < transform.childCount; i++)
            {
                UIControls.SetLayerRecursively(transform.GetChild(i).gameObject, layer);
            }
        }

        // 创建面板
        public static GameObject CreatePanel(UIControls.Resources resources)
        {
            GameObject gameObject = UIControls.CreateUIElementRoot("Panel", UIControls.s_ThickElementSize);
            RectTransform component = gameObject.GetComponent<RectTransform>();
            component.anchorMin = Vector2.zero;
            component.anchorMax = Vector2.one;
            component.anchoredPosition = Vector2.zero;
            component.sizeDelta = Vector2.zero;
            Image image = gameObject.AddComponent<Image>();
            image.sprite = resources.background;
            image.type = Image.Type.Sliced;
            image.color = UIControls.s_PanelColor;
            return gameObject;
        }

        // 创建按钮
        public static GameObject CreateButton(UIControls.Resources resources, string Text)
        {
            GameObject gameObject = UIControls.CreateUIElementRoot("Button", UIControls.s_ThickElementSize);
            GameObject gameObject2 = new GameObject("Text");
            gameObject2.AddComponent<RectTransform>();
            UIControls.SetParentAndAlign(gameObject2, gameObject);
            Image image = gameObject.AddComponent<Image>();
            image.sprite = resources.standard;
            image.type = Image.Type.Sliced;
            image.color = UIControls.s_DefaultSelectableColor;
            Button defaultColorTransitionValues = gameObject.AddComponent<Button>();
            UIControls.SetDefaultColorTransitionValues(defaultColorTransitionValues);

            Text text = gameObject2.AddComponent<Text>();
            text.text = Text;
            text.alignment = TextAnchor.MiddleCenter;

            UIControls.SetDefaultTextValues(text);
            RectTransform component = gameObject2.GetComponent<RectTransform>();
            component.anchorMin = Vector2.zero;
            component.anchorMax = Vector2.one;
            component.sizeDelta = Vector2.zero;

            // 设置按钮宽高
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(80, 40);

            return gameObject;
        }

        // 创建文本
        public static GameObject CreateText(UIControls.Resources resources)
        {
            GameObject gameObject = UIControls.CreateUIElementRoot("Text", UIControls.s_ThickElementSize);
            Text text = gameObject.AddComponent<Text>();
            text.text = "New Text";
            UIControls.SetDefaultTextValues(text);
            return gameObject;
        }

        // 创建图像
        public static GameObject CreateImage(UIControls.Resources resources)
        {
            GameObject gameObject = UIControls.CreateUIElementRoot("Image", UIControls.s_ImageElementSize);
            Image image = gameObject.AddComponent<Image>();
            image.sprite = resources.background;

            return gameObject;
        }

        // 创建原生图像
        public static GameObject CreateRawImage(UIControls.Resources resources)
        {
            GameObject gameObject = UIControls.CreateUIElementRoot("RawImage", UIControls.s_ImageElementSize);
            RawImage rawImage = gameObject.AddComponent<RawImage>();
            rawImage.texture = resources.background.texture;
            return gameObject;
        }

        // 创建滑块
        public static GameObject CreateSlider(UIControls.Resources resources)
        {
            GameObject gameObject = UIControls.CreateUIElementRoot("Slider", UIControls.s_ThinElementSize);
            GameObject gameObject2 = UIControls.CreateUIObject("Background", gameObject);
            GameObject gameObject3 = UIControls.CreateUIObject("Fill Area", gameObject);
            GameObject gameObject4 = UIControls.CreateUIObject("Fill", gameObject3);
            GameObject gameObject5 = UIControls.CreateUIObject("Handle Slide Area", gameObject);
            GameObject gameObject6 = UIControls.CreateUIObject("Handle", gameObject5);
            Image image = gameObject2.AddComponent<Image>();
            image.sprite = resources.background;
            image.type = Image.Type.Sliced;
            image.color = UIControls.s_DefaultSelectableColor;
            RectTransform component = gameObject2.GetComponent<RectTransform>();
            component.anchorMin = new Vector2(0f, 0.25f);
            component.anchorMax = new Vector2(1f, 0.75f);
            component.sizeDelta = new Vector2(0f, 0f);
            RectTransform component2 = gameObject3.GetComponent<RectTransform>();
            component2.anchorMin = new Vector2(0f, 0.25f);
            component2.anchorMax = new Vector2(1f, 0.75f);
            component2.anchoredPosition = new Vector2(-5f, 0f);
            component2.sizeDelta = new Vector2(-20f, 0f);
            Image image2 = gameObject4.AddComponent<Image>();
            image2.sprite = resources.standard;
            image2.type = Image.Type.Sliced;
            image2.color = UIControls.s_DefaultSelectableColor;
            RectTransform component3 = gameObject4.GetComponent<RectTransform>();
            component3.sizeDelta = new Vector2(10f, 0f);
            RectTransform component4 = gameObject5.GetComponent<RectTransform>();
            component4.sizeDelta = new Vector2(-20f, 0f);
            component4.anchorMin = new Vector2(0f, 0f);
            component4.anchorMax = new Vector2(1f, 1f);
            Image image3 = gameObject6.AddComponent<Image>();
            image3.sprite = resources.knob;
            image3.color = UIControls.s_DefaultSelectableColor;
            RectTransform component5 = gameObject6.GetComponent<RectTransform>();
            component5.sizeDelta = new Vector2(20f, 0f);
            Slider slider = gameObject.AddComponent<Slider>();
            slider.fillRect = gameObject4.GetComponent<RectTransform>();
            slider.handleRect = gameObject6.GetComponent<RectTransform>();
            slider.targetGraphic = image3;
            slider.direction = Slider.Direction.LeftToRight;
            UIControls.SetDefaultColorTransitionValues(slider);
            return gameObject;
        }

        // 创建滚动条
        public static GameObject CreateScrollbar(UIControls.Resources resources)
        {
            GameObject gameObject = UIControls.CreateUIElementRoot("Scrollbar", UIControls.s_ThinElementSize);
            GameObject gameObject2 = UIControls.CreateUIObject("Sliding Area", gameObject);
            GameObject gameObject3 = UIControls.CreateUIObject("Handle", gameObject2);
            Image image = gameObject.AddComponent<Image>();
            image.sprite = resources.background;
            image.type = Image.Type.Sliced;
            image.color = UIControls.s_DefaultSelectableColor;
            Image image2 = gameObject3.AddComponent<Image>();
            image2.sprite = resources.standard;
            image2.type = Image.Type.Sliced;
            image2.color = UIControls.s_DefaultSelectableColor;
            RectTransform component = gameObject2.GetComponent<RectTransform>();
            component.sizeDelta = new Vector2(-20f, -20f);
            component.anchorMin = Vector2.zero;
            component.anchorMax = Vector2.one;
            RectTransform component2 = gameObject3.GetComponent<RectTransform>();
            component2.sizeDelta = new Vector2(20f, 20f);
            Scrollbar scrollbar = gameObject.AddComponent<Scrollbar>();
            scrollbar.handleRect = component2;
            scrollbar.targetGraphic = image2;
            UIControls.SetDefaultColorTransitionValues(scrollbar);
            return gameObject;
        }

        // 创建切换
        public static GameObject CreateToggle(UIControls.Resources resources)
        {
            GameObject gameObject = UIControls.CreateUIElementRoot("Toggle", UIControls.s_ThinElementSize);
            GameObject gameObject2 = UIControls.CreateUIObject("Background", gameObject);
            GameObject gameObject3 = UIControls.CreateUIObject("Checkmark", gameObject2);
            GameObject gameObject4 = UIControls.CreateUIObject("Label", gameObject);
            Toggle toggle = gameObject.AddComponent<Toggle>();
            toggle.isOn = true;
            Image image = gameObject2.AddComponent<Image>();
            image.sprite = resources.standard;
            image.type = Image.Type.Sliced;
            image.color = UIControls.s_DefaultSelectableColor;
            Image image2 = gameObject3.AddComponent<Image>();
            image2.sprite = resources.checkmark;
            Text text = gameObject4.AddComponent<Text>();
            text.text = "Toggle";
            UIControls.SetDefaultTextValues(text);
            toggle.graphic = image2;
            toggle.targetGraphic = image;
            UIControls.SetDefaultColorTransitionValues(toggle);
            RectTransform component = gameObject2.GetComponent<RectTransform>();
            component.anchorMin = new Vector2(0f, 1f);
            component.anchorMax = new Vector2(0f, 1f);
            component.anchoredPosition = new Vector2(10f, -10f);
            component.sizeDelta = new Vector2(20f, 20f);
            RectTransform component2 = gameObject3.GetComponent<RectTransform>();
            component2.anchorMin = new Vector2(0.5f, 0.5f);
            component2.anchorMax = new Vector2(0.5f, 0.5f);
            component2.anchoredPosition = Vector2.zero;
            component2.sizeDelta = new Vector2(20f, 20f);
            RectTransform component3 = gameObject4.GetComponent<RectTransform>();
            component3.anchorMin = new Vector2(0f, 0f);
            component3.anchorMax = new Vector2(1f, 1f);
            component3.offsetMin = new Vector2(23f, 1f);
            component3.offsetMax = new Vector2(-5f, -2f);
            return gameObject;
        }

        // 创建输入框
        public static GameObject CreateInputField(UIControls.Resources resources)
        {
            GameObject gameObject = UIControls.CreateUIElementRoot("InputField", UIControls.s_ThickElementSize);
            GameObject gameObject2 = UIControls.CreateUIObject("Placeholder", gameObject);
            GameObject gameObject3 = UIControls.CreateUIObject("Text", gameObject);
            Image image = gameObject.AddComponent<Image>();
            image.sprite = resources.inputField;
            image.type = Image.Type.Sliced;
            image.color = UIControls.s_DefaultSelectableColor;
            InputField inputField = gameObject.AddComponent<InputField>();
            UIControls.SetDefaultColorTransitionValues(inputField);
            Text text = gameObject3.AddComponent<Text>();
            text.text = "";
            text.supportRichText = false;
            UIControls.SetDefaultTextValues(text);
            Text text2 = gameObject2.AddComponent<Text>();
            text2.text = "Enter text...";
            text2.fontStyle = FontStyle.Italic;
            Color color = text.color;
            color.a *= 0.5f;
            text2.color = color;
            RectTransform component = gameObject3.GetComponent<RectTransform>();
            component.anchorMin = Vector2.zero;
            component.anchorMax = Vector2.one;
            component.sizeDelta = Vector2.zero;
            component.offsetMin = new Vector2(10f, 6f);
            component.offsetMax = new Vector2(-10f, -7f);
            RectTransform component2 = gameObject2.GetComponent<RectTransform>();
            component2.anchorMin = Vector2.zero;
            component2.anchorMax = Vector2.one;
            component2.sizeDelta = Vector2.zero;
            component2.offsetMin = new Vector2(10f, 6f);
            component2.offsetMax = new Vector2(-10f, -7f);
            inputField.textComponent = text;
            inputField.placeholder = text2;
            return gameObject;
        }

        // 创建下拉框
        public static GameObject CreateDropdown(UIControls.Resources resources, List<string> options, Color LabelColor)
        {
            GameObject gameObject = UIControls.CreateUIElementRoot("Dropdown", UIControls.s_ThickElementSize);
            GameObject gameObject2 = UIControls.CreateUIObject("Label", gameObject);
            GameObject gameObject3 = UIControls.CreateUIObject("Arrow", gameObject);
            GameObject gameObject4 = UIControls.CreateUIObject("Template", gameObject);
            GameObject gameObject5 = UIControls.CreateUIObject("Viewport", gameObject4);
            GameObject gameObject6 = UIControls.CreateUIObject("Content", gameObject5);
            GameObject gameObject7 = UIControls.CreateUIObject("Item", gameObject6);
            GameObject gameObject8 = UIControls.CreateUIObject("Item Background", gameObject7);
            GameObject gameObject9 = UIControls.CreateUIObject("Item Checkmark", gameObject7);
            GameObject gameObject10 = UIControls.CreateUIObject("Item Label", gameObject7);
            GameObject gameObject11 = UIControls.CreateScrollbar(resources);

            gameObject11.name = "Scrollbar";
            UIControls.SetParentAndAlign(gameObject11, gameObject4);

            Scrollbar component = gameObject11.GetComponent<Scrollbar>();
            component.SetDirection(Scrollbar.Direction.BottomToTop, true);

            RectTransform component2 = gameObject11.GetComponent<RectTransform>();
            component2.anchorMin = Vector2.right;
            component2.anchorMax = Vector2.one;
            component2.pivot = Vector2.one;
            component2.sizeDelta = new Vector2(component2.sizeDelta.x, 0f);

            Text text = gameObject10.AddComponent<Text>();
            UIControls.SetDefaultTextValues(text);
            text.alignment = TextAnchor.MiddleLeft;

            Image image = gameObject8.AddComponent<Image>();
            image.color = new Color32(245, 245, 245, byte.MaxValue);
            Image image2 = gameObject9.AddComponent<Image>();
            image2.sprite = resources.checkmark;
            Toggle toggle = gameObject7.AddComponent<Toggle>();
            toggle.targetGraphic = image;
            toggle.graphic = image2;
            toggle.isOn = true;
            Image image3 = gameObject4.AddComponent<Image>();
            image3.sprite = resources.standard;
            image3.type = Image.Type.Sliced;

            ScrollRect scrollRect = gameObject4.AddComponent<ScrollRect>();

            // These 2 lines were causing the cast error, why did Unity use this here and elsewere the GetComponent()???
            //scrollRect.content = (RectTransform)gameObject6.transform;
            //scrollRect.viewport = (RectTransform)gameObject5.transform;
            scrollRect.content = gameObject6.GetComponent<RectTransform>();
            scrollRect.viewport = gameObject5.GetComponent<RectTransform>();

            scrollRect.horizontal = false;
            scrollRect.movementType = ScrollRect.MovementType.Clamped;
            scrollRect.verticalScrollbar = component;
            scrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;
            scrollRect.verticalScrollbarSpacing = -3f;

            Mask mask = gameObject5.AddComponent<Mask>();
            mask.showMaskGraphic = false;

            Image image4 = gameObject5.AddComponent<Image>();
            image4.sprite = resources.mask;
            image4.type = Image.Type.Sliced;

            Text text2 = gameObject2.AddComponent<Text>();
            UIControls.SetDefaultTextValues(text2);
            text2.alignment = TextAnchor.MiddleLeft;
            // 设置字体颜色
            text2.color = LabelColor;
            // UIControls.HTMLString2Color("#EFEBE9FF");

            Image image5 = gameObject3.AddComponent<Image>();
            image5.sprite = resources.dropdown;
            Image image6 = gameObject.AddComponent<Image>();
            image6.sprite = resources.standard;
            image6.color = UIControls.s_DefaultSelectableColor;
            //image6.color = UIControls.HTMLString2Color("#EFEBE9FF");
            image6.type = Image.Type.Sliced;

            Dropdown dropdown = gameObject.AddComponent<Dropdown>();
            dropdown.targetGraphic = image6;
            UIControls.SetDefaultColorTransitionValues(dropdown);
            dropdown.template = gameObject4.GetComponent<RectTransform>();
            dropdown.captionText = text2;
            dropdown.itemText = text;

            text.text = options[0];
            foreach (var item in options)
            {
                
                dropdown.options.Add(new Dropdown.OptionData
                {
                    text = item
                });
            }
            
            //text.text = "Option A";
            //dropdown.options.Add(new Dropdown.OptionData
            //{
            //    text = "Option A"
            //});
            //dropdown.options.Add(new Dropdown.OptionData
            //{
            //    text = "Option B"
            //});
            //dropdown.options.Add(new Dropdown.OptionData
            //{
            //    text = "Option C"
            //});

            dropdown.RefreshShownValue();

            RectTransform component3 = gameObject2.GetComponent<RectTransform>();
            component3.anchorMin = Vector2.zero;
            component3.anchorMax = Vector2.one;
            component3.offsetMin = new Vector2(10f, 6f);
            component3.offsetMax = new Vector2(-25f, -7f);

            RectTransform component4 = gameObject3.GetComponent<RectTransform>();
            component4.anchorMin = new Vector2(1f, 0.5f);
            component4.anchorMax = new Vector2(1f, 0.5f);
            component4.sizeDelta = new Vector2(20f, 20f);
            component4.anchoredPosition = new Vector2(-15f, 0f);

            RectTransform component5 = gameObject4.GetComponent<RectTransform>();
            component5.anchorMin = new Vector2(0f, 0f);
            component5.anchorMax = new Vector2(1f, 0f);
            component5.pivot = new Vector2(0.5f, 1f);
            component5.anchoredPosition = new Vector2(0f, 2f);
            component5.sizeDelta = new Vector2(0f, 150f);

            RectTransform component6 = gameObject5.GetComponent<RectTransform>();
            component6.anchorMin = new Vector2(0f, 0f);
            component6.anchorMax = new Vector2(1f, 1f);
            component6.sizeDelta = new Vector2(-18f, 0f);
            component6.pivot = new Vector2(0f, 1f);

            RectTransform component7 = gameObject6.GetComponent<RectTransform>();
            component7.anchorMin = new Vector2(0f, 1f);
            component7.anchorMax = new Vector2(1f, 1f);
            component7.pivot = new Vector2(0.5f, 1f);
            component7.anchoredPosition = new Vector2(0f, 0f);
            component7.sizeDelta = new Vector2(0f, 28f);

            RectTransform component8 = gameObject7.GetComponent<RectTransform>();
            component8.anchorMin = new Vector2(0f, 0.5f);
            component8.anchorMax = new Vector2(1f, 0.5f);
            component8.sizeDelta = new Vector2(0f, 20f);

            RectTransform component9 = gameObject8.GetComponent<RectTransform>();
            component9.anchorMin = Vector2.zero;
            component9.anchorMax = Vector2.one;
            component9.sizeDelta = Vector2.zero;

            RectTransform component10 = gameObject9.GetComponent<RectTransform>();
            component10.anchorMin = new Vector2(0f, 0.5f);
            component10.anchorMax = new Vector2(0f, 0.5f);
            component10.sizeDelta = new Vector2(20f, 20f);
            component10.anchoredPosition = new Vector2(10f, 0f);

            RectTransform component11 = gameObject10.GetComponent<RectTransform>();
            component11.anchorMin = Vector2.zero;
            component11.anchorMax = Vector2.one;
            component11.offsetMin = new Vector2(20f, 1f);
            component11.offsetMax = new Vector2(-10f, -2f);

            gameObject4.SetActive(false);

            return gameObject;
        }

        // 创建滚动视图
        public static GameObject CreateScrollView(UIControls.Resources resources)
        {
            GameObject gameObject = UIControls.CreateUIElementRoot("Scroll View", new Vector2(200f, 200f));
            GameObject gameObject2 = UIControls.CreateUIObject("Viewport", gameObject);
            GameObject gameObject3 = UIControls.CreateUIObject("Content", gameObject2);
            GameObject gameObject4 = UIControls.CreateScrollbar(resources);
            gameObject4.name = "Scrollbar Horizontal";
            UIControls.SetParentAndAlign(gameObject4, gameObject);
            RectTransform component = gameObject4.GetComponent<RectTransform>();
            component.anchorMin = Vector2.zero;
            component.anchorMax = Vector2.right;
            component.pivot = Vector2.zero;
            component.sizeDelta = new Vector2(0f, component.sizeDelta.y);
            GameObject gameObject5 = UIControls.CreateScrollbar(resources);
            gameObject5.name = "Scrollbar Vertical";
            UIControls.SetParentAndAlign(gameObject5, gameObject);
            gameObject5.GetComponent<Scrollbar>().SetDirection(Scrollbar.Direction.BottomToTop, true);
            RectTransform component2 = gameObject5.GetComponent<RectTransform>();
            component2.anchorMin = Vector2.right;
            component2.anchorMax = Vector2.one;
            component2.pivot = Vector2.one;
            component2.sizeDelta = new Vector2(component2.sizeDelta.x, 0f);
            RectTransform component3 = gameObject2.GetComponent<RectTransform>();
            component3.anchorMin = Vector2.zero;
            component3.anchorMax = Vector2.one;
            component3.sizeDelta = Vector2.zero;
            component3.pivot = Vector2.up;
            RectTransform component4 = gameObject3.GetComponent<RectTransform>();
            component4.anchorMin = Vector2.up;
            component4.anchorMax = Vector2.one;
            component4.sizeDelta = new Vector2(0f, 300f);
            component4.pivot = Vector2.up;
            ScrollRect scrollRect = gameObject.AddComponent<ScrollRect>();
            scrollRect.content = component4;
            scrollRect.viewport = component3;
            scrollRect.horizontalScrollbar = gameObject4.GetComponent<Scrollbar>();
            scrollRect.verticalScrollbar = gameObject5.GetComponent<Scrollbar>();
            scrollRect.horizontalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;
            scrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;
            scrollRect.horizontalScrollbarSpacing = -3f;
            scrollRect.verticalScrollbarSpacing = -3f;
            Image image = gameObject.AddComponent<Image>();
            image.sprite = resources.background;
            image.type = Image.Type.Sliced;
            image.color = UIControls.s_PanelColor;
            Mask mask = gameObject2.AddComponent<Mask>();
            mask.showMaskGraphic = false;
            Image image2 = gameObject2.AddComponent<Image>();
            image2.sprite = resources.mask;
            image2.type = Image.Type.Sliced;
            return gameObject;
        }
        #endregion

        #region[创建组件]

        // 将 16进制格式颜色转换为Color32
        public static Color32 HTMLString2Color(string htmlcolorstring)
        {
            #region[DevNote]
            // Unity ref: https://docs.unity3d.com/ScriptReference/ColorUtility.TryParseHtmlString.html
            // Note: Color strings can also set alpha: "#7AB900" vs. w/alpha "#7AB90003" 
            //ColorUtility.TryParseHtmlString(htmlcolorstring, out color); // Unity's Method, This may have been stripped
            #endregion

            Color32 color = htmlcolorstring.HexToColor();

            return color;
        }

        public static Texture2D createDefaultTexture(string htmlcolorstring)
        {
            Color32 color = HTMLString2Color(htmlcolorstring);

            // Make a new sprite from a texture
            Texture2D SpriteTexture = new Texture2D(1, 1);
            SpriteTexture.SetPixel(0, 0, color);
            SpriteTexture.Apply();

            return SpriteTexture;
        }

        // 通过文件创建 Texture2D贴图格式
        public static Texture2D createTextureFromFile(string FilePath)
        {
            // Load a PNG or JPG file from disk to a Texture2D
            // 将 PNG 或 JPG 文件从磁盘加载到 Texture2D
            Texture2D Tex2D;
            byte[] FileData;

            if (File.Exists(FilePath))
            {
                FileData = File.ReadAllBytes(FilePath);
                Tex2D = new Texture2D(265, 198);
                Tex2D.LoadRawTextureData(FileData);
                //Tex2D.LoadImage(FileData, false);  // This is Broke. Unhollower/Texture2D doesn't like it...
                Tex2D.Apply();
                return Tex2D;
            }
            return null;
        }

        // 通过纹理贴图创建元素
        public static Sprite createSpriteFrmTexture(Texture2D SpriteTexture)
        {
            // Create a new Sprite from Texture
            Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0), 100.0f, 0, SpriteMeshType.Tight);

            return NewSprite;
        }


        // 创建画布
        public static GameObject createUICanvas()
        {
            Debug.Log("创建画布");

            // Create a new Canvas Object with required components
            GameObject CanvasGO = new GameObject("CanvasGO");
            Object.DontDestroyOnLoad(CanvasGO);

            // 传入 Canvas 类型
            Canvas canvas = CanvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;

            CanvasScaler cs = CanvasGO.AddComponent<CanvasScaler>();
            cs.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
            cs.referencePixelsPerUnit = 100f;
            cs.referenceResolution = new Vector2(1024f, 788f);

            GraphicRaycaster gr = CanvasGO.AddComponent<GraphicRaycaster>();
            ;
            return CanvasGO;
        }

        // 创建UI面板
        public static GameObject createUIPanel(GameObject canvas, string height, string width, Sprite BgSprite = null)
        {
            UIControls.Resources uiResources = new UIControls.Resources();

            uiResources.background = BgSprite;

            //log.LogMessage("   Creating UI Panel");
            Debug.Log("创建UI面板");
            GameObject uiPanel = UIControls.CreatePanel(uiResources);
            uiPanel.transform.SetParent(canvas.transform, false);

            RectTransform rectTransform = uiPanel.GetComponent<RectTransform>();

            float size;
            size = Single.Parse(height); // 它们在 Unhollower 中没有浮动支持，这样可以避免错误
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);
            size = Single.Parse(width);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);
            // 您也可以使用 rectTransform.sizeDelta = new Vector2(width, height);

            return uiPanel;
        }

        // 创建按钮
        public static GameObject createUIButton(GameObject parent, string backgroundColor, string Text, UnityAction action, Vector3 localPosition = new Vector3())
        {
            Debug.Log("创建UI按钮");

            Sprite btnSprite = createSpriteFrmTexture(createDefaultTexture(backgroundColor));
            UIControls.Resources uiResources = new UIControls.Resources();
            uiResources.standard = btnSprite;

            GameObject uiButton = UIControls.CreateButton(uiResources, Text);
            uiButton.transform.SetParent(parent.transform, false);
            // 创建点击事件
            Button btnComp = uiButton.GetComponent<Button>();
            btnComp.onClick.AddListener(action);
            uiButton.GetComponent<RectTransform>().localPosition = localPosition;

            // 鼠标悬停透明度变淡
            ColorBlock colors = btnComp.colors;
            colors.highlightedColor = new Color(1f, 1f, 1f, 0.5f);
            btnComp.colors = colors;


            return uiButton;
        }

        // 创建切换
        public static GameObject createUIToggle(GameObject parent, Sprite BgSprite, Sprite customCheckmarkSprite)
        {
            UIControls.Resources uiResources = new UIControls.Resources();
            uiResources.standard = BgSprite;
            uiResources.checkmark = customCheckmarkSprite;

            Debug.Log("创建UI切换");
            GameObject uiToggle = UIControls.CreateToggle(uiResources);
            uiToggle.transform.SetParent(parent.transform, false);

            return uiToggle;
        }

        // 创建滑块
        public static GameObject createUISlider(GameObject parent, Sprite BgSprite, Sprite FillSprite, Sprite KnobSprite)
        {
            UIControls.Resources uiResources = new UIControls.Resources();
            uiResources.background = BgSprite;
            uiResources.standard = FillSprite;
            uiResources.knob = KnobSprite;

            Debug.Log("创建滑块");
            GameObject uiSlider = UIControls.CreateSlider(uiResources);
            uiSlider.transform.SetParent(parent.transform, false);

            return uiSlider;
        }

        // 创建输入框
        public static GameObject createUIInputField(GameObject parent, Sprite BgSprite, string textColor)
        {
            UIControls.Resources uiResources = new UIControls.Resources();
            uiResources.inputField = BgSprite;

            Debug.Log("创建UI输入框");
            GameObject uiInputField = UIControls.CreateInputField(uiResources);
            uiInputField.transform.SetParent(parent.transform, false);

            var textComps = uiInputField.GetComponentsInChildren<Text>();
            foreach (var text in textComps)
            {
                text.color = HTMLString2Color(textColor);
            }
            return uiInputField;
        }

        // 创建下拉菜单
        public static GameObject createUIDropDown(GameObject parent, Sprite BgSprite, Sprite ScrollbarSprite, Sprite DropDownSprite, Sprite CheckmarkSprite, Sprite customMaskSprite, List<string> options, Color LabelColor)
        {
            UIControls.Resources uiResources = new UIControls.Resources();
            uiResources.standard = BgSprite;    // 设置背景和处理图像
            uiResources.background = ScrollbarSprite;    // 设置滚动条背景图像
            uiResources.dropdown = DropDownSprite;  // 设置下拉句柄图像
            uiResources.checkmark = CheckmarkSprite;    // 设置复选标记图像
            uiResources.mask = customMaskSprite;     // 设置视口蒙版

            Debug.Log("创建 UI 下拉菜单");
            var uiDropdown = UIControls.CreateDropdown(uiResources, options, LabelColor);
            uiDropdown.transform.SetParent(parent.transform, false);

            return uiDropdown;
        }

        // 创建图片
        public static GameObject createUIImage(GameObject parent, Sprite BgSprite)
        {
            UIControls.Resources uiResources = new UIControls.Resources();
            uiResources.background = BgSprite;

            Debug.Log("创建图片");
            GameObject uiImage = UIControls.CreateImage(uiResources);
            uiImage.transform.SetParent(parent.transform, false);

            return uiImage;
        }

        // 创建原始图片
        public static GameObject createUIRawImage(GameObject parent, Sprite BgSprite)
        {
            UIControls.Resources uiResources = new UIControls.Resources();
            uiResources.background = BgSprite;

            Debug.Log("创建原始图片");
            GameObject uiRawImage = UIControls.CreateRawImage(uiResources);
            uiRawImage.transform.SetParent(parent.transform, false);

            return uiRawImage;
        }

        // 创建滚动条
        public static GameObject createUIScrollbar(GameObject parent, Sprite ScrollbarSprite)
        {
            UIControls.Resources uiResources = new UIControls.Resources();
            uiResources.background = ScrollbarSprite;

            //log.LogMessage("   Creating UI Scrollbar");
            Debug.Log("创建滚动条");
            GameObject uiScrollbar = UIControls.CreateScrollbar(uiResources);
            uiScrollbar.transform.SetParent(parent.transform, false);

            return uiScrollbar;
        }

        // 创建滚动视图
        public static GameObject createUIScrollView(GameObject parent, Sprite BgSprite, Sprite customMaskSprite, Sprite customScrollbarSprite)
        {
            UIControls.Resources uiResources = new UIControls.Resources();
            uiResources.background = BgSprite;
            uiResources.knob = BgSprite;

            uiResources.standard = customScrollbarSprite;
            uiResources.mask = customMaskSprite;

            //log.LogMessage("   Creating UI ScrollView");
            Debug.Log("创建滚动视图");
            GameObject uiScrollView = UIControls.CreateScrollView(uiResources);
            uiScrollView.transform.SetParent(parent.transform, false);

            return uiScrollView;
        }

        // 创建文本
        public static GameObject createUIText(GameObject parent, Sprite BgSprite, string textColor = null )
        {
            UIControls.Resources uiResources = new UIControls.Resources();
            uiResources.background = BgSprite;

            Debug.Log("创建文本");
            GameObject uiText = UIControls.CreateText(uiResources);
            uiText.transform.SetParent(parent.transform, false);

            //uiText.transform.GetChild(0).GetComponent<Text>().font = (Font)Resources.GetBuiltinResource<Font>("Arial.ttf"); // 设置字体
            if (textColor != null) uiText.GetComponent<Text>().color = HTMLString2Color(textColor);
            return uiText;
        }

        #endregion
    }
}
