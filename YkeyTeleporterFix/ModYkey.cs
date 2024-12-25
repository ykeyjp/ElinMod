using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;

namespace YkeyTeleporterFix
{
    [BepInPlugin("jp.cmbc.mod.elin.ykey-teleporterfix", "YK Teleporter Fix", "1.0.0.0")]
    public class ModYkey : BaseUnityPlugin
    {
        public static ConfigEntry<bool>? IsNamedSort;
        public static ConfigEntry<bool>? IsSortDesc;

        private void Start()
        {
            IsNamedSort = Config.Bind("Order", "NamedSort", false, "Sort by zone name");
            IsSortDesc = Config.Bind("Order", "SortDesc", false, "Sort in descending order");

            Logger.LogInfo("Plugin ykey-teleporterfix is loaded!");
            Harmony.CreateAndPatchAll(typeof(Patches.PatchTeleporter));
        }
    }
}
