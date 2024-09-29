[System.Serializable]
public class VillagerData
{
    public string villagerName;
    public int maxHealth;
    public int health;
    public float movementSpeed;
    public int seedsCost;

    public void InitHealth()
    {
        health = maxHealth;
    }
}
