using UnityEngine;

public class Seed : MonoBehaviour 
{
    [SerializeField] private float force = 4f;
    public int value; //amount
    public bool canLoot;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void InitSeed(int value)
    {
        this.value = value;

        Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(0.5f, 1f)).normalized;
        rb.AddForce(randomDirection * force, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            print("seed landed on platform");
            rb.gravityScale *= 0;
            rb.velocity = Vector2.zero;
            canLoot = true;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            print("player looted the seed");
        }
    }
}
