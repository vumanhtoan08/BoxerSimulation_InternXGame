using UnityEngine;
using UnityEngine.UI;

public class StateMachinePlayer : MonoBehaviour
{
    private IState currentState;

    public IState CurrentState => currentState;

    [SerializeField] private string currentStateString;

    [SerializeField] private GameObject weight_L;
    [SerializeField] private GameObject weight_R;

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
        currentStateString = currentState.ToString();

        currentState?.Excute();
    }

    #region Button 

    [Header("Button")]
    [SerializeField] private HoldButton blockButton;

    #endregion

    public void OnStart()
    {
        blockButton.onHoldStart += OnBlockStart;
        blockButton.onHoldEnd += OnBlockEnd;
    }

    private void OnBlockStart()
    {
        var player = PlayerController.Instance;

        if (currentState.ToString() != "PlayerBlockState" && !PlayerController.Instance.IsBlocking)
        {
            ChangeState(new PlayerBlockState(player));
        }
    }

    private void OnBlockEnd()
    {
        var player = PlayerController.Instance;
        player.SetBlock(false);
    }

    public void OnPunchAction() // Gán vào AttackButton
    {
        // ✅ Kiểm tra điều kiện để player được phép đấm
        if (currentState.ToString() != "PlayerPunchState"
            && !PlayerController.Instance.IsPunch
            && PlayerController.Instance.Data.CheckStamina(PlayerController.Instance.Data.DataRuntime.PunchCost))
        {
            ChangeState(new PlayerPunchState(PlayerController.Instance));
        }

        // ✅ Lấy state hiện tại của Enemy
        var enemyState = EnemyManager.Instance.EnemyController.StateMachine.CurrentStateString;

        // ❌ Chỉ bỏ qua khi enemy đang chết hoặc đang bị hit
        if (enemyState == "EnemyHitState" || enemyState == "EnemyDeadState")
            return;

        // ✅ Cho phép gọi OnPlayerPunch() trong các state khác (Idle, Move, Counter, Punch, v.v.)
        EnemyManager.Instance.EnemyController.StateMachine.OnPlayerPunch();
    }

    public void OnCounterAction() // gan vao counterbutton
    {
        if (currentState.ToString() != "PlayerCounterState" && !PlayerController.Instance.IsCounter
            && PlayerController.Instance.Data.CheckStamina(PlayerController.Instance.Data.DataRuntime.CounterCost))
        {
            ChangeState(new PlayerCounterState(PlayerController.Instance));
        }
    }

    public void SetActiveForWeight(bool isActive)
    {
        weight_L.SetActive(isActive);
        weight_R.SetActive(isActive);
    }

    #region Sound

    public void PlayPunchSound()
    {
        if (GameManager.Instance.GameState == Game_State.OnTraning)
        {
            var randomPitch = Random.Range(0.6f, 1f);
            SoundManager.Instance.PlaySound(SoundKey.Punch, 0.7f, randomPitch);
        }
    }

    public void PlayBreathSound()
    {
        if (GameManager.Instance.GameState == Game_State.OnTraning)
        {
            var randomPitch = Random.Range(1f, 1.2f);
            SoundManager.Instance.PlaySound(SoundKey.Breath, 2f, randomPitch);
        }
    }

    public void PlayerMoveSound()
    {
        if (GameManager.Instance.GameState == Game_State.OnTraning)
        {
            var randomPitch = Random.Range(1f, 1.2f);
            SoundManager.Instance.PlaySound(SoundKey.FootStep, 1.2f, randomPitch);
        }
        else
        {
            var randomPitch = Random.Range(0.6f, 0.8f);
            SoundManager.Instance.PlaySound(SoundKey.FootStep, 0.7f, randomPitch);
        }
    }

    #endregion
}
