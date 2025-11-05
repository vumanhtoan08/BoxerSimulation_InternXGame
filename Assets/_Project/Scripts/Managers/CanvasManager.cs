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

    public void OnUpdate() { }
    #endregion

    #region Training UI

    [Header("Canvas For Training")]
    [SerializeField] private GameObject trainingPanel;
    [SerializeField] private Image fillEnergyBar;
    [SerializeField] private Button exitBtn;

    [Header("Energy Settings")]
    [SerializeField] private float startFill = 0.5f;
    [SerializeField] private float increaseAmount = 0.3f;
    [SerializeField] private float fillDuration = 0.5f;
    [SerializeField] private float cooldown = 0.5f;
    [SerializeField] private float decayPerSecond = 0.25f;

    private float currentFill = 0f;
    private float lastTapTime = -999f;
    private Coroutine decayCo;

    public event Action OnBoxingComplete;
    public event Action OnRuningComplete;
    public event Action OnSquatComplete;

    public void OnActiveTrainingPanel()
    {
        ResetEnergyToStart();
        UpdateUI();
        trainingPanel?.SetActive(true);

        if (decayCo != null) StopCoroutine(decayCo);
        decayCo = StartCoroutine(CoDecay());

        exitBtn.onClick.RemoveAllListeners();
        exitBtn.onClick.AddListener(() =>
        {
            AnimateButton(exitBtn.transform); // 🔹 Tween
            OnUnActiveTrainingPanel();
            GameManager.Instance.ChangeGameState(Game_State.Training);
            PlayerController.Instance.StateMachine.SetActiveForWeight(false);
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
        DOTween.Kill(fillEnergyBar);
    }

    public void OnTraining()
    {
        if (Time.time - lastTapTime < cooldown) return;
        lastTapTime = Time.time;

        TriggerTrainingAnim();

        float target = Mathf.Clamp01(currentFill + increaseAmount);
        DOTween.Kill(fillEnergyBar);

        fillEnergyBar
            .DOFillAmount(target, fillDuration)
            .SetEase(Ease.OutQuad)
            .OnUpdate(() => currentFill = fillEnergyBar.fillAmount)
            .SetTarget(fillEnergyBar);

        if (target >= 1f)
        {
            DOVirtual.DelayedCall(fillDuration, () =>
            {
                OnUnActiveTrainingPanel();
                switch (playerController.CameraForInteract.CurrentInteractable.Type)
                {
                    case TYPE_TRAINING.BOXING: OnBoxingComplete?.Invoke(); break;
                    case TYPE_TRAINING.RUNING: OnRuningComplete?.Invoke(); break;
                    case TYPE_TRAINING.SQUAT: OnSquatComplete?.Invoke(); break;
                }
                OnResetAction();
            });
        }
    }

    private int currentSquatBlend = 0;

    private void TriggerTrainingAnim()
    {
        switch (playerController.CameraForInteract.CurrentInteractable.Type)
        {
            case TYPE_TRAINING.BOXING:
                playerController.Animator.SetTrigger("isPunch");
                break;
            case TYPE_TRAINING.RUNING:
                playerController.Animator.SetBool("isRunning", true);
                break;
            case TYPE_TRAINING.SQUAT:
                PlayerController.Instance.StateMachine.SetActiveForWeight(true);
                currentSquatBlend = 1 - currentSquatBlend; // 🔁 Đảo giữa 0 và 1 mỗi lần gọi
                playerController.Animator.SetFloat("squatValue", currentSquatBlend);
                playerController.Animator.SetTrigger("isSquat");
                break;
        }
    }

    private IEnumerator CoDecay()
    {
        while (trainingPanel != null && trainingPanel.activeSelf)
        {
            currentFill = Mathf.Max(0f, currentFill - decayPerSecond * Time.deltaTime);
            fillEnergyBar.fillAmount = currentFill;
            yield return null;
        }
    }

    private void ResetEnergyToStart()
    {
        currentFill = Mathf.Clamp01(startFill);
        fillEnergyBar.fillAmount = currentFill;
        lastTapTime = -999f;
    }

    private void UpdateUI() => fillEnergyBar.fillAmount = currentFill;

    #region Action

    private void OnResetAction()
    {
        OnBoxingComplete = null;
        OnRuningComplete = null;
        OnSquatComplete = null;
    }

    #endregion

    #endregion

    #region Arena Info

    [Header("Canvas For Arena")]
    [SerializeField] private GameObject playerInfor;
    [SerializeField] private GameObject playerInforBattle;
    [SerializeField] private Button attackButton;
    [SerializeField] private Button counterButton;
    [SerializeField] private Button blockButton;

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

    public void UpdateActionButtonsByGameState(Game_State state)
    {
        switch (state)
        {
            case Game_State.Training:
            case Game_State.OnTraning:
                SetStateForCounterAndBlockButton(false);
                break;
            case Game_State.Battle:
                SetStateForCounterAndBlockButton(true);
                break;
        }

        ChangeAbilityButtonInteract(state);
    }

    #region Set position

    [Header("Setting Position")]
    [SerializeField] private Vector3 playerBattlePositon;
    [SerializeField] private Vector3 playerTrainingPositon;
    [SerializeField] private Vector3 enemyBattlePositon;

    public void MovePlayerToBattle()
    {
        playerController.CameraLook.SetCameraRotation(Vector3.zero);
        playerController.CharacterController.enabled = false;
        playerController.transform.position = playerBattlePositon;
        playerController.transform.rotation = Quaternion.Euler(0, -45, 0);
        playerController.CharacterController.enabled = true;
    }

    public void MovePlayerToTraining()
    {
        playerController.CameraLook.SetCameraRotation(Vector3.zero);
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

    public void ChangeAbilityButtonInteract(Game_State state)
    {
        attackButton.onClick.RemoveAllListeners();
        counterButton.onClick.RemoveAllListeners();
        blockButton.onClick.RemoveAllListeners();

        switch (state)
        {
            case Game_State.Training:
                attackButton.onClick.AddListener(() =>
                {
                    AnimateButton(attackButton.transform); // 🔹 Tween
                    playerController.CameraForInteract.OnInteractButtonClicked();
                });
                break;
            case Game_State.OnTraning:
                attackButton.onClick.AddListener(() =>
                {
                    AnimateButton(attackButton.transform); // 🔹 Tween
                    OnTraining();
                });
                break;
            case Game_State.Battle:
                attackButton.onClick.AddListener(() =>
                {
                    AnimateButton(attackButton.transform);
                    playerController.StateMachine.OnPunchAction();
                });
                counterButton.onClick.AddListener(() =>
                {
                    AnimateButton(counterButton.transform);
                    playerController.StateMachine.OnCounterAction();
                });
                blockButton.onClick.AddListener(() =>
                {
                    AnimateButton(blockButton.transform);
                });
                break;
        }
    }

    private void OnStartForMainButton()
    {
        attackButton.onClick.RemoveAllListeners();
        attackButton.onClick.AddListener(() =>
        {
            AnimateButton(attackButton.transform);
            PlayerController.Instance.CameraForInteract.OnInteractButtonClicked();
        });
    }

    #endregion

    #region UI Infor Player and Enemy In Battle

    [Header("Enemy Health, name, avatar")]
    [SerializeField] private Image enemyHealthImg;
    [SerializeField] private Image enemyAvaterImg;
    [SerializeField] private Text enemyName;
    [SerializeField] private Text enemyBattleHealth;

    public void SetupUIBeforeBattle()
    {
        EnemyRuntimeData enemyRuntimeData = EnemyManager.Instance.EnemyController.RuntimeData;
        EnemyHealth enemyHealth = EnemyManager.Instance.EnemyController.Health;

        enemyAvaterImg.sprite = enemyRuntimeData.EnemyData.Avatar;
        enemyName.text = enemyRuntimeData.EnemyData.Name;
        enemyBattleHealth.text = $"{enemyHealth.CurrentHealth} / {enemyHealth.MaxHealth}";
        enemyHealthImg.fillAmount = (float)enemyHealth.CurrentHealth / enemyHealth.MaxHealth;

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
        enemyBattleHealth.text = $"{(int)enemyHealth.CurrentHealth} / {enemyHealth.MaxHealth}";
    }

    [Header("Player Health and Stamina")]
    [SerializeField] private Image playerHealthImg;
    [SerializeField] private Image playerStaminaImg;
    [SerializeField] private Text playerBattleHealth;

    public void OnUpdateUIPlayer()
    {
        PlayerRunTimeDatas data = PlayerController.Instance.Data;
        PlayerHealth health = PlayerController.Instance.Health;

        playerHealthImg.fillAmount = health.CurrentHealth / health.MaxHealth;
        playerStaminaImg.fillAmount = data.CurrentStamina / data.DataRuntime.Stamina;
        playerBattleHealth.text = $"{(int)health.CurrentHealth} / {health.MaxHealth}";
    }

    public void OnPlayerHealthChange()
    {
        PlayerHealth health = PlayerController.Instance.Health;
        playerHealthImg.fillAmount = health.CurrentHealth / health.MaxHealth;
        playerBattleHealth.text = $"{(int)health.CurrentHealth} / {health.MaxHealth}";
    }

    public void OnPlayerStaminaChange()
    {
        PlayerRunTimeDatas data = PlayerController.Instance.Data;
        playerStaminaImg.fillAmount = data.CurrentStamina / data.DataRuntime.Stamina;
    }

    #endregion

    #region UI Player Info

    [Header("Information Day")]
    [SerializeField] private Text textDay;
    [Header("Infomation Energy")]
    [SerializeField] private Text textEnergy;
    [Header("Infomation Money")]
    [SerializeField] private Text textMoney;
    [SerializeField] private Text textMoneyPopupShop;

    public void OnNextDay() => textDay.text = $"DAY {DayManager.Instance.CurrentDay}";
    public void OnEnergyChange() => textEnergy.text = playerController.Data.CurrentEnergy.ToString();

    public void OnUpdateUIMoney()
    {
        textMoney.text = $"{WalletManager.Instance.DataRuntime.currentMoney}";
        textMoneyPopupShop.text = $"{WalletManager.Instance.DataRuntime.currentMoney}";
    }

    #endregion

    #region Dark Canvas

    [SerializeField] private GameObject darkCanvasObj;
    [SerializeField] private Image darkCanvasImg;
    [SerializeField] private Text dayTxt;

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
        return darkCanvasImg.DOFade(0f, 1f).SetEase(Ease.Linear)
            .OnComplete(() => darkCanvasObj.SetActive(false));
    }

    public Tween ShowDayText()
    {
        dayTxt.text = $"DAY {DayManager.Instance.CurrentDay}";
        Color c = dayTxt.color; c.a = 0f; dayTxt.color = c;
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

    #region Tween Helper

    /// <summary>
    /// 🔹 Làm hiệu ứng scale 1 → 1.1 → 1 trong 0.05s
    /// </summary>
    private void AnimateButton(Transform target)
    {
        if (target == null) return;
        target.DOKill();
        target.localScale = Vector3.one;
        target.DOScale(1.1f, 0.05f)
              .SetEase(Ease.OutQuad)
              .OnComplete(() => target.DOScale(1f, 0.05f).SetEase(Ease.InQuad));
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
