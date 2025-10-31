using UnityEngine;

public class EnemyIdleState : IState
{
    private EnemyController enemyController;
    private Transform enemyTransform;
    private Transform playerTransform;

    private float decisionCooldown;           // Cooldown hiện tại (thay đổi ngẫu nhiên mỗi lần)
    private float decisionTimer;              // Bộ đếm thời gian
    private float dangerHealthRatio = 0.25f;  // Ngưỡng máu thấp

    public EnemyIdleState(EnemyController enemyController)
    {
        this.enemyController = enemyController;
        enemyTransform = enemyController.transform;
        playerTransform = enemyController.PlayerTransform;
    }

    public void Enter()
    {
        //enemyController.Animator.ResetTrigger("isPunch");
        enemyController.Animator.SetTrigger("isIdle");
        decisionCooldown = Random.Range(0.05f, 0.1f);
        decisionTimer = 0f;
    }

    public void Excute()
    {
        decisionTimer += Time.deltaTime;

        if (decisionTimer < decisionCooldown) return;

        CheckDistanceToDecide();

        decisionTimer = 0f;
        decisionCooldown = Random.Range(0.05f, 0.1f);
    }

    public void Exit()
    {
    }

    private void CheckDistanceToDecide()
    {
        float currentDistance = Vector3.Distance(enemyTransform.position, playerTransform.position);
        float healthRatio = enemyController.Health.CurrentHealth / enemyController.Health.MaxHealth;
        int randomDecision = Random.Range(0, 100);

        if (currentDistance > enemyController.DetectedRange && healthRatio > dangerHealthRatio)
        {
            enemyController.StateMachine.ChangeState(new EnemyMoveState(enemyController));
            return;
        }

        if (healthRatio > dangerHealthRatio)
        {
            if (randomDecision < 100)
                enemyController.StateMachine.ChangeState(new EnemyPunchState(enemyController));
            else
                enemyController.StateMachine.ChangeState(new EnemyCounterState(enemyController));
        }
        else
        {
            if (randomDecision < 45)
                enemyController.StateMachine.ChangeState(new EnemyBlockState(enemyController));
            else if (randomDecision < 90)
                enemyController.StateMachine.ChangeState(new EnemyMoveState(enemyController));
            else
                enemyController.StateMachine.ChangeState(new EnemyPunchState(enemyController));
        }
    }
}
