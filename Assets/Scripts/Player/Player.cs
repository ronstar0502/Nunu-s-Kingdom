using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    private BuildingSpot _buildingSpot;
    private void Awake()
    {
        playerData.SetInitHealth();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("BuildingSpot")&& Input.GetKeyDown(KeyCode.Space))
        {
            _buildingSpot = collision.gameObject.GetComponent<BuildingSpot>();
            _buildingSpot.BuildAtSpot();
        }
    }
}
