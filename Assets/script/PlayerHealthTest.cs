using UnityEngine;

public class PlayerHealthTest : MonoBehaviour
{
    public HealthBar healthBar;
    int maxHP = 100;
    int currentHP;

    void Start()
    {
        currentHP = maxHP;
        healthBar.SetMaxHealth(maxHP);
    }

    void Update()
    {
      
        if (Input.GetKeyDown(KeyCode.H))
        {
            currentHP -= 10;
            healthBar.SetHealth(currentHP);
        }
    }
}
