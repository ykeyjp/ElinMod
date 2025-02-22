﻿using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace YkeyCraftIntegration.Patches
{
    public class PatchLayerCraft
    {
        public static List<Thing> factories = [];
        public static bool IsIntegration = false;

        [HarmonyPrefix, HarmonyPatch(typeof(LayerCraft), nameof(LayerCraft.SetFactory))]
        public static void LayerCraft_SetFactory_Prefix(LayerCraft __instance, ref Thing t)
        {
            if (t != null)
            {
                IsIntegration = false;
                return;
            }
            IsIntegration = true;

            factories = EClass._map.things.Where(t2 => t2.trait is TraitFactory).ToList();
            if (factories.Count == 0)
            {
                IsIntegration = false;
                return;
            }


            t = new YKLinkedFactory();
            try
            {
                var rectTab = __instance.Find("Window").Find("Before Tab").Find("Tab (1)").Rect();

                var vertical = rectTab.gameObject.GetComponent<VerticalLayoutGroup>();
                vertical.SetActive(false);
                UnityEngine.Object.DestroyImmediate(vertical);

                var grid = rectTab.gameObject.AddComponent<GridLayoutGroup>();
                grid.startAxis = GridLayoutGroup.Axis.Vertical;
                grid.constraint = GridLayoutGroup.Constraint.FixedRowCount;
                grid.constraintCount = 14;
                grid.childAlignment = TextAnchor.UpperRight;
                grid.startCorner = GridLayoutGroup.Corner.UpperRight;
                grid.spacing = new Vector2(-20, -15);
                grid.cellSize = new Vector2(160, 50);

                var layout = rectTab.gameObject.GetComponent<LayoutElement>();
                layout.flexibleWidth = 1;
            }
            catch (Exception e)
            {
                Debug.Log("[YK CI] " + e.Message);
            }
        }

        [HarmonyPrefix, HarmonyPatch(typeof(LayerCraft), nameof(LayerCraft.OnClickCraft))]
        public static void LayerCraft_OnClickCraft_Prefix(out Thing __state, LayerCraft __instance)
        {
            __state = __instance.factory;
            if (!IsIntegration)
            {
                return;
            }

            foreach (var t in factories)
            {
                if (t.trait is TraitFactory f && f.Contains(__instance.recipe.source))
                {
                    Debug.Log("use factory: " + t.GetName(NameStyle.Full));
                    t.c_charges = f.FuelCost * __instance.inputNum.Num;
                    __instance.factory = t;
                    return;
                }
            }

            __instance.factory = null;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(LayerCraft), nameof(LayerCraft.OnClickCraft))]
        public static void LayerCraft_OnClickCraft_Postfix(Thing __state, LayerCraft __instance)
        {
            if (!IsIntegration)
            {
                return;
            }

            __instance.factory = __state;
        }
    }
}
