using UnityEngine;

[CreateAssetMenu(fileName ="New Building Data",menuName ="Building Data",order =1)]
public class BuildingData : ScriptableObject
{
    public string buildingName;
    [Header("Building Sprite And Level")]
    public Sprite sprite;
    public int level;
    [Header("Building Data")]
    public float health;
    public int cost;
    public bool isMaxLevel;
    [Header("Building Next Level")]
    public BuildingData nextLevelBuilding;
}
