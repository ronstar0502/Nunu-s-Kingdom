using UnityEngine;

public class BuildingSpot : MonoBehaviour
{
    [SerializeField] private GameObject building;

    public void BuildAtSpot()
    {
        GameObject spotBuilding = Instantiate(building,transform.position,Quaternion.identity,transform);
    }
    public GameObject GetBuilding()
    {
        return building;
    }
}
