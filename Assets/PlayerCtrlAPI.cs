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
        if (CallPlayerWeaponRotation()) return;
        if (CallPlayerCameraMove()) return;
        if (CallPlayerMove()) return;
    }

    bool CallPlayerCameraMove()
    {
        bool isCameraMove = false;
        PlayerCtrl playerCtrl = player.GetComponent<PlayerCtrl>();
        BodyState bodyState = GameObject.Find("BodyState").GetComponent<BodyState>();

        if ((bodyState._turn == 1 || bodyState._turn == -1))
        {
            isCameraMove = true;
            playerCtrl.CameraMoveX(bodyState._turn);
        }

        return isCameraMove;
    }


    bool CallPlayerMove()
    {
        bool isPlayerMove = false;
        PlayerCtrl playerCtrl = player.GetComponent<PlayerCtrl>();
        BodyState bodyState = GameObject.Find("BodyState").GetComponent<BodyState>();

        if (bodyState._moveV == 1 || bodyState._moveV == -1)
        {
            isPlayerMove = true;
            playerCtrl.PlayerMoveV(bodyState._moveV);
        }

        if (bodyState._moveH == 1 || bodyState._moveH == -1)
        {
            isPlayerMove = true;
            playerCtrl.PlayerMoveH(bodyState._moveH);
        }

        return isPlayerMove;
    }

    bool CallPlayerWeaponRotation()
    {
        bool isPlayerWeaponRotation = false;
        BodyState bodyState = GameObject.Find("BodyState").GetComponent<BodyState>();
        PlayerAttack playerAttack = player.GetComponent<PlayerAttack>();
        float startAngle = playerAttack.GetWeaponAngle();
        float endAngle = bodyState._handDegree;
        //Debug.Log(startAngle + "," + endAngle);

        if (bodyState._isCatch)
        {
            playerAttack.RotateWeapon(startAngle, endAngle);
            isPlayerWeaponRotation = true;
        }

        return isPlayerWeaponRotation;
    }

}