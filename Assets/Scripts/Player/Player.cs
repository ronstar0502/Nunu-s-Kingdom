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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent<IInteractable>(out IInteractable interactable))
        {
            interactableObj = interactable;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        interactableObj=null;
    }

    
}
