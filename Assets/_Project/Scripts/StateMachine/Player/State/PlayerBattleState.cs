using UnityEngine;

public class PlayerBattleState : IState
{
    [Header("Ref")]
    private PlayerController playerController;
    private bool isAttacking; 

    public PlayerBattleState(PlayerController playerController)
    {
        this.playerController = playerController;
    }

    public void Enter()
    {
        playerController.Animator.SetTrigger("isBattle");
        isAttacking = false;
    }

    public void Excute()
    {

    }

    public void Exit()
    {

    }

    private void HandleInput()
    {
        if (isAttacking) return;
    }
}
