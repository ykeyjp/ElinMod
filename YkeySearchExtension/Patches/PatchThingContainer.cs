using HarmonyLib;

namespace YkeySearchExtension.Patches;

public static class PatchThingContainer
{
    [HarmonyPrefix, HarmonyPatch(typeof(ThingContainer), nameof(ThingContainer.RefreshGrid), [typeof(UIMagicChest), typeof(Window.SaveData)])]
    public static bool ThingContainer_RefreshGrid(ThingContainer __instance, UIMagicChest magic, Window.SaveData data)
    {
        var lastSearch = magic.lastSearch;
        if (lastSearch.IsEmpty()) return true;

        // オリジナルの準備処理
        magic.filteredList.Clear();
        magic.cats.Clear();
        magic.catCount.Clear();
        __instance.grid = new List<Thing>(new Thing[__instance.GridSize]);
        bool flag = !lastSearch.IsEmpty();
        bool flag2 = !magic.idCat.IsEmpty();
        Window.SaveData.CategoryType category = data.category;
        bool flag3 = category != Window.SaveData.CategoryType.None;
        string text = "";
        List<Thing> tempList = [];// カテゴリフィルター適用済みの一時リスト
        foreach (Thing thing in __instance)
        {
            if (flag3)
            {
                switch (category)
                {
                    case Window.SaveData.CategoryType.Main:
                        text = thing.category.GetRoot().id;
                        break;
                    case Window.SaveData.CategoryType.Sub:
                        text = thing.category.GetSecondRoot().id;
                        break;
                    case Window.SaveData.CategoryType.Exact:
                        text = thing.category.id;
                        break;
                }
                magic.cats.Add(text);
                if (magic.catCount.ContainsKey(text))
                {
                    Dictionary<string, int> catCount = magic.catCount;
                    string key = text;
                    int num = catCount[key];
                    catCount[key] = num + 1;
                }
                else
                {
                    magic.catCount.Add(text, 1);
                }
            }
            // --
            // アイテム名検索部分削除
            // --
            if (!flag2 || !(text != magic.idCat))
            {
                //magic.filteredList.Add(thing);
                tempList.Add(thing);
            }
        }
        // オリジナルここまで

        // 独自検索処理
        var ext = new SearchExtension(lastSearch);
        ext.onlyShop = false;
        ext.Execute(tempList);
        foreach (var c in ext.foundCard)
        {
            if (c is Thing t) magic.filteredList.Add(t);
        }

        // オリジナルの後処理
        if (flag2 && !magic.cats.Contains(magic.idCat))
        {
            magic.idCat = "";
            __instance.RefreshGrid(magic, data);
            return false;
        }
        magic.pageMax = (magic.filteredList.Count - 1) / __instance.GridSize;
        if (magic.page > magic.pageMax)
        {
            magic.page = magic.pageMax;
        }
        for (int i = 0; i < __instance.GridSize; i++)
        {
            int num2 = magic.page * __instance.GridSize + i;
            if (num2 >= magic.filteredList.Count)
            {
                break;
            }
            Thing thing2 = magic.filteredList[num2];
            __instance.grid[i] = thing2;
            thing2.invX = i;
        }
        magic.RefreshCats();
        // オリジナルここまで

        return false;
    }
}
