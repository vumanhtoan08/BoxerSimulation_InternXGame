using UnityEngine;

public class EnemyBlockState : IState
{
    private EnemyController enemyController;

    private float blockDuration;
    private float blockTimer;

    public EnemyBlockState(EnemyController enemyController)
    {
        this.enemyController = enemyController;
    }

    public void Enter()
    {
        blockDuration = Random.Range(0.8f, 1f);
        blockTimer = 0f;

        enemyController.Animator.SetBool("isBlock", true);
    }

    public void Excute()
    {
        blockTimer += Time.deltaTime;

        if (blockTimer >= blockDuration)
        {
            enemyController.StateMachine.ChangeState(new EnemyIdleState(enemyController));
        }
    }
    public void Exit()
    {
        enemyController.Animator.SetBool("isBlock", false);
    }
}
