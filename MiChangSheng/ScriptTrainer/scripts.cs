using System;
using System.Text.RegularExpressions;
using UnityEngine;
using GUIPackage;
using Fungus;

namespace ScriptTrainer
{


    public class Script : MonoBehaviour
    {
        /// <summary>
        /// 获取游戏数据
        /// </summary>
        public static void GetGameData()
        {
            //try
            //{
            //    //foreach (var item in jsonData.instance.ItemJsonData)
            //    //{
            //    //    item i = new item(item.Value["id"].I);
            //    //    ItemData.itemList.Add(i);
            //    //}

            //    Debug.addLog("数据导入完成");
            //}
            //catch (Exception)
            //{
            //    Debug.addLog("未进入游戏");
            //}


        }

        /// <summary>
        /// 检查是否是int
        /// </summary>
        /// <param name="ItemText"></param>
        public static int CheckIsInt(string ItemText)
        {
            int newCount = 0;
            ItemText = Regex.Replace(ItemText, @"[\-?][^0-9.]", "");
            try
            {
                if (ItemText != null && ItemText.Length < 10 && ItemText.Length != 0)
                {
                    newCount = Int32.Parse(ItemText);
                }
                else
                {
                    ItemText = newCount.ToString();
                }
            }
            catch (Exception) { throw; }

            return newCount;
        }

        /// <summary>
        /// 修改门派声望
        /// </summary>
        /// <param name="change"></param>
        public static void ChangeMenPaiShengWang(int change)
        {
            int shengW = PlayerEx.GetMenPaiShengWang();
            if (change != shengW)
            {
                PlayerEx.AddMenPaiShengWang(change - shengW);
            }
        }
        /// <summary>
        /// 修改俸禄
        /// </summary>
        /// <param name="change"></param>
        public static void ChangeFengLuMoney(int change)
        {
            KBEngine.Avatar player = Tools.instance.getPlayer();    // 获取玩家 
            int fengLu = player.chenghaomag.GetAllFengLuMoney();
            if (change != fengLu)
            {
                player.chenghaomag.AddFengLu(change - fengLu);
            }
        }

        /// <summary>
        /// 修改灵感
        /// </summary>
        /// <param name="change"></param>
        public static void ChangeLingGan(int change)
        {
            KBEngine.Avatar player = Tools.instance.getPlayer();    // 获取玩家 
            if (change != player.LingGan)
            {
                player.AddLingGan(change - player.LingGan);
            }
        }
       

        /// <summary>
        /// 修改宁州声望
        /// </summary>
        /// <param name="change"></param>
        public static void ChangeNingZhouShengWang(int change)
        {
            if (change != PlayerEx.GetNingZhouShengWang())
            {
                PlayerEx.AddNingZhouShengWang(change - PlayerEx.GetNingZhouShengWang());
            }            
        }

        /// <summary>
        /// 修改NPC好感度
        /// </summary>
        /// <param name="change"></param>
        public static void ChangeNPCFavor(int change, UINPCData npc)
        {
            if (change != npc.Favor)
            {
                NPCEx.AddFavor(npc.ID, change - npc.Favor, false, true);
                npc.Favor = change;
            }
        }
        /// <summary>
        /// 修改NPC情分
        /// </summary>
        /// <param name="change"></param>
        /// <param name="npc"></param>
        public static void ChangeNPCQingFen(int change, UINPCData npc)
        {
            //UI_ErrorHint._instance.errorShow(jsonData.instance.AvatarRandomJsonData[npcid.ToString()]["Name"].Str + "对你的好感度" + str, showType);
            if (change != npc.QingFen)
            {
                UI_ErrorHint._instance.errorShow(String.Format("你和{0}的情分提升了{1}", npc.Name, change - npc.QingFen), 0);
                NPCEx.AddQingFen(npc.ID, change - npc.QingFen);
                npc.QingFen = change;
            }
           
        }
    }
}