using UnityEngine;

public class EnemyTauntState : IState
{
    private EnemyController enemyController;
    private EnemyAuraEffect enemyAuraEffect;

    private AnimatorStateInfo info;
    private readonly string animStateName = "Taunt"; // tên Animation clip trong Animator

    public EnemyTauntState(EnemyController enemyController)
    {
        this.enemyController = enemyController;
        enemyAuraEffect = enemyController.Health.EnemyAuraEffect;
    }

    public void Enter()
    {
        CanvasManager.Instance.OnUpdateUIEnemy();
        Debug.Log("Enter Taunt");

        enemyController.Animator.ResetTrigger("isTaunt");
        enemyController.Animator.SetTrigger("isTaunt");
    }

    public void Excute()
    {
        info = enemyController.Animator.GetCurrentAnimatorStateInfo(0);

        if (info.IsName(animStateName) && info.normalizedTime >= 0.9f)
        {
            enemyController.StateMachine.ChangeState(new EnemyIdleState(enemyController));
        }
    }

    public void Exit()
    {
        enemyAuraEffect.SetActiveAura(true);
        enemyController.Animator.speed = 1.5f;
    }
}
