using System.Collections;
using UnityEngine;
public class Villager : MonoBehaviour
{
    [SerializeField] protected VillagerData villagerData;
    [SerializeField] protected GameObject villagerTool; // unemployed doesnt have a tool
    [SerializeField] protected VillagerState villagerState;
    [SerializeField] protected AudioClip spawnSound;
    [SerializeField] protected SoundEffectManger soundEffectManger;
    protected SpriteRenderer sr;
    protected Rigidbody2D rb;
    protected Animator animator;
    protected HQ HQ;
    protected Vector2 targetPosition;
    protected int direction;

    public bool isProffesionRecruited;

    protected virtual void Awake()
    {
        villagerData.InitHealth();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        soundEffectManger = FindAnyObjectByType<SoundEffectManger>();
        HQ = FindObjectOfType<HQ>();
    }

    protected virtual void Start()
    {
        SetState(VillagerState.Spawned);
        PlaySFX();
    }

    protected virtual void Update()
    {
        if(villagerState == VillagerState.Patrol)
        {
            VillagerPatrol();           
        }
    }

    protected void VillagerMoveToTarget(Vector2 targetPos) //method to move the villager to target position on X axsis only
    {
        SetVillagerDirection(targetPos.x);
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(targetPos.x,transform.position.y), villagerData.movementSpeed * Time.deltaTime);
    }

    private void VillagerArrivalToProffesionBuilding(GameObject targetProffesionBuilding)  //method for the arrival of the unepmloyed to the proffesion building
    {
        ProffesionBuilding proffesionBuilding = targetProffesionBuilding.GetComponent<ProffesionBuilding>();
        proffesionBuilding.VillagerProffesionChange_OnArrival(gameObject);
    }

    public VillagerData GetVillagerData() { return villagerData;}

    public IEnumerator ChangeVillagerProffesion(GameObject proffesionBuilding,Vector2 recruitPosition) // sets unemployed target proffesion building
    {
        SetState(VillagerState.ProffesionAction);
        targetPosition = new Vector2(recruitPosition.x, transform.position.y);

        while ((Vector2)transform.position != targetPosition)
        {
            VillagerMoveToTarget(targetPosition);
            yield return null;
        }
        VillagerArrivalToProffesionBuilding(proffesionBuilding);
    }

    private IEnumerator GoToProffesionBuilding() //sets the proffesion building for the unemployed to go to
    {
        while((Vector2)transform.position != targetPosition)
        {
            VillagerMoveToTarget(targetPosition);
            yield return null;
        }
    }

    protected void VillagerPatrol()  //patrol for villagers that are not assigned to other tasks
    {
        if (!isProffesionRecruited)
        {
            VillagerMoveToTarget(targetPosition);
            if(transform.position == (Vector3)targetPosition)
            {
                SetPatrolPosition();
            }
        }

    }

    private void SetPatrolPosition()
    {
        if (!isProffesionRecruited)
        {
            float randomPatrolPoint = Random.Range(HQ.leftPatrolBorderTransform.position.x, HQ.rightPatrolBorderTransform.position.x);
            targetPosition = new Vector2(randomPatrolPoint, transform.position.y);
        }
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

    protected void SetState(VillagerState state) //changing villager state
    {
        villagerState = state;
        switch (state)
        {
            case VillagerState.Spawned:
                SetState(VillagerState.Patrol);
                print("villager is now patrolling");
                break;
            case VillagerState.ProffesionAction:                
                break;
            case VillagerState.InProffesionBuilding:
                break;
            case VillagerState.Patrol:
                SetPatrolPosition();
                VillagerPatrol();
                break;
            case VillagerState.Combat:
                SetPatrolPosition();
                break;
        }
    }
    protected void PlaySFX()
    {
        soundEffectManger.PlaySFX(spawnSound);
    }
}

