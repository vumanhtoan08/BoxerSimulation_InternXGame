using System.Collections.Generic;
using UnityEngine;

public class EntityHealth : MonoBehaviour, IHealth
{
    protected float maxHealth;
    protected float currentHealth;
    protected Animator animator;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

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