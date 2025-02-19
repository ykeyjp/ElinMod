using UnityEngine;
using UnityEngine.UI;

namespace YKF;

public class YKScroll : YKLayout
{
    public override void OnLayout()
    {
        _scrollRect = YK.GetResource<ScrollRect>("Scrollview parchment with Header");
        var rect = _scrollRect.Rect();
        rect.SetParent(transform);

        var element = _scrollRect.gameObject.GetComponent<LayoutElement>();
        element.flexibleHeight = 10;

        var headerRect = (RectTransform)rect.Find("Header Top Parchment");
        headerRect.DestroyObject();

        var viewport = (RectTransform)rect.Find("Viewport");

        var content = (RectTransform)viewport.Find("Content");
        content.DestroyAllChildren();
        _contentTransform = content;

        var layout = content.gameObject.GetComponent<VerticalLayoutGroup>();
        layout.childControlHeight = false;
        layout.childForceExpandHeight = false;
        layout.childControlWidth = true;
        layout.childForceExpandWidth = true;
        _layout = layout;

        var fitter = content.gameObject.AddComponent<ContentSizeFitter>();
        fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
        fitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;
        _fitter = fitter;
    }

    protected ScrollRect? _scrollRect;
    public ScrollRect ScrollRect
    {
        get { return _scrollRect!; }
    }

    protected RectTransform? _contentTransform;
    public RectTransform ContentTransform
    {
        get { return _contentTransform!; }
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
