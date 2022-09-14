using BepInEx;
using HarmonyLib;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityGameUI;

namespace ScriptTrainer
{
    public class Scripts: MonoBehaviour
    {
        public static void AddMoney()
        {
            UIWindows.SpawnInputDialog("您想添加多少灵石？", "添加", "1000", (string money) => {
                //ms.Earn(money.ConvertToIntDef(100000), 14);
                KBEngine.Avatar player = Tools.instance.getPlayer();
                if (player != null)
                {
                    ulong n = (ulong)money.ConvertToIntDef(1000); ;

                    player.money += n;

                    UIPopTip.Inst.Pop(string.Format("灵石{0}了{1}", n > 0 ? "增加" : "减少", money), n > 0 ? PopTipIconType.上箭头 : PopTipIconType.下箭头);
                }
                else
                {
                    Debug.Log("玩家不存在");
                }
            });
        }

        public static void AddNingZhouShengWang()
        {
            UIWindows.SpawnInputDialog("您想添加多少宁州声望？", "添加", "100", (string money) => {
                //ms.Earn(money.ConvertToIntDef(100000), 14);
                KBEngine.Avatar player = Tools.instance.getPlayer();
                if (player != null)
                {
                    int n = money.ConvertToIntDef(100);
                    PlayerEx.AddNingZhouShengWang(n);
                    UIPopTip.Inst.Pop(string.Format("宁州声望{0}了{1}", n > 0? "增加":"减少" , money), n > 0 ? PopTipIconType.上箭头 : PopTipIconType.下箭头);
                }
                else
                {
                    Debug.Log("玩家不存在");
                }
            });
        }

        public static void AddSeaShengWang()
        {
            UIWindows.SpawnInputDialog("您想添加多少海域声望？", "添加", "100", (string money) =>
            {
                //ms.Earn(money.ConvertToIntDef(100000), 14);
                KBEngine.Avatar player = Tools.instance.getPlayer();
                if (player != null)
                {
                    int n = money.ConvertToIntDef(100);
                    PlayerEx.AddSeaShengWang(n);
                    UIPopTip.Inst.Pop(string.Format("海域声望{0}了{1}", n > 0 ? "增加" : "减少", money), n > 0 ? PopTipIconType.上箭头 : PopTipIconType.下箭头);
                }
                else
                {
                    Debug.Log("玩家不存在");
                }
            });
        }

        public static void AddExp()
        {
            UIWindows.SpawnInputDialog("您想添加多少修为？", "添加", "100000", (string money) =>
            {
                //ms.Earn(money.ConvertToIntDef(100000), 14);
                KBEngine.Avatar player = Tools.instance.getPlayer();
                if (player != null)
                {
                    ulong n = (ulong)money.ConvertToIntDef(100000);

                    player.exp += n;

                    UIPopTip.Inst.Pop(string.Format("修为{0}了{1}", n > 0 ? "增加" : "减少", money), n > 0 ? PopTipIconType.上箭头 : PopTipIconType.下箭头);
                }
                else
                {
                    Debug.Log("玩家不存在");
                }
            });
        }
        
        public static void MaxExp()
        {
            KBEngine.Avatar player = Tools.instance.getPlayer();
            player.exp = (ulong)jsonData.instance.LevelUpDataJsonData[player.level.ToString()]["MaxExp"].I;
            player.PlayerLvUP();
            Debug.Pop("当前修为全满");
        }

        public static void MaxHp()
        {
            KBEngine.Avatar player = Tools.instance.getPlayer();
            player.HP = player.HP_Max;
            Debug.Pop("当前血量全满");
        }

        public static void AddMenPaiShengWang()
        {
            UIWindows.SpawnInputDialog("您想添加多少声望？", "添加", "1000", (string shengWang) => {

                int n = shengWang.ConvertToIntDef(1000);
                
                PlayerEx.AddMenPaiShengWang(n);
                Debug.Pop("门派声望添加了" + n);
                
            });
        }
    }
}
