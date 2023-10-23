using ScriptTrainer;
using BepInEx.Configuration;

public static class STConfig
{
    public const string PLUGIN_GUID = "com.github.3dmxm.ScriptTrainer";
    public const string PLUGIN_NAME = MyPluginInfo.PLUGIN_NAME;
    public const string PLUGIN_VERSION = MyPluginInfo.PLUGIN_VERSION;

    public static ConfigEntry<KeyboardShortcut> ShowCounter { get; set; }    // 启动按键

    public static bool OpenWindow
    {
        get; set;
    }
}