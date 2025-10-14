using UnityEngine;

public class PlayerMovingState :  IState
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

    private Vector3 velocity;
    private bool isGrounded;

    public PlayerMovingState(Animator animator, FixedJoystick joystick, CharacterController character,
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
        animator?.SetBool("isMoving", true);
        Debug.Log("ENTER MOVING");
    }

    public void Excute()
    {
        Moving();
    }

    public void Exit()
    {
        Debug.Log("EXIT MOVING");
    }

    private void Moving()
    {
        isGrounded = Physics.CheckSphere(player.position, groundCheckDistance, groundMask);
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        float joyX = joystick.Horizontal;
        float joyZ = joystick.Vertical;

        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        float finalX = Mathf.Abs(joyX) > 0.1f ? joyX : inputX;
        float finalZ = Mathf.Abs(joyZ) > 0.1f ? joyZ : inputZ;

        Vector3 move = player.right * finalX + player.forward * finalZ;
        character.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        character.Move(velocity * Time.deltaTime);

        bool isMoving = move.magnitude > 0.1f;
        if (!isMoving)
        {
            stateMachine.ChangeState(new PlayerIdleState(animator, joystick, character,
                speed, gravity, groundCheckDistance, groundMask, stateMachine, player));
        }
    }
}
