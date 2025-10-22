using UnityEngine;

public class RuningMachineInteractable : InteractableBase
{
    [SerializeField] private InteractableData data;

    public override void Init()
    {
        base.Init();
        data.SetDataForInteractable(TYPE_TRAINING.RUNING);
    }

    public override void Interact()
    {
        base.Interact();
        if (playerController.Data.CurrentEnergy <= 0)
        {
            Debug.Log("Het nang luong");
            return;
        }

        playerController.StateMachine.ChangeState(new PlayerIdleState(playerController));
        playerController.StateMachine.ChangeState(new PlayerTrainingState(playerController));
    }
}
