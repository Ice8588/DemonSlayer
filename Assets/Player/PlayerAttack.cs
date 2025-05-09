using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Collider weaponCollider;
    public Animator animator;
    public PlayerCore playerCore;

    void Start()
    {
        weaponCollider.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    public void Attack()
    {
        weaponCollider.enabled = true;
        
        
        weaponCollider.enabled = false;
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
