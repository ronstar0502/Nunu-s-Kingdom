using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class GameManger : MonoBehaviour
{
    [SerializeField] private float dayDuration, nightDuration;
    private Farm[] farms;
    //private List<Farm> farms;
    private Spawner spawner;
    public GameState gameState;
    private float lastStateSwapped=0f;

    private void Awake()
    {
        spawner = FindAnyObjectByType<Spawner>();
        gameState = GameState.Day;
    }

    private void Start()
    {
        farms = FindObjectsByType<Farm>(FindObjectsSortMode.None);
    }
    void Update()
    {
        ChangeStateTimerCheck();
    }

    /*public void AddFarm(Farm farm)
    {
        farms.Add(farm);
    }*/
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
            print("DayTime!");
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
        //DeactivateFarms();
        spawner.StartSpawning();
    }
    private void StartDayActivities() //starts all day activities
    {
        print("DayTime!");
        HarvestFarms();
        //ActivateFarms();
    }

    private void HarvestFarms()
    {
        if (farms.Length > 0)
        {
            for (int i = 0; i < farms.Length; i++)
            {
                farms[0].FarmSeeds();
            }
        }
        print("harvested farms");
    }
    /*private void ActivateFarms() 
    {
        if(farms.Length > 0)
        {
            for (int i = 0; i < farms.Length; i++)
            {
                farms[0].ActivateFarm();
            }
        }
        print("activated farms");
    }
    private void DeactivateFarms()
    {
        if (farms.Length > 0)
        {
            for (int i = 0; i < farms.Length; i++)
            {
                farms[0].DeactivateFarm();
            }
        }
        print("deactivated farms");
    }*/



}
