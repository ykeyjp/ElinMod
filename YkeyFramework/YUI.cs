using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace YK;

public static class YUI
{
    private static readonly Dictionary<Type, UnityEngine.Object> UIObjects;
    private static readonly Dictionary<Type, string> UIElementsList;
    public const float SliderMarginTop = 15f;
    public const float SliderHeight = 60f;
    public const float TextHeight = 20f;
    public const float ButtonWidth = 110f;
    public const float ButtonHeight = 40f;
    public const float RawSliderHeight = 60f;
    public const float SliderWidth = 160f;
    public const float TextWidth = 100f;
    public const float WindowXOffset = 30f;
    public const float WindowYOffset = 40f;
    public const float HeaderHeight = 30f;
    public static readonly Slider RawSlider;

    static YUI()
    {
        UIElementsList = new Dictionary<Type, string>
        {
            { typeof(UIText), "text caption" },
            { typeof(InputField), "InputField" },
            { typeof(UIButton), "ButtonBottom Parchment" },
            { typeof(Layer), "Layers(Float)" },
            { typeof(ScrollRect), "Scrollview parchment with Header" },
            { typeof(UIScrollView), "Scrollview default" },
            { typeof(Window), "Window Parchment" },
        };
        UIObjects = UIElementsList
            .ToDictionary(
                (KeyValuePair<Type, string> k) => k.Key,
                (KeyValuePair<Type, string> v) => Resources.FindObjectsOfTypeAll(v.Key).FirstOrDefault((UnityEngine.Object x) => x.name == v.Value)
            );
        var layerConfig = Layer.Create<LayerConfig>();
        RawSlider = UnityEngine.Object.Instantiate(layerConfig.sliderBGM);
        UnityEngine.Object.DestroyImmediate(layerConfig.gameObject);
    }

    public static string _(string ja, string en = "")
    {
        if (en == null) return ja;
        return Lang.isJP ? ja : en;
    }

    public static T GetResource<T>() where T : UnityEngine.Object
    {
        return (T)(object)UnityEngine.Object.Instantiate(UIObjects[typeof(T)]);
    }

    public static T GetResourceDirect<T>(string name) where T : UnityEngine.Object
    {
        return UnityEngine.Object.Instantiate((T)Resources.FindObjectsOfTypeAll(typeof(T)).FirstOrDefault((UnityEngine.Object x) => x.name == name));
    }

    public static T LoadUI<T, TA>(TA args) where T : BuildUI<TA> where TA : IBuildUIArgs
    {
        var l = EMono.ui.layers.Find(o => o.GetType() == typeof(T)) as T;
        if (l != null)
        {
            l.SetActive(true);
            return l;
        }

        var resource = GetResource<Layer>();
        T tui = resource.ReplaceComponent<Layer, T>();
        tui.option = new Layer.Option
        {
            hideOthers = true,
            screenClickCloseRight = true,
            screenlockType = Layer.Option.ScreenlockType.None,
        };
        tui.onKill = new UnityEvent();

        tui.Setup(args);

        EMono.ui.AddLayer(tui);
        return tui;
    }

    public static RectTransform GenerateTransform(Transform? parent = null, string? name = null)
    {
        var go = new GameObject(name ?? "YK Object", [typeof(RectTransform)]);
        var c = go.GetComponent<RectTransform>();
        c.Zero();
        if (parent != null) c.SetParent(parent);
        return c;
    }

    public static Anime GeAnimeLR(bool left = true) => Resources.Load<Anime>("Media/Anime/" + (left ? "In Window from left" : "In Window from right"));

    public static TDerived ReplaceComponent<TBase, TDerived>(this TBase original) where TBase : MonoBehaviour where TDerived : MonoBehaviour
    {
        var gameObject = original.gameObject;
        TDerived tderived = gameObject.AddComponent<TDerived>();
        foreach (FieldInfo fieldInfo in typeof(TBase).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
        {
            fieldInfo.SetValue(tderived, fieldInfo.GetValue(original));
        }
        GameObject.Destroy(original);
        gameObject.name = typeof(TDerived).Name;
        gameObject.transform.DestroyChildren();
        return tderived;
    }

    public static T MakeNew<T>(Transform? parent = null) where T : MonoBehaviour
    {
        var rect = GenerateTransform(parent, null);
        rect.gameObject.name = typeof(T).Name;
        return rect.gameObject.AddComponent<T>();
    }

    public static UIText MakeText(string text, Action<UIText>? mod = null, RectTransform? parent = null)
    {
        var uitext = MakeNew<UIText>(parent);
        if (mod != null)
        {
            mod(uitext);
        }
        else
        {
            uitext.skinType = SkinType.Default;
            uitext.fontColor = FontColor.Default;
        }
        uitext.ApplySkin();

        var rect = uitext.rectTransform;
        rect.sizeDelta = new Vector2(100f, 20f);

        uitext.text = text;
        uitext.alignment = TextAnchor.MiddleLeft;
        uitext.transform.position = new Vector3(0f, 0f);
        return uitext;
    }

    public static UIButton MakeButton(string? text = null)
    {
        var btn = GetResource<UIButton>();
        btn.buttonType = ButtonType.Default;
        if (text != null)
        {
            if (!btn.mainText)
            {
                btn.mainText = MakeText(text, null, null);
                btn.mainText.transform.SetParent(btn.transform);
            }
            else
            {
                btn.mainText.text = text;
            }
        }
        btn.ApplySkin();
        var rect = btn.Rect();
        rect.Zero();
        rect.sizeDelta = new Vector2(110f, 40f);
        return btn;
    }

    public static UISlider MakeSlider(Transform parent)
    {
        var el = Layer.Create<LayerEditPCC>();
        var sliderPortrait = el.sliderPortrait;
        var rect = sliderPortrait.transform.parent.Rect();
        rect.SetParent(parent);
        rect.Zero();
        UnityEngine.Object.Destroy(el.gameObject);
        return sliderPortrait;
    }

    public static UIInputText MakeInputField()
    {
        var transform = Util.Instantiate("UI/Element/Input/InputText");
        var trans = transform.Find("InputField");
        trans.SetParent(null);

        var canvas = trans.GetComponent<CanvasRenderer>();
        canvas.SetColor(new Color(0.99f, 0.99f, 0.99f, 0.3f));
        GameObject.Destroy(trans.Find("Image").gameObject);
        GameObject.Destroy(transform.gameObject);

        var input = trans.GetComponent<UIInputText>();
        input.field.textComponent.color = new Color(0.239f, 0.175f, 0.079f);
        input.field.image.color = new Color(1f, 1f, 1f, 0.25f);
        input.field.contentType = InputField.ContentType.Standard;
        input.field.textComponent.fontSize = 15;

        return input;
    }
}
