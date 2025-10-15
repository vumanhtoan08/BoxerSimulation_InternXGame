using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<DataManager>
{
    [Header("REFERENCE")]
    [SerializeField] private DataManager dataManager;
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private DayManager dayManager;
    [SerializeField] private CanvasManager canvasManager;

    private void Start()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;

        //
        dayManager?.OnStart();
        canvasManager?.OnStart();
    }

    private void Update()
    {
        dayManager?.OnUpdate();
        canvasManager?.OnUpdate();
    }
}
