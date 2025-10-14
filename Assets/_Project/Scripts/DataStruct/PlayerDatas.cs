using UnityEngine;
using System;

[Serializable]
public class PlayerDatas
{
    [Header("Base Stats")]
    public float Attack;
    public float Health;
    public float Stamina;

    [Header("Training Process")]
    public int CurrentAttackProcess;
    public int MaxAttackProcess;
    public int CurrentHealthProcess;
    public int MaxHealthProcess;
    public int CurrentStaminaProcess;
    public int MaxStaminaProcess;

    [Header("Cost Stamina")]
    public float PunchCost;
    public float CounterCost;
    public float BlockCost;
}

#region Player's Stats 

[Serializable]
public class LevelDataForStats
{
    public string ID;
    public int Level;
    public float Value;
    public int CurrentProcess;
    public int MaxProcess;
}

#endregion 

#region Player's Data Skill 

[Serializable]
public class LevelDataForSkillPunch
{
    public string ID;
    public int Level;
    public float Cost;
}

#endregion