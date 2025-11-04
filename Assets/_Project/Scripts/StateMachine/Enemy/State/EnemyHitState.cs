using UnityEngine;

public class EnemyHitState : IState
{
    private EnemyController enemyController;
    private AnimatorStateInfo info;

    private readonly string animStateName = "HeadHit";

    public EnemyHitState(EnemyController enemyController)
    {
        this.enemyController = enemyController;
    }

    public void Enter()
    {
        Debug.Log("Enter Hit");
        CanvasManager.Instance.OnUpdateUIEnemy();

        if (enemyController.Health.IsAuraActive)
        {
            enemyController.StateMachine.ChangeState(new EnemyIdleState(enemyController));
            return;
        }
        
        enemyController.Animator.ResetTrigger("isIdle");
        enemyController.Animator.SetTrigger("isHeadHit");
    }

    public void Excute()
    {
        if (enemyController.Health.IsAuraActive) return;

            info = enemyController.Animator.GetCurrentAnimatorStateInfo(0);
        Debug.Log($"[Punch] State={info.IsName(animStateName)} | Time={info.normalizedTime}");

        // Khi animation HeadHit kết thúc (Animator đã tự chuyển sang Idle)
        if (info.IsName(animStateName) && info.normalizedTime >= 1f)
        {
            enemyController.StateMachine.ChangeState(new EnemyIdleState(enemyController));
            
        }
    }

    public void Exit()
    {
        //enemyController.Animator.SetTrigger("isIdle");
    }
}
