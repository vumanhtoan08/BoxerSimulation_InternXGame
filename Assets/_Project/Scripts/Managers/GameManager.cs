using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [Header("REFERENCE")]
    [SerializeField] private DataManager dataManager;
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private DayManager dayManager;
    [SerializeField] private CanvasManager canvasManager;
    [SerializeField] private InteractableManager interactableManager;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private PopupManager popupManager;
    [SerializeField] private WalletManager walletManager;
    [SerializeField] private ShopManager shopManager;

    [SerializeField] private PlayerController playerController;
    [SerializeField] private FixedTouchField fixedTouchField; 

    [Header("Parameters")]
    private Game_State gameState;
    public Game_State GameState => gameState;

    protected override void Awake()
    {
        base.Awake();
        dataManager?.OnAwake();

        soundManager?.OnAwake();

        playerController?.OnAwake();
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;

        //
        StartState();
       

        dataManager?.OnStart();
        playerController?.OnStart();
        soundManager?.OnStart();
        dayManager?.OnStart();
        interactableManager?.OnStart();
        enemyManager?.OnStart();
        popupManager?.OnStart();
        walletManager?.OnStart();
        shopManager?.OnStart();
        canvasManager?.OnStart();

        StartingSetupForSplashScreen();
        ChangeGameState(Game_State.Training);
    }

    private void Update()
    {
        fixedTouchField.OnUpdate();

        //dataManager?.OnUpdate();
        soundManager?.OnUpdate();
        dayManager?.OnUpdate();
        canvasManager?.OnUpdate();
        interactableManager?.OnUpdate();

        if (gameState == Game_State.Battle)
            enemyManager?.OnUpdate();

        popupManager?.OnUpdate();
        walletManager?.OnUpdate();
        shopManager?.OnUpdate();

        playerController?.OnUpdate();
    }

    #region StateGame Manager

    private void StartState()
    {
        gameState = Game_State.Init;
    }

    public void ChangeGameState(Game_State newState)
    {
        gameState = newState;

        switch (gameState)
        {
            case Game_State.Init:
                break;
            case Game_State.Pause:
                break;
            case Game_State.Training:
                PlayerController.Instance.StateMachine.ChangeState(new PlayerIdleState(PlayerController.Instance));
                break;
            case Game_State.OnTraning:
                break;
            case Game_State.Battle:
                PlayerController.Instance.StateMachine.ChangeState(new PlayerBattleState(PlayerController.Instance));
                break;
            case Game_State.Win:
                PopupManager.Instance.ShowPopup(Type_Popup.Win);
                break;
            case Game_State.Lose:
                CanvasManager.Instance.OnPlayerHealthChange();
                PopupManager.Instance.ShowPopup(Type_Popup.Lose);
                break;
        }

        CanvasManager.Instance.UpdateActionButtonsByGameState(gameState);
    }

    #endregion

    #region For Splash Screen 

    [SerializeField] private GameObject splashScreen;
    [SerializeField] private Image fillImage;
    [SerializeField] private float duration = 2f;

    private Tween fillTween;

    private void StartingSetupForSplashScreen()
    {
        if (fillImage == null) return;

        // Reset trạng thái
        fillImage.fillAmount = 0f;
        splashScreen.SetActive(true);

        // Dừng tween cũ nếu còn chạy
        fillTween?.Kill();

        // Tạo tween tăng fillAmount từ 0 → 1 trong duration
        fillTween = fillImage.DOFillAmount(1f, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                splashScreen.SetActive(false);
                Debug.Log("Splash done!");
            
                CanvasManager.Instance.DarkPanelUnActive();
            });
    }

    #endregion
}

public enum Game_State
{
    Init,
    Pause,
    Training,
    OnTraning, 
    Battle,
    Win,
    Lose,
}