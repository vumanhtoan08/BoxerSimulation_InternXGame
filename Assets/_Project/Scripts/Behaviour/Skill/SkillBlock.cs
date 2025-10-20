using UnityEngine;

public class SkillBlock : SkillBase
{
    private float blockValue;

    protected override void ActiveSkill(SKILL_TYPE type, float costStamina)
    {
        base.ActiveSkill(type, costStamina);
    }
}