
using DG.Tweening;
using UnityEngine;

public class EnemyCounterState : IState
{
    private EnemyController enemyController;
    private AnimatorStateInfo info;
    private bool hasDealtDamage;

    private readonly string animStateName = "Counter";
    private const float damageTriggerPercent = 0.32f;

    public EnemyCounterState(EnemyController enemyController)
    {
        this.enemyController = enemyController;
    }

    public void Enter()
    {
        Debug.Log("Enter EnemyPunchState");

        hasDealtDamage = false;
        enemyController.Animator.ResetTrigger("isIdle");
        enemyController.Animator.CrossFade(animStateName, 0.05f);
    }

    public void Excute()
    {
        info = enemyController.Animator.GetCurrentAnimatorStateInfo(0);

        if (info.IsName(animStateName) && info.normalizedTime >= damageTriggerPercent && !hasDealtDamage)
        {
            hasDealtDamage = true;
            DealDamageToPlayer();
        }

        if (info.IsName(animStateName) && info.normalizedTime >= 0.9f)
        {
            enemyController.StateMachine.ChangeState(new EnemyIdleState(enemyController));
        }
    }

    public void Exit()
    {
        //enemyController.Animator.ResetTrigger("isCounter");
    }

    private void DealDamageToPlayer()
    {
        Transform hitPoint = enemyController.LeftHand;
        float radius = 1f;
        LayerMask playerLayerMask = enemyController.LayerPlayer; // <-- nhớ đặt Player vào Layer

        Collider[] hits = Physics.OverlapSphere(hitPoint.position, radius, playerLayerMask);

        if (hits.Length > 0)
        {
            Debug.Log($"Va cham vao {hits[0].name}");
            var playerController = hits[0].GetComponent<PlayerController>();
            Transform effect = ObjectPooling.GetObject(DictionaryEffect.Instance.enemyHitEffect, hitPoint.position);

            SoundManager.Instance.PlaySound(SoundKey.Counter, 1, 1);
            playerController.Health.ChangeHealth(-enemyController.RuntimeData.EnemyData.Attack);

            DOVirtual.DelayedCall(1f, () =>
            {
                ObjectPooling.ReturnObject(effect);
            });
        }
    }
}
