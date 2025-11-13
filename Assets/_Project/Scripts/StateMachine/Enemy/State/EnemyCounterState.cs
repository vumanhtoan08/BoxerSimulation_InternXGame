
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

        if (info.IsName(animStateName) && info.normalizedTime >= 1f)
        {
            switch (enemyController.RuntimeData.EnemyData.Difficult)
            {
                case Enemy_Difficult.Easy:
                    enemyController.StateMachine.ChangeState(new EnemyIdleState(enemyController));
                    break;
                case Enemy_Difficult.Med:
                    DecideNextActionMedEnemy();
                    break;
                case Enemy_Difficult.Hard:
                    DecideNextActionMedEnemy();
                    break;
                default:
                    break;
            }
        }
    }

    public void Exit()
    {
        enemyController.Animator.SetTrigger("isIdle");
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
            TimeEffect.HitTimeEffect();

            SoundManager.Instance.PlaySound(SoundKey.Counter, 1, 1);
            if (EnemyManager.Instance.EnemyController.RuntimeData.EnemyData.Difficult != Enemy_Difficult.Easy
                && EnemyManager.Instance.EnemyController.Health.IsAuraActive)
            {
                playerController.Health.ChangeHealth(-enemyController.RuntimeData.EnemyData.Attack * (1f + (int)EnemyManager.Instance.EnemyController.RuntimeData.EnemyData.Difficult * 0.25f));
            }
            else
            {
                playerController.Health.ChangeHealth(-enemyController.RuntimeData.EnemyData.Attack);
            }

            DOVirtual.DelayedCall(1f, () =>
            {
                ObjectPooling.ReturnObject(effect);
            });
        }
    }

    private void DecideNextActionMedEnemy()
    {
        float rand = Random.value; // 0 → 1
        if (rand < 0.5f)
        {
            enemyController.StateMachine.ChangeState(new EnemyPunchState(enemyController));
        }
        else
        {
            enemyController.StateMachine.ChangeState(new EnemyIdleState(enemyController));
        }
    }
}
