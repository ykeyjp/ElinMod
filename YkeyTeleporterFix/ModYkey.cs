using BepInEx;
using HarmonyLib;

namespace YkeyTeleporterFix
{
    [BepInPlugin("jp.cmbc.mod.elin.ykey-teleporterfix", "YK Teleporter Fix", "1.0.0.0")]
    public class ModYkey : BaseUnityPlugin
    {
        private void Start()
        {
            Logger.LogInfo("Plugin ykey-teleporterfix is loaded!");
            Harmony.CreateAndPatchAll(typeof(Patches.PatchTeleporter));
        }
    }
}
