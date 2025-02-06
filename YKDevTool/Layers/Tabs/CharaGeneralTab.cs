using UnityEngine.UI;
using YKF;

namespace YKDev.Layers.Tabs;

public class CharaGeneralTab : YKLayout<Chara>
{
    public override void OnLayout()
    {
        var chara = Layer.Data;
        var headerWidth = 120;
        Header(chara.GetName(NameStyle.Full));

        {
            // 年齢
            {
                var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
                group.HeaderSmall("年齢"._("Age")).WithMinWidth(headerWidth);
                group.InputText(chara.bio.age.ToString(), (i) => { chara.bio.age = i; });
            }
            // 誕生日
            {
                var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
                group.HeaderSmall("誕生日"._("Birthday")).WithMinWidth(headerWidth);
                group.InputText(chara.bio.birthYear.ToString(), (i) => { chara.bio.birthYear = i; });
                group.InputText(chara.bio.birthMonth.ToString(), (i) => { chara.bio.birthMonth = i; });
                group.InputText(chara.bio.birthDay.ToString(), (i) => { chara.bio.birthDay = i; });
            }
        }
        {
            // 身長
            {
                var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
                group.HeaderSmall("身長"._("Height")).WithMinWidth(headerWidth);
                group.InputText(chara.bio.height.ToString(), (i) => { chara.bio.height = i; });
            }
            // 体重
            {
                var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
                group.HeaderSmall("体重"._("Weight")).WithMinWidth(headerWidth);
                group.InputText(chara.bio.weight.ToString(), (i) => { chara.bio.weight = i; });
            }

            // 性別
            {
                var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
                group.HeaderSmall("性別"._("Gender")).WithMinWidth(headerWidth);
                group.Dropdown(["???", "女性"._("Female"), "男性"._("Male")], chara.bio.SetGender, chara.bio.gender);
            }
        }
        {
            // 種族
            {
                var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
                group.HeaderSmall("種族"._("Race")).WithMinWidth(headerWidth);
                group.Dropdown(EClass.sources.races.rows.Select(x => x.GetName()).ToList(), (i) =>
                {
                    chara.ChangeRace(EClass.sources.races.rows[i].id);
                }, EClass.sources.races.rows.FindIndex((r) => r.id == chara.race.id)).WithWidth(150);
            }
            // 職業
            {
                var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
                group.HeaderSmall("職業"._("Job")).WithMinWidth(headerWidth);
                group.Dropdown(EClass.sources.jobs.rows.Select(x => x.GetName()).ToList(), (i) =>
                {
                    chara.ChangeJob(EClass.sources.jobs.rows[i].id);
                }, EClass.sources.jobs.rows.FindIndex((r) => r.id == chara.job.id)).WithWidth(150);
            }
        }
        if (!chara.IsPC)
        {
            // Lv
            {
                var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
                group.HeaderSmall("Lv").WithMinWidth(headerWidth);
                group.InputText(chara.LV.ToString(), (i) => { chara.SetLv(i); });
            }
            // 戦術
            {
                var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
                group.HeaderSmall("戦術"._("Tactics")).WithMinWidth(headerWidth);
                group.Dropdown(EClass.sources.tactics.rows.Select(x => x.GetName()).ToList(), (i) =>
                {
                    chara.tactics.source = EClass.sources.tactics.rows[i];
                }, EClass.sources.tactics.rows.FindIndex((j) => j.id == chara.tactics.source.id)).WithWidth(150);
            }
        }
        // 信仰
        {
            var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
            group.HeaderSmall("信仰"._("Faith")).WithMinWidth(headerWidth);
            group.Dropdown(EClass.sources.religions.rows.Select(x => x.GetName()).ToList(), (i) =>
            {
                chara.SetFaith(EClass.sources.religions.rows[i].id);
            }, EClass.sources.religions.rows.FindIndex((j) => j.id == chara.faith.id)).WithWidth(150);
            group.InputText(chara.elements.GetElement(85)?.vBase.ToString() ?? "0", (i) =>
            {
                chara.elements.SetBase(85, i, 0);
                chara.RefreshFaithElement();
            });
        }
        {
            // 趣味
            {
                var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
                group.HeaderSmall("趣味"._("Hobby")).WithMinWidth(headerWidth);
                group.Dropdown(EClass.sources.hobbies.listHobbies.Select(x => x.GetName()).ToList(), (i) =>
                {
                    chara._hobbies[0] = EClass.sources.hobbies.listHobbies[i].id;
                }, EClass.sources.hobbies.listHobbies.FindIndex((j) => j.id == chara._hobbies[0])).WithWidth(150);
            }
            // 仕事
            {
                var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
                group.HeaderSmall("仕事"._("Works")).WithMinWidth(headerWidth);
                group.Dropdown(EClass.sources.hobbies.listWorks.Select(x => x.GetName()).ToList(), (i) =>
                {
                    chara._works[0] = EClass.sources.hobbies.listWorks[i].id;
                }, EClass.sources.hobbies.listWorks.FindIndex((j) => j.id == chara._works[0])).WithWidth(150);
            }
        }
        if (chara.IsPC)
        {
            // カルマ
            {
                var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
                group.HeaderSmall("カルマ"._("Karma")).WithMinWidth(headerWidth);
                var num = group.InputText("").WithPlaceholder("加算、減算"._("plus, minus"));
                group.Button("取得"._("Gain"), () => { EClass.player.ModKarma(num.Num); });
            }
            // 名声
            {
                var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
                group.HeaderSmall("名声"._("Fame")).WithMinWidth(headerWidth);
                var num = group.InputText("").WithPlaceholder("加算、減算"._("plus, minus"));
                group.Button("取得"._("Gain"), () => { EClass.player.ModFame(num.Num); });
            }
        }
        // エーテル
        {
            var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
            group.HeaderSmall("エーテル進行度"._("Ether Progression")).WithMinWidth(headerWidth);
            var num = group.InputText("").WithPlaceholder("加算、減算"._("plus, minus"));
            group.Button("取得"._("Gain"), () => { chara.ModCorruption(num.Num); });
        }
        // 貢献
        if (chara.IsPC)
        {
            var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
            group.HeaderSmall("ギルド貢献"._("Contribution")).WithMinWidth(headerWidth);
            var num = group.InputText("").WithPlaceholder("加算、減算"._("plus, minus"));
            group.Button("取得"._("Gain"), () => { Guild.GetCurrentGuild()?.AddContribution(num.Num); });
        }
        else
        {
            // 友好度
            var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
            group.HeaderSmall("友好度"._("Friendship")).WithMinWidth(headerWidth);
            var num = group.InputText(chara._affinity.ToString(), (i) => { chara._affinity = i; });
        }
        // フィートP
        {
            var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
            group.HeaderSmall("フィートP"._("Feat P")).WithMinWidth(headerWidth);
            var num = group.InputText(chara.feat.ToString(), (i) => { chara.feat = i; });
        }
        // 飢餓
        {
            var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
            group.HeaderSmall("空腹度"._("Hunger")).WithMinWidth(headerWidth);
            var num = group.InputText(chara.hunger.GetValue().ToString(), (i) => { chara.hunger.Set(i); });
        }
        // 衛生
        {
            var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
            group.HeaderSmall("衛生"._("Hygiene")).WithMinWidth(headerWidth);
            var num = group.InputText(chara.hygiene.GetValue().ToString(), (i) => { chara.hygiene.Set(i); });
        }
        // 眠気
        {
            var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
            group.HeaderSmall("眠気"._("Drowsiness")).WithMinWidth(headerWidth);
            var num = group.InputText(chara.sleepiness.GetValue().ToString(), (i) => { chara.sleepiness.Set(i); });
        }
        // トイレ
        {
            var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
            group.HeaderSmall("トイレ"._("Bladder")).WithMinWidth(headerWidth);
            var num = group.InputText(chara.bladder.GetValue().ToString(), (i) => { chara.bladder.Set(i); });
        }
        // HP
        {
            var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
            group.HeaderSmall("HP").WithMinWidth(headerWidth);
            var num = group.InputText(chara.hp.ToString(), (i) => { chara.hp = i == 0 ? chara.MaxHP : i; });
        }
        // マナ
        {
            var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
            group.HeaderSmall("マナ"._("Mana")).WithMinWidth(headerWidth);
            var num = group.InputText(chara.mana.GetValue().ToString(), (i) => { chara.mana.Set(i); });
        }
        // スタミナ
        {
            var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
            group.HeaderSmall("スタミナ"._("Stamina")).WithMinWidth(headerWidth);
            var num = group.InputText(chara.stamina.GetValue().ToString(), (i) => { chara.stamina.Set(i); });
        }

        if (!chara.IsPC)
        {
            // トレジャー
            Header("トレジャー"._("Treasure"));
            {
                var group = Horizontal();
                group.Button("カード"._("Card"), () =>
                {
                    var r = ThingGen.Create("figure3");
                    r.MakeRefFrom(chara.source.id);
                    EClass._zone.TryAddThing(r, EClass.pc.pos);
                });
                group.Button("剥製"._("Stuffed"), () =>
                {
                    var r = ThingGen.Create("figure");
                    r.MakeFigureFrom(chara.source.id);
                    EClass._zone.TryAddThing(r, EClass.pc.pos);
                });
                group.Button("有精卵"._("Fertilized eggs"), () =>
                {
                    var t = ThingGen.Create("egg_fertilized").SetNum(1);
                    t.MakeFoodFrom(chara);
                    t.c_idMainElement = chara.c_idMainElement;
                    EClass._zone.TryAddThing(t, EClass.pc.pos);
                });
                group.Button("ミルク"._("Milk"), () =>
                {
                    var t = ThingGen.Create("milk").SetNum(1);
                    t.MakeRefFrom(chara);
                    var num2 = chara.LV - chara.source.LV;
                    if (num2 >= 10) { t.SetEncLv(num2 / 10); }
                    EClass._zone.TryAddThing(t, EClass.pc.pos);
                });
                group.Button("クローン"._("Clone"), () =>
                {
                    var c = chara.Duplicate();
                    EClass._zone.AddCard(c, EClass.pc.pos);
                    c.MakePartyMemeber();
                });
            }
            // DNA
            {
                var group = Horizontal().WithFitMode(ContentSizeFitter.FitMode.PreferredSize).WithPivot(0f, 0.5f);
                group.HeaderSmall("遺伝子"._("Gene")).WithMinWidth(headerWidth);
                var typelist = new DNA.Type[] { DNA.Type.Default, DNA.Type.Superior, DNA.Type.Inferior, DNA.Type.Brain };
                var typenames = new List<string> { "普通"._("Normal"), "優性"._("Superior"), "劣性"._("Inferior"), "脳"._("Brain") };
                var dropdown = group.Dropdown(typenames);
                group.Button("生成"._("Generate"), () =>
                {
                    var t = DNA.GenerateGene(chara, typelist[dropdown.value]);
                    EClass._zone.TryAddThing(t, EClass.pc.pos);
                });
            }
        }
    }
}
