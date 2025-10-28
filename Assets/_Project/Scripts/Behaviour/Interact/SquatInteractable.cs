using UnityEngine;

public class SquatInteractable : InteractableBase
{
    public override void Init()
    {
        base.Init();
        data.SetDataForInteractable(TYPE_TRAINING.SQUAT);
    }

    public override void Interact()
    {
        base.Interact();
        if (playerController.Data.CurrentEnergy <= 0) return;

        playerController.StateMachine.ChangeState(new PlayerIdleState(playerController));
        playerController.StateMachine.ChangeState(new PlayerTrainingState(playerController));
        SetPlayerPositionToIteractable();
    }

    [SerializeField] private Vector3 playerPosition;
    [SerializeField] private Vector3 playerRotation;
    [SerializeField] private Vector3 playerLook;

    protected override void SetPlayerPositionToIteractable()
    {
        base.SetPlayerPositionToIteractable();

        PlayerController.Instance.CharacterController.enabled = false;
        PlayerController.Instance.transform.position = playerPosition;
        PlayerController.Instance.transform.rotation = Quaternion.Euler(playerRotation);
        PlayerController.Instance.CameraForInteract.transform.rotation = Quaternion.Euler(playerLook);
        PlayerController.Instance.CharacterController.enabled = true;
    }
}
