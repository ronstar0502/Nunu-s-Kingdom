using System.Collections;
using UnityEngine;

public class DayNightSpriteChange : MonoBehaviour
{
    [SerializeField] Color32 dayColor;
    [SerializeField] Color32 nightColor;
    private GameManger gameManger;
    private SpriteRenderer _sr;
    private float dayDuration, nightDuration;
    private float timeElapsed;

    private void Awake()
    {
        gameManger = FindObjectOfType<GameManger>();
        _sr = GetComponent<SpriteRenderer>();
        dayDuration = gameManger.GetDayDuration() -1f;
        nightDuration = gameManger.GetNightDuration() -1f;
        InitSpriteColor();
    }

    private void Start()
    {
        //InitSpriteColor();
        StartCoroutine(DayNightColorTransition());
    }

    private void InitSpriteColor()
    {
        print($"time elapsed since start of day time: {timeElapsed}");
        float targetColor = timeElapsed / dayDuration; //since i can only build at day state
        print($"target color time {targetColor}");
        _sr.color = Color.Lerp(dayColor, nightColor, targetColor);
    }
    public IEnumerator DayNightColorTransition()
    {
        yield return new WaitForSeconds(0.9f);
        float duration = (gameManger.gameState == GameState.Day) ? dayDuration : nightDuration;
        float transitionTime = duration - timeElapsed;  // calculates remaining time for transition if during day night cycle

        Color startColor = _sr.color;  // start transitioning from the current color
        Color targetColor = (gameManger.gameState == GameState.Day) ? nightColor : dayColor;

        float time = 0f;

        while (time < transitionTime)
        {
            time += Time.deltaTime;
            float timeLerpFactor = time / transitionTime;
            if (_sr != null)
            {
                _sr.color = Color.Lerp(startColor, targetColor, timeLerpFactor);
            }
            yield return null;
        }
        _sr.color = targetColor;
    }
}
