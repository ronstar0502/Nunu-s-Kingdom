using System.Collections;
using UnityEngine;

public class DayNightSpriteChange : MonoBehaviour
{
    [SerializeField] Color32 dayColor;
    [SerializeField] Color32 nightColor;
    private GameManger _gameManger;
    private SpriteRenderer _sr;
    private float _dayDuration, _nightDuration;
    private float _timeElapsed;

    private void Awake()
    {
        _gameManger = FindObjectOfType<GameManger>();
        _sr = GetComponent<SpriteRenderer>();
        _dayDuration = _gameManger.GetDayDuration() -1f;
        _nightDuration = _gameManger.GetNightDuration() -1f;
        InitSpriteColor();
    }

    private void Start()
    {
        //InitSpriteColor();
        StartCoroutine(DayNightColorTransition());
    }

    private void InitSpriteColor()
    {
        //print($"time elapsed since start of day time: {timeElapsed}");
        float targetColor = _timeElapsed / _dayDuration; //since i can only build at day state
        //print($"target color time {targetColor}");
        _sr.color = Color.Lerp(dayColor, nightColor, targetColor);
    }

    public IEnumerator DayNightColorTransition()
    {
        yield return new WaitForSeconds(0.9f);
        float duration = (_gameManger.gameState == GameState.Day) ? _dayDuration : _nightDuration;
        float transitionTime = duration - _timeElapsed;  // calculates remaining time for transition if during day night cycle

        Color startColor = _sr.color;  // start transitioning from the current color
        Color targetColor = (_gameManger.gameState == GameState.Day) ? nightColor : dayColor;

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
