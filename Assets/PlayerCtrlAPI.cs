using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrlAPI : MonoBehaviour
{
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CallPlayerCameraMove();
        CallPlayerMove();

        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            CallPlayerWeaponRotation();
        }

        if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0))
        {
            CallPlayerAttack();
        }
    }

    void CallPlayerCameraMove()
    {
        PlayerCtrl playerCtrl = player.GetComponent<PlayerCtrl>();

        if (Input.GetKey(KeyCode.RightArrow) && !(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
        {
            playerCtrl.CameraMoveX(1);
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && !(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
        {
            playerCtrl.CameraMoveX(-1);
        }

        if (Input.GetKey(KeyCode.UpArrow) && !(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
        {
            playerCtrl.CameraMoveY(1);
        }
        else if (Input.GetKey(KeyCode.DownArrow) && !(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
        {
            playerCtrl.CameraMoveY(-1);
        }
    }


    void CallPlayerMove()
    {
        PlayerCtrl playerCtrl = player.GetComponent<PlayerCtrl>();

        if (Input.GetKey(KeyCode.W))
        {
            playerCtrl.PlayerMoveV(1);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            playerCtrl.PlayerMoveV(-1);
        }

        if (Input.GetKey(KeyCode.A))
        {
            playerCtrl.PlayerMoveH(-1);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            playerCtrl.PlayerMoveH(1);
        }
    }

    void CallPlayerWeaponRotation()
    {
        PlayerAttack playerAttack = player.GetComponent<PlayerAttack>();
        float startAngle = playerAttack.GetWeaponAngle();
        float endAngle = startAngle;
        //Debug.Log(startAngle + "," + endAngle);

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            endAngle += 100f * Time.deltaTime;
            playerAttack.RotateWeapon(startAngle, endAngle);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            endAngle -= 100f * Time.deltaTime;
            playerAttack.RotateWeapon(startAngle, endAngle);
        }
    }

    void CallPlayerAttack()
    {
        PlayerAttack playerAttack = player.GetComponent<PlayerAttack>();
        playerAttack.Attack();
    }
}
