using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    protected BuildingData buildingData; //seperated the data to different script for easier use

    private void Awake()
    {
        buildingData = GetComponent<BuildingDataHolder>().buildingData;
    }
    private void Start()
    {
        print(buildingData.ToString());
    }

    public BuildingData GetBuildingData() { return buildingData; }
}
