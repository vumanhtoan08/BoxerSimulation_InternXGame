using UnityEngine;

public class PlayerDeadState : IState
{
    private PlayerController controller;

    public PlayerDeadState(PlayerController controller)
    {
        this.controller = controller;
    }

    public void Enter()
    {
        controller.Animator.SetTrigger("isDead");
    }

    public void Excute()
    {

    }

    public void Exit()
    {

    }
}