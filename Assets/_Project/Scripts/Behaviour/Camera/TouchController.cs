using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour
{
    [SerializeField] private FixedTouchField fixedTouchField;
    [SerializeField] private CameraLook cameraLook;

    public CameraLook CameraLook => cameraLook;
  
    public void OnStart()
    {
        cameraLook?.OnStart();
    }

    
    public void OnUpdate()
    {
        cameraLook?.OnUpdate();

        cameraLook.LockAxis = fixedTouchField.TouchDist;
    }
}
