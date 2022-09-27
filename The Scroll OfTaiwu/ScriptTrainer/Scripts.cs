using Config;
using GameData.Common;
using GameData.Domains;
using GameData.Domains.Character;
using GameData.GameDataBridge;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityGameUI;
using GameData.Domains.Character;

namespace ScriptTrainer
{
    public static class Scripts
    {
        #region[全局参数]
        public static int playerId
        {
            get
            {
                return SingletonObject.getInstance<BasicGameData>().TaiwuCharId;
            }
        }
        public static int CurCharacterId
        {
            get
            {
                try
                {
                    UI_CharacterMenu ui_CharacterMenuSubPageBase = UIElement.CharacterMenu.UiBaseAs<UI_CharacterMenu>();
                    //int CurCharacterId = Traverse.Create(typeof(UI_CharacterMenu)).Field<int>("CurCharacterId").Value;
                    return ui_CharacterMenuSubPageBase.CurCharacterId;
                }
                catch (Exception)
                {

                    return -1;
                }
                
            }
        }
        #endregion

        #region[添加资源]
        public static void AddPlayerMoney()
        {
            UIWindows.SpawnInputDialog("您想要添加多少现金？", "添加", "1000", (string count) =>
            {
                GMFunc.GetAdvancedResource(count.ConvertToIntDef(1000), true, false, false);
            });
        }

        public static void AddPlayerAuthority()
        {
            UIWindows.SpawnInputDialog("您想要添加多少威望？", "添加", "1000", (string count) =>
            {
                GMFunc.GetAdvancedResource(count.ConvertToIntDef(1000), false, true, false);
            });
        }

        public static void AddPlayerGoldIron()
        {
            UIWindows.SpawnInputDialog("您想要添加多少金铁？", "添加", "1000", (string count) =>
            {
                AddResource(2, count);
            });
        }

        public static void AddPlayerJadeStone()
        {
            UIWindows.SpawnInputDialog("您想要添加多少玉石？", "添加", "1000", (string count) =>
            {
                AddResource(3, count);
            });
        }
        public static void AddPlayerCloth()
        {
            UIWindows.SpawnInputDialog("您想要添加多少织物？", "添加", "1000", (string count) =>
            {
                AddResource(4, count);
            });           
        }
        public static void AddPlayerMedicine()
        {
            UIWindows.SpawnInputDialog("您想要添加多少药材？", "添加", "1000", (string count) =>
            {
                AddResource(5, count);
            });
        }
        public static void AddPlayerWood()
        {
            UIWindows.SpawnInputDialog("您想要添加多少木材？", "添加", "1000", (string count) =>
            {
                AddResource(1, count);
            });
        }

        public static void AddPlayerFood()
        {
            UIWindows.SpawnInputDialog("您想要添加多少食材？", "添加", "1000", (string count) =>
            {
                AddResource(0, count);
            });
        }


        private static void  AddResource(sbyte type, string count)
        {
            Traverse.Create(typeof(GMFunc)).Method("AddResource", type, count.ConvertToIntDef(1000)).GetValue();

            //GameDataBridge.AddMethodCall<sbyte, int>(-1, 5, 6, type, count.ConvertToIntDef(1000));
        }
        #endregion

        #region[玩家功能]

        /// <summary>
        /// 设置伤势
        /// </summary>
        /// <param name="isInnerInjury">是否是内伤</param>
        /// <param name="bodyPartType">身体部位值 (0-胸背；1-腰腹；2-头颅；3-左臂；4-右臂；5-左腿；6-右腿)</param>
        /// <param name="delta">伤势值 最大6 负数为降低</param>
        public static void ChangeInjury(bool isInnerInjury, sbyte bodyPartType, sbyte delta)
        {
            //int charId = playerId();
            GameDataBridge.AddMethodCall<int, bool, sbyte, sbyte>(-1, 4, 76, playerId, isInnerInjury, bodyPartType, delta);

            //GMFunc.ChangeInjury(playerId, isInnerInjury, bodyPartType, delta);
        }
        
        // 设置中毒
        public static void ChangePoisoned(sbyte poisonType, int changeValue)
        {
            //int charId = playerId();
            GameDataBridge.AddMethodCall<int, sbyte, int>(-1, 4, 77, playerId, poisonType, changeValue);

            //GMFunc.ChangePoisoned(playerId, poisonType, changeValue);
        }

        // 改变地区恩义?
        public static void ChangeSpiritualDebt(int areaId, int spiritualDebt)
        {
            GMFunc.ChangeSpiritualDebt(areaId, spiritualDebt);

            //GameDataBridge.AddMethodCall<short, short>(-1, 2, 4, (short)areaId, (short)spiritualDebt);
        }

        // 修改玩家年龄
        public static void ChangeAge(int charid = 0)            
        {
            if (charid == 0)
            {
                UIWindows.SpawnInputDialog($"您想将自己修改为多少岁？", "设置", "18", (string count) =>
                {
                    GMFunc.EditActualAge(playerId, count.ConvertToIntDef(18));
                });
            }
            else
            {
                UIWindows.SpawnInputDialog($"您想将{charid}修改为多少岁？", "设置", "18", (string count) =>
                {
                    GMFunc.EditActualAge(charid, count.ConvertToIntDef(18));
                });
            }
            
        }
        
        public static void ChangeHp()
        {
            UIWindows.SpawnInputDialog("您想将血量设置为多少？", "设置", "200", (string count) =>
            {
                short value = (short) count.ConvertToIntDef(200);

                GameDataBridge.AddDataModification<short>(4, 0, (ulong)playerId, 19U, (short)value);
                GameDataBridge.AddDataModification<short>(4, 0, (ulong)playerId, 20U, (short)value);
            });
        }

        // 修改主要属性
        public static void ChangeMainAttributes(short[] attributes, int charId = 0)
        {
            if (charId == 0)
            {
                // 修改主要属性
                GameDataBridge.AddDataModification<MainAttributes>(4, 0, (ulong)playerId, 18U, new MainAttributes(attributes));
                GameDataBridge.AddDataModification<MainAttributes>(4, 0, (ulong)playerId, 43U, new MainAttributes(attributes));
            }
            else
            {
                // 修改主要属性
                GameDataBridge.AddDataModification<MainAttributes>(4, 0, (ulong)charId, 18U, new MainAttributes(attributes));
                GameDataBridge.AddDataModification<MainAttributes>(4, 0, (ulong)charId, 43U, new MainAttributes(attributes));
            }
            
        }

        public static void ChangeNeiLi(int[] allocation)
        {
            Debug.Log($"{allocation[0]}-{allocation[1]}-{allocation[2]}-{allocation[3]}");

            GMFunc.EditExtraNeiliAllocation(playerId, allocation[0], allocation[1], allocation[2], allocation[3]);

            //NeiliAllocation arg = default(NeiliAllocation);

            //*arg.Items.FixedElementField = allocation[0];

            //GameDataBridge.AddMethodCall<int, NeiliAllocation>(-1, 4, 67, playerId, arg);
        }

        // 编辑基础道德
        public static void ChangeBaseMorality()
        {
            UIWindows.SpawnInputDialog("您想修改道德为多少？", "设置", "18", (string count) =>
            {
                GMFunc.EditBaseMorality(playerId, count.ConvertToIntDef(18));
            });
        }

        #endregion


        #region[获取资源]

        public static void GetItem(sbyte itemType, int itemId, int count)
        {
            //int charId = playerId();

            //GameDataBridge.AddMethodCall<int, sbyte, short, int>(-1, 4, 17, charId, itemType, (short)itemId, count);

            if (itemType == 99)
            {
                GameDataBridge.AddMethodCall<int, int>(-1, 9, 85, itemId, playerId);
            }
            else
            {
                GMFunc.GetItem(playerId, count, itemType, (short)itemId, null);
            }
        }

        #endregion


        #region[Npc功能]
        
        public static void ChangeFavor(int charId1, int charId2)
        {
            UIWindows.SpawnInputDialog("您想修改好感为多少？", "设置", "6000", (string count) =>
            {
                GameDataBridge.AddMethodCall(-1, 4, 58, charId1, charId2, (short)count.ConvertToIntDef(18));
                //GMFunc.ChangeFavorability(charId1, charId2, (short)count.ConvertToIntDef(18));
            });

        }

        #endregion


        public static void Test()
        {
            Debug.Log(CurCharacterId.ToString());

            //GMFunc.CricketForceWin();

            // 获取名誉相关事件
            //for (int i = 0; i < FameAction.Instance.Count; i++)
            //{
            //    Debug.Log($"{FameAction.Instance[i].Name}, {i}");
            //}

            // 事件参与者
            //for (int i = 0; i < EventActors.Instance.Count; i++)
            //{
            //    Debug.Log($"{EventActors.Instance[i].Name}, {i}");
            //}

            // CharacterPropertyReferenced
            //for (int i = 0; i < Character.Instance.Count; i++)
            //{
            //    Debug.Log($"{Character.Instance[i].GivenName}, {Character.Instance[i].TemplateId}");
            //}

            //foreach (var item in Character.Instance)
            //{
            //    Debug.Log($"{item.Surname} {item.GivenName}, {item.TemplateId}");
            //}

            //foreach (var item in Config.Armor.Instance)
            //{
            //    Debug.Log($"{item.Name}, {item.TemplateId}");
            //}

            //foreach (var item in Config.MapArea.Instance)
            //{
            //    Debug.Log($"{item.Name} = {item.TemplateId}");
            //}


            //int a =   Config.CharacterFeature.GetCharacterPropertyBonus(playerId(), ECharacterPropertyReferencedType.AttackSpeed);

            //Debug.Log(a.ToString());

        }
    }
}
