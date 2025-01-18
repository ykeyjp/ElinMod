using YKF;

namespace YKDev.Lists;

public class RecipeSourceList : YKList<RecipeSource, YKHorizontal>
{
    public override void OnLayout()
    {
        base.OnLayout();
        this.WithConstraintCount(4).WithCellSize(230, 50).WithSpace(10, 0);

        var callbacks = new UIList.Callback<RecipeSource, YKHorizontal>
        {
            onList = OnListHandler,
            onInstantiate = (r, l) =>
            {
                l.Toggle("", IsKnown(r.id), (b) => { SetKnown(r.id, b); }).WithName("toggle");
                var lvInput = l.InputText(GetLv(r.id).ToString(), (i) => { SetLv(r.id, i); }).WithName("lv").WithMinWidth(50);
                l.TextSmall(r.Name).WithName("name").WithWidth(120);
            },
            onRedraw = (r, l, i) =>
            {
                var toggle = l.Find<UIButton>("toggle");
                var lvInput = l.Find<UIInputText>("lv");
                var nameText = l.Find<UIText>("name");

                toggle.SetToggle(IsKnown(r.id), (b) => { SetKnown(r.id, b); });
                lvInput.Text = GetLv(r.id).ToString();
                lvInput.onValueChanged = (i) => { SetLv(r.id, i); };
                nameText.text = r.Name;
            },
            onRefresh = () => { }
        };

        var mold = Horizontal().WithLayerParent();
        callbacks.mold = mold;

        List.callbacks = callbacks;
    }

    private bool IsKnown(string key)
    {
        return EClass.player.recipes.knownRecipes.ContainsKey(key);
    }

    private int GetLv(string key)
    {
        if (IsKnown(key))
        {
            return EClass.player.recipes.knownRecipes[key];
        }

        return 0;
    }

    private void SetLv(string key, int lv)
    {
        if (IsKnown(key))
        {
            EClass.player.recipes.knownRecipes[key] = lv;
        }
    }

    private void SetKnown(string key, bool isOn)
    {
        if (isOn)
        {
            if (!IsKnown(key))
            {
                EClass.player.recipes.Add(key, false);
            }
        }
        else
        {
            if (IsKnown(key))
            {
                EClass.player.recipes.knownRecipes.Remove(key);
            }
        }
    }
}
