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
                playerController.Animator.SetBool("isBoxing", true);
                break;
            case TYPE_TRAINING.RUNING:
                playerController.Animator.SetBool("isRunning", true);
                break;
            case TYPE_TRAINING.SQUAT:
                playerController.Animator.SetBool("isSquat", true);
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
                playerController.Animator.SetBool("isBoxing", false);
                break;
            case TYPE_TRAINING.RUNING:
                playerController.Animator.SetBool("isRunning", false);
                break;
            case TYPE_TRAINING.SQUAT:
                playerController.Animator.SetBool("isSquat", false);
                break;
            default:
                break;
        }
    }

    #region Methods

    private void OnChangeStateTraining()
    {
        CanvasManager.Instance.OnBoxingComplete += OnBoxingComplete;
        CanvasManager.Instance.OnRuningComplete += OnRunningComplete;
        CanvasManager.Instance.OnSquatComplete += OnSquatComplete;
    }

    private void OnBoxingComplete()
    {
        // tang suc manh
        playerController.StateMachine.ChangeState(new PlayerIdleState(playerController));
        playerController.Data.EnergyUse(1);
        Debug.Log("Hoan thanh bai boxing");
    }

    private void OnRunningComplete()
    {
        // tang suc manh
        playerController.StateMachine.ChangeState(new PlayerIdleState(playerController));
        playerController.Data.EnergyUse(1);
        Debug.Log("Hoan thanh bai chay");
    }
    private void OnSquatComplete()
    {
        // tang suc manh
        playerController.StateMachine.ChangeState(new PlayerIdleState(playerController));
        playerController.Data.EnergyUse(1);
        Debug.Log("Hoan thanh bai squat");
    }


    #endregion
}
