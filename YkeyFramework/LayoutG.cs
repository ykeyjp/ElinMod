using UnityEngine;
using UnityEngine.UI;

namespace YK;

public class LayoutG : LayoutBase
{
    public GridLayoutGroup Group;
    public UIItemList ItemList;
    public ContentSizeFitter Fitter;

    public LayoutG(ILayoutable layout, Rect? rect = null) : base(layout, rect)
    {
        var go = layout.Transform.gameObject;
        this.Group = go.GetOrAddComonent<GridLayoutGroup>();
        this.ItemList = go.GetOrAddComonent<UIItemList>();
        this.Fitter = go.GetOrAddComonent<ContentSizeFitter>();
    }

    public LayoutG(Window layout) : this(layout.Wrap(), null)
    {
        var go = layout.gameObject;
        this.Group = go.GetOrAddComonent<GridLayoutGroup>();
        this.ItemList = go.GetOrAddComonent<UIItemList>();
        this.Fitter = go.GetOrAddComonent<ContentSizeFitter>();
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
        if (this.Group == null || this.ItemList == null)
        {
            this.Group = go.GetOrAddComonent<GridLayoutGroup>();
            this.ItemList = go.GetOrAddComonent<UIItemList>();
            this.Fitter = go.GetOrAddComonent<ContentSizeFitter>();
        }

        var rect = go.GetComponent<RectTransform>();
        rect.SetAnchor(RectPosition.TopLEFT);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0);

        this.ItemList.gridLayout.cellSize = new Vector2(180f, 50f);
        this.ItemList.gridLayout.spacing = new Vector2(2f, 2f);
        this.ItemList.gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        this.ItemList.gridLayout.constraintCount = 2;
    }

    protected override void StretchElement(RectTransform element)
    {
    }

    public override T AddElement<T>(T element)
    {
        element.Transform.SetParent(this.Layoutable.Transform);
        this.Hosts.Add(element);
        this.ItemList.Add(element.Transform);
        return element;
    }
}
