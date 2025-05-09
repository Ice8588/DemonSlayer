using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Transform weaponTransform;            // 武器物件（例如一把劍）
    public Collider weaponCollider;              // 需為 Trigger
    public Animator animator;                    // 播放下劈動畫
    public float rotationSpeed = 120f;           // 滑鼠旋轉速度
    public float attackDuration = 0.5f;          // 攻擊持續時間
    private float weaponAngle = 0f;              // 目前方向（0 ~ 360）
    private bool isAttacking = false;

    void Start()
    {
        weaponCollider.enabled = false;
    }

    void Update()
    {
        HandleRotation();

        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            Attack();
        }
    }


    void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X");
        weaponAngle += mouseX * rotationSpeed * Time.deltaTime;

        // 限制在 0 ~ 360 度之間
        if (weaponAngle >= 360f) weaponAngle -= 360f;
        if (weaponAngle < 0f) weaponAngle += 360f;

        transform.rotation = Quaternion.Euler(0f, weaponAngle, 0f); // 旋轉玩家 Y 軸
    }

    void Attack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");

        // 啟用碰撞器
        weaponCollider.enabled = true;

        // 計時後關閉碰撞與攻擊狀態
        Invoke(nameof(ResetAttack), attackDuration);
    }

    void ResetAttack()
    {
        weaponCollider.enabled = false;
        isAttacking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isAttacking && other.CompareTag("Enemy"))
        {
            // 傳遞傷害（需要 enemy 有 EnemyCore.cs）
            other.GetComponent<EnemyCore>()?.TakeDamage(20);
        }
    }

    public void DisableWeapon()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            var enemy = other.GetComponent<EnemyCore>();
            if (enemy != null)
            {
                enemy.TakeDamage(playerCore.attackPower);
            }
        }
    }
}
