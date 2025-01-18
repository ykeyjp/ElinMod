using UnityEngine;
using YKF;

namespace YKDev.Layers.Tabs;

public class ZoneTab : YKLayout<object>
{
    public override void OnLayout()
    {
        Header("拠点"._("Faction Branch"));
        GenerateZones(EClass.world.region.children.Cast<Zone>().Where((z) => z.IsPCFaction).ToArray());

        Header("町"._("Town"));
        GenerateZones(EClass.world.region.children.Cast<Zone>().Where((z) => z.IsTown).ToArray());

        Header("その他"._("Others"));
        GenerateZones(EClass.world.region.children.Cast<Zone>().Where((z) => !(z.IsPCFaction || z.IsTown || z.IsNefia)).ToArray());

        Header("ネフィア"._("Nefia"));
        GenerateZones2(EClass.world.region.children.Cast<Zone>().Where((z) => z.IsNefia).ToArray());
    }

    private void GenerateZones(Zone[] list)
    {
        var group = Grid();
        group.Layout.cellSize = new Vector2(180, 50);

        foreach (var zone in list)
        {
            group.Button(zone.NameWithDangerLevel, () =>
            {
                EClass.pc.MoveZone(zone, new ZoneTransition
                {
                    state = ZoneTransition.EnterState.Region,
                });
            });
        }
    }

    private void GenerateZones2(Zone[] list)
    {
        var group = Grid();
        group.Layout.cellSize = new Vector2(280, 50);
        group.Layout.constraintCount = 2;

        foreach (var zone in list)
        {
            group.Button(zone.Name + "(" + zone.DangerLv.ToString() + ")", () =>
            {
                EClass.pc.MoveZone(zone, new ZoneTransition
                {
                    state = ZoneTransition.EnterState.Region,
                });
            });
        }
    }
}
