using UnityEngine;

namespace YKF;

public static class YK
{
    public static Dictionary<Type, UnityEngine.Object> UIObjects = [];

    public static string GamePatchStatus
    {
        get
        {
            // public,nightly
            if (Steamworks.SteamApps.GetCurrentBetaName(out var name, 128))
            {
                return name;
            }
            return "public";
        }
    }

    public static T GetResource<T>(string hint) where T : Component
    {
        if (!UIObjects.TryGetValue(typeof(T), out UnityEngine.Object obj))
        {
            obj = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault((T x) => x.name == hint);
            UIObjects.Add(typeof(T), obj);
        }
        var res = (T)UnityEngine.Object.Instantiate(obj);
        if (res == null)
        {
            Debug.Log("[YKF] not instantiate resource: " + hint);
        }
        return res!;
    }

    public static T Create<T>(Transform? parent = null) where T : MonoBehaviour
    {
        GameObject gameObject = new(typeof(T).Name, [typeof(RectTransform)]);
        if (parent != null)
        {
            gameObject.transform.SetParent(parent);
        }
        return gameObject.AddComponent<T>();
    }

    public static T CreateLayer<T, T2>(T2 arg) where T : YKLayer<T2>
    {
        var l = EMono.ui.layers.Find(o => o.GetType() == typeof(T)) as T;
        if (l != null)
        {
            l.SetActive(true);
            return l;
        }

        var widget_ = Create<T>();
        var widget = UnityEngine.Object.Instantiate(widget_);
        widget.gameObject.name = typeof(T).Name;
        widget_.DestroyObject();

        widget.Data = arg;
        widget.AddWindow(new Window.Setting
        {
            textCaption = widget.Title,
            bound = widget.Bound,
            allowMove = true,
            transparent = false,
            openLastTab = false,
        });
        widget.OnLayout();

        EMono.ui.AddLayer(widget);

        return widget;
    }

    public static T CreateLayer<T>() where T : YKLayer<object>
    {
        return CreateLayer<T, object>(0);
    }
}
