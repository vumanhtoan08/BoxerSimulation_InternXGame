using UnityEngine;

public class SquatInteractable : InteractableBase
{
    public override void Interact()
    {
        base.Interact();
        playerController.StateMachine.ChangeState(new PlayerTrainingState(playerController.Animator, playerController.FixedJoystick,
                                                                    playerController.CharacterController, playerController.Speed,
                                                                    playerController.Gravity, playerController.GroundCheckDistance,
                                                                    playerController.GroundMask, playerController.StateMachine,
                                                                    playerController.transform));
    }
}
