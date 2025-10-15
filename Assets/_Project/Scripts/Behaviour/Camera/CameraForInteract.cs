using UnityEngine;

public class CameraForInteract : MonoBehaviour
{
    [SerializeField] private Transform interactorSource;
    [SerializeField] private float interactRange = 3f;
    [SerializeField] private PlayerController playerController;

    private InteractableBase currentInteractable;

    public InteractableBase CurrentInteractable => currentInteractable;

    public void OnStart()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void OnUpdate()
    {
        Ray ray = new Ray(interactorSource.position, interactorSource.forward);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, interactRange))
        {
            if (hitInfo.collider.TryGetComponent(out InteractableBase interactOBJ))
            {
                if (currentInteractable != interactOBJ)
                {
                    currentInteractable?.OnRaycastExit();

                    currentInteractable = interactOBJ;
                    currentInteractable.OnRaycastHit();
                }

                if (Input.GetKeyDown(KeyCode.E) 
                    && playerController.StateMachine.CurrentState.ToString() != "PlayerTrainingState")
                {
                    currentInteractable.Interact();
                }
            }
        }
        else
        {
            if (currentInteractable != null)
            {
                currentInteractable.OnRaycastExit();
                currentInteractable = null;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (interactorSource == null) return;

        Gizmos.color = Color.green;
        Vector3 start = interactorSource.position;
        Vector3 end = start + interactorSource.forward * interactRange;
        Gizmos.DrawLine(start, end);
        Gizmos.DrawSphere(end, 0.05f);
    }
}
