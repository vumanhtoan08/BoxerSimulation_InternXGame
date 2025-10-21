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
        Debug.Log("Enter Block");

        blockDuration = Random.Range(1f, 2f);
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
