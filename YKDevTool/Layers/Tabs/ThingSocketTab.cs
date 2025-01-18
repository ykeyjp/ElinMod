using UnityEngine.UI;
using YKDev.Lists;
using YKF;

namespace YKDev.Layers.Tabs;

public class ThingSocketTab : YKLayout<Thing>
{
    public override void OnLayout()
    {
        var thing = Layer.Data;
        var headerWidth = 120;
        Header(thing.GetName(NameStyle.Full));

        if (thing.trait is not TraitToolRange)
        {
            Text("ソケットに対応していません"._("Sockets are not supported")).WithWidth(300);
            return;
        }

        var skillList = EClass.sources.elements.rows.Where(e => e.tag.Contains("modRanged")).ToArray();

        {
            var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
            group.HeaderSmall("容量"._("Capacity")).WithMinWidth(headerWidth);
            group.Button("追加"._("Add"), () =>
            {
                thing.AddSocket();
                RefreshSocketList();
            }).WithWidth(120);
            group.Button("空っぽ"._("Empty"), () =>
            {
                for (var i = 0; i < thing.sockets.Count; i++)
                {
                    var sock = thing.sockets[i];
                    var id = sock / 100;
                    var lv = sock % 100;

                    thing.elements.ModBase(id, -lv);
                    thing.sockets[i] = 0;
                }
                RefreshSocketList();
            }).WithWidth(120);
            group.Button("すべて削除"._("Remove All"), () =>
            {
                for (var i = 0; i < thing.sockets.Count; i++)
                {
                    var sock = thing.sockets[i];
                    var id = sock / 100;
                    var lv = sock % 100;

                    thing.elements.ModBase(id, -lv);
                    thing.sockets[i] = 0;
                }
                thing.sockets = [];
                RefreshSocketList();
            }).WithWidth(120);
        }
        // 改造
        {
            var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
            group.HeaderSmall("改造"._("Mod")).WithMinWidth(headerWidth);
            var dropdown = group.Dropdown(skillList.Select(x => x.GetName()).ToList()).WithWidth(150);
            var baseInput = group.InputText("").WithPlaceholder("ベース"._("Base"));
            group.Button("取得"._("Gain"), () =>
            {
                var el = skillList[dropdown.value];
                var lv = Math.Max(baseInput.Num, 1);
                thing.ApplySocket(el.id, lv);
                RefreshSocketList();
            });
        }

        // 現在値
        {
            Header("ソケット"._("Socket"));
            _socketList = Create<SocketList>();
            _socketList.Thing = thing;
            _socketList.OnList = (m) => thing.sockets.Select(x => new SocketList.Socket { Id = x / 100, Lv = x % 100 }).ToList();
            _socketList.OnRemove = RefreshSocketList;

            RefreshSocketList();
        }
    }

    private SocketList? _socketList;

    private void RefreshSocketList()
    {
        _socketList?.Refresh();
    }
}
