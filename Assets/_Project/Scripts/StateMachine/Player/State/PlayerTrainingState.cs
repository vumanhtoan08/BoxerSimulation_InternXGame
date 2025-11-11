using DG.Tweening;
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
        //CanvasManager.Instance.OnBoxingComplete += CanvasManager.Instance.OnEnergyChange;
        CanvasManager.Instance.OnBoxingComplete += OnBoxingComplete;
        CanvasManager.Instance.OnRuningComplete += CanvasManager.Instance.OnEnergyChange;
        CanvasManager.Instance.OnRuningComplete += OnRunningComplete;
        CanvasManager.Instance.OnSquatComplete += CanvasManager.Instance.OnEnergyChange;
        CanvasManager.Instance.OnSquatComplete += OnSquatComplete;
    }

    private void OnBoxingComplete()
    {
        Sequence seq = DOTween.Sequence();

        seq.AppendCallback(() =>
        {
            CanvasManager.Instance.OnEnergyBarActive(false);
            playerController.Data.DataRuntime.UpProcess(TYPE_TRAINING.BOXING);
            playerController.Data.EnergyUse(1);
            CanvasManager.Instance.OnEnergyChange();
        })
        .AppendCallback(() =>
        {
            CanvasManager.Instance.OnEmitParticleAttractor(
                DataManager.Instance.ListInteractableTable.InteractableTables[0]
                    .Levels[DataManager.Instance.CurrentInteractableData.BoxingLevel].Value);

            CanvasManager.Instance.OnProcessTxtUpdate(TYPE_TRAINING.BOXING);
            CanvasManager.Instance.OnProcessImgFillUpdate(TYPE_TRAINING.BOXING);
        })
        .AppendInterval(2.5f)
        .AppendCallback(() =>
        {
            CanvasManager.Instance.OnUnActiveTrainingPanel();
            GameManager.Instance.ChangeGameState(Game_State.Training);
            
            if (!TutorialManager.Instance.Data.isPass)
            {
                TutorialManager.Instance.OnInteractWithRunningMachineTutorial(true);
            }
        });
    }

    private void OnRunningComplete()
    {
        Sequence seq = DOTween.Sequence();

        seq.AppendCallback(() =>
        {
            CanvasManager.Instance.OnEnergyBarActive(false);
            playerController.Data.DataRuntime.UpProcess(TYPE_TRAINING.RUNING);
            playerController.Data.EnergyUse(1);
            CanvasManager.Instance.OnEnergyChange();
        })
        .AppendCallback(() =>
        {
            CanvasManager.Instance.OnEmitParticleAttractor(
                DataManager.Instance.ListInteractableTable.InteractableTables[1]
                    .Levels[DataManager.Instance.CurrentInteractableData.RunningLevel].Value);

            CanvasManager.Instance.OnProcessTxtUpdate(TYPE_TRAINING.RUNING);
            CanvasManager.Instance.OnProcessImgFillUpdate(TYPE_TRAINING.RUNING);
        })
        .AppendInterval(2.5f)
        .AppendCallback(() =>
        {
            CanvasManager.Instance.OnUnActiveTrainingPanel();
            GameManager.Instance.ChangeGameState(Game_State.Training);

            if (!TutorialManager.Instance.Data.isPass)
            {
                TutorialManager.Instance.OnInteractWithDumbelRackTutorial(true);
            }
        });
    }

    private void OnSquatComplete()
    {
        Sequence seq = DOTween.Sequence();

        seq.AppendCallback(() =>
        {
            CanvasManager.Instance.OnEnergyBarActive(false);
            playerController.Data.DataRuntime.UpProcess(TYPE_TRAINING.SQUAT);
            playerController.Data.EnergyUse(1);
            CanvasManager.Instance.OnEnergyChange();
        })
        .AppendCallback(() =>
        {
            CanvasManager.Instance.OnEmitParticleAttractor(
                DataManager.Instance.ListInteractableTable.InteractableTables[2]
                    .Levels[DataManager.Instance.CurrentInteractableData.SquatLevel].Value);

            CanvasManager.Instance.OnProcessTxtUpdate(TYPE_TRAINING.SQUAT);
            CanvasManager.Instance.OnProcessImgFillUpdate(TYPE_TRAINING.SQUAT);
        })
        .AppendInterval(2.5f)
        .AppendCallback(() =>
        {
            CanvasManager.Instance.OnUnActiveTrainingPanel();
            GameManager.Instance.ChangeGameState(Game_State.Training);
        });
    }

    #endregion
}
