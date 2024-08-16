[System.Serializable]
public class PlayerData
{
    public float maxHealth;
    public float health;
    public int seedAmount;//currency
    public void SetInitHealth()
    {
        health = maxHealth;
    }
}
