using UnityEngine;

public class EnemyPunchState : IState
{
    private EnemyController enemyController;
    AnimatorStateInfo info;
    private bool hasDealtDamage;

    private readonly string animStateName = "Punch";

    private const float damageTriggerPercent = 0.31f;

    public EnemyPunchState(EnemyController enemyController)
    {
        this.enemyController = enemyController;
    }

    public void Enter()
    {
        Debug.Log("Enter Punch");
        hasDealtDamage = false;
        enemyController.Animator.SetTrigger("isPunch");
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
        enemyController.Animator.ResetTrigger("isPunch");
    }

    private void DealDamageToPlayer()
    {
        Transform hitPoint = enemyController.RightHand;
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
