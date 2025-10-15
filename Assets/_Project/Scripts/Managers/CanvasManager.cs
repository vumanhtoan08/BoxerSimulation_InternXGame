using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : Singleton<CanvasManager>
{
    [Header("Canvas For Training")]
    [SerializeField] private GameObject trainingPanel;
    [SerializeField] private Image fillEnergyBar;

    [Header("Energy Settings")]
    [SerializeField] private float increaseAmount = 0.2f;
    [SerializeField] private float fillDuration = 0.5f;
    [SerializeField] private float cooldown = 0.75f;

    private float currentFill = 0f;
    private float lastTapTime = -999f; // lưu thời điểm tương tác cuối

    public void OnStart()
    {
        trainingPanel?.SetActive(false);
    }

    public void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            OnUnActiveTrainingPanel();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            OnTraining();
        }
    }

    #region Training Logic

    public void OnActiveTrainingPanel()
    {
        ResetEnergy();
        UpdateUI();
        trainingPanel?.SetActive(true);
    }

    public void OnUnActiveTrainingPanel() => trainingPanel?.SetActive(false);

    public void OnTraining()
    {
        // Nếu chưa qua cooldown thì return
        if (Time.time - lastTapTime < cooldown)
            return;

        lastTapTime = Time.time; // cập nhật thời điểm tương tác

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
            DOVirtual.DelayedCall(fillDuration, () => OnUnActiveTrainingPanel());
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

    #endregion
}
