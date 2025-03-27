using YKDev.Lists;
using YKF;

namespace YKDev.Layers.Tabs;

public class DomainTab : YKLayout<object>
{
    public override void OnLayout()
    {
        _domainList = Create<DomainList>();
        _domainList.OnList = (m) =>
        {
            return EClass.sources.elements.rows.Where(e => e.categorySub == "eleAttack").ToList();
        };

        RefreshList();
    }

    private DomainList? _domainList;

    private void RefreshList()
    {
        _domainList?.Refresh();
    }
}
