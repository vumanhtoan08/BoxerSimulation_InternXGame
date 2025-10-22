using UnityEngine;

public class EnemyCounterState : IState
{
    private EnemyController enemyController;
    AnimatorStateInfo info;
    private bool hasDealtDamage;

    private readonly string animStateName = "Counter";

    private const float damageTriggerPercent = 0.4f;

    public EnemyCounterState(EnemyController enemyController)
    {
        this.enemyController = enemyController;
    }

    public void Enter()
    {
        hasDealtDamage = false;
        enemyController.Animator.SetTrigger("isCounter");
    }

    public void Excute()
    {
        info = enemyController.Animator.GetCurrentAnimatorStateInfo(0);

        if (info.IsName(animStateName) && info.normalizedTime >= damageTriggerPercent && !hasDealtDamage)
        {
            hasDealtDamage = true;
            DealDamageToPlayer();
        }

        if (info.IsName(animStateName) && info.normalizedTime >= 0.95f)
        {
            enemyController.StateMachine.ChangeState(new EnemyIdleState(enemyController));
        }
    }

    public void Exit()
    {
        enemyController.Animator.ResetTrigger("isCounter");
    }

    private void DealDamageToPlayer()
    {
        Transform hitPoint = enemyController.LeftHand;
        float radius = 0.3f;

        LayerMask playerLayerMask = enemyController.LayerPlayer; // <-- nhớ đặt Player vào Layer

        Collider[] hits = Physics.OverlapSphere(hitPoint.position, radius, playerLayerMask);

        if (hits.Length > 0)
        {
            Debug.Log($"Va cham vao {hits[0].name}");
            var playerController = hits[0].GetComponent<PlayerController>();
            playerController.Health.ChangeHealth(-enemyController.RuntimeData.EnemyData.Attack);
        }
    }
}
