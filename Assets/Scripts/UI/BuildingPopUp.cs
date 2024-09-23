using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuildingPopUp : MonoBehaviour
{
    [SerializeField] private TMP_Text upgradeTxt;
    [SerializeField] private TMP_Text recruitTxt;
    private HQ _HQ;
    private Player _player;
    private void Start()
    {
        _HQ = FindObjectOfType<HQ>();
        _player = FindObjectOfType<Player>();
        gameObject.SetActive(false);
    }
    public void EnableBuildingPopUp(int upgradeCost ,int recruitCost,bool canRecruit)
    {
        upgradeTxt.text = $"Upgrade cost: {upgradeCost}";
        recruitTxt.text = $"Recruit cost: {recruitCost}";
        SetUpgradeTxtColor(upgradeCost);
        SetRecruitTxtColor(recruitCost, canRecruit);
    }

    public void EnableBuildingPopUp(int upgradeCost)
    {
        upgradeTxt.text = $"Upgrade cost: {upgradeCost}";
        SetUpgradeTxtColor(upgradeCost);

    }
    private void SetRecruitTxtColor(int recruitCost , bool canRecruit)
    {
        if (_player.GetPlayerData().seedAmount >= recruitCost && canRecruit)
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
        if (_player.GetPlayerData().seedAmount >= upgradeCost)
        {
            upgradeTxt.color = Color.green;
        }
        else
        {
            upgradeTxt.color = Color.red;
        }
    }

}
