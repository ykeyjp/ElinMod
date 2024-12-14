using UnityEngine;
using UnityEngine.UI;

namespace YK;

public class LayoutH : LayoutBase
{
    public LayoutH(ILayoutable layout, Rect? rect = null) : base(layout, rect)
    {
    }

    public LayoutH(Window layout) : this(layout.Wrap(), null)
    {
    }

    protected override float GetElementSize(float width, float height)
    {
        return width;
    }

    protected override float GetRemainingSize()
    {
        return this.CurrentRect.width;
    }

    protected override Rect NextPart(float part)
    {
        return this.CurrentRect.LeftPartPixels(part);
    }

    protected override void Shrink(float size)
    {
        this.CurrentRect = this.CurrentRect.ShrinkLeft(size);
    }

    protected override void PrepareWindow()
    {
        this.Layoutable.Transform.gameObject.GetOrAddComonent<HorizontalLayoutGroup>().SetupLayoutGroup();
    }

    protected override void StretchElement(RectTransform element)
    {
        element.StretchY();
    }
}
