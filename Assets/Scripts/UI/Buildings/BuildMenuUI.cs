using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject buildMenuScrollViewContent;
    [SerializeField] private GameObject buildingUIPrefab;
    public void InitBuildMenu(List<GameObject> buildings,BuildingSpot buildingSpot)
    {
        foreach (GameObject buildingObj in buildings)
        {
            BuildingData buildingData = buildingObj.GetComponent<BuildingDataHolder>().buildingData;
            GameObject buildingItemUI = Instantiate(buildingUIPrefab, buildMenuScrollViewContent.transform);
            buildingItemUI.GetComponent<BuildingUI>().InitBuildingItemUI(buildingData.buildingName,buildingData.cost,buildingData.buildingSprite,buildingObj,buildingSpot);
        }
    }

    

}
