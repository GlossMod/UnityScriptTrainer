using BepInEx.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityGameUI;

namespace ScriptTrainer
{
    public class RecipeWindow
    {
        private static GameObject recipeWindow;

        private static RecipeProto[] RecipeBak = LDB.recipes.dataArray;

        private static Dictionary<int, int> RecipeID_SpeedDic = new Dictionary<int, int>();


        // 组件位置
        private static int initialX
        {
            get => -MainWindow.width / 2 + 100 + 10;
        }

        private static int initialY
        {
            get => MainWindow.height / 2 - 85 - 10;
        }

        private static int elementX = initialX;
        private static int elementY = initialY;

        private static PlanetFactory factory
        {
            get { return GameMain.data.mainPlayer.factory; }
        }

        private static FactorySystem factorySystem
        {
            get { return factory.factorySystem; }
        }

        public RecipeWindow(GameObject canvas)
        {
            recipeWindow = UIControls.createUIPanel(canvas, "330", "630");
            recipeWindow.GetComponent<Image>().color = UIControls.HTMLString2Color("#00000000");
            recipeWindow.name = "recipeWindow";
            AddRecipe(canvas);
        }

        public static void AddRecipe(GameObject canvas)
        {
            // GameObject RecipeScripts = UIControls.createUIPanel(canvas, "630", "630", null);
            var scrollView = UIControls.createUIScrollView(canvas, null, null, null);

            scrollView.GetComponent<Image>().color = UIControls.HTMLString2Color("#00000033");
            scrollView.name = "RecipeScripts";
            var rt = scrollView.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(630, 330);
            var scrollbar = scrollView.GetComponent<Scrollbar>();
            // scrollbar.onValueChanged.AddListener(value =>
            // {
            //     float contentHeight = rt.rect.height;
            //     float panelHeight = scrollView.rect.height;
            //     float scrollableHeight = contentHeight - panelHeight;
            //     float newY = value * scrollableHeight;
            //     // 设置Content的新位置
            //     rt.anchoredPosition = new Vector2(contentStartPosition.x, newY);
            // } );


            #region[旧内容]
            //AddCount(scrollView, "组装机输出倍率", ScriptTrainer.AssemblerOutRate.Value, rate =>
            //{
            //    SetOUTRate(rate, ERecipeType.Assemble);
            //    ScriptTrainer.AssemblerOutRate.SetSerializedValue(rate.ToString());
            //});

            //AddCount(scrollView, "组装机速度倍率", ScriptTrainer.AssemblerSpeed.Value, rate =>
            //{
            //    SetSpeedRate(rate, ERecipeType.Assemble);
            //    ScriptTrainer.AssemblerSpeed.SetSerializedValue(rate.ToString());
            //});

            //AddCount(scrollView, "熔炉输出倍率", ScriptTrainer.SmelterOutRate.Value, rate =>
            //{
            //    SetOUTRate(rate, ERecipeType.Smelt);
            //    ScriptTrainer.SmelterOutRate.SetSerializedValue(rate.ToString());
            //});
            //hr();
            //AddCount(scrollView, "熔炉速度倍率", ScriptTrainer.SmelterSpeed.Value, rate =>
            //{
            //    SetSpeedRate(rate, ERecipeType.Smelt);
            //    ScriptTrainer.SmelterSpeed.SetSerializedValue(rate.ToString());
            //});
            //hr();
            //AddCount(scrollView, "化工厂输出倍率", ScriptTrainer.ChemicalPlantOutRate.Value, rate =>
            //{
            //    SetOUTRate(rate, ERecipeType.Chemical);
            //    ScriptTrainer.ChemicalPlantOutRate.SetSerializedValue(rate.ToString());
            //});
            //AddCount(scrollView, "化工厂速度倍率", ScriptTrainer.ChemicalPlantSpeed.Value, rate =>
            //{
            //    SetSpeedRate(rate, ERecipeType.Chemical);
            //    ScriptTrainer.ChemicalPlantSpeed.SetSerializedValue(rate.ToString());
            //});
            //hr();
            //AddCount(scrollView, "粒子对撞机输出倍率", ScriptTrainer.ParticleColliderOutRate.Value, rate =>
            //{
            //    SetOUTRate(rate, ERecipeType.Particle);
            //    ScriptTrainer.ParticleColliderOutRate.SetSerializedValue(rate.ToString());
            //});
            //AddCount(scrollView, "粒子对撞机速度倍率", ScriptTrainer.ParticleColliderSpeed.Value, rate =>
            //{
            //    SetSpeedRate(rate, ERecipeType.Particle);
            //    ScriptTrainer.ParticleColliderSpeed.SetSerializedValue(rate.ToString());
            //});
            //hr();
            //AddCount(scrollView, "研究所输出倍率", ScriptTrainer.ResearchOutRate.Value, rate =>
            //{
            //    SetOUTRate(rate, ERecipeType.Research);
            //    ScriptTrainer.ResearchOutRate.SetSerializedValue(rate.ToString());
            //});
            //AddCount(scrollView, "研究所速度倍率", ScriptTrainer.ResearchSpeed.Value, rate =>
            //{
            //    SetSpeedRate(rate, ERecipeType.Research);
            //    ScriptTrainer.ResearchSpeed.SetSerializedValue(rate.ToString());
            //});
            //hr();
            //AddCount(scrollView, "精炼厂输出倍率", ScriptTrainer.RefineryOutRate.Value, rate =>
            //{
            //    SetOUTRate(rate, ERecipeType.Refine);
            //    ScriptTrainer.RefineryOutRate.SetSerializedValue(rate.ToString());
            //});
            //AddCount(scrollView, "精炼厂速度倍率", ScriptTrainer.RefinerySpeed.Value, rate =>
            //{
            //    SetSpeedRate(rate, ERecipeType.Refine);
            //    ScriptTrainer.RefinerySpeed.SetSerializedValue(rate.ToString());
            //});
            //hr();
            //AddCount(scrollView, "交换站输出倍率", ScriptTrainer.ExchangeOutRate.Value, rate =>
            //{
            //    SetOUTRate(rate, ERecipeType.Exchange);
            //    ScriptTrainer.ExchangeOutRate.SetSerializedValue(rate.ToString());
            //});
            //AddCount(scrollView, "交换站速度倍率", ScriptTrainer.ExchangeSpeed.Value, rate =>
            //{
            //    SetSpeedRate(rate, ERecipeType.Exchange);
            //    ScriptTrainer.ExchangeSpeed.SetSerializedValue(rate.ToString());
            //});
            //hr();
            //AddCount(scrollView, "分馏塔输出倍率", ScriptTrainer.FractionateOutRate.Value, rate =>
            //{
            //    SetOUTRate(rate, ERecipeType.Fractionate);
            //    ScriptTrainer.FractionateOutRate.SetSerializedValue(rate.ToString());
            //});
            //AddCount(scrollView, "分馏塔速度倍率", ScriptTrainer.FractionateSpeed.Value, rate =>
            //{
            //    SetSpeedRate(rate, ERecipeType.Fractionate);
            //    ScriptTrainer.FractionateSpeed.SetSerializedValue(rate.ToString());
            //});
            //hr();
            //AddCount(scrollView, "光子储存环输出倍率", ScriptTrainer.PhotonStoreOutRate.Value, rate =>
            //{
            //    SetOUTRate(rate, ERecipeType.PhotonStore);
            //    ScriptTrainer.PhotonStoreOutRate.SetSerializedValue(rate.ToString());
            //});
            //AddCount(scrollView, "光子储存环速度倍率", ScriptTrainer.PhotonStoreSpeed.Value, rate =>
            //{
            //    SetSpeedRate(rate, ERecipeType.PhotonStore);
            //    ScriptTrainer.PhotonStoreSpeed.SetSerializedValue(rate.ToString());
            //});
            #endregion


            List<List<object>> recipe = new List<List<object>>() {
                new List<object>() {
                    "组装输出倍率" , ScriptTrainer.AssemblerOutRate, (UnityAction<int>)(rate =>
                    {
                        SetOUTRate(rate, ERecipeType.Assemble);
                        ScriptTrainer.AssemblerOutRate.SetSerializedValue(rate.ToString());
                    })
                },
                new List<object>()
                {
                    "组装速度倍率", ScriptTrainer.AssemblerSpeed, (UnityAction<int>)( rate =>
                    {
                        SetSpeedRate(rate, ERecipeType.Assemble);
                        ScriptTrainer.AssemblerSpeed.SetSerializedValue(rate.ToString());
                    })
                },
                new List<object>()
                {
                    "熔炉输出倍率", ScriptTrainer.SmelterOutRate, (UnityAction<int>)( rate =>
                    {
                        SetOUTRate(rate, ERecipeType.Smelt);
                        ScriptTrainer.SmelterOutRate.SetSerializedValue(rate.ToString());
                    })
                },
                new List<object>()
                {
                    "熔炉速度倍率", ScriptTrainer.SmelterSpeed, (UnityAction<int>)( rate =>
                    {
                        SetSpeedRate(rate, ERecipeType.Smelt);
                        ScriptTrainer.SmelterSpeed.SetSerializedValue(rate.ToString());
                    })
                },
                new List<object>()
                {
                    "工厂输出倍率", ScriptTrainer.ChemicalPlantOutRate, (UnityAction<int>)( rate =>
                    {
                        SetOUTRate(rate, ERecipeType.Chemical);
                        ScriptTrainer.ChemicalPlantOutRate.SetSerializedValue(rate.ToString());
                    })
                },
                new List<object>()
                {
                    "工厂速度倍率", ScriptTrainer.ChemicalPlantSpeed, (UnityAction<int>)( rate =>
                    {
                        SetSpeedRate(rate, ERecipeType.Chemical);
                        ScriptTrainer.ChemicalPlantSpeed.SetSerializedValue(rate.ToString());
                    })
                },
                new List<object>()
                {
                    "对撞输出倍率", ScriptTrainer.ParticleColliderOutRate, (UnityAction<int>)( rate =>
                    {
                        SetOUTRate(rate, ERecipeType.Particle);
                        ScriptTrainer.ParticleColliderOutRate.SetSerializedValue(rate.ToString());
                    })
                },
                new List<object>()
                {
                    "对撞速度倍率", ScriptTrainer.ParticleColliderSpeed, (UnityAction<int>)( rate =>
                    {
                        SetSpeedRate(rate, ERecipeType.Particle);
                        ScriptTrainer.ParticleColliderSpeed.SetSerializedValue(rate.ToString());
                    })
                },
                new List<object>()
                {
                    "研究输出倍率", ScriptTrainer.ResearchOutRate, (UnityAction<int>)( rate =>
                    {
                        SetOUTRate(rate, ERecipeType.Research);
                        ScriptTrainer.ResearchOutRate.SetSerializedValue(rate.ToString());
                    })
                },
                new List<object>()
                {
                    "研究速度倍率", ScriptTrainer.ResearchSpeed, (UnityAction<int>)( rate =>
                    {
                        SetSpeedRate(rate, ERecipeType.Research);
                        ScriptTrainer.ResearchSpeed.SetSerializedValue(rate.ToString());
                    })
                },
                new List<object>()
                {
                    "精炼输出倍率", ScriptTrainer.RefineryOutRate, (UnityAction<int>)( rate =>
                    {
                        SetOUTRate(rate, ERecipeType.Refine);
                        ScriptTrainer.RefineryOutRate.SetSerializedValue(rate.ToString());
                    })
                },
                new List<object>()
                {
                    "精炼速度倍率", ScriptTrainer.RefinerySpeed, (UnityAction<int>)( rate =>
                    {
                         SetSpeedRate(rate, ERecipeType.Refine);
                        ScriptTrainer.RefinerySpeed.SetSerializedValue(rate.ToString());
                    })
                },
                new List<object>()
                {
                    "交换输出倍率", ScriptTrainer.ExchangeOutRate, (UnityAction<int>)( rate =>
                    {
                        SetOUTRate(rate, ERecipeType.Exchange);
                        ScriptTrainer.ExchangeOutRate.SetSerializedValue(rate.ToString());
                    })
                },
                new List<object>()
                {
                    "交换速度倍率", ScriptTrainer.ExchangeSpeed, (UnityAction<int>)( rate =>
                    {
                        SetSpeedRate(rate, ERecipeType.Exchange);
                        ScriptTrainer.ExchangeSpeed.SetSerializedValue(rate.ToString());
                    })
                },
                new List<object>()
                {
                    "分馏输出倍率", ScriptTrainer.FractionateOutRate, (UnityAction<int>)( rate =>
                    {
                        SetOUTRate(rate, ERecipeType.Fractionate);
                        ScriptTrainer.FractionateOutRate.SetSerializedValue(rate.ToString());
                    })
                },
                new List<object>()
                {
                    "分馏速度倍率", ScriptTrainer.FractionateSpeed, (UnityAction<int>)( rate =>
                    {
                         SetSpeedRate(rate, ERecipeType.Fractionate);
                        ScriptTrainer.FractionateSpeed.SetSerializedValue(rate.ToString());
                    })
                },
                new List<object>()
                {
                    "光子输出倍率", ScriptTrainer.PhotonStoreOutRate, (UnityAction<int>)( rate =>
                    {
                        SetOUTRate(rate, ERecipeType.PhotonStore);
                        ScriptTrainer.PhotonStoreOutRate.SetSerializedValue(rate.ToString());
                    })
                },
                new List<object>()
                {
                    "光子速度倍率", ScriptTrainer.PhotonStoreSpeed, (UnityAction<int>)( rate =>
                    {
                        SetSpeedRate(rate, ERecipeType.PhotonStore);
                        ScriptTrainer.PhotonStoreSpeed.SetSerializedValue(rate.ToString());
                    })
                }
            };

            int i = 0;
            recipe.ForEach(item =>
            {
                AddCount(scrollView, (string)item[0], ((ConfigEntry<int>)item[1]).Value, (UnityAction<int>)item[2], 1, 1);

                i++;

                if (i % 3 == 0)
                {
                    hr();
                }
            });

            hr();
            // 作者名称
            Sprite txtBgSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#7AB900FF"));
            GameObject uiText = UIControls.createUIText(scrollView, txtBgSprite, "#FFFFFFFF");
            //uiText.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 30);
            uiText.GetComponent<RectTransform>().localPosition = new Vector3(-300 + 80, -160 + 10, 0 );
            Text text = uiText.GetComponent<Text>();
            text.text = "此功能By:@qq1091192337";
            text.alignment = TextAnchor.MiddleLeft;
            text.fontSize = 14;

        }

        // 添加数字 加减按钮
        public static Transform AddCount(GameObject panel, string label, int value, UnityAction<int> action,
            int step = 1, int? min = null, int? max = null)
        {
            // 计算x轴偏移
            elementX += 120 / 2 - 30;
            // label
            Sprite txtBgSprite = UIControls.createSpriteFrmTexture(UIControls.createDefaultTexture("#7AB900FF"));
            GameObject uiText = UIControls.createUIText(panel, txtBgSprite, "#FFFFFFFF");
            uiText.GetComponent<Text>().text = label;
            uiText.GetComponent<RectTransform>().localPosition = new Vector3(elementX, elementY, 0);
            uiText.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

            // 坐标偏移
            elementX += 60;

            // 加减按钮
            Transform count = Components.createCount(panel);
            RectTransform rt = count.GetComponent<RectTransform>();
            rt.localPosition = new Vector3(elementX, elementY, 0);

            // times-value 显示值
            Transform times_value = count.Find("times-value");
            times_value.GetComponent<Text>().text = value.ToString();

            // - 按钮
            UIButton minusUIButton = count.Find("-").GetComponent<UIButton>();

            minusUIButton.onClick += (int a) =>
            {
                value -= step;

                if (min != null)
                    if (value <= min)
                        value = (int)min;

                times_value.GetComponent<Text>().text = value.ToString();
                action(value);
            };
            minusUIButton.onRightClick += (int a) =>
            {
                value -= step * 10;

                if (min != null)
                    if (value <= min)
                        value = (int)min;

                times_value.GetComponent<Text>().text = value.ToString();
                action(value);
            };


            // + 按钮
            UIButton plusUIButton = count.Find("+").GetComponent<UIButton>();
            plusUIButton.onClick += (int a) =>
            {
                value += step;

                if (max != null)
                    if (value >= max)
                        value = (int)max;


                times_value.GetComponent<Text>().text = value.ToString();
                action(value);
            };
            plusUIButton.onRightClick += (int a) =>
            {
                value += step * 10;

                if (max != null)
                    if (value >= max)
                        value = (int)max;


                times_value.GetComponent<Text>().text = value.ToString();
                action(value);
            };

            elementX += 120;

            return count;
        }

        // 重置坐标
        public static void ResetCoordinates(bool x, bool y = false)
        {
            if (x) elementX = initialX;
            if (y) elementY = initialY;
        }

        // 换行
        public static void hr(int offsetX = 0, int offsetY = 0)
        {
            ResetCoordinates(true);
            elementX += offsetX;
            elementY -= 50 + offsetY;
        }

        private static void SetOUTRate(int rate, ERecipeType type)
        {
            for (int i = 0; i < factorySystem.assemblerPool.Length; i++)
            {
                if (factorySystem.assemblerPool[i].recipeType == type && factorySystem.assemblerPool[i].speedOverride != 0)
                {
                    for (int j = 0; j < factorySystem.assemblerPool[i].productCounts.Length; j++)
                    {
                        factorySystem.assemblerPool[i].productCounts[j] = RecipeBak[factorySystem.assemblerPool[i].recipeId - 1].ResultCounts[j] * rate;
                    }
                }
            }
        }

        private static void SetSpeedRate(int rate, ERecipeType type)
        {
            for (int i = 0; i < factorySystem.assemblerPool.Length; i++)
            {
                if (factorySystem.assemblerPool[i].recipeType == type && factorySystem.assemblerPool[i].speedOverride != 0)
                {
                    if (!RecipeID_SpeedDic.ContainsKey(factorySystem.assemblerPool[i].recipeId - 1))
                    {
                        RecipeID_SpeedDic[factorySystem.assemblerPool[i].recipeId - 1] = factorySystem.assemblerPool[i].speed;
                    }
                    factorySystem.assemblerPool[i].speed = RecipeID_SpeedDic[factorySystem.assemblerPool[i].recipeId - 1] * rate;


                }
            }
        }
    }
}