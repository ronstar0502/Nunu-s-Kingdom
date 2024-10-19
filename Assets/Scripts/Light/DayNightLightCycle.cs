using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightLightCycle : MonoBehaviour
{
    [SerializeField]private Light2D _globalLight;
    private GameManger _gameManager;

    private float _dayDuration;
    private float _nightDuration;
    private float _totalCycleTime;
    private float _cycleTimer;

    // Day and night intensities
    [SerializeField] private float dayIntensity = 1f;
    [SerializeField] private float nightIntensity = 0.2f;

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManger>();
        _dayDuration = _gameManager.GetDayDuration();
        _nightDuration = _gameManager.GetNightDuration();

        _totalCycleTime = _dayDuration + _nightDuration;
        _globalLight = GetComponent<Light2D>();
    }

    private void Update()
    {
        _cycleTimer += Time.deltaTime;
        if (_cycleTimer >= _totalCycleTime)
        {
            _cycleTimer = 0f;
        }
        AdjustLightIntensity();
    }

    private void AdjustLightIntensity()
    {
        float normalizedTime = _cycleTimer / _totalCycleTime;
        float intensity = Mathf.Lerp(dayIntensity, nightIntensity, Mathf.Sin(normalizedTime * Mathf.PI));
        _globalLight.intensity = intensity;
    }
}
