using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingPopUp : MonoBehaviour
{
    [SerializeField] private TMP_Text upgradeTxt;
    [SerializeField] private TMP_Text recruitTxt;
    [SerializeField] private TMP_Text maxLevelTxt;
    [SerializeField] private TMP_Text buildingLevelTxt;
    [SerializeField] private Image[] images;
    private HQ _HQ;
    private Player _player;
    private void Start()
    {
        _HQ = FindObjectOfType<HQ>();
        _player = FindObjectOfType<Player>();
        if (maxLevelTxt != null)
        {
            maxLevelTxt.enabled = false;
        }
        gameObject.SetActive(false);
    }
    public void EnableBuildingPopUp(int upgradeCost ,int recruitCost,bool canRecruit , int buildingLevel)
    {
        SetUpgradeText(upgradeCost);
        SetRecruitText(recruitCost, canRecruit);
        buildingLevelTxt.text = $"Building Level: {buildingLevel}";
    }

    public void EnableBuildingPopUp(int upgradeCost, int buildingLevel)
    {
        SetUpgradeText(upgradeCost);
        buildingLevelTxt.text = $"Building Level: {buildingLevel}";
    }

    public void EnableBuildingSpotPopUp(int upgradeCost, int upgradeLevel)
    {
        SetBuildingText(upgradeCost);
    }

    public void SetToMaxLevelBuildingPopUp()
    {
        upgradeTxt.enabled = false;
        buildingLevelTxt.enabled = false;
        if (recruitTxt != null)
        {
            recruitTxt.enabled = false;
        }
        for (int i = 0; i < images.Length; i++) 
        {
            if (images[i] != null)
            {
                images[i].enabled = false;
            }
        }
        maxLevelTxt.enabled = true;
    }

    private void SetBuildingText(int upgradeCost)
    {
        upgradeTxt.text = $"Building cost: {upgradeCost}";
        SetUpgradeTxtColor(upgradeCost);
    }
    private void SetUpgradeText(int upgradeCost)
    {
        upgradeTxt.text = $"Upgrade cost: {upgradeCost}";
        SetUpgradeTxtColor(upgradeCost);
    }

    private void SetRecruitText(int recruitCost , bool canRecruit)
    {
        recruitTxt.text = $"Recruit cost: {recruitCost}";
        SetRecruitTxtColor(recruitCost, canRecruit);
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
