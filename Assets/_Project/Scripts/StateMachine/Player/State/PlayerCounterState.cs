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

    private const float hitFrame = 40f;
    private const float totalFrames = 135f;
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
        playerController.Animator.ResetTrigger("isCounter");
        playerController.Animator.SetTrigger("isCounter");
    }   

    public void Excute()
    {
        info = playerController.Animator.GetCurrentAnimatorStateInfo(0);

        if (info.IsName("Counter"))
        {
            float t = info.normalizedTime;

            if (!hasDealtDamage && t >= hitTimeNormalized)
            {
                hasDealtDamage = true;
                DealtDamage();
            }

            if (t >= 0.95f)
            {
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
        Collider[] hits = Physics.OverlapSphere(playerController.LeftHand.position, 0.2f, playerController.EnemyMask);

        foreach (var hit in hits)
        {
            StateMachineEnemy stateMachine = hit.GetComponent<StateMachineEnemy>();
            IHealth health = hit.GetComponent<IHealth>();
            if (health != null || stateMachine.CurrentState.ToString() == "EnemyBlockState")
            {
                health.ChangeHealth(-attack);
                SoundManager.Instance.PlaySound(SoundKey.Counter, 1, 1);
            }
        }
    }
}