using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace YkeyModConfigurationLite;

[BepInPlugin("jp.cmbc.mod.elin.ykey-modconfigurationlite", "YK Mod Configuration Lite", "1.0.0.0")]
public class ModYkey : BaseUnityPlugin
{
    private void Start()
    {
        Harmony.CreateAndPatchAll(typeof(Patcher));
        Logger.LogInfo("Plugin ykey-modconfigurationlite is loaded!");
    }
}
