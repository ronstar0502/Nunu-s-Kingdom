using UnityEngine;

public class BlackSmith : ProffesionBuilding
{
    private bool playerInRange;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerInRange)
        {
            RecruitVillagerProffesion();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
