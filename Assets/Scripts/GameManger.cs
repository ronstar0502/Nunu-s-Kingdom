using TMPro;
using UnityEngine;

public class GameManger : MonoBehaviour
{
    [SerializeField] private TMP_Text daysCountText;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameEndMenu;
    [SerializeField] private TMP_Text gameEndTxt;
    [SerializeField] private float dayDuration, nightDuration;
    [SerializeField] private int maxDaysInLevel;
    public GameState gameState;
    private HQ HQ; // will be pre built
    private Spawner spawner;
    private FlowerSpawner flowerSpawner;
    private Player player;
    private TimeLineIndicator timeLineIndicator;
    private float lastStateSwapped = 0f;
    private int currDay = 1;

    private void Awake()
    {
        timeLineIndicator = FindObjectOfType<TimeLineIndicator>();
        timeLineIndicator.SetDayCycleTimer(dayDuration + nightDuration);
        spawner = FindAnyObjectByType<Spawner>();
        flowerSpawner = FindAnyObjectByType<FlowerSpawner>();
        gameState = GameState.Day;
        HQ = FindObjectOfType<HQ>();
        player = FindObjectOfType<Player>();

        if(Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
            print("restarted level time");
        }
    }

    private void Start()
    {
        SetDaysCountTxt();
        player.SetCanBuild(gameState);
        gameEndMenu.SetActive(false);
    }

    private void SetDaysCountTxt()
    {
        daysCountText.text = $"{currDay} / {maxDaysInLevel} Days";
    }

    void Update()
    {
        ChangeStateTimerCheck();
        CheckVictory();
        CheckDefeat();

        if (Input.GetKeyDown(KeyCode.V)) //place holder for debugging if it works
        {
            gameEndMenu.SetActive(true);
            gameEndTxt.text = "Victory!";
            Time.timeScale = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Escape)) // pause menu
        {
            OpenPauseMenu();
        }
    }

    private void CheckDefeat()
    {
        if (HQ == null)
        {
            print("Game Over");
            gameEndMenu.SetActive(true);
            gameEndTxt.text = "Defeat :(";
            Time.timeScale = 0f;
        }
    }

    private void CheckVictory()
    {
        if (currDay == maxDaysInLevel + 1)
        {
            print("You Won!");
            gameEndMenu.SetActive(true);
            gameEndTxt.text = "Victory!";
            Time.timeScale = 0f;
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

    private void OpenPauseMenu()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
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
        BuildingsSpriteDayNightChange();
    }

    private void StartNightActivities() //starts all night activities
    {
        print("NightTime!");
        StartCoroutine(spawner.StartSpawning());
        StartCoroutine(flowerSpawner.SpawnFlowers());
        HQ.SetAmmigasToCombatMode();
    }
    private void StartDayActivities() //starts all day activities
    {
        currDay++;
        spawner.EnableAllPortals();
        flowerSpawner.isNight = false;
        SetDaysCountTxt();
        print($"Day {currDay}");
        HarvestFarm();
        HQ.SetAmmigastToPatrolMode();
    }

    private void HarvestFarm()
    {
        if (HQ.farm != null)
        {
            HQ.FarmSeeds();
        }
    }

    private void BuildingsSpriteDayNightChange()
    {
        DayNightSpriteChange[] dayNightSpriteChangeScripts = FindObjectsOfType<DayNightSpriteChange>();
        foreach (DayNightSpriteChange dayNightSpriteChangeScript in dayNightSpriteChangeScripts)
        {
            if (dayNightSpriteChangeScript != null)
            {
                StartCoroutine(dayNightSpriteChangeScript.DayNightColorTransition());
            }
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
