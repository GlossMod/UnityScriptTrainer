using System;
using System.Collections.Generic;
using System.IO;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;
using System.Collections;
//using ConfigurationManager;
using UnityEngine.Experimental.Rendering;
using System.Reflection;
using BepInEx.Logging;
using System.Text.RegularExpressions;

namespace ScriptTrainer
{

    [BepInPlugin("aoe.top.ScriptTrainer", "Dyson Script Trainer By:小莫", "3.0")]
    public class ScriptTrainer
    {
        public void Start()
        {
            Console.WriteLine("Dyson Script Trainer By:小莫");
        }
    }
}
