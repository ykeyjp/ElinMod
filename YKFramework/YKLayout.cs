using UnityEngine;
using UnityEngine.UI;

namespace YKF;

public class YKLayout<T> : YKLayout
{
    protected YKLayer<T>? _layer;
    public YKLayer<T> Layer
    {
        get { return _layer!; }
        set { _layer = value; }
    }

    public override void OnSwitchContent(int idTab)
    {
        Build();
    }
}

public class YKLayout : UIContent
{
    public virtual void OnLayout() { }

    public RectTransform Spacer(int height, int width = 1)
    {
        var space = Util.Instantiate<Transform>("UI/Element/Deco/Space", layout);
        space.SetParent(transform);
        var rect = space.Rect();
        rect.sizeDelta = new Vector2(width, height);
        if (height != 1)
        {
            rect.LayoutElement().preferredHeight = height;
        }
        if (width != 1)
        {
            rect.LayoutElement().preferredWidth = width;
        }

        return rect;
    }

    public UIItem Header(string text, Sprite? sprite = null)
    {
        var item = AddHeader(text, sprite);
        item.transform.SetParent(transform);
        return item;
    }

    public UIItem HeaderCard(string text, Sprite? sprite = null)
    {
        var item = AddHeaderCard(text, sprite);
        item.transform.SetParent(transform);
        return item;
    }

    public UIItem HeaderSmall(string text, Sprite? sprite = null)
    {
        var item = AddHeader("HeaderNoteSmall", text, sprite);
        item.transform.SetParent(transform);
        return item;
    }

    public UIText Text(string text, FontColor color = FontColor.DontChange)
    {
        var item = AddText(text, color);
        item.transform.SetParent(transform);
        item.text1.horizontalOverflow = HorizontalWrapMode.Wrap;

        var element = item.GetOrCreate<LayoutElement>();
        element.minWidth = 80;

        return item.GetComponent<UIText>();
    }

    public UIText TextLong(string text, FontColor color = FontColor.DontChange)
    {
        var item = AddText("NoteText_long", text, color);
        item.transform.SetParent(transform);
        item.text1.horizontalOverflow = HorizontalWrapMode.Wrap;

        var element = item.GetOrCreate<LayoutElement>();
        element.minWidth = 80;

        return item.GetComponent<UIText>();
    }

    public UIText TextMedium(string text, FontColor color = FontColor.DontChange)
    {
        var item = AddText("NoteText_medium", text, color);
        item.transform.SetParent(transform);
        item.text1.horizontalOverflow = HorizontalWrapMode.Wrap;

        var element = item.GetOrCreate<LayoutElement>();
        element.minWidth = 80;

        return item.GetComponent<UIText>();
    }

    public UIText TextSmall(string text, FontColor color = FontColor.DontChange)
    {
        var item = AddText("NoteText_small", text, color);
        item.transform.SetParent(transform);
        item.text1.horizontalOverflow = HorizontalWrapMode.Wrap;

        var element = item.GetOrCreate<LayoutElement>();
        element.minWidth = 80;

        return item.GetComponent<UIText>();
    }

    public UIText TextFlavor(string text, FontColor color = FontColor.DontChange)
    {
        var item = AddText("NoteText_flavor", text, color);
        item.transform.SetParent(transform);

        var element = item.GetOrCreate<LayoutElement>();
        element.minWidth = 80;

        return item.GetComponent<UIText>();
    }

    public UIItem Topic(string text, string? value = null)
    {
        var item = AddTopic("TopicDefault", text, value);
        item.transform.SetParent(transform);
        return item;
    }

    public UIItem TopicAttribute(string text, string? value = null)
    {
        var item = AddTopic("TopicAttribute", text, value);
        item.transform.SetParent(transform);
        return item;
    }

    public UIItem TopicDomain(string text, string? value = null)
    {
        var item = AddTopic("TopicDomain", text, value);
        item.transform.SetParent(transform);
        return item;
    }

    public UIItem TopicLeft(string text, string? value = null)
    {
        var item = AddTopic("TopicLeft", text, value);
        item.transform.SetParent(transform);
        return item;
    }

    public UIItem TopicPair(string text, string? value = null)
    {
        var item = AddTopic("TopicPair", text, value);
        item.transform.SetParent(transform);
        return item;
    }

    public UIButton Button(string text, Action action)
    {
        var button = AddButton(text, () => { SE.ClickGeneral(); action(); });
        button.transform.SetParent(transform);

        var element = button.GetOrCreate<LayoutElement>();
        element.minWidth = 80;

        return button;
    }

    public UIButton Toggle(string text, bool isOn = false, Action<bool>? onClick = null)
    {
        var toggle = AddToggle(text, isOn, onClick);
        toggle.transform.SetParent(transform);
        return toggle;
    }

    public UIDropdown Dropdown(List<string>? list = null, Action<int>? action = null, int? value = null)
    {
        var dropdown = Util.Instantiate("UI/Element/Input/DropdownDefault", layout);
        dropdown.transform.SetParent(transform);

        var element = dropdown.GetOrCreate<LayoutElement>();
        element.preferredWidth = 100;

        var text = dropdown.Find("Label").GetComponent<Text>();
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.verticalOverflow = VerticalWrapMode.Truncate;

        var dp = dropdown.GetComponent<UIDropdown>();
        dp.options = list == null ? [] : list.ToDropdownOptions();

        if (value != null)
        {
            dp.value = value.Value;
        }

        if (action != null)
        {
            dp.onValueChanged.AddListener((i) => action(i));
        }

        return dp;
    }

    public UIInputText InputText(string text, Action<int>? onInput = null)
    {
        var input_transform = Util.Instantiate("UI/Element/Input/InputText", layout);
        var input_text_transform = input_transform.Find("InputField");
        input_text_transform.SetParent(transform);
        input_transform.DestroyObject();

        var input_text = input_text_transform.GetComponent<UIInputText>();
        input_text.Text = text;
        if (onInput != null)
        {
            input_text.onValueChanged = onInput;
        }
        input_text.Rect().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 36);

        var canvas = input_text.gameObject.GetComponent<CanvasRenderer>();
        canvas.SetColor(new Color(0.8f, 0.8f, 0.8f, 0.2f));

        var text_transform = input_text.Find("Text");
        text_transform.gameObject.GetComponent<Text>().color = new Color(0.28f, 0.22f, 0.13f, 1f);

        input_transform.Find("text invalid (1)").SetActive(false);
        input_text.Find("Image").SetActive(false);

        var placeholder = input_text.Find("Placeholder");
        var placeholderText = placeholder.GetComponent<Text>();
        placeholderText.color = new Color(0.28f, 0.22f, 0.13f, 0.8f);

        var element = input_text.GetOrCreate<LayoutElement>();
        element.preferredWidth = 100;

        return input_text;
    }

    public UISlider Slider<TValue>(int index, IList<TValue> list, Action<int, TValue> onChange, Func<TValue, string>? getInfo = null)
    {
        if (!YK.UIObjects.ContainsKey(typeof(UISlider)))
        {
            var pccLayer = Layer.Create<LayerEditPCC>();
            YK.UIObjects.Add(typeof(UISlider), Instantiate(pccLayer.sliderPortrait));
            Destroy(pccLayer.gameObject);
        }

        var uislider = (UISlider)Instantiate(YK.UIObjects[typeof(UISlider)]);
        var rect = uislider.Rect();
        rect.SetParent(transform);

        uislider.SetList(index, list, onChange, getInfo);
        uislider.textMain.text = "";
        uislider.textInfo.text = "";

        return uislider;
    }

    public Slider Slider(float value, Action<float> setvalue, float min, float max, Func<float, string>? labelfunc = null)
    {
        if (!YK.UIObjects.ContainsKey(typeof(Slider)))
        {
            var configLayer = Layer.Create<LayerConfig>();
            YK.UIObjects.Add(typeof(Slider), Instantiate(configLayer.sliderBGM));
            Destroy(configLayer.gameObject);
        }

        var slider = (Slider)Instantiate(YK.UIObjects[typeof(Slider)]);
        var rect = slider.Rect();
        rect.SetParent(transform);

        var labelfunc2 = labelfunc;
        slider.SetSlider(value, delegate (float v)
        {
            var result = ((labelfunc2 != null) ? labelfunc2(v) : null) ?? v.ToString();
            setvalue(v);
            return result;
        }, (int)min, (int)max, true);

        return slider;
    }

    public YKHorizontal Horizontal()
    {
        var group = YK.Create<YKHorizontal>(transform);
        group.OnLayout();
        return group;
    }

    public YKVertical Vertical()
    {
        var group = YK.Create<YKVertical>(transform);
        group.OnLayout();
        return group;
    }

    public YKGrid Grid()
    {
        var group = YK.Create<YKGrid>(transform);
        group.OnLayout();
        return group;
    }

    public YKScroll Scroll()
    {
        var group = YK.Create<YKScroll>(transform);
        group.OnLayout();
        return group;
    }

    public T Create<T>() where T : YKLayout
    {
        var l = YK.Create<T>(transform);
        l.OnLayout();
        return l;
    }
}
