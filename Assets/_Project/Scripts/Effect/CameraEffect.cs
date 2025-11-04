using Unity.Cinemachine;
using UnityEngine;

public class CameraEffect : Singleton<CameraEffect> 
{
    [Header("Cameras")]
    [SerializeField] private CinemachineCamera playerCine;
    [SerializeField] private CinemachineCamera enemyCine; 


}
