using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace YKF;

public static class Ext
{
    public static List<Dropdown.OptionData> ToDropdownOptions(this List<string> list)
    {
        return list.Select(x => new Dropdown.OptionData(x)).ToList();
    }

    public static string _(this string ja, string en = "")
    {
        if (en == null) return ja;
        return Lang.isJP ? ja : en;
    }

    public static void DestroyAllChildren(this Transform parent)
    {
        foreach (Transform child in parent)
        {
            child.gameObject.SetActive(false);
            UnityEngine.Object.Destroy(child.gameObject);
            child.SetActive(false);
            UnityEngine.Object.Destroy(child);
        }
    }

    public static T2 ReplaceComponent<T, T2>(this T original) where T : MonoBehaviour where T2 : MonoBehaviour
    {
        var gameObject = original.gameObject;
        for (var i = 0; i < original.transform.childCount; i++)
        {
            var go = original.transform.GetChild(i).gameObject;
            go.SetActive(false);
        }
        UnityEngine.Object.Destroy(original);
        gameObject.name = typeof(T2).Name;

        var tderived = gameObject.AddComponent<T2>();
        try
        {
            foreach (var fieldInfo in typeof(T).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                fieldInfo.SetValue(tderived, fieldInfo.GetValue(original));
            }
        }
        catch (Exception) { }

        return tderived;
    }

    public static LayoutElement LayoutElement(this Component component)
    {
        return component.GetOrCreate<LayoutElement>();
    }

    public static void DestroyObject(this Component component)
    {
        UnityEngine.Object.Destroy(component.gameObject);
    }

    public static T ReplaceLayerComponent<T>(this Layer original) where T : MonoBehaviour
    {
        var gameObject = original.gameObject;
        T derived = gameObject.AddComponent<T>();
        try
        {
            foreach (FieldInfo fieldInfo in typeof(Layer).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                fieldInfo.SetValue(derived, fieldInfo.GetValue(original));
            }
            UnityEngine.Object.Destroy(original);
            gameObject.name = typeof(T).Name;

        }
        catch (Exception) { }

        return derived;
    }

    public static Window AddWindow(this Layer layer, Window.Setting setting)
    {
        setting.tabs ??= [];

        var layerList = (LayerList)Layer.Create(typeof(LayerList).Name);
        var window = layerList.windows.First();
        window.transform.SetParent(layer.transform);
        layer.windows.Add(window);
        UnityEngine.Object.Destroy(layerList.gameObject);

        window.setting = setting;
        window.RectTransform.sizeDelta = setting.bound.size;
        window.RectTransform.position = setting.bound.position;

        var content = window.Find("Content View");
        content.DestroyChildren();
        content.DestroyAllChildren();

        return window;
    }

    public static UIInputText WithPlaceholder(this UIInputText input, string text)
    {
        var placeholder = input.Find("Placeholder");
        var placeholderText = placeholder.GetComponent<Text>();

        placeholder.SetActive(true);
        placeholderText.text = text;

        return input;
    }

    public static T WithName<T>(this T component, string text) where T : Component
    {
        component.gameObject.name = text;

        return component;
    }

    public static T WithWidth<T>(this T component, int size) where T : Component
    {
        var element = component.GetOrCreate<LayoutElement>();
        element.preferredWidth = size;

        return component;
    }

    public static T WithMinWidth<T>(this T component, int size) where T : Component
    {
        var element = component.GetOrCreate<LayoutElement>();
        element.minWidth = size;

        return component;
    }

    public static T WithHeight<T>(this T component, int size) where T : Component
    {
        var element = component.GetOrCreate<LayoutElement>();
        element.preferredHeight = size;

        return component;
    }

    public static T WithMinHeight<T>(this T component, int size) where T : Component
    {
        var element = component.GetOrCreate<LayoutElement>();
        element.minHeight = size;

        return component;
    }

    public static T WithPivot<T>(this T component, float x, float y) where T : Component
    {
        component.Rect().pivot = new Vector2(x, y);

        return component;
    }

    public static T WithConstraintCount<T>(this T component, int size) where T : YKGrid
    {
        component.Layout.constraintCount = size;

        return component;
    }

    public static T WithCellSize<T>(this T component, int width, int height) where T : YKGrid
    {
        component.Layout.cellSize = new Vector2(width, height);

        return component;
    }

    public static T WithSpace<T>(this T component, int size, int size2 = 0) where T : YKLayout
    {
        if (component is YKGrid grid)
        {
            grid.Layout.spacing = new Vector2(size, size2);
        }
        if (component is YKHorizontal group1)
        {
            group1.Layout.spacing = size;
        }
        if (component is YKVertical group2)
        {
            group2.Layout.spacing = size;
        }

        return component;
    }

    public static T WithFitMode<T>(this T component, ContentSizeFitter.FitMode horizontal, ContentSizeFitter.FitMode? vertical = null) where T : YKHorizontal
    {
        var fitter = component.Layout.GetComponent<ContentSizeFitter>();
        if (fitter != null)
        {
            fitter.horizontalFit = horizontal;

            if (vertical != null)
            {
                fitter.verticalFit = vertical.Value;
            }
        }

        return component;
    }

    public static T WithLayerParent<T>(this T component) where T : Component
    {
        try
        {
            component.transform.SetParent(component.transform.GetComponentInParent<ELayer>().transform);
        }
        catch { }
        component.SetActive(false);
        return component;
    }
}
