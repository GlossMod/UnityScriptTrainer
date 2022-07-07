using BepInEx;
using BepInEx.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pathea;
using Pathea.ActorNs;
using Pathea.FrameworkNs;
using Pathea.Plants;
using Pathea.UISystemV2.UI;
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;



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
        
        public static long CheckIsLong(string ItemText)
        {
            long newLong = 0;
            ItemText = Regex.Replace(ItemText, @"[\-?][^0-9.]", "");
            try
            {
                if (ItemText != null && ItemText.Length < 20 && ItemText.Length != 0)
                {
                    newLong = Int64.Parse(ItemText);
                }
                else
                {
                    ItemText = newLong.ToString();
                }
            }
            catch (Exception) { throw; }

            return newLong;

        }

        // 设置玩家经验
        public static void SetPlayerExp(int newExp, int oldExp, Player player)
        {
            // 计算exp判断是增加exp还是减少exp
            int exp = newExp - oldExp;
            player.AddExp(exp, null, true);

        }
        // 设置玩家金钱
        public static void ChangeGold(long newGold,long oldGold, Player player)
        {
            long gold = newGold - oldGold;
            player.bag.ChangeGold((int)gold);
        }


        // 光标复位
        public static void CursorReset(bool change)
        {

            //CursorLockMode old = Cursor.lockState;
            //Cursor.lockState = CursorLockMode.None;
            //Cursor.lockState = old;
            if (change)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }


        // 联网检查版本更新
        public static bool CheckUpdates(out string text)
        {
            // 获取本地版本号
            float localVersion = constant.version;

            // 获取服务器版本号
            // 服务器地址 https://mod.3dmgame.com/mod/API/185597 json解析读取 mods_version 参数
            string url = "https://mod.3dmgame.com/mod/API/185597";

            //using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            //{
            //    text = "";
            //    yield return webRequest.SendWebRequest();

            //}

            string json = UnityWebRequest.Get(url).downloadHandler.text;
            // 解析json
            JObject jsonData = (JObject)JsonConvert.DeserializeObject(json);

            float serverVersion = (float)jsonData["mods_version"];

            // 比较版本号
            if (localVersion < serverVersion)
            {
                text = "有新版本可以更新！";
                return true;
            }
            else
            {
                text = "没有新版本可以更新！";
                return false;
            }











            // string serverVersion = "";
            // string url = "https://mod.3dmgame.com/mod/API/185597";
            // string json = "";
            // using (System.Net.WebClient client = new System.Net.WebClient())
            // {
            //     json = client.DownloadString(url);
            // }
            // if (json != "")
            // {
            //     JsonData jsonData = JsonMapper.ToObject(json);
            //     serverVersion = jsonData["mods_version"].ToString();
            // }
            // else
            // {
            //     text = "检查更新失败";
            //     return false;
            // }

            // // 比较版本号
            // if (localVersion >= serverVersion)
            // {
            //     text = "已是最新版本";
            //     return false;
            // }
            // else
            // {
            //     text = "有新版本可用";
            //     return true;
            // }
        }
    }
}