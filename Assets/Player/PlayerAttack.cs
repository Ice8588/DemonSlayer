using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Transform weaponTransform;
    public Collider weaponCollider;
    //public Animator animator;
    private float rotationSpeed = 360f;
    public float attackDuration = 0.8f;
    private float weaponAngle = 0f;
    private bool isAttacking = false;

    void Start()
    {
        weaponCollider.enabled = false;
        weaponTransform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    void Update()
    {

        Vector3 pos = weaponTransform.position;

        // X 軸（紅色）
        Debug.DrawRay(pos, weaponTransform.right * 0.5f, Color.red);

        // Y 軸（綠色）
        Debug.DrawRay(pos, weaponTransform.up * 0.5f, Color.green);

        // Z 軸（藍色）
        Debug.DrawRay(pos, weaponTransform.forward * 0.5f, Color.blue);
        if (Input.GetMouseButton(1))
        {
            ChangeRotation();
        }

        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            Attack();
        }
    }

    void ChangeRotation()
    {
        weaponTransform.Rotate(Vector3.forward, Input.GetAxis("Mouse X") * -rotationSpeed * Time.deltaTime, Space.Self);
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
        int swingAngle = 60;

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