using UnityEngine;
using System.Collections.Generic;

public class InteractableManager : Singleton<InteractableManager>
{
    [SerializeField] private List<InteractableBase> interactableLists = new List<InteractableBase>();

    #region UnityMethods
    public void OnStart()
    {
        GameObject[] interactables = GameObject.FindGameObjectsWithTag("Interactable");
        foreach (var item in interactables)
        {
            interactableLists.Add(item.GetComponent<InteractableBase>());
        }
    }

    public void OnUpdate()
    {

    }
    #endregion
}
