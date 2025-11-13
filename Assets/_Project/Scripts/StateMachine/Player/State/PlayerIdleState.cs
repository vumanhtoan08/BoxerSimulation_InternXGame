using UnityEngine;

public class PlayerIdleState : IState
{
    [Header("Ref")]
    private PlayerController playerController;

    public PlayerIdleState(PlayerController playerController)
    {
        this.playerController = playerController;
    }

    public void Enter()
    {
        playerController.Animator.SetTrigger("isTraning");
    }

    public void Excute()
    {
        float inputX = Mathf.Abs(Input.GetAxisRaw("Horizontal"));
        float inputZ = Mathf.Abs(Input.GetAxisRaw("Vertical"));
        float joyX = Mathf.Abs(playerController.FixedJoystick.Horizontal);
        float joyZ = Mathf.Abs(playerController.FixedJoystick.Vertical);

        bool hasInput = (inputX + inputZ + joyX + joyZ) > 0.1f;

        if (hasInput)
        {
            playerController.StateMachine.ChangeState(new PlayerMovingState(playerController));
        }
    }

    public void Exit()
    {
    }
}
