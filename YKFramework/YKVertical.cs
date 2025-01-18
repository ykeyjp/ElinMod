using UnityEngine.UI;

namespace YKF;

public class YKVertical : YKLayout
{
    public override void OnLayout()
    {
        var group = gameObject.AddComponent<VerticalLayoutGroup>();
        group.childControlHeight = false;
        group.childForceExpandHeight = false;
        group.childControlWidth = true;
        group.childForceExpandWidth = true;
        group.childAlignment = UnityEngine.TextAnchor.MiddleLeft;
        _layout = group;

        var fitter = gameObject.AddComponent<ContentSizeFitter>();
        fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
        fitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;
        _fitter = fitter;
    }

    protected VerticalLayoutGroup? _layout;
    public VerticalLayoutGroup Layout
    {
        get { return _layout!; }
    }

    protected ContentSizeFitter? _fitter;
    public ContentSizeFitter Fitter
    {
        get { return _fitter!; }
    }
}
