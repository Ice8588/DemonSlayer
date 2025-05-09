using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAppearanceDrawGizmos : MonoBehaviour
{
    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, new Vector3(0.5f, 1.8f, 0.5f));
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
