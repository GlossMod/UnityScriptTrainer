using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;

namespace ScriptTrainer
{
    class Script: MonoBehaviour
    {
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

        public static float CheckIsFloat(string ItemText)
        {
            float newFloat = 0f;

            try
            {
                if (ItemText != null && ItemText.Length < 10 && ItemText.Length != 0)
                {
                    float.TryParse(ItemText, out float result);

                    newFloat = result;
                }
                else
                {
                    ItemText = newFloat.ToString();
                }
            }
            catch (Exception)
            {

                throw;
            }

            return newFloat;
        }

        private static float oldMaxHp;  // 100
        public static void ChangeMaxHp(float MaxHp, PlayerAnimControl player)
        {
            // 101
            if ((oldMaxHp <= MaxHp - 5f && oldMaxHp < MaxHp) || (oldMaxHp >= MaxHp + 5f && oldMaxHp > MaxHp))
            {
                player.playerParameter.MAX_HP = MaxHp;

            }
            else
            {
                Debug.Log($"MaxHp是 {MaxHp};oldMaxHp是 {oldMaxHp}");
            }

            oldMaxHp = MaxHp;


        }
    }
}
