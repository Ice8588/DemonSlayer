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
    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        /*
        CameraMoveX();
        CameraMoveY();
        PlayerMoveH();
        PlayerMoveV();
        */
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
