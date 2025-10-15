using UnityEngine;

[RequireComponent(typeof(Outline))]
public class InteractableBase : MonoBehaviour, IInteractable
{
    protected Outline outline;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }

    public virtual void Interact()
    {
        Debug.Log("Interacted with " + gameObject.name);
    }

    public virtual void OnRaycastHit()
    {
        outline.enabled = true; 
    }

    public virtual void OnRaycastExit()
    {
        outline.enabled = false; 
    }
}
