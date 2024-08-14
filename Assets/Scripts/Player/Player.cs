using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    private BuildingSpot _buildingSpot;
    private bool isOverlappingBuildingSpot;
    private void Awake()
    {
        playerData.SetInitHealth();
    }
    //somethng
    private void Update()
    {
        //checking if the player is overlapping a building spot when pressing space to build
        if (isOverlappingBuildingSpot && Input.GetKeyDown(KeyCode.Space))
        {
            //checking if there iss a building already at the building spot
            if (!_buildingSpot.hasBuilding && ! _buildingSpot.isBuildMenuOpen)
            {
                _buildingSpot.EnableBuildMenu();
            }
            if (_buildingSpot.isBuildMenuOpen)
            {
                _buildingSpot.DisableBuildMenu();
            }
        }
    }

    //on trigger -> checks if  the player is overlapping a building spot
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BuildingSpot"))
        {
            _buildingSpot = collision.gameObject.GetComponent<BuildingSpot>();
            isOverlappingBuildingSpot = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BuildingSpot"))
        {
            _buildingSpot = null;
            isOverlappingBuildingSpot = false;
        }
    }
}
