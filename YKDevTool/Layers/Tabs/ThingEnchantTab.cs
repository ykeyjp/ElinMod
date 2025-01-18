using UnityEngine.UI;
using YKDev.Lists;
using YKF;

namespace YKDev.Layers.Tabs;

public class ThingEnchantTab : YKLayout<Thing>
{
    public override void OnLayout()
    {
        var thing = Layer.Data;
        var headerWidth = 120;
        Header(thing.GetName(NameStyle.Full));

        var attributeList = EClass.sources.elements.rows.Where(e => e.category == "attribute").ToArray();
        var skillList = EClass.sources.elements.rows.Where(e => e.category == "skill").ToArray();
        var enchantList = EClass.sources.elements.rows.Where(e => e.category == "enchant").ToArray();
        var resistList = EClass.sources.elements.rows.Where(e => e.category == "resist").ToArray();
        var spellList = EClass.sources.elements.rows.Where((e) => e.group == "SPELL" && e.alias.Last() != '_').ToArray();
        var featList = EClass.sources.elements.rows.Where(e => e.category == "feat").ToArray();
        var slotList = EClass.sources.elements.rows.Where(e => e.category == "slot").ToArray();
        var mutationtList = EClass.sources.elements.rows.Where(e => e.category == "mutation").ToArray();
        var etherList = EClass.sources.elements.rows.Where(e => e.category == "ether").ToArray();
        var otherList = EClass.sources.elements.rows.Where(e => (e.group == "ELEMENT" || e.group == "SKILL") && e.category == "").ToArray();

        // 能力
        {
            var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
            group.HeaderSmall("能力"._("Attribute")).WithMinWidth(headerWidth);
            var dropdown = group.Dropdown(attributeList.Select(x => x.GetName()).ToList()).WithWidth(150);
            var baseInput = group.InputText("").WithPlaceholder("ベース"._("Base"));
            group.Button("取得"._("Gain"), () =>
            {
                GainElement(thing, attributeList[dropdown.value], baseInput.Num);
                RefreshElementList();
            });
        }
        // スキル
        {
            var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
            group.HeaderSmall("スキル"._("Skill")).WithMinWidth(headerWidth);
            var dropdown = group.Dropdown(skillList.Select(x => x.GetName()).ToList()).WithWidth(150);
            var baseInput = group.InputText("").WithPlaceholder("ベース"._("Base"));
            group.Button("取得"._("Gain"), () =>
            {
                GainElement(thing, skillList[dropdown.value], baseInput.Num);
                RefreshElementList();
            });
        }
        // エンチャント
        {
            var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
            group.HeaderSmall("エンチャント"._("Enchant")).WithMinWidth(headerWidth);
            var dropdown = group.Dropdown(enchantList.Select(x => x.GetName()).ToList()).WithWidth(150);
            var baseInput = group.InputText("").WithPlaceholder("ベース"._("Base"));
            group.Button("取得"._("Gain"), () =>
            {
                GainElement(thing, enchantList[dropdown.value], baseInput.Num);
                RefreshElementList();
            });
        }
        // 耐性
        {
            var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
            group.HeaderSmall("耐性"._("Resist")).WithMinWidth(headerWidth);
            var dropdown = group.Dropdown(resistList.Select(x => x.GetName()).ToList()).WithWidth(150);
            var baseInput = group.InputText("").WithPlaceholder("ベース"._("Base"));
            group.Button("取得"._("Gain"), () =>
            {
                GainElement(thing, resistList[dropdown.value], baseInput.Num);
                RefreshElementList();
            });
        }
        // 魔法
        {
            var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
            group.HeaderSmall("魔法"._("Spell")).WithMinWidth(headerWidth);
            var dropdown = group.Dropdown(spellList.Select(x => x.GetName()).ToList()).WithWidth(150);
            var baseInput = group.InputText("").WithPlaceholder("ベース"._("Base"));
            group.Button("取得"._("Gain"), () =>
            {
                GainElement(thing, spellList[dropdown.value], baseInput.Num);
                RefreshElementList();
            });
        }
        // その他
        {
            var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
            group.HeaderSmall("その他"._("Others")).WithMinWidth(headerWidth);
            var dropdown = group.Dropdown(otherList.Select(x => x.GetName()).ToList()).WithWidth(150);
            var baseInput = group.InputText("").WithPlaceholder("ベース"._("Base"));
            group.Button("取得"._("Gain"), () =>
            {
                GainElement(thing, otherList[dropdown.value], baseInput.Num);
                RefreshElementList();
            });
        }
        // フィート
        {
            var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
            group.HeaderSmall("フィート"._("Feat")).WithMinWidth(headerWidth);
            var dropdown = group.Dropdown(featList.Select(x => x.GetName()).ToList()).WithWidth(150);
            var baseInput = group.InputText("").WithPlaceholder("ベース"._("Base"));
            group.Button("取得"._("Gain"), () =>
            {
                GainElement(thing, featList[dropdown.value], baseInput.Num);
                RefreshElementList();
            });
        }
        // スロット
        {
            var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
            group.HeaderSmall("スロット"._("Slot")).WithMinWidth(headerWidth);
            var dropdown = group.Dropdown(slotList.Select(x => x.GetName()).ToList()).WithWidth(150);
            var baseInput = group.InputText("").WithPlaceholder("ベース"._("Base"));
            group.Button("取得"._("Gain"), () =>
            {
                GainElement(thing, slotList[dropdown.value], baseInput.Num);
                RefreshElementList();
            });
        }
        // 変異
        {
            var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
            group.HeaderSmall("変異"._("Mutation")).WithMinWidth(headerWidth);
            var dropdown = group.Dropdown(mutationtList.Select(x => x.GetName()).ToList()).WithWidth(150);
            var baseInput = group.InputText("").WithPlaceholder("ベース"._("Base"));
            group.Button("取得"._("Gain"), () =>
            {
                GainElement(thing, mutationtList[dropdown.value], baseInput.Num);
                RefreshElementList();
            });
        }
        // エーテル
        {
            var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
            group.HeaderSmall("エーテル"._("Ether")).WithMinWidth(headerWidth);
            var dropdown = group.Dropdown(etherList.Select(x => x.GetName()).ToList()).WithWidth(150);
            var baseInput = group.InputText("").WithPlaceholder("ベース"._("Base"));
            group.Button("取得"._("Gain"), () =>
            {
                GainElement(thing, etherList[dropdown.value], baseInput.Num);
                RefreshElementList();
            });
        }

        // 現在値
        {
            Header("主能力"._("Main"));
            _attributeElementList = Create<ElementSingleList>();
            _attributeElementList.Container = thing.elements;
            _attributeElementList.OnList = (m) => thing.elements.dict.Where(e => e.Value is AttbMain).Select(x => x.Value).ToList();

            Header("スキル"._("Skill"));
            _skillElementList = Create<ElementSingleList>();
            _skillElementList.Container = thing.elements;
            _skillElementList.OnList = (m) => thing.elements.dict.Where(e => e.Value.source.category == "skill").Select(x => x.Value).ToList();

            Header("エンチャント"._("Enchant"));
            _enchantElementList = Create<ElementSingleList>();
            _enchantElementList.Container = thing.elements;
            _enchantElementList.OnList = (m) => thing.elements.dict.Where(e => e.Value.source.category == "enchant").Select(x => x.Value).ToList();

            Header("耐性"._("Resist"));
            _resistElementList = Create<ElementSingleList>();
            _resistElementList.Container = thing.elements;
            _resistElementList.OnList = (m) => thing.elements.dict.Where(e => e.Value.source.category == "resist").Select(x => x.Value).ToList();

            Header("魔法"._("Spell"));
            _spellElementList = Create<ElementSingleList>();
            _spellElementList.Container = thing.elements;
            _spellElementList.OnList = (m) => thing.elements.dict.Where(e => e.Value is Spell).Select(x => x.Value).ToList();

            Header("その他"._("Others"));
            _otherElementList = Create<ElementSingleList>();
            _otherElementList.Container = thing.elements;
            _otherElementList.OnList = (m) => thing.elements.dict.Where(e => (e.Value.source.group == "ELEMENT" || e.Value.source.group == "SKILL") && e.Value.source.category == "").Select(x => x.Value).ToList();

            Header("フィート"._("Feat"));
            _featElementList = Create<ElementSingleList>();
            _featElementList.Container = thing.elements;
            _featElementList.OnList = (m) => thing.elements.dict.Where(e => e.Value.source.category == "feat").Select(x => x.Value).ToList();

            Header("スロット"._("Slot"));
            _slotElementList = Create<ElementSingleList>();
            _slotElementList.Container = thing.elements;
            _slotElementList.OnList = (m) => thing.elements.dict.Where(e => e.Value.source.category == "slot").Select(x => x.Value).ToList();

            Header("変異"._("Mutation"));
            _mutationElementList = Create<ElementSingleList>();
            _mutationElementList.Container = thing.elements;
            _mutationElementList.OnList = (m) => thing.elements.dict.Where(e => e.Value.source.category == "mutation").Select(x => x.Value).ToList();

            Header("エーテル"._("Ether"));
            _etherElementList = Create<ElementSingleList>();
            _etherElementList.Container = thing.elements;
            _etherElementList.OnList = (m) => thing.elements.dict.Where(e => e.Value.source.category == "ether").Select(x => x.Value).ToList();

            RefreshElementList();
        }
    }

    private void GainElement(Thing thing, SourceElement.Row el, int lv)
    {
        if (lv == 0)
        {
            thing.elements.Remove(el.id);
        }
        else
        {
            var element = thing.elements.GetOrCreateElement(el.id);
            if (element.ValueWithoutLink == 0)
            {
                thing.elements.ModBase(el.id, 1);
            }
            thing.elements.SetBase(el.id, lv);
        }
    }

    private ElementSingleList? _attributeElementList;
    private ElementSingleList? _skillElementList;
    private ElementSingleList? _enchantElementList;
    private ElementSingleList? _resistElementList;
    private ElementSingleList? _spellElementList;
    private ElementSingleList? _otherElementList;
    private ElementSingleList? _featElementList;
    private ElementSingleList? _slotElementList;
    private ElementSingleList? _mutationElementList;
    private ElementSingleList? _etherElementList;

    private void RefreshElementList()
    {
        _attributeElementList?.Refresh();
        _skillElementList?.Refresh();
        _enchantElementList?.Refresh();
        _resistElementList?.Refresh();
        _spellElementList?.Refresh();
        _otherElementList?.Refresh();
        _featElementList?.Refresh();
        _slotElementList?.Refresh();
        _mutationElementList?.Refresh();
        _etherElementList?.Refresh();
    }
}
