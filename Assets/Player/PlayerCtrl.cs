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
        CameraMove();
        PlayerMove();
    }

    void CameraMove()
    {
        float arrowX = 0;
        float arrowY = 0;

        if (Input.GetKey(KeyCode.RightArrow) && !(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
        {
            arrowX = 1;
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && !(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
        {
            arrowX = -1;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            arrowY = 1;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            arrowY = -1;
        }

        transform.Rotate(Vector3.up * arrowX * cameraMoveSpeed);

        cameraPitch -= arrowY * cameraMoveSpeed;
        cameraPitch = Mathf.Clamp(cameraPitch, -80f, 80f);
        playerCamera.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
    }

    void PlayerMove()
    {
        float h = 0;
        float v = 0;

        if (Input.GetKey(KeyCode.W))
        {
            v = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            v = -1;
        }

        if (Input.GetKey(KeyCode.A))
        {
            h = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            h = 1;
        }

        Vector3 move = transform.right * h + transform.forward * v;
        controller.Move(move * moveSpeed * Time.deltaTime);
    }

}
