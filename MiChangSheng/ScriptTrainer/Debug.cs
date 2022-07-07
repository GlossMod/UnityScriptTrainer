using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptTrainer
{


    class Debug
    {
        public static void addLog(string text)
        {
            StreamWriter sw = File.AppendText("log.txt");

            DateTime now = DateTime.Now;

            //sw.WriteLine(String.Format("{0}:{1}:{2}：{3}.", now.Hour, now.Minute, now.Second, text));
            sw.WriteLine(String.Format("{0}-{1}-{2} {3}:{4}:{5}：{6}.", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, text));
            sw.Close();
        }

        public static void EmptyLog()
        {
            StreamWriter sw = new StreamWriter("log.txt");
            sw.WriteLine("");
            sw.Close();
        }

        public static void GetItem(string text)
        {
            StreamWriter sw = File.AppendText("ItemList.txt");
            sw.WriteLine(text);
            sw.Close();
        }
    }
}
