using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VillageInfo : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] private TMP_Text seedsAmountText;
    [SerializeField] private TMP_Text totalVillagersText;
    [SerializeField] private TMP_Text unemployedAmountText;
    [SerializeField] private TMP_Text farmersAmountText;
    [SerializeField] private TMP_Text warriorsAmountText;
    [SerializeField] private TMP_Text archersAmountText;

    public void InitInfo(int maxVillagers,int seeds)
    {
        totalVillagersText.text = $"0 / {maxVillagers}";
        seedsAmountText.text = $"{seeds}";

        unemployedAmountText.text = $"0";
        farmersAmountText.text = $"0";
        warriorsAmountText.text = $"0";
        archersAmountText.text = $"0";
    }

    public void SetSeedsText(int seeds)
    {
        seedsAmountText.text = $"{seeds}";
    }

    public void SetVillagersAmountText(int currentVillagers,int maxVillagers)
    {
        totalVillagersText.text = $"{currentVillagers} / {maxVillagers}";
    }

    public void SetUnemployedText(int unemployedAmount)
    {
        unemployedAmountText.text = $"{unemployedAmount}";
    }

    public void SetFarmersText(int farmersAmount, int unemployedAmount)
    {
        farmersAmountText.text = $"{farmersAmount}";
        SetUnemployedText(unemployedAmount);
    }
    public void SetWarriorsText(int warriosAmount, int unemployedAmount)
    {
        warriorsAmountText.text = $"{warriosAmount}";
        SetUnemployedText(unemployedAmount);
    }
    public void SetArchersText(int archersAmount, int unemployedAmount)
    {
        archersAmountText.text = $"{archersAmount}";
        SetUnemployedText(unemployedAmount);
    }
}
