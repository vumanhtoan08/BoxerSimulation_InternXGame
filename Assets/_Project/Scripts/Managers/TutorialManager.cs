using Coffee.UIExtensions;
using DG.Tweening;
using System.Collections.Generic;
using Unity.Android.Gradle.Manifest;
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
    private bool isPassBattle;

    [Header("Ref")]
    [SerializeField] private GameObject irisShot;
    [SerializeField] private Unmask unmask;
    [SerializeField] private Image unmaskImg;
    [SerializeField] private RectTransform unmaskRect;
    [SerializeField] private Sprite circleSprite;
    [SerializeField] private Sprite rectangleSprite;

    [Header("Player Btn")]
    [SerializeField] private RectTransform moveBtn;
    [SerializeField] private RectTransform interactBtn;
    [SerializeField] private RectTransform fightBtn;

    [Header("Hand Anim")]
    [SerializeField] private GameObject handTut_Cirle;
    [SerializeField] private GameObject handTut_Tap;
    [SerializeField] private GameObject handTut_Tap_2;

    [Header("Interact")]
    [SerializeField] private Transform boxingBag;
    [SerializeField] private Transform runningMachine;
    [SerializeField] private Transform dumbelRack;
    [SerializeField] private Transform battleRing;

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

    #region Story Telling 

    [Header("Image")]
    [SerializeField] private GameObject canvasStory;
    [SerializeField] private List<Image> listImgStory;
    [SerializeField] private List<Text> textsStory;

    public void StoryTelling(bool isActive)
    {
        if (!isActive)
        {
            canvasStory.SetActive(false);
            return;
        }

        Sequence seq = DOTween.Sequence();

        seq.AppendCallback(() => SoundManager.Instance.PlaySound(SoundKey.LoiThoai_01))
            .Append(FadeImageByIndex(0))
            .Append(FadeTextByIndex(0))
            .AppendInterval(2.5f)

            .AppendCallback(() => SoundManager.Instance.PlaySound(SoundKey.LoiThoai_02))
            .Append(FadeImageByIndex(1))
            .Append(FadeTextByIndex(1))
            .AppendInterval(6f)

            .AppendCallback(() => SoundManager.Instance.PlaySound(SoundKey.LoiThoai_03))
            .Append(FadeImageByIndex(2))
            .Append(FadeTextByIndex(2))
            .AppendInterval(4.5f)

            .AppendCallback(() => SoundManager.Instance.PlaySound(SoundKey.LoiThoai_04))
            .Append(FadeImageByIndex(3))
            .Append(FadeTextByIndex(3))
            .AppendInterval(4f)

            .AppendCallback(() => SoundManager.Instance.PlaySound(SoundKey.LoiThoai_05))
            .Append(FadeImageByIndex(4))
            .Append(FadeTextByIndex(4))
            .AppendInterval(3.5f)

            .AppendCallback(() => StoryTelling(false));
    }

    /// <summary>
    /// Fade ảnh theo index trong listImgStory từ 0 -> 1 opacity trong 0.5s
    /// </summary>
    public Tween FadeImageByIndex(int index)
    {
        if (index < 0 || index >= listImgStory.Count) return null;

        Image img = listImgStory[index];
        img.color = new Color(img.color.r, img.color.g, img.color.b, 0f); // set alpha ban đầu
        return img.DOFade(1f, 0.5f).SetEase(Ease.Linear);
    }

    /// <summary>
    /// Fade text theo index trong textsStory từ 0 -> 1 opacity trong 0.5s
    /// </summary>
    public Tween FadeTextByIndex(int index)
    {
        if (index < 0 || index >= textsStory.Count) return null;

        Text txt = textsStory[index];
        txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 0f); // set alpha ban đầu
        return txt.DOFade(1f, 0.5f).SetEase(Ease.Linear);
    }


    #endregion

    #region Step 1 Moving

    public void OnMoveTutorialActive(bool isActive)
    {
        if (isActive)
        {
            irisShot.SetActive(true);
            unmaskImg.sprite = circleSprite;
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

    #region Step 5 Battle

    public void OnInteractWithBattleRingTutorial(bool isActive)
    {
        if (isActive)
        {
            arrowObj.SetActive(true);
            arrowTransform.position = new Vector3(-6.5f, 2f, -6.5f);
            PlayerController.Instance.CameraLook.LookAtTarget(battleRing);
        }
        else
        {
            arrowObj.SetActive(false);
        }
    }

    public void OnTrainingBattleTutorial(bool isActive)
    {
        if (isActive)
        {
            irisShot.SetActive(true);
            unmaskImg.sprite = rectangleSprite;
            unmask.FitTo(fightBtn);
            handTut_Tap_2.SetActive(true);
        }
        else
        {
            irisShot.SetActive(false);
            handTut_Tap_2.SetActive(false);

            CompleteTutorial();
        }
    }

    private void CompleteTutorial()
    {
        data.isPass = true;
        DataManager.Instance.CurrentTutorialData.isPass = data.isPass;
        DataManager.Instance.SaveData();
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
