using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ScriptTrainer
{
    internal class MessageBox: MonoBehaviour
    {
        public static void Failed(string text)
        {
            DialogCmd cmd = new DialogCmd
            {
                Title = LocalStringManager.Get("LK_Failed"),
                Content = LocalStringManager.Get(text)
            };
            UIElement.Dialog.SetOnInitArgs(EasyPool.Get<ArgumentBox>().SetObject("Cmd", cmd));
            UIManager.Instance.ShowUI(UIElement.Dialog);
        }
    }
}
