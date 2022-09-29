using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityGameUI;
using Navigation = UnityGameUI.Navigation;
using Object = UnityEngine.Object;

namespace ScriptTrainer
{
    internal class MainWindow: MonoBehaviour
    {
        #region[声明]
        // Trainer Base
        public static GameObject obj = null;
        public static MainWindow instance;
        public static bool initialized = false;
        public static bool _optionToggle = false;
        private static TooltipGUI toolTipComp = null;

        // UI
        public static AssetBundle testAssetBundle = null;
        public static GameObject canvas = null;
        private static bool isVisible = false;
        private static GameObject uiPanel = null;
        private static readonly int width = Mathf.Min(Screen.width, 740);
        private static readonly int height = (Screen.height < 400) ? Screen.height : (450);

        // 窗口开关
        public static bool optionToggle
        {
            get => _optionToggle;
            set
            {
                _optionToggle = value;


                if (!initialized)
                {
                    instance.CreateUI();
                }
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

        private static int elementX = initialX;
        private static int elementY = initialY;
        #endregion

        public MainWindow()
        {
            instance = this;
        }

        public void Initialize()
        {
            initialized = true;
            instance.CreateUI();
        }

        public void CreateUI()
        {
            
        }
    }
}
