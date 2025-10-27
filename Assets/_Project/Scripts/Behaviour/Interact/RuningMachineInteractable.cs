using UnityEngine;

public class RuningMachineInteractable : InteractableBase
{

    public override void Init()
    {
        base.Init();
        data.SetDataForInteractable(TYPE_TRAINING.RUNING);
    }

    public override void Interact()
    {
        base.Interact();
        if (playerController.Data.CurrentEnergy <= 0) return;

        playerController.StateMachine.ChangeState(new PlayerIdleState(playerController));
        playerController.StateMachine.ChangeState(new PlayerTrainingState(playerController));
    }
}
