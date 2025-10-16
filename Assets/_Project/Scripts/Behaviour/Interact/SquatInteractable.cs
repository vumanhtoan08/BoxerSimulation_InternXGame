using UnityEngine;

public class SquatInteractable : InteractableBase
{
    public override void Interact()
    {
        base.Interact();
        if (playerController.Data.CurrentEnergy <= 0)
        {
            Debug.Log("Het nang luong");
            return;
        }

        playerController.StateMachine.ChangeState(new PlayerTrainingState(playerController));
    }
}
