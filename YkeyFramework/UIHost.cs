
using UnityEngine;

namespace YK;

public readonly struct UIHost<T> : IUIHost<T>, IUIHost, IDisposable
{
    public UIHost(LayoutBase layout, T obj, RectTransform transform, Rect rect)
    {
        this.Host = obj;
        this.AllocatedRect = rect;
        this.Transform = transform;
        this.Layout = layout;
        this.Width = rect.width;
        this.Height = rect.height;
    }

    public LayoutBase Layout { get; }
    public Rect AllocatedRect { get; }
    public RectTransform Transform { get; }
    public float Width { get; }
    public float Height { get; }
    public T Host { get; }

    public void Dispose()
    {
        this.Transform.SetParent(null);
        GameObject.Destroy(this.Transform.gameObject);
    }
}
