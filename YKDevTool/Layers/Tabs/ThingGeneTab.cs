using UnityEngine.UI;
using YKDev.Lists;
using YKF;

namespace YKDev.Layers.Tabs;

public class ThingGeneTab : YKLayout<Thing>
{
    public override void OnLayout()
    {
        var thing = Layer.Data;
        var headerWidth = 120;
        Header(thing.GetName(NameStyle.Full));

        if (thing == null || thing.c_DNA == null)
        {
            Text("DNAに対応していません"._("Not compatible with DNA")).WithWidth(300);
            return;
        }

        var attributeList = EClass.sources.elements.rows.Where(e => e.category == "attribute").ToArray();
        var skillList = EClass.sources.elements.rows.Where(e => e.category == "skill").ToArray();
        var enchantList = EClass.sources.elements.rows.Where(e => e.category == "enchant").ToArray();
        var spellList = EClass.sources.elements.rows.Where(e => e.group == "SPELL" && e.alias.Last() != '_').ToArray();
        var resistList = EClass.sources.elements.rows.Where(e => e.category == "resist").ToArray();
        var featList = EClass.sources.elements.rows.Where(e => e.category == "feat").ToArray();
        var slotList = EClass.sources.elements.rows.Where(e => e.category == "slot").ToArray();
        var mutationtList = EClass.sources.elements.rows.Where(e => e.category == "mutation").ToArray();
        var etherList = EClass.sources.elements.rows.Where(e => e.category == "ether").ToArray();
        var otherList = EClass.sources.elements.rows.Where(e => (e.group == "ELEMENT" || e.group == "SKILL") && e.category == "").ToArray();

        // タイプ
        {
            var typeText = new string[] { "普通"._("Normal"), "優性"._("Superior"), "優性"._("Inferior") };
            var typeList = new List<DNA.Type> { DNA.Type.Default, DNA.Type.Superior, DNA.Type.Inferior };
            var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
            group.HeaderSmall("タイプ"._("Type")).WithMinWidth(headerWidth);
            group.Dropdown([.. typeText], (i) =>
            {
                thing.c_DNA.type = typeList[i];
            }, (int)(thing.blessedState + 2)).WithWidth(150);
        }
        // LV
        {
            var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
            group.HeaderSmall("Lv").WithMinWidth(headerWidth);
            group.InputText(thing.c_DNA.lv.ToString(), (i) => { thing.c_DNA.lv = i; });
        }
        // コスト
        {

            var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
            group.HeaderSmall("コスト"._("Cost")).WithMinWidth(headerWidth);
            group.InputText(thing.c_DNA.cost.ToString(), (i) => { thing.c_DNA.cost = i; });
        }

        // 能力
        {
            var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
            group.HeaderSmall("能力"._("Attribute")).WithMinWidth(headerWidth);
            var dropdown = group.Dropdown(attributeList.Select(x => x.GetName()).ToList()).WithWidth(150);
            var baseInput = group.InputText("").WithPlaceholder("ベース"._("Base"));
            group.Button("取得"._("Gain"), () =>
            {
                ApplyGene(thing, attributeList[dropdown.value], baseInput.Num);
                RefreshGeneList();
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
                ApplyGene(thing, skillList[dropdown.value], baseInput.Num);
                RefreshGeneList();
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
                ApplyGene(thing, enchantList[dropdown.value], baseInput.Num);
                RefreshGeneList();
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
                ApplyGene(thing, spellList[dropdown.value], baseInput.Num);
                RefreshGeneList();
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
                ApplyGene(thing, resistList[dropdown.value], baseInput.Num);
                RefreshGeneList();
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
                ApplyGene(thing, otherList[dropdown.value], baseInput.Num);
                RefreshGeneList();
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
                ApplyGene(thing, featList[dropdown.value], baseInput.Num);
                RefreshGeneList();
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
                ApplyGene(thing, slotList[dropdown.value], baseInput.Num);
                RefreshGeneList();
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
                ApplyGene(thing, mutationtList[dropdown.value], baseInput.Num);
                RefreshGeneList();
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
                ApplyGene(thing, etherList[dropdown.value], baseInput.Num);
                RefreshGeneList();
            });
        }

        // 現在値
        {
            Header("DNA");
            _geneList = Create<GeneList>();
            _geneList.Thing = thing;
            _geneList.OnList = (m) =>
            {
                var genes = new List<GeneList.Gene> { };
                for (var i = 0; i < thing.c_DNA.vals.Count; i += 2)
                {
                    var id = thing.c_DNA.vals[i];
                    var lv = thing.c_DNA.vals[i + 1];
                    genes.Add(new GeneList.Gene { Id = id, Lv = lv });
                }

                return genes;
            };
            _geneList.OnRemove = RefreshGeneList;
        }

        RefreshGeneList();
    }

    private void ApplyGene(Thing thing, SourceElement.Row el, int lv)
    {
        var ok = false;
        for (var i = 0; i < thing.c_DNA.vals.Count; i += 2)
        {
            if (thing.c_DNA.vals[i] == el.id)
            {
                thing.c_DNA.vals[i + 1] = lv;
                ok = true;
                break;
            }
        }
        if (!ok)
        {
            thing.c_DNA.vals.Add(el.id);
            thing.c_DNA.vals.Add(lv);
        }
    }

    private GeneList? _geneList;

    private void RefreshGeneList()
    {
        _geneList?.Refresh();
    }
}
