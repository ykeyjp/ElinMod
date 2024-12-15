using HarmonyLib;

namespace YkeyTeleporterFix.Patches
{
    public class PatchTeleporter
    {
        [HarmonyPrefix, HarmonyPatch(typeof(TraitTeleporter), nameof(TraitTeleporter.TryTeleport))]
        public static bool TraitTeleporter_TryTeleport_Prefix(TraitTeleporter __instance, ref bool __result)
        {
            if (EClass._zone is Zone_Tent)
            {
                return true;
            }
            if (__instance.id.IsEmpty() && !__instance.GetParam(1, null).IsEmpty())
            {
                __result = false;
                return false;
            }
            if (__instance.teleportedTurn == EClass.pc.turn)
            {
                __instance.teleportedTurn = 0;
                __result = true;
                return false;
            }

            List<TraitTeleporter> list = new List<TraitTeleporter>();
            foreach (Thing thing in EClass._map.things)
            {
                TraitTeleporter? traitTeleporter = thing.trait as TraitTeleporter;
                if (traitTeleporter != null
                    && traitTeleporter != __instance
                    && traitTeleporter.owner.IsInstalled
                    && traitTeleporter.owner.pos.IsInBounds
                    && traitTeleporter.IsOn
                    && traitTeleporter.id == __instance.id)
                {
                    list.Add(traitTeleporter);
                }
            }
            if (list.Count > 0)
            {
                __result = true;
                return true;
            }

            var zones = GetTeleportZones(__instance);
            if (zones.Count == 1)
            {
                return true;
            }

            var menu = EClass.ui.CreateContextMenuInteraction();
            foreach (var z in zones)
            {
                menu.AddButton(z.Name, () =>
                {
                    EClass.pc.MoveZone(z, new ZoneTransition
                    {
                        state = __instance.enterState,
                        idTele = __instance.id.IsEmpty(__instance.GetParam(3, null))
                    });
                }, true);
            }
            menu.Show();
            __result = true;
            return false;
        }


        public static List<Zone> GetTeleportZones(TraitTeleporter t)
        {
            string id = t.id;
            int uid = t.owner.uid;
            if (id.IsEmpty())
            {
                return [];
            }
            List<Zone> list = new List<Zone>();
            foreach (KeyValuePair<int, TeleportManager.Item> keyValuePair in EClass.game.teleports.items)
            {
                if (keyValuePair.Key != uid && keyValuePair.Value.id == id)
                {
                    Zone zone = EClass.game.spatials.Find(keyValuePair.Value.uidZone);
                    if (zone != null && zone != EClass._zone && !(zone is Zone_Tent))
                    {
                        list.Add(zone);
                    }
                }
            }
            return list;
        }
    }
}
