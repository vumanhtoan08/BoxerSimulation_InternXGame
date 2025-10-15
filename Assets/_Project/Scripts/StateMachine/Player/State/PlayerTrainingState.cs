using UnityEngine;

public class PlayerTrainingState : IState
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

    public PlayerTrainingState(Animator animator, FixedJoystick joystick, CharacterController character,
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
        CanvasManager.Instance.OnActiveTrainingPanel();
        OnChangeStateTraining();
    }

    public void Excute()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            CanvasManager.Instance.OnUnActiveTrainingPanel();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            CanvasManager.Instance.OnTraining();
        }
    }

    public void Exit()
    {

    }

    #region Methods

    private void OnChangeStateTraining()
    {
        CanvasManager.Instance.OnBoxingComplete += OnBoxingComplete;
    }

    private void OnBoxingComplete()
    {
        // tang suc manh

        stateMachine.ChangeState(new PlayerIdleState(animator, joystick, character, speed, gravity, groundCheckDistance,
                                                    groundMask, stateMachine, player));
        Debug.Log("Hoan thanh bai boxing");
    }

    #endregion
}
