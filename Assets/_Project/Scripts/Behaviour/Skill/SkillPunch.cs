using UnityEngine; 

public class SkillPunch : SkillBase
{
    private float attack;

    protected override void ActiveSkill(SKILL_TYPE type, float costStamina)
    {
        base.ActiveSkill(type, costStamina);
    }
}