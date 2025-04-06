using HarmonyLib;
using UnityEngine;
using YKF;

namespace YKDev.Layers;

[HarmonyPatch]
public class WidgetThingGenerator
{
    [HarmonyPrefix, HarmonyPatch(typeof(WidgetCodex), nameof(WidgetCodex.OnActivate))]
    static void WidgetCodex_OnActivate(WidgetCodex __instance)
    {
        if (!IsEnabled) return;
        if (__instance.GetType() != typeof(WidgetCodex)) return;

        var layout = __instance.GetOrCreate<YKLayout>();

        layout.HeaderSmall("個数"._("Quantity"));
        layout.InputText("").WithName("Quantity").WithPlaceholder("個数"._("Quantity"));
        layout.HeaderSmall("素材"._("Material"));
        var materials = new List<string> { "デフォルト"._("Default") };
        materials.AddRange(EClass.sources.materials.rows.Select(x => x.GetName()));
        layout.Dropdown(materials).WithName("Material");
        layout.HeaderSmall("品質"._("Quality"));
        layout.Dropdown([.. Lang.GetList("quality")], null, 1).WithName("Quality");
        var blessedText = new List<string> { "堕落"._("Doomed"), "呪い"._("Cursed"), "通常"._("Normal"), "祝福"._("Blessed") };
        layout.HeaderSmall("祝福"._("Bless"));
        layout.Dropdown(blessedText, null, 2).WithName("Blessed");

        var content = __instance.Find("Content View").Rect();
        content.anchoredPosition = new Vector2(180, 0);
        content.sizeDelta = new Vector2(404, 375);
    }

    [HarmonyPostfix, HarmonyPatch(typeof(WidgetCodex), nameof(WidgetCodex.RefreshList))]
    static void WidgetCodex_RefreshList(WidgetCodex __instance)
    {
        if (!IsEnabled) return;
        if (__instance.GetType() != typeof(WidgetCodex)) return;

        var material = __instance.Find<UIDropdown>("Material");
        var quality = __instance.Find<UIDropdown>("Quality");
        var bleesed = __instance.Find<UIDropdown>("Blessed");
        var quantity = __instance.Find<UIInputText>("Quantity");

        if (__instance.list.callbacks is UIList.Callback<Recipe, ButtonGrid>)
        {
            if (__instance.list.callbacks is UIList.Callback<Recipe, ButtonGrid> callback)
            {
                callback.onClick = delegate (Recipe a, ButtonGrid b)
                {
                    var thing = ThingGen.Create(a.source.id, a.idMat);
                    if (thing != null)
                    {
                        thing.ChangeMaterial(material.value == 0 ? a.DefaultMaterial : EClass.sources.materials.rows[material.value - 1], false);
                        thing.ChangeRarity((Rarity)(quality.value - 1));
                        thing.SetBlessedState((BlessedState)(bleesed.value - 2));
                        thing.SetNum(Math.Max(quantity.Num, 1));
                        EClass._zone.AddCard(thing, EClass.pc.pos);
                    }
                };
            }
        }
    }

    [HarmonyPrefix, HarmonyPatch(typeof(WidgetCodex), nameof(WidgetCodex.Search))]
    static bool WidgetCodex_Search(WidgetCodex __instance, string s)
    {
        if (!IsEnabled) return true;
        if (__instance.GetType() != typeof(WidgetCodex)) return true;

        s = s.ToLower();
        __instance.buttonClear.SetActive(__instance.field.text != "");
        if (s == __instance.lastSearch)
        {
            return false;
        }

        RecipeManager.BuildList();
        HashSet<Recipe> hashSet;
        if (!s.IsEmpty())
        {
            hashSet = RecipeManager.list.Where(r => !r.isChara && (r.row.GetSearchName(false).Contains(s) || r.row.GetSearchName(true).Contains(s)))
                .Select(r => Recipe.Create(r, -1, null)).ToHashSet();
        }
        else
        {
            hashSet = RecipeManager.list.Where(r => !r.isChara).Select(r => Recipe.Create(r, -1, null)).ToHashSet();
        }
        if (!hashSet.SetEquals(__instance.recipes))
        {
            __instance.recipes = hashSet;
            __instance.RefreshList();
        }
        __instance.lastSearch = s;

        return false;
    }

    [HarmonyPostfix, HarmonyPatch(typeof(Widget), nameof(Widget.OnDeactivate))]
    static void Widget_OnDeactivate(Widget __instance)
    {
        if (__instance.GetType() != typeof(WidgetCodex)) return;
        IsEnabled = false;
    }

    public static bool IsEnabled = false;

    public static void Create()
    {
        var widget = EMono.ui.widgets.GetWidget("Codex");
        if (widget != null)
        {
            if (IsEnabled)
            {
                return;
            }
            EMono.ui.widgets.DeactivateWidget(widget);
        }
        IsEnabled = true;
        EMono.ui.widgets.ActivateWidget("Codex");
    }
}
