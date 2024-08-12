[System.Serializable]
public class PlayerData
{
    public float maxHealth;
    public float health;
    public void SetInitHealth()
    {
        health = maxHealth;
    }
}
