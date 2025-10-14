using UnityEngine;

public class PlayerIdleState : IState
{
    [Header("Ref")]
    private Animator animator;
    private FixedJoystick joystick;
    private CharacterController character;
    private StateMachinePlayer stateMachine;
    private Transform player; 

    [Header("Var")]
    private float speed;
    private float gravity;
    private float groundCheckDistance;
    private LayerMask groundMask;

    public PlayerIdleState(Animator animator, FixedJoystick joystick, CharacterController character,
                           float speed, float gravity, float groundCheckDistance, LayerMask groundMask,
                           StateMachinePlayer stateMachine, Transform player)
    {
        this.animator = animator;
        this.joystick = joystick;
        this.character = character;
        this.speed = speed;
        this.gravity = gravity;
        this.groundCheckDistance = groundCheckDistance;
        this.groundMask = groundMask;
        this.stateMachine = stateMachine;
        this.player = player;
    }

    public void Enter()
    {
        animator.SetBool("isMoving", false);
    }

    public void Excute()
    {
        float inputX = Mathf.Abs(Input.GetAxisRaw("Horizontal"));
        float inputZ = Mathf.Abs(Input.GetAxisRaw("Vertical"));
        float joyX = Mathf.Abs(joystick.Horizontal);
        float joyZ = Mathf.Abs(joystick.Vertical);

        bool hasInput = (inputX + inputZ + joyX + joyZ) > 0.1f;

        if (hasInput)
        {
            stateMachine.ChangeState(new PlayerMovingState(animator, joystick, character, speed, gravity,
                groundCheckDistance, groundMask, stateMachine, player)
            );
        }
    }

    public void Exit()
    {
    }
}
