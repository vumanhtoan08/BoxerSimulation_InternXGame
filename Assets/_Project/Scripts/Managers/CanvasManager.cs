using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : Singleton<CanvasManager>
{
    private PlayerController playerController;

    public void OnStart()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        trainingPanel?.SetActive(false);
        canvasArena?.SetActive(false);
        OnResetAction();
    }

    public void OnUpdate()
    {
        
    }

    #region Training Logic

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
            DOVirtual.DelayedCall(fillDuration, () => {
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
    }

    public void MovePlayerToBattle()
    {
        playerController.CharacterController.enabled = false;
        playerController.transform.position = playerBattlePositon;
        playerController.transform.rotation = Quaternion.Euler(0, -45, 0);
        playerController.CharacterController.enabled = true;
    }

    #endregion
}
