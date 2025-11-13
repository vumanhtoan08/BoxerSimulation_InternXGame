using UnityEngine;

public class StateMachineEnemy : MonoBehaviour
{
    private IState currentState;
    public IState CurrentState => currentState;
    public string CurrentStateString;

    private EnemyRuntimeData data;

    // ⏱️ Bộ đếm Taunt
    private float tauntTimer = 0f;
    private float tauntInterval = 15f;

    public void ChangeState(IState newState)
    {
        if (currentState != null && newState == currentState)
            return;

        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    public void OnUpdate()
    {
        if (CurrentStateString != currentState?.ToString())
            CurrentStateString = currentState?.ToString();

        currentState?.Excute();

        // 🧠 Kiểm tra độ khó & tăng bộ đếm
        data = EnemyManager.Instance.EnemyController.RuntimeData;
        if (data.EnemyData.Difficult == Enemy_Difficult.Hard && EnemyManager.Instance.EnemyController.Health.IsAuraActive)
            HandleTauntTimer();
    }

    private void HandleTauntTimer()
    {
        // Nếu đang ở trạng thái Taunt thì không đếm
        if (currentState is EnemyTauntState) return;

        // Nếu Enemy chết thì không đếm (bảo vệ lỗi)
        if (EnemyManager.Instance.EnemyController.Health.IsDead) return;

        tauntTimer += Time.deltaTime;

        if (tauntTimer >= tauntInterval)
        {
            tauntTimer = 0f;
            Debug.Log("⏱️ 15s trôi qua - Enemy chuyển sang Taunt!");
            ChangeState(new EnemyTauntState(EnemyManager.Instance.EnemyController));
        }
    }

    public void ResetTauntTimer()
    {
        tauntTimer = 0f;
    }

    public void OnPlayerPunch()
    {
        data = EnemyManager.Instance.EnemyController.RuntimeData;

        switch (data.EnemyData.Difficult)
        {
            case Enemy_Difficult.Easy:
                break;

            case Enemy_Difficult.Med:
                TryBlock(20f);
                break;

            case Enemy_Difficult.Hard:
                TryBlock(40f);
                break;
        }
    }

    private void TryBlock(float blockChancePercent)
    {
        float rand = Random.Range(0f, 100f);
        if (rand <= blockChancePercent)
        {
            Debug.Log($"Enemy triggered Block! (Chance: {blockChancePercent}%, Rolled: {rand:F1})");
            ChangeState(new EnemyBlockState(EnemyManager.Instance.EnemyController));
            ResetTauntTimer(); // reset luôn khi block thành công
        }
        else
        {
            Debug.Log($"Enemy failed to Block. (Chance: {blockChancePercent}%, Rolled: {rand:F1})");
        }
    }

    public void OnPlayerCounter()
    {
        // TODO: Logic cho phản ứng khi người chơi counter
    }
}
