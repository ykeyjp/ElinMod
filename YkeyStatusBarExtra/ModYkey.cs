using BepInEx;
using HarmonyLib;

namespace YkeyStatusBarExtra;

[BepInPlugin("jp.cmbc.mod.elin.ykey-statusbarextra", "YK Status Bar Extra", "1.0.0.0")]
public class ModYkey : BaseUnityPlugin
{
    private void Start()
    {
        Logger.LogInfo("Plugin ykey-statusbarextra is loaded!");
        Harmony.CreateAndPatchAll(typeof(PatchWidgetStatusBar));
    }
}