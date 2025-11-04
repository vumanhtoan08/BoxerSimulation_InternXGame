using UnityEngine;

public class EnemyHealth : EntityHealth
{
    [SerializeField] private EnemyAuraEffect enemyAuraEffect;

    private EnemyController enemyController;
    private bool isDead;
    private bool isAuraActive;
    public bool IsDead => isDead;
    public bool IsAuraActive => isAuraActive;
    public EnemyAuraEffect EnemyAuraEffect => enemyAuraEffect;

    public void Init(EnemyController enemyController)
    {
        this.animator = enemyController.Animator;
        this.enemyController = enemyController;

        maxHealth = enemyController.RuntimeData.EnemyData.Health;
        currentHealth = maxHealth;
        isDead = false;
        isAuraActive = false;
        enemyAuraEffect.SetActiveAura(isAuraActive);
    }

    protected override void Dead()
    {
        base.Dead();
        enemyController.StateMachine.ChangeState(new EnemyDeadState(enemyController));
        isDead = true;
    }

    protected override void Hurt()
    {
        base.Hurt();
        if (currentHealth <= maxHealth / 2 && !isAuraActive)
        {
            isAuraActive = true;
            ChangeHealth(maxHealth / 4);
            enemyController.StateMachine.ChangeState(new EnemyTauntState(enemyController));
        }
        else
        {
            enemyController.StateMachine.ChangeState(new EnemyHitState(enemyController));
        }
    }
}

[System.Serializable]
public class EnemyAuraEffect
{
    public ParticleSystem ef1;
    public ParticleSystem ef2;
    public ParticleSystem ef3;
    public ParticleSystem ef4;

    public void SetActiveAura(bool isActive)
    {
        ef1.gameObject.SetActive(isActive);
        ef2.gameObject.SetActive(isActive);
        ef3.gameObject.SetActive(isActive);
        ef4.gameObject.SetActive(isActive);
    }

    public void SetTimescaleEffect(float scale)
    {
        var main1 = ef1.main;
        main1.simulationSpeed = scale;

        var main2 = ef2.main;
        main2.simulationSpeed = scale;

        var main3 = ef3.main;
        main3.simulationSpeed = scale;

        var main4 = ef4.main;
        main4.simulationSpeed = scale;
    }
}