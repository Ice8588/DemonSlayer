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
    private bool isRotating = false;

    void Start()
    {
        weaponCollider.enabled = false;
        weaponTransform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    void Update()
    {
        /*
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            ChangeRotation();
        }

        if ((Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0)) && !isAttacking)
        {
            Attack();
        }
        */
    }

    public void RotateWeapon(float startAngle, float endAngle)
    {
        if (isRotating)
        {
            return;
        }

        isRotating = true;
        StartCoroutine(SwingAnimation(startAngle, endAngle));
        isRotating = false;
    }

    private IEnumerator SwingAnimation(float startAngle, float endAngle)
    {
        float elapsedTime = 0f;
        float duration = 0.01f;
        Quaternion startRotation = Quaternion.Euler(0f, 0f, startAngle);
        Quaternion endRotation = Quaternion.Euler(0f, 0f, endAngle);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            weaponTransform.localRotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / duration);
            yield return null;
        }

        weaponTransform.localRotation = endRotation;
    }

    public float GetWeaponAngle()
    {
        return weaponTransform.localEulerAngles.z;
    }

    public void Attack()
    {
        if (isAttacking)
        {
            return;
        }

        isAttacking = true;
        //animator.SetTrigger("Attack");

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
}