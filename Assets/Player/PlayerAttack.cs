/*
 * PlayerAttack.cs
 * This script handles the player's attack mechanics, including weapon rotation and attack animations.
 * It uses Unity's coroutine system to create smooth transitions for weapon swings and rotations.
 * The script also manages the weapon's collider to detect hits during attacks.
 * It is designed to be attached to the player character.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Transform weaponTransform; // Reference to the weapon's transform
    public Collider weaponCollider;   // Reference to the weapon's collider
    //public Animator animator;
    private float attackDuration = 0.8f; // Duration of the attack
    private bool isAttacking = false;    // Is the player currently attacking?
    private bool isRotating = false;     // Is the weapon currently rotating?

    public AudioClip swingSFX;
    private AudioSource audioSource;

    void Start()
    {
        // Disable the weapon's collider at the start of the game
        weaponCollider.enabled = false;
        // Reset the weapon's rotation to the default
        weaponTransform.rotation = Quaternion.Euler(0f, 0f, 0f);

        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        // Uncomment to enable manual rotation and attack input
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

    // Rotates the weapon from startAngle to endAngle
    public void RotateWeapon(float startAngle, float endAngle)
    {
        // Prevent multiple simultaneous rotations
        int startBlock = Mathf.FloorToInt(startAngle / 45f);
        int endBlock = Mathf.FloorToInt(endAngle / 45f);

        if (IsCorresponding(startBlock, endBlock))
        {
            Attack(startBlock);
        }
        else
        {
            if (!isRotating)
            {
                StartCoroutine(RotateAnimation(startAngle, endAngle));
            }
        }



        isRotating = true;
        StartCoroutine(RotateAnimation(startAngle, endAngle));
        isRotating = false;
    }

    // Coroutine for rotating the weapon smoothly between two angles
    private IEnumerator RotateAnimation(float startAngle, float endAngle)
    {
        float elapsedTime = 0f;
        float duration = 0.01f; // Duration of the rotation
        Quaternion startRotation = Quaternion.Euler(0f, 0f, startAngle);
        Quaternion endRotation = Quaternion.Euler(0f, 0f, endAngle);

        // Smoothly interpolate between start and end rotation
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            weaponTransform.localRotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / duration);
            yield return null;
        }

        // Ensure the final rotation is set
        weaponTransform.localRotation = endRotation;
    }

    // Returns the current Z angle of the weapon in degrees
    public float GetWeaponAngle()
    {
        return weaponTransform.localEulerAngles.z;
    }

    // Initiates the attack if not already attacking
    public void Attack(int block)
    {
        isAttacking = true;

        // Start the swing animation coroutine
        StartCoroutine(SwingAnimation());
    }

    // Coroutine for swinging the weapon
    IEnumerator SwingAnimation()
    {
        if (swingSFX != null && audioSource != null)
        {
            audioSource.PlayOneShot(swingSFX);
        }

        int swingAngle = 120; // The angle to swing the weapon

        Quaternion startRot = weaponTransform.localRotation;
        Quaternion swingRot = Quaternion.AngleAxis(swingAngle, Vector3.right);
        Quaternion targetRot = startRot * swingRot;

        float t = 0f;
        // Swing forward
        while (t < 0.3f)
        {
            t += Time.deltaTime;
            weaponTransform.localRotation = Quaternion.Slerp(startRot, targetRot, t / 0.3f);
            yield return null;
        }

        t = 0f;
        // Swing back to original position
        while (t < 0.5f)
        {
            t += Time.deltaTime;
            weaponTransform.localRotation = Quaternion.Slerp(targetRot, startRot, t / 0.5f);
            yield return null;
        }

        // Ensure the weapon returns to its original rotation
        weaponTransform.localRotation = startRot;

        // Need animation
        weaponTransform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        
        // Allow new attacks
        isAttacking = false;
    }

    private bool IsCorresponding(int blockA, int blockB)
    {
        //Debug.Log("blockA: " + blockA + ", blockB: " + blockB);
        for (int offset = 3; offset <= 5; offset++)
        {
            int target = ((blockA + offset - 1) % 8) + 1;
            if (blockB == target)
            {
                //Debug.Log("Corresponding: " + blockA + " -> " + blockB);
                return true;
            }
        }
        return false;
    }
}