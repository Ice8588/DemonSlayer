using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Image fillImage;

    public void SetMaxHealth(int maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
        fillImage.enabled = true;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
        fillImage.enabled = (health > 0);
    }
}
