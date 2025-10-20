using UnityEngine;

public class SkillBase : MonoBehaviour
{
    [Header("Para")]
    [SerializeField] protected SKILL_TYPE skillType;
    protected float costStamina; 

    protected virtual void ActiveSkill(SKILL_TYPE type, float costStamina)
    {
        Debug.Log("Kich hoat skill" + type);
    }
}

public enum SKILL_TYPE
{
    Punch, 
    Counter, 
    Block
}