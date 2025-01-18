using YKDev.Lists;
using YKF;

namespace YKDev.Layers.Tabs;

public class RecipeSourceTab : YKLayout<object>
{
    private static readonly string[] s_categories;

    static RecipeSourceTab()
    {
        RecipeManager.BuildList();
        s_categories = EClass.sources.categories.map.Where(r => !r.Value._parent.IsEmpty()).Select(x => x.Key).ToArray();
    }

    private string searchCategory = "";

    public override void OnLayout()
    {
        {
            var group = Horizontal();
            group.Layout.childControlWidth = false;
            group.Layout.spacing = 20;

            group.Header("カテゴリー"._("Category"));
            var cat = new List<string> { "----" };
            cat.AddRange(s_categories.Select(x => EClass.sources.categories.map[x].GetName() ?? ""));
            var dropdown = group.Dropdown(cat).WithWidth(200);
            dropdown.onValueChanged.AddListener((i) =>
            {
                RefreshList(i == 0 ? "" : s_categories[i - 1]);
            });
        }

        {
            Header("レシピ"._("Recipe"));
            _recipeList = Create<RecipeSourceList>();
            _recipeList.OnList = (m) =>
            {
                if (searchCategory == "") return [];
                return RecipeManager.list.Where(x => x.row.category == searchCategory).ToList();
            };
        }

        RefreshList("");
    }

    private RecipeSourceList? _recipeList;

    private void RefreshList(string category)
    {
        searchCategory = category;
        _recipeList?.Refresh();
    }
}
