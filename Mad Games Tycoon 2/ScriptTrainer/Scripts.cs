using UnityEngine;
using UnityGameUI;

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
                    // 有些员工就算改了属性也会掉
                    component.s_gamedesign = 1000;
                    component.s_programmieren = 1000;
                    component.s_grafik = 1000;
                    component.s_sound = 1000;
                    component.s_pr = 1000;
                    component.s_gametests = 1000;
                    component.s_technik = 1000;
                    component.s_forschen = 1000;
                }
            }
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

        // 我的所有游戏最高人气
        public static void MaxHype()
        {
            GameObject[] array = GameObject.FindGameObjectsWithTag("Game");
            foreach (var item in array)
            {
                if (item)
                {
                    gameScript component = item.GetComponent<gameScript>();
                    // 判断是否是我的游戏
                    if (component.IsMyGame())
                    {
                        // 这里热度最高是100
                        // 虽然可以强行将热度改到1000 但是会因为游戏本身的限制 导致掉粉 并强行将热度改回100        
                        component.AddHype(100);
                    }
                }
            }
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
    }
}
