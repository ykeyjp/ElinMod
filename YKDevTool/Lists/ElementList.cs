using YKF;

namespace YKDev.Lists;

public class ElementList : YKList<Element, YKHorizontal>
{
    public override void OnLayout()
    {
        base.OnLayout();
        this.WithConstraintCount(2).WithCellSize(280, 50).WithSpace(20, 0);

        var callbacks = new UIList.Callback<Element, YKHorizontal>
        {
            onList = OnListHandler,
            onInstantiate = (e, l) =>
            {
                l.TextSmall(e.ShortName).WithName("name").WithWidth(100);
                l.InputText(e.vBase.ToString(), (i) => { SetBase(e, i); }).WithName("base").WithMinWidth(70);
                l.InputText(e.vPotential.ToString(), (i) => { SetPotential(e, i); }).WithName("potential").WithMinWidth(70);
            },
            onRedraw = (e, l, i) =>
            {
                var nameText = l.Find<UIText>("name");
                var baseInput = l.Find<UIInputText>("base");
                var potentialInput = l.Find<UIInputText>("potential");

                nameText.text = e.ShortName;
                baseInput.Num = e.vBase;
                baseInput.onValueChanged = (i) => { SetBase(e, i); };
                potentialInput.Num = e.vPotential;
                potentialInput.onValueChanged = (i) => { SetPotential(e, i); };
            },
            onRefresh = () => { }
        };

        var mold = Horizontal().WithLayerParent();
        callbacks.mold = mold;

        List.callbacks = callbacks;
    }

    private void SetBase(Element el, int value)
    {
        Container?.SetBase(el.id, value, el.vPotential);
    }

    private void SetPotential(Element el, int value)
    {
        Container?.SetBase(el.id, el.vBase, value);
    }

    public ElementContainer? Container { get; set; }
}
