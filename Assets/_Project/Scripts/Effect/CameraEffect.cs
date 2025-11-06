using Unity.Cinemachine;
using UnityEngine;

public class CameraEffect : Singleton<CameraEffect> 
{
    [Header("Cameras")]
    [SerializeField] private CinemachineBrain brain;

    [SerializeField] private CinemachineCamera mainCine;
    [SerializeField] private CinemachineCamera playerDeadCine;
    [SerializeField] private CinemachineCamera enemyIntroCine; 


}
