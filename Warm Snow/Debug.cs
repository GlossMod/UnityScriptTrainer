using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace ScriptTrainer;

public class Debug
{

    public static void Log(string text)
    {
        Main.Logs(text);
    }
}