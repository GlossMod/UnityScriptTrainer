using System;
using UnityEngine;
using UnityGameUI;

namespace ScriptTrainer;
public class Scripts
{

    public static GameObject Main
    {
        get => GameObject.FindGameObjectWithTag("Main");
    }

    public static mainScript Ms
    {
        get => Main.GetComponent<mainScript>();
    }

    // 添加现金
    public static void AddMoney()
    {
        UIWindows.SpawnInputDialog("您需要添加多少钱？", "添加", "1000000", (string money) =>
        {
            Ms.Earn(money.ConvertToLongDef(100000), 14);
        });
    }
    // 添加粉丝
    public static void AddFans()
    {
        UIWindows.SpawnInputDialog("您想要添加多少粉丝？", "添加", "10000", (string fans) =>
        {
            // 均衡分配粉丝
            Ms.AddFans(fans.ConvertToIntDef(10000), -1);
        });
    }

    // 员工技能全满
    public static void SuperStaff()
    {
        GameObject[] array = GameObject.FindGameObjectsWithTag("Character");    // 获取员工列表
        foreach (var item in array)
        {
            if (item)
            {
                characterScript component = item.GetComponent<characterScript>();
                // this.s_gamedesign + this.s_programmieren + this.s_grafik + this.s_sound + this.s_pr + this.s_gametests + this.s_technik + this.s_forschen
                // 有些员工就算改了属性也会掉
                component.s_gamedesign = 100;
                component.s_programmieren = 100;
                component.s_grafik = 100;
                component.s_sound = 100;
                component.s_pr = 100;
                component.s_gametests = 100;
                component.s_technik = 100;
                component.s_forschen = 100;
            }
        }
    }

    // 全部研究
    public static void StudyAll()
    {
        // 类型
        genres genres = Main.GetComponent<genres>();
        genres.UnlockAll();
        genres.ResearchAll();

        // 主题
        themes themes = Main.GetComponent<themes>();
        themes.ResearchAll();

        // 引擎
        engineFeatures engineFeatures = Main.GetComponent<engineFeatures>();
        engineFeatures.UnlockAll();
        engineFeatures.MaxLevelAll();

        // 游戏功能
        gameplayFeatures gameFeatures = Main.GetComponent<gameplayFeatures>();
        gameFeatures.UnlockAll();
        gameFeatures.MaxLevelAll();

        // 硬件设备
        hardware hardware = Main.GetComponent<hardware>();
        hardware.UnlockAll();
        hardware.ResearchAll();

        // 游戏机功能
        hardwareFeatures hardwareFeatures = Main.GetComponent<hardwareFeatures>();
        hardwareFeatures.UnlockAll();
        hardwareFeatures.ResearchAll();

    }

    // 移除所有负面buff
    public static void RemoveStaffBuff()
    {
        GameObject[] array = GameObject.FindGameObjectsWithTag("Character");    // 获取员工列表
        Debug.Log(array.Length);
        foreach (var item in array)
        {
            if (item)
            {
                characterScript component = item.GetComponent<characterScript>();

                // 18.19.20.21.22.27 为负面buff
                component.perks[18] = false;
                component.perks[19] = false;
                component.perks[20] = false;
                component.perks[21] = false;
                component.perks[22] = false;
                component.perks[27] = false;

            }
        }
    }

    public static void AddAllBuff()
    {
        GameObject[] array = GameObject.FindGameObjectsWithTag("Character");    // 获取员工列表
        Debug.Log(array.Length);
        foreach (var item in array)
        {
            if (item)
            {
                characterScript component = item.GetComponent<characterScript>();

                // 18.19.20.21.22.27 为负面buff

                for (int i = 2; i < 30; i++)
                {
                    component.perks[i] = true;
                }

                component.perks[18] = false;
                component.perks[19] = false;
                component.perks[20] = false;
                component.perks[21] = false;
                component.perks[22] = false;
                component.perks[27] = false;
            }
        }
    }

    // 知道所有类型的倾向度
    public static void AllKnow(bool konw)
    {
        ScriptPatch.setingsAllKnow = konw;
    }

    // 不发薪水
    public static void NoSalary(bool state)
    {
        ScriptPatch.settingsNoSalary = state;
    }

    // 修改工作效率
    public static void AddMotivation()
    {
        UIWindows.SpawnInputDialog("修改员工工作效率", "修改", "1000", (string motivation) =>
        {
            ScriptPatch.settingsWorkEfficiency = true;
            ScriptPatch.workEfficiency = motivation.ConvertToIntDef(1000);

        });
    }

    // 我的所有游戏最高人气
    public static void MaxHype()
    {
        UIWindows.SpawnInputDialog("修改游戏人气", "添加", "100", (string fans) =>
       {
           // 修改游戏人气
            ScriptPatch.setingsMaxHype1 = fans.ConvertToFloatDef(100);
       });
        UIWindows.SpawnInputDialog("修改主机人气", "添加", "100", (string fans) =>
       {
           // 修改主机人气
           ScriptPatch.setingsMaxHype2 = fans.ConvertToFloatDef(100);
       });

        // ScriptPatch.setingsMaxHype = state;
        // GameObject[] array = GameObject.FindGameObjectsWithTag("Game");
        // foreach (var item in array)
        // {
        //     if (item)
        //     {
        //         gameScript component = item.GetComponent<gameScript>();
        //         // 判断是否是我的游戏
        //         if (component.IsMyGame())
        //         {
        //             // 这里热度最高是200
        //             // 虽然可以强行将热度改到1000 但是会因为游戏本身的限制 导致掉粉 并强行将热度改回100        
        //             component.AddHype(200);
        //         }
        //     }
        // }
    }
    // 无限储存空间
    public static void InfiniteSpace(bool state)
    {
        ScriptPatch.setingsInfiniteSpace = state;

    }
    public static void InfiniteServerplatz(bool state)
    {
        ScriptPatch.setingsInfiniteServerplatz = state;
    }

    // 重新上市
    public static void Relist()
    {
        GameObject[] array = GameObject.FindGameObjectsWithTag("Game");
        foreach (var item in array)
        {
            if (item)
            {
                gameScript component = item.GetComponent<gameScript>();
                if (component.IsMyGame())
                {
                    component.weeksOnMarket = 1;
                }
            }
        }

        GameObject[] array2 = GameObject.FindGameObjectsWithTag("Platform");
        foreach (var item in array2)
        {
            if (item)
            {
                platformScript component2 = item.GetComponent<platformScript>();
                component2.weeksOnMarket = 1;
            }
        }
    }

    // 主机销量倍率
    public static void HostSalesRatio()
    {
        UIWindows.SpawnInputDialog("销量乘数", "确定", "10000", (string fans) =>
       {
           ScriptPatch.setingsHostSalesRatio = fans.ConvertToFloatDef(10000);
       });
    }

    public static void AcquisitionCompany(bool state)
    {
        ScriptPatch.setingsAcquisitionCompany = state;
        // // publisherScript[] ps = Ms.arrayPublisherScripts;
        // foreach (var item in Ms.arrayPublisherScripts)
        // {
        //     Debug.Log(item.GetName());
        //     item.lockToBuy = 0;
        //     item.GetMoneyExklusiv();
        // }
    }
}
