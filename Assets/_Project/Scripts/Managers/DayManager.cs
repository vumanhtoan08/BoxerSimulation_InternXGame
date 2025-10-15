using UnityEngine;

public class DayManager : Singleton<DayManager>
{
    [SerializeField] private int currentDay;
    [SerializeField] private PlayerRunTimeDatas data; 

    public void OnStart()
    {
        data = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerRunTimeDatas>();
    }

    public void CheckConditionToNextDay()
    {
        if (data.EnergyUse())
        {
            MoveToNextDay();
        }
        else
        {
            Debug.Log("con nang luong");
        }
    }

    private void MoveToNextDay()
    {
        currentDay += 1;
        data.EnergyRegen();
        Debug.Log("Sang ngay moi" + currentDay);
    }

    public void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            data.EnergyUse(1);
        }
    }
}
