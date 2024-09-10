using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuildingPopUp : MonoBehaviour
{
    [SerializeField] private TMP_Text upgradeTxt;
    [SerializeField] private TMP_Text recruitTxt;
    private HQ HQ;
    private Player player;

    private void Start()
    {
        gameObject.SetActive(false);
        HQ = FindObjectOfType<HQ>();
        player = FindObjectOfType<Player>();
    }
    public void EnableBuildingPopUp(int upgradeCost ,int recruitCost)
    {
        upgradeTxt.text = $"Upgrade cost: {upgradeCost}";
        recruitTxt.text = $"Recruit cost: {recruitCost}";
        SetUpgradeTxtColor(upgradeCost);
        SetRecruitTxtColor(recruitCost);
    }

    public void EnableBuildingPopUp(int upgradeCost)
    {
        upgradeTxt.text = $"Upgrade cost: {upgradeCost}";
        SetUpgradeTxtColor(upgradeCost);

    }
    private void SetRecruitTxtColor(int recruitCost)
    {
        if (player.GetPlayerData().seedAmount >= recruitCost && HQ.HasUnemployedVillager())
        {
            recruitTxt.color = Color.green;
        }
        else
        {
            recruitTxt.color = Color.red;
        }
    }

    private void SetUpgradeTxtColor(int upgradeCost)
    {
        if (player.GetPlayerData().seedAmount >= upgradeCost)
        {
            upgradeTxt.color = Color.green;
        }
        else
        {
            upgradeTxt.color = Color.red;
        }
    }

}
