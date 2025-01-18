using UnityEngine.UI;

namespace YKF;

public class YKGrid : YKLayout
{
    public override void OnLayout()
    {
        var group = gameObject.AddComponent<GridLayoutGroup>();
        group.childAlignment = UnityEngine.TextAnchor.MiddleLeft;
        group.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        group.constraintCount = 3;
        _layout = group;

        var fitter = gameObject.AddComponent<ContentSizeFitter>();
        fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        _fitter = fitter;
    }

    protected GridLayoutGroup? _layout;
    public GridLayoutGroup Layout
    {
        get { return _layout!; }
    }

    protected ContentSizeFitter? _fitter;
    public ContentSizeFitter Fitter
    {
        get { return _fitter!; }
    }
}
