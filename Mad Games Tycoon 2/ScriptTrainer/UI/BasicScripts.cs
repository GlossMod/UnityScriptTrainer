using System;
using UnityEngine;

namespace ScriptTrainer;
public class BasicScripts
{

    public BasicScripts(GameObject panel)
    {
        #region[添加功能按钮]
        Components.AddButton("添加现金", panel, Scripts.AddMoney);
        Components.AddButton("添加粉丝", panel, Scripts.AddFans);
        Components.AddButton("工作效率", panel, Scripts.AddMotivation);
        Components.AddButton("超级员工", panel, Scripts.SuperStaff);
        Components.AddButton("研究全部", panel, Scripts.StudyAll);
        Components.AddButton("重新上市", panel, Scripts.Relist);
        Components.Hr();
        Components.AddButton("主机销量倍率", panel, Scripts.HostSalesRatio);
        Components.AddButton("最高热度",  panel,   Scripts.MaxHype);



        Components.Hr();
        {
            MainWindow.ElementX += 150 / 2 - 40;
            GameObject button = Components.AddButton("添加所有正面buff", panel, Scripts.AddAllBuff);
            button.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 40);
            MainWindow.ElementX += 150 / 2 + 10;
        }
        {
            MainWindow.ElementX += 150 / 2 - 60;
            GameObject button = Components.AddButton("移除员工负面buff", panel, Scripts.RemoveStaffBuff);
            // 设置宽度为 1500
            button.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 40);
            MainWindow.ElementX += 150 / 2 + 10;
        }

        Components.Hr();
        Components.AddToggle("不发薪水", 150, panel, Scripts.NoSalary);
        Components.AddToggle("全能开发", 150, panel, Scripts.AllKnow);
        Components.AddToggle("无限储存空间", 150, panel, Scripts.InfiniteSpace);
        Components.AddToggle("无限服务器空间", 150, panel, Scripts.InfiniteServerplatz);
        // Components.AddToggle("最高热度", 150, panel, Scripts.MaxHype);
        Components.Hr();
        Components.AddToggle("免费收购", 150, panel, Scripts.AcquisitionCompany);

        #endregion


    }

}
