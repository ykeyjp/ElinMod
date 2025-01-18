using YKF;

namespace YKDev.Lists;

public class GeneList : YKList<GeneList.Gene, YKHorizontal>
{
    public override void OnLayout()
    {
        base.OnLayout();
        this.WithConstraintCount(2).WithCellSize(280, 50).WithSpace(20, 0);

        var callbacks = new UIList.Callback<Gene, YKHorizontal>
        {
            onList = OnListHandler,
            onInstantiate = (g, l) =>
            {
                var el = Element.Create(g.Id, g.Lv);
                l.TextSmall(el.FullName).WithName("name").WithWidth(100);
                l.InputText(g.Lv.ToString(), (i) => { SetLv(g.Id, i); }).WithName("lv").WithMinWidth(40);
                l.Button("削除"._("Del"), () => { Remove(g); }).WithName("del");
            },
            onRedraw = (g, l, i) =>
            {
                var el = Element.Create(g.Id, g.Lv);
                var nameText = l.Find<UIText>("name");
                var lvText = l.Find<UIInputText>("lv");
                var button = l.Find<UIButton>("del");

                nameText.text = el.FullName;
                lvText.Text = g.Lv.ToString();
                lvText.onValueChanged = (i) => { SetLv(g.Id, i); };
                button.SetOnClick(() => { Remove(g); });
            },
            onRefresh = () => { }
        };

        var mold = Horizontal().WithLayerParent();
        callbacks.mold = mold;

        List.callbacks = callbacks;
    }

    private void SetLv(int id, int lv)
    {
        if (Thing == null) return;

        for (var j = 0; j < Thing.c_DNA.vals.Count; j += 2)
        {
            if (Thing.c_DNA.vals[j] == id)
            {
                Thing.c_DNA.vals[j + 1] = lv;
                break;
            }
        }
    }

    private void Remove(Gene g)
    {
        if (Thing == null) return;

        var el = Element.Create(g.Id, g.Lv);
        for (var j = 0; j < Thing.c_DNA.vals.Count; j += 2)
        {
            if (Thing.c_DNA.vals[j] == el.id)
            {
                Thing.c_DNA.vals[j] = -1;
                Thing.c_DNA.vals[j + 1] = -1;
                break;
            }
        }
        Thing.c_DNA.vals = Thing.c_DNA.vals.Where(r => r != -1).ToList();
    }

    public Thing? Thing { get; set; }
    public Action? OnRemove { get; set; }

    public class Gene
    {
        public int Id { get; set; }
        public int Lv { get; set; }
    }
}
