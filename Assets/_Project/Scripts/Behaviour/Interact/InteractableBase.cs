using UnityEngine;

[RequireComponent(typeof(Outline))]
public class InteractableBase : MonoBehaviour, IInteractable
{
    [SerializeField] protected PlayerController playerController;
    [SerializeField] protected TYPE_TRAINING type; 
    protected Outline outline;
    [SerializeField] protected InteractableData data;
    public InteractableData Data => data;

    public TYPE_TRAINING Type => type;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public virtual void Init()
    {

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

public enum TYPE_TRAINING
{
    NONE,
    BOXING, 
    RUNING,
    SQUAT
}