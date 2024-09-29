using UnityEngine;
public class Player : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    private VillageInfo _villageInfo;
    private IInteractable _interactableObj;
    private bool _canBuild; 

    private void Awake()
    {
        playerData.InitPlayer();
    }

    private void Start()
    {
        _villageInfo =  FindObjectOfType<VillageInfo>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _interactableObj!=null)
        {
            if (_canBuild)
            {
                _interactableObj.Interact();
            }
            else
            {
                print("cant build at night");
            }
        }
    }

    public PlayerData GetPlayerData() { return playerData; }

    public void SetCanBuild(GameState state)
    {
        if(state == GameState.Day)
        {
            _canBuild = true;
        }else if(state == GameState.Night)
        {
            _canBuild = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent<IInteractable>(out IInteractable interactable)) //looks for building spot to interact with ot other interactable objects
        {
            if (collision.gameObject.CompareTag("BuildingSpot"))
            {
                collision.gameObject.GetComponent<BuildingSpot>().ShowBuildingGhost();
            }
            _interactableObj = interactable;
        }

        if (collision.gameObject.CompareTag("Building"))
        {
            collision.gameObject.GetComponent<Building>().EnableBuildingPopUp();
        }

        if(collision.gameObject.CompareTag("Seed"))
        {
            Seed seed = collision.gameObject.GetComponent<Seed>();
            if (seed.canLoot)
            {
                seed.PlayPickupSFX();
                int amount = seed.value;
                playerData.AddSeedsAmount(amount);
                _villageInfo.SetSeedsText();
                print($"seeds in invetory: {playerData.seedAmount}");
                Destroy(collision.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BuildingSpot"))
        {
            collision.gameObject.GetComponent<BuildingSpot>().HideBuildingGhost();
        }

        if (collision.gameObject.CompareTag("Building"))
        {
            collision.gameObject.GetComponent<Building>().DisableBuildingPopUp();
        }
        _interactableObj =null;
    }
  
}
