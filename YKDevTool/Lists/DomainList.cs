using YKF;

namespace YKDev.Lists;

public class DomainList : YKList<SourceElement.Row, YKHorizontal>
{
    public override void OnLayout()
    {
        base.OnLayout();
        this.WithConstraintCount(4).WithCellSize(230, 50).WithSpace(10, 0);

        var callbacks = new UIList.Callback<SourceElement.Row, YKHorizontal>
        {
            onList = OnListHandler,
            onInstantiate = (e, l) =>
            {
                l.Toggle(e.GetName(), IsActive(e.id), (b) => { SetActive(e.id, b); }).WithName("toggle").WithMinWidth(200);
            },
            onRedraw = (e, l, i) =>
            {
                var toggle = l.Find<UIButton>("toggle");

                toggle.SetToggle(IsActive(e.id), (b) => { SetActive(e.id, b); });
                toggle.mainText.text = e.GetName();
            },
            onRefresh = () => { }
        };

        var mold = Horizontal().WithLayerParent();
        callbacks.mold = mold;

        List.callbacks = callbacks;
    }

    private bool IsActive(int id)
    {
        return EClass.player.domains.Contains(id);
    }

    private void SetActive(int id, bool isOn)
    {
        if (isOn)
        {
            if (!IsActive(id))
            {
                EClass.player.domains.Add(id);
            }
        }
        else
        {
            if (IsActive(id))
            {
                EClass.player.domains.Remove(id);
            }
        }
    }
}
