using UnityEngine;

public enum AmiggaState
{
    Spawned,
    ProffesionAction,
    InProffesionBuilding,
    Patrol,
    Combat
}
public class Villager : MonoBehaviour
{
    [SerializeField] protected VillagerData amiggaData;
    [SerializeField] protected GameObject amiggaTool; // unemployed doesnt have a tool
    [SerializeField] protected AmiggaState amiggaState;
    protected SpriteRenderer sr;
    protected Rigidbody2D rb;
    protected Animator animator;
    protected Vector2 targetPosition;
    protected int direction;
    private GameObject _buildingTarget;
    private bool _isUnemployed = true;

    [Header("Temporary Patrol Points")] //place holder for testing will update later
    [SerializeField] protected float leftPatrolBorder;
    [SerializeField] protected float rightPatrolBorder;
    protected HQ HQ;

    protected void Awake()
    {
        amiggaData.InitHealth();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        InitAmmiga();
    }

    protected virtual void Start()
    {
        if (!HQ.isNightMode)
        {
            SetState(AmiggaState.Spawned);           
        }
    }

    protected void InitAmmiga()
    {
        HQ = FindObjectOfType<HQ>();

        //place holder to test patrol system
        leftPatrolBorder = HQ.transform.position.x - 10f;
        rightPatrolBorder = HQ.transform.position.x + 10f;
    }

    protected void Update()
    {
        //place holder will change later for better performance
        if(_isUnemployed && amiggaState == AmiggaState.ProffesionAction)
        {
            AmiggaMoveToTarget(targetPosition);
            CheckAmiggaArrivalToTarget();
        }

        if(amiggaState == AmiggaState.Patrol)
        {
            AmiggaPatrol();           
        }
    }

    protected void AmiggaMoveToTarget(Vector2 targetPos) //method to move the villager to target position on X axsis only
    {
        SetVillagerDirection(targetPos.x);
        //float movementDirection = targetPosition.x - transform.position.x;
        //rb.velocity = new Vector2(movementDirection* direction * villagerData.movementSpeed * Time.deltaTime,rb.velocity.y);

        transform.position = Vector2.MoveTowards(transform.position, new Vector2(targetPos.x,transform.position.y), amiggaData.movementSpeed * Time.deltaTime);
    }
    protected void CheckAmiggaArrivalToTarget() //checks if the unemployed villager arrive to the proffesion building destination
    {
        if (transform.position == (Vector3)targetPosition)
        {
            rb.velocity = Vector2.zero;
            if(amiggaState == AmiggaState.ProffesionAction)
            {
                AmiggaArrivalToProffesionBuilding();
                //isUnemployed = false;
                //ChangeState(VillagerState.Spawned);
            }
            else if(amiggaState == AmiggaState.Patrol || amiggaState == AmiggaState.Combat)
            {
                AmiggaPatrol();
            }
        }
    }

    private void AmiggaArrivalToProffesionBuilding()  //method for the arrival of the unepmloyed to the proffesion building
    {
        ProffesionBuilding proffesionBuilding = _buildingTarget.GetComponent<ProffesionBuilding>();
        proffesionBuilding.VillagerProffesionChange_OnArrival(gameObject);
    }

    public VillagerData GetAmiggaData() { return amiggaData;}

    public void ChangeAmiggaProffesion(GameObject proffesionBuilding,Vector2 recruitPosition) // sets unemployed target proffesion building
    {
        SetState(AmiggaState.ProffesionAction);
        GoToProffesionBuilding(proffesionBuilding, recruitPosition);
    }

    private void GoToProffesionBuilding(GameObject proffesionBuilding, Vector2 recruitPosition) //sets the proffesion building for the unemployed to go to
    {
        targetPosition = new Vector2(recruitPosition.x, transform.position.y);
        _buildingTarget = proffesionBuilding;
        AmiggaMoveToTarget(targetPosition);
    }
    protected void StartPatroling()
    {
        SetState(AmiggaState.Patrol);
    }
    protected void AmiggaPatrol()  //patrol for villagers that are not assigned to other tasks
    {
        AmiggaMoveToTarget(targetPosition);
        if(transform.position == (Vector3)targetPosition)
        {
            SetPatrolPosition();
        }

    }

    private void SetPatrolPosition()
    {
        float randomPatrolPoint = Random.Range(leftPatrolBorder, rightPatrolBorder);
        targetPosition = new Vector2(randomPatrolPoint, transform.position.y);
    }

    protected void SetVillagerDirection(float targetPoint) //sets the direction of the sprite and movement
    {
        if (targetPoint < transform.position.x)
        {
            direction = -1;
            sr.flipX = true;
        }
        else if (targetPoint > transform.position.x)
        {
            direction = 1;
            sr.flipX = false;
        }
    }

    protected void SetState(AmiggaState state) //changing villager state
    {
        amiggaState = state;
        switch (state)
        {
            case AmiggaState.Spawned:               
                Invoke(nameof(StartPatroling),2f);
                break;
            case AmiggaState.ProffesionAction:                
                break;
            case AmiggaState.InProffesionBuilding:
                break;
            case AmiggaState.Patrol:
                SetPatrolPosition();
                AmiggaPatrol();
                break;
            case AmiggaState.Combat:
                SetPatrolPosition();
                break;
        }
    }
}

