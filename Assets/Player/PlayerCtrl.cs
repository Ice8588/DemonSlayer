using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float cameraMoveSpeed = 0.2f;
    //public float mouseSensitivity = 2f;
    public Transform playerCamera;
    private CharacterController controller;
    private float cameraPitch = 0f;
    public float gravity = -9.81f;
    public float verticalVelocity = 0f;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (controller.isGrounded && verticalVelocity < 0)
        {
            // 重設垂直速度以避免累加到地底下
            verticalVelocity = -2f; // 小負值防止貼地浮空
        }
        else
        {
            // 套用重力
            verticalVelocity += gravity * Time.deltaTime;
        }

        Vector3 move = new Vector3(0, verticalVelocity, 0);
        controller.Move(move * Time.deltaTime);
    }

    public void CameraMoveX(int direction = 0) //-1 = left, 0 = none, 1 = right
    {
        float arrowX = direction;
        transform.Rotate(arrowX * cameraMoveSpeed * Vector3.up);
    }

    public void CameraMoveY(int direction = 0) //-1 = down, 0 = none, 1 = up
    {
        float arrowY = direction;

        cameraPitch -= arrowY * cameraMoveSpeed;
        cameraPitch = Mathf.Clamp(cameraPitch, -80f, 80f);
        playerCamera.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
    }

    public void PlayerMoveH(int direction = 0) //-1 = left, 0 = none, 1 = right
    {
        float h = direction;
        Vector3 move = transform.right * h;
        controller.Move(move * moveSpeed * Time.deltaTime);
    }

    public void PlayerMoveV(int direction = 0) //-1 = back, 0 = none, 1 = forward
    {
        float v = direction;
        Vector3 move = transform.forward * v;
        controller.Move(move * moveSpeed * Time.deltaTime);
    }

}
