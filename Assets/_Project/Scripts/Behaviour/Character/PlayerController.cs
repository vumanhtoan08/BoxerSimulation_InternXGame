using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(StateMachinePlayer))]
public class PlayerController : MonoBehaviour
{
    [Header("Reference")]
    private Animator animator;
    private CharacterController character;
    private StateMachinePlayer stateMachine;

    #region Moving
    [Header("Ref")]
    [SerializeField] private FixedJoystick joystick;

    [Header("Variable")]
    [SerializeField, Range(0, 10)] private float speed = 2f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask groundMask = 0;

    #endregion

    private void Awake()
    {
        animator = GetComponent<Animator>();
        character = GetComponent<CharacterController>();
        stateMachine = GetComponent<StateMachinePlayer>();
    }

    private void Start()
    {
        stateMachine.ChangeState(new PlayerIdleState(animator, joystick, character, speed,
                                    gravity, groundCheckDistance, groundMask, stateMachine, this.transform));
    }

    public void Update()
    {
        stateMachine?.OnUpdate();
    }
}
