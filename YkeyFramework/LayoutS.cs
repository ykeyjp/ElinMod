using UnityEngine;
using UnityEngine.UI;

namespace YK;

public class LayoutS : LayoutBase
{
    public LayoutS(ILayoutable layout, Rect? rect = null) : base(layout, rect)
    {
    }

    public LayoutS(Window layout) : this(layout.Wrap(), null)
    {
    }

    protected override float GetElementSize(float width, float height)
    {
        return height;
    }

    protected override float GetRemainingSize()
    {
        return this.CurrentRect.height;
    }

    protected override Rect NextPart(float part)
    {
        return this.CurrentRect.TopPartPixels(part);
    }

    protected override void Shrink(float size)
    {
        this.CurrentRect = this.CurrentRect.ShrinkTop(size);
    }

    protected override void PrepareWindow()
    {
        var go = this.Layoutable.Transform.gameObject;
        var vl = go.GetOrAddComonent<VerticalLayoutGroup>().SetupLayoutGroup();
        vl.spacing = 5;
        vl.padding = new RectOffset(5, 5, 3, 3);
        go.GetOrAddComonent<ContentSizeFitter>().SetupFitter();
    }

    protected override void StretchElement(RectTransform element)
    {
    }

    public void SetScrollPosition(float p)
    {
        if (this.Layoutable is LayoutSRectHost host)
        {
            host.Controller.verticalNormalizedPosition = p;
        }
    }
}
