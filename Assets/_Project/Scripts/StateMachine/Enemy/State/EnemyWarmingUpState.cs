using UnityEngine;

public class EnemyWarmingUpState : IState
{
    private EnemyController enemyController;
    AnimatorStateInfo info;

    public EnemyWarmingUpState(EnemyController enemyController)
    {
        this.enemyController = enemyController;
    }

    public void Enter()
    {
        float randomAnim = Random.Range(0f, 2f);
        enemyController.Animator.SetFloat("randomWarmingUp", randomAnim);

        enemyController.Animator.SetTrigger("isWarmingUp");
    }

    public void Excute()
    {
        info = enemyController.Animator.GetCurrentAnimatorStateInfo(0);

        if (info.IsName("WarmingUp") && info.normalizedTime >= 1f && !enemyController.Animator.IsInTransition(0))
        {
            enemyController.StateMachine.ChangeState(new EnemyIdleState(enemyController));
        }
    }
    public void Exit()
    {
        enemyController.Animator.ResetTrigger("isWarmingUp");
    }
}
