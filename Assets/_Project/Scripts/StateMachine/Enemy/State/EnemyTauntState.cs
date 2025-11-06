using UnityEngine;

public class EnemyTauntState : IState
{
    private EnemyController enemyController;
    private EnemyAuraEffect enemyAuraEffect;
    private AnimatorStateInfo info;
    private CharacterController playerCC;

    private readonly string animStateName = "Taunt";

    private float pushInterval = 0.5f;
    private float lastPushTime = 0f;

    private Vector3 currentPushVelocity = Vector3.zero;
    private float pushSmoothTime = 0.25f;

    public EnemyTauntState(EnemyController enemyController)
    {
        this.enemyController = enemyController;
        enemyAuraEffect = enemyController.Health.EnemyAuraEffect;
        playerCC = PlayerController.Instance.CharacterController;
    }

    public void Enter()
    {
        CanvasManager.Instance.OnUpdateUIEnemy();
        Debug.Log("Enter Taunt");

        lastPushTime = -999f;
        currentPushVelocity = Vector3.zero;

        enemyController.Animator.ResetTrigger("isTaunt");
        enemyController.Animator.SetTrigger("isTaunt");
        SoundManager.Instance.PlaySound(SoundKey.Taunt, 1.5f, 1.5f);
    }

    public void Excute()
    {
        info = enemyController.Animator.GetCurrentAnimatorStateInfo(0);

        if (info.IsName(animStateName))
        {
            float normalizedTime = info.normalizedTime;

            if (normalizedTime >= (20f / 170f))
            {
                var difficult = enemyController.RuntimeData.EnemyData.Difficult;

                if (difficult == Enemy_Difficult.Hard)
                {
                    enemyController.Health.SetExplosionActive(true);
                    enemyAuraEffect.SetActiveAura(true);

                    if (Time.time - lastPushTime >= pushInterval)
                    {
                        AddPushImpulse();
                        lastPushTime = Time.time;
                    }

                    SmoothPushPlayer();
                }
                else
                {
                    enemyAuraEffect.SetActiveAura(true);
                }
            }

            if (normalizedTime >= 1f)
            {
                enemyController.StateMachine.ChangeState(new EnemyIdleState(enemyController));
            }
        }
    }

    private void AddPushImpulse()
    {
        var player = PlayerController.Instance;
        if (player == null) return;

        Vector3 direction = (player.transform.position - enemyController.transform.position).normalized;
        Vector3 pushImpulse = direction * 0.5f;
        currentPushVelocity += pushImpulse;

        if (player.StateMachine.CurrentState is not PlayerBlockState)
        {
            float damage = enemyController.RuntimeData.EnemyData.Attack * 0.2f; // 50% damage cơ bản, bạn chỉnh tùy ý
            player.Health.ChangeHealth(-damage);
            SoundManager.Instance.PlaySound(SoundKey.Punch);
        }
        else
        {
            SoundManager.Instance.PlaySound(SoundKey.Block);
        }
    }

    private void SmoothPushPlayer()
    {
        if (playerCC == null) return;

        playerCC.Move(currentPushVelocity * Time.deltaTime * 5f);
        currentPushVelocity = Vector3.Lerp(currentPushVelocity, Vector3.zero, Time.deltaTime / pushSmoothTime);
    }

    public void Exit()
    {
        enemyController.Animator.speed = 1.5f;
        enemyController.Health.SetExplosionActive(false);
        currentPushVelocity = Vector3.zero;

        // ✅ Reset lại timer để 15s sau mới gồng tiếp
        enemyController.StateMachine.ResetTauntTimer();
    }
}
