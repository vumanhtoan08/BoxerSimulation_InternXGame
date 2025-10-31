using DG.Tweening;
using UnityEngine;

public class EnemyPunchState : IState
{
    private EnemyController enemyController;
    private AnimatorStateInfo info;
    private bool hasDealtDamage;

    private readonly string animStateName = "Punch";
    private const float damageTriggerPercent = 0.3f;

    public EnemyPunchState(EnemyController enemyController)
    {
        this.enemyController = enemyController;
    }

    public void Enter()
    {
        hasDealtDamage = false;
        enemyController.Animator.ResetTrigger("isIdle");
        enemyController.Animator.CrossFade(animStateName, 0.05f);
    }

    public void Excute()
    {
        info = enemyController.Animator.GetCurrentAnimatorStateInfo(0);
        Debug.Log($"[Punch] State={info.IsName(animStateName)} | Time={info.normalizedTime}");

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
        // Không reset trigger quá sớm, nếu muốn thì có thể reset sau delay
        // enemyController.Animator.ResetTrigger("isPunch");
    }

    private void DealDamageToPlayer()
    {
        Transform hitPoint = enemyController.RightHand;
        float radius = 0.4f;
        LayerMask playerLayerMask = enemyController.LayerPlayer;

        Collider[] hits = Physics.OverlapSphere(hitPoint.position, radius, playerLayerMask);

        if (hits.Length > 0)
        {
            Debug.Log($"Enemy hit {hits[0].name}");
            var playerController = hits[0].GetComponent<PlayerController>();
            Transform effect = ObjectPooling.GetObject(DictionaryEffect.Instance.enemyHitEffect, enemyController.RightHand.position);

            if (playerController.StateMachine.CurrentState.ToString() == "PlayerBlockState")
            {
                SoundManager.Instance.PlaySound(SoundKey.Block, 1, 1);

                DOVirtual.DelayedCall(1f, () =>
                {
                    ObjectPooling.ReturnObject(effect);
                });
                return;
            }

            SoundManager.Instance.PlaySound(SoundKey.Punch, 1, 1);
            playerController.Health.ChangeHealth(-enemyController.RuntimeData.EnemyData.Attack);

            DOVirtual.DelayedCall(1f, () =>
            {
                ObjectPooling.ReturnObject(effect);
            });
        }
    }
}
