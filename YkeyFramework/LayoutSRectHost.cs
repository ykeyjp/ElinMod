using Mosframe;
using UnityEngine;
using UnityEngine.UI;

namespace YK;

public readonly struct LayoutSRectHost : ILayoutable
{
    public readonly UIScrollView Controller;

    public LayoutSRectHost(UIScrollView view)
    {
        this.Controller = view;
        this.Transform = view.content;
    }

    public Vector2 Size { get => this.Transform.getSize(); }
    public readonly Vector2 TLOffset { get => default; }
    public readonly RectTransform Transform { get; }
}
