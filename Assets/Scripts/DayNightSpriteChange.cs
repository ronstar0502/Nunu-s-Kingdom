using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightSpriteChange : MonoBehaviour
{
    [SerializeField] Color32 dayColor;
    [SerializeField] Color32 nightColor;
    [SerializeField] private float delayBetweenTransitions;
    private GameManger gameManger;
    private SpriteRenderer _sr;
    private float dayDuration, nightDuration;
    private float timeElapsed;

    private void Awake()
    {
        gameManger = FindObjectOfType<GameManger>();
        _sr = GetComponent<SpriteRenderer>();
        dayDuration = gameManger.GetDayDuration();
        nightDuration = gameManger.GetNightDuration();
    }

    private void Start()
    {
        InitSpriteColor();
        StartCoroutine(DayNightColorTransition());
    }

    private void InitSpriteColor()
    {
        timeElapsed = Time.time - gameManger.GetLastStateSwapedTime();
        float targetColor = timeElapsed / dayDuration; //since i can only build at day state
        _sr.color = Color.Lerp(dayColor, nightColor, targetColor);
    }
    private IEnumerator DayNightColorTransition()
    {
        if(gameObject != null)
        {
            float duration = (gameManger.gameState == GameState.Day) ? dayDuration : nightDuration;
            float transitionTime = duration - timeElapsed;  // calculates remaining time for transition if during day night cycle

            Color startColor = _sr.color;  // start transitioning from the current color
            Color targetColor = (gameManger.gameState == GameState.Day) ? nightColor : dayColor;

            float time = 0f;

            while (time < transitionTime && gameObject != null)
            {
                time += Time.deltaTime;
                float timeLerpFactor = time / transitionTime;
                _sr.color = Color.Lerp(startColor, targetColor, timeLerpFactor);
                yield return null;
            }
            _sr.color = targetColor;
        }
        StartCoroutine(DayNightColorTransition()); // starts the cycle again
    }
}
