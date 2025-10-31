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
        CanvasManager.Instance.OnEnemyHealthChange();
        enemyController.Animator.ResetTrigger("isIdle");
        enemyController.Animator.CrossFade(animStateName, 0.05f);
    }

    public void Excute()
    {
        info = enemyController.Animator.GetCurrentAnimatorStateInfo(0);
        Debug.Log($"[Punch] State={info.IsName(animStateName)} | Time={info.normalizedTime}");

        // Khi animation HeadHit kết thúc (Animator đã tự chuyển sang Idle)
        if (info.IsName(animStateName) && info.normalizedTime >= 0.9f)
        {
            enemyController.StateMachine.ChangeState(new EnemyIdleState(enemyController));
        }
    }

    public void Exit()
    {
        //enemyController.Animator.ResetTrigger("isHeadHit");
    }
}
