using System;
using System.IO;

namespace ScriptTrainer
{
    public class Debug : UnityEngine.Debug
    {
        private static string logFile = $"{ModManager.GetModRootFolder()}//log.txt";

        public static void Log(string text)
        {
            // 写入日志
            // 格式 hh-mm-ss:log
            StreamWriter sw = File.AppendText(logFile);
            DateTime now = DateTime.Now;
            sw.WriteLine(String.Format("{0}-{1}-{2} {3}:{4}:{5}：{6}.", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, text));
            sw.Close();
        }
    }
}
