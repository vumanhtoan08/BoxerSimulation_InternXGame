using UnityEngine;

public class ArenaInteractable : InteractableBase
{
    public override void Interact()
    {
        base.Interact();
        //CanvasManager.Instance.OnActiveEnemyInfoPanel();
        PopupManager.Instance.ShowPopup(Type_Popup.InfoEnemy);
    }
}
