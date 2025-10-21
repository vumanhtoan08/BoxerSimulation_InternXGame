using UnityEngine;

public class EnemyIdleState : IState
{
    private EnemyController enemyController;
    private Transform enemyTransform;
    private Transform playerTransform;

    private float currentHealth;
    private float maxHealth;
    private float dangerHealthRatio = 0.25f; 

    public EnemyIdleState(EnemyController enemyController)
    {
        this.enemyController = enemyController;
        enemyTransform = enemyController.transform;
        playerTransform = enemyController.PlayerTransform;

        currentHealth = enemyController.Health.CurrentHealth;
        maxHealth = enemyController.Health.MaxHealth;
    }

    public void Enter()
    {
        Debug.Log("Vao Idle");
    }

    public void Excute()
    {
        Debug.Log("Đang chạy IDle");

        CheckDistanceToDecided();
    }
    public void Exit()
    {
        
    }

    private void CheckDistanceToDecided()
    {
        float currentDistance = Vector3.Distance(enemyTransform.position, playerTransform.position);
        float healthRatio = currentHealth / maxHealth;
        int randomDecision = Random.Range(0, 100);

        if (currentDistance > enemyController.DetectedRange && healthRatio > dangerHealthRatio)
        {
            enemyController.StateMachine.ChangeState(new EnemyMoveState(enemyController));
            return;
        }

        if (healthRatio > dangerHealthRatio)
        {
            if (randomDecision < 50)
                enemyController.StateMachine.ChangeState(new EnemyPunchState(enemyController));
            else if (randomDecision < 80)
                enemyController.StateMachine.ChangeState(new EnemyCounterState(enemyController));
            else
                enemyController.StateMachine.ChangeState(new EnemyBlockState(enemyController));
        }
        else
        {
            if(randomDecision < 45)
                enemyController.StateMachine.ChangeState(new EnemyBlockState(enemyController));
            else if(randomDecision < 90)
                enemyController.StateMachine.ChangeState(new EnemyMoveState(enemyController));
            else
                enemyController.StateMachine.ChangeState(new EnemyPunchState(enemyController));
        }
    }
}
