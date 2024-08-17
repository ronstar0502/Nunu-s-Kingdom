using System.Collections.Generic;
using UnityEngine;

public class BuildingSpot : MonoBehaviour , IInteractable
{
    [SerializeField] private GameObject buildingObj;
    private Player player;
    private int buildingInitialCost;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        buildingInitialCost = buildingObj.GetComponent<Building>().GetBuildingData().costLevels[0];
    }

    //method to build a building at the current building spot
    public void BuildAtSpot()
    {
        if (buildingObj == null)
        {
            print("No Building Prefab Avilable");
            return;
        }
        Instantiate(buildingObj, transform.position, Quaternion.identity);
        player.GetPlayerData().SubstarctSeedsAmount(buildingInitialCost);
        print("instatiated");
        Destroy(gameObject);
    }

    public void Interact()
    {
        if(player.GetPlayerData().seedAmount >= buildingInitialCost)
        {
            BuildAtSpot();
        }
        else
        {
            print("not enough seeds!");
        }

    }
}
