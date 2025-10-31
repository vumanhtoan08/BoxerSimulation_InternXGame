using UnityEngine;
using UnityEngine.Rendering;

public class EnemyController : MonoBehaviour
{
    [Header("Ref")]
    private Animator animator;
    private EnemyHealth health;
    private StateMachineEnemy stateMachine;
    private Transform playerTransform;
    private EnemyRuntimeData runtimeData;
    private Rigidbody rb;

    [Header("For Attack")]
    [SerializeField, Range(1f, 10f)] private float detectedRange;
    [SerializeField] private Transform rightHand; 
    [SerializeField] private Transform leftHand;
    [SerializeField] private LayerMask layerPlayer;
    [SerializeField] private Collider dynamicCollider;

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

    public Collider DynamicCollider => dynamicCollider;

    public Rigidbody Rigidbody => rb;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        health = GetComponent<EnemyHealth>();
        animator = GetComponent<Animator>();
        stateMachine = GetComponent<StateMachineEnemy>();
        runtimeData = GetComponent<EnemyRuntimeData>();
        rb = GetComponent<Rigidbody>();
    }

    public void OnStart()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        runtimeData?.OnStart();
        
        stateMachine.ChangeState(new EnemyIdleState(this));
        health?.Init(this);
    }

    public void OnUpdate()
    {
        stateMachine?.OnUpdate();

        if (!health.IsDead)
            LookToPlayer();
    }
    [SerializeField, Range(0, 10)] private float leftHandRadius; 

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectedRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(RightHand.position, 0.3f);
        Gizmos.DrawWireSphere(LeftHand.position, leftHandRadius);
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
