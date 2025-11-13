using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : EntityHealth
{
    [SerializeField] private EnemyAuraEffect enemyAuraEffect;
    [SerializeField] private GameObject energyExplosion; 

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
        SoundManager.Instance.PlaySound(SoundKey.Dead, 1f, 1f);
        enemyController.StateMachine.ChangeState(new EnemyDeadState(enemyController));
        isDead = true;
    }

    protected override void Hurt()
    {
        base.Hurt();
        var random = Random.Range(0.6f, 1.2f);
        SoundManager.Instance.PlaySound(SoundKey.Hurt_1, 1f, random);

        if (currentHealth <= maxHealth / 2 && !isAuraActive && enemyController.RuntimeData.EnemyData.Difficult != Enemy_Difficult.Easy)
        {
            isAuraActive = true;
            enemyController.StateMachine.ChangeState(new EnemyTauntState(enemyController));
        }
        else
        {
            enemyController.StateMachine.ChangeState(new EnemyHitState(enemyController));
        }
    }

    public void SetExplosionActive(bool value)
    {
        if (energyExplosion == null) return;

        if (value)
        {
            energyExplosion.SetActive(true);
            energyExplosion.transform.localScale = Vector3.zero;

            // Scale lên 10 lần trong 0.8 giây (có easing mượt)
            energyExplosion.transform
                .DOScale(Vector3.one * 5f, 8f)
                .SetEase(Ease.OutBack); // hoặc Ease.OutCubic nếu muốn mềm hơn
        }
        else
        {
            // Thu nhỏ lại rồi ẩn
            energyExplosion.SetActive(false);
        }
    }
}

[System.Serializable]
public class EnemyAuraEffect
{
    public List<ParticleSystem> effects = new(); 

    public void SetActiveAura(bool isActive)
    {
        foreach (ParticleSystem p in effects)
        {
            p.gameObject.SetActive(isActive);
        }
    }

    public void SetTimescaleEffect(float scale)
    {
        foreach (ParticleSystem p in effects)
        {
            var main = p.main;
            main.simulationSpeed = scale;
        }
    }
}