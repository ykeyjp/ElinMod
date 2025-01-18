using UnityEngine.UI;

namespace YKF;

public class YKHorizontal : YKLayout
{
    public override void OnLayout()
    {
        var group = gameObject.AddComponent<HorizontalLayoutGroup>();
        group.childControlHeight = false;
        group.childForceExpandHeight = false;
        group.childControlWidth = true;
        group.childForceExpandWidth = false;
        group.childAlignment = UnityEngine.TextAnchor.MiddleLeft;
        _layout = group;

        var fitter = gameObject.AddComponent<ContentSizeFitter>();
        fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
        fitter.verticalFit = ContentSizeFitter.FitMode.MinSize;
        _fitter = fitter;
    }

    protected HorizontalLayoutGroup? _layout;
    public HorizontalLayoutGroup Layout
    {
        get { return _layout!; }
    }

    protected ContentSizeFitter? _fitter;
    public ContentSizeFitter Fitter
    {
        get { return _fitter!; }
    }
}
