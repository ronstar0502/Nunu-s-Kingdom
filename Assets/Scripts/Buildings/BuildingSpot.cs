using System.Collections.Generic;
using UnityEngine;

public class BuildingSpot : MonoBehaviour
{
    [SerializeField] private GameObject building;
    public bool hasBuilding;
    public bool isBuildMenuOpen;


    //method to build a building at the current building spot
    public void BuildAtSpot()
    {
        if (building == null)
        {
            print("No Building Prefab Avilable");
            return;
        }
        if (hasBuilding) return;
        Instantiate(building, transform.position, Quaternion.identity, transform);
        print("instatiated");
        hasBuilding = true;
    }

}
