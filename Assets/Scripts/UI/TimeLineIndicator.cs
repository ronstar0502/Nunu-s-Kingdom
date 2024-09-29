using UnityEngine;

public class TimeLineIndicator : MonoBehaviour
{
    [SerializeField]private Transform startPos,EndPos;
    private float _DayCycleTime;
    private Vector3 _startPosition;
    private Vector3 _target;
    private float _timeToReachTarget;//time of day+night
    private float _time;

    void Start()
    { 
        transform.position = _startPosition = startPos.position;
        _target= EndPos.position;
        SetDestination(_target, _DayCycleTime);
    }

    void Update()
    {
        _startPosition = startPos.position;
        _target=EndPos.position;
        _time += Time.deltaTime / _timeToReachTarget;
        transform.position = Vector3.Lerp(_startPosition, _target, _time);
        if(transform.position== _target)//start of new day
        {
            transform.position = _startPosition;
            SetDestination(_target, _DayCycleTime);
        }
    }

    public void SetDayCycleTimer(float time)
    {
        _DayCycleTime = time;
    }

    public void SetDestination(Vector3 destination, float time)
    {
        this._time = 0;
        _timeToReachTarget = time;
        _target = destination;
    }
}
