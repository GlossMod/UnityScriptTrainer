
using System;
using UnityEngine;

namespace UnityGameUI
{
    internal class TooltipGUI : MonoBehaviour
    {
        public static TooltipGUI instance = null;
        public static bool EnableTooltip = true;
        public static string Tooltip = "";
        private static GUIStyle tooltipStyle;
        private static bool fired = false;  // 调试信息

        public TooltipGUI()
        {
            //BepInExLoader.log.LogMessage("TooltipGUI Loaded");
            instance = this;
        }

        public void OnGUI()
        {
            if (!fired)
            {
                Debug.Log("TooltipGUI OnGUI Fired");
                fired = true;
            }

            if (Tooltip != "" && EnableTooltip == true)
            {
                GUI.backgroundColor = Color.black;
                GUIContent content = new GUIContent(Tooltip);

                tooltipStyle = new GUIStyle(GUI.skin.box);
                tooltipStyle.normal.textColor = Color.white;

                float width = tooltipStyle.CalcSize(content).x;
                float height = tooltipStyle.CalcSize(content).y;

                var mousepos = UnityEngine.Input.mousePosition;
                //var mousepos = EventSystem.current.currentInputModule.input.mousePosition; // Instead of Input.mousePosition
                GUI.Box(new Rect(mousepos.x + 15, Screen.height - mousepos.y + 15, width, 25), content, tooltipStyle); // The +15 are cursor offsets                
            }
        }
    }
}
