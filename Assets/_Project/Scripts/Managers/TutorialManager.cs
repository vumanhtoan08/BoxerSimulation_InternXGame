using Coffee.UIExtensions;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : Singleton<TutorialManager>
{
    private TutorialData data = new();
    public TutorialData Data => data;

    private bool isPassBoxing;
    public bool IsPassBoxing => isPassBoxing;
    private bool isPassRunning;
    private bool isPassSquat;

    [Header("Ref")]
    [SerializeField] private GameObject irisShot;
    [SerializeField] private Unmask unmask;
    [SerializeField] private RectTransform unmaskRect;

    [Header("Player Btn")]
    [SerializeField] private RectTransform moveBtn;
    [SerializeField] private RectTransform interactBtn;

    [Header("Hand Anim")]
    [SerializeField] private GameObject handTut_Cirle;
    [SerializeField] private GameObject handTut_Tap;

    [Header("Interact")]
    [SerializeField] private Transform boxingBag;
    [SerializeField] private Transform runningMachine;
    [SerializeField] private Transform dumbelRack;

    [Header("Arrow")]
    [SerializeField] private GameObject arrowObj;
    [SerializeField] private Transform arrowTransform;

    #region Unity Methods

    public void OnStart()
    {
        data.SetDataForTutorial();

        OnMoveTutorialActive(!data.isPass);
    }

    public void OnUpdate()
    {
        if (data.isPass) return;

            if (!isPassBoxing && Vector3.Distance(PlayerController.Instance.transform.position, boxingBag.position) <= 4f)
        {
            isPassBoxing = true;
            OnTrainingBoxingTutorial(true);
        }

        if (!isPassBoxing) return;

        if (!isPassRunning && Vector3.Distance(PlayerController.Instance.transform.position, runningMachine.position) <= 4f)
        {
            isPassRunning = true;
            OnTrainingRunningTutorial(true);
        }

        if (!isPassBoxing || !isPassRunning) return;

        if (!isPassSquat && Vector3.Distance(PlayerController.Instance.transform.position, dumbelRack.position) <= 4f)
        {
            isPassSquat = true;
            OnTrainingDumbelTutorial(true);
        }
    }

    #endregion

    #region Step 1 Moving

    public void OnMoveTutorialActive(bool isActive)
    {
        if (isActive)
        {
            irisShot.SetActive(true);
            unmask.FitTo(moveBtn);
            handTut_Cirle.SetActive(true);
        }
        else
        {
            irisShot.SetActive(false);
            handTut_Cirle.SetActive(false);
        }
    }

    #endregion

    #region Step 2 Boxing

    public void OnInteractWithBoxingTutorial(bool isActive)
    {
        if (isActive)
        {
            arrowObj.SetActive(true);
            arrowTransform.position = new Vector3(9f, 4f, 7.42f);
            PlayerController.Instance.CameraLook.LookAtTarget(boxingBag);
        }
        else
        {
            arrowObj.SetActive(false);
        }
    }

    public void OnTrainingBoxingTutorial(bool isActive)
    {
        if (isActive)
        {
            irisShot.SetActive(true);
            unmask.FitTo(interactBtn);
            handTut_Tap.SetActive(true);
            PlayerController.Instance.CameraLook.LookAtTarget(boxingBag);
        }
        else
        {
            irisShot.SetActive(false);
            handTut_Tap.SetActive(false);
        }
    }

    #endregion

    #region Step 3 Running

    public void OnInteractWithRunningMachineTutorial(bool isActive)
    {
        if (isActive)
        {
            arrowObj.SetActive(true);
            arrowTransform.position = new Vector3(-2.5f, 2.5f, 7.5f);
            PlayerController.Instance.CameraLook.LookAtTarget(runningMachine);
        }
        else
        {
            arrowObj.SetActive(false);
        }
    }

    public void OnTrainingRunningTutorial(bool isActive)
    {
        if (isActive)
        {
            handTut_Tap.SetActive(true);
            PlayerController.Instance.CameraLook.LookAtTarget(runningMachine);
        }
        else
        {
            handTut_Tap.SetActive(false);
        }
    }

    #endregion

    #region Step 4 Dumbell

    public void OnInteractWithDumbelRackTutorial(bool isActive)
    {
        if (isActive)
        {
            arrowObj.SetActive(true);
            arrowTransform.position = new Vector3(8.5f, 2f, 0f);
            PlayerController.Instance.CameraLook.LookAtTarget(dumbelRack);
        }
        else
        {
            arrowObj.SetActive(false);
        }
    }

    public void OnTrainingDumbelTutorial(bool isActive)
    {
        if (isActive)
        {
            handTut_Tap.SetActive(true);
            PlayerController.Instance.CameraLook.LookAtTarget(dumbelRack);
        }
        else
        {
            handTut_Tap.SetActive(false);
        }
    }

    #endregion
}

[System.Serializable]
public class DataSaveForTutorial
{
    public bool isPass;

    public DataSaveForTutorial()
    {
        isPass = false;
    }
}

[System.Serializable]
public class TutorialData
{
    public bool isPass;

    public void SetDataForTutorial()
    {
        isPass = DataManager.Instance.CurrentTutorialData.isPass;
    }
}
