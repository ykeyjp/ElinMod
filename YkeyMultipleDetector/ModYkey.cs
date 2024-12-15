using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace YkeyMultipleDetector
{
    [BepInPlugin("jp.cmbc.mod.elin.ykey-multipledetector", "YK Multiple Detector", "1.0.0.0")]
    public class ModYkey : BaseUnityPlugin
    {
        public void Start()
        {
            Logger.LogInfo("Plugin ykey-multipledetector is loaded!");
            Harmony.CreateAndPatchAll(typeof(Patcher));
        }
    }
}
