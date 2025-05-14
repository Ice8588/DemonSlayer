using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 敵人的主要行為控制器
/// 處理生命值、攻擊模式和方向系統
/// </summary>
public class Enemy : MonoBehaviour, IDirectionProvider
{
    #region Components
    [Header("Components")]
    public Rigidbody rigidbody;
    private EnemyMovement movement;
    #endregion

    #region Stats Settings
    [Header("Stats Settings")]
    [SerializeField]
    private float strength = 1.0f;

    [SerializeField]
    private float maxHealth = 100f;
    
    [SerializeField]
    private float currentHealth;

    [SerializeField]
    private float weakPointDamage = 20f;
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
    public UnityEvent onDeath;
    public UnityEvent<float> onHealthChanged;
    public UnityEvent onWeakPointHit;
    public UnityEvent onAttackBlocked;
    public UnityEvent onAttackStart;
    public UnityEvent<float> onPlayerDamaged;
    #endregion

    #region Private Fields
    private float nextUpdateTime = 0f;
    private int currentAttackIndex = -1;
    private bool isAttacking = false;
    
    [SerializeField]
    private DirectionType[] directions = new DirectionType[8];
    #endregion

    #region Unity Lifecycle
    private void Start()
    {
        InitializeEnemy();
    }

    private void Update()
    {
        UpdateAttackPattern();
    }
    #endregion

    #region Initialization
    private void InitializeEnemy()
    {
        currentHealth = maxHealth;
        movement = GetComponent<EnemyMovement>();
        if (movement == null)
        {
            movement = gameObject.AddComponent<EnemyMovement>();
        }
        UpdateDirections();
    }
    #endregion

    #region Direction Management
    private void UpdateDirections()
    {
        ResetAttackState();
        ResetDirections();
        GenerateAttackPoint();
        GenerateWeakPoints();
    }

    private void ResetDirections()
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
        currentAttackIndex = -1;
    }

    private void GenerateAttackPoint()
    {
        float currentAttackProbability = CalculateAttackProbability();
        if (Random.value < currentAttackProbability)
        {
            int attackIndex = Random.Range(0, 8);
            directions[attackIndex] = DirectionType.AttackPoint;
            currentAttackIndex = attackIndex;
            StartCoroutine(AttackCoroutine(attackIndex));
        }
    }

    private float CalculateAttackProbability()
    {
        float probability = baseAttackProbability + (strength * 0.1f);
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
        float pointMultiplier = 1.0f / strength;
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
    private void UpdateAttackPattern()
    {
        if (Time.time >= nextUpdateTime)
        {
            UpdateDirections();
            nextUpdateTime = Time.time + patternUpdateInterval;
        }
    }

    private IEnumerator AttackCoroutine(int attackIndex)
    {
        isAttacking = true;
        onAttackStart?.Invoke();

        // 攻擊時暫時停止移動
        movement.StopMovement();

        yield return new WaitForSeconds(attackDelay);

        if (isAttacking && directions[attackIndex] == DirectionType.AttackPoint)
        {
            DamagePlayer();
        }

        // 攻擊結束後恢復移動
        movement.ResumeMovement();
        ResetAttackState();
    }

    private void DamagePlayer()
    {
        onPlayerDamaged?.Invoke(attackDamage);
        Debug.Log($"Player takes {attackDamage} damage!");
    }
    #endregion

    #region Health System
    private void TakeDamage(float damage)
    {
        currentHealth = Mathf.Max(0, currentHealth - damage);
        float healthPercentage = currentHealth / maxHealth;
        onHealthChanged?.Invoke(healthPercentage);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        movement.StopMovement();
        onDeath?.Invoke();
        Destroy(gameObject);
    }
    #endregion

    #region Public Interface
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
                if (isAttacking && directionIndex == currentAttackIndex)
                {
                    OnAttackBlocked();
                    return true;
                }
                return false;

            case DirectionType.WeakPoint:
                OnWeakPointHit();
                return true;

            default:
                return false;
        }
    }

    private void OnAttackBlocked()
    {
        ResetAttackState();
        onAttackBlocked?.Invoke();
        Debug.Log("Attack blocked!");
    }

    private void OnWeakPointHit()
    {
        TakeDamage(weakPointDamage);
        onWeakPointHit?.Invoke();
        Debug.Log($"Weak point hit! Damage: {weakPointDamage}");
    }

    public float GetHealthPercentage()
    {
        return currentHealth / maxHealth;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
    #endregion
}
