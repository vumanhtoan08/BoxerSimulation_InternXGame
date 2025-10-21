using UnityEngine;

public class EnemyMoveState : IState
{
    private EnemyController enemyController;
    private Transform enemyTransform;
    private Transform playerTransform;

    private float currentHealth;
    private float maxHealth;
    private float dangerHealthRatio = 0.25f;
    private float healthRatio;

    private float retreatTimer;
    private float retreatDuration;

    public EnemyMoveState(EnemyController enemyController)
    {
        this.enemyController = enemyController;
        enemyTransform = enemyController.transform;
        playerTransform = enemyController.PlayerTransform;

        currentHealth = enemyController.Health.CurrentHealth;
        maxHealth = enemyController.Health.MaxHealth;
    }

    public void Enter()
    {
        Debug.Log("Enter Move");
        enemyController.Animator.SetBool("isMoving", true);

        retreatTimer = 0f;
        retreatDuration = Random.Range(1f, 2f);
    }

    public void Excute()
    {
        healthRatio = currentHealth / maxHealth;

        if (healthRatio > dangerHealthRatio)
        {
            MoveToward();
        }
        else
        {
            StepBack();
            retreatTimer += Time.deltaTime;
            if (retreatTimer >= retreatDuration)
            {
                enemyController.StateMachine.ChangeState(new EnemyIdleState(enemyController));
                return;
            }
        }

        if (CheckDistanceToPlayer() <= enemyController.DetectedRange && healthRatio > dangerHealthRatio)
        {
            enemyController.StateMachine.ChangeState(new EnemyIdleState(enemyController));
        }
    }
    public void Exit()
    {
        enemyController.Animator.SetBool("isMoving", false);
    }

    private float CheckDistanceToPlayer()
    {
        return Vector3.Distance(enemyTransform.position, playerTransform.position);
    }

    private void MoveToward()
    {
        enemyController.Animator.SetFloat("moveValue", 0);
    }

    private void StepBack()
    {
        enemyController.Animator.SetFloat("moveValue", 1);
    }
}
