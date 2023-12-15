using System;
using UnityEngine;
using 函数库;

namespace ScriptTrainer;
public class BasicScripts
{

    private 总结脚本 SummaryScript => GameObject.Find("总结")?.GetComponent<总结脚本>();
    // private 模块_玩家_建造 建造脚本 => GameObject.Find("玩家")?.GetComponent<模块_玩家_建造>();
    public static 模块_玩家_动力 玩家动力
    {
        get
        {
            try
            {
                return 常用函数.搜索物体并返回物体内组件<模块_玩家_动力>("玩家");

            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);

                return null;
            }
        }
    }


    public BasicScripts(GameObject panel)
    {
        #region[添加功能按钮]
        Components.AddH3("常用功能", panel);
        {
            Components.AddInputField("现金", 150, SummaryScript.当前金钱数.ToString(), panel, (string value) =>
            {
                SummaryScript.当前金钱数 = int.Parse(value);
            });

            if (玩家动力)
            {
                Components.AddInputField("动力", 150, 玩家动力.当前动力.ToString(), panel, (string value) =>
                {
                    玩家动力.当前动力 = int.Parse(value);
                });
            }



            Components.AddInputField("基地血量", 150, SummaryScript.当前基地血量.ToString(), panel, (string value) =>
            {
                SummaryScript.当前基地血量 = int.Parse(value);
            });
        }

        Components.Hr();
        Components.AddToggle("无限CD", 150, panel, (bool value) => ScriptPatch.settingsNoCD = value);
        Components.AddToggle("无需动力", 150, panel, (bool value) => ScriptPatch.settingsNoMotivation = value);

        #endregion

    }

}
