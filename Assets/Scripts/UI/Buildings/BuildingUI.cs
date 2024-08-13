using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUI : MonoBehaviour
{
    [SerializeField] private TMP_Text buildingName, buildingCost;
    [SerializeField] private Image buildingSprite;
    public GameObject buildingObject;
    private BuildingSpot buildingSpot;

    //method to initialize the building UI in the building menu
    public void InitBuildingItemUI(string name, int cost,Sprite sprite ,GameObject building,BuildingSpot currentBuildingSpot)
    {
        buildingName.text = $"{name}";
        buildingCost.text = $"Amount: {cost}";
        buildingSprite.sprite = sprite ;
        buildingObject = building;
        buildingSpot = currentBuildingSpot;
    }

    //method for the button in the build ui to build the specific building
    public void ConstructBuilding()
    {
        print("clicked build");
        if (buildingSpot != null)
        {
            buildingSpot.BuildAtSpot(buildingObject);
        }
        else
        {
            print("no building spot");
        }
    }
}
