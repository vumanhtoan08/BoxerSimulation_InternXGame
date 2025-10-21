using UnityEngine;

public class EnemyHealth : EntityHealth
{
    private EnemyController enemyController;
    private bool isDead; 
    public bool IsDead => isDead;

    public void Init(EnemyController enemyController)
    {
        this.animator = enemyController.Animator;
        this.enemyController = enemyController;

        currentHealth = maxHealth;
        isDead = false;
    }

    protected override void Dead()
    {
        base.Dead();
        animator.SetTrigger("isDead");
        isDead = true;
    }

    protected override void Hurt()
    {
        base.Hurt();
        animator.SetTrigger("isHeadHit");
        enemyController.StateMachine.ChangeState(new EnemyIdleState(enemyController));
    }
}