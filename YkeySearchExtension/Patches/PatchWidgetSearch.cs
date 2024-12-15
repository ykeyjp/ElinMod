using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace YkeySearchExtension.Patches
{
    public class PatchWidgetSearch
    {
        public static bool onlyShop = false;

        [HarmonyPostfix, HarmonyPatch(typeof(WidgetSearch), nameof(WidgetSearch.OnActivate))]
        public static void WidgetSearch_OnActivate_Postfix(WidgetSearch __instance)
        {

            var rect = __instance.gameObject.GetComponent<RectTransform>();
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 400);
            rect.Find("Search Box").Find("ButtonGeneral").Rect().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 8, 30);

            var content = new GameObject(typeof(UIContent).Name, new Type[]
            {
                typeof(RectTransform)
            });
            content.transform.SetParent(rect);
            var group = content.AddComponent<HorizontalLayoutGroup>();
            group.childControlHeight = false;
            group.childForceExpandHeight = false;
            group.childAlignment = TextAnchor.MiddleLeft;
            var items = content.gameObject.AddComponent<UIItemList>();
            items.layoutItems.padding = new RectOffset(2, 2, 2, 2);

            var toggle = Util.Instantiate("UI/Element/Button/ButtonToggle", group.transform);
            toggle.name = "OnlyShop";
            var bb = toggle.GetComponent<UIButton>();
            bb.mainText.text = "SHOP";
            bb.SetToggle(onlyShop, (b) =>
            {
                onlyShop = b;
                var txt = __instance.lastSearch;
                __instance.Clear();
                __instance.Search(txt);
            });

            group.Rect().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 36);

            rect.Find("Content View").Rect().anchoredPosition = new Vector2(0, -94);
        }


        [HarmonyPrefix, HarmonyPatch(typeof(WidgetSearch), nameof(WidgetSearch.Search))]
        public static bool WidgetSearch_Search_Prefix(WidgetSearch __instance, string s)
        {
            if (!s.IsEmpty())
            {
                __instance.extra.lastSearch = s;
            }
            s = s.ToLower();
            __instance.buttonClear.SetActive(__instance.field.text != "");
            if (s == __instance.lastSearch)
            {
                return false;
            }
            SearchExtension? ext = null;
            if (!s.IsEmpty())
            {
                ext = new SearchExtension(s);
                ext.onlyShop = onlyShop;
                ext.Execute();
            }
            if (ext != null && !ext.foundCard.SetEquals(__instance.cards))
            {
                __instance.cards = ext.foundCard;
                __instance.RefreshList();
            }
            __instance.cgResult.alpha = ((__instance.list.ItemCount > 0) ? 1f : 0f);
            __instance.lastSearch = s;

            return false;
        }
    }
}
