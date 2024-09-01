using UnityEngine;

public enum VillagerState
{
    Spawned,
    ProffesionAction,
    InProffesionBuilding,
    Patrol,
    Combat
}
public class Villager : MonoBehaviour
{
    [SerializeField] protected VillagerData villagerData;
    [SerializeField] protected GameObject villagerTool; // unemployed doesnt have a tool
    [SerializeField] protected VillagerState villagerState;
    protected SpriteRenderer sr;
    protected Rigidbody2D rb;
    protected int direction;
    private GameObject buildingTarget;
    private Vector2 targetPosition;
    private bool isUnemployed = true;

    [Header("Temporary Patrol Points")] //place holder for testing will update later
    [SerializeField] protected float leftPatrolBorder;
    [SerializeField] protected float rightPatrolBorder;
    protected HQ HQ;

    private void Awake()
    {
        villagerData.InitHealth();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {
        InitVillager();
        ChangeState(VillagerState.Spawned);
    }

    protected void InitVillager()
    {
        HQ = FindObjectOfType<HQ>();
        //place holder to test patrol system
        leftPatrolBorder = HQ.transform.position.x - 10f;
        rightPatrolBorder = HQ.transform.position.x + 10f;
    }

    protected void Update()
    {
        //place holder will change later for better performance
        if(isUnemployed && villagerState == VillagerState.ProffesionAction)
        {
            VillagerMoveTo(targetPosition);
            CheckVillagerArrivalToTarget();
        }

        if(villagerState == VillagerState.Patrol)
        {
            VillagerMoveTo(targetPosition);
            CheckVillagerArrivalToTarget();
        }
    }

    protected void VillagerMoveTo(Vector2 targetPos) //method to move the villager to target position on X axsis only
    {
        SetVillagerDirection(targetPos.x);
        //float movementDirection = targetPosition.x - transform.position.x;
        //rb.velocity = new Vector2(movementDirection* direction * villagerData.movementSpeed * Time.deltaTime,rb.velocity.y);

        transform.position = Vector2.MoveTowards(transform.position, new Vector2(targetPos.x,transform.position.y), villagerData.movementSpeed * Time.deltaTime);
    }
    private void CheckVillagerArrivalToTarget() //checks if the unemployed villager arrive to the proffesion building destination
    {
        if (transform.position == (Vector3)targetPosition)
        {
            rb.velocity = Vector2.zero;
            if(villagerState == VillagerState.ProffesionAction)
            {
                VilagerArrivalAction();
                isUnemployed = false;
                ChangeState(VillagerState.Spawned);
            }
            else if(villagerState == VillagerState.Patrol)
            {
                VillagerPatrol();
            }
        }
    }

    private void VilagerArrivalAction()  //method for the arrival of the unepmloyed to the proffesion building
    {
        ProffesionBuilding proffesionBuilding = buildingTarget.GetComponent<ProffesionBuilding>();
        proffesionBuilding.VillagerProffesionChange_OnArrival(gameObject);
    }

    public VillagerData GetVillagerData() { return villagerData;}

    public void ChangeProffesion(GameObject proffesionBuilding,Vector2 recruitPosition) // sets unemployed target proffesion building
    {
        ChangeState(VillagerState.ProffesionAction);
        GoToProffesionBuilding(proffesionBuilding, recruitPosition);
    }

    private void GoToProffesionBuilding(GameObject proffesionBuilding, Vector2 recruitPosition) //sets the proffesion building for the unemployed to go to
    {
        targetPosition = new Vector2(recruitPosition.x, transform.position.y);
        buildingTarget = proffesionBuilding;
        VillagerMoveTo(targetPosition);
    }
    protected void StartPatroling()
    {
        ChangeState(VillagerState.Patrol);
    }
    protected void VillagerPatrol()  //patrol for villagers that are not assigned to other tasks
    {
        print("starts patroling");
        float randomPatrolPoint = Random.Range(leftPatrolBorder, rightPatrolBorder);
        targetPosition = new Vector2(randomPatrolPoint, transform.position.y);
        VillagerMoveTo(targetPosition);
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

    protected void ChangeState(VillagerState state) //changing villager state
    {
        villagerState = state;
        switch (state)
        {
            case VillagerState.Spawned:
                Invoke(nameof(StartPatroling),2f);
                break;
            case VillagerState.ProffesionAction:                
                break;
            case VillagerState.InProffesionBuilding:
                break;
            case VillagerState.Patrol:
                VillagerPatrol();
                break;
            case VillagerState.Combat:
                break;

        }
    }
}

