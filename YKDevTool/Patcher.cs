using HarmonyLib;
using UnityEngine;
using YKDev.Layers;
using YKF;

namespace YKDev;

[HarmonyPatch]
public class Patcher
{
    [HarmonyPrefix, HarmonyPatch(typeof(UIContextMenu), nameof(UIContextMenu.Show), [])]
    public static void UIContextMenu_Show(UIContextMenu __instance)
    {
        if (EClass.scene.mouseTarget.card == null) return;

        if (__instance.name == "ContextInteraction(Clone)")
        {
            if (EClass.scene.mouseTarget.card is Chara chara)
            {
                var btn1 = __instance.AddButton("検査"._("Inspect"), () =>
                {
                    YK.CreateLayer<LayerCharaEditor, Chara>(chara);
                    __instance.Hide();
                }, true);
                return;
            }
            else if (EClass.scene.mouseTarget.card is Thing thing)
            {
                var btn1 = EClass.ui.contextMenu?.currentMenu.AddButton("検査"._("Inspect"), () =>
                {
                    YK.CreateLayer<LayerThingEditor, Thing>(thing);
                    __instance.Hide();
                }, true);
                return;
            }
        }
    }

    [HarmonyPrefix, HarmonyPatch(typeof(ActPlan), nameof(ActPlan.ShowContextMenu))]
    public static void ActPlan_ShowContextMenu(ActPlan __instance)
    {
        if (__instance.pos.Equals(EClass.pc.pos))
        {
            var act = new DynamicAct("検査"._("Inspect"), () =>
            {
                YK.CreateLayer<LayerCharaEditor, Chara>(EMono.pc);
                return true;
            });
            var list = __instance.list;
            var item = new ActPlan.Item
            {
                act = act
            };
            list.Add(item);
        }
    }

    [HarmonyPrefix, HarmonyPatch(typeof(ActPlan.Item), nameof(ActPlan.Item.Perform))]
    public static bool ActPlan_Item_Perform(ActPlan.Item __instance)
    {
        if (__instance.act is DynamicAct dynamicAct && dynamicAct.id == "検査"._("Inspect"))
        {
            dynamicAct.Perform();
            return false;
        }
        return true;
    }

    [HarmonyPostfix, HarmonyPatch(typeof(InvOwner), nameof(InvOwner.ShowContextMenu))]
    public static void InvOwner_ShowContextMenu(InvOwner __instance, ButtonGrid button)
    {
        if (button == null || button.card == null || button.card.Thing == null)
        {
            return;
        }

        var thing = button.card.Thing;
        var menu = EClass.ui.contextMenu?.currentMenu ?? EClass.ui.CreateContextMenuInteraction();
        if (menu == null) return;
        var btn1 = menu.AddButton("検査"._("Inspect"), () =>
        {
            YK.CreateLayer<LayerThingEditor, Thing>(thing);
            menu?.Hide();
        }, true);
        menu.Show();
    }

    [HarmonyPrefix, HarmonyPatch(typeof(InvOwner), nameof(InvOwner.AllowHold))]
    public static bool InvOwner_AllowHold(InvOwner __instance, ref bool __result)
    {
        __result = true;
        return false;
    }

    [HarmonyPostfix, HarmonyPatch(typeof(LayerQuestBoard), nameof(LayerQuestBoard.OnInit))]
    public static void LayerQuestBoard_OnInit(LayerQuestBoard __instance)
    {
        var button = __instance.gameObject.transform.Find("Window (1)")?.Find("Corner")?.Find("HangIcon Window")?.Find("pivot")?.Find("Chain")?.Find("HangIcon")?.gameObject?.GetComponent<UIButton>();
        button?.onClick.AddListener(() =>
        {
            switch (__instance.windows[0].idTab)
            {
                case 0:
                    ELayer._zone.UpdateQuests(true);
                    __instance.RefreshQuest();
                    break;
                case 1:
                    ELayer.Branch.UpdateReqruits(true);
                    __instance.RefreshHire();
                    break;
                case 2:
                    break;
            }
        });
    }

    [HarmonyPostfix, HarmonyPatch(typeof(HotItemContext), nameof(HotItemContext.Show))]
    public static void HotItemContext_Show(string id, Vector3 pos)
    {
        if (EClass.ui.contextMenu.currentMenu == null) return;
        if (id == "system")
        {
            var menu = EClass.ui.contextMenu.currentMenu.AddOrGetChild("tool");
            menu.AddButton("開発ツール"._("DevTool"), () =>
            {
                YK.CreateLayer<LayerTestMain>();
            }, true);
            menu.AddButton("レシピツール"._("Recipe Tool"), () =>
            {
                YK.CreateLayer<LayerRecipeTool>();
            }, true);
            menu.AddButton("アイテム生成ツール"._("Item Generator"), () =>
            {
                WidgetThingGenerator.Create();
            }, true);
        }
    }
}
