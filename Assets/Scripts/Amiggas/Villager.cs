using System.Collections;
using UnityEngine;
public class Villager : MonoBehaviour
{
    [SerializeField] protected VillagerData amiggaData;
    [SerializeField] protected GameObject amiggaTool; // unemployed doesnt have a tool
    [SerializeField] protected AmiggaState amiggaState;
    [SerializeField] protected AudioClip spawnSound;
    [SerializeField] protected SoundEffectManger soundEffectManger;
    protected SpriteRenderer sr;
    protected Rigidbody2D rb;
    protected Animator animator;
    protected Vector2 targetPosition;
    protected int direction;
    //private GameObject _buildingTarget;

    [Header("Temporary Patrol Points")] //place holder for testing will update later
    [SerializeField] protected float leftPatrolBorder;
    [SerializeField] protected float rightPatrolBorder;
    protected HQ HQ;

    public bool isProffesionRecruited;
    protected virtual void Awake()
    {
        amiggaData.InitHealth();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        soundEffectManger = FindAnyObjectByType<SoundEffectManger>();
        InitAmmiga();
    }

    protected virtual void Start()
    {
        SetState(AmiggaState.Spawned);
        soundEffectManger.PlaySFX(spawnSound);

    }

    protected void InitAmmiga()
    {
        HQ = FindObjectOfType<HQ>();

        //place holder to test patrol system
        leftPatrolBorder = HQ.transform.position.x - 10f;
        rightPatrolBorder = HQ.transform.position.x + 10f;
    }

    protected virtual void Update()
    {
        if(amiggaState == AmiggaState.Patrol)
        {
            AmiggaPatrol();           
        }
    }

    protected void AmiggaMoveToTarget(Vector2 targetPos) //method to move the villager to target position on X axsis only
    {
        SetAmmigaDirection(targetPos.x);
        //float movementDirection = targetPosition.x - transform.position.x;
        //rb.velocity = new Vector2(movementDirection* direction * villagerData.movementSpeed * Time.deltaTime,rb.velocity.y);

        transform.position = Vector2.MoveTowards(transform.position, new Vector2(targetPos.x,transform.position.y), amiggaData.movementSpeed * Time.deltaTime);
    }
    private void AmiggaArrivalToProffesionBuilding(GameObject targetProffesionBuilding)  //method for the arrival of the unepmloyed to the proffesion building
    {
        ProffesionBuilding proffesionBuilding = targetProffesionBuilding.GetComponent<ProffesionBuilding>();
        proffesionBuilding.VillagerProffesionChange_OnArrival(gameObject);
    }

    public VillagerData GetAmiggaData() { return amiggaData;}

    public IEnumerator ChangeAmiggaProffesion(GameObject proffesionBuilding,Vector2 recruitPosition) // sets unemployed target proffesion building
    {
        SetState(AmiggaState.ProffesionAction);
        //_buildingTarget = proffesionBuilding;
        targetPosition = new Vector2(recruitPosition.x, transform.position.y);

        while ((Vector2)transform.position != targetPosition)
        {
            AmiggaMoveToTarget(targetPosition);
            yield return null;
        }
        AmiggaArrivalToProffesionBuilding(proffesionBuilding);

        //StartCoroutine(GoToProffesionBuilding());
    }

    private IEnumerator GoToProffesionBuilding() //sets the proffesion building for the unemployed to go to
    {
        while((Vector2)transform.position != targetPosition)
        {
            AmiggaMoveToTarget(targetPosition);
            yield return null;
        }
        //AmiggaArrivalToProffesionBuilding();

    }
    protected void AmiggaPatrol()  //patrol for villagers that are not assigned to other tasks
    {
        if (!isProffesionRecruited)
        {
            AmiggaMoveToTarget(targetPosition);
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
            float randomPatrolPoint = Random.Range(leftPatrolBorder, rightPatrolBorder);
            targetPosition = new Vector2(randomPatrolPoint, transform.position.y);
        }
    }

    protected void SetAmmigaDirection(float targetPoint) //sets the direction of the sprite and movement
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
                SetState(AmiggaState.Patrol);
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

