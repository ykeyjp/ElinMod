using UnityEngine;
using UnityEngine.UI;

namespace YK;

public class LayoutV : LayoutBase
{
    public LayoutV(ILayoutable window, Rect? rect = null) : base(window, rect)
    {
    }

    public LayoutV(Window window) : this(window.Wrap(), null)
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
        var vl = this.Layoutable.Transform.gameObject.GetOrAddComonent<VerticalLayoutGroup>().SetupLayoutGroup();
        vl.spacing = 5;
    }

    protected override void StretchElement(RectTransform element)
    {
    }
}
