using UnityEngine;
using YKF;

namespace YKDev.Layers.Tabs;

public class GeneralTab : YKLayout<object>
{
    public override void OnLayout()
    {
        {
            var group = Grid();
            group.Layout.cellSize = new Vector2(200, 50);
            group.Layout.constraintCount = 2;

            group.Button("ふっかつ！"._("Revive"), () =>
            {
                EClass.pc.Revive(null, false);
                EClass.pc.hp = EClass.pc.MaxHP;
            });
            group.Button("オレン（1M）"._("Oren (1M)"), () =>
            {
                var thing = ThingGen.Create("money", -1, -1).SetNum(1000000);
                EClass._zone.AddCard(thing, EClass.pc.pos);
            });
            group.Button("金塊（1M）"._("Gold (1M)"), () =>
            {
                var thing = ThingGen.Create("money2", -1, -1).SetNum(1000000);
                EClass._zone.AddCard(thing, EClass.pc.pos);
            });
            group.Button("旅糧（10）"._("Ration (10)"), () =>
            {
                var thing = ThingGen.Create("ration", -1, -1).SetNum(10);
                EClass._zone.AddCard(thing, EClass.pc.pos);
            });
            group.Button("プラチナ硬貨（1M）"._("Platinum (1M)"), () =>
            {
                var thing = ThingGen.Create("plat", -1, -1).SetNum(1000000);
                EClass._zone.AddCard(thing, EClass.pc.pos);
            });
            group.Button("矢（1K）"._("Arrow (1K)"), () =>
            {
                var thing = ThingGen.Create("arrow", 99, 60).SetNum(1000);
                EClass._zone.AddCard(thing, EClass.pc.pos);
            });
            group.Button("弾（1K）"._("Bullet (1K)"), () =>
            {
                var thing = ThingGen.Create("bullet", 99, 60).SetNum(1000);
                EClass._zone.AddCard(thing, EClass.pc.pos);
            });
            group.Button("すべてのレシピを習得"._("All recipes"), () =>
            {
                foreach (var recipeSource in RecipeManager.list)
                {
                    if (!EClass.player.recipes.knownRecipes.ContainsKey(recipeSource.id))
                    {
                        EClass.player.recipes.knownRecipes.Add(recipeSource.id, 1);
                    }
                }
            });
            group.Button("レシピをリセット"._("Reset recipes"), () =>
            {
                EClass.player.recipes.knownRecipes.Clear();
            });
            group.Button("エーテル病を治療"._("Treat ether disease"), () =>
            {
                CoreDebug.Fix_EtherDisease();
            });
            group.Button("マップを明らかにする"._("Map reveal all"), () =>
            {
                EClass._map.RevealAll(true);
            });
            group.Button("地殻変動"._("Crustal Movement"), () =>
            {
                EClass.world.region.RenewRandomSites();
            });
        }

        Header("デバッグ(フラグ)"._("Debug(flag)"));
        ELayer.debug.enable = false;
        ELayer.player.flags.debugEnabled = false;

        Toggle("表示拡張"._("Show Extra"), EClass.debug.showExtra, (isOn) =>
        {
            EClass.debug.showExtra = isOn;
        });
        Toggle("神モード"._("GOD MODE"), EClass.debug.godMode, (isOn) =>
        {
            EClass.debug.godMode = isOn;
        });
        Toggle("神モード（ビルド）"._("GOD MODE(Build)"), EClass.debug._godBuild, (isOn) =>
        {
            EClass.debug._godBuild = isOn;
            EClass.debug.ignoreBuildRule = isOn;
        });
        Toggle("神モード（クラフト）"._("GOD MODE(Craft)"), EClass.debug.godCraft, (isOn) =>
        {
            EClass.debug.godCraft = isOn;
        });
        Toggle("神モード（フード）"._("GOD MODE(Food)"), EClass.debug.godFood, (isOn) =>
        {
            EClass.debug.godFood = isOn;
        });
        Toggle("重量無視"._("Ignore weight"), EClass.debug.ignoreWeight, (isOn) =>
        {
            EClass.debug.ignoreWeight = isOn;
        });
        Toggle("AutoAdvanceQuest"._("AutoAdvanceQuest"), EClass.debug.autoAdvanceQuest, (isOn) =>
        {
            EClass.debug.autoAdvanceQuest = isOn;
        });
    }
}
