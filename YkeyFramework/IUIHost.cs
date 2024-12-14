using UnityEngine;

namespace YK;

public interface IUIHost : IDisposable
{
    LayoutBase Layout { get; }
    Rect AllocatedRect { get; }
    RectTransform Transform { get; }
    float Height { get; }
    float Width { get; }
}

public interface IUIHost<T> : IUIHost, IDisposable
{
    T Host { get; }
}
