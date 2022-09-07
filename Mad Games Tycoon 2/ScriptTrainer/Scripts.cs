using HarmonyLib;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ScriptTrainer
{
    public class Scripts : MonoBehaviour
    {
        private static GameObject main_ = GameObject.FindGameObjectWithTag("Main");
        private static mainScript ms = main_.GetComponent<mainScript>();

        public Scripts()
        {
            
        }
        // 添加现金
        public static void AddMoney()
        {
            UIWindows.SpawnInputDialog("您需要添加多少钱？", "添加", "1000000", (string money) => {                                
                ms.Earn(money.ConvertToIntDef(100000), 14);
            });
        }
        // 添加粉丝
        public static void AddFans()
        {
            UIWindows.SpawnInputDialog("您想要添加多少粉丝？", "添加", "10000", (string fans) =>
            {
                // 均衡分配粉丝
                ms.AddFans(fans.ConvertToIntDef(10000), -1);
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
        
        // 知道所有类型的倾向度
        public static void AllKnow(bool konw)
        {
            ScriptPatch.setingsAllKnow = konw;
        }

        // 不发薪水
        public static void NoSalary(bool state)
        {
            ScriptPatch.settingsNoSalary = state;

            //GameObject[] array = GameObject.FindGameObjectsWithTag("Character");    // 获取员工列表
            //foreach (var item in array)
            //{
            //    if (item)
            //    {
            //        characterScript component = item.GetComponent<characterScript>();
            //        if (state)
            //        {
            //            component.gehalt = 1;
            //        }
            //        else
            //        {
            //            component.gehalt = component.CalcGehalt();
            //        }
            //        Debug.Log(component.GetGehalt());
            //    }
            //}
        }

        // 修改工作效率
        public static void AddMotivation()
        {
            UIWindows.SpawnInputDialog("修改员工工作效率", "修改", "1000", (string motivation) =>
            {
                ScriptPatch.settingsWorkEfficiency = true;
                ScriptPatch.workEfficiency = motivation.ConvertToIntDef(1000);

                ////GameObject main_ = GameObject.FindWithTag("Main");
                ////mainScript mS_ = main_.GetComponent<mainScript>();
                //////mS_.settings_arbeitsgeschwindigkeitAnpassen = false;

                ////Debug.Log(mS_.settings_arbeitsgeschwindigkeitAnpassen);
                ////mS_.speedSetting = motivation.ConvertToIntDef(1000);

                //GameObject[] array = GameObject.FindGameObjectsWithTag("Character");    // 获取员工列表
                //foreach (var item in array)
                //{
                //    if (item)
                //    {
                //        characterScript component = item.GetComponent<characterScript>();
                //        component.s_motivation = motivation.ConvertToIntDef(1000);
                //    }
                //}
            });
        }

        public static void MaxPopularity(bool state)
        {
            
        }



    }
}
