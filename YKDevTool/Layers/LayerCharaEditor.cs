using YKF;

namespace YKDev.Layers;

public class LayerCharaEditor : YKLayer<Chara>
{
    public override void OnLayout()
    {
        CreateTab<Tabs.CharaGeneralTab>("一般"._("General"), "yk.chara.general");
        CreateTab<Tabs.CharaSkillTab>("スキル"._("Skill"), "yk.chara.skill");
        CreateTab<Tabs.CharaAbilityTab>("アビリティ"._("Ability"), "yk.chara.ability");
        CreateTab<Tabs.CharaSpellTab>("魔法"._("Spell"), "yk.chara.spell");
        CreateTab<Tabs.CharaActionTab>("行動"._("Action"), "yk.chara.action");
    }
}
