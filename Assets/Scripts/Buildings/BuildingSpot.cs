using UnityEngine;

public class BuildingSpot : MonoBehaviour
{
    [SerializeField] private GameObject building;
    private bool hasBuilding;
    public void BuildAtSpot()
    {
        if (hasBuilding) return;
        GameObject spotBuilding = Instantiate(building,transform.position,Quaternion.identity,transform);
    }
    public GameObject GetBuilding()
    {
        return building;
    }
}
