using UnityEngine;

public class GameManger : MonoBehaviour
{
    [SerializeField] private float dayDuration, nightDuration;
    private HQ HQ; // will be pre built
    private Spawner spawner;
    public GameState gameState;
    private float lastStateSwapped=0f;
    private int currday = 1;

    private void Awake()
    {
        spawner = FindAnyObjectByType<Spawner>();
        gameState = GameState.Day;
        HQ = FindObjectOfType<HQ>();
    }

    void Update()
    {
        ChangeStateTimerCheck();

        if(gameState == GameState.Night)
        {
            if(HQ == null)
            {
                print("Game Over");
                Time.timeScale = 0f;
            }
        }
    }
    private void ChangeStateTimerCheck() //check if game state needs to change based on time for state of Day/Night
    {
        if (Time.time >= lastStateSwapped + dayDuration && gameState == GameState.Day)
        {
            
            SetState(GameState.Night);
            lastStateSwapped = Time.time;
        }
        else if (Time.time >= lastStateSwapped + nightDuration && gameState == GameState.Night)
        {
            SetState(GameState.Day);
            lastStateSwapped = Time.time;
        }
    }

    private void SetState(GameState state) //sets the game states
    {
        gameState = state;
        switch (gameState)
        {
            case GameState.Day:
                StartDayActivities();
                break;
            case GameState.Night:
                StartNightActivities();
                break;
        }
    }

    private void StartNightActivities() //starts all night activities
    {
        print("NightTime!");
        spawner.StartSpawning();
        HQ.SetCombatVillagers();
    }
    private void StartDayActivities() //starts all day activities
    {
        currday++;
        print($"Day {currday}");
        HarvestFarm();
        HQ.SetCombatToPatrol();
        currday++;
    }

    private void HarvestFarm()
    {
        if (HQ.farm != null)
        {
            HQ.FarmSeeds();
        }
    }
    
}
