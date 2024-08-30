using UnityEngine;

public class TimeLineIndicator : MonoBehaviour
{
    [SerializeField]private Transform startPos,EndPos;
    [SerializeField] private float DayCycleTime;
    Vector3 startPosition;
    Vector3 target;
    float timeToReachTarget;//time of day+night
    float t;


    void Start()
    {
        transform.position = startPosition = startPos.position;
        target= EndPos.position;
        SetDestination(target, DayCycleTime);
    }

    void Update()
    {
        startPosition = startPos.position;
        target=EndPos.position;
        t += Time.deltaTime / timeToReachTarget;
        transform.position = Vector3.Lerp(startPosition, target, t);
        if(transform.position== target)//start of new day
        {
            transform.position = startPosition;
            SetDestination(target, DayCycleTime);
        }
    }

    public void SetDestination(Vector3 destination, float time)
    {
        this.t = 0;
        timeToReachTarget = time;
        target = destination;
    }
}
