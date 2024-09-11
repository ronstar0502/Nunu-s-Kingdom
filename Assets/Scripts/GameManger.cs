using TMPro;
using UnityEngine;

public class GameManger : MonoBehaviour
{
    [SerializeField] private TMP_Text daysCountText;
    [SerializeField] private float dayDuration, nightDuration;
    [SerializeField] private int maxDaysInLevel;
    public GameState gameState;
    private HQ HQ; // will be pre built
    private Spawner spawner;
    private Player player;
    private TimeLineIndicator timeLineIndicator;
    private float lastStateSwapped=0f;
    private int currDay = 1;

    private void Awake()
    {
        timeLineIndicator = FindObjectOfType<TimeLineIndicator>();
        timeLineIndicator.SetDayCycleTimer(dayDuration+nightDuration);
        spawner = FindAnyObjectByType<Spawner>();
        gameState = GameState.Day;
        HQ = FindObjectOfType<HQ>();
        player = FindObjectOfType<Player>();
    }

    private void Start()
    {
        SetDaysCountTxt();
        player.SetCanBuild(gameState);
    }

    private void SetDaysCountTxt()
    {
        daysCountText.text = $"{currDay} / {maxDaysInLevel} Days";
    }

    void Update()
    {
        ChangeStateTimerCheck();
        
        if(currDay == maxDaysInLevel + 1)
        {
            print("You Won!");
            Time.timeScale = 0f;
        }

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
        player.SetCanBuild(gameState);
    }

    private void StartNightActivities() //starts all night activities
    {
        print("NightTime!");
        StartCoroutine(spawner.StartSpawning()); //place holder
        HQ.SetAmmigasToCombatMode();
    }
    private void StartDayActivities() //starts all day activities
    {
        currDay++;
        SetDaysCountTxt();
        print($"Day {currDay}");
        HarvestFarm();
        HQ.SetAmmigastToPatrolMode();
        currDay++;
    }

    private void HarvestFarm()
    {
        if (HQ.farm != null)
        {
            HQ.FarmSeeds();
        }
    }

    public float GetDayDuration()
    {
        return dayDuration;
    }

    public float GetNightDuration()
    {
        return nightDuration;
    }

    public float GetLastStateSwapedTime()
    {
        return lastStateSwapped;
    }
    
}
