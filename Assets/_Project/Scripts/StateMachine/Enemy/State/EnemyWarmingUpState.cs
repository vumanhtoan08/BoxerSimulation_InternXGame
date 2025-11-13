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
        float randomAnim = Random.Range(0f, 1f);
        enemyController.Animator.SetFloat("randomWarmingUp", randomAnim);

        enemyController.Animator.SetTrigger("isWarmingUp");
    }

    public void Excute()
    {
    }
    public void Exit()
    {
        enemyController.Animator.ResetTrigger("isWarmingUp");
    }
}
