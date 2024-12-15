using BepInEx;
using HarmonyLib;

namespace YkeyCraftIntegration
{
    [BepInPlugin("jp.cmbc.mod.elin.ykey-craftintegration", "YK Craft Integration", "1.0.0.0")]
    public class ModYkey : BaseUnityPlugin
    {
        private void Start()
        {
            Logger.LogInfo("Plugin ykey-craftintegration is loaded!");
            Harmony.CreateAndPatchAll(typeof(Patches.PatchLayerCraft));
        }
    }
}
