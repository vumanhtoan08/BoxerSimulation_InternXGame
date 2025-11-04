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

        if (info.IsName(animStateName))
        {
            float normalizedTime = info.normalizedTime;

            // Kích hoạt Aura ở frame 20/170
            if (normalizedTime >= (20f / 170f))
            {
                enemyAuraEffect.SetActiveAura(true);
            }

            // Kết thúc animation => trở về Idle
            if (normalizedTime >= 1f)
            {
                enemyController.StateMachine.ChangeState(new EnemyIdleState(enemyController));
            }
        }
    }


    public void Exit()
    {
        enemyController.Animator.speed = 1.5f;
    }
}
