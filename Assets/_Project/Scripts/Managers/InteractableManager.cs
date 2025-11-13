using UnityEngine;
using System.Collections.Generic;

public class InteractableManager : Singleton<InteractableManager>
{
    [SerializeField] private List<InteractableBase> interactableLists = new List<InteractableBase>();

    public List<InteractableBase> InteractableLists => interactableLists;

    #region UnityMethods
    public void OnStart()
    {
        GameObject[] interactables = GameObject.FindGameObjectsWithTag("Interactable");
        foreach (var item in interactables)
        {
            interactableLists.Add(item.GetComponent<InteractableBase>());
        }

        foreach (var item in interactableLists)
        {
            item.Init();
        }
    }

    public void OnUpdate()
    {

    }
    #endregion
}
