using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    [Header("Parameters")]
    private Game_State gameState;
    public Game_State GameState => gameState;

    protected override void Awake()
    {
        base.Awake();
        dataManager?.OnAwake();

        playerController?.OnAwake();
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;

        //
        StartState();
        StartingSetupForSplashScreen();

        dataManager?.OnStart();
        soundManager?.OnStart();
        dayManager?.OnStart();
        interactableManager?.OnStart();
        enemyManager?.OnStart();
        popupManager?.OnStart();
        walletManager?.OnStart();
        shopManager?.OnStart();

        playerController?.OnStart();

        canvasManager?.OnStart();

        ChangeGameState(Game_State.Training);
    }

    private void Update()
    {
        UpdateFillImage();

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
    
    private float timer;
    private bool isRunning = true;

    private void StartingSetupForSplashScreen()
    {
        if (fillImage != null)
            fillImage.fillAmount = 0f;

        timer = 0f;
        isRunning = true;
    }

    private void UpdateFillImage()
    {
        if (!isRunning) return;

        timer += Time.deltaTime;
        float progress = Mathf.Clamp01(timer / duration);

        if (fillImage != null)
            fillImage.fillAmount = progress;

        // Khi đầy thì tắt splash
        if (progress >= 1f)
        {
            isRunning = false;
            splashScreen.SetActive(false); // Tắt canvas splash
        }
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