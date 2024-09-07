using UnityEngine;
public class Player : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    private IInteractable interactableObj;
    private bool canBuild; 
    private void Awake()
    {
        playerData.SetInitHealth();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && interactableObj!=null)
        {
            if (canBuild)
            {
                interactableObj.Interact();
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
            canBuild = true;
        }else if(state == GameState.Night)
        {
            canBuild = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) //test to see if enemy drops loot
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(1000);
        }

        if(collision.gameObject.TryGetComponent<IInteractable>(out IInteractable interactable)) //looks for building spot to interact with ot other interactable objects
        {
            if (collision.gameObject.CompareTag("BuildingSpot"))
            {
                collision.gameObject.GetComponent<BuildingSpot>().ShowBuildingGhost();
            }
            interactableObj = interactable;
        }

        if (collision.gameObject.CompareTag("Building"))
        {
            collision.gameObject.GetComponent<Building>().EnableBuildingPopUp();
        }

        if(collision.gameObject.CompareTag("Seed"))
        {
            int amount = collision.gameObject.GetComponent<Seed>().amount;
            playerData.AddSeedsAmount(amount);
            print($"seeds in invetory: {playerData.seedAmount}");
            Destroy(collision.gameObject);
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
        interactableObj =null;
    }

    
}
