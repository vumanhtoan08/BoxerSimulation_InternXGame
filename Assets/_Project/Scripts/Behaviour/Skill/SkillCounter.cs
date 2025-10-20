using UnityEngine;

public class SkillCounter : SkillBase
{
    private float attack;

    protected override void ActiveSkill(SKILL_TYPE type, float costStamina)
    {
        base.ActiveSkill(type, costStamina);
    }
}