using System;
using UnityEngine;

namespace ScriptTrainer;
public class BasicWindow
{

    public BasicWindow(GameObject panel)
    {
        #region[添加功能按钮]
        Components.AddButton("添加现金", panel, Scripts.AddMoney);
        Components.AddButton("添加San", panel, Scripts.AddSan);
        Components.AddButton("添加灵魂", panel, Scripts.AddSouls);
        Components.AddButton("添加清洁度", panel, Scripts.AddCleanliness);
        Components.AddButton("降低恶值", panel, Scripts.ReduceEvil);
        Components.AddButton("添加行动力", panel, Scripts.AddActionPiont);
        // ActionPiont

        Components.Hr();

        Components.AddButton("耶芙娜好感度", panel, Scripts.AddDragonFavor);
        Components.AddButton("小叶子好感度", panel, Scripts.AddMaidFavor);
        Components.AddButton("霞露零好感度", panel, Scripts.AddElfFavor);
        Components.AddButton("特莉波好感度", panel, Scripts.AddDeathFavor);

        #endregion


    }

}
