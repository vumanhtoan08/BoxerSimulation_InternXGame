using UnityEngine;
using UnityEngine.UI;

public class StateMachinePlayer : MonoBehaviour 
{
    private IState currentState; 

    public IState CurrentState => currentState;

    [SerializeField] private string currentStateString; 

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

        if (currentState.ToString() != "PlayerBlockState" && !PlayerController.Instance.IsBlocking
             && PlayerController.Instance.Data.CheckStamina(PlayerController.Instance.Data.DataRuntime.BlockCost))
        {
            ChangeState(new PlayerBlockState(player));
        }
    }

    private void OnBlockEnd()
    {
        var player = PlayerController.Instance;
        player.SetBlock(false);
    }

    public void OnPunchAction() // gan vao attackbutton
    {
        if (currentState.ToString() != "PlayerPunchState" && !PlayerController.Instance.IsPunch 
            && PlayerController.Instance.Data.CheckStamina(PlayerController.Instance.Data.DataRuntime.PunchCost))
        {
            ChangeState(new PlayerPunchState(PlayerController.Instance));
        }
    }

    public void OnCounterAction() // gan vao counterbutton
    {
        if (currentState.ToString() != "PlayerCounterState" && !PlayerController.Instance.IsCounter 
            && PlayerController.Instance.Data.CheckStamina(PlayerController.Instance.Data.DataRuntime.CounterCost))
        {
            ChangeState(new PlayerCounterState(PlayerController.Instance));
        }
    }
}
