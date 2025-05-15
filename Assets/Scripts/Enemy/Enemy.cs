using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 敵人的主要協調器，管理生命值並與其他組件（如移動和攻擊）交互。
/// </summary>
public class Enemy : MonoBehaviour
{
    #region Components
    [Header("Components")]
    public Rigidbody rigidbody;
    private EnemyMovement movementController;
    private EnemyAttack attackController;
    // IDirectionProvider 現在由 EnemyAttack 實現
    #endregion

    #region Stats Settings
    [Header("Stats Settings")]
    [SerializeField]
    private float strength = 1.0f; // 敵人強度，影響攻擊模式的複雜度

    [SerializeField]
    private float maxHealth = 100f;
    
    [SerializeField]
    private float currentHealth;

    [SerializeField]
    private float weakPointReceivedDamage = 20f; // 弱點被擊中時受到的傷害
    #endregion

    #region Events
    [Header("Events")]
    public UnityEvent onDeath;
    public UnityEvent<float> onHealthChanged; // 參數是健康百分比 (0-1)
    // 注意：攻擊相關的事件現在由 EnemyAttack 處理
    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        InitializeEnemy();
    }
    #endregion

    #region Initialization
    private void InitializeEnemy()
    {
        currentHealth = maxHealth;
        movementController = GetComponent<EnemyMovement>();
        if (movementController == null)
        {
            movementController = gameObject.AddComponent<EnemyMovement>();
        }

        attackController = GetComponent<EnemyAttack>();
        if (attackController == null)
        {
            attackController = gameObject.AddComponent<EnemyAttack>();
        }
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
        movementController?.StopMovement();
        // 如果攻擊控制器也需要停止某些行為，可以在此處調用
        // attackController?.StopAttack(); 
        onDeath?.Invoke();
        Destroy(gameObject);
    }
    #endregion

    #region Public Interface for Stats and State
    public float GetStrength()
    {
        return strength;
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

    /// <summary>
    /// 由 EnemyAttack 調用，當敵人的弱點被擊中時。
    /// </summary>
    public void RegisterWeakPointHit()
    {
        TakeDamage(weakPointReceivedDamage);
    }

    /// <summary>
    /// 外部系統（例如玩家的攻擊腳本）應調用此方法與敵人的方向系統交互。
    /// </summary>
    public bool InteractWithDirection(int directionIndex)
    {
        if (attackController != null)
        {
            return attackController.InteractWithDirection(directionIndex);
        }
        return false;
    }

    /// <summary>
    /// 獲取方向類型，主要供GUI或其他需要顯示敵人狀態的系統使用。
    /// </summary>
    public DirectionType GetDirectionType(int directionIndex)
    {
        if (attackController != null)
        {
            return attackController.GetDirectionType(directionIndex);
        }
        return DirectionType.None;
    }

    public DirectionType[] GetAllDirectionTypes()
    {
        if (attackController != null)
        {
            return attackController.GetAllDirectionTypes();
        }
        return new DirectionType[8]; // 返回空數組或默認值
    }
    #endregion
}
