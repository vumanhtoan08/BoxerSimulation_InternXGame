using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

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

    public void PlayerDeadCine(bool isActive)
    {
        playerDeadCine.Priority = isActive ? 100 : 0;
        if (isActive)
        {
            playerBehaviour.SetActive(false);
        }
        else
        {
            playerBehaviour.SetActive(true);
        }
    }

    #region Intro Cavas WorldSpace

    [SerializeField] private GameObject introduceObj;
    [SerializeField] private RectTransform darkPanelRect;
    [SerializeField] private RectTransform enemyNameRect;
    [SerializeField] private RectTransform difficultRect;
    [SerializeField] private Text enemyName; 
    [SerializeField] private Text difficult;

    [Header("Tween Settings")]
    [SerializeField] private float tweenDuration = 0.25f; 
    [SerializeField] private float offScreenOffset = 1920f;

    public void OnActiveIntro(bool isActive)
    {
        DOTween.Kill(darkPanelRect);
        DOTween.Kill(enemyNameRect);
        DOTween.Kill(difficultRect);

        if (isActive)
        {
            introduceObj.SetActive(true);

            enemyName.text = EnemyManager.Instance.EnemyController.RuntimeData.EnemyData.Name;
            difficult.text = EnemyManager.Instance.EnemyController.RuntimeData.EnemyData.Difficult.ToString();
            switch (EnemyManager.Instance.EnemyController.RuntimeData.EnemyData.Difficult)
            {
                case Enemy_Difficult.Easy:
                    difficult.color = Color.white;
                    break;
                case Enemy_Difficult.Med:
                    difficult.color = Color.yellow;
                    break;
                case Enemy_Difficult.Hard:
                    difficult.color = Color.red;
                    break;
            }

            darkPanelRect.anchoredPosition = new Vector2(offScreenOffset, 0);
            enemyNameRect.anchoredPosition = new Vector2(-offScreenOffset, 30);
            difficultRect.anchoredPosition = new Vector2(offScreenOffset, -30f);

            Time.timeScale = 0.5f;

            Sequence seq = DOTween.Sequence();

            seq.Append(darkPanelRect.DOAnchorPos(Vector2.zero, tweenDuration).SetEase(Ease.OutCubic))
               .Join(enemyNameRect.DOAnchorPos(new Vector2(0, 30), tweenDuration).SetEase(Ease.OutBack))
               .Join(difficultRect.DOAnchorPos(new Vector2(40f, -30f), tweenDuration).SetEase(Ease.OutBack))
               .SetUpdate(true); // chạy ngay cả khi timeScale != 1
        }
        else
        {
            Sequence seq = DOTween.Sequence();

            seq.Append(darkPanelRect.DOAnchorPos(new Vector2(offScreenOffset, 0), tweenDuration).SetEase(Ease.InCubic))
               .Join(enemyNameRect.DOAnchorPos(new Vector2(-offScreenOffset, 30), tweenDuration).SetEase(Ease.InBack))
               .Join(difficultRect.DOAnchorPos(new Vector2(offScreenOffset, -30f), tweenDuration).SetEase(Ease.InBack))
               .SetUpdate(true)
               .OnComplete(() =>
               {
                   introduceObj.SetActive(false);
                   Time.timeScale = 1f;
               });
        }
    }

    #endregion

}
