using UnityEngine;

public class PlayerTrainingState : IState
{
    [Header("Ref")]
    private PlayerController playerController;

    public PlayerTrainingState(PlayerController playerController)
    {
        this.playerController = playerController;
    }

    public void Enter()
    {
        switch (playerController.CameraForInteract.CurrentInteractable.Type)
        {
            case TYPE_TRAINING.NONE:
                break;
            case TYPE_TRAINING.BOXING:
                break;
            case TYPE_TRAINING.RUNING:
                playerController.Animator.SetBool("isRunning", true);
                break;
            case TYPE_TRAINING.SQUAT:
                break;
            default:
                break;
        }


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
        switch (playerController.CameraForInteract.CurrentInteractable.Type)
        {
            case TYPE_TRAINING.NONE:
                break;
            case TYPE_TRAINING.BOXING:
                playerController.Animator.SetBool("isMoving", false);
                break;
            case TYPE_TRAINING.RUNING:
                playerController.Animator.SetBool("isMoving", false);
                playerController.Animator.SetBool("isRunning", false);
                break;
            case TYPE_TRAINING.SQUAT:
                playerController.Animator.SetBool("isMoving", false);
                PlayerController.Instance.StateMachine.SetActiveForWeight(false);
                break;
            default:
                break;
        }
    }

    #region Methods

    private void OnChangeStateTraining()
    {
        CanvasManager.Instance.OnBoxingComplete += OnBoxingComplete;
        CanvasManager.Instance.OnBoxingComplete += CanvasManager.Instance.OnEnergyChange;
        CanvasManager.Instance.OnRuningComplete += OnRunningComplete;
        CanvasManager.Instance.OnRuningComplete += CanvasManager.Instance.OnEnergyChange;
        CanvasManager.Instance.OnSquatComplete += OnSquatComplete;
        CanvasManager.Instance.OnSquatComplete += CanvasManager.Instance.OnEnergyChange;
    }

    private void OnBoxingComplete()
    {
        // tang suc manh
        playerController.StateMachine.ChangeState(new PlayerIdleState(playerController));
        playerController.Data.EnergyUse(1);
        playerController.Data.DataRuntime.UpProcess(TYPE_TRAINING.BOXING);
        Debug.Log("Hoan thanh bai boxing");

        GameManager.Instance.ChangeGameState(Game_State.Training);
    }

    private void OnRunningComplete()
    {
        // tang suc manh
        playerController.StateMachine.ChangeState(new PlayerIdleState(playerController));
        playerController.Data.EnergyUse(1);
        playerController.Data.DataRuntime.UpProcess(TYPE_TRAINING.RUNING);
        Debug.Log("Hoan thanh bai chay");

        GameManager.Instance.ChangeGameState(Game_State.Training);
    }
    private void OnSquatComplete()
    {
        // tang suc manh
        playerController.StateMachine.ChangeState(new PlayerIdleState(playerController));
        playerController.Data.EnergyUse(1);
        playerController.Data.DataRuntime.UpProcess(TYPE_TRAINING.SQUAT);
        Debug.Log("Hoan thanh bai squat");

        GameManager.Instance.ChangeGameState(Game_State.Training);
    }


    #endregion
}
