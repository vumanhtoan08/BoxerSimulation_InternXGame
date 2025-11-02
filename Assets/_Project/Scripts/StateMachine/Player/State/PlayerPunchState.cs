using DG.Tweening;
using UnityEngine;

public class PlayerPunchState : IState
{
    [Header("Ref")]
    private PlayerController playerController;
    private AnimatorStateInfo info;
    private bool hasDealtDamage;

    [Header("Paras")]
    private float staminaCost;
    private float attack;

    private const float hitFrame = 40f;
    private const float totalFrames = 131f;
    private readonly float hitTimeNormalized = hitFrame / totalFrames;

    public PlayerPunchState(PlayerController playerController)
    {
        this.playerController = playerController;

        staminaCost = this.playerController.Data.DataRuntime.CounterCost;
        attack = this.playerController.Data.DataRuntime.Attack;
    }

    public void Enter()
    {
        playerController.Data.ChangeStamina(-staminaCost);

        playerController.SetPunch(true);
        hasDealtDamage = false;
        playerController.Animator.SetTrigger("isPunch");
    }

    public void Excute()
    {
        info = playerController.Animator.GetCurrentAnimatorStateInfo(0);
        var next = playerController.Animator.GetNextAnimatorStateInfo(0);

        if (info.IsName("Punch") && !playerController.Animator.IsInTransition(0))
        {
            float t = info.normalizedTime;

            if (!hasDealtDamage && t >= hitTimeNormalized)
            {
                hasDealtDamage = true;
                DealtDamage();
            }

            if (t >= 0.6f)
            {
                playerController.Animator.Play("IdleBattle", 0, 0f);
                playerController.StateMachine.ChangeState(new PlayerBattleState(playerController));
            }
        }
    }

    public void Exit()
    {
        playerController.SetPunch(false);
    }

    private void DealtDamage()
    {
        Collider[] hits = Physics.OverlapSphere(playerController.RightHand.position, 0.2f, playerController.EnemyMask);

        foreach (var hit in hits)
        {
            StateMachineEnemy stateMachine = hit.GetComponent<StateMachineEnemy>();
            IHealth health = hit.GetComponent<IHealth>();
            if (health != null && stateMachine.CurrentState.ToString() != "EnemyBlockState")
            {
                health.ChangeHealth(-attack);
                SoundManager.Instance.PlaySound(SoundKey.Punch, 1, 1);
                Transform effect = ObjectPooling.GetObject(DictionaryEffect.Instance.hitEffect, playerController.RightHand.position);
                TimeEffect.HitTimeEffect();

                DOVirtual.DelayedCall(1f, () =>
                {
                    ObjectPooling.ReturnObject(effect);
                });
            }
            else
            {
                SoundManager.Instance.PlaySound(SoundKey.Block, 1, 1);
            }
        }
    }
}