using UnityEngine;

public class EnemyDeadState : IState
{
    private EnemyController enemyController;
    AnimatorStateInfo info;
    private EnemyAuraEffect effect;

    public EnemyDeadState(EnemyController enemyController)
    {
        this.enemyController = enemyController;
        effect = enemyController.Health.EnemyAuraEffect;
    }

    public void Enter()
    {
        GameManager.Instance.ChangeGameState(Game_State.Win);
        CanvasManager.Instance.OnUpdateUIEnemy();

        Debug.Log("Enter Dead");
        enemyController.Animator.SetTrigger("isDead");
        enemyController.DynamicCollider.enabled = false;
    }

    public void Excute()
    {

    }
    public void Exit()
    {
        enemyController.Animator.ResetTrigger("isDead");
        enemyController.DynamicCollider.enabled = true;
        effect.SetActiveAura(false);
    }
}
