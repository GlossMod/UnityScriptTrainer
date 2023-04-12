
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityGameUI;
using WinAPI;
using Object = UnityEngine.Object;

namespace ScriptTrainer
{
    internal class DSNavigation
    {
        #region[全局参数]
        private Transform horizontal_tab;
        private Vector3 localPosition = new Vector3(16, -57, 0);
        private Vector2 sizeDelta = new Vector2(100, 250);

        private int elementY = 0;

        public static List<ITabs> tabs = new List<ITabs>();

        #endregion

        public DSNavigation(GameObject canvas)
        {
            //UIStatisticsWindow statWindow = Object.Instantiate<UIStatisticsWindow>(UIRoot.instance.uiGame.statWindow);
            //horizontal_tab = statWindow.transform.Find("panel-bg/horizontal-tab");

            horizontal_tab = Object.Instantiate(UIRoot.instance.uiGame.statWindow.transform.Find("panel-bg/horizontal-tab"));
            horizontal_tab.SetParent(canvas.transform);

            // 删除多余的内容
            Object.Destroy(horizontal_tab.transform.Find("milestone-btn").gameObject);
            Object.Destroy(horizontal_tab.transform.Find("production-btn").gameObject);
            Object.Destroy(horizontal_tab.transform.Find("power-btn").gameObject);
            Object.Destroy(horizontal_tab.transform.Find("research-btn").gameObject);
            Object.Destroy(horizontal_tab.transform.Find("dyson-btn").gameObject);
            Object.Destroy(horizontal_tab.transform.Find("performance-btn").gameObject);
            Object.Destroy(horizontal_tab.transform.Find("achievement-btn").gameObject);
            Object.Destroy(horizontal_tab.transform.Find("property-btn").gameObject);

            RectTransform rt = horizontal_tab.GetComponent<RectTransform>();
            rt.localPosition = localPosition;
            rt.sizeDelta = sizeDelta;
            //Create();
        }




        public void AddTab(string text, GameObject module, bool show = false)
        {
            tabs.Add(new ITabs(text, module, show));

            //createButton(text, horizontal_tab.gameObject);
        }

        public void Create()
        {

            foreach (var item in tabs)
            {
                Transform button = createButton(item.text, horizontal_tab.gameObject);

                button.GetComponent<UIButton>().onClick += (int i) =>
                {
                    item.SetActive();
                };

                item.module.SetActive(item.show);
            }

        }


        public Transform createButton(string text, GameObject canvas)
        {
            UIStatisticsWindow statWindow = Object.Instantiate<UIStatisticsWindow>(UIRoot.instance.uiGame.statWindow);
            Transform button = statWindow.transform.Find("panel-bg/horizontal-tab/milestone-btn");

            //var a = UIRoot.instance.uiGame.statWindow.transform.Find("panel-bg/horizontal-tab/milestone-btn");
            //Console.WriteLine("a:" + a);
            //Transform button = Object.Instantiate(a);

            button.SetParent(canvas.transform);
            button.name = "tab-button";

            RectTransform rt = button.GetComponent<RectTransform>();
            rt.localPosition = new Vector3(-50, elementY, 0);

            Object.Destroy(button.GetComponentInChildren<Localizer>());

            button.GetComponentInChildren<Text>().text = text;

            elementY -= 50;

            return button;
        }


    }

    public class ITabs
    {
        public string text { get; set; }
        public GameObject module { get; set; }
        public bool show { get; set; }

        public ITabs(string text, GameObject module, bool show)
        {
            this.text = text;
            this.module = module;
            this.show = show;
        }
        internal void SetActive(bool v)
        {
            show = v;
            module.SetActive(v);
        }
    }

    public static class Extensions
    {
        public static void SetActive(this ITabs nav)
        {
            foreach (ITabs n in DSNavigation.tabs)
            {
                if (n == nav)
                {
                    n.SetActive(true);
                }
                else
                {
                    n.SetActive(false);
                }
            }

        }
    }
}
