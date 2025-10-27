using UnityEngine; 

public class PlayerHealth : EntityHealth
{
    private PlayerController playerController;

    public void Init(PlayerController playerController)
    {
        this.playerController = playerController;

        maxHealth = playerController.Data.DataRuntime.Health;
        currentHealth = maxHealth;
        animator = playerController.Animator;
    }

    protected override void Dead()
    {
        base.Dead();
        GameManager.Instance.ChangeGameState(Game_State.Lose);
    }

    protected override void Hurt()
    {
        base.Hurt();
        CanvasManager.Instance.OnPlayerHealthChange();
    }

    public void OnSettingHealthBeforeBattle()
    {
        maxHealth = playerController.Data.DataRuntime.Health;
        currentHealth = maxHealth;
        PlayerController.Instance.Data.ChangeStamina(PlayerController.Instance.Data.DataRuntime.Stamina); 
    }
}