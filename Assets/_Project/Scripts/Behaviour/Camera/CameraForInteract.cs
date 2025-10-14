using UnityEngine;

public class CameraForInteract : MonoBehaviour
{
    [SerializeField] private Transform interactorSource;
    [SerializeField] private float interactRange;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray r = new Ray(interactorSource.position, interactorSource.forward);
            if (Physics.Raycast(r, out RaycastHit hitInfo, interactRange))
                if (hitInfo.collider.TryGetComponent(out IInteractable interactOBJ))
                {
                    interactOBJ.Interact();
                }
        }
    }

    private void OnDrawGizmos()
    {
        if (interactorSource == null) return;

        Gizmos.color = Color.green; // màu của tia
        Vector3 start = interactorSource.position;
        Vector3 end = start + interactorSource.forward * interactRange;

        Gizmos.DrawLine(start, end);
        Gizmos.DrawSphere(end, 0.05f); // vẽ chấm nhỏ ở điểm kết thúc (tùy chọn)
    }
}