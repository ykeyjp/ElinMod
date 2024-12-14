using UnityEngine;

namespace YK;

public interface ILayoutable
{
    RectTransform Transform { get; }
    Vector2 TLOffset { get; }
    Vector2 Size { get; }
}
