using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(StateMachinePlayer))]
public class PlayerController : Singleton<PlayerController>
{
    [Header("Reference")]
    private Animator animator;
    private CharacterController character;
    private StateMachinePlayer stateMachine;
    private PlayerRunTimeDatas data;
    private PlayerHealth health;

    [SerializeField] private Transform playerEyes;
    private CameraForInteract cameraForInteract;
    private CameraLook cameraLook;
    private TouchController touchController;

    #region Moving
    [Header("Ref")]
    [SerializeField] private FixedJoystick joystick;

    [Header("Variable")]
    [SerializeField, Range(0, 10)] private float speed = 2f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask groundMask = 0;

    #endregion

    [Header("Attack Collider")]
    [SerializeField] private Transform rightHand;
    [SerializeField] private Transform leftHand;
    [SerializeField] private LayerMask enemyMask;
    private bool isPunch = false;
    private bool isCounter = false;
    private bool isBlock = false;

    #region GETSET 

    public Animator Animator => animator;
    public CharacterController CharacterController => character;
    public StateMachinePlayer StateMachine => stateMachine;
    public PlayerRunTimeDatas Data => data;
    public PlayerHealth Health => health;
    public FixedJoystick FixedJoystick => joystick;

    public CameraForInteract CameraForInteract => cameraForInteract;

    public float Speed => speed;
    public float Gravity => gravity;
    public float GroundCheckDistance => groundCheckDistance;
    public LayerMask GroundMask => groundMask;

    public Transform RightHand => rightHand;
    public Transform LeftHand => leftHand;

    public LayerMask EnemyMask => enemyMask;

    public bool IsPunch => isPunch;
    public bool IsCounter => isCounter;

    public bool IsBlocking => isBlock;

    public bool SetPunch(bool value) => isPunch = value;
    public bool SetCounter(bool value) => isCounter = value;

    public bool SetBlock(bool value) => isBlock = value;

    #endregion

    public void OnAwake()
    {
        animator = GetComponent<Animator>();
        character = GetComponent<CharacterController>();
        stateMachine = GetComponent<StateMachinePlayer>();
        data = GetComponent<PlayerRunTimeDatas>();
        cameraForInteract = playerEyes.GetComponent<CameraForInteract>();
        cameraLook = playerEyes.GetComponent<CameraLook>();
        touchController = GetComponent<TouchController>();
        health = GetComponent<PlayerHealth>();
    }

    public void OnStart()
    {
        stateMachine.ChangeState(new PlayerIdleState(this));
        stateMachine?.OnStart();
        data?.OnStart();
        cameraForInteract?.OnStart();
        touchController?.OnStart();

        health?.Init(this);
    }

    public void OnUpdate()
    {
        stateMachine?.OnUpdate();
        cameraForInteract?.OnUpdate();
        touchController?.OnUpdate();
    }

    #region Sound

    private float randomPitch;

    public void PlayerFootStep()
    {
        randomPitch = Random.Range(0.6f, 0.8f);
        SoundManager.Instance.PlaySound(SoundKey.FootStep, 0.7f, randomPitch);
    }

    public void PlayerRun()
    {
        randomPitch = Random.Range(1f, 1.2f);
        SoundManager.Instance.PlaySound(SoundKey.FootStep, 0.7f, randomPitch);
    }

    public void PlayerBreath()
    {
        randomPitch = Random.Range(1f, 1.2f);
        SoundManager.Instance.PlaySound(SoundKey.Breath, 2f, randomPitch);
    }

    public void PlayerPunch()
    {
        randomPitch = Random.Range(0.6f, 1f);
        SoundManager.Instance.PlaySound(SoundKey.Punch, 0.7f, randomPitch);
    }

    #endregion
}
