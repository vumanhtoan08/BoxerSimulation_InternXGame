using DG.Tweening;
using UnityEngine;

public class EnemyDeadState : IState
{
    private EnemyController enemyController;
    private EnemyAuraEffect effect;

    public EnemyDeadState(EnemyController enemyController)
    {
        this.enemyController = enemyController;
        effect = enemyController.Health.EnemyAuraEffect;
    }

    public void Enter()
    {
        CanvasManager.Instance.OnUpdateUIEnemy();

        Debug.Log("Enter Dead");
        enemyController.Animator.SetTrigger("isDead");
        enemyController.DynamicCollider.enabled = false;

        effect.SetActiveAura(false);
    }

    public void Excute()
    {
        Debug.Log("Excute Dead");
        var info = enemyController.Animator.GetCurrentAnimatorStateInfo(0);
        Debug.Log($"LayerWeight: {enemyController.Animator.GetLayerWeight(0)} | CurrentAnim: {info.IsName("Dead")} | Normalized: {info.normalizedTime}");


        if (info.IsName("Dead") && info.normalizedTime >= 1.5f)
        {
            GameManager.Instance.ChangeGameState(Game_State.Win);
        }
    }
    public void Exit()
    {
        enemyController.DynamicCollider.enabled = true;
    }
}
