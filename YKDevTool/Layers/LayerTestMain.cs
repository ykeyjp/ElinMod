using YKF;

namespace YKDev.Layers;

public class LayerTestMain : YKLayer<object>
{
    public override void OnLayout()
    {
        CreateTab<Tabs.GeneralTab>("一般"._("General"), "yk.dev.general");
        CreateTab<Tabs.ZoneTab>("ゾーン"._("Zone"), "yk.dev.zone");
        CreateTab<Tabs.BranchTab>("拠点"._("Base"), "yk.dev.branch");
    }
}
