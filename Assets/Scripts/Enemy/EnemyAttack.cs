using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAttack : MonoBehaviour, IDirectionProvider
{
    #region Dependencies
    private Enemy enemyStats;
    private EnemyMovement movementController;
    #endregion

    #region Attack Settings
    [Header("Attack Settings")]
    [SerializeField]
    private float patternUpdateInterval = 2.0f;

    [SerializeField]
    private float baseAttackProbability = 0.4f;

    [SerializeField]
    private float attackDamage = 20f;

    [SerializeField]
    private float attackDelay = 1.0f;
    #endregion

    #region Events
    [Header("Events")]
    public UnityEvent onAttackStart; // 攻擊開始時的事件（用於播放動畫等）
    public UnityEvent<float> onPlayerDamaged; // 玩家受傷時的事件，參數為傷害值
    public UnityEvent onAttackBlocked; // 攻擊被格擋時的事件
    public UnityEvent onWeakPointHit; // 弱點被擊中時的事件
    #endregion

    #region Private Fields
    private float nextPatternUpdateTime = 0f;
    private int currentAttackDirectionIndex = -1;
    private bool isAttacking = false;
    
    [SerializeField]
    private DirectionType[] directions = new DirectionType[8]; // 8個方向的狀態
    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        enemyStats = GetComponent<Enemy>();
        movementController = GetComponent<EnemyMovement>();
        if (enemyStats == null)
        {
            Debug.LogError("Enemy component missing on this GameObject.", this);
        }
    }

    private void Start()
    {
        InitializeAttackSystem();
    }

    private void Update()
    {
        UpdateAttackPatternTimer();
    }
    #endregion

    #region Initialization
    private void InitializeAttackSystem()
    {
        UpdateDirectionPattern();
    }
    #endregion

    #region Direction Management
    private void UpdateDirectionPattern()
    {
        ResetAttackState();
        ResetAllDirections();
        GenerateAttackPoint();
        GenerateWeakPoints();
    }

    private void ResetAllDirections()
    {
        for (int i = 0; i < directions.Length; i++)
        {
            directions[i] = DirectionType.None;
        }
    }

    private void ResetAttackState()
    {
        StopAllCoroutines();
        isAttacking = false;
        currentAttackDirectionIndex = -1;
    }

    private void GenerateAttackPoint()
    {
        float currentAttackProbability = CalculateAttackProbability();
        if (Random.value < currentAttackProbability)
        {
            int attackIndex = Random.Range(0, 8);
            directions[attackIndex] = DirectionType.AttackPoint;
            currentAttackDirectionIndex = attackIndex;
            StartCoroutine(AttackCoroutine(attackIndex));
        }
    }

    private float CalculateAttackProbability()
    {
        if (enemyStats == null) return baseAttackProbability; // Fallback if stats not found
        float probability = baseAttackProbability + (enemyStats.GetStrength() * 0.1f);
        return Mathf.Clamp01(probability);
    }

    private void GenerateWeakPoints()
    {
        int weakPointCount = CalculateWeakPointCount();
        for (int i = 0; i < weakPointCount; i++)
        {
            GenerateSingleWeakPoint();
        }
    }

    private int CalculateWeakPointCount()
    {
        if (enemyStats == null) return 1; // Fallback if stats not found
        float pointMultiplier = 1.0f / enemyStats.GetStrength();
        int weakPoints = Mathf.CeilToInt(3 * pointMultiplier);
        return Mathf.Clamp(weakPoints, 0, 3);
    }

    private void GenerateSingleWeakPoint()
    {
        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, 8);
        } while (directions[randomIndex] != DirectionType.None);
        directions[randomIndex] = DirectionType.WeakPoint;
    }
    #endregion

    #region Attack System
    private void UpdateAttackPatternTimer()
    {
        if (Time.time >= nextPatternUpdateTime)
        {
            UpdateDirectionPattern();
            nextPatternUpdateTime = Time.time + patternUpdateInterval;
        }
    }

    private IEnumerator AttackCoroutine(int attackIndex)
    {
        isAttacking = true;
        onAttackStart?.Invoke();

        movementController?.StopMovement();

        yield return new WaitForSeconds(attackDelay);

        if (isAttacking && directions[attackIndex] == DirectionType.AttackPoint)
        {
            DamagePlayer();
        }

        movementController?.ResumeMovement();
        ResetAttackState();
    }

    private void DamagePlayer()
    {
        onPlayerDamaged?.Invoke(attackDamage);
        Debug.Log($"Player takes {attackDamage} damage!");
    }
    #endregion

    #region IDirectionProvider Implementation
    public DirectionType GetDirectionType(int directionIndex)
    {
        if (directionIndex >= 0 && directionIndex < directions.Length)
        {
            return directions[directionIndex];
        }
        return DirectionType.None;
    }

    public DirectionType[] GetAllDirectionTypes()
    {
        return directions;
    }

    public bool InteractWithDirection(int directionIndex)
    {
        if (directionIndex < 0 || directionIndex >= directions.Length)
        {
            return false;
        }

        DirectionType type = directions[directionIndex];
        
        switch (type)
        {
            case DirectionType.AttackPoint:
                if (isAttacking && directionIndex == currentAttackDirectionIndex)
                {
                    HandleAttackBlocked();
                    return true;
                }
                return false;

            case DirectionType.WeakPoint:
                HandleWeakPointHit();
                return true;

            default:
                return false;
        }
    }
    #endregion

    #region Interaction Handlers
    private void HandleAttackBlocked()
    {
        movementController?.ResumeMovement(); // Ensure movement resumes if attack is interrupted
        ResetAttackState();
        onAttackBlocked?.Invoke();
        Debug.Log("Attack blocked by player!");
    }

    private void HandleWeakPointHit()
    {
        // Damage to the enemy is handled by the Enemy script
        enemyStats?.RegisterWeakPointHit(); 
        onWeakPointHit?.Invoke();
        Debug.Log("Enemy weak point hit!");
    }
    #endregion
} 