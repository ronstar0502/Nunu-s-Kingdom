[System.Serializable]
public class PlayerData
{
    public float maxHealth;
    public float health;
    public int startingSeedsAmount; //currency
    public int seedAmount; //currency
    public void InitPlayer()
    {
        health = maxHealth;
        seedAmount = startingSeedsAmount;
    }

    public void SubstarctSeedsAmount(int amount)
    {
        seedAmount -= amount;
    }

    public void AddSeedsAmount(int amount)
    {
        seedAmount += amount;
    }
}
