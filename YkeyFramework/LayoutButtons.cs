using System;
using UnityEngine;
using UnityEngine.UI;

namespace YK;

public class LayoutButtons : IUIHost, IDisposable
{
    public const float DefaultButtonWidth = 110f;
    public List<UIHost<UIButton>> Buttons = new();
    public float Margin = 20f;

    public Rect Rect;
    public LayoutBase Layout { get; }
    public float Height { get => 40f; }
    public float Width { get => Buttons.Count * (110f + this.Margin); }
    public Rect AllocatedRect { get => this.Rect; }
    public RectTransform Transform { get; }

    public LayoutButtons(LayoutBase layout)
    {
        this.Layout = layout;
        this.Rect = layout.CurrentRect.TopPartPixels(this.Height);
        this.Transform = YUI.GenerateTransform(null, null);
        this.Transform.sizeDelta = this.AllocatedRect.size;
        var hl = this.Transform.gameObject.AddComponent<HorizontalLayoutGroup>().SetupLayoutGroup();
        var fitter = this.Transform.gameObject.AddComponent<ContentSizeFitter>().SetupFitter();
        hl.childControlHeight = true;
        hl.childForceExpandHeight = true;
    }

    public UIHost<UIButton> AddButton(string text, Action<UIButton> action, float width = 110f)
    {
        var rectt = YUI.GenerateTransform(null, null);
        rectt.sizeDelta = new Vector2(width, 40f);
        var b = YUI.MakeButton(text);
        b = b.WithPress(delegate { action?.Invoke(b); });

        var rectt2 = b.Rect();
        rectt2.sizeDelta = new Vector2(width, 40f);
        rectt2.SetParent(rectt);
        rectt2.StretchY();
        rectt2.AddPositionX(-15f);
        var num = width + this.Margin;
        var rect = this.Rect.LeftPartPixels(num);
        this.Rect = this.Rect.ShrinkLeft(num);

        rectt.SetParent(this.Transform);

        var uiref = new UIHost<UIButton>(this.Layout, b, rectt, rect);
        this.Buttons.Add(uiref);
        return uiref;
    }

    public UIHost<UIButton> AddButton2(string text, Action<UIButton>? action, float width = 110f)
    {
        var b = Util.Instantiate<UIButton>("UI/Element/Button/ButtonNote");
        b.mainText.text = text;
        b.onClick.AddListener(delegate () { action?.Invoke(b); });
        var rectt = b.gameObject.GetOrAddComonent<RectTransform>();
        rectt.sizeDelta = new Vector2(width, 40f);

        var le = b.gameObject.GetOrAddComonent<LayoutElement>();
        le.minHeight = 40;
        le.preferredHeight = 40;

        var rectt2 = b.Rect();
        rectt2.sizeDelta = new Vector2(width, 40f);
        rectt2.SetParent(rectt);
        rectt2.StretchY();
        var num = width + this.Margin;
        var rect = this.Rect.LeftPartPixels(num);
        this.Rect = this.Rect.ShrinkLeft(num);

        rectt.SetParent(this.Transform);

        var uiref = new UIHost<UIButton>(this.Layout, b, rectt, rect);
        this.Buttons.Add(uiref);

        var hl = b.transform.parent.gameObject.GetComponent<HorizontalLayoutGroup>();
        if (hl)
        {
            hl.childControlHeight = true;
            hl.childForceExpandHeight = false;
        }

        return uiref;
    }

    public void Dispose()
    {
        foreach (UIHost<UIButton> tsuiref in this.Buttons)
        {
            tsuiref.Dispose();
        }
    }
}
