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
    private const float totalFrames = 104f;
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
        playerController.Animator.ResetTrigger("isPunch");
        playerController.Animator.SetTrigger("isPunch");
    }

    public void Excute()
    {
        info = playerController.Animator.GetCurrentAnimatorStateInfo(0);

        if (info.IsName("Punch"))
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
        playerController.SetPunch(false);
    }

    private void DealtDamage()
    {
        Collider[] hits = Physics.OverlapSphere(playerController.RightHand.position, 0.2f, playerController.EnemyMask);

        foreach (var hit in hits)
        {
            IHealth health = hit.GetComponent<IHealth>();
            if (health != null)
            {
                health.ChangeHealth(-attack);
            }
        }
    }
}