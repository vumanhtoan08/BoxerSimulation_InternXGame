using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour
{
    [SerializeField] private FixedTouchField fixedTouchField;
    [SerializeField] private CameraLook cameraLook;
  
    void Start()
    {
        cameraLook?.OnStart();
    }

    
    public void Update()
    {
        cameraLook?.OnUpdate();

        cameraLook.LockAxis = fixedTouchField.TouchDist;
    }
}
