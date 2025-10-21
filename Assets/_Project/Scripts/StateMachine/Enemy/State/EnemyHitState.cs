using UnityEngine;

public class EnemyHitState : IState
{
    private EnemyController enemyController;
    AnimatorStateInfo info;

    public EnemyHitState(EnemyController enemyController)
    {
        this.enemyController = enemyController;
    }

    public void Enter()
    {
        Debug.Log("Enter Hit");
        enemyController.Animator.SetTrigger("isHeadHit");
    }

    public void Excute()
    {
        info = enemyController.Animator.GetCurrentAnimatorStateInfo(0);

        if (info.IsName("HeadHit") && info.normalizedTime >= 0.95f)
        {
            enemyController.StateMachine.ChangeState(new EnemyIdleState(enemyController));
        }
    }
    public void Exit()
    {
        enemyController.Animator.ResetTrigger("isHeadHit");
    }
}
