using HarmonyLib;
using UnityEngine;
using YK;

namespace YkeyModConfigurationLite;

public class Patcher
{
    [HarmonyPostfix, HarmonyPatch(typeof(HotItemContext), nameof(HotItemContext.Show))]
    public static void HotItemContext_Show(string id, Vector3 pos)
    {
        if (EClass.ui.contextMenu.currentMenu == null) return;
        if (id == "system")
        {
            var menu = EClass.ui.contextMenu.currentMenu.AddOrGetChild("tool");
            menu.AddButton(YUI._("YK Mod 設定", "YK Mod Config"), () =>
            {
                YUI.LoadUI<YKModConfigWindow, YKModConfigWindow.Args>(new YKModConfigWindow.Args { });
            }, true);
        }
    }
}
