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
        OnResetAction();
        OnEnergyChange();

        // Setup Info day
        OnNextDay();
        OnUpdateUIMoney();

        // Setup Info Enemy
        OnUpdateUIEnemy();
        OnUpdateUIPlayer();
    }

    public void OnUpdate()
    {

    }
    #endregion

    #region Training UI

    [Header("Canvas For Training")]
    [SerializeField] private GameObject trainingPanel;
    [SerializeField] private Image fillEnergyBar;
    [SerializeField] private Button exitBtn; 

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

        exitBtn.onClick.RemoveAllListeners();
        exitBtn.onClick.AddListener(() =>
        {
            trainingPanel?.SetActive(false);
            GameManager.Instance.ChangeGameState(Game_State.Training);
            OnResetAction();
        });
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
    [SerializeField] private GameObject playerInfor;        // Ngày, thể lực, tiền, shop
    [SerializeField] private GameObject playerInforBattle;  // thanh máu, thanh lực 
    [SerializeField] private Button attackButton;
    [SerializeField] private Button counterButton;          // Ẩn khi training 
    [SerializeField] private Button blockButton;            // Ẩn khi training

    [Header("Enemy Stats")]
    [SerializeField] private TextMeshProUGUI enemyAttackTxt;
    [SerializeField] private TextMeshProUGUI enemyHealthTxt;
    [SerializeField] private Image enemyAvatarTmg;
    [SerializeField] private Button fightButton;

    #region Update UI 

    [SerializeField] private GameObject interactIconImg; 
    [SerializeField] private GameObject punchIconImg; 

    public void UpdateInfoEnemy()
    {
        EnemyRuntimeData enemyData = EnemyManager.Instance.EnemyController.RuntimeData;
        enemyAttackTxt.text = enemyData.EnemyData.Attack.ToString();
        enemyHealthTxt.text = enemyData.EnemyData.Health.ToString();
    }

    private void SetStateForCounterAndBlockButton(bool value)
    {
        playerInfor.SetActive(!value);
        interactIconImg.SetActive(!value);

        playerInforBattle.SetActive(value);
        counterButton.gameObject.SetActive(value);
        blockButton.gameObject.SetActive(value);
        punchIconImg.SetActive(value);
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

    #endregion

    #region Set position 

    [Header("Setting Position")]
    [SerializeField] private Vector3 playerBattlePositon;
    [SerializeField] private Vector3 playerTrainingPositon;
    [SerializeField] private Vector3 enemyBattlePositon;

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
    public void MoveEnemyToBattle()
    {
        EnemyManager.Instance.EnemyController.transform.position = enemyBattlePositon;
        EnemyManager.Instance.EnemyController.transform.rotation = Quaternion.Euler(0, 135, 0);
    }

    #endregion

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

    private void OnStartForMainButton()
    {
        attackButton.onClick.RemoveAllListeners();
        attackButton.onClick.AddListener(PlayerController.Instance.CameraForInteract.OnInteractButtonClicked);
    }

    #region UI Infor Player and Enemy In Battle

    [Header("Enemy Health, name, avatar")]
    [SerializeField] private Image enemyHealthImg;
    [SerializeField] private Image enemyAvaterImg;
    [SerializeField] private TextMeshProUGUI enemyName; 

    public void OnUpdateUIEnemy()
    {
        EnemyController enemyController = EnemyManager.Instance.EnemyController;
        EnemyHealth enemyHealth = EnemyManager.Instance.EnemyController.Health;

        enemyName.text = $"{enemyController.RuntimeData.EnemyData.Name}";
        enemyHealthImg.fillAmount = (float)enemyHealth.CurrentHealth / enemyHealth.MaxHealth;
    }

    public void OnEnemyHealthChange()
    {
        EnemyHealth enemyHealth = EnemyManager.Instance.EnemyController.Health;

        enemyHealthImg.fillAmount = (float)enemyHealth.CurrentHealth / enemyHealth.MaxHealth;
    }

    [Header("Player Health and Stamina")]
    [SerializeField] private Image playerHealthImg;
    [SerializeField] private Image playerStaminaImg;

    public void OnUpdateUIPlayer()
    {
        PlayerRunTimeDatas data = PlayerController.Instance.Data;
        PlayerHealth health = PlayerController.Instance.Health;

        playerHealthImg.fillAmount = health.CurrentHealth / health.MaxHealth;
        playerStaminaImg.fillAmount = data.CurrentStamina / data.DataRuntime.Stamina;
    }

    public void OnPlayerHealthChange()
    {
        PlayerHealth health = PlayerController.Instance.Health;
        playerHealthImg.fillAmount = health.CurrentHealth / health.MaxHealth;
    }
    
    public void OnPlayerStaminaChange()
    {
        PlayerRunTimeDatas data = PlayerController.Instance.Data;
        playerStaminaImg.fillAmount = data.CurrentStamina / data.DataRuntime.Stamina;
    }

    #endregion

    #endregion

    #region UI Player Info

    [Header("Information Day")]
    [SerializeField] private TextMeshProUGUI textDay;
    [Header("Infomation Energy")]
    [SerializeField] private TextMeshProUGUI textEnergy;
    [Header("Infomation Money")]
    [SerializeField] private TextMeshProUGUI textMoney; 

    public void OnNextDay()
    {
        textDay.text = $"DAY {DayManager.Instance.CurrentDay}";
    }

    public void OnEnergyChange()
    {
        textEnergy.text = playerController.Data.CurrentEnergy.ToString();
    }

    public void OnUpdateUIMoney()
    {
        textMoney.text = $"{WalletManager.Instance.DataRuntime.currentMoney}";
    }
    #endregion

    private void OnEnable()
    {
        DayManager.Instance.OnNextDay += OnNextDay;
        DayManager.Instance.OnNextDay += OnEnergyChange;

        WalletManager.Instance.OnDataChange += OnUpdateUIMoney;
    }

    private void OnDisable()
    {
        DayManager.Instance.OnNextDay -= OnNextDay;
        DayManager.Instance.OnNextDay -= OnEnergyChange;

        WalletManager.Instance.OnDataChange -= OnUpdateUIMoney;
    }
}
