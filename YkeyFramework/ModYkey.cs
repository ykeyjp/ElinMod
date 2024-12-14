using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace YK
{
    [BepInPlugin("jp.cmbc.mod.elin.ykey-framework", "YK Framework", "1.0.0.0")]
    public class ModYkey : BaseUnityPlugin
    {
        private void Start()
        {
            Logger.LogInfo("Plugin [YK Framework] is loaded!");
        }
    }
}
