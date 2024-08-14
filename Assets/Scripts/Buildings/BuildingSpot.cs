using System.Collections.Generic;
using UnityEngine;

public class BuildingSpot : MonoBehaviour
{
    [SerializeField] private GameObject buildMenu;
    [SerializeField] private List<GameObject> buildingPrefabs;
    public bool hasBuilding;
    public bool isBuildMenuOpen;


    private void Start()
    {
        buildMenu.SetActive(false);
        buildMenu.GetComponent<BuildMenuUI>().InitBuildMenu(buildingPrefabs, this);
    }
    //method to build a building at the current building spot
    public void BuildAtSpot(GameObject building)
    {
        if (hasBuilding) return;
        Instantiate(building, transform.position, Quaternion.identity, transform);
        print("instatiated");
        hasBuilding = true;
        buildMenu.SetActive(false);
    }

    //method to enable and initialize the build menue
    public void EnableBuildMenu()
    {
        buildMenu.SetActive(true);
        isBuildMenuOpen = true;
    }
    //method to disable the build menu
    public void DisableBuildMenu()
    {
        buildMenu.SetActive(false);
        isBuildMenuOpen = false;
    }
}
