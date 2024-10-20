using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightLightCycle : MonoBehaviour
{
    [SerializeField]private Light2D _globalLight;
    // Day and night intensities
    [SerializeField] private float dayIntensity = 1f;
    [SerializeField] private float nightIntensity = 0.2f;
    private GameManger _gameManager;

    private float _dayDuration;
    private float _nightDuration;
    private float _totalCycleTime;
    private float _cycleTimer;

    private float _currentPhaseDuration;
    private float startIntensity , targetIntensity;


    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManger>();
        _dayDuration = _gameManager.GetDayDuration();
        _nightDuration = _gameManager.GetNightDuration();

        _totalCycleTime = _dayDuration + _nightDuration;
        _globalLight = GetComponent<Light2D>();
        SetPhaseInfo();
    }

    private void Start()
    {
        StartCoroutine(LightIntesityAdjustment());
    }

    private void SetPhaseInfo()
    {
        _currentPhaseDuration = (_gameManager.gameState == GameState.Day) ? _dayDuration : _nightDuration;
        startIntensity = _globalLight.intensity;
        targetIntensity = (_gameManager.gameState == GameState.Day) ? nightIntensity : dayIntensity;
    }

    private void Update()
    {
        
        /*_cycleTimer += Time.deltaTime;
        if (_cycleTimer >= _totalCycleTime)
        {
            _cycleTimer = 0f;
        }
        AdjustLightIntensity();*/
    }

    /*private void AdjustLightIntensity(float normalizTime , float startIntensity , float targetIntensity)
    {
        float intensity = Mathf.Lerp(startIntensity, targetIntensity, normalizTime);
        if(_globalLight)
        _globalLight.intensity = intensity;
    }*/

    private IEnumerator LightIntesityAdjustment()
    {
        SetPhaseInfo();
        float delayToStartLightAdjustment = _currentPhaseDuration * 0.67f;
        yield return new WaitForSeconds(_currentPhaseDuration*0.67f);

        float timeLeftForPhase = _currentPhaseDuration - delayToStartLightAdjustment;
        float time = 0f;
        while (time < timeLeftForPhase)
        {
            time += Time.deltaTime;
            float normalizedTime = time / timeLeftForPhase;
            if (_globalLight != null)
            {
                _globalLight.intensity = Mathf.Lerp(startIntensity, targetIntensity, normalizedTime);              
            }
            yield return null;
        }
        StartCoroutine(LightIntesityAdjustment());
    }
}
