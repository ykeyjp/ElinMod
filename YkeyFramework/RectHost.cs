using UnityEngine;

namespace YK;

public readonly struct RectHost : ILayoutable
{
    public RectHost(RectTransform transform)
    {
        this.Transform = transform;
    }

    public Vector2 Size { get => this.Transform.sizeDelta; }
    public Vector2 TLOffset { get => default; }
    public RectTransform Transform { get; }
}
