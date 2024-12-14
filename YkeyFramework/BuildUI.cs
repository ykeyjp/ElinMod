using System;

namespace YK;

public abstract class BuildUI<TA> : ELayer, IBuildUI<BuildUI<TA>, TA> where TA : IBuildUIArgs
{
    public virtual BuildUI<TA> Setup(TA args)
    {
        SE.Tab();
        return this;
    }

    public override void OnKill()
    {
        SE.PopWindow();
        base.OnKill();
    }

    private void LateUpdate()
    {
        foreach (Window window in this.windows)
        {
            window.cg.alpha = window.setting.transparent ? window.Skin.transparency : 1f;
        }
    }
}
