using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    private IInteractable interactableObj;
    private void Awake()
    {
        playerData.SetInitHealth();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && interactableObj!=null)
        {
            interactableObj.Interact();
        }
    }
    public PlayerData GetPlayerData() { return playerData; }
    /*private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("pressed space");
            if(collision.gameObject.CompareTag("BuildingSpot"))
            {
                collision.gameObject.GetComponent<BuildingSpot>().Interact();
            }
            if(collision.gameObject.CompareTag("Building")) // upgrade building
            {
                collision.gameObject.GetComponent<Building>().Interact();
            }
        }
    }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent<IInteractable>(out IInteractable interactable))
        {
            interactableObj = interactable;
            print($"object tag {collision.gameObject.tag}");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        interactableObj=null;
        print($"object tag {collision.gameObject.tag}");
    }
}
