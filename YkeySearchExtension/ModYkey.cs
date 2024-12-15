using BepInEx;
using HarmonyLib;

namespace YkeySearchExtension
{
    [BepInPlugin("jp.cmbc.mod.elin.ykey-searchextension", "YK Search Extension", "1.0.0.0")]
    public class ModYkey : BaseUnityPlugin
    {
        private void Start()
        {
            Logger.LogInfo("Plugin ykey-searchextension is loaded!");
            Harmony.CreateAndPatchAll(typeof(Patches.PatchWidgetSearch));
            Harmony.CreateAndPatchAll(typeof(Patches.PatchThingContainer));
        }
    }
}
