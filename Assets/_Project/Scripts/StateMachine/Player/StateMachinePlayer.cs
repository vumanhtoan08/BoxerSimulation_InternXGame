using UnityEngine;

public class StateMachinePlayer : MonoBehaviour 
{
    [SerializeField] private IState currentState; 

    public IState CurrentState => currentState;

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
        currentState?.Excute();
    }
}
