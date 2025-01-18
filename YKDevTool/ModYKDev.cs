using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;
using YKF;

namespace YKDev;

[BepInPlugin("jp.cmbc.mod.elin.yk-devtool", "YK DevTool", "1.0.0.0")]
public class ModYkey : BaseUnityPlugin
{
    private void Start()
    {
        KeyCodeOpenMainTool = Config.Bind("KeyConfig", "OpenMainTool", KeyCode.None, "open main tool");
        KeyCodeOpenRecipeTool = Config.Bind("KeyConfig", "OpenRecipeTool", KeyCode.None, "open recipe tool");
        KeyCodeOpenThingGeneratorTool = Config.Bind("KeyConfig", "OpenThingGeneratorTool", KeyCode.None, "open thing generator tool");

        Harmony.CreateAndPatchAll(typeof(Patcher));
        Harmony.CreateAndPatchAll(typeof(Layers.WidgetThingGenerator));
        Logger.LogInfo("Plugin [YK DevTool] is loaded!");
    }

    public static ConfigEntry<KeyCode>? KeyCodeOpenMainTool;
    public static ConfigEntry<KeyCode>? KeyCodeOpenRecipeTool;
    public static ConfigEntry<KeyCode>? KeyCodeOpenThingGeneratorTool;

    private void Update()
    {
        if (KeyCodeOpenMainTool != null && KeyCodeOpenMainTool.Value != KeyCode.None && Input.GetKeyUp(KeyCodeOpenMainTool.Value))
        {
            YK.CreateLayer<Layers.LayerTestMain>();
        }
        if (KeyCodeOpenRecipeTool != null && KeyCodeOpenRecipeTool.Value != KeyCode.None && Input.GetKeyUp(KeyCodeOpenRecipeTool.Value))
        {
            YK.CreateLayer<Layers.LayerRecipeTool>();
        }
        if (KeyCodeOpenThingGeneratorTool != null && KeyCodeOpenThingGeneratorTool.Value != KeyCode.None && Input.GetKeyUp(KeyCodeOpenThingGeneratorTool.Value))
        {
            Layers.WidgetThingGenerator.Create();
        }
    }
}
