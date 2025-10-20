using UnityEngine;

public class EntityHealth : MonoBehaviour, IHealth
{
    protected float maxHealth;
    protected float currentHealth;
    protected PlayerRunTimeDatas data;
    protected Animator animator;

    public virtual void Init(PlayerController playerController)
    {
        animator = playerController.Animator;
        data = playerController.Data;

        maxHealth = data.DataRuntime.Health;
        currentHealth = maxHealth;
    }

    public virtual void ChangeHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        if (currentHealth <= 0)
        {
            Dead();
        }
        else
        {
            Hurt();
        }
    }

    protected virtual void Dead()
    {

    }

    protected virtual void Hurt()
    {

    }
}
