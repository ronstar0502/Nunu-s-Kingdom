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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("BuildingSpot") && Input.GetKeyDown(KeyCode.Space))
        {
            collision.gameObject.GetComponent<BuildingSpot>().Interact();
        }
        if(collision.gameObject.CompareTag("Building") && Input.GetKeyDown(KeyCode.Space)) // upgrade building
        {
            collision.gameObject.GetComponent<Building>().Interact();
        }
        
    }
}
