using UnityEngine;

public class TimeLineIndicator : MonoBehaviour
{
    [SerializeField]private Transform startPos,EndPos;
    Vector3 startPosition;
    Vector3 target;
    float timeToReachTarget;//time of day+night
    float t;

    void Start()
    {
        transform.position = startPosition = startPos.position;
        target= EndPos.position;
        SetDestination(target,10f);
    }

    void Update()
    {
        t += Time.deltaTime / timeToReachTarget;
        transform.position = Vector3.Lerp(startPosition, target, t);
        if(transform.position== target)//start of new day
        {
            transform.position = startPosition;
            SetDestination(target, 10f);
        }
    }

    public void SetDestination(Vector3 destination, float time)
    {
        this.t = 0;
        startPosition = transform.position;
        timeToReachTarget = time;
        target = destination;
    }
}
