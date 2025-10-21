using UnityEngine;

public class EnemyPunchState : IState
{
    private EnemyController enemyController;
    AnimatorStateInfo info;

    private readonly string animStateName = "Punch";

    public EnemyPunchState(EnemyController enemyController)
    {
        this.enemyController = enemyController;
    }

    public void Enter()
    {
        Debug.Log("Enter Punch");
        enemyController.Animator.SetTrigger("isPunch");
    }

    public void Excute()
    {
        Debug.Log("Excute Punch");
        info = enemyController.Animator.GetCurrentAnimatorStateInfo(0);

        if (info.IsName(animStateName) && info.normalizedTime >= 0.95f)
        {
            enemyController.StateMachine.ChangeState(new EnemyIdleState(enemyController));
        }
    }
    public void Exit()
    {
        Debug.Log("Exit Move");
        enemyController.Animator.ResetTrigger("isPunch");
    }
}
