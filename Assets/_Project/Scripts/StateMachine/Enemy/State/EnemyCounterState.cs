using UnityEngine;

public class EnemyCounterState : IState
{
    private EnemyController enemyController;
    AnimatorStateInfo info;

    private readonly string animStateName = "Counter";

    public EnemyCounterState(EnemyController enemyController)
    {
        this.enemyController = enemyController;
    }

    public void Enter()
    {
        Debug.Log("Enter Counter");
        enemyController.Animator.SetTrigger("isCounter");
    }

    public void Excute()
    {
        info = enemyController.Animator.GetCurrentAnimatorStateInfo(0);

        if (info.IsName(animStateName) && info.normalizedTime >= 0.95f)
        {
            enemyController.StateMachine.ChangeState(new EnemyIdleState(enemyController));
        }
    }
    public void Exit()
    {
        Debug.Log("Exit Counter");
        enemyController.Animator.ResetTrigger("isCounter");
    }
}
