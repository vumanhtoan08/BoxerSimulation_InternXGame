using System.Collections.Generic;
using UnityEngine;

public class ShopManager : Singleton<ShopManager>
{
    #region Unity Methods

    public void OnStart()
    {

    }

    public void OnUpdate()
    {

    }

    #endregion

    #region Shop Behaviour Methods

    private InteractableBase currentInteractable;

    public void OnUpgradeInteractableItem()
    {

    }

    public void UpgradeBoxing(List<BoxingBagInteractable> boxingBags)
    {
        List<BoxingBagInteractable> boxings = new List<BoxingBagInteractable>();
    }

    #endregion
}
