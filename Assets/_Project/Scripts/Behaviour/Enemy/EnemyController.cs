using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Ref")]
    private Animator animator;
    private EnemyHealth health;
    private StateMachineEnemy stateMachine;
    private Transform playerTransform;

    [Header("For Attack")]
    [SerializeField, Range(1f, 10f)] private float detectedRange;

    #region Get Set

    public Animator Animator => animator;
    public EntityHealth Health => health;
    public StateMachineEnemy StateMachine => stateMachine;
    public Transform PlayerTransform => playerTransform;
    public float DetectedRange => detectedRange;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        health = GetComponent<EnemyHealth>();
        animator = GetComponent<Animator>();
        stateMachine = GetComponent<StateMachineEnemy>();
    }

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        health?.Init(this);
        stateMachine.ChangeState(new EnemyIdleState(this));
    }

    private void Update()
    {
        stateMachine?.OnUpdate();

        if (!health.IsDead)
            LookToPlayer();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectedRange);
    }

    #endregion

    private void LookToPlayer()
    {
        if (playerTransform == null) return;

        Vector3 direction = playerTransform.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * 5f
        );
    }
}
