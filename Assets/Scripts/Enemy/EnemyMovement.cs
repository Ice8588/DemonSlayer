using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    #region Components
    private NavMeshAgent agent;
    private Transform playerTransform;
    #endregion

    #region Movement Settings
    [Header("Movement Settings")]
    [SerializeField]
    private float moveSpeed = 3.5f;
    
    [SerializeField]
    private float rotationSpeed = 5f;

    [SerializeField]
    private float minDistanceToPlayer = 2f;

    [SerializeField]
    private float maxDistanceToPlayer = 10f;

    [SerializeField]
    private float updatePathInterval = 0.5f;

    [SerializeField]
    private float chaseSpeedMultiplier = 1.5f;

    [SerializeField]
    private float retreatSpeedMultiplier = 0.7f;
    #endregion

    #region State
    private bool isMovementEnabled = true;
    #endregion

    #region Unity Lifecycle
    private void Start()
    {
        InitializeMovement();
    }

    private void Update()
    {
        if (isMovementEnabled)
        {
            UpdateMovement();
        }
    }
    #endregion

    #region Initialization
    private void InitializeMovement()
    {
        // 獲取必要的組件
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component is missing!");
            return;
        }

        // 設置 NavMeshAgent 參數
        agent.speed = moveSpeed;
        agent.stoppingDistance = minDistanceToPlayer;
        agent.angularSpeed = rotationSpeed * 100f;

        // 尋找玩家
        FindPlayer();
    }

    private void FindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogWarning("Player not found! Make sure it has the 'Player' tag.");
        }
    }
    #endregion

    #region Movement System
    private void UpdateMovement()
    {
        if (playerTransform == null || agent == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // 根據與玩家的距離決定移動行為
        if (distanceToPlayer > maxDistanceToPlayer)
        {
            ChasePlayer();
        }
        else if (distanceToPlayer > minDistanceToPlayer)
        {
            MoveTowardsPlayer();
        }
        else
        {
            MoveAwayFromPlayer();
        }

        // 始終面向玩家
        RotateTowardsPlayer();
    }

    private void ChasePlayer()
    {
        agent.speed = moveSpeed * chaseSpeedMultiplier;
        SetDestination(playerTransform.position);
    }

    private void MoveTowardsPlayer()
    {
        agent.speed = moveSpeed;
        SetDestination(playerTransform.position);
    }

    private void MoveAwayFromPlayer()
    {
        Vector3 directionToPlayer = (transform.position - playerTransform.position).normalized;
        Vector3 targetPosition = transform.position + directionToPlayer * minDistanceToPlayer;
        
        agent.speed = moveSpeed * retreatSpeedMultiplier;
        SetDestination(targetPosition);
    }

    private void RotateTowardsPlayer()
    {
        if (playerTransform == null) return;

        Vector3 direction = (playerTransform.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }

    private void SetDestination(Vector3 target)
    {
        if (agent != null && agent.isActiveAndEnabled)
        {
            agent.SetDestination(target);
        }
    }
    #endregion

    #region Public Interface
    public void StopMovement()
    {
        isMovementEnabled = false;
        if (agent != null)
        {
            agent.isStopped = true;
        }
    }

    public void ResumeMovement()
    {
        isMovementEnabled = true;
        if (agent != null)
        {
            agent.isStopped = false;
        }
    }

    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
        if (agent != null)
        {
            agent.speed = speed;
        }
    }

    public void SetTarget(Transform target)
    {
        playerTransform = target;
    }

    public float GetDistanceToPlayer()
    {
        if (playerTransform == null) return float.MaxValue;
        return Vector3.Distance(transform.position, playerTransform.position);
    }
    #endregion
} 