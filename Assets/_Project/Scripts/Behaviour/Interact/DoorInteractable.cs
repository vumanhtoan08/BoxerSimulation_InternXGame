using UnityEngine;

public class DoorInteractable : InteractableBase
{
    public override void Interact()
    {
        base.Interact();
        DayManager.Instance.CheckConditionToNextDay();
    }
}
