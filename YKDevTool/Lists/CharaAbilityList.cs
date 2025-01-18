using YKF;

namespace YKDev.Lists;

public class CharaAbilityList : YKList<ActList.Item, YKHorizontal>
{
    public override void OnLayout()
    {
        base.OnLayout();
        this.WithConstraintCount(2).WithCellSize(280, 50).WithSpace(20, 0);

        var callbacks = new UIList.Callback<ActList.Item, YKHorizontal>
        {
            onList = OnListHandler,
            onInstantiate = (a, l) =>
            {
                l.TextSmall(a.act.FullName).WithName("name").WithWidth(100);
                l.TextSmall(a.chance.ToString()).WithName("chance").WithMinWidth(40);
                l.TextSmall(a.pt ? "PT" : "").WithName("pt").WithMinWidth(40);
                l.Button("削除"._("Del"), () => { Remove(a); }).WithName("del");
            },
            onRedraw = (a, l, i) =>
            {
                var nameText = l.Find<UIText>("name");
                var chanceText = l.Find<UIText>("chance");
                var ptText = l.Find<UIText>("pt");
                var button = l.Find<UIButton>("del");

                nameText.text = a.act.FullName;
                chanceText.text = a.chance.ToString();
                ptText.text = a.pt ? "PT" : "";
                button.SetOnClick(() => { Remove(a); });
            },
            onRefresh = () => { }
        };

        var mold = Horizontal().WithLayerParent();
        callbacks.mold = mold;

        List.callbacks = callbacks;
    }

    private void Remove(ActList.Item a)
    {
        if (Ability != null)
        {
            Ability.Remove(a.act.id * (a.pt ? -1 : 1));
            Ability.Refresh();
            OnRemove?.Invoke();
        }
    }

    public CharaAbility? Ability { get; set; }
    public Action? OnRemove { get; set; }
}
