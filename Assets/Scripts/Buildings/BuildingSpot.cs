using UnityEngine;

public class BuildingSpot : MonoBehaviour
{
    [SerializeField] private GameObject building;
    public bool hasBuilding;
    public void BuildAtSpot()
    {
        if (hasBuilding) return; 
        GameObject spotBuilding = Instantiate(building,transform.position,Quaternion.identity,transform);
        hasBuilding = true;
    }
    public GameObject GetBuilding()
    {
        return building;
    }
}
