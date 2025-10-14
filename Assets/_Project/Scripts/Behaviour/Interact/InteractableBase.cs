using UnityEngine;

[RequireComponent(typeof(Outline))]
public class InteractableBase : MonoBehaviour, IInteractable
{
    private Outline outline;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }

    public void Interact()
    {
        Debug.Log("Interacted with " + gameObject.name);
    }

    public void OnRaycastHit()
    {
        outline.enabled = true; 
    }

    public void OnRaycastExit()
    {
        outline.enabled = false; 
    }
}
