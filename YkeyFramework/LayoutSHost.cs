using UnityEngine;

namespace YK;

public readonly struct LayoutSHost : IUIHost<LayoutS>, IUIHost, IDisposable
{
    public LayoutSHost(RectTransform rect, LayoutS scroll)
    {
        this.Transform = rect;
        this.Host = scroll;
    }

    public LayoutBase Layout
    {
        get
        {
            return this.Host.ParentLayout!;
        }
    }

    public Rect AllocatedRect
    {
        get
        {
            return this.Transform.rect;
        }
    }

    public RectTransform Transform { get; }

    public float Height
    {
        get
        {
            return this.AllocatedRect.height;
        }
    }

    public float Width
    {
        get
        {
            return this.AllocatedRect.width;
        }
    }

    public LayoutS Host { get; }

    public void Dispose()
    {
        this.Host.Dispose();
    }
}
