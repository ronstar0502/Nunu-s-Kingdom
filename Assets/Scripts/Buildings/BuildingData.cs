using UnityEngine;

[System.Serializable]
public class BuildingData 
{
    public Sprite buildingSprite;
    public string buildingName;
    public float buildingHP;
    public float buildingTime;
    public int cost;

    public override string ToString()
    {
        return $"{buildingName} : HP -> {buildingHP} , Building Cost -> {cost}";
    }
}
