using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptTrainer;
public class SettingsScripts
{


    public SettingsScripts(GameObject panel)
    {
        #region[添加功能按钮]
        Components.AddH3("设置", panel);
        {
            Components.AddInputField("音量", 150, AudioListener.volume.ToString(), panel, (string value) =>
            {
                AudioListener.volume = float.Parse(value);
            });

            List<string> options = new List<string>(){
                "1920x1080", "1280x720", "960x540", "640x360"
            };

            // 下拉框 选择分辨率
            Components.AddDropdown("分辨率", 150, options, panel, (int value) =>
            {
                switch (value)
                {
                    case 0:
                        Screen.SetResolution(1920, 1080, false);
                        break;
                    case 1:
                        Screen.SetResolution(1280, 720, false);
                        break;
                    case 2:
                        Screen.SetResolution(960, 540, false);
                        break;
                    case 3:
                        Screen.SetResolution(640, 360, false);
                        break;
                    default:
                        break;
                }
            });
        }

        #endregion

    }

}
