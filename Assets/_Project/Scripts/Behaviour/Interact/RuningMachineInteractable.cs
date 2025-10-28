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
        SetPlayerPositionToIteractable();
    }

    [SerializeField] private Vector3 playerPosition;
    [SerializeField] private Vector3 playerRotation;
    [SerializeField] private Vector3 playerLook;

    protected override void SetPlayerPositionToIteractable()
    {
        base.SetPlayerPositionToIteractable();

        PlayerController.Instance.CharacterController.enabled = false;
        PlayerController.Instance.transform.position = new Vector3(transform.position.x, playerPosition.y, playerPosition.z);
        PlayerController.Instance.transform.rotation = Quaternion.Euler(playerRotation);
        PlayerController.Instance.CameraForInteract.transform.rotation = Quaternion.Euler(playerLook);
        PlayerController.Instance.CharacterController.enabled = true;
    }
}
