using UnityEngine;

public class PlayerMovingState :  IState
{
    [Header("Ref")]
    private PlayerController playerController; 

    private Vector3 velocity;
    private bool isGrounded;

    public PlayerMovingState(PlayerController playerController)
    {
        this.playerController = playerController;
    }

    public void Enter()
    {
        playerController.Animator?.SetBool("isMoving", true);
    }

    public void Excute()
    {
        Moving();
    }

    public void Exit()
    {
    }

    private void Moving()
    {
        isGrounded = Physics.CheckSphere(playerController.transform.position, playerController.GroundCheckDistance, playerController.GroundMask);
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        float joyX = playerController.FixedJoystick.Horizontal;
        float joyZ = playerController.FixedJoystick.Vertical;

        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        float finalX = Mathf.Abs(joyX) > 0.1f ? joyX : inputX;
        float finalZ = Mathf.Abs(joyZ) > 0.1f ? joyZ : inputZ;

        Vector3 move = playerController.transform.right * finalX + playerController.transform.forward * finalZ;
        playerController.CharacterController.Move(move * playerController.Speed * Time.deltaTime);

        velocity.y += playerController.Gravity * Time.deltaTime;
        playerController.CharacterController.Move(velocity * Time.deltaTime);

        bool isMoving = move.magnitude > 0.1f;
        if (!isMoving)
        {
            playerController.StateMachine.ChangeState(new PlayerIdleState(playerController));
        }
    }
}
