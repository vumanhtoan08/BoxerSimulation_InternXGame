using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Ref")]
    private Animator animator;
    private EnemyHealth health;
    private StateMachineEnemy stateMachine;
    private Transform playerTransform;
    private EnemyRuntimeData runtimeData;

    [Header("For Attack")]
    [SerializeField, Range(1f, 10f)] private float detectedRange;
    [SerializeField] private Transform rightHand; 
    [SerializeField] private Transform leftHand;
    [SerializeField] private LayerMask layerPlayer; 

    #region Get Set

    public Animator Animator => animator;
    public EnemyHealth Health => health;
    public StateMachineEnemy StateMachine => stateMachine;
    public Transform PlayerTransform => playerTransform;
    public float DetectedRange => detectedRange;
    public EnemyRuntimeData RuntimeData => runtimeData;
    public Transform RightHand => rightHand;
    public Transform LeftHand => leftHand; 
    public LayerMask LayerPlayer => layerPlayer;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        health = GetComponent<EnemyHealth>();
        animator = GetComponent<Animator>();
        stateMachine = GetComponent<StateMachineEnemy>();
        runtimeData = GetComponent<EnemyRuntimeData>();
    }

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        runtimeData?.OnStart();
        
        stateMachine.ChangeState(new EnemyIdleState(this));
        health?.Init(this);
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

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(RightHand.position, 0.2f);
        Gizmos.DrawWireSphere(LeftHand.position, 0.2f);
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
