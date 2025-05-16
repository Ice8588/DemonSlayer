using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public const int MAXHEALTH = 100;
    public int currentHealth = 100;
    public int attackPower = 10;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = MAXHEALTH;
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player Died");
    }
}
