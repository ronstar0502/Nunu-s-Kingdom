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

    private void Update()
    {
        if (isOverlappingBuildingSpot && Input.GetKeyDown(KeyCode.Space))
        {
            _buildingSpot.BuildAtSpot();
        }
    }

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
