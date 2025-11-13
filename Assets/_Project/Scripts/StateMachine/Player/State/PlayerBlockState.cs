using UnityEngine;

public class PlayerBlockState : IState
{
    [Header("Ref")]
    private PlayerController playerController;

    [Header("Paras")]
    private float staminaCost;

    public PlayerBlockState(PlayerController playerController)
    {
        this.playerController = playerController;

        staminaCost = playerController.Data.DataRuntime.BlockCost;
    }

    public void Enter()
    {
        playerController.SetBlock(true);
        playerController.Animator.SetBool("isBlock", playerController.IsBlocking);
    }

    public void Excute()
    {
        if (!playerController.IsBlocking)
        {
            playerController.StateMachine.ChangeState(new PlayerBattleState(playerController));
            return;
        }

        if (playerController.IsBlocking && playerController.Data.CurrentStamina > 0)
        {
            float cost = staminaCost * Time.deltaTime;
            playerController.Data.ChangeStamina(-cost);

            // Khi stamina cạn → tự thoát trạng thái
            if (playerController.Data.CurrentStamina <= 0)
            {
                playerController.SetBlock(false);
                playerController.StateMachine.ChangeState(new PlayerBattleState(playerController));
            }
        }
    }

    public void Exit()
    {
        Debug.Log("❌ Thoát trạng thái đỡ");
        playerController.Animator.SetBool("isBlock", playerController.IsBlocking);
    }
}