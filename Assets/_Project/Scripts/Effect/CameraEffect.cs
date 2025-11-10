using Unity.Cinemachine;
using UnityEngine;

public class CameraEffect : Singleton<CameraEffect>
{
    [Header("Cameras")]
    [SerializeField] private CinemachineBrain brain;

    [SerializeField] private CinemachineCamera mainCine;
    [SerializeField] private CinemachineCamera playerDeadCine;
    [SerializeField] private CinemachineCamera enemyIntroCine;


    [Header("Object")]
    [SerializeField] private GameObject playerBehaviour;
    [SerializeField] private GameObject battleInfor;            // thanh mau 

    public void EnemyIntroCine(bool isActive, float blendTime)
    {
        // chỉnh thời gian blend cho CinemachineBrain
        var blend = brain.DefaultBlend;
        blend.Time = blendTime;
        brain.DefaultBlend = blend;

        // chuyển camera
        enemyIntroCine.Priority = isActive ? 100 : 0;
        if (isActive)
        {
            playerBehaviour.SetActive(false);
            battleInfor.SetActive(false);
        }
        else
        {
            playerBehaviour.SetActive(true);
            battleInfor.SetActive(true);
        }
    }

}
