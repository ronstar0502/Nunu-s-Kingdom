using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] protected BuildingData buildingData;

    private void Start()
    {
        print(buildingData.ToString());
    }
}
