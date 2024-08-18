using UnityEngine;

public class BlackSmith : WeaponryBuilding
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CraftWeapon();
        }
    }
}
