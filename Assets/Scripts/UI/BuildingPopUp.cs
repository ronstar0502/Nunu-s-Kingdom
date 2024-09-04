using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuildingPopUp : MonoBehaviour
{
    [SerializeField] private TMP_Text upgradeTxt;
    [SerializeField] private TMP_Text recruitTxt;
    private Player player;

    private void Start()
    {
        gameObject.SetActive(false);
        player = FindObjectOfType<Player>();
    }
    public void EnableBuildingPopUp(int upgradeCost ,int recruitCost)
    {
        upgradeTxt.text = $"Upgrade cost: {upgradeCost}";
        recruitTxt.text = $"Recruit cost: {recruitCost}";

        if(player.GetPlayerData().seedAmount >= upgradeCost)
        {
            upgradeTxt.color = Color.green;
        }
        else
        {
            upgradeTxt.color = Color.red;
        }

        if (player.GetPlayerData().seedAmount >= recruitCost)
        {
            upgradeTxt.color = Color.green;
        }
        else
        {
            upgradeTxt.color = Color.red;
        }
    }

    public void EnableBuildingPopUp(int upgradeCost)
    {
        upgradeTxt.text = $"Upgrade cost: {upgradeCost}";

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
