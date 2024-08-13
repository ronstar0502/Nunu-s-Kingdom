using System.Collections.Generic;
using UnityEngine;

public class BuildingSpot : MonoBehaviour
{
    [SerializeField] private GameObject buildMenu;
    [SerializeField] private List<GameObject> buildingPrefabs;
    public bool hasBuilding;

    private void Awake()
    {
        buildMenu.SetActive(false);
    }
    public void BuildAtSpot(GameObject building)
    {
        if (hasBuilding) return;
        Instantiate(building, transform.position, Quaternion.identity, transform);
        print("instatiated");
        hasBuilding = true;
        buildMenu.SetActive(false);
    }

    public void EnableBuildMenu()
    {
        buildMenu.SetActive(true);
        buildMenu.GetComponent<BuildMenuUI>().InitBuildMenu(buildingPrefabs,this);
    }
}
