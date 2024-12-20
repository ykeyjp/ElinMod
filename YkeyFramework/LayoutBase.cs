
using UnityEngine;
using UnityEngine.UI;

namespace YK;

public abstract class LayoutBase : IDisposable, IUIHost<LayoutBase>, IUIHost
{
    public LayoutBase? ParentLayout;
    public ILayoutable Layoutable;
    public float HSpace;
    public float Space;
    public Rect OriginalRect;
    public Rect CurrentRect;
    public List<IUIHost> Hosts = [];
    public List<LayoutBase> ChildLayouts = [];

    public LayoutBase(ILayoutable layout, Rect? rect = null)
    {
        this.Layoutable = layout;
        var size = layout.Size;
        this.CurrentRect = rect ?? new Rect(0f, 0f, size.x, size.y);
        this.OriginalRect = this.CurrentRect;
        this.PrepareWindow();
    }

    public LayoutBase(Window window, Rect? rect = null)
    {
        this.CurrentRect = rect ?? new Rect(0f, 0f, window.setting.bound.width, window.setting.bound.height);
        this.OriginalRect = this.CurrentRect;
        this.Layoutable = new WindowHost(window, this.CurrentRect);
        this.PrepareWindow();
    }

    protected abstract void PrepareWindow();
    protected abstract void Shrink(float size);
    protected abstract Rect NextPart(float part);
    protected abstract float GetRemainingSize();
    protected abstract void StretchElement(RectTransform element);
    protected abstract float GetElementSize(float width, float height);

    public virtual T AddElement<T>(T element) where T : IUIHost
    {
        element.Transform.SetParent(this.Layoutable.Transform);
        this.Hosts.Add(element);
        return element;
    }

    public LayoutBase AddChildLayout(LayoutBase child)
    {
        this.ChildLayouts.Add(child);
        child.ParentLayout = this;
        return child;
    }

    public void Clear()
    {
        foreach (var h in this.Hosts) h.Dispose();
        this.Hosts.Clear();
        foreach (var c in this.ChildLayouts) c.Dispose();
        this.CurrentRect = this.OriginalRect;
    }


    public void Dispose()
    {
        this.Clear();
        if (this.ParentLayout != null)
        {
            this.ParentLayout.ChildLayouts.Remove(this);
        }
        this.ParentLayout = null;
        this.Layoutable.Transform.SetParent(null);
        GameObject.Destroy(this.Layoutable.Transform.gameObject);
    }

    LayoutBase IUIHost.Layout { get { return this.ParentLayout!; } }
    Rect IUIHost.AllocatedRect { get { return this.OriginalRect; } }
    RectTransform IUIHost.Transform { get { return this.Layoutable.Transform; } }
    float IUIHost.Height { get { return this.OriginalRect.height; } }
    float IUIHost.Width { get { return this.OriginalRect.width; } }
    LayoutBase IUIHost<LayoutBase>.Host { get => this; }

    /**
    *** レイアウト・コントロールの実装
    **/

    public LayoutS Scroll(float? height = null)
    {
        height ??= this.GetRemainingSize();

        var value2 = this.NextPart(height.Value);
        this.Shrink(height.Value);
        var rectt = YUI.GenerateTransform(null, null);
        rectt.sizeDelta = value2.size;

        var res = YUI.GetResource<UIScrollView>();
        var rectt2 = res.Rect();
        rectt2.Zero();
        rectt2.sizeDelta = value2.size + new Vector2(0f, 30f);
        rectt2.SetParent(rectt);
        foreach (var uiheader in res.GetComponentsInChildren<UIHeader>(true))
        {
            uiheader.SetActive(false);
        }
        rectt2.AddPositionY(30f);

        res.content.DestroyAllChildren();
        res.content.Zero();
        res.content.sizeDelta = default;

        return this.AddElement(new LayoutSHost(rectt, new LayoutS(new LayoutSRectHost(res), new Rect?(value2)))).Host;
    }

    public LayoutV Vertical(float? size = null)
    {
        size ??= this.GetRemainingSize();

        var value2 = this.NextPart(size.Value);
        this.Shrink(size.Value);
        var rectt = YUI.GenerateTransform(null, null);
        rectt.sizeDelta = value2.size;
        return this.AddElement(new LayoutV(new RectHost(rectt), new Rect?(value2)));
    }

    public LayoutH Horizontal(float? size = null)
    {
        size ??= this.GetRemainingSize();

        var value2 = this.NextPart(size.Value);
        this.Shrink(size.Value);
        var rectt = YUI.GenerateTransform(null, null);
        rectt.sizeDelta = value2.size;
        return this.AddElement(new LayoutH(new RectHost(rectt), new Rect?(value2)));
    }

    public LayoutG Grid(float? size = null)
    {
        size ??= this.GetRemainingSize();

        var value2 = this.NextPart(size.Value);
        this.Shrink(size.Value);
        var rectt = YUI.GenerateTransform(null, null);
        rectt.sizeDelta = value2.size;
        return this.AddElement(new LayoutG(new RectHost(rectt), new Rect?(value2)));
    }

    public UIHost<LayoutH> SliderLabel<TValue>(string label, int index, IList<TValue> list, Action<int, TValue> onChange, Func<TValue, string>? getInfo = null)
    {
        var hl = this.Horizontal(new float?(this.GetElementSize(100f + this.Space + 160f, Mathf.Max(20f, 60f))));
        var currentRect = hl.CurrentRect;
        hl.Text(label, null);
        hl.Slider(index, list, onChange, getInfo);
        return new UIHost<LayoutH>(this, hl, hl.Layoutable.Transform, currentRect);
    }

    public UIHost<UISlider> Slider<TValue>(int index, IList<TValue> list, Action<int, TValue> onChange, Func<TValue, string>? getInfo = null)
    {
        var num = this.GetElementSize(160f, 60f) + this.Space;
        var rect = this.NextPart(num);
        var rectt = YUI.GenerateTransform(null, null);
        var uislider = YUI.MakeSlider(rectt);
        uislider.SetList(index, list, onChange, getInfo);
        rectt.sizeDelta = rect.size;
        this.StretchElement(uislider.transform.parent.Rect());
        this.Shrink(num);

        uislider.textMain.text = "";
        uislider.textInfo.text = "";

        return this.AddElement(new UIHost<UISlider>(this, uislider, rectt, rect));
    }

    public UIHost<Slider> Slider(Func<float> getvalue, Action<float> setvalue, float min, float max, Func<float, string>? labelfunc = null, float? width = null)
    {
        var num = this.GetElementSize(110f, 60f) + this.Space;
        var rect = this.NextPart(num);
        var rectt = YUI.GenerateTransform(null, null);
        rectt.sizeDelta = rect.size;
        var slider = UnityEngine.Object.Instantiate<Slider>(YUI.RawSlider);
        var rectt2 = slider.Rect();
        rectt2.Zero();
        rectt2.sizeDelta = new Vector2(width ?? rect.size.x, rectt2.sizeDelta.y);
        rectt2.AddPositionY(-20f);
        rectt2.SetParent(rectt);
        var value = getvalue();
        slider.SetSlider(value, delegate (float e)
        {
            Func<float, string>? labelfunc2 = labelfunc;
            var result = ((labelfunc2 != null) ? labelfunc2(e) : null) ?? e.ToString();
            setvalue(e);
            return result;
        }, (int)min, (int)max, true);
        this.Shrink(num);

        return this.AddElement(new UIHost<Slider>(this, slider, rectt, rect));
    }

    public UIHost<UIText> Text(string text, Action<UIText>? modify = null, float? width = null)
    {
        var rectt = YUI.GenerateTransform(null, null);

        var uitext = YUI.MakeText(text, modify, rectt);
        uitext.alignment = TextAnchor.UpperLeft;
        var uirect = uitext.rectTransform;
        var num = this.GetElementSize(200f, 30f);
        var rect = this.NextPart(num);
        if (width != null)
        {
            rect.size = new Vector2(width.Value, rect.size.y);
        }
        rectt.sizeDelta = rect.size;
        this.Shrink(num);
        uirect.ResetPosition();
        uirect.sizeDelta = rect.size;

        return this.AddElement(new UIHost<UIText>(this, uitext, rectt, rect));
    }

    public UIHost<UIButton> Button(string text, Action<UIButton> action, float width = 110f)
    {
        var row = this.ButtonRow();
        return row.AddButton2(text, action, width);
    }

    public UIHost<UIButton> Toggle(string text, Action<bool> toggle, bool isOn = false)
    {
        return this.Toggle(text, toggle, () => isOn);
    }

    public UIHost<UIButton> Toggle(string text, Action<bool> toggle, Func<bool> isOn)
    {
        var num = this.GetElementSize(110f, 35f) + this.Space;
        var rect = this.NextPart(num);
        var rectt = YUI.GenerateTransform(null, null);
        var res = YUI.GetResourceDirect<UIButton>("ButtonToggle");
        res.mainText.SetText(text);
        res.SetToggle(isOn(), delegate (bool b)
        {
            toggle(b);
        });
        res.transform.SetParent(rectt);
        var rectt2 = res.Rect();
        rectt2.Zero();
        rectt2.sizeDelta = rect.size;
        rectt2.StretchY();
        rectt.sizeDelta = rect.size;
        this.Shrink(num);

        return this.AddElement(new UIHost<UIButton>(this, res, rectt, rect));
    }

    public UIHost<UIDropdown> Dropdown()
    {
        var dropdown = Util.Instantiate("UI/Element/Input/DropdownDefault");
        var dp = dropdown.GetComponent<UIDropdown>();
        var text = dropdown.Find("Label").GetComponent<Text>();
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.verticalOverflow = VerticalWrapMode.Truncate;

        var num = this.GetElementSize(110f, 35f) + this.Space;
        var rect = this.NextPart(num);
        var rectt = YUI.GenerateTransform(null, null);

        dropdown.transform.SetParent(rectt);
        var rectt2 = dropdown.Rect();
        rectt2.Zero();
        rectt2.sizeDelta = rect.size;
        rectt2.StretchY();
        rectt.sizeDelta = rect.size;
        this.Shrink(num);

        dp.ClearOptions();

        return this.AddElement(new UIHost<UIDropdown>(this, dp, rectt, rect));
    }

    public LayoutButtons ButtonRow()
    {
        var element = new LayoutButtons(this);
        float size = this.GetElementSize(110f, 40f) + this.Space;
        this.Shrink(size);
        return this.AddElement(element);
    }

    public UIHost<UIItem> Topic(string text, string? value = null)
    {
        var uiitem = Util.Instantiate<UIItem>("UI/Element/Text/TopicDefault");
        uiitem.text1.SetText(text.lang());
        uiitem.text2.SetText(value);
        uiitem.text2.SetActive(!value.IsEmpty());

        var num = this.GetElementSize(210f, 35f) + this.Space;
        var rect = this.NextPart(num);
        var rectt = YUI.GenerateTransform(null, null);

        uiitem.transform.SetParent(rectt);
        var rectt2 = uiitem.Rect();
        rectt2.Zero();
        rectt2.sizeDelta = rect.size;
        rectt2.StretchY();
        rectt.sizeDelta = rect.size;
        this.Shrink(num);

        return this.AddElement(new UIHost<UIItem>(this, uiitem, rectt, rect));
    }

    public UIHost<UIItem> Header(string text, Sprite? sprite = null, string headerType = "Note", float width = 210f)
    {
        var uiitem = Util.Instantiate<UIItem>("UI/Element/Header/Header" + headerType);
        uiitem.text1.SetText(text.lang());
        if (uiitem.image1)
        {
            if (sprite)
            {
                uiitem.image1.sprite = sprite;
                uiitem.image1.SetNativeSize();
            }
            else
            {
                uiitem.image1.SetActive(false);
            }
        }
        uiitem.Rect().ResetPosition();

        var num = this.GetElementSize(width, 35f) + this.Space;
        var rect = this.NextPart(num);
        var rectt = YUI.GenerateTransform(null, null);

        uiitem.transform.SetParent(rectt);
        var rectt2 = uiitem.Rect();
        rectt2.Zero();
        rectt2.sizeDelta = rect.size;
        rectt2.StretchY();
        rectt.sizeDelta = rect.size;
        this.Shrink(num);

        return this.AddElement(new UIHost<UIItem>(this, uiitem, rectt, rect));
    }

    public UIHost<UIInputText> InputText(string? value = null, Action<UIInputText>? onchange = null, float width = 90f)
    {
        var uiinput = YUI.MakeInputField();
        if (value != null) uiinput.Text = value;
        if (onchange != null) uiinput.onValueChanged = (i) => { onchange(uiinput); };
        uiinput.max = int.MaxValue;

        var num = this.GetElementSize(width, -15f) + this.Space;
        var rect = this.NextPart(num);
        var rectt = YUI.GenerateTransform(null, null);

        uiinput.transform.SetParent(rectt);
        var rectt2 = uiinput.Rect();
        rectt2.Zero();
        rectt2.sizeDelta = rect.size;
        rectt2.StretchY();
        this.Shrink(num);
        rectt.sizeDelta = new Vector2(rectt.sizeDelta.x, 40);

        return this.AddElement(new UIHost<UIInputText>(this, uiinput, rectt, rect));
    }

    public UIHost<LayoutElement> Spacer(int sizeY = 0, int sizeX = 1)
    {
        var rectTransform = Util.Instantiate<Transform>("UI/Element/Deco/Space").Rect();
        rectTransform.sizeDelta = (sizeY == 0) ? default : new Vector2(sizeX, sizeY);

        var num = this.GetElementSize(90f, -15f) + this.Space;
        var rect = this.NextPart(num);
        var rectt = YUI.GenerateTransform(null, null);

        var le = rectTransform.GetComponent<LayoutElement>();
        if (sizeX != 1)
        {
            le.preferredWidth = sizeX;
        }
        le.transform.SetParent(rectt);
        rectt.sizeDelta = rectTransform.sizeDelta;

        return this.AddElement(new UIHost<LayoutElement>(this, le, rectTransform, rect));
    }
}
