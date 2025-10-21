using UnityEngine;

public class EnemyHealth : EntityHealth
{

    public void Init(EnemyController enemyController)
    {
        this.animator = enemyController.Animator;

        currentHealth = maxHealth;
    }

    protected override void Dead()
    {
        base.Dead();
        animator.SetTrigger("isDead");
    }

    protected override void Hurt()
    {
        base.Hurt();
        animator.SetTrigger("isHeadHit");
    }
}