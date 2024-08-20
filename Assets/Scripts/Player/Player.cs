using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    private void Awake()
    {
        playerData.SetInitHealth();
    }

    public PlayerData GetPlayerData() { return playerData; }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("pressed space");
            print($"object tag {collision.gameObject.tag}");
            if(collision.gameObject.CompareTag("BuildingSpot"))
            {
                collision.gameObject.GetComponent<BuildingSpot>().Interact();
            }
            if(collision.gameObject.CompareTag("Building")) // upgrade building
            {
                collision.gameObject.GetComponent<Building>().Interact();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
