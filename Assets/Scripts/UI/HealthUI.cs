using System.Collections;
using System.Collections.Generic;
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
    public void TakeDamage()
    {
        _image.fillAmount -= .25f;
    }
}
