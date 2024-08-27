using UnityEngine;

public class Villager : MonoBehaviour
{
    [SerializeField] protected VillagerData villagerData;
    [SerializeField] protected GameObject villagerTool; // unemployed doesnt have a tool
    [SerializeField] protected VillagerState villagerState;
    private GameObject buildingTarget;
    private Vector2 targetPosition;
    private bool isUnemployed = true;

    [Header("Temporary Patrol Points")] //place holder for testing will update later
    [SerializeField] protected float leftPatrolBorder;
    [SerializeField] protected float rightPatrolBorder;
    protected HQ HQ;

    protected void Awake()
    {
        villagerData.InitHealth();
    }

    private void Start()
    {
        villagerState = VillagerState.Spawned;
        Invoke(nameof(StartPatroling),1f);
        
        //place holder to test patrol system
        HQ = FindObjectOfType<HQ>();
        leftPatrolBorder = HQ.transform.position.x - 10f;
        rightPatrolBorder = HQ.transform.position.x + 10f;
    }

    private void Update()
    {
        //place holder will change later for better performance
        if(isUnemployed && villagerState == VillagerState.ProffesionAction)
        {
            VillagerMoveTo();
            CheckVillagerArrivalToTarget();
        }

        if(villagerState == VillagerState.Patrol)
        {
            VillagerMoveTo();
            CheckVillagerArrivalToTarget();
        }
    }

    private void VillagerMoveTo()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, villagerData.speed * Time.deltaTime);
    }
    private void CheckVillagerArrivalToTarget() //checks if the unemployed villager arrive to the proffesion building destination
    {
        if (transform.position == (Vector3)targetPosition)
        {
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
    }

    protected void StartPatroling()
    {
        ChangeState(VillagerState.Patrol);
    }
    protected void VillagerPatrol()  //patrol for villagers that are not assigned to other tasks
    {
        print("starts patroling");
        float randomPatrolPoint = Random.Range(leftPatrolBorder,rightPatrolBorder);
        targetPosition = new Vector2(randomPatrolPoint, transform.position.y);
    }
    private void ChangeState(VillagerState state) //changing villager state
    {
        villagerState = state;
        switch (state)
        {
            case VillagerState.Spawned:
                ChangeState(VillagerState.Patrol);
                Invoke(nameof(StartPatroling),2f);
                break;
            case VillagerState.ProffesionAction:             
                break;
            case VillagerState.Idle:
                break;
            case VillagerState.Patrol:
                VillagerPatrol();
                break;
            case VillagerState.Combat:
                break;

        }
    }
}


public enum VillagerState
{
    Spawned,
    ProffesionAction,
    Idle,
    Patrol,
    Combat
}
