using System.Collections.Generic;

namespace ScriptTrainer
{



    public class windowsStat
    {
        public string Key { get; set; }
        public bool Value { get; set; }
        public string Text { get; set; }

        public List<windowsStat> list;


    }


    public static class SetWindowsStatFun
    {
        /// <summary>
        /// 切换窗口状态
        /// </summary>
        /// <param name="key">设置的值</param>
        /// <param name="stat">设置的状态</param>
        public static void ChangeWindowStat<T>(this List<windowsStat> source, string key, bool stat)
        {
            foreach (var item in source)
            {
                if (item.Key == key)
                {
                    item.Value = stat;
                }
                else
                {
                    item.Value = false;
                }
            }
        }
        /// <summary>
        /// 获取窗口是否开启
        /// </summary>
        /// <returns></returns>
        public static bool GetWindowStat<T>(this List<windowsStat> source)
        {
            foreach (var item in source)
            {
                if (item.Value)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 获取单独窗口是否开启
        /// </summary>
        /// <param name="key">窗口key</param>
        /// <returns></returns>
        public static bool GetWindowStat<T>(this List<windowsStat> source, string key)
        {
            foreach (var item in source)
            {
                if (item.Key == key && item.Value)
                {
                    return true;
                }
            }
            return false;
        }
    }
}