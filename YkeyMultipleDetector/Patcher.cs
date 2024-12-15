using HarmonyLib;
using UnityEngine;

namespace YkeyMultipleDetector;

[HarmonyPatch]
public class Patcher
{
    [HarmonyPrefix, HarmonyPatch(typeof(TraitDetector), nameof(TraitDetector.Search))]
    public static bool TraitDetector_Search(TraitDetector __instance)
    {
        if (__instance.term.IsEmpty() || (!__instance.term.Contains(" ") && !__instance.term.Contains(",")))
        {
            return true;
        }
        var term = __instance.owner.c_idRefName;

        int n = 0;
        foreach (var t in term.Split([' ', ',']))
        {
            Debug.Log("search: " + t);
            int dist;
            if ((dist = Search(__instance, t)) > -1)
            {
                var distText = Distance(dist);
                if (Lang.isJP) { Msg.SayRaw("[" + t + "]を探知した！(" + distText + ")"); n++; }
                else { Msg.SayRaw("[" + t + "] was detected!(" + distText + ")"); }

            }
        }

        if (n > 0) { EClass.pc.ShowEmo(Emo.idea); }

        return false;
    }

    public static int Search(TraitDetector trait, string term)
    {
        Card? card = null;
        int num = 999;
        foreach (Thing thing in EClass._map.things)
        {
            int num2 = EClass.pc.Dist(thing);
            if ((thing.id.ToLower().Contains(term.ToLower()) || thing.Name.ToLower().Contains(term.ToLower())) && num2 < num)
            {
                num = num2;
                card = thing;
            }
        }

        trait.interval = 10;
        if (card == null)
        {
            EClass.pc.PlaySound("detect_none");
            return -1;
        }
        else
        {
            EClass.pc.PlaySound("detect_" + ((num <= 1) ? "detected" : ((num < 5) ? "near" : ((num < 15) ? "medium" : ((num < 30) ? "far" : ((num < 50) ? "veryFar" : "superFar"))))));
            trait.interval = ((num <= 1) ? 1 : ((num < 5) ? 2 : ((num < 15) ? 4 : ((num < 30) ? 7 : 10))));
        }

        trait.owner.PlayAnime(AnimeID.HitObj);
        return num;
    }

    private static string Distance(int dist)
    {
        if (dist <= 1) return Lang.isJP ? "目前" : "detected";
        if (dist < 5) return Lang.isJP ? "近い" : "near";
        if (dist < 15) return Lang.isJP ? "そこそこ近い" : "medium";
        if (dist < 30) return Lang.isJP ? "遠い" : "far";
        if (dist < 50) return Lang.isJP ? "とても遠い" : "veryFar";
        return Lang.isJP ? "超遠い" : "superFar";
    }
}
