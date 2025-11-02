using DG.Tweening;
using System;
using System.Collections;
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
    [SerializeField] private float startFill = 0.5f;       // bắt đầu ở 0.5
    [SerializeField] private float increaseAmount = 0.2f;  // mỗi lần bấm +0.2
    [SerializeField] private float fillDuration = 0.5f;    // thời gian tween khi bấm
    [SerializeField] private float cooldown = 0.75f;       // chống spam
    [SerializeField] private float decayPerSecond = 0.25f; // tốc độ tụt mỗi giây

    private float currentFill = 0f;
    private float lastTapTime = -999f;
    private Coroutine decayCo;

    public event Action OnBoxingComplete;
    public event Action OnRuningComplete;
    public event Action OnSquatComplete;

    public void OnActiveTrainingPanel()
    {
        ResetEnergyToStart();    // set 0.5
        UpdateUI();
        trainingPanel?.SetActive(true);

        // Bắt đầu decay theo thời gian
        if (decayCo != null) StopCoroutine(decayCo);
        decayCo = StartCoroutine(CoDecay());

        exitBtn.onClick.RemoveAllListeners();
        exitBtn.onClick.AddListener(() =>
        {
            OnUnActiveTrainingPanel();
            GameManager.Instance.ChangeGameState(Game_State.Training);
            OnResetAction();
        });
    }

    public void OnUnActiveTrainingPanel()
    {
        trainingPanel?.SetActive(false);
        if (decayCo != null)
        {
            StopCoroutine(decayCo);
            decayCo = null;
        }
        // Hủy mọi tween còn gắn lên thanh năng lượng để không leak
        DOTween.Kill(fillEnergyBar);
    }

    public void OnTraining()
    {
        if (Time.time - lastTapTime < cooldown) return;
        lastTapTime = Time.time;

        // Tính mục tiêu mới sau khi bấm
        float target = Mathf.Clamp01(currentFill + increaseAmount);

        // Hủy tween cũ trên cùng target để tránh xung đột
        DOTween.Kill(fillEnergyBar);

        // Tween UI lên target; currentFill sẽ được cập nhật trong OnUpdate của tween
        fillEnergyBar
            .DOFillAmount(target, fillDuration)
            .SetEase(Ease.OutQuad)
            .OnUpdate(() => currentFill = fillEnergyBar.fillAmount)
            .SetTarget(fillEnergyBar);

        // Nếu đạt 1 thì hoàn tất sau khi tween kết thúc
        if (target >= 1f)
        {
            DOVirtual.DelayedCall(fillDuration, () =>
            {
                // kết thúc: tắt panel, dừng decay
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
                }

                OnResetAction();
            });
        }
    }

    private IEnumerator CoDecay()
    {
        // Decay liên tục mỗi frame khi panel đang bật
        while (trainingPanel != null && trainingPanel.activeSelf)
        {
            // Trừ dần theo thời gian (giới hạn [0,1])
            currentFill = Mathf.Max(0f, currentFill - decayPerSecond * Time.deltaTime);
            fillEnergyBar.fillAmount = currentFill;
            yield return null;
        }
    }

    // == Helpers ==
    private void ResetEnergyToStart()
    {
        currentFill = Mathf.Clamp01(startFill); // 0.5
        fillEnergyBar.fillAmount = currentFill;
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


    #region Update UI 

    [Header("For button action")]
    [SerializeField] private GameObject interactIconImg; 
    [SerializeField] private GameObject punchIconImg; 

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
    [SerializeField] private TextMeshProUGUI enemyBattleHealth;

    public void SetupUIBeforeBattle()
    {
        // Enemy 
        EnemyRuntimeData enemyRuntimeData = EnemyManager.Instance.EnemyController.RuntimeData;
        EnemyHealth enemyHealth = EnemyManager.Instance.EnemyController.Health;

        enemyAvaterImg.sprite =  enemyRuntimeData.EnemyData.Avatar;
        enemyName.text = enemyRuntimeData.EnemyData.Name;
        enemyBattleHealth.text = $"{enemyHealth.CurrentHealth} / {enemyHealth.MaxHealth}";
        enemyHealthImg.fillAmount = (float)enemyHealth.CurrentHealth / enemyHealth.MaxHealth;

        // Player
        PlayerRunTimeDatas data = PlayerController.Instance.Data;
        PlayerHealth health = PlayerController.Instance.Health;

        playerHealthImg.fillAmount = health.CurrentHealth / health.MaxHealth;
        playerStaminaImg.fillAmount = data.CurrentStamina / data.DataRuntime.Stamina;
        playerBattleHealth.text = $"{health.CurrentHealth} / {health.MaxHealth}";
    }

    public void OnUpdateUIEnemy()
    {
        EnemyHealth enemyHealth = EnemyManager.Instance.EnemyController.Health;

        enemyHealthImg.fillAmount = (float)enemyHealth.CurrentHealth / enemyHealth.MaxHealth;
        enemyBattleHealth.text = $"{enemyHealth.CurrentHealth} / {enemyHealth.MaxHealth}";
    }

    [Header("Player Health and Stamina")]
    [SerializeField] private Image playerHealthImg;
    [SerializeField] private Image playerStaminaImg;
    [SerializeField] private TextMeshProUGUI playerBattleHealth;

    public void OnUpdateUIPlayer()
    {
        PlayerRunTimeDatas data = PlayerController.Instance.Data;
        PlayerHealth health = PlayerController.Instance.Health;

        playerHealthImg.fillAmount = health.CurrentHealth / health.MaxHealth;
        playerStaminaImg.fillAmount = data.CurrentStamina / data.DataRuntime.Stamina;
        playerBattleHealth.text = $"{health.CurrentHealth} / {health.MaxHealth}";
    }

    public void OnPlayerHealthChange()
    {
        PlayerHealth health = PlayerController.Instance.Health;
        playerHealthImg.fillAmount = health.CurrentHealth / health.MaxHealth;
        playerBattleHealth.text = $"{health.CurrentHealth} / {health.MaxHealth}";
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

    #region Dark Canvas 

    [SerializeField] private GameObject darkCanvasObj;
    [SerializeField] private Image darkCanvasImg;
    [SerializeField] private TextMeshProUGUI dayTxt; 

    public Tween DarkPanelActive()
    {
        darkCanvasObj.SetActive(true);

        Color color = darkCanvasImg.color;
        color.a = 0; 
        darkCanvasImg.color = color;

        return darkCanvasImg.DOFade(1f, 1f).SetEase(Ease.Linear);
    }

    public Tween DarkPanelUnActive()
    {
        return darkCanvasImg.DOFade(0f, 1f).SetEase(Ease.Linear).OnComplete(() => darkCanvasObj.SetActive(false));
    }

    public Tween ShowDayText()
    {
        dayTxt.text = $"DAY {DayManager.Instance.CurrentDay}";
        dayTxt.alpha = 0;
        dayTxt.transform.localScale = Vector3.zero;
        dayTxt.gameObject.SetActive(true);

        Sequence seq = DOTween.Sequence();
        seq.Append(dayTxt.DOFade(1f, 1f));
        seq.Join(dayTxt.transform.DOScale(1f, 0.4f).SetEase(Ease.OutBack));
        seq.AppendInterval(1.2f);
        seq.Append(dayTxt.DOFade(0f, 1f));
        seq.AppendCallback(() => dayTxt.gameObject.SetActive(false));

        return seq;
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
