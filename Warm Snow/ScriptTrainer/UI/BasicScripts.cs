using System;
using UnityEngine;

namespace ScriptTrainer;
public class BasicScripts
{

    public static PlayerAnimControl player = PlayerAnimControl.instance;

    public BasicScripts(GameObject panel)
    {
        #region[添加功能按钮]
        Components.AddH3("常用功能", panel);
        {
            Components.AddInputField("灵魂", 150, player.Souls.ToString(), panel, (string text) =>
            {
                // player.Souls = int.Parse(text);
                player.SetSoul(int.Parse(text));
            });
            Components.AddInputField("红魂", 150, player.RedSouls.ToString(), panel, (string text) =>
            {
                player.RedSouls = int.Parse(text);
            });
            Components.AddInputField("移速", 150, player.playerParameter.RUN_SPEED_RATE.ToString(), panel, (string text) =>
            {
                player.playerParameter.RUN_SPEED_RATE = float.Parse(text);
            });
            Components.AddInputField("攻速", 150, player.playerParameter.EXTRA_ATTACK_SPEED.ToString(), panel, (string text) =>
            {
                player.playerParameter.EXTRA_ATTACK_SPEED = float.Parse(text);
            });

            Components.Hr();

            Components.AddInputField("生命值", 150, player.playerParameter.MAX_HP.ToString(), panel, (string text) =>
            {
                player.playerParameter.MAX_HP = float.Parse(text);
            });

            Components.Hr();
            Components.AddButton("满血", panel, () =>
            {
                player.playerParameter.HP = player.playerParameter.MAX_HP;
            });
            Components.AddButton("通用技能书", panel, () =>
            {
                SkillDropPool.instance.Pop(player.transform.position, false, true, false);
            });
            Components.AddButton("宗门技能书", panel, () =>
            {
                SkillDropPool.instance.Pop(player.transform.position, true, true, false);
            });
            Components.AddButton("噩梦技能书", panel, () =>
            {
                SkillDropPool.instance.Pop(player.transform.position, false, true, true);
            });
            Components.AddButton("获取法印", panel, () =>
            {
                NightmarePool.instance.Pop(NightmareMagicSwordPrefabType.SealDrop, true).transform.position = player.transform.position;
            });
        }

        #endregion

    }

}
