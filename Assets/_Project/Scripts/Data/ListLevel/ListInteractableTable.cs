using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInteractableTable", menuName = "Database/Interactable List")]
public class ListInteractableTable : ScriptableObject
{
    public List<InteractableTable> InteractableTables = new List<InteractableTable>();
}
