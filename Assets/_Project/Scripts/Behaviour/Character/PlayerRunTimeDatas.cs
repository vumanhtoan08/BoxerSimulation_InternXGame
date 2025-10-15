using UnityEngine;

public class PlayerRunTimeDatas : MonoBehaviour
{
    [Header("Data Runtime")]
    private PlayerDatas dataRuntime;

    [Header("Data Energy")]
    [SerializeField] private int maxEnergy = 3;
    private int currentEnergy;

    public void OnStart()
    {
        currentEnergy = maxEnergy;
    }

    #region Methods Energy
    
    public bool EnergyUse(int amount = 0)
    {
        currentEnergy -= amount;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
        bool isHasEnergy = currentEnergy <= 0 ? true : false;
        return isHasEnergy;
    }

    public void EnergyRegen()
    {
        currentEnergy = maxEnergy;
        Debug.Log("Hoi phuc the luc");
    }

    #endregion
}
