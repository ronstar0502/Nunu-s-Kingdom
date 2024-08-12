using UnityEngine;

[System.Serializable]
public class BuildingData 
{
    [SerializeField] protected Sprite buildingSprite;
    [SerializeField] protected string buildingName;
    [SerializeField] protected float buildingHP;
    [SerializeField] protected float buildingTime;
    [SerializeField] protected int cost;

    public override string ToString()
    {
        return $"{buildingName} : HP -> {buildingHP} , Building Cost -> {cost}";
    }
}
