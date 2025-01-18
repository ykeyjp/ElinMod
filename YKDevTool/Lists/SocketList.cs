using YKF;

namespace YKDev.Lists;

public class SocketList : YKList<SocketList.Socket, YKHorizontal>
{
    public override void OnLayout()
    {
        base.OnLayout();
        this.WithConstraintCount(2).WithCellSize(280, 50).WithSpace(20, 0);

        var callbacks = new UIList.Callback<Socket, YKHorizontal>
        {
            onList = OnListHandler,
            onInstantiate = (s, l) =>
            {
                var el = Element.Create(s.Id, s.Lv);
                l.TextSmall(el == null ? "空"._("Empty") : el.FullName).WithName("name").WithWidth(100);
                l.TextSmall(s.Lv.ToString()).WithName("lv").WithMinWidth(40);
                l.Button("削除"._("Del"), () => { Remove(s); }).WithName("del");
            },
            onRedraw = (s, l, i) =>
            {
                var nameText = l.Find<UIText>("name");
                var lvText = l.Find<UIText>("lv");
                var button = l.Find<UIButton>("del");
                var el = Element.Create(s.Id, s.Lv);

                nameText.text = el == null ? "空"._("Empty") : el.FullName;
                lvText.text = s.Lv.ToString();
                button.SetOnClick(() => { Remove(s); });
            },
            onRefresh = () => { }
        };

        var mold = Horizontal().WithLayerParent();
        callbacks.mold = mold;

        List.callbacks = callbacks;
    }

    private void Remove(Socket s)
    {
        if (Thing != null)
        {
            var key = s.Id * 100 + s.Lv;
            var index = Thing.sockets.IndexOf(key);
            if (index > -1)
            {
                Thing.elements.ModBase(s.Id, -s.Lv);
                Thing.sockets[index] = 0;
                OnRemove?.Invoke();
            }
        }
    }

    public Thing? Thing { get; set; }
    public Action? OnRemove { get; set; }

    public class Socket
    {
        public int Id { get; set; }
        public int Lv { get; set; }
    }
}
