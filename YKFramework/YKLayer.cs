using UnityEngine;
using UnityEngine.UI;

namespace YKF;

public abstract class YKLayer<T> : ELayer
{
    public virtual void OnLayout() { }

    public L CreateTab<L>(string idLang, string id) where L : YKLayout<T>
    {
        var parent = Window.Find("Content View");
        var rect = parent.gameObject.GetComponent<RectTransform>();

        // scroll
        var scroll = YK.GetResource<ScrollRect>("Scrollview parchment with Header");
        scroll.gameObject.name = id;
        var scroll_rect = scroll.Rect();
        scroll_rect.SetParent(parent);
        var scroll_ui = scroll.GetOrCreate<UIContent>();

        var headerRect = (RectTransform)scroll_rect.Find("Header Top Parchment");
        headerRect.SetActive(false);

        var viewport = (RectTransform)scroll_rect.Find("Viewport");

        var scroll_content = (RectTransform)viewport.Find("Content");
        scroll_content.DestroyAllChildren();
        var scroll_layout = scroll_content.gameObject.GetComponent<VerticalLayoutGroup>();
        scroll_layout.childControlHeight = true;
        scroll_layout.padding = new RectOffset(5, 5, 5, 5);

        // tab content
        var content = YK.Create<L>(scroll_content);
        content.gameObject.name = id;
        var rect_content = content.GetComponent<RectTransform>();

        // layout adjust
        var layout = content.gameObject.AddComponent<VerticalLayoutGroup>();
        layout.childControlHeight = false;
        layout.childForceExpandHeight = false;
        layout.padding = new RectOffset(10, 10, 0, 10);

        content.Layer = this;
        content.OnLayout();
        content.RebuildLayout(true);

        Window.AddTab(idLang, scroll_ui);

        return content;
    }

    public virtual string Title { get; } = "ウィンドウ"._("Window");

    public virtual Rect Bound { get; } = new Rect(0, 0, 640, 480);

    public Window Window
    {
        get
        {
            return windows[0];
        }
    }

    protected T? _data;
    public T Data
    {
        get
        {
            return _data!;
        }
        set
        {
            _data = value;
        }
    }

    public override void OnBeforeAddLayer()
    {
        this.option.rebuildLayout = true;
    }

    public override void OnAfterAddLayer()
    {
        foreach (var w in this.windows)
        {
            var r = w.Rect();
            w.RectTransform.localPosition = new Vector3(w.setting.bound.x, w.setting.bound.y, 0);
        }
    }

    public override bool blockWidgetClick
    {
        get
        {
            return false;
        }
    }
}
