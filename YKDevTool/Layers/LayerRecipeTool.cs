using UnityEngine;
using YKF;

namespace YKDev.Layers;

public class LayerRecipeTool : YKLayer<object>
{
    public override void OnLayout()
    {
        CreateTab<Tabs.RecipeSourceTab>("レシピ"._("Recipe"), "yk.recipe.source");
    }

    public override Rect Bound => new(0, 0, 1000, 600);
}
