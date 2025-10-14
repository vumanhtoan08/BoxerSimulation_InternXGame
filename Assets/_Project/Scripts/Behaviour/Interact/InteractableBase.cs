using UnityEngine;

public class InteractableBase : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log(gameObject.name);
    }
}
