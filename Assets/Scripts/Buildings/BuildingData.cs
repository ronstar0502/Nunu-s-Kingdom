using UnityEngine;

[System.Serializable]
public class BuildingData 
{
    public string buildingName;
    [Header("Building Sprites And Levels")]
    public Sprite[] buildingSprites;
    public int[] costLevels;
    public int buildingLevel;
    [Header("Building Data")]
    public float maxBuildingHP;
    public float buildingHP;
    public float buildingTime;

    public void InitBuildingHP()
    {
        buildingHP = maxBuildingHP;
    }
    public void TakeDamage(float damage)
    {
        buildingHP -= damage;
    }

    public void LevelUp()
    {
        buildingLevel++;
    }
    public int GetNextLevelCost()
    {
        return costLevels[buildingLevel];
    }
    public override string ToString()
    {
        return $"{buildingName} : HP -> {buildingHP} , Building Cost -> {costLevels[0]}";
    }
}
