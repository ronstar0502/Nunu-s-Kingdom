using System.Collections.Generic;
using UnityEngine;

public class BuildingSpot : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject buildingObj;
    private Player player;
    private int buildingStartingCost;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        buildingStartingCost = buildingObj.GetComponent<Building>().GetBuildingData().costLevels[0];
    }

    //method to build a building at the current building spot
    public void BuildAtSpot()
    {
        if (buildingObj == null) //check first if the buildingObj prefab is not null
        {
            print("No Building Prefab Avilable");
            return;
        }
        Instantiate(buildingObj, transform.position, Quaternion.identity); //spawns the building at the desired position
        player.GetPlayerData().SubstarctSeedsAmount(buildingStartingCost); //substracts seeds amount based on building starting cost
        print("instatiated");

        //add a delay with animation of the building being built?

        Destroy(gameObject); //destroying the building spot after the building is built

    }

    public void Interact() //interaction with the building spot
    {
        if (player.GetPlayerData().seedAmount >= buildingStartingCost)
        {
            BuildAtSpot();
        }
        else
        {
            print("not enough seeds!");
        }

    }
}
