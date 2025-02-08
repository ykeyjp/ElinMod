using UnityEngine;
using YKDev.Lists;
using YKF;

namespace YKDev.Layers.Tabs;

public class BranchTab : YKLayout<object>
{
    static readonly SourceElement.Row[] s_sourceElements = [];
    protected FactionBranch _branch = EClass._zone.branch;

    static BranchTab()
    {
        s_sourceElements = EClass.sources.elements.rows.Where((e) => { return e.group == "FACTION" || e.group == "POLICY"; }).ToArray();
    }

    public override void OnLayout()
    {
        if (_branch == null)
        {
            transform.SetActive(false);
            return;
        }

        Header(_branch.owner.Name);

        Button("掃除（掃除や雑用のスキルが必要）"._("Cleaning(required: Cleaning, Chore)"), () =>
        {
            _branch.AutoClean();
        });

        {
            var hgroup = Horizontal();
            hgroup.Layout.spacing = 20;
            UIInputText? lvInput = null;

            // 経験値
            {
                var group = hgroup.Horizontal();
                group.HeaderSmall("経験値"._("Exp."));
                var input = group.InputText("");
                group.Button("取得"._("Gain"), () =>
                {
                    _branch.ModExp(input.Num);
                    if (lvInput != null) lvInput.Num = _branch.lv;
                });
            }

            // Lv
            {
                var group = hgroup.Horizontal();
                group.HeaderSmall("Lv");
                lvInput = group.InputText(_branch.lv.ToString());
                group.Button("適用"._("Apply"), () =>
                {
                    _branch.lv = lvInput.Num;
                });
            }
        }

        {
            var group = Horizontal();

            group.HeaderSmall("スキル"._("Skill")).WithWidth(100);
            var dropdown = group.Dropdown(s_sourceElements.Select(x => x.GetName()).ToList()).WithWidth(200);
            var baseInput = group.InputText("").WithPlaceholder("ベース"._("Base"));
            var potentialInput = group.InputText("").WithPlaceholder("潜在"._("Potential"));

            var button = group.Button("適用"._("Apply"), () =>
            {
                var el = s_sourceElements[dropdown.value];
                if (baseInput.Num == 0)
                {
                    _branch.elements.Remove(el.id);
                }
                else
                {
                    if (!_branch.elements.Has(el))
                    {
                        if (el.group == "POLICY")
                        {
                            _branch.policies.AddPolicy(el.id);
                        }
                        else if (el.category == "landfeat")
                        {
                            _branch.AddFeat(el.id, baseInput.Num);
                        }
                    }

                    _branch.elements.SetBase(el.id, baseInput.Num, potentialInput.Num);
                }

                RefreshList();
            });
        }

        {
            Header("スキル"._("Skill"));
            _skillList = Create<ElementList>();
            _skillList.Container = _branch.elements;
            _skillList.OnList = (m) => _branch.elements.dict.Where(e => e.Value.GetType() == typeof(Skill)).Where(e => e.Value.source.group != "POLICY").Select(e => e.Value).ToList();

            Header("フィート"._("Feat"));
            _featList = Create<ElementList>();
            _featList.Container = _branch.elements;
            _featList.OnList = (m) => _branch.elements.dict.Where(e => e.Value.GetType() == typeof(LandFeat)).Select(x => x.Value).ToList();

            Header("ポリシー"._("Policy"));
            _policyList = Create<ElementList>();
            _policyList.Container = _branch.elements;
            _policyList.OnList = (m) => _branch.elements.dict.Where(e => e.Value.source.category == "policy").Select(x => x.Value).ToList();
        }

        RefreshList();
    }

    private ElementList? _skillList;
    private ElementList? _featList;
    private ElementList? _policyList;

    private void RefreshList()
    {
        _skillList?.Refresh();
        _featList?.Refresh();
        _policyList?.Refresh();
    }
}
