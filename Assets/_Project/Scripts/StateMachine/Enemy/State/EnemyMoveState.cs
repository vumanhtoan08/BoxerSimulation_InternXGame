using Unity.Mathematics;
using UnityEngine;

public class EnemyMoveState : IState
{
    private EnemyController enemyController;
    private Transform enemyTransform;
    private Transform playerTransform; 

    private float currentHealth;
    private float maxHealth;
    private float dangerHealthRatio;
    private float healthRatio; 


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
    }

    public void Excute()
    {
        healthRatio = currentHealth / maxHealth;
        if (healthRatio > dangerHealthRatio)
            MoveToward();
        else
            StepBack();

        if (CheckDistanceToPlayer() <= enemyController.DetectedRange)
        {
            enemyController.StateMachine.ChangeState(new EnemyIdleState(enemyController));
        }
    }
    public void Exit()
    {
        Debug.Log("Exit Move");
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
