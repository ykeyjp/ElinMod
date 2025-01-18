using UnityEngine.UI;
using YKDev.Lists;
using YKF;

namespace YKDev.Layers.Tabs;

public class CharaSkillTab : YKLayout<Chara>
{
    public override void OnLayout()
    {
        var chara = Layer.Data;
        var headerWidth = 120;
        Header(chara.GetName(NameStyle.Full));

        var attributeList = EClass.sources.elements.rows.Where(e => e.category == "attribute").ToArray();
        var skillList = EClass.sources.elements.rows.Where(e => e.category == "skill").ToArray();
        SourceElement.Row[] attrAndSkillList = [.. attributeList, .. skillList];
        var featList = EClass.sources.elements.rows.Where(e => e.category == "feat").ToArray();
        var slotList = EClass.sources.elements.rows.Where(e => e.category == "slot").ToArray();
        var mutationtList = EClass.sources.elements.rows.Where(e => e.category == "mutation").ToArray();
        var etherList = EClass.sources.elements.rows.Where(e => e.category == "ether").ToArray();

        // 能力
        {
            var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
            group.HeaderSmall("能力"._("Attribute")).WithMinWidth(headerWidth);
            var dropdown = group.Dropdown(attributeList.Select(x => x.GetName()).ToList()).WithWidth(150);
            var baseInput = group.InputText("").WithPlaceholder("ベース"._("Base"));
            var potentialInput = group.InputText("").WithPlaceholder("潜在"._("Potential"));
            group.Button("取得"._("Gain"), () =>
            {
                GainElement(chara, attributeList[dropdown.value], baseInput.Num, potentialInput.Num);
                RefreshElementList();
            });
        }
        // スキル
        {
            var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
            group.HeaderSmall("スキル"._("Skill")).WithMinWidth(headerWidth);
            var dropdown = group.Dropdown(skillList.Select(x => x.GetName()).ToList()).WithWidth(150);
            var baseInput = group.InputText("").WithPlaceholder("ベース"._("Base"));
            var potentialInput = group.InputText("").WithPlaceholder("潜在"._("Potential"));
            group.Button("取得"._("Gain"), () =>
            {
                GainElement(chara, skillList[dropdown.value], baseInput.Num, potentialInput.Num);
                RefreshElementList();
            });
        }
        {
            var group = Horizontal();
            group.Layout.childForceExpandWidth = true;
            group.Button("未取得のスキルをLv1で習得"._("Acquire an unacquired skill at Lv1"), () =>
            {
                foreach (var el in skillList)
                {
                    var element = chara.elements.GetOrCreateElement(el.id);
                    if (element.ValueWithoutLink == 0)
                    {
                        chara.elements.ModBase(el.id, 1);
                    }
                }
            });
        }
        // Exp
        {
            var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
            group.HeaderSmall("経験値"._("Exp")).WithMinWidth(headerWidth);
            var dropdown = group.Dropdown(attrAndSkillList.Select(x => x.GetName()).ToList()).WithWidth(150);
            var expInput = group.InputText("").WithPlaceholder("加算、減算"._("plus, minus"));
            group.Button("取得"._("Gain"), () =>
            {
                chara.ModExp(attrAndSkillList[dropdown.value].id, expInput.Num);
                RefreshElementList();
            });
        }
        // フィート
        {
            var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
            group.HeaderSmall("フィート"._("Feat")).WithMinWidth(headerWidth);
            var dropdown = group.Dropdown(featList.Select(x => x.GetName()).ToList()).WithWidth(150);
            var baseInput = group.InputText("").WithPlaceholder("ベース"._("Base"));
            var potentialInput = group.InputText("").WithPlaceholder("潜在"._("Potential"));
            group.Button("取得"._("Gain"), () =>
            {
                GainElement(chara, featList[dropdown.value], baseInput.Num, potentialInput.Num);
                RefreshElementList();
            });
        }
        // スロット
        {
            var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
            group.HeaderSmall("スロット"._("Slot")).WithMinWidth(headerWidth);
            var dropdown = group.Dropdown(slotList.Select(x => x.GetName()).ToList()).WithWidth(150);
            var baseInput = group.InputText("").WithPlaceholder("ベース"._("Base"));
            var potentialInput = group.InputText("").WithPlaceholder("潜在"._("Potential"));
            group.Button("取得"._("Gain"), () =>
            {
                GainElement(chara, slotList[dropdown.value], baseInput.Num, potentialInput.Num);
                RefreshElementList();
            });
        }
        // 変異
        {
            var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
            group.HeaderSmall("変異"._("Mutation")).WithMinWidth(headerWidth);
            var dropdown = group.Dropdown(mutationtList.Select(x => x.GetName()).ToList()).WithWidth(150);
            var baseInput = group.InputText("").WithPlaceholder("ベース"._("Base"));
            var potentialInput = group.InputText("").WithPlaceholder("潜在"._("Potential"));
            group.Button("取得"._("Gain"), () =>
            {
                GainElement(chara, mutationtList[dropdown.value], baseInput.Num, potentialInput.Num);
                RefreshElementList();
            });
        }
        // エーテル
        {
            var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
            group.HeaderSmall("エーテル"._("Ether")).WithMinWidth(headerWidth);
            var dropdown = group.Dropdown(etherList.Select(x => x.GetName()).ToList()).WithWidth(150);
            var baseInput = group.InputText("").WithPlaceholder("ベース"._("Base"));
            var potentialInput = group.InputText("").WithPlaceholder("潜在"._("Potential"));
            group.Button("取得"._("Gain"), () =>
            {
                GainElement(chara, etherList[dropdown.value], baseInput.Num, potentialInput.Num);
                RefreshElementList();
            });
        }

        // 現在値
        {
            Header("主能力"._("Main"));
            _attributeElementList = Create<ElementList>();
            _attributeElementList.Container = chara.elements;
            _attributeElementList.OnList = (m) => chara.elements.dict.Where(e => e.Value.GetType() == typeof(AttbMain)).Select(x => x.Value).ToList();

            Header("一般"._("General"));
            _generalElementList = Create<ElementList>();
            _generalElementList.Container = chara.elements;
            _generalElementList.OnList = (m) => chara.elements.dict.Where(
                e => e.Value.source.category == "skill"
                && e.Value.source.categorySub != "craft"
                && e.Value.source.categorySub != "combat"
                && e.Value.source.categorySub != "weapon").Select(x => x.Value).ToList();

            Header("製作"._("Craft"));
            _craftElementList = Create<ElementList>();
            _craftElementList.Container = chara.elements;
            _craftElementList.OnList = (m) => chara.elements.dict.Where(e => e.Value.source.categorySub == "craft").Select(x => x.Value).ToList();

            Header("戦闘"._("Battle"));
            _battleElementList = Create<ElementList>();
            _battleElementList.Container = chara.elements;
            _battleElementList.OnList = (m) => chara.elements.dict.Where(e => e.Value.source.categorySub == "combat").Select(x => x.Value).ToList();

            Header("武器"._("Weapon"));
            _weaponElementList = Create<ElementList>();
            _weaponElementList.Container = chara.elements;
            _weaponElementList.OnList = (m) => chara.elements.dict.Where(e => e.Value.source.categorySub == "weapon").Select(x => x.Value).ToList();

            Header("フィート"._("Feat"));
            _featElementList = Create<ElementList>();
            _featElementList.Container = chara.elements;
            _featElementList.OnList = (m) => chara.elements.dict.Where(e => e.Value.source.category == "feat").Select(x => x.Value).ToList();

            Header("スロット"._("Slot"));
            _slotElementList = Create<ElementList>();
            _slotElementList.Container = chara.elements;
            _slotElementList.OnList = (m) => chara.elements.dict.Where(e => e.Value.source.category == "slot").Select(x => x.Value).ToList();

            Header("変異"._("Mutation"));
            _mutationElementList = Create<ElementList>();
            _mutationElementList.Container = chara.elements;
            _mutationElementList.OnList = (m) => chara.elements.dict.Where(e => e.Value.source.category == "mutation").Select(x => x.Value).ToList();

            Header("エーテル"._("Ether"));
            _etherElementList = Create<ElementList>();
            _etherElementList.Container = chara.elements;
            _etherElementList.OnList = (m) => chara.elements.dict.Where(e => e.Value.source.category == "ether").Select(x => x.Value).ToList();

            RefreshElementList();
        }
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

    private ElementList? _attributeElementList;
    private ElementList? _generalElementList;
    private ElementList? _craftElementList;
    private ElementList? _battleElementList;
    private ElementList? _weaponElementList;
    private ElementList? _featElementList;
    private ElementList? _slotElementList;
    private ElementList? _mutationElementList;
    private ElementList? _etherElementList;

    private void RefreshElementList()
    {
        _attributeElementList?.Refresh();
        _generalElementList?.Refresh();
        _craftElementList?.Refresh();
        _battleElementList?.Refresh();
        _weaponElementList?.Refresh();
        _featElementList?.Refresh();
        _slotElementList?.Refresh();
        _mutationElementList?.Refresh();
        _etherElementList?.Refresh();
    }
}
