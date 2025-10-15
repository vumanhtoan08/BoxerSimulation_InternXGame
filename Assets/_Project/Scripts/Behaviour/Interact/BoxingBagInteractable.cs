using UnityEngine;

public class BoxingBagInteractable : InteractableBase
{
    public override void Interact()
    {
        base.Interact();
        CanvasManager.Instance.OnActiveTrainingPanel();
    }
}
