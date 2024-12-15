using UnityEngine;
using YkeyCraftIntegration.Patches;

namespace YkeyCraftIntegration
{
    public class YKLinkedFactory : Thing
    {
        public YKLinkedFactory()
        {
            trait = new TraitLinkedFactory();
        }

        public class TraitLinkedFactory : TraitFactory
        {
            public override ToggleType ToggleType
            {
                get
                {
                    return ToggleType.None;
                }
            }

            public override bool Contains(RecipeSource r)
            {
                foreach (var t in PatchLayerCraft.factories)
                {
                    if (r.idFactory == t.id || r.idFactory == "self")
                    {
                        return true;
                    }
                }

                return false;
            }
        }
    }
}
