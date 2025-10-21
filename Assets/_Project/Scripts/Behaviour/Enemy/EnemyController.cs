using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Ref")]
    private Animator animator;
    private EnemyHealth health;
    private StateMachineEnemy stateMachine;

    #region Get Set

    public Animator Animator => animator;
    public EntityHealth Health => health;
    public StateMachineEnemy StateMachine => stateMachine;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        health = GetComponent<EnemyHealth>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        health?.Init(this);
    }

    #endregion
}
