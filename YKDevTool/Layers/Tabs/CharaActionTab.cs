using UnityEngine.UI;
using YKDev.Lists;
using YKF;

namespace YKDev.Layers.Tabs;

public class CharaActionTab : YKLayout<Chara>
{
    public override void OnLayout()
    {
        var chara = Layer.Data;
        var headerWidth = 120;
        Header(chara.GetName(NameStyle.Full));

        var actList = ACT.dict.Select(a => a.Value).ToArray();

        // アクション
        {
            var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
            group.HeaderSmall("アクション"._("Action")).WithMinWidth(headerWidth);
            var dropdown = group.Dropdown(actList.Select(x => x.FullName).ToList()).WithWidth(150);
            var toggle = group.Toggle("パーティー"._("Party")).WithWidth(150);
            group.Button("取得"._("Gain"), () =>
            {
                var act = actList[dropdown.value];
                chara.ability.Add(act.id, 50, toggle.isChecked);
                RefreshActions();
            });
        }
        // 現在値
        {
            Header("アクション"._("Action"));
            _abilityList = Create<CharaAbilityList>();
            _abilityList.Ability = chara.ability;
            _abilityList.OnList = (m) => [.. chara.ability.list.items];
            _abilityList.OnRemove = RefreshActions;
        }

        RefreshActions();
    }

    private CharaAbilityList? _abilityList;

    private void RefreshActions()
    {
        _abilityList?.Refresh();
    }
}
