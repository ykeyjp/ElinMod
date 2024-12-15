using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace YkeyStatusBarExtra;

public class PatchWidgetStatusBar
{
    [HarmonyPostfix, HarmonyPatch(typeof(WidgetStatsBar), nameof(WidgetStatsBar.OnSetContextMenu))]
    public static void WidgetStatsBar_OnSetContextMenuPostfix(WidgetStatsBar __instance, UIContextMenu m)
    {
        UIContextMenu uicontextMenu = m.AddOrGetChild("setting");
        var extra = __instance.extra as YKWidgetStatusBarExtra;
        uicontextMenu.AddToggle("popu", extra != null && extra.population, delegate (bool a)
        {
            var extra = ReplaceExtra(__instance);
            extra.population = a;
            RestoreExtra(__instance);
            __instance.Build();
        });
        uicontextMenu.AddToggle("pasture", extra != null && extra.pasture, delegate (bool a)
        {
            var extra = ReplaceExtra(__instance);
            extra.pasture = a;
            RestoreExtra(__instance);
            __instance.Build();
        });
    }

    [HarmonyPostfix, HarmonyPatch(typeof(WidgetStatsBar), nameof(WidgetStatsBar.Build))]
    public static void WidgetStatsBar_Build(WidgetStatsBar __instance)
    {
        var extra = __instance.extra as YKWidgetStatusBarExtra;
        if (extra == null) return;
        if (extra.population)
        {
            __instance.Add(
                null,
                "popu",
                SpriteSheet.Get("icon_LayerPeople"),
                () =>
                {
                    if (EClass.Branch == null) return "";
                    var b = EClass.Branch;
                    return "  " + b.CountMembers(FactionMemberType.Default).ToString()
                        + " / " + b.MaxPopulation.ToString()
                        + " (" + b.CountMembers(FactionMemberType.Livestock).ToString()
                        + "|" + b.CountMembers(FactionMemberType.Slave).ToString() + ")";
                },
                delegate
                {
                    if (EClass.Branch == null) return FontColor.Default;
                    var b = EClass.Branch;
                    var max = b.MaxPopulation;
                    if (max <= 0) return FontColor.Bad;
                    var rate = b.CountMembers(FactionMemberType.Default) / max;
                    if (rate < 0.9) { return FontColor.Default; }
                    if (rate >= 1) { return FontColor.Bad; }
                    return FontColor.Warning;
                },
                delegate
                {
                    return EClass.Branch != null;
                });
        }
        if (extra.pasture)
        {
            __instance.Add(
                null,
                "pasture",
                SpriteSheet.Get("icon_menu_2"),
                () =>
                {
                    if (EClass.Branch == null) return "";
                    var b = EClass.Branch;
                    return "  " + b.CountPasture().ToString() + "(" + b.GetPastureCost().ToString() + "/d)";
                },
                delegate
                {
                    if (EClass.Branch == null) return FontColor.Default;
                    var b = EClass.Branch;
                    var max = b.CountPasture();
                    if (max <= 0) return FontColor.Bad;
                    var rate = b.GetPastureCost() / max;
                    if (rate < 0.9) { return FontColor.Default; }
                    if (rate >= 1) { return FontColor.Bad; }
                    return FontColor.Warning;
                },
                delegate
                {
                    return EClass.Branch != null;
                });
        }
    }

    private static YKWidgetStatusBarExtra ReplaceExtra(WidgetStatsBar bar)
    {
        if (bar.config.extra is YKWidgetStatusBarExtra extra)
        {
            return extra;
        }
        var _ex = (WidgetStatsBar.Extra)bar.config.extra;
        var ex = new YKWidgetStatusBarExtra
        {
            attributes = _ex.attributes,
            money = _ex.money,
            money2 = _ex.money2,
            plat = _ex.plat,
            medal = _ex.medal,
            karma = _ex.karma,
            mood = _ex.mood,
            weight = _ex.weight,
            influence = _ex.influence,
            maxAlly = _ex.maxAlly,
            hearthLv = _ex.hearthLv,
            tourism_value = _ex.tourism_value,
            fame = _ex.fame,
            dv = _ex.dv,
            fertility = _ex.fertility,
        };
        bar.config.extra = ex;
        return ex;
    }

    private static void RestoreExtra(WidgetStatsBar bar)
    {
        if (bar.config.extra is YKWidgetStatusBarExtra _ex)
        {
            if (_ex.population || _ex.pasture)
            {
                return;
            }
            else
            {
                var ex = new WidgetStatsBar.Extra
                {
                    attributes = _ex.attributes,
                    money = _ex.money,
                    money2 = _ex.money2,
                    plat = _ex.plat,
                    medal = _ex.medal,
                    karma = _ex.karma,
                    mood = _ex.mood,
                    weight = _ex.weight,
                    influence = _ex.influence,
                    maxAlly = _ex.maxAlly,
                    hearthLv = _ex.hearthLv,
                    tourism_value = _ex.tourism_value,
                    fame = _ex.fame,
                    dv = _ex.dv,
                    fertility = _ex.fertility,
                };
                bar.config.extra = ex;
            }
        }
    }
}

public class YKWidgetStatusBarExtra : WidgetStatsBar.Extra
{
    public bool population;
    public bool pasture;
}
