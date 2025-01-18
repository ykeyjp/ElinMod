using UnityEngine;

namespace YKF;

public class YKList<T, T2> : YKGrid where T2 : Component
{
    public override void OnLayout()
    {
        base.OnLayout();

        _list = Layout.GetOrCreate<UIList>();
        _list.useDefaultNoItem = false;
    }

    protected UIList? _list;
    public UIList List
    {
        get { return _list!; }
    }

    public Func<UIList.SortMode, List<T>>? OnList { get; set; }

    protected virtual void OnListHandler(UIList.SortMode mode)
    {
        List.items = [];
        if (OnList != null)
        {
            List.items = OnList(mode).Select(x => (object)x!).ToList();
        }
    }

    public void Refresh()
    {
        List.List();
    }
}
