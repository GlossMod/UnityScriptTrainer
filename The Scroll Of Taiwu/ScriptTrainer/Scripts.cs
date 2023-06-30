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
using GameData.Domains.Character.Relation;
using GameData.Domains.Character.Display;
using GameData.Serializer;

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
        public static string CurCharacterName
        {
            get
            {
                //GameDataBridge.AddMethodCall<int>(-1, DomainHelper.DomainIds.Character, CharacterDomainHelper.MethodIds.GetNameRelatedData, CurCharacterId);
                string name = "";

                //SingletonObject.getInstance<AsynchMethodDispatcher>().AsynchMethodCall<int>(DomainHelper.DomainIds.Character, CharacterDomainHelper.MethodIds.GetNameRelatedData, CurCharacterId, (int offset, GameData.Utilities.RawDataPool dataPool) =>
                //{
                //    NameRelatedData list = new NameRelatedData();
                //    Serializer.Deserialize(dataPool, offset, ref list);

                //     name = list.FullName.ToString();


                //});



                return name;


                //UI_CharacterMenu ui_CharacterMenuSubPageBase = UIElement.CharacterMenu.UiBaseAs<UI_CharacterMenu>();
                ////int CurCharacterId = Traverse.Create(typeof(UI_CharacterMenu)).Field<int>("CurCharacterId").Value;
                //string name = "";
                //foreach (var item in ui_CharacterMenuSubPageBase.Names)
                //{
                //    name += item;
                //}
                //return name;
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

        // 设置伤势
        public static void ChangeInjury(bool isInnerInjury, sbyte bodyPartType, sbyte delta, int charId = -1)
        {
            if (charId == -1) charId = playerId;

            // bodyPartType: 0-胸背；1-腰腹；2-头颅；3-左臂；4-右臂；5-左腿；6-右腿
            GameDataBridge.AddMethodCall<int, bool, sbyte, sbyte>(-1, DomainHelper.DomainIds.Character, CharacterDomainHelper.MethodIds.GmCmd_ChangeInjury, charId, isInnerInjury, bodyPartType, delta);
        }
        
        // 设置中毒
        public static void ChangePoisoned(sbyte poisonType, int changeValue, int charId = -1)
        {

            if (charId == -1) charId = playerId;

            //poisonType: 0-烈毒；1-郁毒；2-寒毒；3-赤毒；4-腐毒；5-幻毒            
            GameDataBridge.AddMethodCall<int, sbyte, int>(-1, DomainHelper.DomainIds.Character, CharacterDomainHelper.MethodIds.GmCmd_ChangePoisonByType, charId, poisonType, changeValue);
        }

        // 改变地区恩义?
        public static void ChangeSpiritualDebt(int areaId, int spiritualDebt)
        {
            GMFunc.ChangeSpiritualDebt(areaId, spiritualDebt);

            //GameDataBridge.AddMethodCall<short, short>(-1, 2, 4, (short)areaId, (short)spiritualDebt);
        }

        // 修改玩家年龄
        public static void ChangeAge(int charId = -1)            
        {
            if (charId == -1)
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
            UIWindows.SpawnInputDialog("您想将血量设置为多少？", "设置", "1200", (string count) =>
            {
                short value = (short) count.ConvertToIntDef(1200);

                GameDataBridge.AddDataModification<short>(4, 0, (ulong)playerId, 19U, (short)value);
                GameDataBridge.AddDataModification<short>(4, 0, (ulong)playerId, 20U, (short)value);
            });
        }

        // 修改主要属性
        public static void ChangeMainAttributes(short[] attributes, int charId = -1)
        {
            if (charId == -1)
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

        // 修改内力
        public static void ChangeNeiLi(int[] allocation)
        {
            Debug.Log($"{allocation[0]}-{allocation[1]}-{allocation[2]}-{allocation[3]}");

            GMFunc.EditExtraNeiliAllocation(playerId, allocation[0], allocation[1], allocation[2], allocation[3]);

            //NeiliAllocation arg = default(NeiliAllocation);

            //*arg.Items.FixedElementField = allocation[0];

            //GameDataBridge.AddMethodCall<int, NeiliAllocation>(-1, 4, 67, playerId, arg);
        }

        
        // 解锁所有技艺
        public static void UnlockAllSkills(int charId = -1)
        {
            if (charId == -1) charId = playerId;

            List<GameData.Domains.Character.LifeSkillItem> list_item = new List<GameData.Domains.Character.LifeSkillItem>();

            foreach (var item in Config.LifeSkill.Instance)
            {
                Debug.Log($"已学习 {item.Name}");
                GameData.Domains.Character.LifeSkillItem lifeSkillItem = new GameData.Domains.Character.LifeSkillItem(item.TemplateId, 5);
                list_item.Add(lifeSkillItem);
            }
            GameDataBridge.AddMethodCall<int, List<GameData.Domains.Character.LifeSkillItem>>(-1, DomainHelper.DomainIds.Character, CharacterDomainHelper.MethodIds.GmCmd_SetLearnedLifeSkills, charId, list_item);
            // 基础 技艺 资质
            //LifeSkillShorts shorts = new LifeSkillShorts(100, 100, 100);
            //GameDataBridge.AddDataModification<LifeSkillShorts>(4, 0, (ulong)((long)charId), 30U, shorts);
        }


        // 编辑基础道德 有 Bug
        public static void ChangeBaseMorality()
        {
            UIWindows.SpawnInputDialog("您想修改道德为多少？", "设置", "18", (string count) =>
            {
                GMFunc.EditBaseMorality(playerId, count.ConvertToIntDef(18));
            });
        }

        // 解锁武学 有 Bug
        public static void UnlockAllArts()
        {
            //  Learn Combat Skill

            foreach (var item in CombatSkill.Instance)
            {
                Debug.Log($"已学习 {item.Name} - {item.TemplateId}");

                // GmCmd_ForgetCombatSkill(DataContext context, int charId, short skillTemplateId)
                GameDataBridge.AddMethodCall<int, short>(-1, DomainHelper.DomainIds.Character, CharacterDomainHelper.MethodIds.GmCmd_ForgetCombatSkill, playerId, item.TemplateId);

                // public void GmCmd_RevokeCombatSkill(DataContext context, int charId, List<short> skillTemplateIdList)
                GameDataBridge.AddMethodCall<int, short>(-1, DomainHelper.DomainIds.Building, GameData.Domains.Building.BuildingDomainHelper.MethodIds.AcceptBuildingBlockRecruitPeople, playerId, item.TemplateId);

                GameDataBridge.AddMethodCall<int, short, sbyte, ushort>(-1, DomainHelper.DomainIds.Character, CharacterDomainHelper.MethodIds.LearnCombatSkill, playerId, item.TemplateId, 100, 10);

                //GameDataBridge.AddDataModification<ushort>(7, 0, (ulong)new GameData.Domains.CombatSkill.CombatSkillKey(playerId, item.TemplateId), 2U, ushort.MaxValue);
                //GameDataBridge.AddDataModification<sbyte>(7, 0, (ulong)new GameData.Domains.CombatSkill.CombatSkillKey(playerId, item.TemplateId), 1U, 100);

            }

            
            // LearnCombatSkill(int charId, short skillId)
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
        // 修改好感度
        public static void ChangeFavor(int charId1, int charId2)
        {
            UIWindows.SpawnInputDialog("您想修改好感为多少？", "设置", "10000", (string count) =>
            {
                GameDataBridge.AddMethodCall(-1, DomainHelper.DomainIds.Character, CharacterDomainHelper.MethodIds.GmCmd_ChangeFavorability, charId1, charId2, (short)count.ConvertToIntDef(10000));
                //GMFunc.ChangeFavorability(charId1, charId2, (short)count.ConvertToIntDef(18));
            });

        }
        // 绑架NPC
        public static void Kidnap(int charId)
        {
            GameDataBridge.AddMethodCall<int, int>(-1, DomainHelper.DomainIds.Character, CharacterDomainHelper.MethodIds.GmCmd_MakeCharacterKidnapped, playerId, charId);
        }
        // 设置关系
        public static void Relationship(int charIdA, int charIdB)
        {
            //GMFunc.AddCharacterRelationship(type: (sbyte)RelationType.Mentor, Scripts.playerId, Scripts.CurCharacterId);

            List<string> options = new List<string>
            {
                "一般", "父母","子女","手足", "义父母", "养子", "养手足", "结义金兰","结为夫妻", "师父","徒弟","朋友","敬仰之人","仇人"
            };
            List<ushort> o_type = new List<ushort> {
                RelationType.General,
                RelationType.BloodParent,
                RelationType.BloodChild,
                RelationType.BloodBrotherOrSister,
                RelationType.StepParent,
                RelationType.StepChild,
                RelationType.AdoptiveBrotherOrSister,
                RelationType.SwornBrotherOrSister,
                RelationType.HusbandOrWife,
                RelationType.Mentor,
                RelationType.Mentee,
                RelationType.Friend,
                RelationType.Adored,
                RelationType.Enemy
            };

            UIWindows.SpawnDropdownDialog($"你想让{charIdB}成为你的什么？", "修改", options, (int call) =>
            {
                GameDataBridge.AddMethodCall<int, int, ushort>(-1, DomainHelper.DomainIds.Character, CharacterDomainHelper.MethodIds.GmCmd_AddRelation, charIdA, charIdB, o_type[call]);
            });
            UIWindows.SpawnDropdownDialog($"你想让你成为{charIdB}的什么？", "修改", options, (int call) =>
            {
                GameDataBridge.AddMethodCall<int, int, ushort>(-1, DomainHelper.DomainIds.Character, CharacterDomainHelper.MethodIds.GmCmd_AddRelation, charIdB, charIdA, o_type[call]);
            });

        }

        #endregion

        //public static void GetCharacterName(int charId, ref string name)
        //{
        //    SingletonObject.getInstance<AsynchMethodDispatcher>().AsynchMethodCall<int>(DomainHelper.DomainIds.Character, CharacterDomainHelper.MethodIds.GetNameRelatedData, CurCharacterId, (int offset, GameData.Utilities.RawDataPool dataPool) =>
        //    {
        //        NameRelatedData list = new NameRelatedData();
        //        Serializer.Deserialize(dataPool, offset, ref list);

        //        name = list.FullName.ToString();

        //    });
        //}



        public static void Test()
        {
            Debug.Log(CurCharacterId.ToString());

        }
    }
}
