using UnityEngine;
using UnityEngine.UI;

public class PlayerCounterState : IState
{
    [Header("Ref")]
    private PlayerController playerController;
    private AnimatorStateInfo info;
    private bool hasDealtDamage;

    [Header("Paras")]
    private float staminaCost;
    private float attack;

    private const float hitFrame = 26f;
    private const float totalFrames = 78f;
    private readonly float hitTimeNormalized = hitFrame / totalFrames;

    public PlayerCounterState(PlayerController playerController)
    {
        this.playerController = playerController;

        staminaCost = this.playerController.Data.DataRuntime.CounterCost;
        attack = this.playerController.Data.DataRuntime.Attack;
    }

    public void Enter()
    {
        playerController.Data.ChangeStamina(-staminaCost);

        playerController.SetCounter(true);
        hasDealtDamage = false;
        //playerController.Animator.CrossFade("Counter", 0f);
        playerController.Animator.SetTrigger("isCounter");
    }

    public void Excute()
    {
        info = playerController.Animator.GetCurrentAnimatorStateInfo(0);
        var next = playerController.Animator.GetNextAnimatorStateInfo(0);

        if (info.IsName("Counter") && !playerController.Animator.IsInTransition(0))
        {
            float t = info.normalizedTime;

            if (!hasDealtDamage && t >= hitTimeNormalized)
            {
                hasDealtDamage = true;
                DealtDamage();
            }

            if (t >= 0.8f)
            {
                playerController.Animator.Play("IdleBattle", 0, 0f);
                playerController.StateMachine.ChangeState(new PlayerBattleState(playerController));
            }
        }
    }

    public void Exit()
    {
        playerController.SetCounter(false);
    }

    private void DealtDamage()
    {
        Collider[] hits = Physics.OverlapSphere(playerController.LeftHand.position, 0.5f, playerController.EnemyMask);

        foreach (var hit in hits)
        {
            StateMachineEnemy stateMachine = hit.GetComponent<StateMachineEnemy>();
            IHealth health = hit.GetComponent<IHealth>();
            if (health != null)
            {
                health.ChangeHealth(-attack);
                SoundManager.Instance.PlaySound(SoundKey.Counter, 1, 1);
            }
        }
    }
}