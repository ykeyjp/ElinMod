using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace YK;

public static class Ext
{
    public static LayoutBase Vertical(this Window window)
    {
        return new LayoutV(window);
    }

    public static LayoutBase Horizontal(this Window window)
    {
        return new LayoutH(window);
    }

    public static LayoutBase Scrollable(this Window window)
    {
        return new LayoutS(window);
    }

    public static LayoutBase Tab(this Window window, string title)
    {
        var layout = new LayoutV(window);
        var content = layout.Layoutable.Transform.gameObject.GetOrAddComonent<UIContent>();
        window.AddTab(title, content);
        return layout;
    }

    public static void SplitLR(this LayoutBase layout, out LayoutV left, out LayoutV right, float margin = 0f, float? size = null)
    {
        layout.CurrentRect.SplitVerticallyWithMargin(out Rect rect, out Rect rect2, margin);
        var hl = layout.Horizontal(size);
        left = hl.Vertical(new float?(rect.width));
        right = hl.Vertical(new float?(rect2.width));
    }

    public static void SplitScroll(this LayoutBase layout, out LayoutS left, out LayoutS right, float margin = 0f, float? size = null)
    {
        layout.CurrentRect.SplitVerticallyWithMargin(out Rect rect, out Rect rect2, margin);
        var hl = layout.Horizontal(size);
        left = hl.Scroll(new float?(rect.width));
        right = hl.Scroll(new float?(rect2.width));
    }

    public static void DestroyAllChildren(this Transform parent)
    {
        parent.DestroyChildren();
        foreach (Transform child in parent)
        {
            child.gameObject.SetActive(false);
            GameObject.Destroy(child.gameObject);
            child.SetActive(false);
            GameObject.Destroy(child);
        }
    }

    public static bool TryGetHeader(this Window window, out UIHeader? header)
    {
        header = null;
        if (!window.rectHeader) return false;
        header = window.rectHeader.GetComponentsInChildren<UIHeader>(true).FirstOrDefault();
        return true;
    }

    public static WindowHost Wrap(this Window window)
    {
        var setting = window.setting;
        return new WindowHost(window, (setting != null) ? setting.bound : window.RectTransform.rect);
    }

    public static Window Window(this Layer layer, int width, int height, string caption)
    {
        return layer.Window(new Window.Setting
        {
            textCaption = caption,
            bound = new Rect(0, 0, width, height),
            transparent = false,
            allowMove = true,
            allowResize = true,
            anime = YUI.GeAnimeLR(true)
        });
    }

    public static Window Window(this Layer layer, Window.Setting setting)
    {
        setting.tabs ??= [];

        LayerList layerList = (LayerList)Layer.Create(typeof(LayerList).Name);
        Window window = layerList.windows.First();
        window.transform.SetParent(layer.transform);
        layer.windows.Add(window);
        UnityEngine.Object.Destroy(layerList.gameObject);

        window.setting = setting;
        window.Init(layer);
        window.RectTransform.position = setting.bound.position;
        window.RectTransform.sizeDelta = setting.bound.size;
        window.GetType().GetMethod("RecalculatePositionCaches", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(window, null);
        window.RebuildLayout(true);

        if (window.TryGetHeader(out UIHeader? c)) c.SetActive(false);
        window.GetComponent<VerticalLayoutGroup>().enabled = false;

        GameObject.Destroy(window.Find("StatsBar").gameObject);

        var content = window.Find("Content View");
        content.DestroyChildren();
        content.DestroyAllChildren();
        var rect_content = content.gameObject.GetComponent<RectTransform>();
        rect_content.anchoredPosition = new Vector2(0, 0);

        var hg = content.GetComponent<HorizontalLayoutGroup>();
        GameObject.Destroy(hg);
        return window;
    }

    // GameObject
    public static T GetOrAddComonent<T>(this GameObject go) where T : Component
    {
        return go.TryGetComponent(out T c) ? c : go.AddComponent<T>();
    }

    // Transform
    public static T WithName<T>(this T t, string name) where T : Transform
    {
        t.gameObject.name = name;
        return t;
    }

    public static RectTransform StretchY(this RectTransform tr)
    {
        tr.SetAnchor(0f, 0f, 0f, 1f);
        tr.SetPositionY(0f);
        tr.sizeDelta = new Vector2(tr.sizeDelta.x, 0f);
        return tr;
    }

    public static RectTransform ResetPosition(this RectTransform tr)
    {
        var v = new Vector2(tr.sizeDelta.x, tr.sizeDelta.y);
        tr.SetAnchor(0f, 0f, 0f, 1f);
        tr.SetPositionY(0f);
        tr.sizeDelta = v;
        return tr;
    }

    public static RectTransform Zero(this RectTransform tr)
    {
        tr.SetAnchor(0f, 1f, 0f, 1f);
        tr.localScale = new Vector3(1f, 1f, 1f);
        tr.SetPivot(0f, 1f);
        tr.position = default;
        tr.localPosition = default;
        return tr;
    }

    // UIButton
    public static UIButton WithPress(this UIButton button, Action action)
    {
        button.onClick.AddListener(delegate ()
        {
            SE.Click();
            action();
        });
        return button;
    }

    public static UIButton SetSize(this UIButton button, Vector2 size)
    {
        button.Rect().sizeDelta = size;
        return button;
    }

    // HorizontalOrVerticalLayoutGroup
    public static T SetupLayoutGroup<T>(this T group) where T : HorizontalOrVerticalLayoutGroup
    {
        group.childControlHeight = false;
        group.childControlWidth = false;
        group.childForceExpandHeight = false;
        group.childForceExpandWidth = false;
        group.padding = new RectOffset(0, 0, 0, 0);
        group.spacing = 0f;
        return group;
    }

    // ContentSizeFitter
    public static T SetupFitter<T>(this T fit) where T : ContentSizeFitter
    {
        fit.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        fit.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        return fit;
    }

    // Vector2
    public static Vector2 InverseY(this Vector2 v, float extra = 0f) => new(v.x, -v.y + extra);
    public static Vector3 InverseY(this Vector3 v, float extra = 0f) => new(v.x, -v.y + extra, v.z);

    // Rect
    public static Rect InverseY(this Rect rect, float extra = 0f) => new(rect.x, -rect.y + extra, rect.width, rect.height);
    public static Rect ShrinkRight(this Rect rect, float p) => new(rect.x, rect.y, rect.width - p, rect.height);
    public static Rect ShrinkLeft(this Rect rect, float p) => new(rect.x + p, rect.y, rect.width - p, rect.height);
    public static Rect ShrinkTop(this Rect rect, float p) => new(rect.x, rect.y + p, rect.width, rect.height - p);
    public static Rect ShrinkBottom(this Rect rect, float p) => new(rect.x, rect.y, rect.width, rect.height - p);
    public static Rect GrowRight(this Rect rect, float p) => rect.ShrinkRight(-p);
    public static Rect GrowLeft(this Rect rect, float p) => rect.ShrinkLeft(-p);
    public static Rect GrowTop(this Rect rect, float p) => rect.ShrinkTop(-p);
    public static Rect GrowBottom(this Rect rect, float p) => rect.ShrinkBottom(-p);
    public static Rect Move(this Rect rect, float x = 0f, float y = 0f) => new(rect.x + x, rect.y + y, rect.width, rect.height);
    public static Rect Square(this Rect rect) => new(rect.x, rect.y, Mathf.Min(rect.width, rect.height), Mathf.Min(rect.width, rect.height));
    public static Rect AtZero(this Rect rect) => new(default, rect.size);
    public static Rect LeftHalf(this Rect rect) => new(rect.x, rect.y, rect.width / 2f, rect.height);
    public static Rect LeftPart(this Rect rect, float pct) => new(rect.x, rect.y, rect.width * pct, rect.height);
    public static Rect LeftPartPixels(this Rect rect, float width) => new(rect.x, rect.y, width, rect.height);
    public static Rect RightHalf(this Rect rect) => new(rect.x + rect.width / 2f, rect.y, rect.width / 2f, rect.height);
    public static Rect RightPart(this Rect rect, float pct) => new(rect.x + rect.width * (1f - pct), rect.y, rect.width * pct, rect.height);
    public static Rect RightPartPixels(this Rect rect, float width) => new(rect.x + rect.width - width, rect.y, width, rect.height);
    public static Rect TopHalf(this Rect rect) => new(rect.x, rect.y, rect.width, rect.height / 2f);
    public static Rect TopPart(this Rect rect, float pct) => new(rect.x, rect.y, rect.width, rect.height * pct);
    public static Rect TopPartPixels(this Rect rect, float height) => new(rect.x, rect.y, rect.width, height);
    public static Rect BottomHalf(this Rect rect) => new(rect.x, rect.y + rect.height / 2f, rect.width, rect.height / 2f);
    public static Rect BottomPart(this Rect rect, float pct) => new(rect.x, rect.y + rect.height * (1f - pct), rect.width, rect.height * pct);
    public static Rect BottomPartPixels(this Rect rect, float height) => new(rect.x, rect.y + rect.height - height, rect.width, height);

    public static bool SplitHorizontallyWithMargin(this Rect rect, out Rect top, out Rect bottom, out float overflow, float compressibleMargin = 0f, float? topHeight = null, float? bottomHeight = null)
    {
        overflow = Mathf.Max(0f, (topHeight ?? bottomHeight.GetValueOrDefault()) - rect.height);
        var height = Mathf.Clamp(topHeight ?? (rect.height - bottomHeight.GetValueOrDefault() - compressibleMargin), 0f, rect.height);
        var num = Mathf.Clamp(bottomHeight ?? (rect.height - topHeight.GetValueOrDefault() - compressibleMargin), 0f, rect.height);
        top = new Rect(rect.x, rect.y, rect.width, height);
        bottom = new Rect(rect.x, rect.yMax - num, rect.width, num);
        return overflow == 0f;
    }

    public static void SplitVerticallyWithMargin(this Rect rect, out Rect left, out Rect right, float margin)
    {
        var num = rect.width / 2f;
        left = new Rect(rect.x, rect.y, num - margin / 2f, rect.height);
        right = new Rect(left.xMax + margin, rect.y, num - margin / 2f, rect.height);
    }

    public static bool SplitVerticallyWithMargin(this Rect rect, out Rect left, out Rect right, out float overflow, float compressibleMargin = 0f, float? leftWidth = null, float? rightWidth = null)
    {
        overflow = Mathf.Max(0f, (leftWidth ?? rightWidth.GetValueOrDefault()) - rect.width);
        var width = Mathf.Clamp(leftWidth ?? (rect.width - rightWidth.GetValueOrDefault() - compressibleMargin), 0f, rect.width);
        var num = Mathf.Clamp(rightWidth ?? (rect.width - leftWidth.GetValueOrDefault() - compressibleMargin), 0f, rect.width);
        left = new Rect(rect.x, rect.y, width, rect.height);
        right = new Rect(rect.xMax - num, rect.y, num, rect.height);
        return overflow == 0f;
    }

    public static void SplitHorizontally(this Rect rect, float topHeight, out Rect top, out Rect bottom)
    {
        rect.SplitHorizontallyWithMargin(out top, out bottom, out float num, 0f, new float?(topHeight), null);
    }

    public static void SplitVertically(this Rect rect, float leftWidth, out Rect left, out Rect right)
    {
        rect.SplitVerticallyWithMargin(out left, out right, out float num, 0f, new float?(leftWidth), null);
    }
}
