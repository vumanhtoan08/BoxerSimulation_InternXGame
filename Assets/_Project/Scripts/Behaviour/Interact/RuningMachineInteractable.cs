using UnityEngine;

public class RuningMachineInteractable : InteractableBase
{
    public override void Interact()
    {
        base.Interact();
        if (playerController.Data.CurrentEnergy <= 0)
        {
            Debug.Log("Het nang luong");
            return;
        }
        playerController.StateMachine.ChangeState(new PlayerTrainingState(playerController.Animator, playerController.FixedJoystick,
                                                                    playerController.CharacterController, playerController.Speed,
                                                                    playerController.Gravity, playerController.GroundCheckDistance,
                                                                    playerController.GroundMask, playerController.StateMachine,
                                                                    playerController.transform));
    }
}
