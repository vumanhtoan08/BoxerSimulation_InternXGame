using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Đang định nghĩa sai tác dụng của CanvasManager, đãng lẽ là ScreenManager
public class CanvasManager : Singleton<CanvasManager>
{
    private PlayerController playerController;

    #region Unity Methods
    public void OnStart()
    {
        OnStartForMainButton();

        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        trainingPanel?.SetActive(false);
        canvasArena?.SetActive(false);
        OnResetAction();
        OnEnergyChange();
    }

    public void OnUpdate()
    {

    }
    #endregion

    #region Training UI

    [Header("Canvas For Training")]
    [SerializeField] private GameObject trainingPanel;
    [SerializeField] private Image fillEnergyBar;

    [Header("Energy Settings")]
    [SerializeField] private float increaseAmount = 0.2f;
    [SerializeField] private float fillDuration = 0.5f;
    [SerializeField] private float cooldown = 0.75f;

    private float currentFill = 0f;
    private float lastTapTime = -999f;

    public event Action OnBoxingComplete;
    public event Action OnRuningComplete;
    public event Action OnSquatComplete;

    public void OnActiveTrainingPanel()
    {
        ResetEnergy();
        UpdateUI();
        trainingPanel?.SetActive(true);
    }

    public void OnUnActiveTrainingPanel() => trainingPanel?.SetActive(false);

    public void OnTraining()
    {
        if (Time.time - lastTapTime < cooldown)
            return;

        lastTapTime = Time.time;

        float target = Mathf.Clamp(currentFill + increaseAmount, 0f, 1f);
        DOTween.Kill(fillEnergyBar);

        DOTween.To(() => currentFill, x =>
        {
            currentFill = x;
            fillEnergyBar.fillAmount = currentFill;
        }, target, fillDuration)
        .SetEase(Ease.OutQuad)
        .SetTarget(fillEnergyBar);

        if (target >= 1f)
        {
            DOVirtual.DelayedCall(fillDuration, () =>
            {
                OnUnActiveTrainingPanel();

                switch (playerController.CameraForInteract.CurrentInteractable.Type)
                {
                    case TYPE_TRAINING.BOXING:
                        OnBoxingComplete?.Invoke();
                        break;
                    case TYPE_TRAINING.RUNING:
                        OnRuningComplete?.Invoke();
                        break;
                    case TYPE_TRAINING.SQUAT:
                        OnSquatComplete?.Invoke();
                        break;
                    default:
                        break;
                }

                OnResetAction();
            });
        }
    }

    private void ResetEnergy()
    {
        currentFill = 0f;
        fillEnergyBar.fillAmount = 0f;
        lastTapTime = -999f;
    }

    private void UpdateUI()
    {
        fillEnergyBar.fillAmount = currentFill;
    }

    #region Action

    private void OnResetAction()
    {
        OnBoxingComplete = null;
        OnRuningComplete = null;
        OnSquatComplete = null;
    }

    #endregion

    #endregion

    #region Arena Infor

    [Header("Canvas For Arena")]
    [SerializeField] private GameObject canvasArena;         // Canvas hiển thị thông tin.
    [SerializeField] private GameObject canvasBehaviour;     // Nút di chuyển của người chơi,...
    [SerializeField] private Button attackButton;
    [SerializeField] private Button counterButton;          // Ẩn khi training 
    [SerializeField] private Button blockButton;            // Ẩn khi training

    [Header("Enemy Stats")]
    [SerializeField] private TextMeshProUGUI enemyAttack;
    [SerializeField] private TextMeshProUGUI enemyHealth;
    [SerializeField] private Image enemyAvatar;
    [SerializeField] private Button fightButton;

    public void OnActiveEnemyInfoPanel()
    {
        UpdateInfoEnemy();
        canvasBehaviour.SetActive(false);
        canvasArena.SetActive(true);
    }

    public void OnUnActiveEnemyInfoPanel()
    {
        canvasArena.SetActive(false);
        canvasBehaviour.SetActive(true);
    }

    public void UpdateInfoEnemy()
    {
        EnemyRuntimeData enemyData = EnemyManager.Instance.EnemyController.RuntimeData;
        enemyAttack.text = enemyData.EnemyData.Attack.ToString();
        enemyHealth.text = enemyData.EnemyData.Health.ToString();
    }

    [Header("Setting Position")]
    [SerializeField] private Vector3 playerBattlePositon;
    [SerializeField] private Vector3 playerTrainingPositon;
    [SerializeField] private Vector3 enemyBattlePositon;

    public void ButtonFightActive()
    {
        Debug.Log("Kich hoat");
        MovePlayerToBattle();

        OnUnActiveEnemyInfoPanel();

        GameManager.Instance.ChangeGameState(Game_State.Battle);
    }

    public void MovePlayerToBattle()
    {
        playerController.CharacterController.enabled = false;
        playerController.transform.position = playerBattlePositon;
        playerController.transform.rotation = Quaternion.Euler(0, -45, 0);
        playerController.CharacterController.enabled = true;
    }

    public void MovePlayerToTraining()
    {
        playerController.CharacterController.enabled = false;
        playerController.transform.position = playerTrainingPositon;
        playerController.transform.rotation = Quaternion.Euler(0, 180, 0);
        playerController.CharacterController.enabled = true;
    }

    // Hiển thị nút, thay đổi icon
    public void UpdateActionButtonsByGameState(Game_State state)
    {
        switch (state)
        {
            case Game_State.Init:
                break;
            case Game_State.Pause:
                break;
            case Game_State.Training:
                SetStateForCounterAndBlockButton(false);
                break;
            case Game_State.OnTraning:
                SetStateForCounterAndBlockButton(false);
                break;
            case Game_State.Battle:
                SetStateForCounterAndBlockButton(true);
                break;
            case Game_State.Win:
                break;
            case Game_State.Lose:
                break;
        }

        ChangeAbilityButtonInteract(state);
    }

    // Thay đổi chức năng của button
    public void ChangeAbilityButtonInteract(Game_State state)
    {
        switch (state)
        {
            case Game_State.Init:
                break;
            case Game_State.Pause:
                break;
            case Game_State.Training:
                attackButton.onClick.RemoveAllListeners();
                attackButton.onClick.AddListener(playerController.CameraForInteract.OnInteractButtonClicked);
                break;
            case Game_State.OnTraning:
                attackButton.onClick.RemoveAllListeners();
                attackButton.onClick.AddListener(OnTraining);
                break;
            case Game_State.Battle:
                attackButton.onClick.RemoveAllListeners();
                attackButton.onClick.AddListener(playerController.StateMachine.OnPunchAction);

                counterButton.onClick.RemoveAllListeners();
                counterButton.onClick.AddListener(playerController.StateMachine.OnCounterAction);
                break;
            case Game_State.Win:
                break;
            case Game_State.Lose:
                break;
        }
    }

    private void SetStateForCounterAndBlockButton(bool value)
    {
        counterButton.gameObject.SetActive(value);
        blockButton.gameObject.SetActive(value);
    }

    private void OnStartForMainButton()
    {
        attackButton.onClick.RemoveAllListeners();
        attackButton.onClick.AddListener(PlayerController.Instance.CameraForInteract.OnInteractButtonClicked);
    }

    #endregion

    #region UI Info

    [Header("Information Day")]
    [SerializeField] private TextMeshProUGUI textDay;
    [Header("Infomation Energy")]
    [SerializeField] private TextMeshProUGUI textEnergy;

    public void OnNextDay()
    {
        textDay.text = $"DAY {DayManager.Instance.CurrentDay}";
    }

    public void OnEnergyChange()
    {
        textEnergy.text = playerController.Data.CurrentEnergy.ToString();
    }
    #endregion

    private void OnEnable()
    {
        DayManager.Instance.OnNextDay += OnNextDay;
        DayManager.Instance.OnNextDay += OnEnergyChange;
    }

    private void OnDisable()
    {
        DayManager.Instance.OnNextDay -= OnNextDay;
        DayManager.Instance.OnNextDay -= OnEnergyChange;
    }
}
