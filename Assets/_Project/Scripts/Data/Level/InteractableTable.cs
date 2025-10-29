using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInteractableTable", menuName = "Database/Interactable Table")]
public class InteractableTable : ScriptableObject
{
    public string ID;
    public string Name;
    public List<InteractableLevelData> Levels = new List<InteractableLevelData>();

    [System.Serializable]
    public class InteractableLevelData
    {
        public int Level;
        public int Value;
        public int Cost;
    }
}
