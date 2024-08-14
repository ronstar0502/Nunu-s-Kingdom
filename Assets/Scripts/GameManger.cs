using UnityEngine;

public class GameManger : MonoBehaviour
{
    [SerializeField] private float dayDuration, nightDuration;
    private GameState gameState;
    private float lastStateSwapped=0f;
    private Spawner spawner;

    private void Awake()
    {
        spawner = FindAnyObjectByType<Spawner>();
        gameState = GameState.Day;
    }
    void Update()
    {
        if(Time.time>= lastStateSwapped+dayDuration && gameState==GameState.Day)
        {
            gameState = GameState.Night;
            lastStateSwapped = Time.time;
            print("NightTime!");
            spawner.StartSpawning();
            
        }
        else if (Time.time>= lastStateSwapped+nightDuration && gameState == GameState.Night)
        {
            gameState = GameState.Day;
            lastStateSwapped = Time.time;
            print("DayTime!");
        }
    }
}
