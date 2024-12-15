using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace YkeyDebugExtraInfo
{
    [BepInPlugin("jp.cmbc.mod.elin.ykey-debugextrainfo", "YK Debug Extra Info", "1.0.0.0")]
    public class ModYkey : BaseUnityPlugin
    {
        public void Start()
        {
            Logger.LogInfo("Plugin ykey-debugextrainfo is loaded!");
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Pause))
            {
                EClass.debug.showExtra = !EClass.debug.showExtra;
            }
        }
    }
}
