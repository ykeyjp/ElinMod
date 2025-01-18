using YKF;

namespace YKDev.Layers;

public class LayerThingEditor : YKLayer<Thing>
{
    public override void OnLayout()
    {
        CreateTab<Tabs.ThingGeneralTab>("一般"._("General"), "yk.thing.general");
        CreateTab<Tabs.ThingEnchantTab>("エンチャント"._("Enchant"), "yk.thing.enchant");
        CreateTab<Tabs.ThingGeneTab>("遺伝子"._("Gene"), "yk.thing.gene");
        CreateTab<Tabs.ThingSocketTab>("ソケット"._("Socket"), "yk.thing.socket");
        CreateTab<Tabs.ThingAmmoTab>("矢弾"._("Arrow/Ammo"), "yk.thing.ammo");
    }
}
