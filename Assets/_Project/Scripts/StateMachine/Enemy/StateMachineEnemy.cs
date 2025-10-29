using UnityEngine;
using UnityEngine.SceneManagement;

public class StateMachineEnemy : MonoBehaviour
{
    private IState currentState;

    public IState CurrentState => currentState;

    public string CurrentStateString; 

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
        if (CurrentStateString != currentState.ToString())
        {
            CurrentStateString = currentState.ToString();
        }

        currentState?.Excute();
    }
}