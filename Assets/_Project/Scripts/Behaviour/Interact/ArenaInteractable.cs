using UnityEngine;

public class ArenaInteractable : InteractableBase
{
    public override void Interact()
    {
        if (!TutorialManager.Instance.Data.isPass)
        {
            TutorialManager.Instance.OnInteractWithBattleRingTutorial(false);
        }

        base.Interact();
        //CanvasManager.Instance.OnActiveEnemyInfoPanel();
        PopupManager.Instance.ShowPopup(Type_Popup.InfoEnemy);
    }
}
