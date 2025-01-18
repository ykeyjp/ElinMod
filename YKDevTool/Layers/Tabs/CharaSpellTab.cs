using UnityEngine.UI;
using YKDev.Lists;
using YKF;

namespace YKDev.Layers.Tabs;

public class CharaSpellTab : YKLayout<Chara>
{
    public override void OnLayout()
    {
        var chara = Layer.Data;
        var headerWidth = 120;
        Header(chara.GetName(NameStyle.Full));

        var spellList = EClass.sources.elements.rows.Where((e) => { return e.group == "SPELL" && e.alias.Last() != '_'; }).ToArray();

        // 呪文
        {
            var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
            group.HeaderSmall("魔法"._("Spell")).WithMinWidth(headerWidth);
            var dropdown = group.Dropdown(spellList.Select(x => x.GetName()).ToList()).WithWidth(150);
            var baseInput = group.InputText("").WithPlaceholder("ベース"._("Base"));
            var potentialInput = group.InputText("").WithPlaceholder("回数"._("Count"));
            group.Button("取得"._("Gain"), () =>
            {
                GainElement(chara, spellList[dropdown.value], baseInput.Num, potentialInput.Num);
                RefreshElementList();
            });
        }
        // Exp
        {
            var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
            group.HeaderSmall("経験値"._("Exp")).WithMinWidth(headerWidth);
            var dropdown = group.Dropdown(spellList.Select(x => x.GetName()).ToList()).WithWidth(150);
            var expInput = group.InputText("").WithPlaceholder("加算、減算"._("plus, minus"));
            group.Button("取得"._("Gain"), () =>
            {
                chara.ModExp(spellList[dropdown.value].id, expInput.Num);
                RefreshElementList();
            });
        }

        // 現在値
        {
            Header("魔法"._("Spell"));
            _spellElementList = Create<ElementList>();
            _spellElementList.Container = chara.elements;
            _spellElementList.OnList = (m) => chara.elements.dict.Where(e => e.Value is Spell).Select(x => x.Value).ToList();
        }

        RefreshElementList();
    }

    private void GainElement(Chara chara, SourceElement.Row el, int lv, int potential = 0)
    {
        if (lv == 0)
        {
            chara.elements.Remove(el.id);
        }
        else
        {
            var element = chara.elements.GetOrCreateElement(el.id);
            if (element.ValueWithoutLink == 0)
            {
                chara.elements.ModBase(el.id, 1);
            }
            chara.elements.SetBase(el.id, lv, potential);
        }
    }

    private ElementList? _spellElementList;

    private void RefreshElementList()
    {
        _spellElementList?.Refresh();
    }
}
