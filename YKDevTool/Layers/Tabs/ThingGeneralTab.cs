using UnityEngine.UI;
using YKF;

namespace YKDev.Layers.Tabs;

public class ThingGeneralTab : YKLayout<Thing>
{
    public override void OnLayout()
    {
        var thing = Layer.Data;
        var headerWidth = 120;
        Header(thing.GetName(NameStyle.Full));

        // 素材
        {
            var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
            group.HeaderSmall("素材"._("Material")).WithMinWidth(headerWidth);
            group.Dropdown(EClass.sources.materials.rows.Select(x => x.GetName()).ToList(), (i) =>
            {
                thing.ChangeMaterial(EClass.sources.materials.rows[i].id);
            }, EClass.sources.materials.rows.FindIndex(m => m.id == thing.material.id)).WithWidth(150);
        }
        // ランク
        {
            var qualityText = Lang.GetList("quality");
            //Rarity.Crude
            //Rarity.Normal
            //Rarity.Superior
            //Rarity.Legendary
            //Rarity.Mythical
            //Rarity.Artifact
            var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
            group.HeaderSmall("品質"._("Quality")).WithMinWidth(headerWidth);
            group.Dropdown([.. qualityText], (i) =>
            {
                thing.ChangeRarity((Rarity)(i - 1));
            }, (int)(thing.rarity + 1)).WithWidth(150);
        }
        // 祝福
        {
            var blessedText = new string[] { "堕落"._("Doomed"), "呪い"._("Cursed"), "通常"._("Normal"), "祝福"._("Blessed") };
            // BlessedState.Doomed
            // BlessedState.Cursed
            // BlessedState.Normal
            // BlessedState.Blessed
            var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
            group.HeaderSmall("祝福"._("Bless")).WithMinWidth(headerWidth);
            group.Dropdown([.. blessedText], (i) =>
            {
                thing.SetBlessedState((BlessedState)(i - 2));
            }, (int)(thing.blessedState + 2)).WithWidth(150);
        }
        // 識別
        {
            var idText = new string[] { "識別済み"._("Identified"), "鑑定が必要"._("Needs appraisal"), "上位鑑定が必要"._("Needs higher appraisal") };
            var c_IDTState = 0;
            switch (thing.c_IDTState)
            {
                case 0:
                    c_IDTState = 0;
                    break;
                case 1:
                    c_IDTState = 1;
                    break;
                case 2:
                case 3:
                    c_IDTState = 2;
                    break;
            }
            var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
            group.HeaderSmall("識別"._("Identify")).WithMinWidth(headerWidth);
            group.Dropdown([.. idText], (i) =>
            {
                switch (i)
                {
                    case 0:
                        thing.c_IDTState = 0;
                        break;
                    case 1:
                        thing.c_IDTState = 1;
                        break;
                    case 2:
                        thing.c_IDTState = 3;
                        break;
                }
            }, c_IDTState).WithWidth(150);
        }
        // 重量
        {
            var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
            group.HeaderSmall("重量"._("Weight")).WithMinWidth(headerWidth);
            group.InputText(thing.SelfWeight.ToString(), thing.ChangeWeight);
        }
        // 個数
        {
            var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
            group.HeaderSmall("個数"._("Quantity")).WithMinWidth(headerWidth);
            group.InputText(thing.Num.ToString(), (i) => { thing.SetNum(i); });
        }
        if (thing.trait.HasCharges)
        {
            // 回数
            {
                var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
                group.HeaderSmall("回数"._("Times")).WithMinWidth(headerWidth);
                group.InputText(thing.c_charges.ToString(), thing.SetCharge);
            }
        }
        // コンテナ
        if (thing.HasContainerSize)
        {
            var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
            group.HeaderSmall("コンテナ"._("Container")).WithMinWidth(headerWidth);
            var wInput = group.InputText(thing.things.width.ToString()).WithPlaceholder("列数"._("Cols"));
            var hInput = group.InputText(thing.things.height.ToString()).WithPlaceholder("行数"._("Rows"));
            group.Button("適用"._("Apply"), () =>
            {
                thing.things.ChangeSize(Math.Max(wInput.Num, 1), Math.Max(hInput.Num, 1));
            });
        }
        // 強化
        {
            var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
            group.HeaderSmall("強化"._("Enhancement")).WithMinWidth(headerWidth);
            group.InputText(thing.encLV.ToString(), thing.SetEncLv);
        }
        // 複製
        {
            var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
            group.HeaderSmall("複製"._("Duplicate")).WithMinWidth(headerWidth);
            var numInput = group.InputText("1").WithPlaceholder("個数"._("Quantity"));
            group.Button("複製"._("Duplicate"), () =>
            {
                var t = thing.Duplicate(Math.Max(numInput.Num, 1));
                EClass._zone.AddCard(t, EClass.pc.pos);
            });
        }
        {
            var group = Horizontal();
            group.Button("消滅 (危険!)"._("Destroy (Danger!)"), () =>
            {
                thing.Destroy();
            }).WithWidth(200);
        }
    }
}
