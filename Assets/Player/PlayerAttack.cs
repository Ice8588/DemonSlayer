using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Transform weaponTransform;
    public Collider weaponCollider;
    //public Animator animator;
    private float rotationSpeed = 180f;
    private float attackDuration = 0.8f;
    private bool isAttacking = false;

    void Start()
    {
        weaponCollider.enabled = false;
        weaponTransform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            ChangeRotation();
        }

        if ((Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0)) && !isAttacking)
        {
            Attack();
        }
    }

    void ChangeRotation()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            weaponTransform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime, Space.Self);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            weaponTransform.Rotate(Vector3.forward, -rotationSpeed * Time.deltaTime, Space.Self);
        }
    }

    void Attack()
    {
        isAttacking = true;
        //animator.SetTrigger("Attack");

        //weaponCollider.enabled = true;

        StartCoroutine(SwingAnimation());
        Invoke(nameof(ResetAttack), attackDuration);
    }

    IEnumerator SwingAnimation()
    {
        int swingAngle = 120;

        Quaternion startRot = weaponTransform.localRotation;
        Quaternion swingRot = Quaternion.AngleAxis(swingAngle, Vector3.right);
        Quaternion targetRot = startRot * swingRot;

        float t = 0f;
        while (t < 0.3f)
        {
            t += Time.deltaTime;
            weaponTransform.localRotation = Quaternion.Slerp(startRot, targetRot, t / 0.3f);
            yield return null;
        }

        t = 0f;
        while (t < 0.5f)
        {
            t += Time.deltaTime;
            weaponTransform.localRotation = Quaternion.Slerp(targetRot, startRot, t / 0.5f);
            yield return null;
        }

        weaponTransform.localRotation = startRot;
    }

    void ResetAttack()
    {
        weaponCollider.enabled = false;
        isAttacking = false;
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (isAttacking && other.CompareTag("Enemy"))
        {
            // 傳遞傷害（需要 enemy 有 EnemyCore.cs）
            other.GetComponent<EnemyCore>()?.TakeDamage(20);
        }
    }*/
}