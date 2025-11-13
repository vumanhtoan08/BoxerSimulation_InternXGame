using UnityEngine;

public class EnemyMoveState : IState
{
    private EnemyController enemyController;
    private Transform enemyTransform;
    private Transform playerTransform;
    private Rigidbody enemyRb;

    private float currentHealth;
    private float maxHealth;
    private float dangerHealthRatio = 0.25f;
    private float healthRatio;

    private float retreatTimer;
    private float retreatDuration;

    private float speed = 1f;

    public EnemyMoveState(EnemyController enemyController)
    {
        this.enemyController = enemyController;
        enemyTransform = enemyController.transform;
        playerTransform = enemyController.PlayerTransform;
        enemyRb = enemyController.Rigidbody;

        currentHealth = enemyController.Health.CurrentHealth;
        maxHealth = enemyController.Health.MaxHealth;
    }

    public void Enter()
    {
        enemyController.Animator.SetBool("isMoving", true);

        retreatTimer = 0f;
        retreatDuration = Random.Range(1f, 2f);
    }

    public void Excute()
    {
        switch (enemyController.RuntimeData.EnemyData.Difficult)
        {
            case Enemy_Difficult.Easy:
                EnemyEasyBehaviour();
                break;
            case Enemy_Difficult.Med:
                EnemyMedBehaviour();
                break;
            case Enemy_Difficult.Hard:
                EnemyMedBehaviour();
                break;
        }
    }

    public void Exit()
    {
        enemyController.Animator.SetBool("isMoving", false);
        enemyRb.linearVelocity = Vector3.zero;
    }

    private float CheckDistanceToPlayer()
    {
        return Vector3.Distance(enemyTransform.position, playerTransform.position);
    }

    private void MoveToward()
    {
        enemyController.Animator.SetFloat("moveValue", 0);
        Vector3 direction = (playerTransform.position - enemyTransform.position).normalized;


        if (enemyController.Health.IsAuraActive)
        {
            enemyRb.linearVelocity = direction * speed * 2f;
        }
        else
        {
            enemyRb.linearVelocity = direction * speed;
        }
    }

    private void StepBack()
    {
        enemyController.Animator.SetFloat("moveValue", 1);
        Vector3 direction = (enemyTransform.position - playerTransform.position).normalized;
        enemyRb.linearVelocity = direction * speed;
    }

    private void EnemyEasyBehaviour()
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

    private void EnemyMedBehaviour()
    {
        MoveToward();

        if (CheckDistanceToPlayer() <= enemyController.DetectedRange)
        {
            enemyController.StateMachine.ChangeState(new EnemyIdleState(enemyController));
        }
    }
}
