using UnityEngine;

public class EnemyWinState : IState
{
    private EnemyController enemyController;
    AnimatorStateInfo info;
    private readonly string animStateName = "Win";

    public EnemyWinState(EnemyController enemyController)
    {
        this.enemyController = enemyController;
    }

    public void Enter()
    {
        enemyController.Animator.SetTrigger("isWin");
    }

    public void Excute()
    {
        info = enemyController.Animator.GetCurrentAnimatorStateInfo(0);

        if (info.IsName(animStateName) && info.normalizedTime >= 1f)
        {
            GameManager.Instance.ChangeGameState(Game_State.Lose);
        }
    }
    public void Exit()
    {
        enemyController.Animator.ResetTrigger("isWin");
    }
}
