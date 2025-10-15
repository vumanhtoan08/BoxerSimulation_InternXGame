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
    private PlayerRunTimeDatas data; 

    #region Moving
    [Header("Ref")]
    [SerializeField] private FixedJoystick joystick;

    [Header("Variable")]
    [SerializeField, Range(0, 10)] private float speed = 2f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask groundMask = 0;

    #endregion

    #region GETSET 

    public Animator Animator => animator;
    public CharacterController CharacterController => character;
    public StateMachinePlayer StateMachine => stateMachine;
    public PlayerRunTimeDatas Data => data;
    public FixedJoystick FixedJoystick => joystick;

    public float Speed => speed;
    public float Gravity => gravity;
    public float GroundCheckDistance => groundCheckDistance;
    public LayerMask GroundMask => groundMask;

    #endregion

    private void Awake()
    {
        animator = GetComponent<Animator>();
        character = GetComponent<CharacterController>();
        stateMachine = GetComponent<StateMachinePlayer>();
        data = GetComponent<PlayerRunTimeDatas>();
    }

    private void Start()
    {
        stateMachine.ChangeState(new PlayerIdleState(animator, joystick, character, speed,
                                    gravity, groundCheckDistance, groundMask, stateMachine, this.transform));
        data?.OnStart();
    }

    public void Update()
    {
        stateMachine?.OnUpdate();
    }
}
