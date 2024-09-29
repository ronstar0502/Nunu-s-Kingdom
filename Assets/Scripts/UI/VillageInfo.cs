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
    [SerializeField] private TMP_Text moreInfoText;
    [SerializeField] private GameObject infoWindow;
    private Player _player;

    public void InitInfo(int maxVillagers,int seeds)
    {
        _player = FindObjectOfType<Player>();
        totalVillagersText.text = $"0 / {maxVillagers}";
        seedsAmountText.text = $"{_player.GetPlayerData().seedAmount}";

        unemployedAmountText.text = $"0";
        farmersAmountText.text = $"0";
        warriorsAmountText.text = $"0";
        archersAmountText.text = $"0";
    }

    private void Start()
    {
        if (infoWindow != null)
        {
            infoWindow.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && infoWindow != null)
        {
            infoWindow.SetActive(true);
            if (moreInfoText != null)
            {
                moreInfoText.text = "";
            }
        }
        else if (Input.GetKeyUp(KeyCode.Tab) && infoWindow!=null)
        {
            infoWindow.SetActive(false);
            if (moreInfoText != null)
            {
                moreInfoText.text = "Hold Tab for more info";
            }
        }


    }

    public void SetSeedsText(bool isLower)
    {
        seedsAmountText.text = $"{_player.GetPlayerData().seedAmount}";
        StartCoroutine(ChangeSeedTextColor(isLower));
    }

    private IEnumerator ChangeSeedTextColor(bool isLower)
    {
        Color original= seedsAmountText.color;
        if (isLower)
        {
            seedsAmountText.color = Color.red;
        }
        else
        {
            seedsAmountText.color = Color.green;
        }
        yield return new WaitForSeconds(1f);
        seedsAmountText.color = original;
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
