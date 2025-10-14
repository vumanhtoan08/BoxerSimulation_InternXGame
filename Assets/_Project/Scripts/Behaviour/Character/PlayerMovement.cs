using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Ref")]
    private Animator animator;
    [SerializeField] private FixedJoystick joystick;
    private CharacterController character;
    private StateMachinePlayer stateMachine; 

    [Header("Variable")]
    [SerializeField, Range(0, 10)] private float speed = 2f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask groundMask = 0;

    private Vector3 velocity;
    private bool isGrounded;

    public void Init(Animator animator, CharacterController character, StateMachinePlayer stateMachine)
    {
        this.animator = animator;
        this.character = character;
        this.stateMachine = stateMachine;
    }

    public void OnUpdate()
    {
        CharacterMove();
    }

    private void CharacterMove()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        float joyX = joystick.Horizontal;
        float joyZ = joystick.Vertical;

        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        float finalX = Mathf.Abs(joyX) > 0.1f ? joyX : inputX;
        float finalZ = Mathf.Abs(joyZ) > 0.1f ? joyZ : inputZ;

        Vector3 move = transform.right * finalX + transform.forward * finalZ;
        character.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        character.Move(velocity * Time.deltaTime);

        bool isMoving = move.magnitude > 0.1f;
        animator?.SetBool("isMoving", isMoving);
    }
}
