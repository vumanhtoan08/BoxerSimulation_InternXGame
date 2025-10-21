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

        maxHealth = enemyController.RuntimeData.EnemyData.Health;
        currentHealth = maxHealth;
        isDead = false;
    }

    protected override void Dead()
    {
        base.Dead();
        enemyController.StateMachine.ChangeState(new EnemyDeadState(enemyController));
        isDead = true;
    }

    protected override void Hurt()
    {
        base.Hurt();
        enemyController.StateMachine.ChangeState(new EnemyHitState(enemyController));
    }
}