using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    protected BuildingData buildingData;

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
