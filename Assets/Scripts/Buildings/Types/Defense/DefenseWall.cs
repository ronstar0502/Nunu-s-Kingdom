using UnityEngine;

public class DefenseWall : Building
{
    private void Start()
    {
        HQ _HQ =  FindObjectOfType<HQ>();
        if(transform.position.x > _HQ.transform.position.x)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        buildingSpot.SetActive(true);
    }
}
