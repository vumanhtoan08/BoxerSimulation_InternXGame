using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("Parameters")]
    private Game_State gameState;
    public Game_State GameState => gameState;

    private void Start()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;

        //
        StartState();

        dataManager?.OnStart();
        soundManager?.OnStart();
        dayManager?.OnStart();
        canvasManager?.OnStart();
        interactableManager?.OnStart();
        enemyManager?.OnStart();
        popupManager?.OnStart();
        walletManager?.OnStart();
        shopManager?.OnStart();

        ChangeGameState(Game_State.Training);
    }

    private void Update()
    {
        dataManager?.OnUpdate();
        soundManager?.OnUpdate();
        dayManager?.OnUpdate();
        canvasManager?.OnUpdate();
        interactableManager?.OnUpdate();

        if (gameState == Game_State.Battle)
            enemyManager?.OnUpdate();

        popupManager?.OnUpdate();
        walletManager?.OnUpdate();
        shopManager?.OnUpdate();
    }

    #region StateGame Manager

    private void StartState()
    {
        gameState = Game_State.Training;
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
            case Game_State.Battle:
                PlayerController.Instance.StateMachine.ChangeState(new PlayerBattleState(PlayerController.Instance));
                break;
            case Game_State.Win:
                break;
            case Game_State.Lose:
                break;
        }

        CanvasManager.Instance.UpdateActionButtonsByGameState(gameState);
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