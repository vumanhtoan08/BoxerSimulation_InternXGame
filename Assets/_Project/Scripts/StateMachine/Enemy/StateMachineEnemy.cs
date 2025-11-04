using UnityEngine;

public class StateMachineEnemy : MonoBehaviour
{
    private IState currentState;
    public IState CurrentState => currentState;
    public string CurrentStateString;

    private EnemyRuntimeData data;

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
        {
            CurrentStateString = currentState?.ToString();
        }

        currentState?.Excute();
    }

    public void OnPlayerPunch()
    {
        data = EnemyManager.Instance.EnemyController.RuntimeData;

        switch (data.EnemyData.Difficult)
        {
            case Enemy_Difficult.Easy:
                // Không block
                break;

            case Enemy_Difficult.Med:
                TryBlock(30f); // ví dụ: 30% cơ hội né đòn
                break;

            case Enemy_Difficult.Hard:
                TryBlock(60f); // 60% cơ hội block trong chế độ khó
                break;
        }
    }

    /// <summary>
    /// Hàm thử né đòn dựa theo tỷ lệ phần trăm
    /// </summary>
    private void TryBlock(float blockChancePercent)
    {
        float rand = Random.Range(0f, 100f);
        if (rand <= blockChancePercent)
        {
            Debug.Log($"Enemy triggered Block! (Chance: {blockChancePercent}%, Rolled: {rand:F1})");
            ChangeState(new EnemyBlockState(EnemyManager.Instance.EnemyController));
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
