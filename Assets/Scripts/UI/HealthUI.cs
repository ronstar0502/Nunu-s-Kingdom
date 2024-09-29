using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    private void Start()
    {
        _image.fillAmount = 1f;
    }

    public void SetHealthBar(float currentHealth,float maxHealth)
    {
        _image.fillAmount = currentHealth/maxHealth;
    }
}
