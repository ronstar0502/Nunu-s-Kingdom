using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManger : MonoBehaviour
{
    [SerializeField] private TMP_Text daysCountText;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameEndMenu;
    [SerializeField] private TMP_Text gameEndTxt;
    [SerializeField] private float dayDuration, nightDuration;
    private int maxDaysInLevel;
    public GameState gameState;
    private HQ _HQ; // will be pre built
    private Spawner _spawner;
    private FlowerSpawner _flowerSpawner;
    private Player _player;
    private TimeLineIndicator _timeLineIndicator;
    private float _lastStateSwapped = 0f;
    private int _currDay = 1;

    private void Awake()
    {
        _timeLineIndicator = FindObjectOfType<TimeLineIndicator>();
        _timeLineIndicator.SetDayCycleTimer(dayDuration + nightDuration);
        _spawner = FindAnyObjectByType<Spawner>();
        _flowerSpawner = FindAnyObjectByType<FlowerSpawner>();
        gameState = GameState.Day;
        _HQ = FindObjectOfType<HQ>();
        _player = FindObjectOfType<Player>();

        if(Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
            print("restarted level time");
        }
    }

    private void Start()
    {
        maxDaysInLevel = _spawner.GetWaveCount();
        SetDaysCountTxt();
        _player.SetCanBuild(gameState);
        gameEndMenu.SetActive(false);
    }

    private void SetDaysCountTxt()
    {
        daysCountText.text = $"{_currDay} / {maxDaysInLevel} Days";
    }

    void Update()
    {
        ChangeStateTimerCheck();
        CheckVictory();
        CheckDefeat();

        if (Input.GetKeyDown(KeyCode.Keypad0)) // cheat code for level 2
        {
            SceneManager.LoadScene(2);
        }

        if (Input.GetKeyDown(KeyCode.Escape)) // pause menu
        {
            OpenPauseMenu();
        }
    }

    private void CheckDefeat()
    {
        if (_HQ == null)
        {
            print("Game Over");
            gameEndMenu.SetActive(true);
            gameEndTxt.text = "Defeat :(";
            Time.timeScale = 0f;
        }
    }

    private void CheckVictory()
    {
        if (_currDay == maxDaysInLevel + 1)
        {
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                SceneManager.LoadScene(2);
                return;
            }
            print("You Won!");
            gameEndMenu.SetActive(true);
            gameEndTxt.text = "Victory!";
            Time.timeScale = 0f;
        }
    }

    private void ChangeStateTimerCheck() //check if game state needs to change based on time for state of Day/Night
    {
        if (Time.time >= _lastStateSwapped + dayDuration && gameState == GameState.Day)
        {

            SetState(GameState.Night);
            _lastStateSwapped = Time.time;
        }
        else if (Time.time >= _lastStateSwapped + nightDuration && gameState == GameState.Night)
        {
            SetState(GameState.Day);
            _lastStateSwapped = Time.time;
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
        _player.SetCanBuild(gameState);
        BuildingsSpriteDayNightChange();
    }

    private void StartNightActivities() //starts all night activities
    {
        print("NightTime!");
        StartCoroutine(_spawner.StartSpawning());
        StartCoroutine(_flowerSpawner.SpawnFlowers());
        _HQ.SetAmmigasToCombatMode();
    }

    private void StartDayActivities() //starts all day activities
    {
        CheckVictory();
        _currDay++;
        DestroyAllFlowers();
        _spawner.EnableAllPortals();
        _flowerSpawner.isNight = false;
        SetDaysCountTxt();
        print($"Day {_currDay}");
        HarvestFarm();
        _HQ.SetAmmigastToPatrolMode();
    }

    private void DestroyAllFlowers()
    {
        GameObject[] flowers = GameObject.FindGameObjectsWithTag("Flower");
        if (flowers.Length == 0)
        {
            return;
        }
        for(int i = 0 ; i<flowers.Length;i++)
        {
            Destroy(flowers[i]);
        }
    }

    private void HarvestFarm()
    {
        if (_HQ.farms.Count>0)
        {
            _HQ.FarmSeeds();
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
        return _lastStateSwapped;
    }

}
