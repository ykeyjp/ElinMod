using Mosframe;
using UnityEngine;

namespace YK;

public readonly struct WindowHost : ILayoutable
{
    private static Vector2 tlOffset = new(30f, 40f);
    private static Vector2 bottomExtra = new(0f, 30f);
    public readonly Window Window;

    public WindowHost(Window window, Rect rect)
    {
        this.Window = window;

        GameObject.Destroy(window.Find("Content View").gameObject);
        var rectTransform = YUI.GenerateTransform(window.transform, null);

        rectTransform.sizeDelta = rect.size;
        rectTransform.position = (rect.size * -0.5f).InverseY(0f) + tlOffset.InverseY(0f);
        rectTransform.sizeDelta -= tlOffset * 2f + bottomExtra;
        this.Transform = rectTransform;
    }

    public Vector2 Size
    {
        get
        {
            var setting = this.Window.setting;
            return (((setting != null) ? new Vector2?(setting.bound.size) : null) - TLOffset * 2f + bottomExtra) ?? this.Transform.getSize();
        }
    }

    public Vector2 TLOffset
    {
        get
        {
            var num = 0f;
            if (!string.IsNullOrEmpty(this.Window.setting.textCaption))
            {
                num += 15f;
            }
            if (this.Window.TryGetHeader(out UIHeader? uiheader) && uiheader != null && uiheader.isActiveAndEnabled)
            {
                num += 20f;
            }
            return new Vector2(tlOffset.x, tlOffset.y + num);
        }
    }

    public RectTransform Transform { get; }
}
